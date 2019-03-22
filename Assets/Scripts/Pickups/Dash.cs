using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperShooter.PB;

namespace SuperShooter.HR
{
    public class Dash : Interactable
    {
        public int speedMultiplier = 2;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == ("Player"))
            {
                other.gameObject.GetComponent<PlayerController>();
                var PC = other.gameObject.GetComponent<PlayerController>();
                PC.isDoubleSpeed = true;
            }
        }
    }
}
