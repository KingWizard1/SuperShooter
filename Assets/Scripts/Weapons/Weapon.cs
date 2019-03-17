using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    /* Task 1:  Draw.io projectile system
     *          Needs the following structure: 
     *              Projectile
     *              /   |   \
     *          Normal Fire Explosive
     *          Variables and functions for each class
     *          
     *############################################################################ 
     * 
     * Task 2:  Ensure the player can't shoot until the weapon is
     *          ready to be fired (fire rate)
     *          Refer to #game-systems-j211 for resources for this task
     *          
     */

    public abstract class Weapon : MonoBehaviour
    {

        [Header("Name")]
        public string displayName;

        [Header("Values")]
        public int damage = 1;
        public int ammo = 30;
        public float accuracy = 1f;
        public float range = 10f;
        public float fireRate = 5f;

        [Header("Prefabs")]
        public GameObject projectile;

        [Header("World Points")]
        public Transform spawnPoint;
        public Vector3 hitPoint;
        Quaternion hitRotation;

        // ------------------------------------------------- //

        protected int currentAmmo = 0;

        // ------------------------------------------------- //

        /// <summary>Required function for all weapons.</summary>
        public abstract void Attack();

        // ------------------------------------------------- //

        /// <summary>Resets the ammo count to the start count. Override this to implement custom mechanics.</summary>
        public virtual void Reload()
        {
            currentAmmo = ammo;
        }

        // ------------------------------------------------- //

        public Quaternion GetTargetNormal()
        {
            RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Screen);
            Vector3 screenCentre = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = Camera.main.ScreenPointToRay(screenCentre);

            if (Physics.Raycast(ray, out hit))
            {
                hitPoint = hit.point;
                hitRotation = hit.transform.rotation;
            }
            return hitRotation;
        }

    }
}
