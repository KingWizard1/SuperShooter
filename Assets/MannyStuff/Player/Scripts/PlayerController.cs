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

    // ----------------------------------------------------- //

    private Animator anim;
    private CharacterController controller;

    /// <summary>Current movement vector.</summary>
    private Vector3 movement;
    /// <summary>Current movement speed.</summary>
    private float moveSpeed;

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
        // NOTE: Add speed mechanic here (crouch/walk/run). Just need if statements.
        moveSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) moveSpeed = runSpeed;
        if (Input.GetKey(KeyCode.LeftControl)) moveSpeed = crouchSpeed;

        if (isDoubleSpeed)
        {
            moveSpeed *= 2;
        }

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
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        Move(inputH, inputV);


        // Is the controller grounded?
        Ray groundRay = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(groundRay, out hit, groundRayDistance)) {
            
            // We're grounded.

            // If jump is pressed
            if (Input.GetButtonDown("Jump"))
            {
                // Move controller up (Y)
                movement.y = jumpHeight;
            }

        }
        else {

            // NOT grounded.
            // Apply gravity. We then select the max value to prevent gravity from
            // growing infinitely while the player is grounded. If we let this happen,
            // then when the player walks off a ledge, their Y vector will be so strong
            // that they'll fall almost immediately into void space... and die.
            movement.y -= gravity * Time.deltaTime;
            movement.y = Mathf.Max(movement.y, -gravity);

        }


        // Move the controller
        controller.Move(movement * Time.deltaTime);  // Returns CollisionFlags
    }

    /// <summary>Handles interaction with items in the world.</summary>
    void Interact()
    {

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

    // ----------------------------------------------------- //


    // ----------------------------------------------------- //

}
