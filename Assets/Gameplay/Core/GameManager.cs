using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class GameManager : MonoBehaviour
    {

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

        public static GameManager Main { get; private set; }

        // ------------------------------------------------- //

        private SpawnPoints spawnPoints;

        // ------------------------------------------------- //

        void Awake()
        {

            Main = this;

            // Get SpawnPoints. They're a collection of transforms listed under a tagged parent object.
            spawnPoints = GameObject.FindGameObjectWithTag("SpawnPoints").GetComponent<SpawnPoints>();
        }

        // ------------------------------------------------- //

        void Update()
        {

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

        /// <summary>Runs on the server/host ONLY.</summary>
        public void SceneSetup(GameMode gameMode)
        {

            // Disable the debug player.
            var debugPlayer = GameObject.FindGameObjectWithTag("Player");
            debugPlayer.name = "DebugPlayer";
            debugPlayer.SetActive(false);


            // Stuff that needs to happen on all clients goes here.


            Debug.Log("[GAME] Setting up for " + gameMode.ToString());

            //// Create empty game object to hold each player.
            //var playerContainer = new GameObject();
            //playerContainer = Instantiate(playerContainer, Vector3.zero, Quaternion.identity);

            //// Spawn local player
            //SpawnPlayer("LocalPlayer", true);

        }

        // ------------------------------------------------- //

        public void SpawnPlayer(string name)
        {

            Debug.Log("[GAME] Spawn " + name);

            // Get random spawn point for local player.
            // FOR DEBUG
            Transform spawnPoint;
            // Create player. They should have control immediately.
            // The static BoltPrefabs class is compiled and updated by Bolt, and
            // contains a unique reference to each prefab with a Bolt Entity on it.
            // You can also pass in a normal GameObject reference instead.
            //player.name = name;


        }

        // ------------------------------------------------- //


        public Transform GetRandomSpawnPoint()
        {

            var spawnPoint = spawnPoints.GetRandomPlayerSpawnPoint();

            // TODO: Check if there is a player already at the spawn point.
            // If yes, re-roll the dice until a point is selected that is not obstructed.

            return spawnPoint;
        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}