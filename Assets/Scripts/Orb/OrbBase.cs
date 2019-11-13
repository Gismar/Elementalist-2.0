using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Elementalist.Orbs
{
    public abstract class OrbBase : MonoBehaviour
    {
        #region Variables
        public OrbState OrbState { get; protected set; }
        public OrbElement OrbElement { get; private set; }

        protected abstract void MainAttack();
        protected abstract void SpecialAttack();
        protected abstract void UpdateAimLine();

        protected abstract float _mainCooldown { get; }
        protected abstract float _specialCooldown { get; }
        protected abstract float _attackDamage { get; }
        protected abstract float _specialDamage { get; }
        //Global Data Handler

        [SerializeField] protected GameObject _mainProjectilePrefab;
        [SerializeField] protected GameObject _specialProjectilePrefab;
        [SerializeField] protected Animator _animator;
        
        protected Transform _player;
        protected Transform _projectilesFolder;
        protected Rigidbody2D _rigidbody;
        protected SpriteRenderer _spriteRenderer;
        protected List<Projectile> _mainProjectilePool;
        protected List<Projectile> _specialProjectilePool;

        private bool _canMainAttack;
        private bool _canSpecialAttack;
        private float _mainAttackTimer;
        private float _specialAttackTimer;
        private float _idlerLerpTimer;

        private readonly Color _transparentColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        #endregion

        public OrbBase Initialize(OrbSetup orbSetup, Transform projectileFolder)
        {
            _player = orbSetup.Player;
            OrbState = orbSetup.OrbState;
            OrbElement = orbSetup.OrbElement;
            _spriteRenderer = _spriteRenderer ?? GetComponent<SpriteRenderer>();
            _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
            _mainProjectilePool = new List<Projectile>(1);
            _specialProjectilePool = new List<Projectile>(1);
            _projectilesFolder = projectileFolder;
            gameObject.SetActive(false);

            return this;
        }

        public OrbBase Enable(Vector2 position, OrbState orbState)
        {
            OrbState = orbState;
            transform.position = position;
            transform.gameObject.SetActive(true);
            //set size to Global data

            return this;
        }

        protected virtual void Update()
        {
            switch (OrbState)
            {
                case OrbState.Idling:
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        OrbState = OrbState.Returning;
                        goto case OrbState.Returning;
                    }
                    break;
                }
                case OrbState.Returning:
                {
                    _rigidbody.MovePosition(Vector2.Lerp(transform.position, _player.position, _idlerLerpTimer));
                    _idlerLerpTimer += Time.deltaTime / Vector2.Distance(transform.position, _player.position);
                    if(_idlerLerpTimer >= 1)
                    {
                        // Set size back to global size;
                        OrbState = OrbState.Orbiting;
                        goto case OrbState.Orbiting;
                    }
                    break;
                }
                case OrbState.Orbiting:
                {
                    _rigidbody.MovePosition(new Vector2(Mathf.Cos(Time.time), Mathf.Sin(Time.time)) 
                                                * transform.localScale 
                                                + (Vector2)_player.position);
                    break;
                }
                case OrbState.Aiming:
                {
                    break;
                }
            }

            _canMainAttack = _mainAttackTimer <= Time.time;
            _canSpecialAttack = _specialAttackTimer <= Time.time;

            if (_canSpecialAttack)
                DoSpecialAttack();
            else
                _spriteRenderer.color = Color.Lerp(Color.white, _transparentColor, (_specialAttackTimer - Time.time) / _specialCooldown);

            if (_canMainAttack)
                DoMainAttack();
        }

        private void DoSpecialAttack()
        {
            if (Input.GetMouseButton(1))
            {
                SpecialAttack();
                SetupAttackTimer(_specialCooldown, ref _specialAttackTimer, ref _canSpecialAttack);
            }
        }

        private void DoMainAttack()
        {
            if (Input.GetMouseButton(0))
            {
                UpdateAimLine();
                OrbState = OrbState.Aiming;
            }

            if (Input.GetMouseButtonUp(0))
            {
                OrbState = OrbState.Attacking;
                UpdateAimLine();
                MainAttack();
                SetupAttackTimer(_mainCooldown, ref _mainAttackTimer, ref _canMainAttack);
            }
        }

        private void SetupAttackTimer(float delay, ref float attackTimer, ref bool canAttack)
        {
            attackTimer = Time.time + delay;
            canAttack = false;
        }

        protected (float rotation, float distance) GetMouseInfo()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mousePos - transform.position).normalized;
            var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var distance = Vector2.Distance(mousePos, transform.position);

            return (rotation, distance);
        }

        protected Projectile GetProjectileFromPool(ref List<Projectile> list, GameObject prefab)
        {
            var projectile = list.FirstOrDefault(p => !p.gameObject.activeInHierarchy);
            if (projectile == default)
            {
                projectile = Instantiate(prefab, _projectilesFolder).GetComponent<Projectile>();
                list.Add(projectile);
            }
            return projectile;
        }
    }
}