using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SuperShooter
{
    public enum EnemyCharacterState
    {
        Idle = 0,
        Walking = 1,
        Running = 2,
        StandingMeleeAttack = 3,
        StandingWeaponAttack = 4,
        RunningWeaponAttack = 5,

        Victory = 32,

        DiedBackward = 64,
        DiedForward = 65,
    }

    public class EnemyCharacter : CharacterEntity
    {

        [Header("Attack")]
        public int meleeDamage = 25;
        public float meleeRange = 5;
        public float shootingRange = 10;
        public GameObject weapon;
        
        


 

        [Header("Rewards")]
        public int XPValue;

        //[Header("Events")]
        //public UnityEvent ev = new UnityEvent();

        // ------------------------------------------------- //

        public EnemyCharacterState characterState { get; private set; } = EnemyCharacterState.Idle;

        // ------------------------------------------------- //

        private Animator _animator;
        private EnemyController _controller;

        // ------------------------------------------------- //

        private void Reset()
        {
            // Force default value.
            type = TargetType.Enemy;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<EnemyController>();
        }

        // ------------------------------------------------- //

        private void Start()
        {


            ResetHealth();
            

        }

        // ------------------------------------------------- //

        private void Update()
        {

            UpdateAgentProperties();
            UpdateCharacterState();


        }



        // ------------------------------------------------- //

        #region Update Methods

        private void UpdateAgentProperties()
        {

            // If we have a weapon equipped, we want the controller
            // to get within shooting range of the target. If no weapon,
            // then we want it to get within melee range instead.
            if (_controller != null)
                if (weapon != null)
                    _controller.stoppingDistance = shootingRange;
                else
                    _controller.stoppingDistance = meleeRange;


        }

        private void UpdateCharacterState()
        {

            // Are we dead? Do nothing.
            if (isDead)
                return;

            // Is character idle? (Agent NOT active)
            if (!_controller.isAgentActive)
            {
                SetCharacterState(EnemyCharacterState.Idle);
                return;
            }

            // Is character idle? (Agent stopped)
            if (_controller.State == EnemyControllerState.Stop)
                SetCharacterState(EnemyCharacterState.Idle);


            // Is the main player dead?
            if (GameManager.Main != null && GameManager.Main.PlayerCharacter != null)
                if (GameManager.Main.PlayerCharacter.isDead)
                {
                    SetCharacterState(EnemyCharacterState.Victory);
                    return;
                }

            // Is searching for a target?
            if (_controller.State == EnemyControllerState.Search)
                SetCharacterState(EnemyCharacterState.Walking);

            // Do we have a weapon equipped?
            if (weapon != null)
            {

                // We will use weapon attacks.
                if (_controller.State == EnemyControllerState.Goto)
                    SetCharacterState(EnemyCharacterState.RunningWeaponAttack);

                if (_controller.isWithinStoppingDistance)
                {
                    // TODO: Shoot.
                }
            }
            else
            {

                // We will use melee attacks.
                if (_controller.State == EnemyControllerState.Goto)
                    SetCharacterState(EnemyCharacterState.Running);

                // Melee attack!
                // The animation clip is expected to have an animation event
                // that will fire a function at the peak of the attack action.
                if (_controller.isWithinStoppingDistance)
                    SetCharacterState(EnemyCharacterState.StandingMeleeAttack);
            }

            // Are we running away from danger?
            if (_controller.State == EnemyControllerState.Flee)
                SetCharacterState(EnemyCharacterState.Running);

            // Done.

        }

        #endregion

        // ------------------------------------------------- //

        #region Character Animation

        private void SetCharacterState(EnemyCharacterState newState)
        {
            // Set the character state
            characterState = newState;

            // Set the animation state.
            _animator.SetInteger("characterState", (int)newState);
        }

        // ------------------------------------------------- //

        //public void AnimationSprintEvent()
        //{
        //    _controller.allowMovement = true;
        //}

        // ------------------------------------------------- //

        // Called by the related animation event script.
        public void AnimationKickEvent(bool finished)
        {
            // If the kick event fired on the frame the animation is finished.
            if (!finished)
            {

                // Try and get a CharacterEntity script from the target.
                // If it has one, tell it to take some damage.
                if (!_controller.hasTarget)
                    return;

                if (_controller.distanceToTarget <= meleeRange)
                    _controller.target.GetComponent<ICharacterEntity>()?.TakeDamage(meleeDamage, this);

            }
            else
            {

                _controller.GoToTarget();

            }

        }

        #endregion

        // ------------------------------------------------- //

        #region Character Event Overrides

        public override void OnTargetKilled(ICharacterEntity target)
        {
            SetCharacterState(EnemyCharacterState.Victory);

        }

        public override void OnDeath()
        {

            // Disable AI
            _controller.Stop();

            // Death/fall animation
            var randomFallAnim = Random.Range((int)EnemyCharacterState.DiedBackward, (int)EnemyCharacterState.DiedForward);
            SetCharacterState((EnemyCharacterState)randomFallAnim);


        }

        public override void BackFromTheDead()
        {
            _controller.Search();
        }

        #endregion

        // ------------------------------------------------- //
    }
}