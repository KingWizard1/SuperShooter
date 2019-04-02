using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class Minigun : Weapon
    {
        [Header("Minigun")]
        public GameObject minigunModel;

        // ------------------------------------------------- //

        private Animator anim;
        
        // True if the gun is ready to be fired/is firing.
        private bool isSpunUp = false;

        private bool isSpinningDown = false;

        // Num frames required to wait until bullets can be fired.
        // Can we get this value automatically from the animations? animation.Length?
        private int framesUntilSpunUp = 60;


        // ------------------------------------------------- //

        protected override void OnAwake()
        {
            //
            if (minigunModel != null) {
                anim = minigunModel.GetComponent<Animator>();
                if (anim == null)
                    Debug.LogWarning("Minigun '" + name + "' does not have an animator attached to its model.");
            }
            else
                Debug.LogWarning("Minigun '" + name + "' is missing its model object references.");


        }

        // ------------------------------------------------- //

        //public void Start()
        //{

        //}

        protected override void OnUpdate()
        {
            if (isSpinningDown)
            {
                if (frames > 0)
                    frames--;
            }
        }

        // ------------------------------------------------- //

        private int frames;

        protected override bool OnShoot()
        {

            if (!isSpunUp)
            {
                // Ensure startup animation is playing
                anim.SetBool("spinUp", true);

                // No longer in spin down mode
                isSpinningDown = false;


                // Count up till the animation is finished
                frames++;

                if (frames >= framesUntilSpunUp) {

                    // Gun is ready.
                    isSpunUp = true;
                    //frames = 0;

                    // Start the sustained fire animation (loops)
                    // TODO

                }

                // Disallow bullets to fire.
                return false;
            }
            else
            {

                // Ensure firing animation is playing
                // TODO
                //Debug.Log("FIRING");


                // Allow bullets to fire.
                return true;
            }

        }

        protected override void OnShootStop()
        {
            // Player has released the shoot button.


            if (isSpunUp) {

                // Start the slow down animation (one shot)
                anim.SetBool("spinUp", false);

                //Debug.Log("STOPPING");

                isSpunUp = false;
                isSpinningDown = true;

            }

        }


        // ------------------------------------------------- //


        // ------------------------------------------------- //



    }

}