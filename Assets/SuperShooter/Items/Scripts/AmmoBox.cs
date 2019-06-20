using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public enum ConsumableType
    {
        Ammo, 
        Health,
    }

    public class Consumable : MonoBehaviour, IInteractablePickup
    {
        public string displayName;

        public ConsumableType consumableType;

        public void Drop()
        {
           
        }

        public string GetDisplayName() => displayName;

        public string GetInteractionString() => displayName;

        public void Pickup(ICharacterController owner)
        {
            return;
        }
    }

    public class AmmoBox : Consumable
    {
        public int ammoAmount;
    }
}
