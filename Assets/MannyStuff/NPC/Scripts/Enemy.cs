using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int health = 100;
    public int damage = 25;

    // ----------------------------------------------------- //

    public void DealDamage(int amount)
    {
        // Deal DMG
        health -= amount;

        // Dead
        if (health <= 0)
            Destroy(gameObject);
    }


    // ----------------------------------------------------- //

}
