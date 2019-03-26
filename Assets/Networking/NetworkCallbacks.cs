using Bolt;
using UnityEngine;
using System.Collections;

// The BGE Attr allows Bolt to automatically detect this script and create
// an instance of it at runtime, and also destroy it when Bolt is shut down.
[BoltGlobalBehaviour]
public class NetworkCallbacks : GlobalEventListener // Extends from its base, which extends from MonoBehaviour.
{

    public override void SceneLoadLocalDone(string scene)
    {
        // This method is run when the scene finishing loading on the local client.

        // Randomize a spawn position
        //var spawnPosition = new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));

        var spawnPointObject = GameObject.FindGameObjectWithTag("SpawnPoints");
        var spawnPoints = spawnPointObject.GetComponent<SpawnPoints>();
        var spawnPoint = spawnPoints.GetRandomPlayerSpawnPoint();

        // Instantiate a cube.
        // The static BoltPrefabs class is compiled and updated by Bolt, and
        // contains a unique reference to each prefab with a Bolt Entity on it.
        // You can also pass in a normal GameObject reference instead.
        BoltNetwork.Instantiate(BoltPrefabs.Player, spawnPoint.position, spawnPoint.rotation);

    }


}