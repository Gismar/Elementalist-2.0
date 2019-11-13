using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elementalist.Enemies;

namespace Elementalist.Orbs
{
    public class WaterOrb : OrbBase
    {
        protected override float _mainCooldown => 0.4f;
        protected override float _specialCooldown => 2f;
        protected override float _attackDamage => 20f;
        protected override float _specialDamage => 40f;

        [SerializeField] private float _speed;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private LineRenderer _aimLine;
        [SerializeField] private AnimationCurve _damageCurve;

        private float _distance;
        private float _moveTimer;

        protected override void MainAttack()
        {
            _rigidbody.velocity = Vector2.zero;
            OrbState = OrbState.Idling;
            _moveTimer = 1;
            transform.localScale = Vector2.one;
        }

        protected override void SpecialAttack()
        {
            _animator.SetTrigger("Pull");
            OrbState = OrbState.Idling;

            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, transform.localScale.magnitude, _mask);
            foreach (Collider2D collider in enemiesHit)
            {
                if (collider.GetComponentInParent<IEnemy>() is IEnemy enemy)
                {
                    Debug.Log("Pulled");
                    enemy.TakeDamage(_specialDamage * _damageCurve.Evaluate(_moveTimer));// * Orb Damage Modifier;
                    float strength = 5f * Vector2.Distance(transform.position, collider.transform.position);
                    enemy.AddKnockback((transform.position - collider.transform.position).normalized * strength);
                }
            }
        }

        protected override void UpdateAimLine()
        {
            (float rotation, float distance) = GetMouseInfo();
            transform.rotation = Quaternion.Euler(0, 0, rotation - 90);
            distance = Mathf.Min(distance, /* Orb Distance Modifier */ 6);
            _rigidbody.velocity = transform.up * distance * _speed;
            _moveTimer += distance * Time.deltaTime;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                enemy.TakeDamage(_attackDamage * Time.deltaTime * _damageCurve.Evaluate(_moveTimer)); // * Orb Damage Modifier
        }
    }
}
