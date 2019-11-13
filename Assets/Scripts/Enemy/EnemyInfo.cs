using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Enemies
{
    public struct EnemyInfo
    {
        public float Multiplier { get; }
        public float MaxHealth { get; }
        public float Speed { get; }
        public float PointValue { get; }
        public int SpawnAmount { get; }
        public EnemyScriptable EnemyScriptable { get; }
        // Global Data Handler;

        /// <summary>
        /// Simple struct that calculates a bit of data on the backend
        /// </summary>
        public EnemyInfo(float multiplier, EnemyScriptable enemyScriptable)
        {
            Multiplier = multiplier;
            MaxHealth = enemyScriptable.BaseHealth * multiplier;
            Speed = enemyScriptable.BaseSpeed;
            PointValue = enemyScriptable.PointValue + enemyScriptable.PointValue * Mathf.Log10(multiplier + 0.1f);
            SpawnAmount = enemyScriptable.SpawnAmount;
            EnemyScriptable = enemyScriptable;
        }
    }
}
