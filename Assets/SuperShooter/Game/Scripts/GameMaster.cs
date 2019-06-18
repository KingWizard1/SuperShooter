using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SuperShooter
{

    public class GameMaster : MonoBehaviour
    {

        [Header("DEBUGState")]
        public int gamePhase = 1;       // Number of times the player has completed a full cycle of areas.
        public int currentArea = 0;     // 0 = Tutorial. 1 = Area 1. 2 = Area 2, etc.

        [Header("List of Areas")]
        public GameObject[] playAreas;      // Area parent objects. Child objects are expected to have spawn points.
        public GameObject[] tutorialArea;   // Tutorial parent objects.

        [Header("Enemy Config")]
        public EnemyManager enemies;


        // ------------------------------------------------- //

        // Singleton
        public static GameMaster Main { get; private set; }

        public static bool Exists => Main != null;


        // ------------------------------------------------- //

        private void Awake()
        {
            Main = this;
        }

        // ------------------------------------------------- //

        private void Start()
        {

            // Do checks
            if (enemies == null)
                Debug.LogError($"The {nameof(GameMaster)} does not have an {nameof(EnemyManager)}!");
            else
            {

                // Subscribe to its events.
                enemies.WaveStarted.AddListener(EnemyWaveStarted);
                enemies.WaveCompleted.AddListener(EnemyWaveCompleted);
                enemies.SequenceCompleted.AddListener(EnemySequenceCompleted);

            }



        }

        // ------------------------------------------------- //

        public void StartTutorial()
        {

        }

        // ------------------------------------------------- //

        public void StartNextArea()
        {


            // Is current area the last area?
            if (currentArea == playAreas.Length)
            {
                // All areas have been completed.
                gamePhase++;

                Debug.Log($"---------- PHASE {gamePhase} BEGIN ----------");

                // TODO
                // Provide option to leave the game world? WIN Condition A.

                // Reset stuff?


                return;
            }


            // Next area!
            currentArea++;

            Debug.Log($"----------  AREA {currentArea} START ----------");

            
            // Get the new area's collection of spawn points.
            // This is for the enemy manager to know where to spawn things in.
            var spawnPoints = playAreas[currentArea].GetComponentsInChildren<SpawnPoint>();

            // Set up new set of waves of enemies.
            // We reset the manager in case things need cleaning up.
            // We then pass in the spawn points for the area, and configure enemy properties.
            enemies.Reset();
            enemies.spawnPoints = spawnPoints.Select(sp => sp.transform).ToArray();
            enemies.currentHealthMultiplier = gamePhase;
            enemies.currentDamageMultiplier = gamePhase;
            enemies.currentXPRewardMultiplier = gamePhase;

            // Aaaaaand GO!
            enemies.StartNextWave();


        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


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