using System;
using UnityEngine;

namespace SuperShooter
{
    /// <summary>Implement on classes that have the ability to interact with <see cref="IInteractable"/>s.</summary>
    public interface IInteractor : ICharacterController
    {

        void DisplayInteractionError(string text);

    }

    public interface IInteractable : IGameEntity
    {

        /// <summary>Determines whether the object is in a state to accept interactions.</summary>
        bool isInteractable { get; }

        /// <summary>Retrieves the display string to be used to indicate to the player how they will interact with the object.</summary>
        string GetInteractionString();

        /// <summary>Perform the default interaction with the object.</summary>
        void Interact(IInteractor interactor);

    }

    public interface IInteractablePickup : IInteractable
    {

        void Drop();

    }

    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        [Header("Interactable")]
        [SerializeField]
        private bool _isInteractable = true;
        /// <summary>Determines whether the object is in a state to accept interactions.</summary>
        public bool isInteractable { get => _isInteractable; set { _isInteractable = value; } }


        // ------------------------------------------------- //

        public abstract string GetInteractionString();

        // ------------------------------------------------- //

        public void Interact(IInteractor interactor)
        {
            if (isInteractable)
                OnInteract(interactor);
        }

        public virtual void OnInteract(IInteractor interactor) { }

        // ------------------------------------------------- //

    }

}