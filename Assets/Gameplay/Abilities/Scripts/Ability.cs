using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    //[RequireComponent(typeof(Rigidbody))]
    //[RequireComponent(typeof(BoxCollider))]
    //[RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(SphereCollider))]
    public class Ability : MonoBehaviour, IInteractable
    {
        [SerializeField]
        public string baseName = "New Ability";

        [Header("Numbers")]
        public float MaxDuration = 5;

        // ------------------------------------------------- //

        // Mechanics
        public float TimeRemaining { get; private set; }

        /// <summary>True if the ability currently being used by the player.</summary>
        public bool IsActive { get; private set; }

        /// <summary>True if the ability is finished and can no longer be consumed.</summary>
        public bool IsDepleted { get; private set; }


        // Components
        //protected Rigidbody rigid;
        //private BoxCollider boxCollider;
        //private LineRenderer lineRenderer;
        private SphereCollider pickupCollider;

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

            // Pickup glow and spin
            pickupSpin.speed = 100f;
            pickupSpin.axis = Vector3.up;
            pickupGlow.SetActive(true);

        }

        private void GetComponentReferences()
        {
            //rigid = GetComponent<Rigidbody>();
            //boxCollider = GetComponent<BoxCollider>();
            //lineRenderer = GetComponent<LineRenderer>();
            pickupCollider = GetComponent<SphereCollider>();

            pickupSpin = GetComponent<Spin>();
            pickupGlow = transform.Find("PickupGlow").gameObject;
        }

        // ------------------------------------------------- //

        private void Start()
        {
            TimeRemaining = MaxDuration;
        }

        // ------------------------------------------------- //

        private void Update()
        {

            // Allow derived class to be notified that we have updated.
            OnUpdate();
        }

        // ------------------------------------------------- //

        public void Pickup()
        {
            // Disable physics (set to true)
            //rigid.isKinematic = true;

            // Disable trigger collider so we don't trigger the UI when we look at the weapon in our hand
            pickupCollider.enabled = false;

            // Disable glow and spin
            pickupSpin.enabled = false;
            pickupGlow.SetActive(false);

            // Allow derived class to be notified that its been picked up.
            OnPickup();
        }

        public void Drop()
        {
            // Enable physics (set to false)
            //rigid.isKinematic = false;

            // Allow trigger collider for pickup
            pickupCollider.enabled = true;
        }

        // ------------------------------------------------- //

        public virtual void Reload()
        {
            
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        protected virtual void OnAwake()
        {
            // Allow derived class to be notified when the object awakens.
        }

        protected virtual void OnUpdate()
        {
            // Allow derived class to be notified when the object has been updated.
        }

        // ------------------------------------------------- //

        protected virtual void OnUse()
        {
            // Allows derived class to be notified when the ability is being used.

        }

        protected virtual void OnStop()
        {
            // Allows derived class to be notified when the ability should stop.

        }

        protected virtual void OnPickup()
        {
            // Allows derived class to be notified when the player picks it up.

        }

        // ------------------------------------------------- //

        /// <summary>Consumes this ability, or elements of it over time.</summary>
        public void Use()
        {

            // Disable use if none left
            if (IsDepleted)
                return;

            // Use the ability over time
            TimeRemaining -= Time.deltaTime;
            OnUse();

            // If we've used it all, set flag.
            if (TimeRemaining <= MaxDuration)
                IsDepleted = true;
        }

        public void StopUse()
        {

            OnStop();

        }

        // ------------------------------------------------- //

        public virtual string GetDisplayName()
        {
            return (!string.IsNullOrEmpty(baseName)) ? baseName : "Unnamed Ability";
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}