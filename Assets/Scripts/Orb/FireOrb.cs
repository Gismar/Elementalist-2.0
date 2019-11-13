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

        protected override float _mainCooldown => 1f;
        protected override float _specialCooldown => 4f;
        protected override float _attackDamage => 20;
        protected override float _specialDamage => 25;

        private int _fireballCount = 10;

        protected override void MainAttack()
        {
            _aimLine.enabled = false;
            (float rotation, _) = GetMouseInfo();
            float damage = _attackDamage; // * Orb Damage Modifier;
            float angle = 360f / _fireballCount;
            for (int i = 0; i < _fireballCount; i++)
                GetProjectileFromPool(ref _mainProjectilePool, _mainProjectilePrefab)
                    .Initialize(transform.position, 5f, rotation - (i * angle) - 90, damage, 0);
            OrbState = OrbState.Idling;
        }   

        protected override void SpecialAttack()
        {
            float damage = _specialDamage; // * Orb Damage Modifier;
            float duration = 2f; //debug;
            float size = 3f; //debug;

            GetProjectileFromPool(ref _specialProjectilePool, _specialProjectilePrefab)
                .Initialize(transform.position, duration, 0, damage, size);
        }

        protected override void UpdateAimLine()
        {
            _aimLine.enabled = true;
            float rotation = GetMouseInfo().rotation * Mathf.Deg2Rad;
            float x = Mathf.Cos(rotation) * 5f;
            float y = Mathf.Sin(rotation) * 5f;
            _aimLine.SetPositions(new Vector3[] {
                new Vector3(x, y, 0),
                new Vector3(0, 0, 0)
            });
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                enemy.TakeDamage(_attackDamage * Time.deltaTime);
        }
    }
}
