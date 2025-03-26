using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyExtensions
{
    public static void Initialize(this Enemy enemy, Transform player)
    {
        enemy.GetType().GetField("_player", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(enemy, player);
    }
}