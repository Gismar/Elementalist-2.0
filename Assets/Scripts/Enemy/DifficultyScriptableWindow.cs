using UnityEditor;

namespace Elementalist.Config
{
    [CustomEditor(typeof(DifficultyScriptable))]
    public class DifficultyScriptableWindow : Editor
    {
        private float _minMultiplier = DifficultyScriptable.MinMultiplier;
        private float _maxMultiplier = DifficultyScriptable.MaxMultiplier;
        private float _minRate = DifficultyScriptable.MinRate;
        private float _maxRate = DifficultyScriptable.MaxRate;
        private float _minPlayer = DifficultyScriptable.MinPlayer;
        private float _maxPlayer = DifficultyScriptable.MaxPlayer;
        private float _minSummoning = DifficultyScriptable.MinSummoning;
        private float _maxSummoning = DifficultyScriptable.MaxSummoning;
        private float _minSpawn = DifficultyScriptable.MinSpawn;
        private float _maxSpawn = DifficultyScriptable.MaxSpawn;
        private int _minBreak = DifficultyScriptable.MinBreak;
        private int _maxBreak = DifficultyScriptable.MaxBreak;


        public override void OnInspectorGUI()
        {
            var _scriptable = (DifficultyScriptable)target;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Enemy Attributes", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            _scriptable.BaseHealthMultiplier = EditorGUILayout.Slider("Base Health Multiplier", _scriptable.BaseHealthMultiplier, _minMultiplier, _maxMultiplier);
            _scriptable.HealthChangeRate = EditorGUILayout.Slider("Health Change Rate", _scriptable.HealthChangeRate, _minRate, _maxRate);
            _scriptable.BaseDamageMultiplier = EditorGUILayout.Slider("Base Damage Multiplier", _scriptable.BaseDamageMultiplier, _minMultiplier, _maxMultiplier);
            _scriptable.DamageChangeRate = EditorGUILayout.Slider("Damage Change Rate", _scriptable.DamageChangeRate, _minRate, _maxRate);
            _scriptable.BaseSpeedMultiplier = EditorGUILayout.Slider("Base Speed Multiplier", _scriptable.BaseSpeedMultiplier, _minMultiplier, _maxMultiplier);
            _scriptable.SpeedChangeRate = EditorGUILayout.Slider("Speed Change Rate", _scriptable.SpeedChangeRate, _minRate, _maxRate);
            _scriptable.SummonMultiplier = EditorGUILayout.Slider("Summoning Multiplier", _scriptable.SummonMultiplier, _minSummoning, _maxSummoning);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Enemy Spawning", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            _scriptable.EnemySpawnRate = EditorGUILayout.Slider("Enemy Spawn Rate", _scriptable.EnemySpawnRate, _minSpawn, _maxSpawn);
            _scriptable.RoundBreakTimer = EditorGUILayout.IntSlider("Round Break Timer", _scriptable.RoundBreakTimer, _minBreak, _maxBreak);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Player Attributes", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            _scriptable.PlayerHealth = EditorGUILayout.Slider("Player Health Multiplier", _scriptable.PlayerHealth, _minPlayer, _maxPlayer);
            _scriptable.PlayerDamage = EditorGUILayout.Slider("Player Damage Multiplier", _scriptable.PlayerDamage, _minPlayer, _maxPlayer);
            _scriptable.PlayerSpeed = EditorGUILayout.Slider("Player Speed Multiplier", _scriptable.PlayerSpeed, _minPlayer, _maxPlayer);
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
                _scriptable.CalculatePointMultiplier();

            EditorGUILayout.FloatField("Point Multiplier", _scriptable.PointMultiplier);
        }
    }
}
