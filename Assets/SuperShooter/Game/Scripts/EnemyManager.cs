using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace SuperShooter
{

    public class EnemyManager : MonoBehaviour
    {

        [Header("Status")]
        public int currentWave;
        [ShowNativeProperty]     public bool isFinished { get; private set; }   // True if all waves have completed.
        [ShowNonSerializedField] public int numberOfWaves = 1;
        [ShowNonSerializedField] public int baseEnemiesPerWave = 1;

        [ShowNonSerializedField] private bool isRunning;                        // True if the spawner has started its first wave. False when all waves are complete.
        [ShowNativeProperty]     public bool isActive { get; private set; }     // True if there is a wave currently active. False if not running, or in between waves.
        [ShowNonSerializedField] private int curEnemiesPerWave = 1;
        [ShowNonSerializedField] private int enemiesSpawnedThisWave;
        [ShowNativeProperty]     public int enemiesRemaining { get; private set; }
        [ShowNonSerializedField] private int enemiesKilled;

        /// <summary>True if <see cref="enemiesRemaining"/> equals zero.</summary>
        private bool IsWaveComplete => enemiesRemaining == 0;


        [Header("Enemy Config")]
        [ReorderableList] public SpawnPoint[] spawnPoints;
        [ReorderableList] public GameObject[] enemyTypes;
        
        public float timeBetweenWaves = 10f;
        public float timeBetweenSpawns = 2f;

        private float nextWaveTimer = 0;
        private float nextSpawnTimer = 0;

        [Header("Enemy Properties")]
        /// <summary>Applies a multiplier to each spawned enemy's <see cref="CharacterEntity.maxHealth"/>.</summary>
        public float currentHealthMultiplier = 1;
        /// <summary>Applies a multiplier to each spawned enemy's <see cref="EnemyCharacter.XPValue"/>.</summary>
        public float currentXPRewardMultiplier = 1;
        /// <summary>Applies a multiplier to each spawned enemy's damage output value.</summary>
        public float currentDamageMultiplier = 1;

        // ------------------------------------------------- //

        [Header("Events")]
        public UnityEvent WaveStarted;
        public UnityEvent WaveCompleted;
        public UnityEvent SequenceCompleted;

        // ------------------------------------------------- //

        private Transform currentWaveParent;

        private List<EnemyCharacter> enemiesInScene = new List<EnemyCharacter>();

        // ------------------------------------------------- //

        void Start()
        {
           
        }


        // ------------------------------------------------- //
        
        void Update()
        {

            // Do nothing if we're not running yet, or if we've finished.
            if (!isRunning || isFinished)
                return;


            if (isActive) {

                if (enemiesSpawnedThisWave != curEnemiesPerWave)
                    _SpawnNextEnemy();

                else if (IsWaveComplete)
                    _FinishTheWave();

                else { }
                    // Do nothing

            }
            else if (IsWaveComplete) {

                // We are no longer active, and the last wave has completed.

                // Check the float timer. Start the next wave if the
                // amount of time to wait between waves has elapsed.
                nextWaveTimer -= Time.deltaTime;
                if (nextWaveTimer <= 0)
                    StartNextWave();

            }
            else {

                // There are active enemies!
                // Do nothing.

            }

        }
        
        // ------------------------------------------------- //

        public void StartNextWave()
        {

            // Check that we have prefabs to spawn
            if (enemyTypes == null || enemyTypes.Length < 1) {
                Debug.LogError($"{name} does not have any {nameof(enemyTypes)} to spawn!");
                return;
            }

            // Check that we have spawn points to use for this wave
            if (spawnPoints == null || spawnPoints.Length < 1) {
                Debug.LogError($"{name} does not have any {nameof(spawnPoints)} assigned to it!");
                return;
            }


            // Its the next wave!
            currentWave++;


            // Flag that we've begun our atttacckkkk!!!
            isRunning = true;
            isActive = true;

            // Create game object to house all our enemies under for this wave.
            // All waves will be child objects of this manager.
            currentWaveParent = new GameObject($"Wave{currentWave}").transform;
            currentWaveParent.SetParent(transform);

            // Important!
            nextSpawnTimer = timeBetweenSpawns;

            enemiesSpawnedThisWave = 0;

            curEnemiesPerWave = baseEnemiesPerWave + (currentWave > 1 ? currentWave : 0);

            enemiesRemaining = curEnemiesPerWave;

            nextWaveTimer = timeBetweenWaves;

            // Let our listeners know.
            WaveStarted.Invoke();


            // Finished.
            // Enemies will spawn in Update().

        }
        // ------------------------------------------------- //

        private void _SpawnNextEnemy()
        {


            nextSpawnTimer -= Time.deltaTime;

            if (nextSpawnTimer > 0)
                return;


            Debug.Log($"Spawning next enemy!");


            // Get the number of spawn points.
            var numSpawnPoints = spawnPoints.Length;

            // Use a while(condition) { } loop to check whether a previously spawned object
            // is in the same position as the spawn point. Inside the loop, keep picking random
            // spawn points until we find one that isn't occupied by an object.

            //var successfulSpawn = false;

            //while (!successfulSpawn)
            //{


            // Pick a random spawn point
            var randoSpawnNum = Random.Range(0, numSpawnPoints);
            var randoSpawn = spawnPoints[randoSpawnNum];

            // TODO
            // Check that nothing is obstructing the spawn area.
            // If yes, choose another spawn point until we have one that isn't obstructed.


            // Spawn enemy at spawn point.
            var entity = Instantiate(enemyTypes[0], randoSpawn.position, randoSpawn.rotation, currentWaveParent);



            // Get the entity's info and store it
            var enemy = entity.GetComponent<EnemyCharacter>();
            enemy.CharacterDied += OnEnemyDied;
            enemiesInScene.Add(enemy);


            // Set the entities health and XP reward multiplier for this wave.
            enemy.XPValue = Mathf.RoundToInt(enemy.XPValue * currentXPRewardMultiplier);
            enemy.maxHealth = Mathf.RoundToInt(enemy.maxHealth * currentHealthMultiplier);
            enemy.ResetHealth();


            // Important
            nextSpawnTimer = timeBetweenSpawns;

            enemiesSpawnedThisWave++;

            //}

        }

        // ------------------------------------------------- //

        private void _FinishTheWave()
        {
            // Flag that we're no longer attacking.
            isActive = false;

            Debug.Log($"Wave {currentWave} has completed.");
            Debug.Log($"{enemiesInScene.Count} corpses will now be destroyed.");

            // Destroy the current wave of enemy objects, and the wave parent.
            foreach (var enemy in enemiesInScene)
                enemy.DestroyWithAnimation();

            // Clear the list of enemy objects
            enemiesInScene.Clear();

            // Invoke wave completed event.
            WaveCompleted?.Invoke();

            // Are we finished?
            isFinished = currentWave == numberOfWaves;

            if (!isFinished)
            {

                // Start a float timer to wait number of seconds between waves.
                // It will decrement in Update().
                nextWaveTimer = timeBetweenWaves;

            }
            else
            {

                isRunning = false;

                Debug.Log($"Enemy spawn sequence complete! No more waves!");

                // Invoke enemy spawn sequence completed.
                SequenceCompleted?.Invoke();

            }


        }


        // ------------------------------------------------- //
        
        private void OnEnemyDied(ICharacterEntity entity)
        {

            // Update counts

            enemiesKilled++;

            //enemiesRemaining = enemiesInScene.Count(e => !e.isDead);
            enemiesRemaining -= 1;

        }
            


        // ------------------------------------------------- //

        public void KillAllEnemies()
        {
            // Kill all enemy characters.
            for (int i = 0; i < enemiesInScene.Count; i++)
            {
                enemiesInScene[i].Kill();
            }

        }

        public void DestroyAllEnemies()
        {
            // Destroy all enemy objects.
            for (int i = 0; i < enemiesInScene.Count; i++)
            {
                Destroy(enemiesInScene[i].gameObject);
            }
        }

        // ------------------------------------------------- //


        public void Reset()
        {

            // TODO
            // Reset this EnemyManager to a state where it can be re-used
            // for a new set of waves of enemies.





        }



        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}