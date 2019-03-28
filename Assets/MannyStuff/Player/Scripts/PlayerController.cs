using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    [Header("Mechanics")]
    public int health = 100;
    public float runSpeed = 7.5f;
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
    public Transform Player;
    public GameObject player;

    //[Header("UI")]
    //public GameObject interactUI;
    //public Transform interactUIParent;

    // ----------------------------------------------------- //

    private Animator anim;
    private CharacterController controller;
    private Vector3 movement;   // Current movement vector

    private int jumps = 0;
    private int maxJumps = 2;

    private bool onLadder = false;

    public Weapon currentWeapon; // Public for testing, make private later.
    private List<Weapon> weapons = new List<Weapon>();
    private int currentWeaponIndex = 0;

    // ----------------------------------------------------- //

    private void OnDrawGizmos()
    {
        Ray groundRay = new Ray(transform.position, -transform.up);
        Gizmos.DrawLine(groundRay.origin, groundRay.origin + groundRay.direction * groundRayDistance);
    }

    #region Initialisation

    private void Awake()
    {

        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

    }

    // ----------------------------------------------------- //

    private void Start()
    {
        SelectWeapon(0);
    }

    // ----------------------------------------------------- //

    void CreateUI()
    {

    }

    // ----------------------------------------------------- //

    void RegisterWeapons()
    {

        weapons = new List<Weapon>(GetComponentsInChildren<Weapon>());

    }

    #endregion

    // ----------------------------------------------------- //

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
        float moveSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) moveSpeed = runSpeed;
        if (Input.GetKey(KeyCode.LeftControl)) moveSpeed = crouchSpeed;

        if (isDoubleSpeed)
            moveSpeed *= 2;
        
        // Apply movement to X and Z.
        movement.x = input.x * moveSpeed;
        movement.z = input.z * moveSpeed;
    }

    #endregion

    // ----------------------------------------------------- //

    #region Combat

    /// <summary>Switch between weapons with given direction.</summary>
    /// <param name="direction">Pass -1 to switch to previous, and +1 to switch to next.</param>
    void SwitchWeapon(int direction)
    {

    }

    /// <summary>Disables GameObjects of every attached weapon.</summary>
    void DisableAllWeapons()
    {

    }

    /// <summary>Add weapon to <see cref="weapons"/> list and attaches it to player's hand.</summary>
    /// <param name="weaponToPickup"></param>
    void Pickup(Weapon weaponToPickup)
    {

    }

    /// <summary>Removes weapon from <see cref="weapons"/> list and drops it from the player's hand.
    void Drop(Weapon weaponToDrop)
    {

    }

    /// <summary>Sets <see cref="currentWeapon"/> to weapon at given index.</summary>
    /// <param name="index"></param>
    void SelectWeapon(int index)
    {

    }

    #endregion

    // ----------------------------------------------------- //

    #region Actions
    
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
            if (Input.GetKey("W"))
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

        }

        // Apply gravity
        movement.y -= gravity * Time.deltaTime;


        // Move the controller
        controller.Move(movement * Time.deltaTime);  // Returns CollisionFlags
    }

    /// <summary>Handles interaction with items in the world.</summary>
    void Interact()
    {

        //interactUI.SetActive(false);

    }

    /// <summary>Handles current weapon fire mechanics.</summary>
    void Shooting()
    {

    }

    /// <summary>Handles cycling/switching through available weapons.</summary>
    void Switching()
    {

    }

    #endregion

    // ----------------------------------------------------- //


    private void Update()
    {
        Movement();
        Interact();
        Shooting();
        Switching();


    }

    // ----------------------------------------------------- //

    private void FixedUpdate()
    {
        
    }

    // ----------------------------------------------------- //

    // ----------------------------------------------------- //

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

}
