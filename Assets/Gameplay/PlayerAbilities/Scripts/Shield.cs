using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public class Shield : Ability
    {
        
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
            player.GetComponent<FPSController>().isInvincible = true;

        }

        protected override void OnStop()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<FPSController>().isInvincible = false;

        }

        // ------------------------------------------------- //

    }
}
