using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class Normal : Projectile
    {
        public override void OnCollisionEnter(Collision col)
        {
            // Ignore collissions with other projectiles
            if (col.gameObject.tag == gameObject.tag)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), col.gameObject.GetComponent<Collider>());
            }
        }

        // ------------------------------------------------- //

        public override void Fire(Vector3 direction)
        {
            base.Fire(direction);
        }
    }

}