using UnityEngine;

namespace SuperShooter
{

    public class UFOEnemySpawner : MonoBehaviour
    {

        [Header("Setup")]
        public Transform spawnPoint;
        public GameObject Parent;
        public GameObject Parent2;
        public GameObject[] enemyPrefabs;

        [Header("Configuration")]
        public int enemiesToSpawn = 3;
        public int Wave2EnemiesToSpawn = 7;
        public float timeBetweenSpawns = 1;
        public bool Wave2 = false;

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


            if (Parent == null && Wave2 == false)
            {
                Wave2 = true;

                    if (spawned < Wave2EnemiesToSpawn)
                    {
                        SpawnEnemy();
                        spawned++;
                    spawnTimer = timeBetweenSpawns;
                    Debug.Log("wave2");
                    }
                
            }

    
        }

        // ------------------------------------------------- //



        // ------------------------------------------------- //


        public void SpawnEnemy()
        {
            
            // Resource check
            if (enemyPrefabs.Length == 0)
            {
                Debug.LogError($"Cannot spawn enemy: no prefabs have been assigned to object '{name}'");
                return;
            }


            // Pick random prefab
            var i = Random.Range(0, enemyPrefabs.Length);

            // Instantiate
            if ( Wave2 == false)
            {
                Instantiate(enemyPrefabs[i], spawnPoint.position, spawnPoint.rotation, Parent.transform);
            }
           if ( Wave2 == true)
           {
               Instantiate(enemyPrefabs[i], spawnPoint.position, spawnPoint.rotation, Parent2.transform);
            }
            



        }


        // ------------------------------------------------- //



        // ------------------------------------------------- //


    }

}