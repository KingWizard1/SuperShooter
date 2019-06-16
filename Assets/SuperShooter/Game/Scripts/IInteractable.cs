using System;
using UnityEngine;

namespace SuperShooter
{

    public interface IInteractable : IGameEntity
    {

        string GetInteractionString();

        string GetDisplayName();

    }

    public interface IInteractablePickup : IInteractable
    {

        void Pickup(ICharacterController owner);

        void Drop();

    }

}