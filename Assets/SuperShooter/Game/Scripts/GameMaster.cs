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


        public MonoBehaviour rqText;
        public bool rq;
        public bool textOn;


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



            // If Phase is over Give option to leave
            if (currentArea == 5 || currentArea == 10 || currentArea == 15 || currentArea == 20)
            {
                StartCoroutine(rageQuit());
            }


            // Finally, update player interface
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