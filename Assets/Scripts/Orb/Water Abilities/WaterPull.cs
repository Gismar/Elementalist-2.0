using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elementalist.Enemies;
using Elementalist.Players;

namespace Elementalist.Orbs
{
    public class WaterPull : AbilityComponent, ISpecialAttackFlag
    {
        public override float Cooldown => 2f;
        public override float Damage => 40f;

        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private LayerMask _mask;
        private Animator _animator;
        private float _charge;
        private SpriteRenderer _sprite;
        private Player _player;

        protected override void Start()
        {
            base.Start();
            _animator = GetComponentInParent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _sprite.enabled = false;
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        public override void MouseHeld((float rotation, float distance) mouseInfo)
        {
            if (_orbBase.OrbState != OrbState.Attacking)
                _orbBase.SetState(OrbState.Attacking);

            if (!_sprite.enabled)
                _sprite.enabled = true;

            _charge += Time.deltaTime;
            transform.parent.localScale = Vector2.one * _curve.Evaluate(_charge);
            transform.localScale = Vector2.one * transform.parent.localScale.magnitude;

            if (_charge > _curve.keys[_curve.keys.Length - 1].time)
                MouseUp(mouseInfo);
        }
        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            _animator.SetTrigger("Pull");
            _orbBase.SetState(OrbState.Idling);

            float curve = _curve.Evaluate(_charge);

            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, transform.parent.localScale.magnitude, _mask);
            foreach (Collider2D collider in enemiesHit)
            {
                if (collider.GetComponentInParent<IEnemy>() is IEnemy enemy)
                {
                    enemy.TakeDamage(Damage * curve);// * Orb Damage Modifier;
                    float strength = 5f * Vector2.Distance(transform.position, collider.transform.position);
                    enemy.AddKnockback((transform.position - collider.transform.position).normalized * strength);
                }
            }
            transform.parent.localScale = Vector2.one;
            _charge = 0;
            _sprite.enabled = false;
            Timer = Time.time + Cooldown;
            _augment.Cast(transform.position, null, _player);
        }

        public override void OnTouchEnter(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
            {
                enemy.TakeDamage(Damage);
                enemy.AddEffect(StatusEffects.Drenched, 1f);
            }
        }

        public override void OnTouchStay(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                enemy.AddEffect(StatusEffects.Drenched, 1f);
        }
    }
}