using System;
using UnityEngine;

namespace SuperShooter
{

    public interface ICanDie
    {

        void Kill();

        void TakeDamage(int damage, IGameEntity from);



    }
}