using System;
using UnityEngine;

namespace SuperShooter
{

    public interface ICharacterController
    {

        ICharacterEntity owner { get; }

        bool characterEnabled { get; set; }

    }

}