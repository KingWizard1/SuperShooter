using UnityEngine;

namespace SuperShooter
{

    [AddComponentMenu("FirstPerson/Camera - Mouse Look")]
    public class SingleMouseLook : MonoBehaviour
    {

        [Header("Sensitivity")]
        public float xSensitivity = 10f;
        public float ySensitivity = 10f;

        [Header("Y Rotation Clamp")]
        public float minY = -60f;
        public float maxY = 60f;
        //we will have to invert our mouse position later to calculate our mouse look correctly

        public bool cursorVisible;

        // ------------------------------------------------- //

        private float rotationY;
        private GameObject player;
        private GameObject fpsCamera;

        // ------------------------------------------------- //

        void Start()
        {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().freezeRotation = true;
            }
            player = GameObject.FindGameObjectWithTag(GOTags.Player);
            fpsCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        // ------------------------------------------------- //

        void Update()
        {
            player.transform.Rotate(0, Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime, 0);

            rotationY += Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);

            fpsCamera.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);


            ToggleCursor(cursorVisible);
        }

        // ------------------------------------------------- //

        public void ToggleCursor(bool enable)
        {
            if (enable)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}