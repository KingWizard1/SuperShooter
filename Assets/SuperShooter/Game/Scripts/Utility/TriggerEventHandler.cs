using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace SuperShooter
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEventHandler : MonoBehaviour
    {
        [Header("Options")]
        public bool disappearOnEnter = false;
        public bool disappearOnExit = false;

        [Header("Whitelist")]
        //[ReorderableList]
        //public PlayerCharacter allowedObjects;
        public bool playerOnly = true;

        [Header("Actions")]
        public UnityEvent OnEnter;
        public UnityEvent OnStay;
        public UnityEvent OnExit;

        // ------------------------------------------------- //

        private Collider col;

        // ------------------------------------------------- //

        private void Awake()
        {
            col = GetComponent<Collider>();
        }

        // ------------------------------------------------- //

        private void OnTriggerEnter(Collider other)
        {

            if (playerOnly && other.tag != "Player")
                return;

            OnEnter?.Invoke();

            if (disappearOnEnter)
                Deactivate();
        }


        private void OnTriggerStay(Collider other)
        {
            if (playerOnly && other.tag != "Player")
                return;

            OnStay?.Invoke();


        }


        private void OnTriggerExit(Collider other)
        {
            if (playerOnly && other.tag != "Player")
                return;

            OnExit?.Invoke();


            if (disappearOnExit)
                Deactivate();
        }

        // ------------------------------------------------- //

        public void Deactivate()
        {
            gameObject.SetActive(false);
            col.enabled = false;
            enabled = false;
        }
    }

}