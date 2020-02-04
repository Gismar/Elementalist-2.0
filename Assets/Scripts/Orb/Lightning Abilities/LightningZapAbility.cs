using Elementalist.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class LightningZapAbility : AbilityComponent, IMainAttackFlag
    {
        public override float Cooldown => 0f;
        public override float Damage => 200f;

        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private float _emissionRate;
        [SerializeField] private LayerMask _mask;

        protected override void Start()
        {
            base.Start();
            _particleSystem = GetComponent<ParticleSystem>();
            _emissionRate = _particleSystem.emission.rateOverTimeMultiplier;
        }

        public override void MouseHeld((float rotation, float distance) mouseInfo)
        {
            _orbBase.OrbState = OrbState.Idling;
            transform.rotation = Quaternion.Euler(0, 0, mouseInfo.rotation - 90);

            if (!_particleSystem.emission.enabled)
            {
                var emission = _particleSystem.emission;
                emission.enabled = true;
            }
        }

        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            _orbBase.OrbState = OrbState.Idling;
            var emission = _particleSystem.emission;
            emission.enabled = false;
        }
        public override void OnTouchEnter(Collider2D collision) { }
        public override void OnTouchStay(Collider2D collision) { }

        private void OnParticleCollision(GameObject other)
        {
            foreach (Collider2D temp in Physics2D.OverlapCircleAll(other.transform.position, 2f, _mask))
            {
                if (temp.GetComponentInParent<IEnemy>() is IEnemy enemy)
                {
                    enemy.AddEffect(StatusEffects.Stunned, 0.1f);
                    float modifier = 1f;

                    if (temp.gameObject == other)
                        if (enemy.Effects.HasFlag(StatusEffects.Drenched))
                            modifier = 2f;
                    else
                        if (!enemy.Effects.HasFlag(StatusEffects.Drenched))
                            modifier = 0.5f;

                    enemy.TakeDamage(Damage / _emissionRate * modifier);
                }
            }
        }
    }
}