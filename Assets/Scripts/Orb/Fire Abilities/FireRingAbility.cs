using Elementalist.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs 
{
    public class FireRingAbility : AbilityComponent, ISpecialAttackFlag
    {
        public override float Cooldown => 4f;
        public override float Damage => 25;

        [SerializeField] private GameObject _fireRingPrefab;
        private List<Projectile> _pool;
        private SpriteRenderer _sprite;
        private float _size;
        private float _maxSize = 5;

        protected override void Start()
        {
            base.Start();
            _pool = new List<Projectile>();
            _sprite = GetComponent<SpriteRenderer>();
            _sprite.enabled = false;
        }

        public override void MouseHeld((float rotation, float distance) mouseInfo)
        {
            if (_orbBase.OrbState != OrbState.Attacking)
                _orbBase.OrbState = OrbState.Aiming;

            _size += Time.deltaTime;
            transform.localScale = Vector2.one * Mathf.Min(_size, _maxSize);
            _sprite.enabled = true;
        }
        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            float duration = 2f; //debug;

            GetProjectileFromPool(ref _pool, _fireRingPrefab)
                .Initialize(transform.position, duration, 0, Damage, Mathf.Min(_size, _maxSize))
                .AddPassive(_passive);
            _size = 1;

            _orbBase.OrbState = OrbState.Idling;
            Timer = Time.time + Cooldown;
            _sprite.enabled = false;
        }

        public override void OnTouchEnter(Collider2D collision) 
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                _passive.Cast(Vector2.zero, enemy, null);
        }
        public override void OnTouchStay(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
                _passive.Cast(Vector2.zero, enemy, null);
        }
    }
}