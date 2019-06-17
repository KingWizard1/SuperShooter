﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SuperShooter
{

    public class UIManager : MonoBehaviour
    {

        [Header("UI Elements")]
        public GameObject playerStatus;
        public GameObject pickupPrompt3D;
        public GameObject crossHairSystem;
        public GameObject progressionSystem;
        public GameObject deathScreen;

        public TextMeshProUGUI contextText;

        public CrossHairUI targetUI;
        public PauseScreenUI pauseUI;
        public PlayerStatusUI statusUI;
        public PlayerProgressionUI progressionUI;

        // ------------------------------------------------- //

        public CrossHairUI CrossHair { get { return crossHairSystem.GetComponent<CrossHairUI>(); } }

        // ------------------------------------------------- //

        public static UIManager Main { get; private set; }

        public static bool Exists => Main != null;

        // ------------------------------------------------- //

        #region Initialisation

        void Awake()
        {
            Main = this;

            targetUI = crossHairSystem?.GetComponent<CrossHairUI>();
            pauseUI = pauseUI?.GetComponent<PauseScreenUI>();
            statusUI = playerStatus?.GetComponent<PlayerStatusUI>();

            if (!targetUI) Debug.LogWarning($"There is no {nameof(CrossHairUI)} specified for the UI!");
            if (!pauseUI) Debug.LogWarning($"There is no {nameof(PauseScreenUI)} specified for the UI!");
            if (!statusUI) Debug.LogWarning($"There is no {nameof(PlayerStatusUI)} specified for the UI!");
        }

        // ------------------------------------------------- //

        void Start()
        {
            SetActionText(string.Empty);
        }

        #endregion

        // ------------------------------------------------- //

        void Update()
        {
         
        }

        // ------------------------------------------------- //

        #region UI Elements

        public void SetPlayerStatus(PlayerCharacter player)
        {

            statusUI?.SetStatus(player);

        }

        public void ShowPlayerDealtDamage(int amount, ICharacterEntity target)
        {
            var color = target.isDead ? Color.red : Color.white;
            targetUI?.ShowHitMarker(color);
        }

        public void ShowPlayerTookDamage()
        {
            statusUI?.ShowDamage();
        }


        /// <summary>Updates the ability bar on the UI.</summary>
        public void SetPlayerAbilityStatus(Ability ability)
        {
            var name = ability.GetDisplayName();
            var maximum = ability.MaxDuration;
            var current = ability.TimeRemaining;
            var isActive = ability.IsActive;

            // TODO

        }

        public void SetPlayerWeaponStatus(Weapon weapon)
        {

            statusUI?.SetWeapon(weapon);


        }

        public void SetPlayerProgression(PlayerCharacter player)
        {
            progressionUI?.SetXPBar(player);
        }

        #endregion

        // ------------------------------------------------- //

        #region Screens

        //public void ShowPickupPrompt2D(string text)
        //{
        //    if (pickupPrompt)
        //        pickupPrompt.GetComponent<PickupPrompt>().ShowPrompt(text);
        //    else
        //        Debug.LogError("[UI] No PickupPrompt assigned to UIManager.");

        //}

        //public void ShowPickupPrompt3D(string text, Vector3 objectPosition)
        //{
        //    if (pickupPrompt3D)
        //        pickupPrompt3D.GetComponent<PickupPrompt3D>().ShowPrompt(text, objectPosition);
        //    else
        //        Debug.LogError("[UI] No PickupPrompt assigned to UIManager.");
        //}

        //public void HideAllPrompts()
        //{
        //    //if (pickupPrompt)
        //    //    pickupPrompt.GetComponent<PickupPrompt>().HidePrompt();
        //    if (pickupPrompt3D)
        //        pickupPrompt3D.GetComponent<PickupPrompt3D>().HidePrompt();
        //}

        /// <summary>Get or set the player's action/interaction text prompt.</summary>
        public void SetActionText(string text, bool useTheForce = false)
        {
            if (contextText)
                contextText.text = text;
            this.useTheForce = useTheForce;
        }

        private bool useTheForce;

        public void ShowActionText(IInteractable interactable, bool isWithinRange)
        {
            if (contextText)
            {

                var interactionKey = "E";

                contextText.text = $"[{interactionKey}] {interactable.GetInteractionString()}";

                contextText.color = isWithinRange ? Color.white : Color.grey;

            }
            else
                Debug.LogError($"[UI] No {nameof(contextText)} is assigned to {name}.");
        }

        public void HideActionText()
        {
            if (contextText && !useTheForce)
                contextText.text = string.Empty;
        }

        // ------------------------------------------------- //

        public void ShowDeathScreen(bool makeActive)
        {
            if (deathScreen)
                deathScreen.SetActive(makeActive);
            else
                Debug.LogError($"[UI] No {nameof(deathScreen)} assigned to {name}.");
            

        }


        // ------------------------------------------------- //

        #endregion

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}