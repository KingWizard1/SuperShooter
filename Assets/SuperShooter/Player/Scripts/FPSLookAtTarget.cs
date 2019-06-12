using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLookAtTarget : MonoBehaviour
{

    [Tooltip("Set to the AimTarget of the CameraSystem.")]
    public Transform target;

    // ------------------------------------------------- //

    private void LateUpdate()
    {
        if (target != null)
            transform.LookAt(target);
    }

    // ------------------------------------------------- //

}
