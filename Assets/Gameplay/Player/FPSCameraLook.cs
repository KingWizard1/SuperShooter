using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class FPSCameraLook : MonoBehaviour
    {
        public Camera attachedCamera;

        [Tooltip("Tick to hide the cursor in the game view.")]
        public bool showCursor = false;

        [Tooltip("Invert mouse look.")]
        public bool isInverted = false;

        public Vector2 speed = new Vector2(120f, 120f);

        // Camera tilt up/down limit
        public float yMinLimit = -80f;
        public float yMaxLimit = 80f;

        public Transform aimTarget;

        // ------------------------------------------------- //

        // Current X and Y rotation
        private float x, y;

        private FPSController parentController;

        // ------------------------------------------------- //

        void Start()
        {

            // Hide and lock the cursor to the game view, if enabled
            Cursor.visible = showCursor;
            Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;

            // Get current camera Euler rotation
            Vector3 angles = transform.eulerAngles;
            x = angles.y; // Pitch (X) Yaw (Y) Roll (Z)
            y = angles.x;

            // Check that our direct parent has the required component for us to rotate with.
            parentController = transform.parent.gameObject.GetComponent<FPSController>();
            if (parentController == null)
                Debug.LogWarning("[FPS] The direct parent of this camera does NOT have a FPSPlayerController attached to it!");
        }

        // ------------------------------------------------- //

        void Update()
        {
            // Prevent mouse movements from affecting gameplay
            // when the game window is not in focus (works both in-editor and out).
            if (!Application.isFocused)
                return;

            // Rotate camera based on mouse X and Y
            var mouseX = Input.GetAxis("Mouse X") * speed.x * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * speed.y * Time.deltaTime;

            // Invert if required
            if (isInverted) mouseY = -mouseY;

            // Add
            x += mouseX;
            y -= mouseY;

            // Clamp the angle of the pitch
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

            // Rotate local on X axis (Pitch)
            // Rotate parent on Y axis (Yaw)
            //if (parentController != null)
            //parentController.SetCameraLook(Quaternion.Euler(0, x, 0));
            //else


            Ray camRay = attachedCamera.ViewportPointToRay(new Vector3(.5f, .5f));
            RaycastHit hit;
            if(Physics.Raycast(camRay, out hit))
            {
                aimTarget.position = hit.point;
            }

            _applyRotation();

        }

        private void _applyRotation()
        {
            transform.parent.rotation = Quaternion.Euler(0, x, 0);
            transform.localRotation = Quaternion.Euler(y, 0, 0);
        }

        // ------------------------------------------------- //

        public void SetRotation(float x, float y)
        {
            this.x = x;
            this.y = y;
            _applyRotation();
        }

        // ------------------------------------------------- //


    }

}