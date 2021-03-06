﻿using UnityEngine;

[CreateAssetMenu(fileName = "玩家資料", menuName = "KID/玩家資效")]
public class PlayerData : ScriptableObject
{
    [Header("血量")]
    public float hp = 200;
    [Header("最大血量 : 不會改變")]
    public float hpMax = 200;
    [Header("冷卻"), Range(0, 3)]
    public float cd = 2.5f;
}
