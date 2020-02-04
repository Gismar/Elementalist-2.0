using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class ThunderCloudAbility : AbilityComponent, ISpecialAttackFlag
    {
        public override float Cooldown => 1f;
        public override float Damage => 10f;

        [SerializeField] private GameObject _thunderCloudPrefab;
        private List<Projectile> _pool;

        protected override void Start()
        {
            base.Start();
            _pool = new List<Projectile>();
        }

        public override void MouseHeld((float rotation, float distance) mouseInfo)
        {
            if (_orbBase.OrbState != OrbState.Attacking)
                _orbBase.OrbState = OrbState.Aiming;
        }

        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            float duration = 10f; //debug;

            GetProjectileFromPool(ref _pool, _thunderCloudPrefab)
                .Initialize(transform.position, duration, 0, Damage, 1);

            _orbBase.OrbState = OrbState.Idling;
            Timer = Time.time + Cooldown;
        }

        public override void OnTouchEnter(Collider2D collision) { }
        public override void OnTouchStay(Collider2D collision) { }
    }
}