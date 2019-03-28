using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperShooter
{
    
    public class SlowTime : MonoBehaviour
    {
        public PlayerController speed;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                speed.crouchSpeed = speed.crouchSpeed / 4;
                speed.walkSpeed = speed.walkSpeed / 4;
                speed.runSpeed = speed.runSpeed / 4;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                speed.runSpeed = 7.5f;
                speed.walkSpeed = 6;
                speed.crouchSpeed = 4;
            }
        }
    }
}