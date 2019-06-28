using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using NaughtyAttributes;

public class TVController : MonoBehaviour
{

    public float timeToStatic = 30f;

    public VideoClip staticClip;

    public float staticVolume = 0.1f;

    public VideoPlayer player;

    public AudioSource audioSource;

    [ShowNonSerializedField]
    private float timer = 0;

    private void Update()
    {

        timer += Time.deltaTime;

        if (timer >= timeToStatic)
        {
            player.clip = staticClip;
            player.isLooping = true;
            audioSource.volume = staticVolume;

            this.enabled = false;
        }

        
    }


}
