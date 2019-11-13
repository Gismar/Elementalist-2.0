using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class EarthOrb : OrbBase
    {
        [SerializeField] private LineRenderer _aimLine;

        protected override float _mainCooldown => 3f;
        protected override float _specialCooldown => 5f;
        protected override float _attackDamage => 0;
        protected override float _specialDamage => 25f;

        protected override void MainAttack()
        {
            _aimLine.enabled = false;
            _animator.SetTrigger("Attack");
            OrbState = OrbState.Idling;

            float duration = 3f; //debug;
            float length = 3f; //debug;

            GetProjectileFromPool(ref _mainProjectilePool, _mainProjectilePrefab)
                .Initialize(transform.position, duration, GetMouseInfo().rotation - 90, 0, length);
        }

        protected override void SpecialAttack()
        {
            _animator.SetTrigger("Attack");

            float size = 1; //debug;
            float duration = 3f; //debug;
            float strength = _specialDamage / 100f;

            GetProjectileFromPool(ref _specialProjectilePool, _specialProjectilePrefab)
                .Initialize(transform.position, duration, 0, strength, size);
        }

        protected override void UpdateAimLine()
        {
            _aimLine.enabled = true;
            _aimLine.transform.rotation = Quaternion.Euler(0, 0, GetMouseInfo().rotation - 90);
            _aimLine.SetPosition(1, Vector2.up * 5f); // * Orb Distance Modifier
        }
    }
}
