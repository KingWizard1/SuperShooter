using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gernade : MonoBehaviour
{
    
    
    public float grenadetime = 6;
    public GameObject timeSphere;
    public Transform gernadePR;


   
    private void Update()
    {
        grenadetime -= Time.deltaTime;
    }
    void FixedUpdate()
    {

        
        if (grenadetime <= 0)
        {
           
            Instantiate(timeSphere, gernadePR.position, gernadePR.rotation);
            Destroy(gameObject);
        }
    }

}
