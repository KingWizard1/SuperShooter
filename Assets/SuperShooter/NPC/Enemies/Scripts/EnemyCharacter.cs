using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SuperShooter
{

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

            _animator.SetBool("meleeAttack", _controller.isWithinStoppingDistance);


        }

        // ------------------------------------------------- //


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