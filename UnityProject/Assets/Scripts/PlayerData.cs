﻿using UnityEngine;

[CreateAssetMenu(fileName = "玩家資料", menuName = "KID/玩家資效")]
public class PlayerData : ScriptableObject
{
    [Header("血量")]
    public float hp = 200;
}
