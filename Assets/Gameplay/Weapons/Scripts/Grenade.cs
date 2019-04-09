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

        public float explosionRadius = 5f; // radius of explosine for force
        public float explosionForce = 700; // add force to physic base object that have rigidBody

        public GameObject damageSphere;
        public GameObject explosionParticle;

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        protected override void OnAwake()
        {
            
        }

        protected override void OnPickup()
        {

            GetComponent<Renderer>().enabled = false;

        }

        protected override bool OnThrowBegin()
        {

            isCooking = true;

            GetComponent<Renderer>().enabled = true;

            return true;

        }

        protected override void OnThrowRelease()
        {
            rigid.isKinematic = false;
            rigid.AddForce(transform.forward * 15, ForceMode.Impulse);
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

                if (explosionParticle != null)
                    Instantiate(explosionParticle, transform.position, transform.rotation);

                //if (damageSphere != null)
                //    Instantiate(damageSphere, transform.position, transform.rotation);

                Collider[] collider = Physics.OverlapSphere(transform.position, explosionRadius);
                
                foreach(Collider nerbyObject in collider)
                {
                    var rb = nerbyObject.GetComponent<Rigidbody>();
                    var killable = nerbyObject.GetComponent<IKillable>();
                    if(rb != null)
                    {
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                    else if (killable != null)
                    {
                        // is it the player
                        killable.TakeDamage(damage);
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