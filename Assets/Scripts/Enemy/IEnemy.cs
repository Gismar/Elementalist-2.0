using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Elementalist.Enemies
{
    public interface IEnemy
    { 
        float Speed { get; set; }
        float MaxHealth { get; set; }
        float CurrentHealth { get; set; }
        float BodyDamage { get; set; }
        StatusEffects Effects { get; }

        void TakeDamage(float damage);
        void AddEffect(StatusEffects type, float duration, float strength = 0);
        void AddKnockback(Vector2 velocity);
        void Die();
    }
}
