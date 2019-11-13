using Elementalist.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class Fireball : Projectile
    {
        private Rigidbody2D _rigidbody;
        private Vector2 _startPoint;
        private Vector2 _endPoint;
        private float _travelLerp;
        private float _damage;

        public override Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float size)
        {
            _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            transform.position = position;

            _startPoint = position;
            _damage = damage;
            _endPoint = transform.up * duration;
            _travelLerp = 0;

            gameObject.SetActive(true);
            return this;
        }

        private void Update()
        {
            _rigidbody.MovePosition(Vector2.Lerp(_startPoint, _endPoint, _travelLerp));
            _travelLerp += Time.deltaTime / 2f;
            if (_travelLerp >= 1f)
                gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                enemy.TakeDamage(_damage);
        }
    }
}
