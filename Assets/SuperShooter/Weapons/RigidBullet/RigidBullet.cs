using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    [RequireComponent(typeof(Rigidbody))]
    public class RigidBullet : MonoBehaviour
    {
        //[Header("Numbers")]
        //public float speed = 50f;

        [Header("Art")]
        public Transform line;
        public GameObject bulletHolePrefab;

        private Collider col;
        private Renderer rend;
        private Rigidbody rigid;

        private AudioSource sound;

        // ------------------------------------------------- //

        public delegate void HitCallback(RigidBullet script, Collision collision);

        private HitCallback hitCallback;

        // ------------------------------------------------- //

        /// <summary>Instantiates and returns a new <see cref="RigidBullet"/> at the specified world location and rotation.
        /// The bullet will be "fired" on the next frame in the specified local direction.
        /// A callback can be specified to receive collision data.</summary>
        /// <param name="bulletPrefab">The prefab of the bullet model to use.</param>
        /// <param name="origin">The world-space coordinates to spawn the bullet at.</param>
        /// <param name="rotation">The world-space rotation of the bullet.</param>
        /// <param name="hitCallback">The function to call when the bullet collides with an object.</param>
        public static RigidBullet SpawnNew(GameObject bulletPrefab, Vector3 origin, Quaternion rotation, HitCallback hitCallback = null)
        {
            var bullet = Instantiate(bulletPrefab, origin, rotation);
            var script = bullet.GetComponent<RigidBullet>();
            if (script == null)
                Debug.LogError($"Cannot properly instantiate bullet: prefab '{bulletPrefab.name}' does not have a {nameof(RigidBullet)} script attached to it.");
            script.hitCallback = hitCallback;
            return script;
        }

        // ------------------------------------------------- //

        private void Awake()
        {

            col = GetComponent<Collider>();
            rend = GetComponentInChildren<Renderer>();
            rigid = GetComponent<Rigidbody>();
            sound = GetComponent<AudioSource>();
        }

        // ------------------------------------------------- //

        private void Start()
        {
            // We want to be OFF by default, until Fire() is called.
            rend.enabled = false;
            if (col) col.enabled = false;

            // Warn if no decal prefab
            if (bulletHolePrefab == null)
                Debug.LogWarning(string.Format(FPSMessages.WARN_BULLET_NO_DECAL_PREFAB, nameof(RigidBullet)));

        }

        // ------------------------------------------------- //

        private void Update()
        {
            if (rigid.velocity.magnitude > 0f)
            {
                //line.transform.rotation = Quaternion.LookRotation(rigid.velocity);
            }

            // Bullets can either be fired immediately, or after a delay.
            // The code here allows for both scenarios to occur.
            if (fire && !fired) {
                fireTimer += Time.deltaTime;
                if (fireTimer >= fireTime) {
                    fired = true;
                    fireFunction?.Invoke();
                }
            }
        }

        // ------------------------------------------------- //

        private bool fire;
        private bool fired;
        private float fireTime = 0;
        private float fireTimer = 0;
        private Action fireFunction;

        public void Fire(Vector3 lineOrigin, Vector3 direction, float speed)
        {
            FireWithDelay(lineOrigin, direction, speed, 0f);
        }

        public void FireWithDelay(Vector3 lineOrigin, Vector3 direction, float speed, float delay)
        {

            // Set up a function to fire when the delay timer reaches the target delay time.
            fireFunction = () => {

                // Turn on
                rend.enabled = true;
                if (col) col.enabled = true;
                if (sound) sound.Play();

                // Set line position to origin
                line.transform.position = lineOrigin;

                // Set bullet flying in direction with speed
                rigid.AddForce(direction * speed, ForceMode.Impulse);

            };

            fireTime = delay;   // Set target time.
            fireTimer = 0;      // Reset timer.
            fire = true;        // Allow fire/timer to happen on next Update.
        }

        // ------------------------------------------------- //

        private void OnCollisionEnter(Collision collision)
        {
            // Ignore other bullets
            if (collision.gameObject.GetComponent<RigidBullet>() != null)
                return;
            
            // -- Collisions
            // Fire the callback if not null.
            //Debug.Log($"Hit {collision.gameObject.name}!!!");
            hitCallback?.Invoke(this, collision);

            

            // -- Art
            // Create bullet hole prefab at bullet contact point
            //if (bulletHolePrefab != null)
            //{
            //    ContactPoint contact = collision.contacts[0];
            //    Instantiate(bulletHolePrefab, contact.point, Quaternion.LookRotation(contact.normal) *
            //                                                 Quaternion.AngleAxis(90, Vector3.right));
            //}

            // Destroy self
            Destroy(gameObject);
        }
    }
}
