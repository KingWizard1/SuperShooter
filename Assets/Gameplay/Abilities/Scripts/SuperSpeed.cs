using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public class SuperSpeed : Ability
    {

        public int speedMultiplier = 2;

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        protected override void OnUse()
        {

            var player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<FPSController>().isDoubleSpeed = true;

        }

        // ------------------------------------------------- //

        public override string GetDisplayName()
        {
            return "Super Speed";
        }

        // ------------------------------------------------- //

    }
}
