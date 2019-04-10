using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    //[RequireComponent(typeof(BoxCollider))]
    //[RequireComponent(typeof(LineRenderer))]
    //[RequireComponent(typeof(Timeline))]
    [RequireComponent(typeof(Projectile))]
    public class Bullet : MonoBehaviour //, IInteractable
    {
        [SerializeField]
        public string baseName = "Bullet";

        public float lifetime = 5;

        // ------------------------------------------------- //

        // Values
        private int damage;
        private int range;
        private float force;
        private Vector3 direction;

        // State
        private bool addForce = true;

        // Components
        private Rigidbody rigid;
        //private BoxCollider boxCollider;
        //private LineRenderer lineRenderer;
        private Projectile projectile;

        // Timing

        private Timeline timeline;


        // ------------------------------------------------- //

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            //timeline.GetComponent<Timeline>();
            projectile = GetComponent<Projectile>();
        }

        // ------------------------------------------------- //

        private void Start()
        {
            addForce = true;
        }

        // ------------------------------------------------- //

        private void Update()
        {
            lifetime -= Time.deltaTime;

            if (lifetime <= 0)
                Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            //transform.position += transform.forward * Time.deltaTime * (speed);

            //if (addForce)
            //    rigid.AddForce(transform.forward * force);
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
            var player = other.GetComponent<PlayerHealth>();
            if (player != null)
                player.TakeDamage(damage);

        }

        // ------------------------------------------------- //

        public static GameObject SpawnNew(GameObject prefab, Transform origin, /*Vector3 direction,*/
                                            int damage, int range, float force)
        {
            var bullet = Instantiate(prefab);
            bullet.transform.SetPositionAndRotation(origin.position, origin.rotation);

            var script = bullet.GetComponent<Bullet>();
            if (script != null)
                script.Configure(/*direction,*/ damage, range, force);
            else
                Debug.LogError("Cannot properly instantiate bullet: prefab has no bullet script attached to it.");
            return bullet;
        }

        public void Configure(/*Vector3 direction,*/ int damage, int range, float force)
        {
            //this.direction = direction;
            this.damage = damage;
            this.range = range;
            this.force = force;

            projectile.amount = range;
            projectile.SetProjectileHitCallback(BulletHitCallback);
        }

        private void BulletHitCallback(RaycastHit hit)
        {

            // Becomes true if we hit something we actually want to
            // register as a 'hit'.
            bool hitSomething = false;

            // Check if rigid
            if (hit.rigidbody) {

                // We hit something
                hitSomething = true;

                // Calculate push direction from move direction.
                var pushDir = hit.transform.position - transform.position;

                // Apply!
                hit.rigidbody.velocity = pushDir.normalized * force;

            }


            // Check if killable
            var killable = hit.transform.GetComponent<IKillable>();
            if (killable != null) {

                // We hit something
                hitSomething = true;

                // Take damage
                killable.TakeDamage(damage);

            }


            // Show UI hit marker
            if (hitSomething)
            {

                // Show HUD hit marker
                UIManager.Main.CrossHair.ShowHitMarker(Color.white);

                // TODO
                // Award points

            }

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