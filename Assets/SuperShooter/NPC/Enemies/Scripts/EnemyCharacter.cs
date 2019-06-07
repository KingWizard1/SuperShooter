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

        DiedBackward = 63,
        DiedForward = 64,
    }

    [RequireComponent(typeof(EnemyController))]
    public class EnemyCharacter : CharacterEntity
    {

        [Header("Attack")]
        public int meleeDamage = 25;
        public GameObject weapon;

        [Header("Rewards")]
        public int XPValue;

        //[Header("Events")]
        //public UnityEvent ev = new UnityEvent();

        // ------------------------------------------------- //

        public EnemyCharacterState characterState = EnemyCharacterState.Idle;

        // ------------------------------------------------- //

        private Animator _animator;
        private EnemyController _controller;

        // ------------------------------------------------- //

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<EnemyController>();
        }

        // ------------------------------------------------- //

        private void Start()
        {
            
        }

        // ------------------------------------------------- //


        private void Update()
        {


            UpdateCharacterState();

        }

        // ------------------------------------------------- //

        private void UpdateCharacterState()
        {

            // Are we dead? Do nothing.
            if (isDead)
                return;

            // Is character idle? (Agent NOT active)
            if (!_controller.isAgentActive) {
                characterState = EnemyCharacterState.Idle;
                return;
            }

            // Is the main player dead?
            if (GameManager.Main != null && GameManager.Main.PlayerCharacter != null)
                if (GameManager.Main.PlayerCharacter.isDead) {
                    characterState = EnemyCharacterState.Victory;
                    return;
                }

            // Do we have a weapon equipped?
            if (weapon != null) {

                // We will use melee attacks.
                if (_controller.movementState == EnemyControllerState.Goto ||
                    _controller.movementState == EnemyControllerState.Search)
                    characterState = EnemyCharacterState.RunningWeaponAttack;
            }
            else {

                // We will use melee attacks.
                if (_controller.movementState == EnemyControllerState.Goto ||
                    _controller.movementState == EnemyControllerState.Search)
                    characterState = EnemyCharacterState.Running;

            }

            // Are we running away from danger?
            if (_controller.movementState == EnemyControllerState.Flee)
                characterState = EnemyCharacterState.Running;


            // Set the animator state.
            _animator.SetInteger("characterState", (int)characterState);

            // Done.

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        public void KickTarget()
        {

            // Try and get a CharacterEntity script from the target.
            // If it has one, tell it to take some damage.
            var target = _controller.target;
            if (target != null)
                target.GetComponent<ICharacterEntity>()?.TakeDamage(meleeDamage, this);

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //
    }
}