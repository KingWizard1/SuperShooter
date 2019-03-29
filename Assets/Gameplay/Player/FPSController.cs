using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    [RequireComponent(typeof(CharacterController))]
    public class FPSController : NetworkPlayerBehaviour, IKillable
    {

        [Header("Mechanics")]
        public int health = 100;
        public float runSpeed = 10f;
        public float walkSpeed = 6f;
        public float gravity = 10f;
        public float crouchSpeed = 4f;
        public float jumpHeight = 20f;
        public float interactRange = 10f;
        public float groundRayDistance = 1.1f;

        public bool isDoubleSpeed;
        
        [Header("References")]
        public Camera attachedCamera;
        public Transform playerHand;

        [Header("Weapons/Abilities")]
        public int maxWeapons = 2;
        public GameObject startingWeapon;

        // ------------------------------------------------- //

        #region Privates

        private Animator anim;
        private CharacterController controller;
        private Timeline timeline;

        private Vector3 movement;   // Current movement vector
        private float moveSpeed;    // Current movemend speed

        private int jumps = 0;      // Current number of jumps executed (resets when grounded)
        private int maxJumps = 2;   // Max number of times player can jump

        private bool onLadder = false;

        private Weapon currentWeapon; // Public for testing, make private later.
        private List<Weapon> weapons = new List<Weapon>();
        private int currentWeaponIndex = 0;

        #endregion

        // ------------------------------------------------- //

        // Always draw.
        private void OnDrawGizmos()
        {

        }

        // Selected means to only draw gizmos when this object is selected.
        private void OnDrawGizmosSelected()
        {
            // Interact ray
            Ray interactRay = attachedCamera.ViewportPointToRay(new Vector2(.5f, .5f));
            Gizmos.color = Color.blue;
            DrawRay(interactRay, interactRange);

            // Show ground ray
            Ray groundRay = new Ray(transform.position, -transform.up);
            Gizmos.color = Color.red;
            DrawRay(groundRay, groundRayDistance);

        }

        void DrawRay(Ray ray, float distance)
        {
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * distance);
        }

        // ------------------------------------------------- //

        #region Initialisation

        private void Awake()
        {
            anim = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            timeline = GetComponent<Timeline>();

            //RegisterWeapons();
        }

        // ------------------------------------------------- //

        private void Start()
        {
            SelectWeapon(0);
        }

        // ------------------------------------------------- //

        //void RegisterWeapons()
        //{
        //    weapons = new List<Weapon>(GetComponentsInChildren<Weapon>());

        //    // FIX: Collection gets modified in Pickup()
        //    foreach (var weapon in weapons)
        //    {
        //        Pickup(weapon);
        //    }
        //}

        #endregion

        // ------------------------------------------------- //

        private void Update()
        {
            Movement();
            Interact();
            Shooting();
            Switching();
        }

        // ------------------------------------------------- //

        #region Update Actions

        /// <summary>Handles player movement.</summary>
        void Movement()
        {

            // Get input from user and set the movement vector
            if (!onLadder)
            {
                // Not on ladder.
                float inputH = Input.GetAxis("Horizontal");
                float inputV = Input.GetAxis("Vertical");
                Move(inputH, inputV);
            }
            else
            {
                // We're on a ladder.
                float inputV = Input.GetAxis("Ladder");
                if (Input.GetKey("w"))
                {
                    movement.y = walkSpeed;
                    movement.x = 0;
                    movement.z = 0;
                }
                else
                {
                    return;
                }

                Move(0, inputV);
            }


            // Is the controller grounded?
            Ray groundRay = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            bool isGrounded = Physics.Raycast(groundRay, out hit, groundRayDistance);

            bool isJumpPressed = Input.GetButtonDown("Jump");
            bool canJump = jumps < maxJumps; // jumps = int, maxJumps = int

            if (isGrounded)
            {

                if (isJumpPressed)
                {
                    jumps = 1;

                    // Move controller up (Y)
                    movement.y = jumpHeight;
                }

            }
            else
            {

                if (isJumpPressed && canJump)
                {
                    movement.y += jumpHeight;
                    jumps++;
                }

                // NOT grounded.
                // Apply gravity. We then select the max value to prevent gravity from
                // growing infinitely while the player is grounded. If we let this happen,
                // then when the player walks off a ledge, their Y vector will be so strong
                // that they'll fall almost immediately into void space... and die.
                //movement.y -= gravity * Time.deltaTime;
                movement.y = Mathf.Max(movement.y, -gravity);
                movement.y -= gravity * timeline.deltaTime;

            }


            // Move the controller
            controller.Move(movement * timeline.deltaTime);  // Returns CollisionFlags
        }

        /// <summary>Handles interaction with items in the world.</summary>
        void Interact()
        {
            // Disable interact UI
            UIManager.Main.HideAllPrompts();

            // Create ray from center of screen.
            // In viewport dimensions, 0 == top left corner, 1 == bottom right corner.
            // Thus, 0.5 on the X and Y == dead center of the screen.
            Ray interactRay = attachedCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            RaycastHit hit;

            // Shoot ray in a range
            if (Physics.Raycast(interactRay, out hit, interactRange))
            {
                // Try getting Interactable object
                var interactable = hit.collider.GetComponent<IInteractable>();

                // Bail if there's nothing to interact with
                if (interactable == null)
                    return;

                // Enable the UI and show the interactable name
                var interactableName = interactable.GetTitle();
                UIManager.Main.ShowPickupPrompt(interactableName);


                if (interactable is Weapon)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                        Pickup(interactable as Weapon);

                }

            }

        }

        /// <summary>Handles current weapon fire mechanics.</summary>
        void Shooting()
        {
            // Is a current weapon selected?
            if (!currentWeapon)
                return;

            if (Input.GetButton("Fire1"))
            {
                currentWeapon.Shoot();
            }
        }

        /// <summary>Handles cycling/switching through available weapons.</summary>
        void Switching()
        {
            // If there is more than one weapon
            if (weapons.Count > 1)
            {
                // Get scroll wheel direction.
                float inputScroll = Input.GetAxis("Mouse ScrollWheel");

                if (inputScroll != 0)
                {
                    int direction = inputScroll > 0
                        ? Mathf.CeilToInt(inputScroll)
                        : Mathf.FloorToInt(inputScroll);

                    // Switch weapons up or down.
                    SwitchWeapon(direction);
                }

            }
        }

        #endregion

        // ------------------------------------------------- //

        private void FixedUpdate()
        {

        }

        // ------------------------------------------------- //

        #region Triggers / Colliders

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.name == "LDRBottom")
            {
                onLadder = true;
            }
            else
            {
                onLadder = false;
            }

        }

        // ------------------------------------------------- //

        /// <summary>Called when the controller hits a collider while performing a move.</summary>
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // This method allows a CharacterController to push Rigidbodies!!

            // Get the Rigidbody of the object we collided with.
            var other = hit.gameObject.GetComponent<Rigidbody>();

            // Bail if the object does not have a Rigidbody, or has one but isKinematic.
            if (other == null || other.isKinematic)
                return;

            // We don't want to push objects below us
            if (hit.moveDirection.y < -0.3)
                return;

            // Calculate push direction from move direction.
            // We only push objects to the sides, never up and down (???)
            var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // Apply!
            other.velocity = pushDir * moveSpeed;


        }

        #endregion

        // ------------------------------------------------- //

        #region Controls

        /// <summary>Move the player by an amount.</summary>
        /// <param name="inputH"></param>
        /// <param name="inputV"></param>
        void Move(float inputH, float inputV)
        {

            // Create direction from input
            Vector3 input = new Vector3(inputH, 0, inputV);

            // Localise direction to player transform
            input = transform.TransformDirection(input);

            // Set move speed
            moveSpeed = walkSpeed;
            if (Input.GetKey(KeyCode.LeftShift)) moveSpeed = runSpeed;
            if (Input.GetKey(KeyCode.LeftControl)) moveSpeed = crouchSpeed;

            if (isDoubleSpeed)
                moveSpeed *= 2;

            // Apply movement to X and Z.
            movement.x = input.x * moveSpeed;
            movement.z = input.z * moveSpeed;
        }

        #endregion

        // ------------------------------------------------- //

        #region Combat

        /// <summary>Switch between weapons with given direction.</summary>
        /// <param name="direction">Pass -1 to switch to previous, and +1 to switch to next.</param>
        void SwitchWeapon(int direction)
        {
            // Add direction to the offset.
            currentWeaponIndex += direction;

            // Loop back to end if index is below zero
            if (currentWeaponIndex < 0)
                currentWeaponIndex = weapons.Count - 1;

            // Reset to zero if index is exceeds length
            if (currentWeaponIndex >= weapons.Count)
                currentWeaponIndex = 0;

            // Select weapon
            SelectWeapon(currentWeaponIndex);
        }

        /// <summary>Disables GameObjects of every attached weapon.</summary>
        void DisableAllWeapons()
        {
            foreach (var weapon in weapons)
            {
                weapon.gameObject.SetActive(false);
            }
        }

        /// <summary>Add weapon to <see cref="weapons"/> list and attaches it to player's hand.</summary>
        /// <param name="weaponToPickup"></param>
        void Pickup(Weapon weaponToPickup)
        {
            // Tell the weapon to change its behavior, its being picked up
            weaponToPickup.Pickup();

            var weaponTransform = weaponToPickup.transform;

            // Attach to player hand, and zero its local pos/rot.
            weaponTransform.SetParent(playerHand);
            weaponTransform.localPosition = Vector3.zero;
            weaponTransform.localRotation = Quaternion.identity;

            // Add to weapon list
            weapons.Add(weaponToPickup);
            SelectWeapon(weapons.Count - 1);
        }

        /// <summary>Removes weapon from <see cref="weapons"/> list and drops it from the player's hand.
        void Drop(Weapon weaponToDrop)
        {
            // Tell the weapon to change its behavior, its no longer under player control.
            weaponToDrop.Drop();

            // Get the transform
            var weaponTransform = weaponToDrop.transform;
            weaponTransform.SetParent(null);    // Detach from player hand.

            // Remove weapon from list
            weapons.Remove(weaponToDrop);

        }

        /// <summary>Sets <see cref="currentWeapon"/> to weapon at given index.</summary>
        /// <param name="index"></param>
        void SelectWeapon(int index)
        {
            // Bound check
            if (index < 0 || index >= weapons.Count)
                return;

            //
            DisableAllWeapons();

            // Select weapon
            currentWeapon = weapons[index];

            // Enable weapon
            currentWeapon.gameObject.SetActive(true);

            // Update current index
            currentWeaponIndex = index;
        }

        #endregion

        // ------------------------------------------------- //

        #region IKillable

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
                Kill();
        }

        public void Kill()
        {
            Debug.LogWarning("PLAYER DIED!");
        }

        #endregion

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}