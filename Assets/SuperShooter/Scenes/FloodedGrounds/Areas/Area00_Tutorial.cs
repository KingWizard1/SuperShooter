using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class Area00_Tutorial : GameArea
    {

        [Header("References")]
        public Weapon weaponToPickup;

        public EnemyCharacter firstKill;
        

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

        protected override bool OnStart()
        {

            if (weaponToPickup == null || firstKill == null) {
                Debug.LogWarning($"One or more required objects have not been assigned to {name}");
                return false;
            }

            // Disable for now
            firstKill.gameObject.SetActive(false);

            return true;

        }

        // ------------------------------------------------- //



        // ------------------------------------------------- //

        protected override void OnUpdate()
        {

            switch (sequence)
            {
                case 0:

                    // Wait till player picks up the gun in this area.
                    if (!weaponToPickup.isPickedUp)
                        return;

                    // TODO
                    // Open the doors

                    // Activate the tutorial enemy.
                    firstKill.gameObject.SetActive(true);

                    // Next
                    NextMilestone();
                        
                    return;

                case 1:

                    // Wait till the tutorial enemy is dead.
                    if (!firstKill.isDead)
                        return;


                    // Tutorial completed.
                    Complete();

                    return;


            }
            
            
        }


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}