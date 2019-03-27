using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnPoints : MonoBehaviour {

    public Transform[] PlayerSpawns;

    // ------------------------------------------------- //

    private void Start () {

        // Relies on this script being placed on the PARENT object which
        // contains sub-objects whose transforms represent player spawn points.

        PlayerSpawns = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            PlayerSpawns[i] = transform.GetChild(i);

    }

    // ------------------------------------------------- //

    public Transform GetRandomPlayerSpawnPoint()
    {
        return PlayerSpawns[Random.Range(0, PlayerSpawns.Length)];
    }

    // ------------------------------------------------- //

    // For debugging only!
    private int DEBUG_CURRENT_SPAWN_ID = 0;

    // For debugging only!
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            var nextSpawn = PlayerSpawns[DEBUG_CURRENT_SPAWN_ID++];
            if (DEBUG_CURRENT_SPAWN_ID >= PlayerSpawns.Length)
                DEBUG_CURRENT_SPAWN_ID = 0;

            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.SetPositionAndRotation(nextSpawn.position, nextSpawn.rotation);

        }


    }

    // ------------------------------------------------- //

}
