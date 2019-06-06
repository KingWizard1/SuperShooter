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
        public EnemyMovementState moves = EnemyMovementState.Search;
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
        public Transform visibleTarget;

        // ------------------------------------------------- //

        private NavMeshAgent agent;

        // ------------------------------------------------- //

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // ------------------------------------------------- //

        void Update()
        {
            FindVisibleTarget();

            disToTarget = (goToPos - transform.position).magnitude;


            if (visibleTarget != null)
            {
                moves = EnemyMovementState.Goto;
                goToPos = visibleTarget.transform.position;
            }
            else
            {
                moves = EnemyMovementState.Search;

            }


            switch (moves)
            {
                case EnemyMovementState.Goto:
                    GoTo();
                    break;
                case EnemyMovementState.Search:
                    Search();
                    break;
            }
        }

        // ------------------------------------------------- //

        #region Sight

        void FindVisibleTarget()
        {
            visibleTarget = null;
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

                        visibleTarget = target;

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
            agent.SetDestination(goToPos);
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
            agent.SetDestination(goToPos);
        }
    }

    public enum EnemyMovementState
    {
        Search,
        Goto,
        Flee,
        Shoot

    }
}