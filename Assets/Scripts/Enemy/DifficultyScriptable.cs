using UnityEngine;
using System.Linq;

namespace Elementalist.Config
{
    [CreateAssetMenu(fileName = "DifficultyConfig", menuName = "DifficultyConfig")]
    public class DifficultyScriptable : ScriptableObject
    {
        public const float MinMultiplier = 0.5f;
        public const float MaxMultiplier = 2f;
        public const float MinRate = 1f;
        public const float MaxRate = 1.5f;
        public const float MinPlayer = 0.5f;
        public const float MaxPlayer = 2f;
        public const float MinSummoning = 0f;
        public const float MaxSummoning = 2f;
        public const float MinSpawn = 0.5f;
        public const float MaxSpawn = 2f;
        public const int MinBreak = 0;
        public const int MaxBreak = 10;

        #region Enemy Attributes Data
        //Health Attributes
        private float _baseHealthMultiplier = 1f;
        public float BaseHealthMultiplier
        {
            get => _baseHealthMultiplier;
            set => _baseHealthMultiplier = Step(value, MinMultiplier, MaxMultiplier, 10f);
        }

        private float _healthChangeRate = 1.15f;
        public float HealthChangeRate
        {
            get => _healthChangeRate;
            set => _healthChangeRate = Step(value, MinRate, MaxRate, 20f);
        }

        //Damage Attributes
        private float _baseDamageMultiplier = 1f;
        public float BaseDamageMultiplier
        {
            get => _baseDamageMultiplier;
            set => _baseDamageMultiplier = Step(value, MinMultiplier, MaxMultiplier, 10f);
        }

        private float _damageChangeRate = 1.15f;
        public float DamageChangeRate
        {
            get => _damageChangeRate;
            set => _damageChangeRate = Step(value, MinRate, MaxRate, 20f);
        }

        //Speed Attributes
        private float _baseSpeedMultiplier = 1f;
        public float BaseSpeedMultiplier
        {
            get => _baseSpeedMultiplier;
            set => _baseSpeedMultiplier = Step(value, MinMultiplier, MaxMultiplier, 10f);
        }

        private float _speedChangeRate = 1.15f;
        public float SpeedChangeRate
        {
            get => _speedChangeRate;
            set => _speedChangeRate = Step(value, MinRate, MaxRate, 20f);
        }

        //Summoners Attributes

        private float _summonMultiplier = 1f;
        public float SummonMultiplier
        {
            get => _summonMultiplier;
            set => _summonMultiplier = Step(value, MinSummoning, MaxSummoning, 10f);
        }
        #endregion

        #region Spawning Data
        private int _roundBreakTimer = 5;
        public int RoundBreakTimer
        {
            get => _roundBreakTimer;
            set => _roundBreakTimer = Mathf.Clamp(value, MinBreak, MaxBreak);
        }

        private float _enemySpawnRate = 1f;
        public float EnemySpawnRate
        {
            get => _enemySpawnRate;
            set => _enemySpawnRate = Step(value, MinSpawn, MaxSpawn, 10f);
        }
        #endregion

        #region Player Data

        private float _playerHealth = 1f;
        public float PlayerHealth
        {
            get => _playerHealth;
            set => _playerHealth = Step(value, MinPlayer, MaxPlayer, 10f);
        }

        private float _playerSpeed = 1f;
        public float PlayerSpeed
        {
            get => _playerSpeed;
            set => _playerSpeed = Step(value, MinPlayer, MaxPlayer, 10f);
        }


        private float _playerDamage = 1f;
        public float PlayerDamage
        {
            get => _playerDamage;
            set => _playerDamage = Step(value, MinPlayer, MaxPlayer, 10f);
        }
        #endregion

        public float PointMultiplier => CalculatePointMultiplier();

        public float CalculatePointMultiplier()
        {
            float[] ratios = new float[]
            {
                //Enemy Attributes
                Map(BaseHealthMultiplier, MinMultiplier, MaxMultiplier),
                Map(BaseDamageMultiplier, MinMultiplier, MaxMultiplier),
                Map(BaseSpeedMultiplier, MinMultiplier, MaxMultiplier),
                Map(HealthChangeRate, MinRate, MaxRate),
                Map(DamageChangeRate, MinRate, MaxRate),
                Map(SpeedChangeRate, MinRate, MaxRate),
                Map(SummonMultiplier, MinSummoning, MaxSummoning),

                //Round Attributes
                Map(RoundBreakTimer, MaxBreak, MinBreak),
                Map(EnemySpawnRate, MinSpawn, MaxSpawn),

                //Player Attributes
                Map(PlayerHealth, MaxPlayer, MinPlayer),
                Map(PlayerSpeed, MaxPlayer, MinPlayer),
                Map(PlayerDamage, MaxPlayer, MinPlayer)
            };  

            return ratios.Average();
        }

        private static float Map(float value, float min, float max, float minMap = 0.5f, float maxMap = 2f) 
            => minMap + (value - min) * (maxMap - minMap) / (max - min);

        private static float Step(float value, float min, float max, float step)
        {
            int temp = Mathf.RoundToInt(value * step);
            return Mathf.Clamp(temp / step, min, max);
        }
    }
}
