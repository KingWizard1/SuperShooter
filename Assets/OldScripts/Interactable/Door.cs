using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooterOld
{

    public class Door : Interactable
    {
        
        public bool isOpen;

        // ------------------------------------------------- //

        private Animator anim;
        
        // ------------------------------------------------- //

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        // ------------------------------------------------- //

        public override void Interact()
        {
            // Flip!
            isOpen = !isOpen;

            // Anim open or close
            anim.SetBool("isOpen", isOpen);
        }
    }

}