using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;
using Elementalist.Players;
using Elementalist.Enemies;
using Elementalist.Orbs;

namespace Elementalist
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject _enemySpawnerPrefab;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _orbHandlerPrefab;
        [SerializeField] private GameObject[] _gridPrefabs;

        private void Start()
        {
            var map = Instantiate(_gridPrefabs[Random.Range(0, _gridPrefabs.Length)]).GetComponentsInChildren<Tilemap>().First();
            var player = Instantiate(_playerPrefab).GetComponent<Player>().Initialize(map.localBounds.center, 100f, 5f);
            var enemySpawner = Instantiate(_enemySpawnerPrefab).GetComponent<EnemySpawner>().Initialize(1, player.transform, map);
            var orbHandler = Instantiate(_orbHandlerPrefab).GetComponent<OrbHandler>().Initialize(player.transform);
        }
    }
}
