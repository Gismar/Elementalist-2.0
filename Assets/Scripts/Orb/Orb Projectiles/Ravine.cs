using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class Ravine : Projectile
    {
        private float _timer;

        public override Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float size)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            transform.localScale = Vector2.right + (Vector2.up * size);
            _timer = Time.time + duration;
            gameObject.SetActive(true);
            return this;
        }

        private void Update()
        {
            if (Time.time > _timer)
                gameObject.SetActive(false);
        }
    }
}
