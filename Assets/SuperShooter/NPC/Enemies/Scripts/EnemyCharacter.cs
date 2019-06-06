using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SuperShooter
{

    [RequireComponent(typeof(EnemyController))]
    public class EnemyCharacter : CharacterEntity
    {

        [Header("Melee")]
        public int damage = 25;

        [Header("Weapons")]
        public GameObject weapon;

        [Header("Rewards")]
        public int XPValue;

        //[Header("Events")]
        //public UnityEvent ev = new UnityEvent();

        // ------------------------------------------------- //

        private EnemyController _controller;

        // ------------------------------------------------- //

        private void Awake()
        {
            _controller = GetComponent<EnemyController>();
        }

        // ------------------------------------------------- //

        private void Start()
        {
            
        }

        // ------------------------------------------------- //


        private void Update()
        {

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //
    }
}