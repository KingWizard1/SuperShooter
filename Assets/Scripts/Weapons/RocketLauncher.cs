using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class RocketLauncher : Weapon
    {

        public float spread;
        public int magSize;

        public override void Attack()
        {
            Quaternion hitRotation = GetTargetNormal();

            GameObject clone = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            Projectile newBullet = clone.GetComponent<Projectile>();

            newBullet.hitRotation = hitRotation;
            newBullet.Fire(transform.forward);
        }
    }

}