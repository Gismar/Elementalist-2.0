using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using Elementalist.Config;

namespace Elementalist.Enemies
{
    public enum EnemyType
    {
        Basic,
        Tank,
        Speedster,
        Popper,
        Popperlings
    }

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private EnemyScriptable[] _enemyScriptables;
        [SerializeField] private Gradient _tierColorGradient;
        [SerializeField] private Sprite[] _tierSprites;

        private Dictionary<EnemyType, List<EnemyBase>> _enemyPools;
        private Dictionary<int, Vector2> _borderLocations;
        private Transform _player;
        private EnemyDifficulty _enemyDifficulty;
        private SpawningDifficulty _spawningDifficulty;
        private bool _isPaused;
        private readonly float _initialSpawnTime = 5f;
        private float _timeOffset;
        private int _round;
        private int _wave;

        // Custom Constructor
        public EnemySpawner Initialize(EnemyDifficulty enemyDifficulty, SpawningDifficulty spawningDifficulty, Transform player, Tilemap map = null, int startingTier = 0)
        {
            _enemyDifficulty = enemyDifficulty;
            _spawningDifficulty = spawningDifficulty;
            _player = player;
            _enemyPools = new Dictionary<EnemyType, List<EnemyBase>>();
            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
                _enemyPools.Add(enemyType, new List<EnemyBase>());

            InitializeSpawnPoints(map);
            _timeOffset = startingTier * 10f + startingTier + startingTier / 9f;

            StopAllCoroutines();
            StartCoroutine(Round(_initialSpawnTime));
            return this;
        }

        // Fills the dictionary with all available tiles on the edge of a rectangle
        private void InitializeSpawnPoints(Tilemap map = null)
        {
            map = map ?? GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();
            map.CompressBounds();
            _borderLocations = new Dictionary<int, Vector2>();
            for (int i = 0; i < 360; i++)
            {
                float angle = i * Mathf.Deg2Rad;
                float cons = Mathf.Abs(Mathf.Cos(angle)) * Mathf.Cos(angle);
                float sins = Mathf.Abs(Mathf.Sin(angle)) * Mathf.Sin(angle);
                var position = new Vector3(Mathf.RoundToInt(map.size.x * (cons + sins) / 2f), Mathf.RoundToInt(map.size.y * (cons - sins) / 2f ));
                _borderLocations.Add(i, position + map.localBounds.center);
            }
        }

        private IEnumerator Round(float timer)
        {
            yield return new WaitUntil(() => timer <= Time.timeSinceLevelLoad);
            _round++;
            int waveCount = Mathf.RoundToInt(Mathf.Log10(_round * _round + 100));
            float enemyAmountMultiplier = _spawningDifficulty.EnemyAmountMultiplier;
            for (_wave = 0; _wave < waveCount; _wave++)
            {
                int enemyCount = Mathf.RoundToInt(Mathf.Log10(_round * _round * enemyAmountMultiplier) + 20 * enemyAmountMultiplier);
                var enemies = new List<EnemyBase>();

                for (int i = 0; i < enemyCount; i++)
                {
                    var enemyType = (EnemyType)Random.Range(0, _enemyScriptables.Where(e => e.SpawnTime <= Time.timeSinceLevelLoad).ToArray().Length);
                    enemies.Add(SpawnEnemy(enemyType));
                    yield return new WaitForSeconds(enemyCount / 30f);
                }
                yield return new WaitUntil(() => enemies.All(e => e.CurrentHealth <= 0f));
                Debug.Log("Wave Over");
            }
            _enemyDifficulty.Update();
            yield return StartCoroutine(Round(Time.timeSinceLevelLoad + _spawningDifficulty.RoundPauseDuration));
        }

        private EnemyBase SpawnEnemy(EnemyType enemyType)
        {
            return GetEnemy(enemyType).Initialize(
                enemyInfo: new EnemyInfo(_enemyDifficulty, _enemyScriptables[(int)enemyType], _round),
                target: _player,
                position: _borderLocations[Random.Range(0, _borderLocations.Count)],
                tier: GetTierInfo(_round % (_tierSprites.Length * _tierColorGradient.colorKeys.Length))
            );
        }

        // Recursive coroutine that spawns enemies
        //private IEnumerator SpawnEnemy(float spawnTimer)
        //{
        //    yield return new WaitUntil(() => spawnTimer <= Time.timeSinceLevelLoad);

        //    // Calculations
        //    float time = Time.timeSinceLevelLoad;
        //    _enemyDifficulty.Update();
        //    int tier = Mathf.FloorToInt((time + _timeOffset)/ 10f - (time + _timeOffset) / 100f);
        //    var enemyType = (EnemyType)Random.Range(0, _enemyScriptables.Where(e => e.SpawnTime <= Time.timeSinceLevelLoad).ToArray().Length);
        //    spawnTimer = time + 1f;// + _timeOffset + 1f/multiplier + (5 - _difficulty/2f);

        //    // Enemy Setup
        //    GetEnemy(enemyType).Initialize(
        //        enemyInfo: new EnemyInfo(_enemyDifficulty, _enemyScriptables[(int)enemyType], _round),
        //        target: _player,
        //        position: _borderLocations[Random.Range(0, _borderLocations.Count)],
        //        tier: GetTierInfo(tier % (_tierSprites.Length * _tierColorGradient.colorKeys.Length))
        //    );

        //    yield return StartCoroutine(SpawnEnemy(spawnTimer));
        //}

        // Gets the appropriate sprite and color based on tier
        private (Color color, Sprite sprite) GetTierInfo(int tier)
        {
            int keyAmount = _tierColorGradient.colorKeys.Length;
            return (_tierColorGradient.Evaluate((tier % keyAmount) / (float)keyAmount), 
                _tierSprites[(tier / _tierSprites.Length) % _tierSprites.Length]);
        }
        
        // Retrieves an EnemyBase from the enemy pool and upon failure will instantiate a new one and add it to the pool.
        private EnemyBase GetEnemy (EnemyType enemyType)
        {
            var enemy = _enemyPools[enemyType].FirstOrDefault(e => !e.gameObject.activeInHierarchy);
            if (enemy == default(EnemyBase))
            {
                enemy = Instantiate(_enemyPrefabs[(int)enemyType], transform).GetComponent<EnemyBase>();
                _enemyPools[enemyType].Add(enemy);
            }
            return enemy;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            foreach (Vector3 position in _borderLocations.Values)
                Gizmos.DrawCube(position, Vector3.one);
        }
    }
}
