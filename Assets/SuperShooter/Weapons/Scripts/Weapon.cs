﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SuperShooter
{
    public interface IWeapon : ICharacterEntity
    {
        ICharacterController owner { get; }

    }

    public enum WeaponFiringMechanism
    {
        //Manual = 0,
        SemiAutomatic = 1,
        Automatic = 2,
    }

    //[RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(SphereCollider))]
    public class Weapon : CharacterEntity, IWeapon, IInteractablePickup
    {
        [SerializeField]
        public string baseName = "New Weapon";

        public Sprite icon;

        [Header("Numbers")]
        public int damage = 1;
        public int clipsToStart = 32;
        public int maxAmmoPerClip = 24;
        public float spread = 2f;
        public float recoil = 1f;
        public float shootRate = .2f;

        [Header("Firing/Bullets")]
        public WeaponFiringMechanism mechanism = WeaponFiringMechanism.SemiAutomatic;
        public GameObject bulletPrefab;
        public Transform bulletOrigin;
        public float bulletSpeed = 50f;
        public int bulletsPerShot = 1;
        public float timeBetweenShots = 0;
        //public float bulletForce = 1f;
        //public int bulletRange = 10;
        public float lineDelay = .1f;

        [Header("Position / Scope")]
        public bool applyHandOffset = true;
        public Vector3 playerHandOffset = Vector3.zero;
        public Vector3 playerHandADSOffset = Vector3.zero;
        public float timeToADS = 0.15f;
        public float timeToUnADS = 0.05f;
        public float[] zoomLevelOffsets = new float[1] { -10f };

        [Header("Cheats")]
        public bool autoReload = false;
        public bool infiniteAmmo = false;

        [Header("Events")]
        UnityEvent<ICharacterEntity> CrossHairTargetChanged;


        // ------------------------------------------------- //

        // Ownership
        public ICharacterController owner { get; private set; }

        // Runtime Mechanics
        public int ammoInClip { get; private set; }
        public int ammoInBarrel { get; private set; }
        public int ammoRemaining { get; private set; }

        public bool isADS { get; private set; }

        public bool isFullClip { get; private set; }    // "Do you want to mess with this?"
        public bool isLastClip { get; private set; }
        public bool isLastReload { get; private set; }
        public bool isBarrelLoaded { get; private set; }
        public bool isReloadRequired { get; private set; }
        public bool isReloadPossible { get; private set; }
        public bool isTriggerPressed { get; private set; }
        public bool isOutOfAmmo { get; private set; }

        public bool isPickedUp => owner != null;


        public bool readyToFire { get; private set; }

        // ------------------------------------------------- //


        private float shootTimer = 0f;

        // ------------------------------------------------- //

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

        // ------------------------------------------------- //

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

        // ------------------------------------------------- //

        private void GetComponentReferences()
        {
            //rigid = GetComponent<Rigidbody>();
            //boxCollider = GetComponent<BoxCollider>();
            lineRenderer = GetComponent<LineRenderer>();
            sphereCollider = GetComponent<SphereCollider>();

            pickupSpin = GetComponent<Spin>();
            pickupGlow = transform.Find("PickupGlow")?.gameObject;
        }

        public virtual string GetDisplayName()
        {
            return (!string.IsNullOrEmpty(baseName)) ? baseName : "Weapon";
        }

        public string GetInteractionString()
        {
            return $"Pickup {GetDisplayName()}";
        }

        // ------------------------------------------------- //

        private void OnDisable()
        {
            ;
        }

        private void Start()
        {
            // Check ownership. Can be null.
            // But generally a weapon should have an owner of some kind.
            // An owner can be set when the weapon is picked up in Pickup();
            //owner = transform.parent?.GetComponentInParent<CharacterEntity>();

            // Check we can shoot from somewhere
            if (bulletOrigin == null)
                Debug.LogWarning(string.Format(FPSMessages.WARN_WEAPON_NO_SHOT_ORIGIN, GetDisplayName()));

            // Check we have a bullet prefab to shoot
            if (bulletPrefab == null)
                Debug.LogWarning(string.Format(FPSMessages.WARN_WEAPON_NO_BULLET_PREFAB, GetDisplayName()));

            // Set ammo, load the barrel
            ammoInClip = maxAmmoPerClip;
            ammoInBarrel = bulletsPerShot;
            ammoRemaining = maxAmmoPerClip * clipsToStart;
            if (ammoRemaining > 999)
                ammoRemaining = 999;    // Cap it.

        }

        // ------------------------------------------------- //

        //public bool TESTAimAtCrosshair = false;

        private Quaternion crossHairDirection;

        private void Update()
        {

            // Aim.
            //if (TESTAimAtCrosshair)
            //    transform.rotation = AimAtCrosshair();
            AimAtCrosshair();

            // Increase shoot timer. If time reaches threshold, we're ready to fire.
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootRate) {
                ReloadChamber();
                readyToFire = true;
            }

            // Set local position hand offset
            if (applyHandOffset)
                UpdateLocalPosition();

            // Allow derived weapons to be notified that we have updated.
            OnUpdate();

        }

        // ------------------------------------------------- //

        protected virtual Quaternion AimAtCrosshair()
        {

            Ray crossHairRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(crossHairRay, out RaycastHit hit, Mathf.Infinity)) // or 1000f? Hmm.
            {
                Vector3 direction = hit.point - bulletOrigin.position;
                bulletOrigin.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                bulletOrigin.localRotation = Quaternion.Euler(0, 90, 0);
            }


            crossHairDirection = bulletOrigin.rotation;


            // Notify target changed. If entity is coming in as null,
            // it should be treated as though the target is exactly that: nothing. no target.
            //CrossHairTargetChanged.Invoke(entity);
            //OnCrossHairTargetChanged(entity);


            return bulletOrigin.rotation;

        }

        // ------------------------------------------------- //

        public void SwitchToADS()
        {
            isADS = true;
        }

        public void SwitchToHipFire()
        {
            isADS = false;
        }

        public void UpdateLocalPosition()
        {
            if (isPickedUp)
                transform.localPosition = isADS ? playerHandADSOffset : playerHandOffset;
        }

        // ------------------------------------------------- //

        public void Pickup(ICharacterController owner)
        {
            // Try transfer ownership
            this.owner = owner;

            //// Disable physics (set to true)
            //rigid.isKinematic = true;

            // Disable trigger collider so we don't trigger the UI when we look at the weapon in our hand
            sphereCollider.enabled = false;

            // Disable glow and spin
            if (pickupSpin) pickupSpin.enabled = false;
            if (pickupGlow) pickupGlow.SetActive(false);

        }

        public void Drop()
        {
            // No owner.
            owner = null;

            //// Enable physics (set to false)
            //rigid.isKinematic = false;

            // Allow trigger collider for pickup
            sphereCollider.enabled = true;
        }

        // ------------------------------------------------- //

        public virtual void Fire()
        {
            // If this is a semi-automatic weapon, and the trigger is being held, we can't fire.
            // This is because our chamber was emptied on the last frame that the trigger was pressed down.
            if (mechanism == WeaponFiringMechanism.SemiAutomatic && isTriggerPressed)
                return;

            // Set trigger bool. It was released, now its pressed.
            // It releases when the player releases the fire button.
            isTriggerPressed = true;

            // Has enough time elapsed for the weapon to be ready to fire again?
            if (!readyToFire)
                return;

            // Allow derived weapon class to handle pre-firing logic.
            // The derived weapon class has the ability to cancel any bullets about to fire.
            if (!OnShoot())
                return;

            // No bullets to fire!
            if (isReloadRequired)
                return;

            // ------ NEW ------

            var cam = Camera.main;
            //var bulletOrigin = cam.transform.position;
            //var bulletRotation = cam.transform.rotation; // Rotation of the bullet
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

            var shotsFired = 0;

            // Instantiate a bullet. Its script will do the rest.
            if (bulletPrefab != null)
            {
                //var direction = shotOrigin.position - aimTarget.position;
                //Bullet.SpawnNew(bulletPrefab, shotOrigin, /*direction,*/ damage, bulletRange, bulletForce);

                //var bulletObject = Instantiate(bulletPrefab, bulletOrigin, bulletRotation);
                //var bulletScript = bulletObject.GetComponent<RigidBullet>();
                //bulletScript.Fire(lineOrigin, direction);

                var bulletDelay = 0f;

                while (ammoInBarrel > 0)
                {
                    var rigidBullet = RigidBullet.SpawnNew(bulletPrefab, bulletOrigin.position, crossHairDirection, hitCallback);
                    rigidBullet.FireWithDelay(bulletOrigin.position, direction, bulletSpeed, bulletDelay);
                    bulletDelay += timeBetweenShots;
                    ammoInBarrel--;
                    shotsFired++;
                }


            }
            else
            {
                // Backup method. Shoot a ray to simulate a bullet.
                SimulateBullet();
            }


            // Deplete ammo by number of bullets fired
            DepleteAmmoInClip(shotsFired);

            // Reset timer
            shootTimer = 0;

            // Can't shoot anymore, gun needs time to be ready for another shot.
            readyToFire = false;
        }

        public void StopFiring()
        {
            // The firing trigger has been released.
            isTriggerPressed = false;

            // Notify derived class that the player has stopped firing.
            OnShootStop();
        }

        // ------------------------------------------------- //

        private void hitCallback(RigidBullet script, Collision collision)
        {
            //Debug.Log($"Hit {collision.gameObject.name} CALLBACK!!!");

            // Get the game object
            var obj = collision.gameObject;

            // Did we hit something of value?
            var entity =
                obj.GetComponent<CharacterEntity>() ??
                obj.GetComponentInParent<CharacterEntity>() ?? 
                obj.GetComponentInChildren<CharacterEntity>();

            if (entity == null)
                return;

            // If the entity is NOT the Player, and NOT unknown.
            if (entity.type != TargetType.Player && entity.type != TargetType.None)
            {

                // Deal damage !!
                DealDamage(damage, entity);

            }

        }

        // ------------------------------------------------- //

        /// <summary>Deplete the ammount of ammo currently in the weapon's magazine by the specified amount.</summary>
        /// <param name="amount"></param>
        private void DepleteAmmoInClip(int amount)
        {
            // Deplete ammunition
            ammoInClip -= amount;
            ammoInBarrel -= amount;
            if (ammoInClip <= 0) {
                ammoInClip = 0;     // Don't want to go in the minus.
                if (autoReload)     // Cheating!
                    Reload();
            }
                

            // Update ammo bools (must always do this after changing ammo counts)
            UpdateAmmunitionStates();
        }

        private void ReloadChamber()
        {
            if (ammoInClip >= bulletsPerShot)
                ammoInBarrel = bulletsPerShot;
            else
                ammoInBarrel = ammoInClip;
        }

        /// <summary>Replenishes the weapon's current ammunition clip as much as possible.</summary>
        public void Reload()
        {

            // Otherwise...
            if (infiniteAmmo) {
                ammoInClip = maxAmmoPerClip;    // Cheater !!!
            }
            else {

                // Is it possible to reload at this time? And can the clip be replenished?
                if (isReloadPossible && !isFullClip) {

                    // How much ammo do we need to fill the clip?
                    var ammoRequired = maxAmmoPerClip - ammoInClip;

                    // Do we have enough bullets to completely fill the clip?
                    // If yes, refill the clip to its max size, and deplete total ammunition by the amount that was required.
                    // If not, insert the number of ammo remaining into the clip, and set the total ammunition to zero.
                    if (ammoRemaining >= ammoRequired) {
                        ammoInClip = maxAmmoPerClip;
                        ammoInBarrel = bulletsPerShot;
                        ammoRemaining -= ammoRequired;
                    }
                    else {
                        ammoInClip += ammoRemaining;
                        ammoInBarrel = ammoInClip;
                        ammoRemaining = 0;
                    }

                }

            }
            
            // Update ammo bools (must always do this after changing ammo counts)
            UpdateAmmunitionStates();
        }

        private void UpdateAmmunitionStates()
        {
            isLastClip = ammoRemaining == 0;
            isFullClip = ammoInClip == maxAmmoPerClip;
            isLastReload = ammoRemaining <= maxAmmoPerClip;
            isOutOfAmmo = ammoRemaining == 0 && ammoInClip == 0;
            isReloadPossible = ammoRemaining > 0 || infiniteAmmo;
            isReloadRequired = ammoInClip == 0 || ammoInBarrel == 0;
            isBarrelLoaded = ammoInBarrel == bulletsPerShot;
        }

        public virtual void OnReloadStart() { }

        public virtual void OnReloadFinished() { }

        // ------------------------------------------------- //

        private void SimulateBullet()
        {

            // Create a bullet ray from shot origin to forward
            Ray bulletRay = new Ray(bulletOrigin.position, bulletOrigin.forward);
            RaycastHit hit;

            // Perform Raycast (Hit Scan)
            if (Physics.Raycast(bulletRay, out hit, bulletSpeed))   // speed = range with RigidBullets...
            {
                var killable = hit.collider.GetComponent<ICharacterEntity>();

                if (killable != null)
                {
                    // Deal damage to enemy
                    killable.TakeDamage(damage, null);
                }
            }

            // Show Line
            StartCoroutine(LineRoutine(bulletRay, lineDelay));

        }

        IEnumerator LineRoutine(Ray bulletRay, float lineDelay)
        {
            // Enable and Set Line
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, bulletRay.origin);
            lineRenderer.SetPosition(1, bulletRay.origin + bulletRay.direction * bulletSpeed);

            // Wait
            yield return new WaitForSeconds(lineDelay);

            // Disable
            lineRenderer.enabled = false;
        }

        // ------------------------------------------------- //

        /// <summary>Allow derived weapons to be notified when the object awakens.</summary>
        protected virtual void OnAwake() { }

        /// <summary>Allow derived weapons to be notified when the object has been updated.</summary>
        protected virtual void OnUpdate() { }

        /// <summary>Override this method to run logic when <see cref="Fire()"/> is called,
        /// but before any bullets are fired. If false is returned, no bullets will be fired.</summary>
        protected virtual bool OnShoot()
        {
            // Allow bullets to be fired (default).
            return true;
        }

        /// <summary>Allows derived weapons to be notified when the player stops shooting.</summary>
        protected virtual void OnShootStop() { }

        /// <summary>Allows derived weapons to be notified when the crosshair target changes.</summary>
        protected virtual void OnCrossHairTargetChanged(ICharacterEntity targetEntity) { }

        // ------------------------------------------------- //

        public override void OnDamageTaken(int amount, ICharacterEntity from)
        {
            // Make sure our owner's character takes the damage too !
            owner?.owner?.TakeDamage(amount, from);
        }

        public override void OnDamageDealt(int amount, ICharacterEntity target)
        {
            // Make sure our owner's character is notified that they dealt damage!
            owner?.owner?.OnDamageDealt(amount, target);
        }

        public override void OnTargetKilled(ICharacterEntity target)
        {
            // Make sure our owner's character is notified that they killed a target!
            owner?.owner?.OnTargetKilled(target);
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}