using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    [RequireComponent(typeof(Rigidbody))]
    public class RigidBullet : MonoBehaviour
    {
        [Header("Numbers")]
        public float speed = 50f;

        [Header("References")]
        public Transform line;
        public GameObject bulletHolePrefab;

        private Rigidbody rigid;

        private void Awake()
        {

            rigid = GetComponent<Rigidbody>();

        }

        private void Start()
        {

            if (bulletHolePrefab == null)
                Debug.LogWarning(string.Format(FPSMessages.WARN_BULLET_NO_DECAL_PREFAB, nameof(RigidBullet)));

        }

        private void Update()
        {
            if (rigid.velocity.magnitude > 0f)
            {
                line.transform.rotation = Quaternion.LookRotation(rigid.velocity);
            }
        }

        public void Fire(Vector3 lineOrigin, Vector3 direction)
        {

            // Set line position to origin
            line.transform.position = lineOrigin;

            // Set bullet flying in direction with speed
            rigid.AddForce(direction * speed, ForceMode.Impulse);

        }

        private void OnCollisionEnter(Collision collision)
        {

            // Create bullet hole prefab at bullet contact point
            if (bulletHolePrefab != null)
            {
                ContactPoint contact = collision.contacts[0];
                Instantiate(bulletHolePrefab, contact.point, Quaternion.LookRotation(contact.normal) *
                                                             Quaternion.AngleAxis(90, Vector3.right));
            }

            // Destroy self
            Destroy(gameObject);
        }
    }
}
