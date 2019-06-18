using UnityEngine;

namespace SuperShooter
{
    public abstract class GameArea : MonoBehaviour
    {

        [Header("Status")]
        [Tooltip("Represents the sub-area that the player is in, or the part of the sequence of events this area is experiencing.")]
        public int milestone = 0;

        /// <summary>Has the area finished its sequence of events?</summary>
        public bool isComplete = false;

        public bool failedToStart = false;

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        private void Start()
        {

            // NEED a game master.
            if (!GameMaster.Exists) {
                Debug.LogError($"There is no {nameof(GameMaster)} in this scene! Area cannot work properly without one. Disabling.");
                enabled = false;
                return;
            }

            // Call override
            failedToStart = OnStart();
            if (!failedToStart) {
                Debug.LogError($"Scripted area '{name}' ran into problems during its {nameof(OnStart)} function. Disabling.");
                enabled = false;
            }
            
        }

        // ------------------------------------------------- //

        /// <summary>When overriden, runs logic to set up the play area, and returns whether it was successful in doing so.</summary>
        protected abstract bool OnStart();

        // ------------------------------------------------- //



        // ------------------------------------------------- //

        public void NextMilestone()
        {
            milestone++;
            Debug.Log($"{name} has progressed to sequence {milestone}.");
        }

        /// <summary>Signals completion of this area and its sequence of events.
        /// Sets <see cref="isComplete"/> to <see cref="true"/> then disables the script.</summary>
        public void Complete()
        {
            isComplete = true;
            enabled = false;
        }


        // ------------------------------------------------- //



        // ------------------------------------------------- //


    }

}