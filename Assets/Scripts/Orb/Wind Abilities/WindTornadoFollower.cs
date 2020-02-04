using Elementalist.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class WindTornadoFollower : Projectile
    {
        [SerializeField] private LayerMask _mask;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private AnimationCurve _knockbackCurve;
        private Rigidbody2D _rigidbody2D;
        private Transform _target;
        private float _range = 4f;
        private float _damage;
        private float _distance;
        private float _duration;
        private float _durationLerp;

        public override Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float size)
        {
            transform.position = position;
            _damage = damage;
            _duration = duration;
            gameObject.SetActive(true);

            _durationLerp = 0;
            _distance = 0;

            return this;
        }

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            _durationLerp += Time.deltaTime / _duration;
            if (_durationLerp >= 1f)
                gameObject.SetActive(false);

            if (_target == null)
            {
                Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _range * _range, _mask);
                float smallestDistance = float.MaxValue;
                foreach (Collider2D temp in hit)
                {
                    Vector3 position = temp.transform.position;
                    if ((position - transform.position).sqrMagnitude < smallestDistance)
                        _target = temp.transform;
                }
            }

            Vector3 direction = (_target.position - transform.position).normalized;
            _rigidbody2D.MovePosition(transform.position + direction * _speed * Time.deltaTime);
            _distance += _speed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
            {
                enemy.TakeDamage(_damage);
                Vector2 deltaPosition = collision.transform.position -_player.transform.position;
                enemy.AddKnockback(deltaPosition.normalized * _distance);
            }
        }
    }
}