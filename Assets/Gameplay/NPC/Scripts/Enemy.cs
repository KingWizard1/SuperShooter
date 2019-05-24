using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class Enemy : MonoBehaviour
    {

        public int health = 100;
        public int damage = 25;
        public int expValue;
        public bool isDead;

        private void Update()
        {

            // Am I still alive?
            if (health <= 0)
            {
                isDead = true;
                XPGive();
            }

        }
        // ------------------------------------------------- //
        public void DealDamage(int amount)
        {
            // Deal DMG
            health -= amount;

            // Dead
            if (health <= 0)
                isDead = true;
        }
        public void XPGive()
        {
            if (isDead == true)
            {
                print("Yes");
            }
        }
        // ------------------------------------------------- //
    }
}