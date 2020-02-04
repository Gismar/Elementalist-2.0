using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Config
{
    public struct Data
    {
        public float ChangeRate { get; }
        public float BaseMultiplier { get; }
        public float TotalMultiplier { get; private set; }
        public Data(float changeRate, float baseMultiplier)
        {
            ChangeRate = changeRate;
            BaseMultiplier = baseMultiplier;
            TotalMultiplier = 1f;
        }

        public void Update() => TotalMultiplier *= 1 + ChangeRate;
    }

    public struct EnemyDifficulty
    {
        public Data EnemyHealth { get; }
        public Data EnemyDamage { get; }
        public Data EnemySpeed { get; }
        public float EnemySummonMultiplier { get; }
        public float PointMultiplier { get; }

        public EnemyDifficulty(DifficultyScriptable scriptable)
        {
            EnemyHealth = new Data(scriptable.HealthChangeRate, scriptable.BaseHealthMultiplier);
            EnemyDamage = new Data(scriptable.DamageChangeRate, scriptable.BaseDamageMultiplier);
            EnemySpeed = new Data(scriptable.SpeedChangeRate, scriptable.BaseSpeedMultiplier);
            EnemySummonMultiplier = scriptable.SummonMultiplier;
            PointMultiplier = scriptable.PointMultiplier;
        }

        public void Update()
        {
            EnemyHealth.Update();
            EnemyDamage.Update();
            EnemySpeed.Update();
        }
    }

    public struct SpawningDifficulty
    {
        public float EnemyAmountMultiplier { get; }
        public float RoundPauseDuration { get; }
        public SpawningDifficulty(DifficultyScriptable scriptable)
        {
            EnemyAmountMultiplier = scriptable.EnemySpawnRate;
            RoundPauseDuration = scriptable.RoundBreakTimer;
        }
    }

    public struct PlayerDifficulty
    {
        public float Health { get; }
        public float Damage { get; }
        public float Speed { get; }
        public PlayerDifficulty(DifficultyScriptable scriptable)
        {
            Health = scriptable.PlayerHealth;
            Damage = scriptable.PlayerDamage;
            Speed = scriptable.PlayerSpeed;
        }
    }
}