using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    [RequireComponent(typeof(FPSController))]
    public class PlayerCharacter : CharacterEntity
    {

        [Header("Progression")]
        public int curXP = 0;
        public int XPRequiredToLevel = 25;
        public int XPLevel = 1;

        [Header("Dash")]
        public float dashSpeed = 25f;
        public float dashDuration = 3f;

        // ------------------------------------------------- //

        // References
        private FPSController controller;

        // Dashing
        private bool isDashing;
        private float dashTimer;


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
            ResetHealth();
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
            UpdateAbilities();

        }

        // ------------------------------------------------- //

        #region Update() Methods

        private void UpdateHealth()
        {
            if (UIManager.Exists)
                UIManager.Main.SetHealth(health, maxHealth, isDead);

        }

        private void UpdateAbilities()
        {

            controller.MoveSpeedMod = Input.GetKey(KeyCode.E) ? dashSpeed : 0;


        }

        #endregion

        // ------------------------------------------------- //

        #region Event Overrides

        public override void BackFromTheDead()
        {
            // Reset UI
            if (UIManager.Exists)
                UIManager.Main.ShowDeathScreen(false);
        }

        public override void OnDamageDealt(int amount, ICharacterEntity target)
        {
            
        }

        public override void OnDamageTaken(int amount, ICharacterEntity from)
        {
            // We have the option here to display
            // how much damage was taken, and from whom.

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

        #endregion

        // ------------------------------------------------- //

        #region Player Progression

        public sealed override void OnTargetKilled(ICharacterEntity target)
        {

            if (target is EnemyCharacter enemy)
            {
                GiveXP(enemy.XPValue);
            }

        }

        private void GiveXP(int amount) // amount = the amount of XP the enemy gives to you
        {
            curXP += amount; // plussing current XP by the amount of XP the enemy gives

            if (curXP >= XPRequiredToLevel) // if you have enough XP to level up
            {
                LevelUp(); // level up
                curXP = 0; // sets XP back to 0
                Debug.Log("LeveledUp");
            }
        }

        private void LevelUp() // is called when you level up
        {
            XPLevel++; // incrimemts your XP level by 1
            Debug.Log(XPLevel);
        }

        #endregion

        // ------------------------------------------------- //

        #region Player Abilities

        public void Dash()
        {
            // reset timer to full duration. it will count down in Update()
            dashTimer = dashDuration;
            isDashing = true;
        }


        public void Respawn()
        {

            // Reset health and enable controller
            Reincarnate();
            controller.characterEnabled = true;

        }


        #endregion

        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }

}