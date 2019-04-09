using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.Utility;

namespace SuperShooter
{

    public class CarAIAutoRespawn : MonoBehaviour
    {
        //[ReadOnly]
        public float currentSpeed;

        public float respawnWhenBelow = 10;

        public float secondsTillRespawn = 5;

        // ------------------------------------------------- //

        // References
        private CarAIControl carAI;
        private CarController carController;
        private WaypointProgressTracker waypointTracker;

        // Start pos
        private Vector3 worldStartPosition;

        // Variables used in Update()
        private float respawnTimer = 0;
        private bool isUnderThreshold;
        private bool timing = false;


        // ------------------------------------------------- //

        private void Awake()
        {
            carAI = GetComponent<CarAIControl>();
            carController = GetComponent<CarController>();
            waypointTracker = GetComponent<WaypointProgressTracker>();
        }

        // ------------------------------------------------- //

        private void Start()
        {
            // Get world pos to respawn at.
            worldStartPosition = transform.position;
        }

        // ------------------------------------------------- //

        private void Update()
        {

            // Update inspector
            if (Application.isEditor)
                currentSpeed = carController.CurrentSpeed;

            isUnderThreshold = carController.CurrentSpeed < respawnWhenBelow;

            // Are we counting down to respawn? (No)
            if (!timing)
            {
                // Check vehicle speed. Is it below the threshold?
                if (isUnderThreshold)
                {
                    // Set timer to wait time, and flag that we're now counting down.
                    respawnTimer = secondsTillRespawn;
                    timing = true;
                }

                // Else do nothing.

            }
            else
            {
                // Cancel timer if we're no longer under the speed threshold.
                if (!isUnderThreshold) {
                    timing = false;
                    return;
                }

                // Decrement timer
                respawnTimer -= Time.deltaTime;

                //Debug.Log("Respawning " + name + " in " + respawnTimer + " sec");

                if (respawnTimer <= 0)
                {
                    // Do the magic
                    Respawn();

                    // Stop timing.
                    timing = false;
                }
            }
        }

        private void Respawn()
        {
            // Move the vehicle back to its start position. 
            transform.SetPositionAndRotation(worldStartPosition, Quaternion.identity);

            // Reset the waypoint tracker so that the first point in the circuit is the next waypoint.
            waypointTracker.Reset();

            // In case its been turned off (like if the car exploded or something Michael Bay-ish)
            if (carAI != null) carAI.enabled = true;

        }
    }

}