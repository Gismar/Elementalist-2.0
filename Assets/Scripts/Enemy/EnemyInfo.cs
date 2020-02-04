using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elementalist.Config;

namespace Elementalist.Enemies
{
    public struct EnemyInfo
    {
        public float MaxHealth { get; }
        public float Speed { get; }
        public float Damage { get; }
        public float PointValue { get; }
        public int SpawnAmount { get; }
        public EnemyScriptable EnemyScriptable { get; }
        public int Round { get; }
        public EnemyDifficulty EnemyDifficulty { get; }
        // Global Data Handler;

        /// <summary>
        /// Simple struct that calculates a bit of data on the backend
        /// </summary>
        public EnemyInfo(EnemyDifficulty enemyDifficulty, EnemyScriptable enemyScriptable, int round)
        {
            MaxHealth = enemyScriptable.BaseHealth * enemyDifficulty.EnemyHealth.TotalMultiplier;
            Speed = enemyScriptable.BaseSpeed * enemyDifficulty.EnemySpeed.TotalMultiplier;
            Damage = enemyScriptable.BaseDamage * enemyDifficulty.EnemyDamage.TotalMultiplier;
            PointValue = enemyScriptable.PointValue * Mathf.Log10(round * enemyDifficulty.PointMultiplier + 10f) * enemyDifficulty.PointMultiplier;
            SpawnAmount = enemyScriptable.SpawnAmount;
            EnemyScriptable = enemyScriptable;
            Round = round;
            EnemyDifficulty = enemyDifficulty;
        }
    }
}
