using System;
using UnityEngine;

namespace SuperShooter
{

    public interface ICharacterController
    {

        bool characterEnabled { get; set; }

    }

    public interface IPlayerController : ICharacterController
    {

        IPlayerCharacter owner { get; }



    }

}