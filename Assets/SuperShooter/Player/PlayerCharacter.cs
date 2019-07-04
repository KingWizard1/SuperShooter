using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperShooter
{
    public interface IPlayerCharacter : ICharacterEntity
    {
        ICharacterController controller { get; }
    }

    [RequireComponent(typeof(FPSController))]
    public class PlayerCharacter : CharacterEntity
    {

        [Header("Progression")]
        public int currentXP = 0;
        public int currentXPLevel = 1;
        private int currentXPScaleValue = 0;
        public int currentXPRequiredToLevel { get; private set; }
        public int initialXPRequiredToLevel = 100;
        public int lastXPRequiredToLevel { get; private set; }
        public float XPLevelScaleFactor = 1.25f;

        [Header("Abilities")]
        public float dashSpeed = 25f;
        public float dashDuration = 3f;

        [Header("Targeting")]
        public LayerMask targetLayer;
        public CharacterEntity currentTarget;

        // ------------------------------------------------- //

        // References
        private FPSController controller;

        // Abilities
        private bool isDashing;
        private float dashTimer;


        // ------------------------------------------------- //


        // ------------------------------------------------- //

        private void Reset()
        {
            // Force default value.
            type = TargetType.Player;

        }

        private void Awake()
        {

            // Get the first player controller we come across.
            controller = GetComponentInChildren<FPSController>();

            // Hide the character model from the camera.
            if (characterModel == null)
                Debug.LogWarning($"{name} does not have a {nameof(characterModel)} assigned to its {nameof(PlayerCharacter)} component.");
            else
                characterModel.GetComponent<Renderer>().enabled = false;
        }

        // ------------------------------------------------- //

        private void Start()
        {
            // Set controller owner
            controller.characterEntity = this;

            // Setup
            ResetHealth();
            ResetXP();
        }

        // ------------------------------------------------- //

        private void Update()
        {
            // LOSE condition
            controller.enabled = !GameMaster.Main.isGameOver;
            if (GameMaster.Main.isGameOver)
                return;


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


            UpdateAbilities();
            UpdateTarget();


        }

        private void LateUpdate()
        {
            // Must happen last to ensure the UI is accurate.
            UpdateUI();
        }

        // ------------------------------------------------- //

        #region Update() Methods

        // ------------------------------------------------- //

        private void UpdateUI()
        {
            UIManager.Main?.SetPlayerStatus(this);
            UIManager.Main?.SetPlayerWeaponStatus(controller.currentWeapon);
            UIManager.Main?.SetPlayerProgression(this);
            UIManager.Main?.SetPlayerTarget(currentTarget);
        }

        // ------------------------------------------------- //

        private void UpdateAbilities()
        {

            controller.MoveSpeedMod = Input.GetKey(KeyCode.F) ? dashSpeed : 0;


        }

        // ------------------------------------------------- //

        private void UpdateTarget()
        {

            Ray crossHairRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(crossHairRay, out RaycastHit hit, Mathf.Infinity, targetLayer)) // or 1000f? Hmm.
            {
                // Did we hit a character?
                var go = hit.transform.gameObject;
                var target = go.GetComponentInParent<CharacterEntity>();
                
                if (target == null) {
                    // No character, no target.
                    currentTarget = null;
                    return;
                }


                // Set target if its a targetable type.
                if (target.type != TargetType.None && !target.isDead)
                    currentTarget = target;
                else
                    currentTarget = null;

            }
            else
                currentTarget = null;

        }

        #endregion

        // ------------------------------------------------- //

        #region Event Overrides

        public override void BackFromTheDead()
        {
            GameMaster.Main?.CancelGameOver();
        }

        public override void OnDamageDealt(int amount, ICharacterEntity target)
        {

            // Show damage indicator on the UI
            UIManager.Main?.ShowPlayerDealtDamage(amount, target);

            // Give XP for dealing damage.
            // 1 XP should be given for each damage event, multiplied by the game phase.
            var xp = 1 * GameMaster.Main?.gamePhase ?? 0;
            GiveXP(xp);

            // Show XP earned
            //UIManager.Main?.progressionUI?.ShowBulletHit(xp);

            // WORKS, BUT WRONG.
            // It's not being called normally because its the WEAPON that is doing damage.
            if (target.isDead)
                OnTargetKilled(target);

        }

        public override void OnDamageTaken(int amount, ICharacterEntity from)
        {
            // We have the option here to display
            // how much damage was taken, and from whom.

            // Show damage indicator on the UI
            UIManager.Main?.ShowPlayerTookDamage();

        }

        public override void OnDeath()
        {
            // Disable the FPSController's character controller.
            controller.characterEnabled = false;

            // Game over!
            GameMaster.Main?.GameOver("YOU DIED.", "You were no match against the invaders.");


        }

        #endregion

        // ------------------------------------------------- //

        #region Player Progression

        public sealed override void OnTargetKilled(ICharacterEntity target)
        {

            if (target is EnemyCharacter enemy)
            {
                GiveXP(enemy.XPValue);

                UIManager.Main?.progressionUI?.ShowEnemyKilled(enemy.XPValue);

            }

        }

        private void ResetXP()
        {
            currentXP = 0;
            currentXPScaleValue = initialXPRequiredToLevel;
            currentXPRequiredToLevel = initialXPRequiredToLevel;
        }

        public void GiveXP(int amount) // amount = the amount of XP the enemy gives to you
        {
            // Add experience by amount
            currentXP += amount;

            // Enough to level up?
            if (currentXP >= currentXPRequiredToLevel)
                LevelUp();

            // Show the XP bar. It's hidden by default. We want to show it
            // when the player earns their first XP point.
            UIManager.Main?.progressionUI?.ShowHideXPBar(true);

        }

        public void LevelUp() // is called when you level up
        {
            // Increment XP Level.
            currentXPLevel++;

            Debug.Log($"Player strength has grown to Level {currentXPLevel}!");

            // Increment next level's XP requirement
            // First, calc the level's scaling value.
            // Then, add that value to the currently required amount of XP.
            // The result equals the TotalXPRequired, across all earned levels. Growing by the scale factor.
            var scaleValue = currentXPScaleValue * XPLevelScaleFactor;
            var xpRequired = currentXPRequiredToLevel + Mathf.RoundToInt(scaleValue);

            // Set the new requirement.
            lastXPRequiredToLevel = currentXPRequiredToLevel;
            currentXPRequiredToLevel = xpRequired;


            Debug.Log($"Current XP: {currentXP}\tRequired For Next Level: {xpRequired}");


        }

        #endregion

        // ------------------------------------------------- //

        #region Player Abilities

        public void AddAmmo(int amount)
        {

            foreach (var weapon in controller.weapons)
            {
                weapon.AddAmmo(amount);
            }

        }

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