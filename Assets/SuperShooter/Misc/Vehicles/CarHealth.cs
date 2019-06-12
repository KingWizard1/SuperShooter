using System;
using UnityEngine;

namespace SuperShooter
{
    public class CarHealth : GameEntity, ICharacterEntity
    {

        public int startingHealth = 50;

        public int health { get; private set; }

        private void Start()
        {
            health = startingHealth;
        }


        public void Kill()
        {

            health = 0;

            Explode();
            KillTheEngine();

        }

        public void TakeDamage(int damage, ICharacterEntity from)
        {

            health -= damage;

            if (health <= 0)
                Kill();

        }


        public float explosionRadius = 5f; // radius of explosine for force
        public float explosionForce = 700; // add force to physic base object that have rigidBody
        public int explosionDamage = 1000;

        public GameObject damageSphere;
        public GameObject explosionParticle;

        public bool isDead => throw new NotImplementedException();

        private void Explode()
        {
            if (explosionParticle != null)
                Instantiate(explosionParticle, transform.position, transform.rotation);

            //if (damageSphere != null)
            //    Instantiate(damageSphere, transform.position, transform.rotation);

            Collider[] collider = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider nerbyObject in collider)
            {
                var rb = nerbyObject.GetComponent<Rigidbody>();
                var killable = nerbyObject.GetComponent<ICharacterEntity>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
                else if (killable != null)
                {
                    // is it the player
                    killable.TakeDamage(explosionDamage, this);
                }

            }

        }


        private void KillTheEngine()
        {

            GetComponent<UnityStandardAssets.Vehicles.Car.CarAIControl>().enabled = false;
            GetComponent<UnityStandardAssets.Vehicles.Car.CarController>().Stop();

        }
    }
}
