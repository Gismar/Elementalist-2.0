using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Players
{
    public class Player : MonoBehaviour
    {
        public float Speed { get; set; }
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public float BodyDamage { get; set; }

        private Dictionary<KeyCode, Vector2> _inputs;
        private Camera _camera;
        private Rigidbody2D _rigidbody;
        private float _invincibilityTimer;

        public void Die()
        {
            //Change to Game Over Scene
        }

        public Player Initialize(Vector2 position, float health, float speed)
        {
            _inputs = new Dictionary<KeyCode, Vector2>
            {
                [KeyCode.W] = Vector2.up,
                [KeyCode.S] = Vector2.down,
                [KeyCode.A] = Vector2.left,
                [KeyCode.D] = Vector2.right
            };

            _camera = Camera.main;
            _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
            MaxHealth = health;
            CurrentHealth = MaxHealth;
            Speed = speed;
            transform.position = position;
            return this;
        }

        private void Start()
        {
            Initialize(Vector2.zero, 100f, 5f);
        }

        private void FixedUpdate()
        {
            Vector2 direction = (_camera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            var move = Vector2.zero;
            foreach (var inputPair in _inputs.Where(k => Input.GetKey(k.Key)))
                move += inputPair.Value;

            transform.rotation = Quaternion.Euler(0, 0, rotation);
            _rigidbody.MovePosition((Vector2)transform.position + move * Time.deltaTime * Speed);
            _camera.transform.position = transform.position + Vector3.back;
        }

        public void TakeDamage(float damage)
        {
            if (_invincibilityTimer >= Time.time)
                return;

            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
                Die();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.transform.GetComponent<Enemies.IEnemy>() is Enemies.IEnemy enemy)
                enemy.TakeDamage(Mathf.Log10(Time.timeSinceLevelLoad + 1f));
        }
    }
}
