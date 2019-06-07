using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuperShooter
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Movement")]
        public EnemyControllerState movementState = EnemyControllerState.Search;
        [SerializeField]
        private Vector3 goToPos;
        [SerializeField]
        private float disToTarget;
        /// <summary>
        /// The distance away from the 'goToPos' that results in the 'Search' function.
        /// </summary>
        public float searchDisToTarget;  

        [Header("Sight")]
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;
        public LayerMask targetLayer;
        public LayerMask obstacleMask;
        public Transform target;

        public float gravity = 10f;
        public float groundRayDistance = 1.1f;

        // ------------------------------------------------- //

        // Scripting properties
        public bool isAgentActive => _agent.enabled;
        public bool isWithinStoppingDistance { get; private set; }

        // ------------------------------------------------- //

        private NavMeshAgent _agent;

        // ------------------------------------------------- //

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = false;  // Disable by default.
        }

        // ------------------------------------------------- //

        void Update()
        {
            FindVisibleTarget();

            disToTarget = (goToPos - transform.position).magnitude;

            // Are we on the ground?
            // If not, simulate gravity and fall until we do hit ground.
            bool isGrounded = transform.CheckIfGrounded(out RaycastHit hit, groundRayDistance);
            if (!isGrounded)
            {
                var y = transform.position.y - (gravity * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                return;
            }

            // Enable or disable the agent depending on whether we are grounded.
            // NavMeshAgent doesn't like it when we call SetDestination() when not on a NavMesh.
            _agent.enabled = isGrounded;

            // If we have a target, lets configure the agent to move toward them.
            if (target != null)
            {
                movementState = EnemyControllerState.Goto;
                goToPos = target.transform.position;

                // Check distance to target
                isWithinStoppingDistance = disToTarget < _agent.stoppingDistance;
            }
            else
            {
                movementState = EnemyControllerState.Search;

                isWithinStoppingDistance = false;
            }


            switch (movementState)
            {
                case EnemyControllerState.Goto:
                    GoTo();
                    break;
                case EnemyControllerState.Search:
                    Search();
                    break;
            }



        }

        // ------------------------------------------------- //

        #region Sight

        void FindVisibleTarget()
        {
            target = null;
            Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);
            for (int i = 0; i < targetsInRadius.Length; i++)
            {
                Transform target = targetsInRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {

                        this.target = target;

                    }
                }
            }
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

        void GoTo()
        {
            _agent.SetDestination(goToPos);
        }

        float time;

        void Search()
        {
            if (disToTarget < searchDisToTarget)
            {
                time = 0;
                goToPos = transform.position + new Vector3(Random.Range(-5, 5), transform.position.y, Random.Range(-5, 5));
            }
            if (time >= 1)
            {
                time = 0;
                goToPos = transform.position + new Vector3(Random.Range(-5, 5), transform.position.y, Random.Range(-5, 5));
            }
            time += Time.deltaTime;
            _agent.SetDestination(goToPos);
        }

    }

    public enum EnemyControllerState
    {
        Search,
        Goto,
        Flee,
        Shoot,
        Melee

    }
}