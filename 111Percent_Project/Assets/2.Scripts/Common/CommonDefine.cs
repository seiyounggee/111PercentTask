using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDefine
{
    public const string ProjectName = "PROJECT 111Percent";

    public const string OutGameScene = "OutGameScene";
    public const string InGameScene = "InGameScene";

    public const float Gravity = 9.8f;

    #region ANIMATION PARAMETERS
    public const string AnimIdle = "isIdle";
    public const string AnimRunning = "isRunning";
    public const string AnimDead = "isDead";
    public const string AnimSpeed = "animSpeed";

    #endregion

    //GameObject Name
    public const string GOName_MapObject = "MapObject";
    public const string GOName_Ground = "Ground";

    //Tag
    public const string TAG_Player = "Player";
    public const string TAG_Enemy = "Enemy";
    public const string TAG_Floor = "Floor";

    //Layer Name
    public const string LayerName_Default = "Default";
    public const string LayerName_Hidden = "Hidden";


    public enum Phase
    { 
        None = -1,
        Initialize,
        OutGame,
        InGameReady,
        InGame,
        InGameResult
    }

}