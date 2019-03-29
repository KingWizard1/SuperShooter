using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooterOld
{
    public enum InteractableType
    {
        Unknown = 0,
        Weapon,
        Ability,
        Consumable,
    }

    // ----------------------------------------------------- //

    public class Interactable : MonoBehaviour
    {

        public InteractableType Type;

        // ------------------------------------------------- //

        //protected PlayerController player;

        // ------------------------------------------------- //

        public virtual void Interact()
        {
            Debug.Log("No interaction has been implented for this object.");
        }
    }
}
