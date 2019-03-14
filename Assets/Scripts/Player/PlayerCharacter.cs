using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Params")]
        public float playerSpeed = 5f;
        public float jumpHeight = 10f;
        public float rayDistance = 1f;

        [Header("Starting weapons")]
        public Weapon[] weapons;

        [Header("Options")]
        public bool rotateToMainCamera = false;
        public bool weaponRotationThing = false;

        // ------------------------------------------------- //

        private Rigidbody rigid;

        private Vector3 moveDirection;

        private bool isJumping = false;

        private Weapon currentWeapon;

        private Interactable currentInteract;

        // ------------------------------------------------- //

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        // ------------------------------------------------- //

        void Start()
        {

            //currentWeapon = weapons[0].GetComponent<Weapon>();
            //currentWeapon.gameObject.SetActive(true);
            //shootPoint = weapons[0].transform.GetChild(0).GetComponent<Transform>();

            SelectWeapon(1);
        }

        // ------------------------------------------------- //

        void OnTriggerEnter(Collider other)
        {

            // Touched interactable?
            currentInteract = other.GetComponent<Interactable>();
            if (currentInteract)
            {
                Debug.Log("Interact: " + currentInteract.name);
                return;
            }

            // Print what we touched
            Debug.Log("Collided with: " + other.name);

        }

        void OnTriggerExit(Collider other)
        {
            currentInteract = null;

            Debug.Log("Moved away from: " + other.name);

        }

        // ------------------------------------------------- //

        void Update()
        {

            Vector3 camEuler = Camera.main.transform.eulerAngles;

            if (rotateToMainCamera)
            {
                moveDirection = Quaternion.AngleAxis(camEuler.y, Vector3.up) * moveDirection;
            }

            Vector3 force = new Vector3(moveDirection.x, rigid.velocity.y, moveDirection.z);

            if (isJumping && IsGrounded())
            {
                force.y = jumpHeight;
                isJumping = false;
            }

            rigid.velocity = force;

            Quaternion playerRotation = Quaternion.AngleAxis(camEuler.y, Vector3.up);
            transform.rotation = playerRotation;

            if (weaponRotationThing)
            {
                Quaternion weaponRotation = Quaternion.AngleAxis(camEuler.x, Vector3.right);
                currentWeapon.transform.localRotation = weaponRotation;
            }

            //if(moveDirection.magnitude > 0)
            //{
            //    transform.rotation = Quaternion.LookRotation(moveDirection);
            //}

        }

        // ------------------------------------------------- //

        public void Attack()
        {
            currentWeapon.Attack();
        }

        public void Reload()
        {
            currentWeapon.Reload();
        }

        // ------------------------------------------------- //

        public void Interact()
        {
            if (currentInteract)
            {
                currentInteract.Interact();
            }
        }
        // ------------------------------------------------- //

        public void Move(Vector3 direction)
        {
            moveDirection = direction;
            moveDirection *= playerSpeed;
        }

        public void Jump()
        {
            isJumping = true;
        }

        // ------------------------------------------------- //

        bool IsGrounded()
        {
            Ray groundRay = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(groundRay, out hit, rayDistance))
            {
                // Currently... we're always "grounded" if theres -something- underneath us.
                // FIX: Check whether we hit a floor or not, and return true or false as appropriate.
                return true;
            }

            return false;
        }

        // ------------------------------------------------- //

        private void OnDrawGizmos()
        {
            // Test
            Ray groundRay = new Ray(transform.position, Vector3.down);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundRay.origin, groundRay.origin + groundRay.direction * rayDistance);
        }

        // ------------------------------------------------- //

        /// <summary>Switch to weapon x, where 0 == the first weapon on the player character.</summary>
        public void SelectWeapon(int slot)
        {

            if (slot < 0 || slot > weapons.Length)
            {
                Debug.LogWarning("There is no weapon stored in slot \"" + slot + "\"");
                return;
            }

            // Deactivate all weapons
            DisableAllWeapons();

            // Switch to and enable new selection
            currentWeapon = weapons[slot];
            currentWeapon.gameObject.SetActive(true);

        }

        public void DisableAllWeapons()
        {
            foreach (var weapon in weapons)
            {
                weapon.gameObject.SetActive(false);
            }
        }

        // ------------------------------------------------- //

    }

}