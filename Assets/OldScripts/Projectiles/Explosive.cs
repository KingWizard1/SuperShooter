using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooterOld
{

    public class Explosive : Projectile
    {
        public float explosionRadius;
        public float explosionDelay;

        public GameObject explosionEffect;

        // ------------------------------------------------- //

        public override void Fire(Vector3 direction)
        {
            base.Fire(direction);
            //rigid.useGravity = true;
        }

        // ------------------------------------------------- //

        public override void OnCollisionEnter(Collision collision)
        {
            string tag = collision.collider.tag;
            if (tag != "Player" || tag != "Weapon")
            {
                DoExplosion();
                DoEffects();
            }
        }

        // ------------------------------------------------- //

        private void DoExplosion()
        {
            //Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
            //foreach(var hit in hits)
            //{
            //	Enemy e = hit.GetComponent<Enemy>();
            //	if(e)
            //	{
            //		//e.TakeDamage(damage);
            //	}
            //}
            GameObject.Destroy(this.gameObject);
        }

        // ------------------------------------------------- //

        private void DoEffects()
        {
            GameObject explosion = Instantiate(explosionEffect);
            explosion.transform.position = transform.position;
            explosion.transform.localRotation = hitRotation;
        }
    }



}