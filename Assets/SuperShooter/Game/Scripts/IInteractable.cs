using System;
using UnityEngine;

namespace SuperShooter
{

    public interface IInteractable
    {

        string GetInteractionString();

        string GetDisplayName();

        void Pickup();

        void Drop();

    }

}