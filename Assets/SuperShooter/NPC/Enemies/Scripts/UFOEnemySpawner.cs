using UnityEngine;

namespace SuperShooter
{

    public class UFOEnemySpawner : MonoBehaviour
    {

        [Header("Setup")]
        public Transform spawnPoint;
        public GameObject[] enemyPrefabs;

        [Header("Configuration")]
        public int enemiesToSpawn = 3;
        public float timeBetweenSpawns = 1;

        // ------------------------------------------------- //

        private int spawned = 0;
        private float spawnTimer = 0;

        // ------------------------------------------------- //

        private void Start()
        {

            spawnTimer = timeBetweenSpawns;

        }

        // ------------------------------------------------- //

        private void Update()
        {

            // Spawn an enemy at each time interval until the
            // desired number of enemies have been spawned.

            if (spawnTimer > 0)
                spawnTimer -= Time.deltaTime;
            else
            {
                if (spawned < enemiesToSpawn)
                {
                    SpawnEnemy();
                    spawned++;
                    spawnTimer = timeBetweenSpawns;
                }
            }


        }

        // ------------------------------------------------- //



        // ------------------------------------------------- //


        public void SpawnEnemy()
        {
            
            // Resource check
            if (enemyPrefabs.Length == 0) {
                Debug.LogError($"Cannot spawn enemy: no prefabs have been assigned to object '{name}'");
                return;
            }


            // Pick random prefab
            var i = Random.Range(0, enemyPrefabs.Length);

            // Instantiate
            Instantiate(enemyPrefabs[i], spawnPoint.position, spawnPoint.rotation, null);



        }


        // ------------------------------------------------- //



        // ------------------------------------------------- //


    }

}