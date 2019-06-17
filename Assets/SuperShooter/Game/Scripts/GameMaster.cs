using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SuperShooter
{

    public class GameMaster : MonoBehaviour
    {

        [Header("DEBUGState")]
        public int gamePhase = 1;
        public int currentArea = 0;     // 0 = Tutorial. 1 = Area 1. 2 = Area 2, etc.


        [Header("EnemyConfig")]
        public EnemyManager manager;


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

        void Start()
        {

            // Do checks
            if (manager == null)
                Debug.LogError($"The {nameof(GameMaster)} does not have an {nameof(EnemyManager)}!");
            else
            {

                // Subscribe to its events.
                manager.WaveStarted.AddListener(EnemyWaveStarted);
                manager.WaveCompleted.AddListener(EnemyWaveCompleted);
                manager.SequenceCompleted.AddListener(EnemySequenceCompleted);

            }



        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        private void EnemyWaveStarted()
        {

            // An enemy wave has begun.

        }

        private void EnemyWaveCompleted()
        {

            // All enemies in current wave have died.

        }

        private void EnemySequenceCompleted()
        {

            // All waves have been defeated.

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        void Update()
        {

            // Do stuff

            UpdateUI();

        }

        // ------------------------------------------------- //

        void UpdateUI()
        {
            // Eventually, update UI elements to indicate
            // to the player the current state of the game.
        }


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}