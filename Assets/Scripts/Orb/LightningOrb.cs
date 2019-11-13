using Elementalist.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class LightningOrb : OrbBase
    {
        [SerializeField] private LayerMask _enemyMask;
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private LineRenderer _aimLine;
        [SerializeField] private CircleCollider2D _collider;

        protected override float _mainCooldown => 1.5f;
        protected override float _specialCooldown => 4f;
        protected override float _attackDamage => 25f;
        protected override float _specialDamage => 10f;

        private const int _projectileCount = 1;

        protected override void MainAttack()
        {
            _aimLine.enabled = false;
            float damage = _attackDamage; // * Orb Damage Modifier
            (float rotation, _) = GetMouseInfo();
            float range = 25f;
            float length = 5f;

            GetProjectileFromPool(ref _mainProjectilePool, _mainProjectilePrefab)
                    .Initialize(transform.position, length, rotation, damage, range);

            float x = Mathf.Cos(rotation * Mathf.Deg2Rad);
            float y = Mathf.Sin(rotation * Mathf.Deg2Rad);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(x, y).normalized, length, _wallMask);

            if (hit.collider != null)
            {
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                transform.position = (new Vector3(x, y).normalized * distance) + transform.position;
            }
            else
            {
                transform.position = (new Vector3(x, y).normalized * length) + transform.position;
            }

            OrbState = OrbState.Idling;
        }


        protected override void SpecialAttack()
        {
            float damage = _specialDamage; // * Orb Damage Modifier;
            float size = 5f; // * Orb Radius Modifier;
            float duration = 8f; // * Orb Duration;

            GetProjectileFromPool(ref _specialProjectilePool, _specialProjectilePrefab)
                .Initialize(transform.position, duration, 0, damage, size);


            OrbState = OrbState.Idling;
        }

        protected override void UpdateAimLine()
        {
            _aimLine.enabled = true;
            transform.rotation = Quaternion.Euler(0, 0, GetMouseInfo().rotation - 90);
            _aimLine.endWidth = transform.localScale.x * _aimLine.widthMultiplier;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy)
            {
                var hits = Physics2D.OverlapCircleAll(transform.position, 1f, _enemyMask);
                foreach (Collider2D hit in hits)
                {
                    if (hit.GetComponentInParent<IEnemy>() is IEnemy enemy)
                    {
                        enemy.TakeDamage(_attackDamage / hits.Length);
                        enemy.AddEffect(StatusEffects.Stunned, 0.25f);
                    }
                }
            }
        }

        public void ToggleInteraction (bool canInteract)
        {
            _collider.enabled = canInteract;
            _spriteRenderer.enabled = canInteract;
        }
    }
}
