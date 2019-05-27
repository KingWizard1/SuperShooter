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


        // Num frames required to wait until bullets can be fired.
        // Can we get this value automatically from the animations? animation.Length?
        //private int framesUntilSpunUp = 60;

        public float minShootRate = .03f, maxShootRate = .1f;
        public float maxCharge = 2f;
        public float chargeSpeed = 0.5f;
        [Range(0f, 1f)]
        public float percentToFire = .95f;

        // Animation
        private float charge = 0f;
        private float targetCharge = 0f;


        // ------------------------------------------------- //

        protected override void OnAwake()
        {
            //
            if (minigunModel != null)
            {
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
            charge = Mathf.MoveTowards(charge, targetCharge, chargeSpeed * Time.deltaTime);
            anim.SetFloat("Blend", charge);
        }

        // ------------------------------------------------- //

        //private int frames;

        protected override bool OnShoot()
        {
            targetCharge = maxCharge; // Ramp tis up!

            float chargePercentage = charge / maxCharge;
            if (chargePercentage >= percentToFire)
            {
                float rate = Mathf.Lerp(maxShootRate, minShootRate, chargePercentage);
                shootRate = rate;
                //print(rate);
                return true;
            }
            return false;

            //else if (charge >= 0.7)
            //{
            //    shootRate = 0.07f;
            //    return true;
            //}
            //else if (charge >= 0.4)
            //{
            //    shootRate = 0.15f;
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}


            //if (charge >= percentToFire) // Percentage of starting fire
            //    return true;
            //else
            //    return false;


        }

        protected override void OnShootStop()
        {
            targetCharge = 0f;

        }


        // ------------------------------------------------- //


        // ------------------------------------------------- //



    }

}