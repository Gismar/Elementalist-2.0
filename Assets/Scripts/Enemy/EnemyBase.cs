using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elementalist.Players;

namespace Elementalist.Enemies
{
    public abstract class EnemyBase : MonoBehaviour, IEnemy
    {
        private static Color _transparent = new Color(1f, 1f, 1f, 0.5f);

        public EnemyType EnemyType { get; set; }
        public float Speed { get; set; }
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public float BodyDamage { get; set; }

        public abstract void Die();
        public abstract EnemyBase Initialize(EnemyInfo enemyInfo, Transform target, Vector2 position, (Color color, Sprite sprite) tier);

        [SerializeField] protected GameObject _body;
        [SerializeField] protected SpriteRenderer _tierRenderer;
        protected Transform _player;

        [SerializeField] private SpriteRenderer _healthBar;
        [SerializeField] private Gradient _healthBarGradient;
        private Rigidbody2D _rigidBody;
        private StatusEffects _statusEffects;
        private (float invincibility, float stun, float slow) _timer;
        private float _slowStrength;

        protected EnemyBase Setup(Transform player)
        {
            gameObject.SetActive(true);
            _rigidBody = _rigidBody ?? GetComponent<Rigidbody2D>();
            _player = player;
            _statusEffects = StatusEffects.None;
            _slowStrength = 1f;

            AddEffect(StatusEffects.Invicibile, 1f);
            UpdateHealthBar();
            return this;
        }

        public virtual void TakeDamage(float damage)
        {
            if (_statusEffects.HasFlag(StatusEffects.Invicibile))
                return;

            CurrentHealth -= damage;
            UpdateHealthBar();
            if (CurrentHealth <= 0f)
                Die();
        }

        private void UpdateHealthBar()
        {
            float ratio = CurrentHealth / MaxHealth;
            _healthBar.transform.localScale = new Vector2(ratio, 0.1f);
            _healthBar.color = _healthBarGradient.Evaluate(Mathf.Clamp01(1.25f - ratio));
        }

        private void Update()
        {
            if (_statusEffects != StatusEffects.None)
            {
                if (_statusEffects.HasFlag(StatusEffects.Knockedback))
                {
                    _rigidBody.velocity = _rigidBody.velocity.magnitude >= 0.1f ? _rigidBody.velocity * 0.9f : Vector2.zero;
                    if (_rigidBody.velocity == Vector2.zero)
                        _statusEffects ^= StatusEffects.Knockedback;
                    return;
                }
                if (_statusEffects.HasFlag(StatusEffects.Stunned))
                {
                    if (_timer.stun < Time.time)
                        _statusEffects ^= StatusEffects.Stunned;
                    return;
                }
                if (_statusEffects.HasFlag(StatusEffects.Slowed))
                {
                    if (_timer.slow < Time.time)
                    {
                        _statusEffects ^= StatusEffects.Slowed;
                        _slowStrength = 1f;
                    }
                }
                if (_statusEffects.HasFlag(StatusEffects.Invicibile))
                {
                    if (_timer.invincibility < Time.time)
                        _statusEffects ^= StatusEffects.Invicibile;
                }
            }

            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _body.transform.rotation = Quaternion.Euler(0, 0, rotation);

            _rigidBody.MovePosition(transform.position + (_body.transform.right * Time.deltaTime * Speed * _slowStrength));
        }

        public void AddEffect(StatusEffects statusEffect, float duration)
        {
            _statusEffects |= statusEffect;
            if (statusEffect == StatusEffects.Invicibile)
                _timer.invincibility = Time.time + duration;
            else if (statusEffect == StatusEffects.Slowed)
                _timer.slow = Time.time + duration;
            else if (statusEffect == StatusEffects.Stunned)
                _timer.stun = Time.time + duration;
        }

        public void AddSlow(float duration, float strength)
        {
            _slowStrength = strength;
            AddEffect(StatusEffects.Slowed, duration);
        }

        public void AddKnockback(Vector2 velocity)
        {
            _rigidBody.velocity = velocity;
            AddEffect(StatusEffects.Knockedback, 1f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.GetComponent<Player>() is IEnemy player)
            {
                AddKnockback(20f * -transform.up);
                if (_statusEffects.HasFlag(StatusEffects.Invicibile))
                    return;

                player.TakeDamage(BodyDamage);
            }
        }
    }
}
