using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SuperShooter
{

    public class EnemyManager : MonoBehaviour
    {

        [Header("Status")]
        public bool isActive;
        public bool isFinished;
        public int currentWave;
        public int numberOfWaves;
        public int enemiesRemaining;
        public int enemiesKilled;

        private bool IsWaveComplete => enemiesRemaining == 0;


        [Header("Config")]
        public Transform[] spawnPoints;
        public GameObject[] enemyTypes;
        public float timeBetweenWaves = 20f;
        private float nextWaveTimer = 0;

        public int baseDamageModifier;  // Future

        // ------------------------------------------------- //

        [Header("Events")]
        public UnityEvent WaveStarted;
        public UnityEvent WaveCompleted;
        public UnityEvent SequenceCompleted;

        // ------------------------------------------------- //

        private List<GameObject> WaveParents;

        private List<EnemyCharacter> enemiesInScene;

        // ------------------------------------------------- //

        void Start()
        {

        }

        // ------------------------------------------------- //

        public void BeginNextWave()
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
            isActive = true;

            // Let our listeners know.
            WaveStarted.Invoke();

            // Create game object to house all our enemies under for this wave.
            // All waves will be child objects of this manager.
            var waveParent = new GameObject($"Wave{currentWave}");
            waveParent.transform.SetParent(transform);


            // TODO
            // Get the number of spawn points.


            // TODO
            // Use Random.RandomRange() to pick a spawn point.


            // TODO
            // Use a while(condition) { } loop to check whether a previously spawned object.
            // is in the same position as the spawn point. Inside the loop, keep picking random
            // spawn points until we find one that isn't occupied by an object.


            // TODO
            // Spawn enemy at spawn point.


            // TODO
            // Set its transform's parent to our own object.


            // Get the entity's info and store it
            var entity = GetComponent<EnemyCharacter>();
            entity.CharacterDied.AddListener(OnEnemyDied);
            enemiesInScene.Add(entity);

            // Finished.

        }

        // ------------------------------------------------- //
        
        private void OnEnemyDied(ICharacterEntity entity)
        {

            enemiesKilled++;

            enemiesRemaining = enemiesInScene.Count(e => !e.isDead);

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


        public void ResetEverything()
        {

            // TODO
            // Reset this EnemyManager to a state where it can be re-used
            // for a new set of waves of enemies.



        }



        // ------------------------------------------------- //


        void Update()
        {

            // Do nothing if we're finished.
            if (isFinished)
                return;


            // Condition: Active wave completed!
            if (isActive && IsWaveComplete)
            {
                // Flag that we're no longer attacking.
                isActive = false;

                // TODO:
                // Destroy the current wave of enemy objects, and the wave parent.



                // Clear the list of enemy objects
                enemiesInScene.Clear();

                // Invoke wave completed event.
                WaveCompleted.Invoke();

                // Are we finished?
                isFinished = currentWave == numberOfWaves;
                if (isFinished)
                {

                    // Invoke enemy spawn sequence completed.
                    SequenceCompleted.Invoke();



                }

                // TODO:
                // Start a float timer to wait number of seconds between waves.




            }
            else if (IsWaveComplete)    // Condition: WaveIsComplete, 
            {

                // Check the float timer.
                // If the amount of time to wait between waves has elapsed.
                if (nextWaveTimer >= timeBetweenWaves)
                {
                    BeginNextWave();
                }

            }
            else
            {

                // There are active enemies!


            }

            




        }

        // ------------------------------------------------- //



        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}