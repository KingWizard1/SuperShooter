using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SuperShooter
{

    public class GameAudio : MonoBehaviour
    {
        [Header("Output Mixer")]
        public AudioMixer mixer;

        [Header("Transform to Follow (Optional)")]
        public Transform target;


        // ------------------------------------------------- //

        private AudioListener listener;

        // ------------------------------------------------- //

        void Start()
        {

            // Check for player and audio listener in the scene
            if (target == null) {
                target = GameObject.FindGameObjectWithTag("Player")?.transform;
            }
            if (target != null) {
                listener = target.GetComponentInChildren<AudioListener>();
                listener = FindObjectOfType<AudioListener>();
            }
            else
                Debug.LogError($"[Audio] No Player is assigned to {nameof(GameAudio)}!");


            // Create AudioChannel objects
            CreateChannels();
        }

        // ------------------------------------------------- //

        public void CreateChannels()
        {
            if (mixer == null) {
                Debug.LogError($"[Audio] No Audio Mixer is assigned! There will be audio issues during this session.");
                return;
            }

            // Get all mixer groups that are sub-groups of Master.
            // Unity's AudioMixer -always- has "Master" as the top-level group.
            AudioMixerGroup[] groups = mixer.FindMatchingGroups("Master");

            // Creates channel objects as a children of this transform.
            // These channels will automatically output to the corresponding AudioMixer groups.
            foreach (var group in groups)
            {
                var newObject = new GameObject();
                newObject.name = group.name;
                newObject.transform.SetParent(transform);

                var channel = newObject.AddComponent<AudioChannel>();
                channel.output = group;
            }
            

        }

        // ------------------------------------------------- //

        void Update()
        {

            // Follow the player.
            if (listener)
                transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);

        }


        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //

    }

}