using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(SphereCollider))]
    public class Throwable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        public string baseName = "New Throwable";

        [Header("Numbers")]
        [SerializeField]
        public int damage = 10;
        public int maxAmmo = 5;
        public int maxClip = 5;
        public float range = 10f;
        public float throwForce = 1f;
        //public float lineDelay = .1f;

        // ------------------------------------------------- //

        // Mechanics
        private int ammo = 0;
        private int clip = 0;

        //private bool canThrow = false;
        //private float shootTimer = 0f;

        // Components
        protected Rigidbody rigid;
        private BoxCollider boxCollider;
        private LineRenderer lineRenderer;
        private SphereCollider sphereCollider;

        private Spin pickupSpin;
        private GameObject pickupGlow;

        // ------------------------------------------------- //

        private void Awake()
        {
            // Get required components.
            GetComponentReferences();

            // Allow dervied weapons to be notified that we have awoken.
            OnAwake();

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
            //lineRenderer.enabled = false;

            // Turn off rigidbody
            rigid.isKinematic = true;

            // Apply bounds to box collider
            boxCollider.center = bounds.center - transform.position;
            boxCollider.size = bounds.size;

            // Configure sphere collider as trigger
            sphereCollider.center = boxCollider.center;
            sphereCollider.radius = boxCollider.size.magnitude * 2f;
            sphereCollider.isTrigger = true;

            // Pickup glow and spin
            pickupSpin.speed = 100f;
            pickupSpin.axis = Vector3.up;
            pickupGlow.SetActive(true);

        }

        private void GetComponentReferences()
        {
            rigid = GetComponent<Rigidbody>();
            boxCollider = GetComponent<BoxCollider>();
            lineRenderer = GetComponent<LineRenderer>();
            sphereCollider = GetComponent<SphereCollider>();

            pickupSpin = GetComponent<Spin>();
            pickupGlow = transform.Find("PickupGlow").gameObject;
        }

        // ------------------------------------------------- //

        private void Start()
        {
            
        }

        // ------------------------------------------------- //

        private void Update()
        {
            //// Increase shoot timer
            //shootTimer += Time.deltaTime;

            //// If time reaches rate
            //if (shootTimer >= throwRate)
            //{
            //    canShoot = true;
            //}

            // Allow derived weapons to be notified that we have updated.
            OnUpdate();
        }

        // ------------------------------------------------- //

        public void Pickup()
        {
            // Disable physics (set to true)
            //rigid.isKinematic = true;

            // Disable trigger collider so we don't trigger the UI when we look at the weapon in our hand
            sphereCollider.enabled = false;

            // Disable glow and spin
            pickupSpin.enabled = false;
            pickupGlow.SetActive(false);

            // Allow derived class to be notified that its been picked up.
            OnPickup();
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

        public virtual void StartThrow()
        {
            //if (!canThrow)
            //    return;

            // Allow derived weapon class to handle pre-firing logic.
            // The derived weapon class has the ability to cancel any bullets about to fire.
            if (!OnThrowBegin())
                return;


            // Detach from player hand
            transform.SetParent(null);


            //if (bulletPrefab != null)
            //{
            //    // Instantiate a bullet. Its script will do the rest.
            //    Bullet.SpawnNew(bulletPrefab, shotOrigin, damage, range, throwForce);

            //}
            //else
            //{
            //    // Backup method. Shoot a ray to simulate a bullet.

            //    // Create a bullet ray from shot origin to forward
            //    Ray bulletRay = new Ray(shotOrigin.position, shotOrigin.forward);
            //    RaycastHit hit;

            //    // Perform Raycast (Hit Scan)
            //    if (Physics.Raycast(bulletRay, out hit, range))
            //    {
            //        var killable = hit.collider.GetComponent<IKillable>();

            //        if (killable != null)
            //        {
            //            // Deal damage to enemy
            //            killable.TakeDamage(damage);
            //        }
            //    }

            //    // Show Line
            //    StartCoroutine(ShowLine(bulletRay, lineDelay));

            //}


            // Reset timer
            //shootTimer = 0;

            // Can't shoot anymore
            //canThrow = false;
        }

        public void StopThrowing()
        {
            OnThrowRelease();
        }

        // ------------------------------------------------- //

        protected virtual void OnAwake()
        {
            // Allow derived weapons to be notified when the object awakens.
        }

        protected virtual void OnUpdate()
        {
            // Allow derived weapons to be notified when the object has been updated.
        }

        // ------------------------------------------------- //

        protected virtual void OnPickup()
        {
            // Allows derived throwables to be notified when the player picks it up.

        }

        /// <summary>Override this method to run logic when <see cref="StartThrow()"/> is called,
        /// but before any bullets are fired. If false is returned, no bullets will be fired.</summary>
        protected virtual bool OnThrowBegin()
        {
            // Allow the throw action to occur (default).
            return true;
        }

        protected virtual void OnThrowRelease()
        {
            // Allows derived throwables to be notified when the player stops/cancels throwing.
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
            return (!string.IsNullOrEmpty(baseName)) ? baseName : "Unnamed Weapon";
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}