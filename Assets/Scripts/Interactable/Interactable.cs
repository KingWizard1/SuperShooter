using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public enum InteractableType
    {
        Weapon,
        Gadget,

    }

    // ----------------------------------------------------- //

    public class Interactable : MonoBehaviour
    {

        public InteractableType Type;

        // ------------------------------------------------- //


        public virtual void Interact()
        {
            Debug.Log("No interaction has been implented for this object.");
        }
    }
}
