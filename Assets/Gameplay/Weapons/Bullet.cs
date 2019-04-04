using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    //[RequireComponent(typeof(BoxCollider))]
    //[RequireComponent(typeof(LineRenderer))]
    public class Bullet : MonoBehaviour //, IInteractable
    {
        [SerializeField]
        public string baseName = "9mm Bullet";


        // ------------------------------------------------- //

        // Values
        private int damage;
        private float range;
        private float force;

        // State
        private bool addForce = true;

        // Components
        private Rigidbody rigid;
        //private BoxCollider boxCollider;
        //private LineRenderer lineRenderer;

        // ------------------------------------------------- //

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        // ------------------------------------------------- //

        private void Start()
        {
            addForce = true;
            StartCoroutine(DestroyAfter(5f));
        }

        private IEnumerator DestroyAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(this);
        }

        // ------------------------------------------------- //

        private void FixedUpdate()
        {
            //transform.position += transform.forward * Time.deltaTime * (speed);

            if (addForce)
                rigid.AddForce(transform.forward * force);
        }

        // ------------------------------------------------- //

        private void OnTriggerEnter(Collider other)
        {
            
            // Cease forward momentum
            addForce = false;

            //// Bail if the object does not have a Rigidbody, or has one but isKinematic.
            //var otherRigid = other.GetComponent<Rigidbody>();
            //if (otherRigid == null || otherRigid.isKinematic)
            //    return;

            //// Calculate push direction from move direction.
            //// We only push objects to the sides, never up and down (???)
            ////var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            //// Apply!
            //otherRigid.velocity = transform.position * speed;


            // Apply damage
            var player = other.GetComponent<FPSController>();
            if (player != null)
                player.TakeDamage(damage);

        }

        // ------------------------------------------------- //

        public static GameObject SpawnNew(GameObject prefab, Transform origin, int damage, float range, float speed)
        {
            var bullet = Instantiate(prefab);
            bullet.transform.SetPositionAndRotation(origin.position, origin.rotation);
            var script = bullet.GetComponent<Bullet>();
            if (script != null)
                script.Configure(damage, range, speed);
            else
                Debug.LogError("Cannot properly instantiate bullet: prefab has no bullet script attached to it.");
            return bullet;
        }

        public void Configure(int damage, float range, float speed)
        {
            this.damage = damage;
            this.range = range;
            this.force = speed;
        }

        // ------------------------------------------------- //

        //IEnumerator ShowLine(Ray bulletRay, float lineDelay)
        //{
        //    // Enable and Set Line
        //    lineRenderer.enabled = true;
        //    lineRenderer.SetPosition(0, bulletRay.origin);
        //    lineRenderer.SetPosition(1, bulletRay.origin + bulletRay.direction * range);

        //    // Wait
        //    yield return new WaitForSeconds(lineDelay);

        //    // Disable
        //    lineRenderer.enabled = false;
        //}

        // ------------------------------------------------- //

        public virtual string GetDisplayName()
        {
            return (!string.IsNullOrEmpty(baseName)) ? baseName : "Bullet";
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}