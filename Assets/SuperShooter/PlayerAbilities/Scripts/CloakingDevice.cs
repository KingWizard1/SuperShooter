using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public class CloakingDevice : Ability
    {
        
        public GameObject pickupModel;

        // ------------------------------------------------- //

        private MeshRenderer playerRend;

        // ------------------------------------------------- //

        protected override void OnPickup()
        {

            // Hide the pickup model
            if (pickupModel != null)
                pickupModel.SetActive(false);


            var player = GameObject.FindGameObjectWithTag("Player");
            playerRend = player.GetComponent<MeshRenderer>();

        }

        // ------------------------------------------------- //

        protected override void OnUse()
        {
            playerRend.enabled = false;
        }

        protected override void OnStop()
        {
            playerRend.enabled = true;
        }

        // ------------------------------------------------- //

    }
}
