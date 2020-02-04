using Elementalist.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class WindTornadoAbility : AbilityComponent, ISpecialAttackFlag
    {
        public override float Cooldown => 5f;
        public override float Damage => 30f;


        [SerializeField] private GameObject _tornadoPrefab;
        private List<Projectile> _pool;

        public override void MouseHeld((float rotation, float distance) mouseInfo) { }
        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            float duration = 10f; //debug;

            GetProjectileFromPool(ref _pool, _tornadoPrefab)
                .Initialize(transform.position, duration, 0, Damage, 1f)
                .AddPlayer(_orbBase.Player);

            _orbBase.OrbState = OrbState.Idling;
            Timer = Time.time + Cooldown;
        }

        public override void OnTouchEnter(Collider2D collision) { }
        public override void OnTouchStay(Collider2D collision) { }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            _pool = new List<Projectile>();
        }
    }
}