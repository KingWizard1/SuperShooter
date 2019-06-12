using UnityEngine;
using UnityEditor;

namespace SuperShooter
{
    [CustomEditor(typeof(UFOEnemySpawner))]
    public class UFOEnemySpawnerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var spawner = target as UFOEnemySpawner;

            if (GUILayout.Button("Spawn Enemy"))
            {

                // Runtime check
                if (!Application.isPlaying) {
                    Debug.Log("You can only do that in Play mode.");
                    return;
                }

                // Spawn
                spawner.SpawnEnemy();
            }
        }

    }

}