using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elementalist.Enemies;

namespace Elementalist.Orbs
{
    public class QuickSand : Projectile
    {
        private float _strength;
        private float _timer;

        public override Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float size)
        {
            transform.position = position;
            transform.localScale = Vector2.one * size;
            _strength = damage;
            _timer = Time.time + duration;
            gameObject.SetActive(true);
            return this;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                enemy.AddEffect(StatusEffects.Slowed, 0.1f, _strength);
        }

        private void Update()
        {
            if (Time.time > _timer)
                gameObject.SetActive(false);
        }

    }
}
