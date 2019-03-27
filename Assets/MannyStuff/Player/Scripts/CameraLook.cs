using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {

    [Tooltip("Tick to hide the cursor in the game view.")]
    public bool showCursor = false;

    [Tooltip("Invert mouse look.")]
    public bool isInverted = false;

    public Vector2 speed = new Vector2(120f, 120f);

    // Camera tilt up/down limit
    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;

    public GameObject objectToRotate;

    // ----------------------------------------------------- //

    // Current X and Y rotation
    private float x, y;

    // ----------------------------------------------------- //

	void Start () {

        // Hide and lock the cursor to the game view, if enabled
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.Locked : CursorLockMode.None;

        // Get current camera Euler rotation
        Vector3 angles = transform.eulerAngles;
        x = angles.y; // Pitch (X) Yaw (Y) Roll (Z)
        y = angles.x;

	}

    // ----------------------------------------------------- //

    void Update () {


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
        transform.parent.rotation = Quaternion.Euler(0, x, 0);
        transform.localRotation = Quaternion.Euler(y, 0, 0);



    }
}
