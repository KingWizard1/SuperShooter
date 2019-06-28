using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GrandfatherClock : MonoBehaviour
{

    public AudioSource chime;

    public AudioSource tickTock;

    private void Update()
    {
        // Wait till the chime finishes
        if (chime.isPlaying)
            return;

        // Now tick tock
        tickTock.loop = true;
        tickTock.Play();
        enabled = false;


    }


}
