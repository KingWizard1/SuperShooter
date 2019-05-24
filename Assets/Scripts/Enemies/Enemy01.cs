using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy01 : MonoBehaviour
{

    public Moves moves = Moves.Search;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetLayer;
    public LayerMask obstacleMask;

    public Transform visibleTarget;

    [HideInInspector]
    public NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FindVisibleTarget()
    {
        visibleTarget = null;
        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, viewRadius,targetLayer);
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




    void Update()
    {
        FindVisibleTarget();
    }
}
public enum Moves
{
    Search,
    Goto,
    Flee,
    Shoot
}