using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace SuperShooter
{
    
    public class Area02_Courtyard : GameArea
    {

        [Header("Area Timer")]
        public int timeLimitInSeconds = 180;        // 180 = 3min

        [Header("Enemies")]
        public EnemyManager waveSpawner;
        public int numberOfWaves = 4;
        public int enemiesPerWave = 4;

        //[Header("OnCompletion")]
        //public InteractiveDoubleDoor[] doorsToOpen;


        // ------------------------------------------------- //

        private ClockTimer clock = new ClockTimer();

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

            // Get phase number. We use this for multiplying things.
            var phase = GameMaster.Main.gamePhase;

            // Set up new set of waves of enemies.
            // We reset the manager in case things need cleaning up.
            // We then pass in the spawn points for the area, and configure enemy properties.
            waveSpawner.Reset();
            waveSpawner.spawnPoints = GetSpawnPoints();
            waveSpawner.numberOfWaves = numberOfWaves;
            waveSpawner.baseEnemiesPerWave = enemiesPerWave * phase;
            waveSpawner.currentHealthMultiplier = phase;
            waveSpawner.currentDamageMultiplier = phase;
            waveSpawner.currentXPRewardMultiplier = phase;

            // Aaaaaand GO!
            waveSpawner.StartNextWave();

            // Start timer
            clock.StartCountdown(timeLimitInSeconds * phase);

            // Show objective
            UIManager.Main?.areaUI?.ShowObjectiveText("Defend the courtyard.");

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

            // Countdown timer
            clock.Update();
            if (clock.currentTime <= 0)
                GameMaster.Main?.GameOver("GAME OVER.", "The grounds have been taken by the invaders.");


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

        protected override void OnUpdateUI()
        {

            // Time remaining
            UIManager.Main?.areaUI?.SetAreaTimerText(clock.currentTimeString);

            // Enemies remaining
            if (waveSpawner.isActive)
                UIManager.Main?.areaUI?.SetEnemyCount(waveSpawner.enemiesRemaining);
            else
                UIManager.Main?.areaUI?.SetEnemyCount(0);

        }


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}