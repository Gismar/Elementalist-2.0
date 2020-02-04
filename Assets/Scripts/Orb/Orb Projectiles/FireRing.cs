using Elementalist.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class FireRing : Projectile
    {
        private Vector2 _targetSize;
        private float _growLerp;
        private float _duration;
        private float _damage;

        public override Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float size)
        {
            transform.localScale = Vector2.zero;
            transform.position = position;
            _targetSize = Vector2.one * size;
            _damage = damage;
            _growLerp = 0;
            _duration = duration;
            gameObject.SetActive(true);
            return this;
        }

        private void Update()
        {
            transform.Rotate(Vector3.forward * 15f);
            transform.localScale = Vector2.Lerp(Vector2.zero, _targetSize, _growLerp);
            _growLerp += Time.deltaTime / _duration;
            if (_growLerp >= 1f)
                gameObject.SetActive(false);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
            {
                enemy.TakeDamage(_damage * Time.deltaTime);
                _passive.Cast(Vector2.zero, enemy, null);
            }
        }
    }
}
