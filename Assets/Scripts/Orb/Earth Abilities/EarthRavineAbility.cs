using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class EarthRavineAbility : AbilityComponent, IMainAttackFlag
    {
        public override float Cooldown => 3f;
        public override float Damage => 0f;

        [SerializeField] private GameObject _projectilePrefab;
        private List<Projectile> _pool;
        private LineRenderer _aimLine;
        private Animator _animator;

        protected override void Start()
        {
            base.Start();
            _aimLine = GetComponent<LineRenderer>();
            _animator = GetComponentInParent<Animator>();
            _aimLine.enabled = false;
            _pool = new List<Projectile>();
        }

        public override void MouseHeld((float rotation, float distance) mouseInfo)
        {
            if (_orbBase.OrbState != OrbState.Attacking)
                _orbBase.OrbState = OrbState.Aiming;

            _aimLine.enabled = true;
            float rotation = mouseInfo.rotation * Mathf.Deg2Rad;
            var pos = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)) * 5f;
            _aimLine.SetPosition(1, pos);
        }

        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            _aimLine.enabled = false;
            _animator.SetTrigger("Attack");
            _orbBase.OrbState = OrbState.Idling;

            float duration = 3f; //debug;
            float length = 3f; //debug;

            GetProjectileFromPool(ref _pool, _projectilePrefab)
                .Initialize(transform.position, duration, mouseInfo.rotation - 90, 0, length);

            Timer = Time.time + Cooldown;
        }

        public override void OnTouchEnter(Collider2D collision) { /* Earth Orb Does Not Have Trigger Component*/ }
        public override void OnTouchStay(Collider2D collision) { /* Earth Orb Does Not Have Trigger Component*/ }
    }
}