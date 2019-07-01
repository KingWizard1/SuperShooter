using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace SuperShooter
{
    
    public class Area01_Mansion : GameArea
    {

        [Header("Enemies")]
        public EnemyManager waveSpawner;
        public int numberOfWaves = 3;
        public int enemiesPerWave = 3;


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

        protected override bool OnStart()
        {

            // Do checks
            if (waveSpawner == null) {
                Debug.LogError($"{name} does not have a {nameof(waveSpawner)} assigned to it!");
                return false;
            }
            

            // Subscribe to its events.
            waveSpawner.WaveStarted.AddListener(EnemyWaveStarted);
            waveSpawner.WaveCompleted.AddListener(EnemyWaveCompleted);
            waveSpawner.SequenceCompleted.AddListener(EnemySequenceCompleted);

            return true;

        }

        // ------------------------------------------------- //

        protected override void OnPlay()
        {

            // Set up new set of waves of enemies.
            // We reset the manager in case things need cleaning up.
            // We then pass in the spawn points for the area, and configure enemy properties.
            waveSpawner.Reset();
            waveSpawner.spawnPoints = GetSpawnPoints();
            waveSpawner.numberOfWaves = numberOfWaves;
            waveSpawner.baseEnemiesPerWave = enemiesPerWave * GameMaster.Main.gamePhase;
            waveSpawner.currentHealthMultiplier = GameMaster.Main.gamePhase;
            waveSpawner.currentDamageMultiplier = GameMaster.Main.gamePhase;
            waveSpawner.currentXPRewardMultiplier = GameMaster.Main.gamePhase;

            // Aaaaaand GO!
            waveSpawner.StartNextWave();


        }


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

        protected override void OnUpdate()
        {

            switch (sequence)
            {
                case 0:

                    // Wait till all waves have completed.
                    if (!waveSpawner.isFinished)
                        return;

                    // Area completed.
                    Complete();

                    return;


            }


        }


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}