using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(SphereCollider))]
    public class Weapon : MonoBehaviour, IInteractable
    {
        [SerializeField]
        public string baseName = "New Weapon";

        [Header("Numbers")]
        [SerializeField]
        public int damage = 10;
        public int maxAmmo = 500;
        public int maxClip = 30;
        public float range = 10f;
        public float shootRate = .2f;
        public float bulletForce = 1f;
        public float lineDelay = .1f;

        [Header("References")]
        public Transform shotOrigin;
        public GameObject bulletPrefab;

        // ------------------------------------------------- //

        // Mechanics
        private int ammo = 0;
        private int clip = 0;

        private bool canShoot = false;
        private float shootTimer = 0f;

        // Components
        private Rigidbody rigid;
        private BoxCollider boxCollider;
        private LineRenderer lineRenderer;
        private SphereCollider sphereCollider;

        // ------------------------------------------------- //

        private void Awake()
        {
            GetComponentReferences();
        }

        private void Reset()
        {
            GetComponentReferences();

            // Extend the box collider so that it encapsulates all child objects.
            var children = GetComponentsInChildren<MeshRenderer>();
            Bounds bounds = new Bounds(transform.position, Vector3.zero);
            foreach (var rend in children)
                bounds.Encapsulate(rend.bounds);

            // Turn off line renderer
            lineRenderer.enabled = false;

            // Turn off rigidbody
            rigid.isKinematic = false;

            // Apply bounds to box collider
            boxCollider.center = bounds.center - transform.position;
            boxCollider.size = bounds.size;

            // Configure sphere collider as trigger
            sphereCollider.center = boxCollider.center;
            sphereCollider.radius = boxCollider.size.magnitude * 2f;
            sphereCollider.isTrigger = true;
        }

        private void GetComponentReferences()
        {
            rigid = GetComponent<Rigidbody>();
            boxCollider = GetComponent<BoxCollider>();
            lineRenderer = GetComponent<LineRenderer>();
            sphereCollider = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            if (shotOrigin == null)
                Debug.LogWarning("[WARN] '" + GetDisplayName() + "' does not have a shot origin!");
        }

        // ------------------------------------------------- //

        private void Update()
        {
            // Increase shoot timer
            shootTimer += Time.deltaTime;

            // If time reaches rate
            if (shootTimer >= shootRate)
            {
                canShoot = true;
            }

        }

        // ------------------------------------------------- //

        public void Pickup()
        {
            // Disable physics (set to true)
            rigid.isKinematic = true;

            // Disable trigger collider so we don't trigger the UI when we look at the weapon in our hand
            sphereCollider.enabled = false;
        }

        public void Drop()
        {
            // Enable physics (set to false)
            rigid.isKinematic = false;

            // Allow trigger collider for pickup
            sphereCollider.enabled = true;
        }

        // ------------------------------------------------- //

        public virtual void Reload()
        {
            // THIS IS CRAP, DON'T USE IT.
            clip += ammo;
            ammo -= maxClip;
        }

        // ------------------------------------------------- //

        public virtual void Shoot()
        {
            if (!canShoot)
                return;


            if (bulletPrefab != null)
            {
                // Instantiate a bullet. Its script will do the rest.
                Bullet.SpawnNew(bulletPrefab, shotOrigin, damage, range, bulletForce);

            }
            else
            {
                // Backup method. Shoot a ray to simulate a bullet.

                // Create a bullet ray from shot origin to forward
                Ray bulletRay = new Ray(shotOrigin.position, shotOrigin.forward);
                RaycastHit hit;

                // Perform Raycast (Hit Scan)
                if (Physics.Raycast(bulletRay, out hit, range))
                {
                    var killable = hit.collider.GetComponent<IKillable>();

                    if (killable != null)
                    {
                        // Deal damage to enemy
                        killable.TakeDamage(damage);
                    }
                }

                // Show Line
                StartCoroutine(ShowLine(bulletRay, lineDelay));

            }


            // Reset timer
            shootTimer = 0;

            // Can't shoot anymore
            canShoot = false;
        }

        // ------------------------------------------------- //

        IEnumerator ShowLine(Ray bulletRay, float lineDelay)
        {
            // Enable and Set Line
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, bulletRay.origin);
            lineRenderer.SetPosition(1, bulletRay.origin + bulletRay.direction * range);

            // Wait
            yield return new WaitForSeconds(lineDelay);

            // Disable
            lineRenderer.enabled = false;
        }

        // ------------------------------------------------- //

        public virtual string GetDisplayName()
        {
            return (!string.IsNullOrEmpty(baseName)) ? baseName : "Unnamed Weapon";
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}