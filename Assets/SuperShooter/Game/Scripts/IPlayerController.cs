using System;
using UnityEngine;

namespace SuperShooter
{

    public interface ICharacterController : IGameEntity
    {

        ICharacterEntity characterEntity { get; }

        bool characterEnabled { get; set; }

    }

}