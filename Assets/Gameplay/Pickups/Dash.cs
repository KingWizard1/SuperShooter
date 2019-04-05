using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public class Dash : MonoBehaviour
    {
        public int speedMultiplier = 2;

        // ------------------------------------------------- //

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == ("Player"))
            {
                other.gameObject.GetComponent<FPSController>();
                var PC = other.gameObject.GetComponent<FPSController>();
                PC.isDoubleSpeed = true;

                this.gameObject.SetActive(false);
            }
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

        public void Pickup()
        {
            throw new System.NotImplementedException();
        }

        public void Drop()
        {
            throw new System.NotImplementedException();
        }

        public string GetDisplayName()
        {
            return "Super Speed";
        }

        // ------------------------------------------------- //

    }
}
