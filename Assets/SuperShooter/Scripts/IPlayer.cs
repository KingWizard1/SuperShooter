using System;
using UnityEngine;

namespace SuperShooter
{

    /// <summary>MUST be coupled with a MonoBehaviour.</summary>
    public interface IGameEntity
    {

        string name { get; set; }

        EntityModifier Modifiers { get; }

        void SetVisibility(bool visible);

        void MoveTo(Vector3 worldPos);

    }

    public abstract class GameEntity : MonoBehaviour, IGameEntity
    {

        public EntityModifier Modifiers { get; }

        public void SetVisibility(bool visible)
        {
            
        }

        public void MoveTo(Vector3 worldPos)
        {
            
        }

    }

    // ----------------------------------------------------- //

    public enum EntityModifier
    {
        None = 0,
        Invincible = 5,
    }

    public class CharacterEntity : GameEntity, ICanDie
    {

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

        public void TakeDamage(int amount, IGameEntity from)
        {
            // Do nothing if we're amazing right now
            if (Modifiers.HasFlag(EntityModifier.Invincible))
                return;

            // Deplete health by amount
            health -= amount;

            //
            Debug.Log($"Entity '{name}' took {amount} damage from '{from.name}'.");

            //
            OnDamageTaken();

            // Did we die?
            if (health <= 0)
                Kill();

        }

        // Override
        public virtual void OnDamageTaken() { }

        // ------------------------------------------------- //

        public void Kill()
        {

            // Die
            isDead = true;

            //
            OnDeath();
            

            Debug.Log($"Entity '{name}' has died.");
            
        }

        // Overrides
        public virtual void OnDeath() { }

        // ------------------------------------------------- //

    }

}