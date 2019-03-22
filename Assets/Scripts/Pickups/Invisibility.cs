using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : MonoBehaviour
{
    public float invisTime = 5.0f;

    private MeshRenderer playerRend;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            playerRend = other.gameObject.GetComponent<MeshRenderer>();
        }
    }

    private void Update()
    {

        if (playerRend)
        {
            playerRend.enabled = false;

            invisTime -= Time.deltaTime;

            if (invisTime <= 0)
            {
                playerRend.enabled = true;
                print("workin");

                playerRend = null;
            }

        }
    }
}
