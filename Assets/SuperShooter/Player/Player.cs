using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    [RequireComponent(typeof(FPSController))]
    public class Player : CharacterEntity
    {

        [Header("Progression")]
        public int XP;
        public int XPRequiredToLevel;
        public int XPLevel;


        // ------------------------------------------------- //

        private FPSController controller;


        // ------------------------------------------------- //


        // ------------------------------------------------- //

        private void Awake()
        {
            // Get the first player controller we come across.
            controller = GetComponentInChildren<FPSController>();

        }

        // ------------------------------------------------- //

        private void Start()
        {
            health = startHealth;

        }

        // ------------------------------------------------- //

        private void Update()
        {

            // Do nothing if dead.
            if (isDead) {
#if DEBUG
                // RESPAWN CHEAT.
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
                    Respawn();
#endif
                // Bail
                return;
            }


            // Usual suspects

            UpdateHealth();


        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

        #region Player Health

        private void UpdateHealth()
        {
            if (UIManager.Exists)
                UIManager.Main.SetHealth(health, startHealth, isDead);

        }

        // ------------------------------------------------- //

        public override void OnDamageTaken()
        {

            // Show damage indicator on the UI
            if (UIManager.Exists)
                UIManager.Main.ShowDamage();

        }

        public override void OnDeath()
        {
            // Disable the FPSController's character controller.
            controller.characterEnabled = false;

            // Show kill screen
            if (UIManager.Exists)
                UIManager.Main.ShowDeathScreen(true);

        }

        // ------------------------------------------------- //

        private void Respawn()
        {

            // Reset health and enable controller
            ResetHealth();
            controller.characterEnabled = true;

            // Reset UI
            if (UIManager.Exists)
                UIManager.Main.ShowDeathScreen(false);
        }

        #endregion

        // ------------------------------------------------- //

        #region Player Progression

        private void GiveXP(int amount)
        {

        }

        #endregion

        // ------------------------------------------------- //


        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}