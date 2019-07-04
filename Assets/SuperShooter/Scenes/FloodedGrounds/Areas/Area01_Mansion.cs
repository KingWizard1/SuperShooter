using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace SuperShooter
{
    
    public class Area01_Mansion : GameArea
    {

        [Header("Area Timer")]
        public int timeLimitInSeconds = 150;        // 150 = 2min 30sec

        [Header("Enemies")]
        public EnemyManager waveSpawner;
        public int numberOfWaves = 3;
        public int enemiesPerWave = 3;

        [Header("OnCompletion")]
        [ReorderableList] public InteractiveDoubleDoor[] doorsToOpen;


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
            UIManager.Main?.areaUI?.ShowObjectiveText("Eliminate the invaders.", true);

        }


        // ------------------------------------------------- //

        private void EnemyWaveStarted()
        {

            // An enemy wave has begun.
            if (waveSpawner.currentWave > 1)
                UIManager.Main?.areaUI?.ShowObjectiveText($"Wave {waveSpawner.currentWave} incoming!");

        }

        private void EnemyWaveCompleted()
        {

            // All enemies in current wave have died.
            UIManager.Main?.areaUI?.ShowObjectiveText("Time To Rest...");


            // Earn XP
            var xp = 50 * GameMaster.Main?.gamePhase ?? 0;
            GameManager.Main?.PlayerCharacter.GiveXP(xp);
            UIManager.Main?.progressionUI?.ShowWaveCompleted(xp);



        }

        private void EnemySequenceCompleted()
        {

            // All waves have been defeated.
            UIManager.Main?.areaUI?.ShowObjectiveText("Area cleared!", true);


            // Earn XP
            var xp = 100 * GameMaster.Main?.gamePhase ?? 0;
            GameManager.Main?.PlayerCharacter.GiveXP(xp);
            UIManager.Main?.progressionUI?.ShowAreaCleared(xp);


            clock.Stop();

        }

        // ------------------------------------------------- //

        protected override void OnUpdate()
        {
            // Countdown timer
            clock.Update();
            if (clock.currentTime <= 0)
                GameMaster.Main?.GameOver("GAME OVER.", "The invasion could not be stopped.");


            switch (sequence)
            {
                case 0:

                    // Wait till all waves have completed.
                    if (!waveSpawner.isFinished)
                        return;


                    // Open the mansion doors
                    foreach (var door in doorsToOpen)
                        door.OpenDoor();


                    // Area completed.
                    Complete();

                    return;


            }


        }

        // ------------------------------------------------- //

        protected override void OnUpdateUI()
        {

            // Time remaining
            if (clock.isRunning)
                UIManager.Main?.areaUI?.SetAreaTimerText(clock.currentTimeString);
            else
                UIManager.Main?.areaUI?.SetAreaTimerText(string.Empty);


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