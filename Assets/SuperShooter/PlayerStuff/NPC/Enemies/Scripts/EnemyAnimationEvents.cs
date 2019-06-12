using UnityEngine;
using UnityEngine.Events;

namespace SuperShooter
{
    [RequireComponent(typeof(EnemyCharacter))]
    public class EnemyAnimationEvents : MonoBehaviour
    {

        private EnemyCharacter _character;

        // ------------------------------------------------- //

        private void Awake()
        {
            _character = GetComponent<EnemyCharacter>();
        }

        // ------------------------------------------------- //

        #region Animation Events

        // OK, so. An 'Animation Event' is a user-definable event that can occur
        // at a specific time/frame of an animation clip. In the Animation window,
        // select the clip you wish to add an event for. There are two buttons next to
        // the clip name: Add Keyframe, and Add Event. Clicking the second button adds a
        // marker to the timeline that can be moved around. Select the event marker.
        // In the inspector, type in the name of the function you wish to call when the
        // playback of the clip reaches the event marker. The name of the function must match
        // the name of a method in a class of a script that is attached to the gameobject
        // that is being animated. i.e. the script MUST be a sibling component of the Animator
        // that is animating the clips/running the events. Its a stupid system and is very
        // unintuitive, but, it works like a charm :)

        // Here, we are defining our animation events for all the enemy animation clips.
        // When then redirect the logic to a UnityEvent (above) which allows us to have
        // more editor-control when creating logic.

        public void IdleEvent()
        {
            //_character.AnimationReturnedToIdle();
        }

        public void KickEvent()
        {
            _character.AnimationKickEvent();
        }

        #endregion

        // ------------------------------------------------- //

    }

}