using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    [RequireComponent(typeof(Rigidbody))]
    public abstract class Projectile : MonoBehaviour
    {
        /// <summary>How fast the projectile travels over time.</summary>
        public float speed;
        /// <summary>Maximum range before the projectile becomes ineffective, and ends.</summary>
        public float range;

        /// <summary>Applies an elemental effect to the projectile.</summary>
        public ElementalEffect element;

        public Vector3 scale;

        public GameObject impact;

        public Quaternion hitRotation;

        // ------------------------------------------------- //

        private Rigidbody rigid;

        // ------------------------------------------------- //

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        // ------------------------------------------------- //

        public virtual void Fire(Vector3 direction)
        {
            rigid.AddForce(direction * speed, ForceMode.Impulse);
        }

        // ------------------------------------------------- //

        public virtual void OnCollisionEnter(Collision collision)
        {

        }

        public virtual void OnHit()
        {

        }

        public virtual void OnKill()
        {

        }

    }

}