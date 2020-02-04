using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Elementalist.Enemies;
using Elementalist.Players;
using System.Linq;

namespace Elementalist.Orbs
{
    public abstract class OrbAugment : MonoBehaviour
    {
        public Action<Vector2, IEnemy, Player> Cast;
        public abstract float CoolDown { get; }

        private Transform _projectilesTransform;

        protected virtual void Start()
        {
            Cast = OnCast;
            _projectilesTransform = GameObject.FindGameObjectWithTag("Projectile Folder").transform;
        }

        /// <summary>
        /// Method called by event.
        /// </summary>
        /// <param name="position">Orb's postion</param>
        /// <param name="enemy">Enemy info incase of collision. </param>
        /// <param name="player">Player's class for info. </param>
        protected abstract void OnCast(Vector2 position, IEnemy enemy, Player player);

        /// <summary>
        /// Get's an unused projectile from the pool. If none exist, will make a new projectile and add it to the list.
        /// </summary>
        /// <param name="list">The pool to which the projectile will be pooled from. </param>
        /// <param name="prefab">The projectile prefab to instantiate in case of no objects available in the pool.</param>
        protected Projectile GetProjectileFromPool(ref List<Projectile> list, GameObject prefab)
        {
            Projectile projectile = list.FirstOrDefault(p => !p.gameObject.activeInHierarchy);
            if (projectile == default)
            {
                projectile = Instantiate(prefab, _projectilesTransform).GetComponent<Projectile>();
                list.Add(projectile);
            }
            return projectile;
        }
    }
}