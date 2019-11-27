using Elementalist.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class FireOrb : OrbBase
    {
        [SerializeField] private LineRenderer _aimLine;

        protected  float _mainCooldown => 1f;
        protected  float _specialCooldown => 4f;
        protected  float _attackDamage => 20;
        protected  float _specialDamage => 25;

        private int _fireballCount = 10;

        protected  void MainAttack()
        {
            
        }   

        protected  void SpecialAttack()
        {
            float damage = _specialDamage; // * Orb Damage Modifier;
            float duration = 2f; //debug;
            float size = 3f; //debug;

            //GetProjectileFromPool(ref _specialProjectilePool, _specialProjectilePrefab)
            //    .Initialize(transform.position, duration, 0, damage, size);
        }

        protected  void UpdateAimLine()
        {
            
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                enemy.TakeDamage(_attackDamage * Time.deltaTime);
        }
    }
}
