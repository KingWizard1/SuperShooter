using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    //[RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(SphereCollider))]
    public class Weapon : MonoBehaviour, IInteractable
    {
        [SerializeField]
        public string baseName = "New Weapon";

        [Header("Numbers")]
        public int damage = 10;
        public int maxClips = 8;
        public int maxAmmoPerClip = 30;
        public float spread = 2f;
        public float recoil = 1f;
        public float shootRate = .2f;
        public float bulletForce = 1f;
        public int bulletRange = 10;
        public float lineDelay = .1f;
        public Vector3 playerHandOffset = Vector3.zero;

        [Header("Scope / ADS")]
        public float timeToADS = 0.15f;
        public float timeToUnADS = 0.05f;
        public float[] zoomLevels = new float[1] { 50f };

        [Header("References")]
        public Transform shotOrigin;
        public GameObject bulletPrefab;

        [Header("Cheats")]
        public bool autoReload = false;
        public bool infiniteAmmo = false;

        // ------------------------------------------------- //

        // Mechanics
        public int ammo { get; private set; }
        public int clips { get; private set; }

        public bool isClipEmpty { get; private set; }
        public bool isOutOfAmmo { get; private set; }

        private bool canShoot = false;
        private float shootTimer = 0f;

        // The destination for the bullet.
        //internal Transform aimTarget;

        // Components
        //private Rigidbody rigid;
        //private BoxCollider boxCollider;
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
            lineRenderer.enabled = false;

            //// Turn off rigidbody
            //rigid.isKinematic = true;

            //// Apply bounds to box collider
            //boxCollider.center = bounds.center - transform.position;
            //boxCollider.size = bounds.size;

            //// Configure sphere collider as trigger
            //sphereCollider.center = boxCollider.center;
            //sphereCollider.radius = boxCollider.size.magnitude * 2f;
            //sphereCollider.isTrigger = true;

            // Pickup glow and spin
            pickupSpin.speed = 100f;
            pickupSpin.axis = Vector3.up;
            pickupGlow.SetActive(true);

        }

        private void GetComponentReferences()
        {
            //rigid = GetComponent<Rigidbody>();
            //boxCollider = GetComponent<BoxCollider>();
            lineRenderer = GetComponent<LineRenderer>();
            sphereCollider = GetComponent<SphereCollider>();

            pickupSpin = GetComponent<Spin>();
            pickupGlow = transform.Find("PickupGlow").gameObject;
        }

        private void Start()
        {
            // Check we can shoot from somewhere
            if (shotOrigin == null)
                Debug.LogWarning(string.Format(FPSMessages.WARN_WEAPON_NO_SHOT_ORIGIN, GetDisplayName()));

            // Check we have a bullet prefab to shoot
            if (bulletPrefab == null)
                Debug.LogWarning(string.Format(FPSMessages.WARN_WEAPON_NO_BULLET_PREFAB, GetDisplayName()));

            // Set ammo
            ammo = maxAmmoPerClip;
            clips = maxClips;
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

            // Check ammo status
            isClipEmpty = ammo == 0;
            isOutOfAmmo = clips == 0;

            // Allow derived weapons to be notified that we have updated.
            OnUpdate();
        }

        // ------------------------------------------------- //

        public void Pickup()
        {
            //// Disable physics (set to true)
            //rigid.isKinematic = true;

            // Disable trigger collider so we don't trigger the UI when we look at the weapon in our hand
            sphereCollider.enabled = false;

            // Disable glow and spin
            pickupSpin.enabled = false;
            pickupGlow.SetActive(false);
        }

        public void Drop()
        {
            //// Enable physics (set to false)
            //rigid.isKinematic = false;

            // Allow trigger collider for pickup
            sphereCollider.enabled = true;
        }

        // ------------------------------------------------- //

        public virtual void Reload()
        {

            if (clips > 0)
            {
                // Use clip
                if (!infiniteAmmo)
                    clips--;

                ammo = maxAmmoPerClip;
                isClipEmpty = false;
            }
            else
            {
                isOutOfAmmo = true;
            }

            
        }

        // ------------------------------------------------- //

        public virtual void Shoot()
        {
            if (!canShoot)
                return;

            // Allow derived weapon class to handle pre-firing logic.
            // The derived weapon class has the ability to cancel any bullets about to fire.
            if (!OnShoot())
                return;

            // No bullets to fire!
            if (isClipEmpty)
                return;

            // ------ NEW ------

            var cam = Camera.main;
            var bulletOrigin = cam.transform.position;
            var bulletRotation = cam.transform.rotation; // Rotation of the bullet
            var lineOrigin = shotOrigin.position; // Where the bullet line starts
            var direction = cam.transform.forward; // Forward direction of camera

            // Apply weapon recoil
            Vector3 euler = Vector3.up * 2f;

            // Randomise the pitch
            euler.x = Random.Range(-1f, 1f);

            // Apply offset to camera
            var player = GameObject.FindGameObjectWithTag("Player");
            var playerCamera = player.GetComponentInChildren<FPSCameraLook>();

            playerCamera.SetTargetOffset(euler * recoil);


            // -----------------


            // Instantiate a bullet. Its script will do the rest.
            if (bulletPrefab != null)
            {
                //var direction = shotOrigin.position - aimTarget.position;
                //Bullet.SpawnNew(bulletPrefab, shotOrigin, /*direction,*/ damage, bulletRange, bulletForce);

                var bulletObject = Instantiate(bulletPrefab, bulletOrigin, bulletRotation);
                var bulletScript = bulletObject.GetComponent<RigidBullet>();
                bulletScript.Fire(lineOrigin, direction);

            }
            else
            {
                // Backup method. Shoot a ray to simulate a bullet.
                SimulateBullet();
            }


            // Deplete ammunition
            ammo--;
            if (ammo == 0) {

                if (autoReload)
                    Reload();
                else
                    isClipEmpty = true;
            }

            // Reset timer
            shootTimer = 0;

            // Can't shoot anymore
            canShoot = false;
        }

        public void StopShooting()
        {
            OnShootStop();
        }

        // ------------------------------------------------- //

        private void SimulateBullet()
        {

            // Create a bullet ray from shot origin to forward
            Ray bulletRay = new Ray(shotOrigin.position, shotOrigin.forward);
            RaycastHit hit;

            // Perform Raycast (Hit Scan)
            if (Physics.Raycast(bulletRay, out hit, bulletRange))
            {
                var killable = hit.collider.GetComponent<ICanDie>();

                if (killable != null)
                {
                    // Deal damage to enemy
                    killable.TakeDamage(damage, null);
                }
            }

            // Show Line
            StartCoroutine(ShowLine(bulletRay, lineDelay));

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

        /// <summary>Override this method to run logic when <see cref="Shoot()"/> is called,
        /// but before any bullets are fired. If false is returned, no bullets will be fired.</summary>
        protected virtual bool OnShoot()
        {
            // Allow bullets to be fired (default).
            return true;
        }

        protected virtual void OnShootStop()
        {
            // Allows derived weapons to be notified when the player stops shooting.
        }

        // ------------------------------------------------- //

        IEnumerator ShowLine(Ray bulletRay, float lineDelay)
        {
            // Enable and Set Line
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, bulletRay.origin);
            lineRenderer.SetPosition(1, bulletRay.origin + bulletRay.direction * bulletRange);

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