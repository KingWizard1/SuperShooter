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

        string GetInteractionString();

        [Obsolete("Do not implement this. It needs to be removed as it is not appropriate/generic enough for interactables.")]
        string GetDisplayName();

        void Interact(IInteractor interactor);

    }

    public interface IInteractablePickup : IInteractable
    {

        void Drop();

    }

    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        [Header("Interactable")]
        public bool canInteract = true;

        // ------------------------------------------------- //

        public virtual string GetDisplayName() => "Display name not set (and obsolete!)";

        public abstract string GetInteractionString();

        // ------------------------------------------------- //

        public void Interact(IInteractor interactor)
        {
            if (canInteract)
                OnInteract(interactor);
        }

        public virtual void OnInteract(IInteractor interactor) { }

        // ------------------------------------------------- //

    }

}