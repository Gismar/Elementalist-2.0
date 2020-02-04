using UnityEngine;

namespace Elementalist.Enemies
{
    public class MultiplyingEnemy : EnemyBase
    {
        [SerializeField] private GameObject _babyPrefab;
        [SerializeField] private EnemyScriptable _babyScriptable;

        private EnemyInfo _info;

        public override EnemyBase Initialize(EnemyInfo enemyInfo, Transform target, Vector2 position, (Color color, Sprite sprite) tier)
        {
            MaxHealth = enemyInfo.MaxHealth;
            CurrentHealth = MaxHealth;
            Speed = enemyInfo.Speed;
            _info = enemyInfo;
            _tierRenderer.sprite = tier.sprite;
            _tierRenderer.color = tier.color;

            return Setup(target);
        }

        public override void Die()
        {
            // Add point to player
            var enemyInfo = new EnemyInfo(_info.EnemyDifficulty, _babyScriptable, _info.Round);
            for(int i = 0; i < _info.SpawnAmount; i++)
            {
                EnemyBase baby = Instantiate(_babyPrefab)
                    .GetComponent<RegularEnemy>()
                    .Initialize(enemyInfo, _player, transform.position, (_tierRenderer.color, _tierRenderer.sprite));

                baby.AddKnockback(Random.insideUnitCircle * 2f);
            }
            gameObject.SetActive(false);
        }
    }
}
