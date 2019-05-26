using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public class SuperSpeed : Ability
    {

        public int speedMultiplier = 2;

        public float cameraFOV = 80;
        public float cameraFOVZoomTime = 0.15f;

        public GameObject pickupModel;

        // ------------------------------------------------- //

        protected override void OnPickup()
        {

            // Hide the pickup model
            if (pickupModel != null)
                pickupModel.SetActive(false);

        }

        // ------------------------------------------------- //

        protected override void OnUse()
        {

            var player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<FPSController>().isDoubleSpeed = true;
            player.GetComponent<FPSController>().cameraLook.ZoomTo(cameraFOV, cameraFOVZoomTime);

        }

        protected override void OnStop()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<FPSController>().isDoubleSpeed = false;
            player.GetComponent<FPSController>().cameraLook.ZoomToDefault(cameraFOVZoomTime);

        }

        // ------------------------------------------------- //

    }
}
