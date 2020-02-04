using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elementalist.Enemies;

namespace Elementalist.Orbs
{
    public class ThunderCloud : Projectile
    {
        [SerializeField] private LayerMask _mask;
        [SerializeField] private Transform _indicator;
        private float _damage;
        private float _duration;
        private float _durationLerp;
        private float _range = 4f;  
        private float _attackTimer;


        public override Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float size)
        {
            transform.position = position;
            _damage = damage;
            _duration = duration;
            gameObject.SetActive(true);

            _durationLerp = 0;

            return this;
        }

        private void Update()
        {
            _durationLerp += Time.deltaTime / _duration;
            _attackTimer += Time.deltaTime;

            _indicator.localScale = Vector2.one * Mathf.Clamp01(_attackTimer) * _range;

            if (_attackTimer >= 1f)
            {
                Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _range * _range, _mask);
                foreach (Collider2D temp in hit)
                {
                    if (temp.GetComponentInParent<IEnemy>() is IEnemy enemy)
                    {
                        Attack(enemy);
                        _attackTimer = 0;
                    }
                }
            }

            if (_durationLerp >= 1f)
                gameObject.SetActive(false);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                Attack(enemy);
        }

        private void Attack(IEnemy enemy)
        {
            if (enemy.Effects.HasFlag(StatusEffects.Drenched))
            {
                enemy.TakeDamage(_damage * Time.deltaTime * 2f);
                enemy.AddEffect(StatusEffects.Stunned, 2f);
            }
            else
            {
                enemy.TakeDamage(_damage * Time.deltaTime);
            }
        }
    }
}