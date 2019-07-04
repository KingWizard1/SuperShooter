using System.Linq;
using UnityEngine;
using NaughtyAttributes;

namespace SuperShooter
{

    public enum GameAreaCompletionAction
    {
        Nothing = 0,
        StartNextArea = 1,
        WinTheGame = 64,
        GameOver = -1,
    }

    public abstract class GameArea : MonoBehaviour
    {

        [Header("Status")]
        [Tooltip("Represents the sub-area that the player is in, or the part of the sequence of events this area is experiencing.")]
        public int sequence = 0;

        /// <summary>Is this area currently in play?</summary>
        public bool isPlaying = false;

        /// <summary>Has the area finished its sequence of events?</summary>
        public bool isComplete = false;

        /// <summary>Did this area fail to start playing out when requested?</summary>
        public bool failedToStart = false;

        [Header("Area Config")]
        [ReorderableList] public TriggerEventHandler[] startTriggers;
        [ReorderableList] public SpawnPoint[] spawnPoints;

        [Header("Sequence Config")]
        public GameAreaCompletionAction actionWhenComplete = GameAreaCompletionAction.StartNextArea;


        // ------------------------------------------------- //

        public bool HasStartTrigger => startTriggers != null && startTriggers.Length > 0;


        // ------------------------------------------------- //

        private void Start()
        {

            // NEED a game master.
            if (!GameMaster.Exists) {
                Debug.LogError($"There is no {nameof(GameMaster)} in this scene! Area {name} will not work properly without one. Disabling.");
                enabled = false;
                return;
            }

            // Call Start override
            failedToStart = OnStart();
            if (!failedToStart) {
                Debug.LogError($"Scripted area '{name}' ran into problems during its {nameof(OnStart)} function. Disabling.");
                enabled = false;
            }

            // Setup trigger callbacks
            foreach (var trigger in startTriggers)
                trigger.OnTriggered += Play;

        }

        // ------------------------------------------------- //

        /// <summary>When overriden, runs logic to set up the play area, and returns whether it was successful in doing so.</summary>
        protected abstract bool OnStart();

        protected virtual void OnUpdate() { }

        protected virtual void OnUpdateUI() { }

        // ------------------------------------------------- //

        /// <summary>Signal to start this area's sequence of events.</summary>
        [Button]
        public void Play()
        {

            Debug.Log($"{nameof(Play)}() called on area '{name}'.");

            OnPlay();

            isPlaying = true;


        }

        /// <summary>Signal to stop this area's sequence of events.</summary>
        [Button]
        public void Stop()
        {
            Debug.Log($"{nameof(Stop)}() called on area '{name}'.");

            OnStop();

            isPlaying = false;

        }

        /// <summary>Signal to stop this area's sequence of events and
        /// do a full reset so <see cref="Play"/> can be called again.</summary>
        [Button]
        public void DoReset()
        {
            Debug.Log($"{nameof(DoReset)}() called on area '{name}'.");

            Stop();

            sequence = 0;
            isComplete = false;
            failedToStart = false;

        }

        protected virtual void OnPlay() { }

        protected virtual void OnStop() { }

        protected virtual void OnReset() { }

        // ------------------------------------------------- //

        private void Update()
        {
            // Do nothing if we're not in play.
            if (!isPlaying)
                return;

            // Signal the derived class to handle its current sequence.
            OnUpdate();

            // Signal the derived class to handle relevant UI elements.
            OnUpdateUI();
        }

        // ------------------------------------------------- //

        /// <summary>Play the next sequence.</summary>
        public void NextMilestone()
        {
            sequence++;
            Debug.Log($"{nameof(NextMilestone)}() called on {name}. Entering sequence {sequence}.");
        }

        /// <summary>Signals completion of this area and its sequence of events.
        /// Sets <see cref="isComplete"/> to <see cref="true"/> then disables the script.</summary>
        public void Complete()
        {
            Debug.Log($"{nameof(Complete)}() called on {name}. AREA COMPLETE!");

            isComplete = true;
            enabled = false;

            // What's next?
            switch (actionWhenComplete)
            {
                case GameAreaCompletionAction.StartNextArea:

                    GameMaster.Main?.StartNextArea();

                    break;

                case GameAreaCompletionAction.WinTheGame:

                    throw new System.NotImplementedException($"Action {nameof(GameAreaCompletionAction.WinTheGame)} not implemented.");

                    break;

                case GameAreaCompletionAction.GameOver:

                    throw new System.NotImplementedException($"Action {nameof(GameAreaCompletionAction.GameOver)} not implemented.");

                    break;

                case GameAreaCompletionAction.Nothing:

                    break;
            }

        }


        // ------------------------------------------------- //



        // ------------------------------------------------- //



        // ------------------------------------------------- //

        [Button("Auto Populate Spawn Points")]
        public SpawnPoint[] GetSpawnPoints()
        {


            // Get the new area's collection of spawn points.
            // This is for the enemy manager to know where to spawn things in.
            spawnPoints = GetComponentsInChildren<SpawnPoint>();
            return spawnPoints;

        }

        // ------------------------------------------------- //

    }

}