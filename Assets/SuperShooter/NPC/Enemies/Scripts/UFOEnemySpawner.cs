using UnityEngine;

namespace SuperShooter
{

    public class UFOEnemySpawner : MonoBehaviour
    {

        [Header("Setup")]
        public GameObject[] spawnPoints;
        public GameObject[] spawnPoints2;
        
        public GameObject Parent;
        public GameObject Parent2;
        public GameObject Parent3;
        public GameObject Parent4;
        public GameObject Parent5;
        public GameObject Parent6;
        public GameObject Parent7;
        public GameObject Parent8;
        public GameObject Parent9;
        public GameObject Parent10;
        public GameObject Parent11;
        public GameObject Parent12;
        public GameObject Parent13;
        public GameObject[] enemyPrefabs;


        [Header("Configuration")]
        public int enemiesToSpawn = 3;
        public int Wave2EnemiesToSpawn = 7;
        public int Wave3EnemiesToSpawn = 11;
        public int Wave4EnemiesToSpawn = 14;
        public int Wave5EnemiesToSpawn = 22;
        public int Wave6EnemiesToSpawn = 32;
        public int Wave7EnemiesToSpawn = 44;
        public int Wave8EnemiesToSpawn = 60;
        public float timeBetweenSpawns = 1;
        public bool Wave2 = false;
        public bool Wave3 = false;
        private bool Wave4 = false;
        private bool Wave5 = false;
        private bool Wave6 = false;
        private bool Wave7 = false;
        private bool Wave8 = false;
        public bool round1;
        public bool round2;
        public bool round3;
        public bool round4;
        public bool round5;
        public bool round6;
        public bool round7;
        public bool round8;
        public bool round9;
        public bool round10;


        
        private GameObject currentPoint;
        private int index;
        public int spawnAmount = 5;
        public float spawnRate = 5.0f;

        public int index2;
        

        public int currentWave;
        public bool nextWave;

        // ------------------------------------------------- //

        private int spawned = 0;
        private float spawnTimer = 0;

        // ------------------------------------------------- //

        private void Start()
        {

            spawnTimer = timeBetweenSpawns;
            


        }

        // ------------------------------------------------- //

        private void LateUpdate()
        {

            changeRound();

            if (currentWave == 1 && Parent.transform.childCount <= 0 && round1 == true)
            {
                
                Debug.Log("round 1 over");
                currentWave = 2;
               
                
            }
            if(currentWave == 2 && Parent2.transform.childCount <= 0 && round2 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 3;
                nextWave = false;
             
            }
            if(currentWave == 3 && Parent3.transform.childCount <= 0 && round3 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 4;
                nextWave = false;
              
            }
            if(currentWave == 4 && Parent4.transform.childCount <= 0 && round4 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 5;
                nextWave = false;
            
            }


    
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


                //WAVE 1
                if (currentWave == 1 )
                {
                    // nextWave = true;
                    Wave2 = false;

                    if (spawned < enemiesToSpawn)
                    {
                        SpawnEnemy();
                        spawned++;
                        spawnTimer = timeBetweenSpawns;
                        Debug.Log("wave2");
                    }

                }
                //WAVE 2
                if (currentWave == 2 )
                {
                    // nextWave = true;
                    Wave2 = true;

                    if (spawned < Wave2EnemiesToSpawn)
                    {
                        SpawnEnemy();
                        spawned++;
                        spawnTimer = timeBetweenSpawns;
                        Debug.Log("wave2");
                    }

                }
                //WAVE 3
                if (currentWave == 3 )
                {
                    //  nextWave = true;
                    Wave3 = true;

                    if (spawned < Wave3EnemiesToSpawn)
                    {
                        SpawnEnemy();
                        spawned++;
                        spawnTimer = timeBetweenSpawns;
                        Debug.Log("wave3");
                    }

                }

                //WAVE 4
                if (currentWave == 4   )
                {
                    // nextWave = true;
                    Wave4 = true;

                    if (spawned < Wave4EnemiesToSpawn)
                    {
                        SpawnEnemy();
                        spawned++;
                        spawnTimer = timeBetweenSpawns;
                        Debug.Log("wave4");
                    }

                }
                //WAVE 5         
                if (Parent.activeSelf == false )
                {
                   // nextWave = true;
                   Wave5 = true;

                    if (spawned < Wave5EnemiesToSpawn)
                    {
                        SpawnEnemy();
                        spawned++;
                        spawnTimer = timeBetweenSpawns;
                        Debug.Log("wave5");
                    }

                }

                //WAVE 6
                if (Parent5.activeSelf == false )//&& Wave6 == false)
                {
                   // nextWave = true;
                    Wave6 = true;

                    if (spawned < Wave6EnemiesToSpawn)
                    {
                        SpawnEnemy();
                        spawned++;
                        spawnTimer = timeBetweenSpawns;
                        Debug.Log("wave6");
                    }

                }

                //WAVE 7
                if (Parent6.activeSelf == false)
                {
                    // nextWave = true;
                    Wave7 = true;

                    if (spawned < Wave7EnemiesToSpawn)
                    {
                        SpawnEnemy();
                        spawned++;
                        spawnTimer = timeBetweenSpawns;
                        Debug.Log("wave7");
                    }

                }

            }



        }

        // ------------------------------------------------- //


        public void SpawnEnemy()
        {

            currentPoint = spawnPoints[index];
            spawnPoints = GameObject.FindGameObjectsWithTag("MansionSpawn");

            index = Random.Range(0, spawnPoints.Length);
            index2 = Random.Range(0, spawnPoints2.Length);
            


            // Resource check
            if (enemyPrefabs.Length == 0)
            {
                Debug.LogError($"Cannot spawn enemy: no prefabs have been assigned to object '{name}'");
                return;
            }


            // Pick random prefab
            var i = Random.Range(0, enemyPrefabs.Length);

            // Instantiate
            if ( Wave2 == false && currentWave == 1)
            {
             //   currentPoint = spawnPoints[index];
               // spawnPoints = GameObject.FindGameObjectsWithTag("MansionSpawn");
               // index = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent.transform);
                //nextWave = true;
                round1 = true;
              

            }


           if ( Wave2 == true && currentWave == 2)
           {

               // currentWave++;
              //  currentPoint = spawnPoints[index];
                //spawnPoints = GameObject.FindGameObjectsWithTag("MansionSpawn");
              //  index = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent2.transform);
                // nextWave = true;
                round2 = true;
               
            }
           if ( Wave3 == true && currentWave == 3)
           {

                //currentWave++;
               // currentPoint = spawnPoints[index];
              //  spawnPoints = GameObject.FindGameObjectsWithTag("MansionSpawn");
               // index = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent3.transform) ;
                // nextWave = true;
                round3 = true;
               
            }
           if ( Wave4 == true)
           {

               // currentWave++;
             //   currentPoint = spawnPoints[index];
               // spawnPoints = GameObject.FindGameObjectsWithTag("MansionSpawn");
              //  index = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent4.transform) ;
                // nextWave = true;
                round4 = true;
              

            }
           if ( Wave5 == true)
           {

               // currentWave++;
             //   currentPoint = spawnPoints[index];
              //  spawnPoints = GameObject.FindGameObjectsWithTag("MansionSpawn");
              //  index = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent5.transform) ;
                round5 = true;
            }
           if ( Wave6 == true)
           {

               // currentWave++;
              //  currentPoint = spawnPoints2[index];
                spawnPoints2 = GameObject.FindGameObjectsWithTag("CourtyardSpawn");
               // index2 = Random.Range(0, spawnPoints2.Length);
                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent6.transform) ;
                round6 = true;
            }
           if ( Wave7 == true)
           {

               // currentWave++;
             //   currentPoint = spawnPoints2[index];
              //  spawnPoints2 = GameObject.FindGameObjectsWithTag("CourtyardSpawn");
              //  index2 = Random.Range(0, spawnPoints2.Length);
                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent7.transform) ;
           
            }
           if ( Wave8 == true)
           {

               // currentWave++;
               // currentPoint = spawnPoints2[index];
               // spawnPoints2 = GameObject.FindGameObjectsWithTag("CourtyardSpawn");
               // index2 = Random.Range(0, spawnPoints2.Length);
                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent8.transform) ;
           
            }
            



        }


        // ------------------------------------------------- //


        public void changeRound()
        {
            if (nextWave == true)
            {
                nextWave = false;
                currentWave++;
            }
               
            
        }
        // ------------------------------------------------- //


    }

}