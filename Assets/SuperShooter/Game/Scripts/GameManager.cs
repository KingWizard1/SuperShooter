using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class GameManager : MonoBehaviour
    {

        public PlayerCharacter PlayerObject;

        // ------------------------------------------------- //

        // Singleton
        public static GameManager Main { get; private set; }

        // ------------------------------------------------- //

        private SpawnPoints spawnPoints;

        public PlayerCharacter PlayerCharacter { get; private set; }

        // ------------------------------------------------- //

        private void Awake()
        {

            Main = this;

            // Get SpawnPoints. They're a collection of transforms listed under a tagged parent object.
            //spawnPoints = GameObject.FindGameObjectWithTag("SpawnPoints")?.GetComponent<SpawnPoints>();

        }

        // ------------------------------------------------- //

        private void Start()
        {
            if (PlayerObject != null) {
                PlayerCharacter = PlayerObject.GetComponent<PlayerCharacter>();
                if (PlayerCharacter == null)
                    Debug.LogError($"Player object '{PlayerObject.name}' does not have the " +
                        $"{nameof(PlayerCharacter)} script attached! Are you using the right prefab??");
            }
            else
                Debug.LogError($"No player has been assigned to the {nameof(GameManager)}!!");
            

        }

        // ------------------------------------------------- //

        private void Update()
        {


            // Set global time scale
            //Time.timeScale = globalTimeScale;




        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


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