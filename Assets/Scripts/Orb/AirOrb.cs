using Elementalist.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class AirOrb : OrbBase
    {
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private LineRenderer _aimLine;

        private (Vector2 start, Vector2 end) _position;
        private (Vector2 start, Vector2 end) _size;
        private (float main, float special) _lerpTimer;
        private (bool main, bool special) _isAttacking;
        private float _distanceTraveled;

        protected override float _mainCooldown => 1f;
        protected override float _specialCooldown => 5f;
        protected override float _attackDamage => 10f;
        protected override float _specialDamage => 0;

        protected override void MainAttack()
        {
            _aimLine.enabled = false;
            _isAttacking.main = true;
            _lerpTimer.main = 0f;
            CalcEndPosition();
            _position.start = transform.position;
        }

        protected override void SpecialAttack()
        {
            _isAttacking.special = true;
            _lerpTimer.special = 0;
            _size = (transform.localScale, transform.localScale * 5f /* Orb Size Modifier*/);
        }

        protected override void UpdateAimLine()
        {
            _aimLine.enabled = true;
            CalcEndPosition();
            _aimLine.SetPosition(1, (_position.end - (Vector2)transform.position) / transform.localScale.x);
            _aimLine.SetPosition(2, (_player.position - transform.position) / transform.localScale.x);
        }

        private void CalcEndPosition()
        {
            (float angle, float distance) = GetMouseInfo();
            angle *= Mathf.Deg2Rad;
            distance = Mathf.Clamp(distance, 0f, 10f);

            _position.end = new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle)) + transform.position;
        }

        protected override void Update()
        {
            if (_isAttacking.main)
                Boomerang();
            if (_isAttacking.special)
                SpecialGrowth();

            if (_isAttacking == (false, false) && OrbState == OrbState.Attacking)
                OrbState = OrbState.Idling;

            base.Update();
        }

        private void SpecialGrowth()
        {
            _lerpTimer.special += Time.deltaTime;
            transform.localScale = Vector2.Lerp(_size.start, _size.end, _animationCurve.Evaluate(_lerpTimer.special));

            if (_lerpTimer.special >= 1f)
                _isAttacking.special = false;
        }

        private void Boomerang()
        {
            _lerpTimer.main += Time.deltaTime;

            Vector3 oldPosition = transform.position;
            _rigidbody.MovePosition(Vector2.Lerp(
                _lerpTimer.main <= 0.5f ? _position.start : (Vector2)_player.position,
                _position.end,
                _animationCurve.Evaluate(_lerpTimer.main)));

            _distanceTraveled += Vector2.Distance(transform.position, oldPosition);

            if (_lerpTimer.main >= 1f)
                _isAttacking.main = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is EnemyBase enemy)
            {
                enemy.TakeDamage(_attackDamage * Mathf.Log((_distanceTraveled + 10) * ((Vector2)transform.localScale).sqrMagnitude, 5f));
                Vector3 deltaPosition = enemy.transform.position - _player.position;
                float strength = transform.localScale.magnitude + (10 - deltaPosition.magnitude);
                enemy.AddKnockback(deltaPosition.normalized * strength);
            }
        }
    }
}
