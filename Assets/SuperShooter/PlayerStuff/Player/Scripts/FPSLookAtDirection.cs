using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLookAtDirection : MonoBehaviour
{

    public Transform target;

    // ------------------------------------------------- //

    private void LateUpdate()
    {
        //if (target != null)
        //    transform.LookAt(target);
        if (target)
            transform.rotation = Quaternion.LookRotation(target.forward);
    }

    // ------------------------------------------------- //

}
