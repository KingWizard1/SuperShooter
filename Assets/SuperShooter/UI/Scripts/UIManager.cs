using System.Collections;
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
        public GameObject deathScreen;
        public TextMeshProUGUI contextText;

        private CrossHairUI crossHairUI;
        private PauseScreenUI pauseScreenUI;
        private PlayerStatusUI playerStatusUI;

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

            crossHairUI = crossHairSystem?.GetComponent<CrossHairUI>();
            pauseScreenUI = pauseScreenUI?.GetComponent<PauseScreenUI>();
            playerStatusUI = playerStatus?.GetComponent<PlayerStatusUI>();

            if (!crossHairUI) Debug.LogWarning($"There is no {nameof(CrossHairUI)} specified for the UI!");
            if (!pauseScreenUI) Debug.LogWarning($"There is no {nameof(PauseScreenUI)} specified for the UI!");
            if (!playerStatusUI) Debug.LogWarning($"There is no {nameof(PlayerStatusUI)} specified for the UI!");
        }

        // ------------------------------------------------- //

        void Start()
        {
            if (contextText)
                contextText.text = string.Empty;
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

            playerStatusUI?.SetStatus(player);

        }

        public void ShowPlayerDealtDamage(int amount, ICharacterEntity target)
        {
            var color = target.isDead ? Color.red : Color.white;
            crossHairUI?.ShowHitMarker(color);
        }

        public void ShowPlayerTookDamage()
        {
            playerStatusUI?.ShowDamage();
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

            playerStatusUI?.SetWeapon(weapon);


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

        public void ShowInteract(IInteractable interactable)
        {
            if (contextText)
            {

                var interactionKey = "E";
                contextText.text = $"[{interactionKey}] {interactable.GetInteractionString()}";

            }
            else
                Debug.LogError($"[UI] No {nameof(contextText)} assigned to {name}.");
        }

        public void HideInteract()
        {
            if (contextText)
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