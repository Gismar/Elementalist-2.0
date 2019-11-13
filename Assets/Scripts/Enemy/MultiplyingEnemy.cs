using UnityEngine;

namespace Elementalist.Enemies
{
    public class MultiplyingEnemy : EnemyBase
    {
        [SerializeField] private GameObject _babyPrefab;
        [SerializeField] private EnemyScriptable _babyScriptable;

        private float _pointValue;
        private int _spawnAmount;
        private float _multiplier;

        public override EnemyBase Initialize(EnemyInfo enemyInfo, Transform target, Vector2 position, (Color color, Sprite sprite) tier)
        {
            MaxHealth = enemyInfo.MaxHealth;
            CurrentHealth = MaxHealth;
            Speed = enemyInfo.Speed;

            _multiplier = enemyInfo.Multiplier;
            _pointValue = enemyInfo.PointValue;
            _spawnAmount = enemyInfo.SpawnAmount;
            _tierRenderer.sprite = tier.sprite;
            _tierRenderer.color = tier.color;

            return Setup(target);
        }

        public override void Die()
        {
            // Add point to player
            var enemyInfo = new EnemyInfo(_multiplier, _babyScriptable);
            for(int i = 0; i < _spawnAmount; i++)
            {
                var baby = Instantiate(_babyPrefab)
                    .GetComponent<RegularEnemy>()
                    .Initialize(enemyInfo, _player, transform.position, (_tierRenderer.color, _tierRenderer.sprite));

                baby.AddKnockback(Random.insideUnitCircle * 2f);
            }
            gameObject.SetActive(false);
        }
    }
}
