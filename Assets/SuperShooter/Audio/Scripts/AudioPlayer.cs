using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SuperShooter
{

    public class AudioPlayer : MonoBehaviour
    {

        [Header("Audio")]
        public AudioMixerGroup output;

        [Header("Container")]
        [Tooltip("When enabled, this audio player will attempt to find all AudioSources" +
            "in or below this object, and move its game object to a single container object.")]
        public bool moveAudioSourcesToContainer = true;

        [Header("Binding (Optional)")]
        public Transform attachTo;

        // ------------------------------------------------- //

        private GameObject clipContainer;

        // ------------------------------------------------- //

        void Start()
        {

            // Create an empty game object on which to play all our clips on.
            // This is just to keep the heirarchy neat and tidy I guess.
            var container = new GameObject();
            container.transform.SetParent(transform);


            // Check if there are any pre-defined AudioSources on this transform.
            if (moveAudioSourcesToContainer) {
                var existingSources = GetComponentsInChildren<AudioSource>();
                foreach (var item in existingSources)
                    item.gameObject.transform.SetParent(clipContainer.transform);
            }
            

        }


        // ------------------------------------------------- //

        void Update()
        {
            if (attachTo)
                transform.SetPositionAndRotation(attachTo.position, attachTo.rotation);

        }


        // ------------------------------------------------- //

        public void PlaySound(AudioClip clip, bool loop = false)
        {

            var source = gameObject.AddComponent<AudioSource>();

            source.clip = clip;
            source.loop = loop;


        }


        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //

    }

}