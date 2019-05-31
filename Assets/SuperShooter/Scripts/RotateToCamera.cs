using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    /// <summary>Billboarding!</summary>
    public class RotateToCamera : MonoBehaviour
    {

        private void Update()
        {
            transform.RotateToMainCamera();
        }

    }

    public static class TransformUtils
    {
        /// <summary>Rotates the transform so that its forward
        /// axis is directly facing the main camera.</summary>
        public static void RotateToMainCamera(this Transform t)
        {
            // Get main cam
            Transform cam = Camera.main.transform;

            // Get the direction to look at
            Vector3 direction = t.position - cam.position;

            // Rotate by looking in the direction of the camera
            t.rotation = Quaternion.LookRotation(direction);
        }


    }

}