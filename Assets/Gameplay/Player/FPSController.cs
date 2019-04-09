using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : NetworkPlayerBehaviour, IPlayer, IKillable
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
        public bool isInvincible;
        public bool isDoubleSpeed;

        [Header("References")]
        public Camera attachedCamera;
        public Transform aimTarget;
        public Transform playerHand;

        [Header("Weapons/Abilities")]
        public int maxWeapons = 2;
        //public int maxThrowables = 2;

        // ------------------------------------------------- //

        #region Privates

        // References
        private Animator anim;
        public static CharacterController controller;
        public FPSCameraLook cameraLook { get; private set; }
        private FPSPhysics physics;
        private Timeline timeline;

        // Movement
        private Vector3 movement;   // Current movement vector
        private int jumps = 0;      // Current number of jumps executed (resets when grounded)
        private int maxJumps = 2;   // Max number of times player can jump

        // Physics
        private Vector3 impact = Vector3.zero;

        // Status
        private bool isDead = false;
        private bool isOnLadder = false;

        // Inventory
        private Ability currentAbility;            // Current ability.
        private Throwable currentThrowable;        // Current throwable.

        private Weapon currentWeapon;       // Current weapon. Public for testing, make private later.
        private List<Weapon> weapons = new List<Weapon>();  // Weapons on hand.
        private int currentWeaponIndex = 0; // Current weapon index.

        // Camera
        private Vector3 defaultPlayerHandPosition = Vector3.zero;

        #endregion

        // ------------------------------------------------- //

        #region Accessors

        /// <summary>The characters current movement speed.</summary>
        public float MoveSpeed { get; private set; }

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
            // Max health
            startHealth = health;


            defaultPlayerHandPosition = playerHand.localPosition;
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

#if DEBUG
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
                    Respawn();
#endif

                return;
            }

            UpdateMovement();
            UpdateInteract();

            UpdateAim();
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

            // Testing, rough specific weapon placement system.
            if (currentWeapon)
            {
                currentWeapon.gameObject.transform.localPosition = currentWeapon.playerHandOffset;
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

            // Inputs
            if (Input.GetMouseButton(0))
                currentWeapon.Shoot();

            if (Input.GetMouseButtonUp(0))
                currentWeapon.StopShooting();

            if (Input.GetKeyDown(KeyCode.R))
                currentWeapon.Reload();

            // Update UI
            UIManager.Main.SetWeaponStatus(currentWeapon);


            var ammoInClip = currentWeapon.ammo;
            var maxAmmoPerClip = currentWeapon.maxAmmoPerClip;
            var totalAmmoLeft = (currentWeapon.maxAmmoPerClip * currentWeapon.clips);
            var weaponName = currentWeapon.GetDisplayName();

            //Debug.Log(string.Format("{0} {1}/{2} ({3})",
            //    weaponName, ammoInClip, maxAmmoPerClip, totalAmmoLeft));
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
            MoveSpeed = walkSpeed;
            if (Input.GetKey(KeyCode.LeftShift)) MoveSpeed = runSpeed;
            if (Input.GetKey(KeyCode.LeftControl)) MoveSpeed = crouchSpeed;

            if (isDoubleSpeed)
                MoveSpeed *= 2;

            // Apply movement to X and Z.
            movement.x = input.x * MoveSpeed;
            movement.z = input.z * MoveSpeed;
        }

        #endregion

        // ------------------------------------------------- //

        #region Combat


        private void UpdateAim()
        {
            // Do we have a weapon?
            if (currentWeapon == null)
                return;

            // On MouseRightDown else Up
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                // Player hand is now up to the their "eye"
                playerHand.localPosition = 
                    new Vector3(0, playerHand.localPosition.y + .05f, playerHand.localPosition.z);

                // Camera FOV
                cameraLook.ZoomTo(currentWeapon.zoomLevels[0], currentWeapon.timeToADS);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                // Return to hip fire position
                playerHand.localPosition =
                    //new Vector3(0.5f, playerHand.localPosition.y - .05f, playerHand.localPosition.z);
                    defaultPlayerHandPosition;

                // Reset camera FOV
                cameraLook.ZoomToDefault(currentWeapon.timeToUnADS);
            }


        }


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

            // Is the item an activatable Ability?
            if (item is Ability && currentAbility == null)
            {
                currentAbility = item as Ability;
            }

            // Is the item a Throwable?
            if (item is Throwable && currentThrowable == null)
            {
                currentThrowable = item as Throwable;
                AttachItemToPlayerHand(item);
            }

            // Is the item a Weapon?
            if (item is Weapon && (weapons.Count <= maxWeapons))
            {
                // Add to weapon list
                weapons.Add(item as Weapon);

                SelectWeapon(weapons.Count - 1);

                //DetachAllWeapons();
                AttachItemToPlayerHand(item);

            }

            // Tell the weapon to change its behavior, its being picked up.
            item.Pickup();

        }

        private void DetachAllWeapons()
        {
            // TODO
        }

        private void AttachItemToPlayerHand(IInteractable item)
        {
            // Attach to player hand, and zero its local pos/rot.
            var itemTransform = ((MonoBehaviour)item).transform;
            itemTransform.SetParent(playerHand);
            itemTransform.localPosition = Vector3.zero;
            itemTransform.localRotation = Quaternion.identity;

            // Each weapon is held differently
            if (item is Weapon) {
                var w = item as Weapon;
                //itemTransform.localPosition = w.playerHandOffset;
                //w.aimTarget = aimTarget;

                var lat = w.gameObject.AddComponent<FPSLookAtTarget>();
                lat.target = aimTarget;
            }
            
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
            // Bail if no throwable can be thrown
            if (currentThrowable == null)
                return;

            // Hold Q to Throw/Cook the item.
            if (Input.GetKey(KeyCode.Q))
                currentThrowable.StartThrow();

            // Release Q to let it go/throw it/release.
            if (Input.GetKeyUp(KeyCode.Q))
            {
                currentThrowable.StopThrowing();
                currentThrowable = null;
            }

            
        }

        // ------------------------------------------------- //

        void UpdateAbilities()
        {
            // Bail if no ability can be used
            if (currentAbility == null)
                return;

            // Consume the ability while Q is being held down.
            // And stop consuming it on the frame Q is released.
            if (Input.GetKey(KeyCode.Q))
                currentAbility.Use();
            if (Input.GetKeyUp(KeyCode.Q))
                currentAbility.StopUse();

            // Has the ability been used up? Destroy it.
            if (currentAbility.IsDepleted)
                Destroy(currentAbility.gameObject);

            // Update UI
            UIManager.Main.SetAbility(currentAbility);

            //Debug.Log(ability.GetDisplayName() + " " + ability.TimeRemaining + " ( " + ability.IsActive + ", " + ability.IsDepleted + ")");

        }

        #endregion

        // ------------------------------------------------- //

        #region IKillable

        public void TakeDamage(int damage)
        {
            // Do nothing if we're amazing right now
            if (isInvincible)
                return;

            // Deplete heatlh by amount
            health -= damage;

            // Update UI
            UIManager.Main.SetHealth(health, false);

            // Did we die?
            if (health <= 0)
                Kill();

        }

        public void Kill()
        {

            // Die
            isDead = true;

            // Disable the character controller.
            // Turns off all character events, including collisions.
            controller.enabled = false;

            // Update UI
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