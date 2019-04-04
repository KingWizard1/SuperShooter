using System;
using UnityEngine;

namespace SuperShooter
{

    public interface IInteractable
    {

        string GetDisplayName();

        void Pickup();
        void Drop();

    }

}