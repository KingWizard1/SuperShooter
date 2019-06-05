using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class GameManager : MonoBehaviour
    {

        [Header("Time")]
        public float globalTimeScale = 1;
        public static GameManager Main { get; private set; }

        // ------------------------------------------------- //

        private SpawnPoints spawnPoints;

        // ------------------------------------------------- //

        void Awake()
        {

            Main = this;

            // Get SpawnPoints. They're a collection of transforms listed under a tagged parent object.
            spawnPoints = GameObject.FindGameObjectWithTag("SpawnPoints")?.GetComponent<SpawnPoints>();
        }

        // ------------------------------------------------- //

        void Update()
        {


            // Set global time scale
            //Time.timeScale = globalTimeScale;




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



            // Bail out before we do stuff that should only be done on the server.
            //if (!BoltNetwork.IsServer)
            //{
            //    Debug.LogError("[ERR!] SceneSetup attempted on localhost when localhost is a non-server peer.");
            //    return;
            //}

            Debug.Log("[GAME] Setting up for " + gameMode.ToString());

            //// Create empty game object to hold each player.
            //var playerContainer = new GameObject();
            //playerContainer = Instantiate(playerContainer, Vector3.zero, Quaternion.identity);

            //// Spawn local player
            //SpawnPlayer("LocalPlayer", true);

            //// Determine network mode
            //var networkMode = BoltNetwork.IsSinglePlayer ?
            //    NetworkMode.SinglePlayer : BoltNetwork.IsServer ?
            //    NetworkMode.MultiplayerHost : NetworkMode.MultiplayerClient;

            //switch (networkMode)
            //{
            //    case NetworkMode.SinglePlayer:
            //        break;
            //    case NetworkMode.MultiplayerHost:
            //        break;
            //    case NetworkMode.MultiplayerClient:
            //        break;
            //    default:
            //        break;
            //}

            ////
            //switch (gameMode)
            //{
            //    case GameMode.Slayer:
            //        break;
            //    case GameMode.TeamDeathmatch:
            //        break;
            //    case GameMode.CaptureTheFlag:
            //        break;
            //    default:
            //        break;
            //}

        }

        // ------------------------------------------------- //

        public void SpawnPlayer(string name)
        {

            Debug.Log("[GAME] Spawn " + name);

            // Get random spawn point for local player.
            // FOR DEBUG
            Transform spawnPoint;

            //if (BoltNetwork.IsServer)
            //    spawnPoint = spawnPoints.PlayerSpawns[1];
            //else if (BoltNetwork.IsClient)
            //    spawnPoint = spawnPoints.PlayerSpawns[0];
            //else
            //    spawnPoint = GetRandomSpawnPoint();

            // Create player. They should have control immediately.
            // The static BoltPrefabs class is compiled and updated by Bolt, and
            // contains a unique reference to each prefab with a Bolt Entity on it.
            // You can also pass in a normal GameObject reference instead.

            //var player = BoltNetwork.Instantiate(BoltPrefabs.FPSController, spawnPoint.position, spawnPoint.rotation);
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