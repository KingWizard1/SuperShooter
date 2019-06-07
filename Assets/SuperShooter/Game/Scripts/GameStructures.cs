using System;
using UnityEngine;

namespace SuperShooter
{

    public enum NetworkMode
    {
        SinglePlayer = 0,
        MultiplayerHost = 1,
        MultiplayerClient = 2
    }

    public enum GameMode
    {
        Slayer = 0,
        TeamDeathmatch = 1,
        CaptureTheFlag = 2
    }

}