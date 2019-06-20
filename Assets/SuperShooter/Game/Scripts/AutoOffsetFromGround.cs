using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace SuperShooter
{
    
    public class AutoOffsetFromGround : MonoBehaviour
    {

        public float offset;

        public bool runOnStart = true;

        public bool runEveryUpdate = false;

        public float raycastDistance = 5f;

        public Vector3 origin;

        private bool _originIsSet = false;

        // ------------------------------------------------- //

        private void Start()
        {

            if (runOnStart)
                OffsetFromGround(raycastDistance);

        }

        // ------------------------------------------------- //

        [Button]
        public void OffsetFromGround() => OffsetFromGround(raycastDistance);

        public void OffsetFromGround(float raycastDistance)
        {
            // Save original position
            if (!_originIsSet) {
                origin = transform.position;
                _originIsSet = true;
            }
            

            var grounded = transform.CheckIfGrounded(out RaycastHit hit, raycastDistance);

            if (grounded)
                transform.position = (origin + new Vector3(0, offset, 0));
            else
                Debug.LogWarning($"{name} could not be offset from ground. " +
                    $"No collider found in a downward direction on the Y axis from this object.");

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}