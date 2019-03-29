using Bolt;
using UnityEngine;
using System.Collections;

namespace SuperShooter
{

    // The BGE Attr allows Bolt to automatically detect this script and create
    // an instance of it at runtime, and also destroy it when Bolt is shut down.
    [BoltGlobalBehaviour]
    public class NetworkCallbacks : GlobalEventListener // Extends from its base, which extends from MonoBehaviour.
    {

        private BoltConnection connection;

        public override void Connected(BoltConnection connection)
        {
            this.connection = connection;
        }

        public override void SceneLoadLocalBegin(string scene, IProtocolToken token)
        {
            Debug.Log("[Bolt] Loading scene '" + scene + "'");
        }

        public override void SceneLoadLocalDone(string scene)
        {
            // This method is run when the scene finishing loading on the local client.

            Debug.Log("[Bolt] SceneLoadLocalDone");


            // Randomize a spawn position
            //var spawnPosition = new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));
            //var spawnPoint = GameManager.Main.GetRandomSpawnPoint();

            // Instantiate a cube.
            // The static BoltPrefabs class is compiled and updated by Bolt, and
            // contains a unique reference to each prefab with a Bolt Entity on it.
            // You can also pass in a normal GameObject reference instead.
            //BoltNetwork.Instantiate(BoltPrefabs.Player, spawnPoint.position, spawnPoint.rotation);

            // Load and setup main map
            if (BoltNetwork.IsServer)
            {
                Debug.Log("[Bolt] Scene loaded as a host.");
                GameManager.Main.SceneSetup(GameMode.Slayer);
                GameManager.Main.SpawnPlayer("HostPlayer");
            }
            if (BoltNetwork.IsClient)
            {
                Debug.Log("[Bolt] Scene loaded as a client.");
                GameManager.Main.SpawnPlayer("Player" + connection.ConnectionId.ToString());
            }


        }

        public override void SceneLoadRemoteDone(BoltConnection connection)
        {

            Debug.Log("[Bolt] Player" + connection.ConnectionId.ToString() + " has loaded in.");

        }


    }
}