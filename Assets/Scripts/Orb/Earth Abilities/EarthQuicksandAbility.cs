using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class EarthQuicksandAbility : AbilityComponent, ISpecialAttackFlag
    {
        public override float Cooldown { get; }
        public override float Damage => 25f;

        [SerializeField] private GameObject _projectilePrefab;
        private List<Projectile> _pool;
        private Animator _animator;

        protected override void Start()
        {
            base.Start();
            _animator = GetComponentInParent<Animator>();
            _pool = new List<Projectile>();
        }

        public override void MouseHeld((float rotation, float distance) mouseInfo) { }
        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            _animator.SetTrigger("Attack");

            float size = 1; //debug;
            float duration = 3f; //debug;
            float strength = Damage / 100f;

            GetProjectileFromPool(ref _pool, _projectilePrefab)
                .Initialize(transform.position, duration, 0, strength, size);

            Timer = Time.time + Cooldown;
        }

        public override void OnTouchEnter(Collider2D collision) { /* Earth Orb Does Not Have Trigger Component*/ }
        public override void OnTouchStay(Collider2D collision) { /* Earth Orb Does Not Have Trigger Component*/ }
    }
}