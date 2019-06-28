using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace SuperShooter
{

    public class GameMaster : MonoBehaviour
    {

        [Header("DEBUGState")]
        public int gamePhase;               // Number of times the player has completed a full cycle of areas.
        public int currentArea;

        [Header("List of Areas")]
        [ReorderableList]
        public GameArea[] gameplayAreas;    // Area parent objects. Child objects are expected to have spawn points.


        public MonoBehaviour roundCounter;
        public bool rq;
        public bool textOn;

        [Header("Setup")]
        public GameObject[] spawnPoints;
        public GameObject[] spawnPoints2;
        public GameObject[] spawnPoints3;
        public GameObject[] spawnPoints4;

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
        public GameObject Parent14;
        public GameObject Parent15;
        public GameObject Parent16;
        public GameObject Parent17;
        public GameObject Parent18;
        public GameObject Parent19;
        public GameObject Parent20;
        public GameObject[] enemyPrefabs;
        public GameObject FirstKill;


        [Header("Configuration")]
        public int enemiesToSpawn = 3;
        public int Wave2EnemiesToSpawn = 7;
        public int Wave3EnemiesToSpawn = 11;
        public int Wave4EnemiesToSpawn = 14;
        public int Wave5EnemiesToSpawn = 22;
        public int Wave6EnemiesToSpawn = 32;
        public int Wave7EnemiesToSpawn = 44;
        public int Wave8EnemiesToSpawn = 60;
        public int Wave9EnemiesToSpawn = 60;
        public int Wave10EnemiesToSpawn = 60;
        public int Wave11EnemiesToSpawn = 60;
        public int Wave12EnemiesToSpawn = 60;
        public int Wave13EnemiesToSpawn = 60;
        public int Wave14EnemiesToSpawn = 60;
        public int Wave15EnemiesToSpawn = 60;
        public int Wave16EnemiesToSpawn = 60;
        public int Wave17EnemiesToSpawn = 60;
        public int Wave18EnemiesToSpawn = 60;
        public int Wave19EnemiesToSpawn = 60;
        public int Wave20EnemiesToSpawn = 60;


        public float timeBetweenSpawns = 1;


        private bool Wave2 = false;
        private bool Wave3 = false;
        private bool Wave4 = false;
        private bool Wave5 = false;
        private bool Wave6 = false;
        private bool Wave7 = false;
        private bool Wave8 = false;
        private bool Wave9 = false;
        private bool Wave10 = false;
        private bool Wave11 = false;
        private bool Wave12 = false;
        private bool Wave13 = false;
        private bool Wave14 = false;
        private bool Wave15 = false;
        private bool Wave16 = false;
        private bool Wave17 = false;
        private bool Wave18 = false;
        private bool Wave19 = false;
        private bool Wave20 = false;
        private bool round1;
        private bool round2;
        private bool round3;
        private bool round4;
        private bool round5;
        private bool round6;
        private bool round7;
        private bool round8;
        private bool round9;
        private bool round10;
        private bool round11;
        private bool round12;
        private bool round13;
        private bool round14;
        private bool round15;
        private bool round16;
        private bool round17;
        private bool round18;
        private bool round19;
        private bool round20;



        private GameObject currentPoint;
        private GameObject currentPoint2;
        private GameObject currentPoint3;
        private GameObject currentPoint4;
        private int index;
        public int spawnAmount = 5;
        public float spawnRate = 5.0f;

        public int index2;
        public int index3;
        public int index4;


        public int currentWave;
        public bool nextWave;

        // ------------------------------------------------- //

        public int spawned = 0;
        public float spawnTimer = 0;

        // ------------------------------------------------- //



        public MonoBehaviour rqText;
        public bool rq2;
        public bool textOn2;


        public float tenSec = 10;
        public bool timerRunning = true;
        int i;
        float sec = 10f;


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

            gamePhase = 1;
            currentArea = -1;

            // Make all areas inactive right now.
            //foreach (var area in gameplayAreas)
            //    area.gameObject.SetActive(false);


        }

        // ------------------------------------------------- //

        public void StartNextArea()
        {


            // Is current area the last area?
            if (currentArea == gameplayAreas.Length)
            {

                UpgradeGamePhaseByOne();

                return;
            }


            // Next area!
            currentArea++;

            Debug.Log($"----------  AREA {currentArea} START ----------");

            // Play!
            var nextArea = gameplayAreas[currentArea];
            nextArea.Play();




        }

        // ------------------------------------------------- //

        public void UpgradeGamePhaseByOne()
        {
            // All areas have been completed.
            gamePhase++;

            Debug.Log($"---------- PHASE {gamePhase} BEGIN ----------");

            // TODO
            // Provide option to leave the game world? WIN Condition A.

            // Reset stuff?


        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //
        
        public IEnumerator rageQuit()
        {

            yield return new WaitForSeconds(2);
            if (textOn == true)
            {
                rqText.gameObject.SetActive(true);
            }

            var countDown = 10f;

            if (Input.GetKeyDown(KeyCode.Tab) && textOn == true)
            {


                //Switch to "Death Screen".
                //temporary...
                Application.Quit();
                Debug.Log("RAGE");

            }



            if (rq == false)
            {
                rq = true;
                textOn = true;

                for (int i = 0; i < 10000; i++)
                {
                    while (countDown >= 0)
                    {
                        //rqText.gameObject.SetActive(true);
                       // Debug.Log(i++);
                        countDown -= Time.smoothDeltaTime;

                        yield return null;
                    }




                }

            }
            yield return new WaitForSeconds(sec);
            textOn = false;
            rqText.gameObject.SetActive(false);
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        void Update()
        {

            // Do stuff
            if (FirstKill == null)
            {
                currentWave = 1;
            }
            // Spawn an enemy at each time interval until the
            // desired number of enemies have been spawned.


            if (spawnTimer > 0)
            {

                spawnTimer -= Time.deltaTime;


            }
            else
            {

                if (spawned < enemiesToSpawn || spawned < Wave2EnemiesToSpawn || spawned < Wave3EnemiesToSpawn || spawned < Wave4EnemiesToSpawn || spawned < Wave5EnemiesToSpawn || spawned < Wave6EnemiesToSpawn || spawned < Wave7EnemiesToSpawn || spawned < Wave8EnemiesToSpawn || spawned < Wave9EnemiesToSpawn || spawned < Wave10EnemiesToSpawn || spawned < Wave11EnemiesToSpawn || spawned < Wave12EnemiesToSpawn || spawned < Wave13EnemiesToSpawn || spawned < Wave14EnemiesToSpawn || spawned < Wave15EnemiesToSpawn || spawned < Wave16EnemiesToSpawn || spawned < Wave17EnemiesToSpawn || spawned < Wave18EnemiesToSpawn || spawned < Wave19EnemiesToSpawn || spawned < Wave20EnemiesToSpawn && currentWave != 0)
                {
                    SpawnEnemy();

                    spawnTimer = timeBetweenSpawns;

                }


            }





            // If Phase is over Give option to leave
            if (currentArea == 5 || currentArea == 10 || currentArea == 15 || currentArea == 20)
            {
                StartCoroutine(rageQuit());
            }


            // Finally, update player interface
            UpdateUI();

        }

        private void LateUpdate()
        {

            changeRound();

            if (Parent.transform.childCount <= 0 && round1 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 2;
                roundCount();

                nextWave = false;


            }
            if (currentWave == 2 && Parent2.transform.childCount <= 0 && round2 == true)
            {
                nextWave = true;
                Debug.Log("round 2 over");
                currentWave = 3;
                roundCount();

                nextWave = false;

            }
            if (currentWave == 3 && Parent3.transform.childCount <= 0 && round3 == true)
            {
                nextWave = true;
                Debug.Log("round 3 over");
                currentWave = 4;


                nextWave = false;

            }

            if (currentWave == 4 && Parent4.transform.childCount <= 0 && round4 == true)
            {
                nextWave = true;
                Debug.Log("round 4 over");
                currentWave = 5;


                nextWave = false;

            }

            if (currentWave == 5 && Parent5.transform.childCount <= 0 && round5 == true)
            {
                nextWave = true;
                Debug.Log("round 5 over");
                currentWave = 6;

                nextWave = false;

            }

            if (currentWave == 6 && Parent6.transform.childCount <= 0 && round6 == true)
            {
                nextWave = true;
                Debug.Log("round 6 over");
                currentWave = 7;

                nextWave = false;

            }

            if (currentWave == 7 && Parent7.transform.childCount <= 0 && round7 == true)
            {
                nextWave = true;
                Debug.Log("round 7 over");
                currentWave = 8;

                nextWave = false;

            }

            if (currentWave == 8 && Parent8.transform.childCount <= 0 && round8 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 9;
                nextWave = false;

            }

            if (currentWave == 9 && Parent9.transform.childCount <= 0 && round9 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 10;
                nextWave = false;

            }

            if (currentWave == 10 && Parent10.transform.childCount <= 0 && round10 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 11;
                nextWave = false;

            }

            if (currentWave == 11 && Parent11.transform.childCount <= 0 && round11 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 12;
                nextWave = false;

            }

            if (currentWave == 12 && Parent12.transform.childCount <= 0 && round12 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 13;
                nextWave = false;

            }

            if (currentWave == 13 && Parent13.transform.childCount <= 0 && round13 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 14;
                nextWave = false;

            }

            if (currentWave == 14 && Parent14.transform.childCount <= 0 && round14 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 15;
                nextWave = false;

            }

            if (currentWave == 15 && Parent15.transform.childCount <= 0 && round15 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 16;
                nextWave = false;

            }

            if (currentWave == 16 && Parent16.transform.childCount <= 0 && round16 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 17;
                nextWave = false;

            }

            if (currentWave == 17 && Parent17.transform.childCount <= 0 && round17 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 18;
                nextWave = false;

            }

            if (currentWave == 18 && Parent18.transform.childCount <= 0 && round18 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 19;
                nextWave = false;

            }

            if (currentWave == 19 && Parent19.transform.childCount <= 0 && round19 == true)
            {
                nextWave = true;
                Debug.Log("round 1 over");
                currentWave = 20;
                nextWave = false;

            }

            if (currentWave == 20 && Parent20.transform.childCount <= 0 && round20 == true)
            {
                nextWave = true;
                Debug.Log("round 20 over");
                currentWave = 21;
                nextWave = false;

            }
        }

        // ------------------------------------------------- //


        public void SpawnEnemy()
        {

            currentPoint = spawnPoints[index];
            currentPoint2 = spawnPoints2[index];
            currentPoint3 = spawnPoints3[index];
            currentPoint4 = spawnPoints4[index];
            spawnPoints = GameObject.FindGameObjectsWithTag("MansionSpawn");
            spawnPoints2 = GameObject.FindGameObjectsWithTag("CourtyardSpawn");
            spawnPoints3 = GameObject.FindGameObjectsWithTag("StreetSpawn");
            spawnPoints4 = GameObject.FindGameObjectsWithTag("ChurchSpawn");

            index = Random.Range(0, spawnPoints.Length);
            index2 = Random.Range(0, spawnPoints2.Length);
            index3 = Random.Range(0, spawnPoints3.Length);
            index4 = Random.Range(0, spawnPoints4.Length);



            // Resource check
            if (enemyPrefabs.Length == 0)
            {
                Debug.LogError($"Cannot spawn enemy: no prefabs have been assigned to object '{name}'");
                return;
            }


            // Pick random prefab
            var i = Random.Range(0, enemyPrefabs.Length);

            // Instantiate
            if (currentWave == 1 && spawned <= enemiesToSpawn)
            {

                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent.transform);
                //nextWave = true;
                round1 = true;
                spawned++;



            }


            if (currentWave == 2 && spawned <= Wave2EnemiesToSpawn)
            {

                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent2.transform);
                // nextWave = true;
                round2 = true;
                spawned++;


            }
            if (currentWave == 3 && spawned <= Wave3EnemiesToSpawn)
            {


                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent3.transform);
                // nextWave = true;
                round3 = true;
                spawned++;

            }
            if (currentWave == 4 && spawned <= Wave4EnemiesToSpawn)
            {


                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent4.transform);
                // nextWave = true;
                round4 = true;
                spawned++;


            }
            if (currentWave == 5 && spawned <= Wave5EnemiesToSpawn)
            {


                Instantiate(enemyPrefabs[i], currentPoint.transform.position, currentPoint.transform.rotation, Parent5.transform);
                round5 = true;
                spawned++;
            }
            if (currentWave == 6 && spawned <= Wave6EnemiesToSpawn)
            {




                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint2.transform.position, currentPoint2.transform.rotation, Parent6.transform);
                round6 = true;
                spawned++;
            }
            if (currentWave == 7 && spawned <= Wave7EnemiesToSpawn)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint2.transform.position, currentPoint2.transform.rotation, Parent7.transform);
                round7 = true;
                spawned++;
            }

            if (currentWave == 8)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint2.transform.position, currentPoint2.transform.rotation, Parent8.transform);
                round8 = true;
                spawned++;
            }

            if (currentWave == 9)
            {

                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint2.transform.position, currentPoint2.transform.rotation, Parent9.transform);
                round9 = true;
                spawned++;
            }

            if (currentWave == 10)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint2.transform.position, currentPoint2.transform.rotation, Parent10.transform);
                round10 = true;
                spawned++;
            }

            if (currentWave == 11)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint3.transform.position, currentPoint3.transform.rotation, Parent11.transform);
                round11 = true;
                spawned++;
            }

            if (currentWave == 12)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint3.transform.position, currentPoint3.transform.rotation, Parent12.transform);
                round12 = true;
                spawned++;
            }

            if (currentWave == 13)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint3.transform.position, currentPoint3.transform.rotation, Parent13.transform);
                round13 = true;
                spawned++;
            }

            if (currentWave == 14)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint3.transform.position, currentPoint3.transform.rotation, Parent14.transform);
                round14 = true;
                spawned++;

            }

            if (currentWave == 15)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint3.transform.position, currentPoint3.transform.rotation, Parent15.transform);
                round15 = true;
                spawned++;

            }

            if (currentWave == 16)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint4.transform.position, currentPoint4.transform.rotation, Parent16.transform);
                round16 = true;
                spawned++;

            }

            if (currentWave == 17)
            {

                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint4.transform.position, currentPoint4.transform.rotation, Parent17.transform);
                round17 = true;
                spawned++;

            }

            if (currentWave == 18)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint4.transform.position, currentPoint4.transform.rotation, Parent18.transform);
                round18 = true;
                spawned++;

            }

            if (currentWave == 19)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint4.transform.position, currentPoint4.transform.rotation, Parent19.transform);
                round19 = true;
                spawned++;

            }

            if (currentWave == 20)
            {


                enemyPrefabs[i] = Instantiate(enemyPrefabs[i], currentPoint4.transform.position, currentPoint4.transform.rotation, Parent20.transform);
                round20 = true;
                spawned++;

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
        public IEnumerator roundCount()
        {


            if (textOn == true)
            {
                roundCounter.gameObject.SetActive(true);
            }

            var countDown = 10f;


            if (rq2 == false)
            {
                rq2 = true;
                textOn2 = true;

                for (int i = 0; i < 10000; i++)
                {
                    while (countDown >= 0)
                    {
                        //rqText.gameObject.SetActive(true);
                        // Debug.Log(i++);
                        countDown -= Time.smoothDeltaTime;

                        yield return null;
                    }




                }

            }
            yield return new WaitForSeconds(3);
            textOn2 = false;
            roundCounter.gameObject.SetActive(false);
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