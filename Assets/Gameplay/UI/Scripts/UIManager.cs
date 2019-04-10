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
        public GameObject healthUI;
        public GameObject pickupPrompt;
        public GameObject pickupPrompt3D;
        public GameObject crossHairSystem;
        public GameObject deathScreen;

        public TextMeshProUGUI ammoText;
        public TextMeshProUGUI weaponText;

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

            // Reset text
            ammoText.text = string.Empty;
            weaponText.text = string.Empty;

        }

        #endregion

        // ------------------------------------------------- //

        void Update()
        {
         
        }

        // ------------------------------------------------- //

        #region UI Elements

        /// <summary>Updates the player health bar on the UI.</summary>
        public void SetHealth(int health, int maxHealth, bool isDead)
        {

            // Update InGame UI

            var playerHealth = healthUI.GetComponent<PlayerHealth>();
            playerHealth.SetHealth(health, maxHealth, isDead);


        }

        public void ShowDamage()
        {
            var playerHealth = healthUI.GetComponent<PlayerHealth>();
            playerHealth.ShowDamage();
        }

        /// <summary>Updates the ability bar on the UI.</summary>
        public void SetAbility(Ability ability)
        {
            var name = ability.GetDisplayName();
            var maximum = ability.MaxDuration;
            var current = ability.TimeRemaining;
            var isActive = ability.IsActive;

            // TODO

        }

        public void SetWeaponStatus(Weapon weapon)
        {

            if (weapon == null)
            {
                // There is no weapon currently equipped by the player.
                // ..
            }
            else
            {
                // Get values
                var currentAmmoInClip = weapon.ammo;
                var maxAmmoPerClip = weapon.maxAmmoPerClip;
                var totalAmmoLeft = (weapon.maxAmmoPerClip * weapon.clips);

                var weaponName = weapon.GetDisplayName();

                // Update UI

                ammoText.text = string.Format("{0}/{1} + {2}", 
                    currentAmmoInClip, maxAmmoPerClip, totalAmmoLeft);

                weaponText.text = weaponName;

            }

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