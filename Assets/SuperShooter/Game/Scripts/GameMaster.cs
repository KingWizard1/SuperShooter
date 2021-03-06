﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace SuperShooter
{

    public class GameMaster : MonoBehaviour
    {

        [Header("Debug")]
        [ShowNonSerializedField] public int gamePhase;               // Number of times the player has completed a full cycle of areas.
        [ShowNonSerializedField] public int currentArea;
        [ShowNonSerializedField] public bool isGameOver = false;

        [Header("List of Areas")]
        [ReorderableList]
        public GameArea[] gameplayAreas;    // Area parent objects. Child objects are expected to have spawn points.


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
            var nextArea = gameplayAreas[currentArea];

            Debug.Log($"----------  AREA {currentArea}: '{nextArea.name}' START ----------");


            // Auto start it if it doesnt have a collider trigger to start it
            if (nextArea.HasStartTrigger)
                Debug.Log("Waiting for start trigger to be triggered.");
            else
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
        
        public void GameOver(string gameOverText, string gameOverReason)
        {

            isGameOver = true;

            Debug.Log($"----- GAME OVER: {gameOverText} {gameOverReason} -----");

            
            // Show kill screen
            UIManager.Main?.deathUI?.Show(gameOverText, gameOverReason);

            // Show restart prompt
#if DEBUG
            UIManager.Main?.SetActionText("Press CTRL+R to cast Reincarnate");
#endif

        }

        public void CancelGameOver()
        {
            isGameOver = false;

            UIManager.Main?.deathUI?.Hide();
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
                //StartCoroutine(rageQuit());
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