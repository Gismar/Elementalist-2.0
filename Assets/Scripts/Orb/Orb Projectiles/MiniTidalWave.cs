using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elementalist.Enemies;

namespace Elementalist.Orbs
{
    public class MiniTidalWave : Projectile
    {
        [SerializeField] private Collider2D _collider2D;
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private float _timer;
        private float _damage;
        private float _knockbackStrength;
        private float _speed = 5f;

        public override Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float strength)
        {
            _timer = duration + Time.time;
            _damage = damage;
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.MovePosition(position);
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, rotation - 90);
            _direction = transform.up.normalized;
            _knockbackStrength = strength;

            return this;
        }

        private void Update()
        {
            _rigidbody.MovePosition((Vector2)transform.position + _direction * Time.deltaTime * _speed);

            if (Time.time > _timer)
                gameObject.SetActive(false);
            else
                Debug.Log($"Time Remaining: {Time.time - _timer}");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
            {
                enemy.TakeDamage(_damage);
                enemy.AddKnockback(_direction * _knockbackStrength);
            }
        }
    }
}