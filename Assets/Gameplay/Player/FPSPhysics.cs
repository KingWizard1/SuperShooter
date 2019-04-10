using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public interface ICharacterControllerPhysics
    {
        void TakeImpact(Vector3 direction, float force);
    }

    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(FPSController))]
  
    public class FPSPhysics : MonoBehaviour, ICharacterControllerPhysics
    {

        #region Privates

        // References
        private FPSController controller;
        private CharacterController character;


        // Values
        private float mass = 3.0f;
        private Vector3 impact = Vector3.zero;

        #endregion

        // ------------------------------------------------- //

        #region Initialisation

        private void Awake()
        {

            controller = GetComponent<FPSController>();
            character = GetComponent<CharacterController>();
          
        }

        // ------------------------------------------------- //

        private void Start()
        {

        }

        // ------------------------------------------------- //

        #endregion

        // ------------------------------------------------- //

        private void Update()
        {
            // Apply the impact force
            if (impact.magnitude > 0.2) character.Move(impact * Time.deltaTime);

            // Consume the impact energy on each cycle;
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        }

        // ------------------------------------------------- //

        #region Collider Physics

        /// <summary>Called when the controller hits a collider while performing a move.</summary>
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {

            // Handle non-physical collisions. Returns false if there weren't any.
            // We want to quit out if the collision was handled i.e. NOT run the default code.
            if (HandleControllerCollision(hit))
                return;

            // See if its a rigidbody collision
            if (HandleRigidBodyCollision(hit))
                return;

        }

        private bool HandleControllerCollision(ControllerColliderHit hit)
        {

            if (hit.gameObject.tag == "Vehicle")
            {

                // We're dead.
                controller.Kill();

                // Apply physics to the player, from the vehicles perspective.
                // The GO is assumed to be one that had a collider on it.
                //var other = hit.gameObject;
                //var rotation = other.transform.rotation.eulerAngles;

                //var pushDir = rotation;

                //takeimpact(rotation, 15);

                return true;
            }


            // There was no condition for us to handle.
            return false;
        }

        private bool HandleRigidBodyCollision(ControllerColliderHit hit)
        {
            // This function allows a CharacterController to push Rigidbodies!

            // Get the Rigidbody of the object we collided with.
            var other = hit.gameObject.GetComponent<Rigidbody>();

            // Bail if the object does not have a Rigidbody, or has one but isKinematic.
            if (other == null || other.isKinematic)
                return false;

            // We don't want to push objects below us
            if (hit.moveDirection.y < -0.3)
                return false;

            // Calculate push direction from move direction.
            // We only push objects to the sides, never up and down (???)
            var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // Apply!
            other.velocity = pushDir * controller.MoveSpeed;

            // Success
            return true;
        }

        #endregion

        // ------------------------------------------------- //

        public void TakeImpact(Vector3 direction, float force)
        {
            //direction.Normalize();
            //if (direction.y < 0) direction.y = -direction.y; // Reflect down force on the ground
            //impact += direction.normalized * force / mass;
        }

        private void UpdatePhysics()
        {
            
        }

        // ------------------------------------------------- //

    }

}