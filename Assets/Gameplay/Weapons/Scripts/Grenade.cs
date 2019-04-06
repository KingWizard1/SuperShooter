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
        public float radius = 5f; // radius of explosine for force
        public float force = 700; // add force to physic base object that have rigidBody

        public GameObject explosionPatical;
        public GameObject damageSphere;

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
                Instantiate(explosionPatical, transform.position, transform.rotation);
                Instantiate(damageSphere, transform.position, transform.rotation);
                Collider[] collider = Physics.OverlapSphere(transform.position, radius);
                
                foreach(Collider nerbyObject in collider)
                {
                    Rigidbody rb = nerbyObject.GetComponent<Rigidbody>();
                    if(rb != null)
                    {
                        rb.AddExplosionForce(force, transform.position, radius);
                    }
                }
               
                Destroy(gameObject);
            }
        }
        
        

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}