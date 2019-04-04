using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    
    public class Grenade : Throwable
    {


        public float timer = 6;

        private bool isCooking = false;

        public GameObject timeSphere;

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        protected override void OnAwake()
        {
            
        }

        protected override void OnPickup()
        {
            
        }

        protected override bool OnThrowBegin()
        {

            isCooking = true;

            return true;

        }

        protected override void OnThrowRelease()
        {
            rigid.isKinematic = false;
            rigid.AddForce(transform.forward * 10, ForceMode.Impulse);
        }

        // ------------------------------------------------- //



        private void Update()
        {
            if (isCooking)
                timer -= Time.deltaTime;

        }

        // ------------------------------------------------- //

        void FixedUpdate()
        {

            if (timer <= 0)
            {
                Instantiate(timeSphere, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}