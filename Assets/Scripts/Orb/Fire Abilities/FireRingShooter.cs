using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs 
{
    public class FireRingShooter : AbilityComponent, ISpecialAttackFlag
    {
        public override float Cooldown => 4f;
        public override float Damage => 25;

        [SerializeField] private GameObject _fireRingPrefab;
        private List<Projectile> _pool;
        private float _size;
        private float _maxSize = 5;

        protected override void Start()
        {
            base.Start();
            _pool = new List<Projectile>();
        }

        public override void MouseHeld((float rotation, float distance) mouseInfo) => _size += Time.deltaTime;
        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            float duration = 2f; //debug;

            GetProjectileFromPool(ref _pool, _fireRingPrefab)
                .Initialize(transform.position, duration, 0, Damage, Mathf.Min(_size, _maxSize));
            _size = 1;

            Timer = Time.time + Cooldown;
        }

        public override void OnTouchEnter(Collider2D collision) { /*Does Nothing*/}
        public override void OnTouchStay(Collider2D collision) { /*Does Nothing*/}
    }
}