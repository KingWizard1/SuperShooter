using UnityEngine;

namespace SuperShooter
{
    public interface ICharacterEntity : IGameEntity
    {

        int health { get; }
        bool isDead { get; }

        void Kill();

        void TakeDamage(int amount, ICharacterEntity from);
    }

    public class CharacterEntity : GameEntity, ICharacterEntity
    {
        [Header("Health")]
        public int startHealth = 100;

        // ------------------------------------------------- //

        public int health { get; protected set; }

        public bool isDead { get; private set; }

        // ------------------------------------------------- //

        public void ResetHealth()
        {
            health = startHealth;
            isDead = false;
        }

        // ------------------------------------------------- //

        public void TakeDamage(int amount, ICharacterEntity from)
        {
            // Do nothing if we're already dead.
            if (isDead)
                return;

            // Do nothing if we're amazing right now
            if (Modifiers.HasFlag(EntityModifier.Invincible))
                return;

            // Deplete health by amount
            health -= amount;

            //
            Debug.Log($"Character '{name}' took {amount} damage from '{from.name}'.");

            //
            OnDamageTaken(amount, from);

            // Did we die?
            if (health <= 0)
                Kill();

        }

        // Override
        public virtual void OnDamageTaken(int amount, ICharacterEntity from) { }    // Do nothing. MUST be overriden.

        // ------------------------------------------------- //

        public void DealDamage(int amount, ICharacterEntity target)
        {
            // Is target already dead?
            if (target.isDead)
                return;

            // Deal damage
            target.TakeDamage(amount, this);

            // Run override
            OnDamageDealt(amount, target);

            if (target.isDead)
                OnTargetKilled(target);


        }

        // Override
        public virtual void OnDamageDealt(int amount, ICharacterEntity target) { }  // Do nothing. MUST be overriden.

        public virtual void OnTargetKilled(ICharacterEntity target) { } // Do nothing. MUST be overriden.

        // ------------------------------------------------- //

        public void Kill()
        {

            // Die
            isDead = true;

            //
            OnDeath();
            

            Debug.Log($"Character '{name}' has died.");
            
        }

        // Override
        public virtual void OnDeath() { }   // Do nothing. MUST be overriden.

        // ------------------------------------------------- //

    }

}