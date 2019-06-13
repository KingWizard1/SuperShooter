﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuperShooter
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        [Header("State")]
        public EnemyControllerState state = EnemyControllerState.Search;
        [SerializeField]
        private Vector3 goToPos;
        [SerializeField]
        private float disToTarget;
        /// <summary>
        /// The distance away from the 'goToPos' that results in the 'Search' function.
        /// </summary>
        public float searchDisToTarget;
        [SerializeField]
        private Vector3 lastKnownPosition;

        [Header("Sight")]
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;
        public LayerMask targetLayer;
        public LayerMask obstacleMask;
        public Transform target;

        [Header("Physics")]
        public float gravity = 10f;
        public float groundRayDistance = 1.1f;
        
        bool hasChecked;


        // ------------------------------------------------- //

        // Scripting properties
        public bool isAgentActive => _agent.enabled;
        public bool isWithinStoppingDistance { get; private set; }

        // Expose the agent's stopping distance.
        public float stoppingDistance {
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

        void Update()
        {
            
            // Are we on the ground?
            // If not, simulate gravity and fall until we do hit ground.
            bool isGrounded = transform.CheckIfGrounded(out RaycastHit hit, groundRayDistance);
            if (!isGrounded) {
                var y = transform.position.y - (gravity * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                return;
            }

            // Enable or disable the agent depending on whether we are grounded.
            // NavMeshAgent doesn't like it when we call SetDestination() when not on a NavMesh.
            _agent.enabled = isGrounded;

            // Bail if theres nothing to do at this point.
            if (!_agent.enabled || _agent.isStopped)
                return;

            // 
            FindVisibleTarget();

            disToTarget = (goToPos - transform.position).magnitude;

            // Check distance to target
            isWithinStoppingDistance = disToTarget < _agent.stoppingDistance;

            if (isWithinStoppingDistance)
            {
                hasChecked = true;


                transform.LookAt(new Vector3(goToPos.x, transform.position.y, goToPos.z));

            }

            else
            {

                transform.rotation = Quaternion.LookRotation(_agent.velocity);
            }

            // If we have a target, lets configure the agent to move toward them.
            if (target != null)
            {
                hasChecked = false;
                lastKnownPosition = target.position;

                state = EnemyControllerState.Goto;
                goToPos = target.position;
            }
            else
            {
                //if they have checked the players last known pos, it will begin the search.
                if (hasChecked)
                {
                    state = EnemyControllerState.Search;
                }
                else
                {
                    goToPos = lastKnownPosition;
                }
            }


            switch (state)
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

        // ------------------------------------------------- //

        public void Stop()
        {
            state = EnemyControllerState.Stop;
            _agent.isStopped = true;
        }

        // ------------------------------------------------- //

    }

    public enum EnemyControllerState
    {
        Stop,
        Search,
        Goto,
        Flee,
        Shoot,
        Melee,
    }
}