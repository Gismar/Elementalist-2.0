using Elementalist.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class LightningBeam : Projectile
    {
        [SerializeField] public float Scale;

        private ParticleSystem.ShapeModule _shapeModule;
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider;
        private Vector2 _startPos;
        private float _damage;
        private float _timer;

        public override Projectile Initialize(Vector2 position, float length, float rotation, float damage, float range)
        {
            gameObject.SetActive(true);
            _timer = Time.time + 1f;
            _damage = damage;
            transform.position = position;

            Transform orbLeft = transform.GetChild(0);
            Transform orbRight = transform.GetChild(1);
            Transform beam = transform.GetChild(2);

            beam.rotation = Quaternion.Euler(0, 0, rotation - 90);
            _shapeModule = beam.GetComponent<ParticleSystem>().shape;
            _spriteRenderer = beam.GetComponent<SpriteRenderer>();
            _collider = beam.GetComponent<BoxCollider2D>();

            float radians = rotation * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            float tan = Mathf.Tan(range * Mathf.Deg2Rad);

            AnimationClip clip = GetComponent<Animation>().clip;
            clip.ClearCurves();
            var scaleCurve = AnimationCurve.Linear(0, 0, 1, length);
            var mainCurveX = AnimationCurve.Linear(0, 0, 1, length * cos);
            var mainCurveY = AnimationCurve.Linear(0, 0, 1, length * sin);
            var rightOrbCurveX = AnimationCurve.Linear(0, 0, 1, length * (cos + (tan * sin)));
            var rightOrbCurveY = AnimationCurve.Linear(0, 0, 1, length * (sin - (tan * cos)));
            var leftOrbCurveX  = AnimationCurve.Linear(0, 0, 1, length * (cos - (tan * sin)));
            var leftOrbCurveY  = AnimationCurve.Linear(0, 0, 1, length * (sin + (tan * cos)));

            clip.SetCurve(orbLeft.name, typeof(Transform), "localPosition.x", rightOrbCurveX);
            clip.SetCurve(orbLeft.name, typeof(Transform), "localPosition.y", rightOrbCurveY);
            clip.SetCurve(orbRight.name, typeof(Transform), "localPosition.x", leftOrbCurveX);
            clip.SetCurve(orbRight.name, typeof(Transform), "localPosition.y", leftOrbCurveY);
            clip.SetCurve(beam.name, typeof(Transform), "localPosition.x", mainCurveX);
            clip.SetCurve(beam.name, typeof(Transform), "localPosition.y", mainCurveY);
            clip.SetCurve("", typeof(LightningBeam), "Scale", scaleCurve);

            return this;
        }

        private void Update()
        {
            if (Time.time > _timer)
                gameObject.SetActive(false);

            _shapeModule.scale = new Vector2(Scale, 0);
            _spriteRenderer.size = new Vector2(Scale, 0.5f);
            _collider.size = new Vector2(Scale, 0.25f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
            {
                enemy.TakeDamage(_damage);
                Debug.Log($"Took Damage {_damage}");

                enemy.AddEffect(StatusEffects.Stunned, 0.5f);
            }
        }
    }
}
