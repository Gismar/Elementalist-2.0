﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elementalist.Enemies;

namespace Elementalist.Orbs
{
    public class WaterSlideAbility : AbilityComponent, IMainAttackFlag
    {
        public override float Cooldown => 2f;
        public override float Damage => 20f;

        [SerializeField] private float _speed = 10f;
        private Rigidbody2D _rigidbody;

        protected override void Start()
        {
            base.Start();
            _rigidbody = GetComponentInParent<Rigidbody2D>();
        }
        public override void MouseHeld((float rotation, float distance) mouseInfo)
        {
            if (_orbBase.OrbState != OrbState.Attacking)
                _orbBase.OrbState = OrbState.Attacking;

            transform.rotation = Quaternion.Euler(0, 0, mouseInfo.rotation - 90);
            mouseInfo.distance = Mathf.Min(mouseInfo.distance, /* Orb Distance Modifier */ 6);
            _rigidbody.velocity = transform.up * mouseInfo.distance * _speed;

            _augment.Cast(transform.position, null, _orbBase.Player);
        }

        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            _rigidbody.velocity = Vector2.zero;
            _orbBase.OrbState = OrbState.Idling;
            transform.localScale = Vector2.one;
            _augment.Cast(transform.position, null, _orbBase.Player);
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
            {
                enemy.TakeDamage(Damage * Time.deltaTime);
                enemy.AddEffect(StatusEffects.Drenched, 1f);
            }
        }
    }
}