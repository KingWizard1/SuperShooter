using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SuperShooter
{

    public class AudioChannel : MonoBehaviour
    {

        public AudioMixerGroup output;

        // ------------------------------------------------- //

        public List<AudioPlayer> objects;

        // ------------------------------------------------- //


        void Start()
        {
            
        }


        // ------------------------------------------------- //

        void Update()
        {

        }


        // ------------------------------------------------- //

        public void Play(AudioClip clip, Transform attachTo)
        {

            // Create audio player object.
            var newObject = new GameObject();
            newObject.transform.SetParent(transform);
            var player = newObject.AddComponent<AudioPlayer>();
            player.attachTo = attachTo;
            objects.Add(player);

        }

        public void PlayWithTransform(AudioSource s, Transform t, bool destroyWithTransform = false)
        {

            

        }

        public void PlayOneShot(AudioClip clip)
        {

            // Create
            var source = gameObject.AddComponent<AudioSource>();

            // Configure
            source.clip = clip;
            source.loop = false;
            source.Play();

            // Auto destroy
            Destroy(source, clip.length);


        }

        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //

    }

}