using Elementalist.Enemies;
using Elementalist.Players;
using UnityEngine;
using System.Collections.Generic;

namespace Elementalist.Orbs
{
    public class MiniTidalWaveAugment : OrbAugment
    {
        public override float CoolDown => 1f;

        [SerializeField] private GameObject _tidalWavePrefab;
        [SerializeField, Range(1, 100)] private int _chance;
        private float _timer;
        private List<Projectile> _pool;

        protected override void Start()
        {
            base.Start();
            _pool = new List<Projectile>();
            _chance = 100;
        }

        protected override void OnCast(Vector2 position, IEnemy enemy, Player player)
        {
            if (Time.time > _timer)
            {
                if (Random.Range(0, 100) <= _chance)
                {
                    Vector2 direction = (position - (Vector2)player.transform.position).normalized;
                    float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    GetProjectileFromPool(ref _pool, _tidalWavePrefab)
                        .Initialize(player.transform.position, 1f, rotation, 25, 7f);
                }

                //_timer = CoolDown + Time.time;
            }
        }
    }
}