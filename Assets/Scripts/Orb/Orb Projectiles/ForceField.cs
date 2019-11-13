using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Elementalist.Enemies;

namespace Elementalist.Orbs
{
    public class ForceField : Projectile
    {
        private float _damage;
        private float _duration;

        public override Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float size)
        {
            transform.position = position;
            transform.GetChild(0).localScale = Vector2.one * size;
            _duration = Time.time + duration;
            _damage = damage;
            transform.gameObject.SetActive(true);

            return this;
        }

        private void Update()
        {
            if (Time.time > _duration)
                transform.gameObject.SetActive(false);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                enemy.TakeDamage(_damage * Time.deltaTime);
        }
    }
}
