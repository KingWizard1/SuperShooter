using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuperShooter
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        [Header("State Debug")]
        [SerializeField]
        public Transform target;
        [SerializeField]
        public Vector3 targetPosition => target?.position ?? Vector3.zero;
        [SerializeField]
        public Vector3 destination => _agent.destination;
        [SerializeField]
        public float distanceToTarget;
        [SerializeField]
        public float distanceToDestination => _agent.remainingDistance;
        //public float distanceToDestination;
        [SerializeField]
        private Vector3 lastKnownTargetPosition;
        [SerializeField]
        private EnemyControllerState state = EnemyControllerState.Search;

        public EnemyControllerState State => state;

        [SerializeField]
        private bool isGrounded = false;

        public bool hasTarget;

        // ------------------------------------------------- //

        [Header("Settings")]
        //public bool allowMovement = true;
        //public bool allowRotation = true;
        private float arrivalDistance = .1f;

        [Range(1, 5)]
        public float searchTime = 1;
        private float searchTimer;



        [Header("Sight")]
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;
        public LayerMask targetLayer;
        public LayerMask obstacleMask;

        [Header("Physics")]
        public float gravity = 10f;
        public float groundRayDistance = 1.1f;

        //bool hasChecked;

        // ------------------------------------------------- //

        // Scripting properties
        public bool isAgentActive => _agent.enabled;
        public bool isWithinStoppingDistance { get; private set; }

        // Expose the agent's stopping distance.
        public float stoppingDistance
        {
            get => _agent.stoppingDistance;
            set => _agent.stoppingDistance = value;
        }

        // ------------------------------------------------- //

        private NavMeshAgent _agent;

        // ------------------------------------------------- //

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = false;  // Disable by default.
            _agent.updateRotation = false;
        }

        // ------------------------------------------------- //

        private void Start()
        {

            // Safeties
            if (arrivalDistance <= 0)
                arrivalDistance = .5f;    // Set to minimum value.
          

        }

        // ------------------------------------------------- //

        void Update()
        {
            
            // Are we on the ground?
            // If not, simulate gravity and fall until we do hit ground.
            isGrounded = transform.CheckIfGrounded(out RaycastHit hit, groundRayDistance);
            if (!isGrounded) {
                var y = transform.position.y - (gravity * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                return;
            }

            // Enable or disable the agent depending on whether we are grounded.
            // NavMeshAgent doesn't like it when we call SetDestination() when not on a NavMesh.
            _agent.enabled = isGrounded;

            // Bail if theres nothing to do at this point.
            if (!_agent.enabled /*|| _agent.isStopped*/)
                return;

            // Look for a target within my sights (my field of view).
            target = FindTargetWithinFOV();
            if (!hasTarget && target != null) {
                hasTarget = true;
                Debug.Log($"Target acquired: {target.name}");
            }
            else if (hasTarget && target == null) {
                Debug.Log($"Target lost!");
                hasTarget = false;
            }


            // Stop here if the controller should be stopped.
            if (state == EnemyControllerState.Stop) {

                //// Look at our target if we have one.
                //if (hasTarget)
                //    transform.LookAt(target.position);

                // Do nothing else. The controller is 'stopped'.
                return;

            }

            // Rotate to look in the direction of movement.
            transform.rotation = Quaternion.LookRotation(_agent.velocity);


            // If we have a target, lets configure the agent to move toward them.
            if (hasTarget)
            {
                // Calculations
                distanceToTarget = (target.position - transform.position).magnitude;
                isWithinStoppingDistance = distanceToTarget < _agent.stoppingDistance;

                // Remember this position in case we lose sight of the target.
                lastKnownTargetPosition = target.position;


                // Are we within stopping distance of our target? 
                if (isWithinStoppingDistance)
                {
                    Debug.Log("Reached stopping distance of target.");
                    Stop();
                }
                else
                {
                    // Tell the agent to go to this position if we're not stopped.
                    if (state != EnemyControllerState.Stop)
                        GoTo(target.position);
                }
            }
            else
            {
                // We have no target in sight.

                // If we have a last known position, try and find the target there.
                // If we dont have a last known position, try and find a target in visible sight.
                var distanceToLastKnown = (transform.position - lastKnownTargetPosition).magnitude;
                if (lastKnownTargetPosition != Vector3.zero && distanceToLastKnown <= arrivalDistance)
                {
                    SearchLastKnown(lastKnownTargetPosition);
                }
                else
                {
                    Search();
                }
            }

        }

        // ------------------------------------------------- //

        #region Sight

        Transform FindTargetWithinFOV()
        {
            
            Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);
            for (int i = 0; i < targetsInRadius.Length; i++)
            {
                var target = targetsInRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        return target;
                    }
                }
            }

            // No targets found.
            return null;
        }

        public Vector3 DirFromAngle(float angle, bool isGlobalAngle)
        {
            if (!isGlobalAngle)
            {
                angle += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        }

        #endregion

        // ------------------------------------------------- //

        /// <summary>Call this to set the controller's current destination.</summary>
        public void SetDestination(Vector3 worldPosition)
        {
            //destination = worldPosition;
            _agent.SetDestination(worldPosition);
            _agent.isStopped = false;
        }

        // ------------------------------------------------- //

        public void Search()
        {

            // Add time
            searchTimer += Time.deltaTime;

            // Bail if we haven't reached the end of our time to search for a player at this position.
            // Otherwise, reset the timer and move to a new destionation to look for a target from.
            if (searchTimer < searchTime)
                return;

            // Reset timer.
            searchTimer = 0;

            // Set new destination.
            var r = 10;
            var randomOffset = new Vector3(Random.Range(-r, r), transform.position.y, Random.Range(-r, r));
            var destinationPos = transform.position + randomOffset;

            SetDestination(destinationPos);

            // Set state
            if (state != EnemyControllerState.Search) {
                state = EnemyControllerState.Search;
                Debug.Log("Searching for new target at new destintation.");
            }

        }

        public void GoTo(Vector3 worldPosition)
        {
            // Set state
            state = EnemyControllerState.Goto;
            SetDestination(worldPosition);
        }

        public void GoToTarget()
        {
            if (target != null)
                GoTo(target.position);
        }

        void SearchLastKnown(Vector3 worldPosition)
        {
            // Set state
            state = EnemyControllerState.SearchLastKnown;

            if (distanceToDestination > arrivalDistance)
            {
                Debug.Log("Moving to lost target's last known position...");
                SetDestination(worldPosition);
            }
            else
            {
                // Arrived at destination. Switch to Search state.
                Debug.Log("Arrived at lost target's last known position.");
                lastKnownTargetPosition = Vector3.zero;
                Search();
            }


        }

        // ------------------------------------------------- //

        public void Stop()
        {
            state = EnemyControllerState.Stop;
            _agent.isStopped = true;
        }

    }

    public enum EnemyControllerState
    {
        /// <summary>The character is idle.</summary>
        Stop,
        /// <summary>The character is trying to look for a new target.</summary>
        Search,
        /// <summary>The character is attempting to find its current target at its last known location.</summary>
        SearchLastKnown,
        /// <summary>The character is making its way to its target.</summary>
        Goto,
        /// <summary>The character is fleeing from its target.</summary>
        Flee,
    }

}