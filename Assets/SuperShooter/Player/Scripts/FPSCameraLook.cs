using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    [RequireComponent(typeof(Camera))]
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

        // Recoil
        public float resolveSpeed = 10f;
        private Vector2 euler; // Current rotation of the camera
        private Vector3 targetOffset, currentOffset;

        

        // ------------------------------------------------- //

        public float FOV => attachedCamera?.fieldOfView ?? 0;

        // ------------------------------------------------- //

        // Current X and Y rotation
        private float x, y;

        public float defaultFOV { get; private set; } = 60;
        private float targetFOV = 0;
        private float lastTargetFOV = 0;
        private float zoomTime = 0;
        const float defaultZoomTime = 0.25f;

        private FPSController _parentController;

        // ------------------------------------------------- //

        void Awake()
        {
            attachedCamera = GetComponent<Camera>();
        }

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


            // Remember current camera FOV
            targetFOV = attachedCamera.fieldOfView;
            defaultFOV = attachedCamera.fieldOfView;
            lastTargetFOV = attachedCamera.fieldOfView;


            // Check that our direct parent has the required component for us to rotate with.
            _parentController = transform.parent.gameObject.GetComponent<FPSController>();
            if (_parentController == null)
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

            // ----- NEW -----

            // Recoil.
            // Lerp the offset towards Target Offset
            targetOffset = Vector3.Lerp(targetOffset, Vector3.zero, resolveSpeed * Time.deltaTime);
            currentOffset = Vector3.MoveTowards(currentOffset, targetOffset, resolveSpeed * Time.deltaTime);

            // ---------------

            // Rotate local on X axis (Pitch)
            // Rotate parent on Y axis (Yaw)
            //if (parentController != null)
            //parentController.SetCameraLook(Quaternion.Euler(0, x, 0));
            //else

            // Move the aim target to the world point
            // that is in the absolute center of the viewport.
            //Ray camRay = attachedCamera.ViewportPointToRay(new Vector3(.5f, .5f));
            //RaycastHit hit;
            //if(Physics.Raycast(camRay, out hit))
            //{
            //    aimTarget.position = hit.point;
            //}

            _applyRotation();

            _applyZoom();

        }

        private void _applyRotation()
        {
            // Origin
            //transform.parent.rotation = Quaternion.Euler(0, x, 0);
            //transform.localRotation = Quaternion.Euler(y, 0, 0);

            // OpenWorldGame
            // Rotate the Player and Transform seperately
            transform.parent.localEulerAngles = new Vector3(0f, x + currentOffset.y, 0f);
            transform.localEulerAngles = new Vector3(y - currentOffset.x, 0f, 0f);
           
            
        }

        // ------------------------------------------------- //

        // Related to recoil
        public void SetTargetOffset(Vector3 offset)
        {
            targetOffset = offset;
        }

        // ------------------------------------------------- //

        private float zoomTimer;
        
        private void _applyZoom()
        {

            zoomTimer += Time.deltaTime;

            if (zoomTimer >= zoomTime) {
                attachedCamera.fieldOfView = targetFOV;
                return;
            }

            // Apply interpolated fov
            var fov = Mathf.Lerp(lastTargetFOV, targetFOV, (zoomTimer / zoomTime));
            attachedCamera.fieldOfView = fov;

        }

        public void ZoomTo(float fov, float time = defaultZoomTime)
        {
            lastTargetFOV = targetFOV;
            targetFOV = fov;
            zoomTime = time;
            zoomTimer = 0;
        }

        public void ZoomToDefault(float time = defaultZoomTime)
        {
            ZoomTo(defaultFOV, time);
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