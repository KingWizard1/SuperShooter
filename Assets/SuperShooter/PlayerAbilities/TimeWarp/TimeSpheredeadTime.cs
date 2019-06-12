using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpheredeadTime : MonoBehaviour
{

    public float timeSphereDie = 10;
    private void Update()
    {
        timeSphereDie -= Time.deltaTime;

        if(timeSphereDie <= 0)
        {
            Destroy(gameObject);
        }
    }
}
