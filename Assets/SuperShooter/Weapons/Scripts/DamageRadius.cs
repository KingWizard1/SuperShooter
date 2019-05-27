using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperShooter
{
    public class DamageRadius : MonoBehaviour
    {
        public Throwable damage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                damage.damage = 40;
                Debug.Log("I Hit Player");
            }
        }
    }
}