using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{

    public class UIManager : MonoBehaviour
    {


        [Header("UI Elements")]
        public GameObject pickupPrompt;
        public GameObject pickupPrompt3D;
        public GameObject crossHairSystem;
        public GameObject deathScreen;

        // ------------------------------------------------- //

        public CrossHairSystem CrossHair { get { return crossHairSystem.GetComponent<CrossHairSystem>(); } }

        // ------------------------------------------------- //

        public static UIManager Main { get; private set; }

        // ------------------------------------------------- //

        #region Initialisation

        void Awake()
        {
            Main = this;
        }

        // ------------------------------------------------- //

        void Start()
        {

        }

        #endregion

        // ------------------------------------------------- //

        void Update()
        {

        }

        // ------------------------------------------------- //

        #region UI Elements

        // James
        // FPSController calls this whenever the player's health changes.
        // It will give you the value for the slider and whether or not
        // the player has been killed. Maybe you can put a skull and cross
        // bones or something to show that they died! Haha. Up to you.

        // NOTE: You can decrease player health by 10 by pressing "H".
        // NOTE2:You can respawn the player to full health by pressing "R".
        // Running into vehicles is an instant death. This is great for testing your
        // UI and all the death screens.

        /// <summary>Updates the player health bar on the UI.</summary>
        public void SetHealth(int health, bool playerIsDead)
        {

            // James
            // Work your magic here!


            // Update InGame UI
            // ...

        }

        /// <summary>Updates the ability bar on the UI.</summary>
        public void SetAbility(Ability ability)
        {
            var name = ability.GetDisplayName();
            var maximum = ability.MaxDuration;
            var current = ability.TimeRemaining;
            var isActive = ability.IsActive;

            // James
            // Work your magic here!

            // ...
        }


        #endregion

        // ------------------------------------------------- //

        #region Screens

        public void ShowPickupPrompt2D(string text)
        {
            if (pickupPrompt)
                pickupPrompt.GetComponent<PickupPrompt>().ShowPrompt(text);
            else
                Debug.LogError("[UI] No PickupPrompt assigned to UIManager.");

        }

        public void ShowPickupPrompt3D(string text, Vector3 objectPosition)
        {
            if (pickupPrompt3D)
                pickupPrompt3D.GetComponent<PickupPrompt3D>().ShowPrompt(text, objectPosition);
            else
                Debug.LogError("[UI] No PickupPrompt assigned to UIManager.");
        }

        public void HideAllPrompts()
        {
            //if (pickupPrompt)
            //    pickupPrompt.GetComponent<PickupPrompt>().HidePrompt();
            if (pickupPrompt3D)
                pickupPrompt3D.GetComponent<PickupPrompt3D>().HidePrompt();
        }

        // ------------------------------------------------- //

        public void ShowDeathScreen(bool makeActive)
        {
            if (deathScreen)
                deathScreen.SetActive(makeActive);
            else
                Debug.LogError("[UI] No PickupPrompt assigned to UIManager.");
            

        }


        // ------------------------------------------------- //

        #endregion

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}