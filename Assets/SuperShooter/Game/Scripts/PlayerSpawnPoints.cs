using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSpawnPoints : MonoBehaviour {

    public Transform[] PlayerSpawns;

    // ------------------------------------------------- //


    // ------------------------------------------------- //

    public static PlayerSpawnPoints Main { get; set; }

    // ------------------------------------------------- //

    private void Awake()
    {
        Main = this;
    }

    private void Start () {

        // Relies on this script being placed on the PARENT object which
        // contains sub-objects whose transforms represent player spawn points.

        PlayerSpawns = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            PlayerSpawns[i] = transform.GetChild(i);

    }

    // ------------------------------------------------- //

    private int CURRENT_SPAWN_ID = 0;

    /// <summary>Gets the next player spawn point in the collection.</summary>
    public Transform GetNextPlayerSpawnPoint()
    {
        // Get next spawn point.
        var nextSpawn = PlayerSpawns[CURRENT_SPAWN_ID++];
        // Loop around back to zero when we reach the end of the collection.
        if (CURRENT_SPAWN_ID >= PlayerSpawns.Length)
            CURRENT_SPAWN_ID = 0;
        // Convert transform to world position (its currently local, because its a sub-object).
        //nextSpawn.rotation = nextSpawn.rotation.Inverse();
        // Return
        return nextSpawn;
    }

    /// <summary>Gets a random next player spawn point from the collection.</summary>
    public Transform GetRandomPlayerSpawnPoint()
    {
        return PlayerSpawns[Random.Range(0, PlayerSpawns.Length)];
    }

    // ------------------------------------------------- //

}
