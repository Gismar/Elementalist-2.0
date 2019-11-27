using UnityEngine;

namespace Elementalist.Orbs
{
    public class OrbBase : MonoBehaviour
    {
        public OrbState OrbState { get; private set; }
        public OrbElement OrbElement { get; private set; }

        private AbilityComponent _mainAttackComponent;
        private AbilityComponent _specialAttackComponent;
        private Transform _player;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private float _idlerLerpTimer;
        private bool _isSpecialAttacking;

        public OrbBase Initialize(OrbSetup orbSetup)
        {
            _player = orbSetup.Player;
            OrbState = orbSetup.OrbState;
            OrbElement = orbSetup.OrbElement;
            _spriteRenderer = _spriteRenderer ?? GetComponent<SpriteRenderer>();
            _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
            _mainAttackComponent = _mainAttackComponent ?? (AbilityComponent)GetComponentInChildren<IMainAttackFlag>();
            _specialAttackComponent = _specialAttackComponent ?? (AbilityComponent)GetComponentInChildren<ISpecialAttackFlag>();
            gameObject.SetActive(false);

            return this;
        }

        public void Start()
        {
            OrbState = OrbState.Orbiting;
            OrbElement = OrbElement.Water;
            _spriteRenderer = _spriteRenderer ?? GetComponent<SpriteRenderer>();
            _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
            _mainAttackComponent = _mainAttackComponent ?? (AbilityComponent)GetComponentInChildren<IMainAttackFlag>();
            _specialAttackComponent = _specialAttackComponent ?? (AbilityComponent)GetComponentInChildren<ISpecialAttackFlag>();
        }

        public OrbBase Enable(Vector2 position, OrbState orbState)
        {
            OrbState = orbState;
            transform.position = position;
            transform.gameObject.SetActive(true);
            //set size to Global data

            return this;
        }

        private void Update()
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
                    var pos = new Vector2(Mathf.Cos(Time.time), Mathf.Sin(Time.time));
                    _rigidbody.MovePosition((pos * transform.localScale) + (Vector2)_player.position);
                    break;
                }
                case OrbState.Aiming:
                {
                    break;
                }
            }

            if (_specialAttackComponent.Predicate)
            {
                _isSpecialAttacking = true;
                if (Input.GetMouseButton(1))
                    _specialAttackComponent.MouseHeld(GetMouseInfo());
                else if (Input.GetMouseButtonUp(1))
                    _specialAttackComponent.MouseUp(GetMouseInfo());
                else
                    _isSpecialAttacking = false;
            }
            else
            {
                _isSpecialAttacking = false;
            }

            if (_mainAttackComponent.Predicate)
            {
                if (Input.GetMouseButton(0))
                    _mainAttackComponent.MouseHeld(GetMouseInfo());
                else if (Input.GetMouseButtonUp(0))
                    _mainAttackComponent.MouseUp(GetMouseInfo()); 
            }
        }

        public void SetState(OrbState state) => OrbState = state;

        private (float rotation, float distance) GetMouseInfo()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePos - transform.position).normalized;
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float distance = Vector2.Distance(mousePos, transform.position);

            return (rotation, distance);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isSpecialAttacking)
                _specialAttackComponent.OnTouchEnter(collision);
            _mainAttackComponent.OnTouchEnter(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_isSpecialAttacking)
                _specialAttackComponent.OnTouchStay(collision);
            _mainAttackComponent.OnTouchStay(collision);
        }

        //private Projectile GetProjectileFromPool(ref List<Projectile> list, GameObject prefab)
        //{
        //    var projectile = list.FirstOrDefault(p => !p.gameObject.activeInHierarchy);
        //    if (projectile == default)
        //    {
        //        projectile = Instantiate(prefab, _projectilesFolder).GetComponent<Projectile>();
        //        list.Add(projectile);
        //    }
        //    return projectile;
        //}
    }
}