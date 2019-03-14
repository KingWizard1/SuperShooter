using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public class Interactable : MonoBehaviour
    {

        public virtual void Interact()
        {
            Debug.Log("No interaction has been implented for this object.");
        }
    }
}
