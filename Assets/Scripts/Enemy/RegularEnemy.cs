using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Enemies
{
    public class RegularEnemy : EnemyBase
    {
        private float _pointValue;

        public override EnemyBase Initialize(EnemyInfo enemyInfo, Transform target, Vector2 position, (Color color, Sprite sprite) tier)
        {
            MaxHealth = enemyInfo.MaxHealth;
            CurrentHealth = MaxHealth;
            Speed = enemyInfo.Speed;
            transform.position = position;

            _pointValue = enemyInfo.PointValue;
            _tierRenderer.sprite = tier.sprite;
            _tierRenderer.color = tier.color;

            return Setup(target);
        }

        public override void Die()
        {
            // Add point to player
            gameObject.SetActive(false);
        }
    }
} 
