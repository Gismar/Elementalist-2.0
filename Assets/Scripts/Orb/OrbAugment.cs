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
        protected abstract void OnCast(Vector2 position, IEnemy enemy, Player player);

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