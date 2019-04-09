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
        public static int startHealth = 100;
        public static int health;
        public float runSpeed = 10f;
        public float walkSpeed = 6f;
        public float gravity = 10f;
        public float crouchSpeed = 4f;
        public float jumpHeight = 20f;
        public float interactRange = 10f;
        public float groundRayDistance = 1.1f;

        [Header("Powerups")]
        public bool isDoubleSpeed;
        
        [Header("References")]
        public Camera attachedCamera;
        public Transform playerHand;

        [Header("Weapons/Abilities")]
        public int maxWeapons = 2;
        public int maxThrowables = 2;

        // ------------------------------------------------- //

        #region Privates

        // References
        private Animator anim;
        public static CharacterController controller;
        private FPSCameraLook cameraLook;
        private FPSPhysics physics;
        private Timeline timeline;

        // Movement
        private Vector3 movement;   // Current movement vector
        private float moveSpeed;    // Current movemend speed
        private int jumps = 0;      // Current number of jumps executed (resets when grounded)
        private int maxJumps = 2;   // Max number of times player can jump

        // Physics
        private Vector3 impact = Vector3.zero;

        // Status
        private bool isDead = false;
        private bool isOnLadder = false;

        // Inventory
        private Ability ability;            // Current ability.
        private Throwable throwable;        // Current throwable.

        private Weapon currentWeapon;       // Current weapon. Public for testing, make private later.
        private List<Weapon> weapons = new List<Weapon>();  // Weapons on hand.
        private int currentWeaponIndex = 0; // Current weapon index.


        #endregion

        // ------------------------------------------------- //

        #region Accessors

        /// <summary>The characters current movement speed.</summary>
        public float MoveSpeed { get { return moveSpeed; } }

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
            cameraLook = GetComponentInChildren<FPSCameraLook>();
            physics = GetComponent<FPSPhysics>();
            timeline = GetComponent<Timeline>();


            TryRegisterWeapons();
        }

        // ------------------------------------------------- //

        private void Start()
        {
            startHealth = health;
            // Nothing yet.
        }

        // ------------------------------------------------- //

        void TryRegisterWeapons()
        {
            // For use when the player is starting out with a weapon already in hand.
            weapons = new List<Weapon>(GetComponentsInChildren<Weapon>());

            if (weapons.Count > 0)
            {
                // Configure the weapon so it knows it should be in its picked up state.
                weapons[0].Pickup();

                // 'Select' this weapon. Disables all weapons except the specified one,
                // as well as ensures our indexes are correct and ready for weapon switching.
                SelectWeapon(0);
            }
            

        }

        #endregion

        // ------------------------------------------------- //

        private void Update()
        {
            // Do nothing if dead.
            if (isDead)
            {

#if DEBUG == true
                if (Input.GetKeyDown(KeyCode.R))
                    Respawn();
#endif

                return;
            }

            UpdateMovement();
            UpdateInteract();

            UpdateAbilities();
            UpdateThrowables();
            UpdateWeaponShooting();
            UpdateWeaponSwitching();

            // DEBUG
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                var nextSpawn = SpawnPoints.Main.GetNextPlayerSpawnPoint();
                //transform.SetPositionAndRotation(nextSpawn.position, nextSpawn.rotation);
                transform.position = nextSpawn.position;
                //transform.Rotate(nextSpawn.eulerAngles);
                cameraLook.SetRotation(nextSpawn.eulerAngles.y, 0f);   // 0 on the Y, to force looking straight ahead
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(10);
            }

        }

        // ------------------------------------------------- //

        #region Update Actions


        /// <summary>Handles player movement.</summary>
        void UpdateMovement()
        {

            // Get input from user and set the movement vector
            if (!isOnLadder)
            {
                // Not on ladder.
                float inputH = Input.GetAxis("Horizontal");
                float inputV = Input.GetAxis("Vertical");
                Move(inputH, inputV);
            }
            else
            {
                isOnLadder = true;

            }

            if (isOnLadder)
            {
                
                
                if ((Input.GetKey("w")) || (Input.GetKey("s")) || (Input.GetButtonDown("Jump")))
                    {
                    isOnLadder = true;
                    // We're on a ladder.
                    float inputV = Input.GetAxis("Ladder");
                    if (Input.GetKey("w"))
                    {
                        movement.y = walkSpeed;
                        movement.x = 0;
                        movement.z = 0;



                    }



                    if (Input.GetKey("s"))
                    {
                        movement.y = -walkSpeed;
                        movement.x = 0;
                        movement.z = 0;
                    }

                    if (Input.GetButtonDown("Jump"))
                    {
                        isOnLadder = false;
                   }


                        Move(0, 0);
                }
                else
                {
                    return;
                }
                
        
                
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
        void UpdateInteract()
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
                var interactableName = interactable.GetDisplayName();
                var interactablePosition = ((MonoBehaviour)interactable).transform.position;
                UIManager.Main.ShowPickupPrompt3D(interactableName, interactablePosition);

                // Pickup the interactable if key is being pressed on this frame
                if (Input.GetKeyDown(KeyCode.E))
                    Pickup(interactable);
            }

        }

        /// <summary>Handles current weapon fire mechanics.</summary>
        void UpdateWeaponShooting()
        {
            // Is a current weapon selected?
            if (!currentWeapon)
                return;

            if (Input.GetButton("Fire1"))
                currentWeapon.Shoot();
            if (Input.GetButtonUp("Fire1"))
                currentWeapon.StopShooting();

            // Update UI
            UIManager.Main.SetWeaponStatus(currentWeapon);
        }

        /// <summary>Handles cycling/switching through available weapons.</summary>
        void UpdateWeaponSwitching()
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
                isOnLadder = true;
            }
            else
            {
                isOnLadder = false;
            }

        }

        // ------------------------------------------------- //

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
        /// <param name="item"></param>
        void Pickup(IInteractable item)
        {

            if (item is Ability && ability == null)
            {
                ability = item as Ability;
            }

            if (item is Throwable && throwable == null)
            {
                throwable = item as Throwable;
            }

            if (item is Weapon && (weapons.Count <= maxWeapons))
            {
                // Add to weapon list
                weapons.Add(item as Weapon);

                SelectWeapon(weapons.Count - 1);

                DetachAllWeapons();
                AttachInteractableToPlayerHand(item);

            }

            // Tell the weapon to change its behavior, its being picked up.
            item.Pickup();

        }

        private void DetachAllWeapons()
        {
            // TODO
        }

        private void AttachInteractableToPlayerHand(IInteractable item)
        {
            // Attach to player hand, and zero its local pos/rot.
            var itemTransform = ((MonoBehaviour)item).transform;
            itemTransform.SetParent(playerHand);
            itemTransform.localPosition = Vector3.zero;
            itemTransform.localRotation = Quaternion.identity;
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

            // Update UI
            if (UIManager.Main != null)
                UIManager.Main.SetWeaponStatus(currentWeapon);
        }

        // ------------------------------------------------- //

        /// <summary>
        /// throw grende add force to grenade
        /// </summary>
        void UpdateThrowables()
        {

            // TODO

            //if (throwable == null)
            //    return;

            //// Hold Q to Throw/Cook the item.
            //if (Input.GetKey(KeyCode.Q))
            //    throwable.StartThrow();

            //// Release Q to let it go/throw it/release.
            //if (Input.GetKeyUp(KeyCode.Q))
            //    throwable.StopThrowing();
        }

        // ------------------------------------------------- //

        void UpdateAbilities()
        {

            if (ability == null)
                return;

            // Consume the ability while Q is being held down.
            // And stop consuming it on the frame Q is released.
            if (Input.GetKey(KeyCode.Q))
                ability.Use();
            if (Input.GetKeyUp(KeyCode.Q))
                ability.StopUse();

            // Update UI
            UIManager.Main.SetAbility(ability);

        }

        #endregion

        // ------------------------------------------------- //

        #region IKillable

        public void TakeDamage(int damage)
        {
            health -= damage;

            UIManager.Main.SetHealth(health, false);


            if (health <= 0)
                Kill();

        }

        public void Kill()
        {

            isDead = true;

            controller.enabled = false;

            UIManager.Main.SetHealth(health, true);
            UIManager.Main.ShowDeathScreen(true);

        }

        #endregion

        // ------------------------------------------------- //

        private void Respawn()
        {
            isDead = false;
            health = 100;
            controller.enabled = true;
            UIManager.Main.ShowDeathScreen(false);
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}