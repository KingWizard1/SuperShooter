using System;
using UnityEngine;

namespace SuperShooter
{

    /// <summary>MUST be coupled with a MonoBehaviour.</summary>
    public interface IGameEntity
    {

        /// <summary><see cref="MonoBehaviour"/></summary>
        string name { get; }

        /// <summary><see cref="MonoBehaviour"/></summary>
        Transform transform { get; }

        //GameObject gameObject { get; }

        // ----------------------------------------------------- //

        //EntityModifier Modifiers { get; }

        //void SetVisibility(bool visible);

        //void MoveTo(Vector3 worldPos);


    }

    public abstract class GameEntity : MonoBehaviour, IGameEntity
    {

        //public EntityModifier Modifiers { get; }

        //public void SetVisibility(bool visible)
        //{
            
        //}

        //public void MoveTo(Vector3 worldPos)
        //{
            
        //}

    }

    // ----------------------------------------------------- //

    //public enum EntityModifier
    //{
    //    None = 0,
    //    Invincible = 5,
    //}

}