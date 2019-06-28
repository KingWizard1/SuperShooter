using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperShooter
{
    public class InteractiveDoubleDoor : Interactable
    {

        [Header("Animated Doors")]
        public Animator leftDoor;
        public Animator rightDoor;

        [Header("Door Settings")]
        public bool isOpen = false;
        public bool isLocked = false;

        [Header("Sound Effects")]
        public AudioSource SFXDoorOpen;
        public AudioSource SFXDoorShut;
        public AudioSource SFXDoorLocked;


        // ------------------------------------------------- //

        private void Start()
        {
            SetState(isOpen, isLocked);
        }

        // ------------------------------------------------- //

#if UNITY_EDITOR
        private void Update()
        {
            SetAnim();
        }
#endif

        // ------------------------------------------------- //

        public void OpenDoor()
        {
            SetState(true, isLocked);
            PlayOpenSound();
        }

        public void CloseDoor()
        {
            SetState(false, isLocked);
            PlayShutSound();
        }

        public void LockDoor() => SetState(isOpen, true);

        public void UnlockDoor() => SetState(isOpen, false);

        // ------------------------------------------------- //

        private void SetState(bool open, bool locked)
        {
            isOpen = open;
            isLocked = locked;

            canInteract = !(isOpen && isLocked);   // i.e. It's open, but can't be closed, so its not interactable in this state.

            SetAnim();
        }

        private void SetAnim()
        {
            leftDoor.SetBool("isOpen", isOpen);
            rightDoor.SetBool("isOpen", isOpen);
        }

        // ------------------------------------------------- //

        #region IInteractable

        public override void OnInteract(IInteractor interactor)
        {
            if (isLocked) {

                // Play locked sound
                PlayLockedSound();

                // Give player the reason why they cant interact
                interactor.DisplayInteractionError("This door is locked.");

            }
            else {

                // Invert state
                SetState(!isOpen, isLocked);


            }
        }

        public override string GetInteractionString() => isOpen ? "Close" : "Open";

        #endregion

        // ------------------------------------------------- //

        public void PlayOpenSound()
        {
            SFXDoorOpen?.PlayOneShot();
        }

        public void PlayShutSound()
        {
            SFXDoorShut?.PlayOneShot();
        }

        public void PlayLockedSound()
        {
            SFXDoorLocked?.PlayOneShot();
        }

        // ------------------------------------------------- //

    }
}