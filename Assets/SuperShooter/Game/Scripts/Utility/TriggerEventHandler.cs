using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace SuperShooter
{
    public delegate void TriggerEventDelegate();

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

        public bool isTriggered = false;

        public TriggerEventDelegate OnTriggered;

        // ------------------------------------------------- //

        private Collider col;

        // ------------------------------------------------- //

        private void Awake()
        {
            col = GetComponent<Collider>();
        }

        private void Start()
        {
            isTriggered = false;
        }

        // ------------------------------------------------- //

        private void OnTriggerEnter(Collider other)
        {

            if (playerOnly && other.tag != "Player")
                return;

            isTriggered = true;

            OnTriggered?.Invoke();

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