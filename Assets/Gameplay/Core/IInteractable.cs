using System;
using UnityEngine;

namespace SuperShooter
{

    public interface IInteractable
    {

        string GetTitle();

        void Pickup();
        void Drop();



    }
}