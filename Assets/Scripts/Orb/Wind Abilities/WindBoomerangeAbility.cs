using Elementalist.Enemies;
using Elementalist.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class WindBoomerangeAbility : AbilityComponent, IMainAttackFlag
    {
        public override float Cooldown => 1f;
        public override float Damage => 20f;

        [SerializeField] private AnimationCurve _boomerangCurve;
        [SerializeField] private AnimationCurve _knockbackCurve;
        private (Vector2 start, Vector2 end) _position;
        private Rigidbody2D _rigidbody;
        private LineRenderer _aimLine;
        private Transform _player;
        private float _lerpTimer = 0;
        private float _distance = 0;
        private float _range = 10;
        private bool _isAttacking = false;
        private int _enemiesHit = 0;

        protected override bool Check() => !_isAttacking;

        public override void MouseHeld((float rotation, float distance) mouseInfo) 
        {
            _aimLine.enabled = true;
            CalcEndPosition(mouseInfo);
            _aimLine.SetPosition(1, (_position.end - (Vector2)transform.position) / transform.localScale.x);
            _aimLine.SetPosition(2, (_player.position - transform.position) / transform.localScale.x);

            _lerpTimer = 0f;

        }

        public override void MouseUp((float rotation, float distance) mouseInfo)
        {
            _aimLine.enabled = false;
            _lerpTimer = 0f;
            CalcEndPosition(mouseInfo);
            _position.start = transform.position;
            _isAttacking = true;
        }

        public override void OnTouchEnter(Collider2D collision)
        {
            if (collision.GetComponentInParent<IEnemy>() is IEnemy enemy)
            {
                _enemiesHit++;
                enemy.TakeDamage(Damage * Mathf.Log(_enemiesHit + 9, 5f));
                Vector2 deltaPosition = collision.transform.position - _player.position;
                float strength = _distance * _knockbackCurve.Evaluate(deltaPosition.magnitude / _range);
                enemy.AddKnockback(deltaPosition.normalized * strength);
            }
        }
        public override void OnTouchStay(Collider2D collision) { }


        protected override void Start()
        {
            base.Start();
            _aimLine = GetComponent<LineRenderer>();
            _player = _orbBase.Player.transform;
            _rigidbody = GetComponentInParent<Rigidbody2D>();
            _aimLine.enabled = false;
        }

        void Update()
        {
            if (_isAttacking)
                Boomerang();

            if (_lerpTimer >= 1f && _isAttacking)
            {
                _orbBase.OrbState = OrbState.Idling;
                _isAttacking = false;
            }
        }

        private void Boomerang()
        {
            _lerpTimer += Time.deltaTime * (_range / _distance);

            Vector3 oldPosition = transform.position;
            _rigidbody.MovePosition(Vector2.Lerp(
                _lerpTimer <= 0.5f ? _position.start : (Vector2)_player.position,
                _position.end,
                _boomerangCurve.Evaluate(_lerpTimer)));
        }

        private void CalcEndPosition((float rotation, float distance) mouseInfo)
        {
            float angle = mouseInfo.rotation * Mathf.Deg2Rad;
            float distance = Mathf.Clamp(mouseInfo.distance, 1f, _range);
            _distance = distance;
            _position.end = new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle)) + transform.position;
        }
    }
}