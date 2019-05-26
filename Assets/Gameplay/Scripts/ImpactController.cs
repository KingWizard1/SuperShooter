using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class ImpactController : MonoBehaviour
    {

        public float force = 5;

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.rigidbody != null)
            {
                // Handle rigid body physics.

                // Not implemented/required right now.

                return;

            }
            else
            {

                var direction = collision.gameObject.transform.position - transform.position;

                var characterPhysics = collision.gameObject.GetComponent<FPSPhysics>();

                if (characterPhysics == null)
                    return;

                characterPhysics.TakeImpact(direction, force);
            }
        }


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //



    }

}