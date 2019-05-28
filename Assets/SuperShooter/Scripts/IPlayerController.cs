using System;
using UnityEngine;

namespace SuperShooter
{

    public interface ICharacterController
    {

        bool isRunning { get; set; }

    }

    public interface IPlayerController : ICharacterController
    {

        



    }

}