using Elementalist.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class FireballShooter : AbilityComponent, IMainAttackFlag
    {
        public override float Cooldown => 1f;
        public override float Damage => 20f;

        [SerializeField] private GameObject _projectilePrefab;
        private List<Projectile> _pool;
        private LineRenderer _aimLine;
        private int _fireballCount = 2;

        protected override void Start()
        {
            base.Start();
            _pool = new List<Projectile>();
            _aimLine = GetComponent<LineRenderer>();
        }

        public override void MouseHeld((float rotation, float distance) mouseInfo)
        {
            _aimLine.enabled = true;
            float rotation = mouseInfo.rotation * Mathf.Deg2Rad;
            float x = Mathf.Cos(rotation) * 5f;
            float y = Mathf.Sin(rotation) * 5f;
            _aimLine.SetPositions(new Vector3[] {
                new Vector3(-x, -y, 0),
                new Vector3(x, y, 0)
            });
        }
        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            _aimLine.enabled = false;
            float damage = Damage; // * Orb Damage Modifier;
            float angle = 360f / _fireballCount;
            for (int i = 0; i < _fireballCount; i++)
                GetProjectileFromPool(ref _pool, _projectilePrefab)
                    .Initialize(transform.position, 5f, mouseInfo.rotation - (i * angle), damage, 0);
            _orbBase.SetState(OrbState.Idling);
            Timer = Time.time + Cooldown;
        }
        public override void OnTouchEnter(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
            {
                enemy.TakeDamage(Damage / 2f);
                enemy.AddEffect(StatusEffects.Ignited, 2f);
            }
        }
        public override void OnTouchStay(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
            {
                enemy.TakeDamage(Damage * Time.deltaTime);
                enemy.AddEffect(StatusEffects.Ignited, 1f);
            }
        }
    }
}