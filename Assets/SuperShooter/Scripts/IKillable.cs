using System;
using UnityEngine;

namespace SuperShooter
{

    public interface IKillable
    {

        void Kill();

        void TakeDamage(int damage);



    }
}