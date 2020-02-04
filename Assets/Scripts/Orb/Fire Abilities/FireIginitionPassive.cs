using System.Collections;
using System.Collections.Generic;
using Elementalist.Enemies;
using Elementalist.Players;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class FireIginitionPassive : OrbAugment, IPassiveFlag
    {
        public override float CoolDown => 0;
        public float Damage => 50f;
        public float Duration => 5f;

        protected override void OnCast(Vector2 position, IEnemy enemy, Player player) 
            => enemy.AddEffect(StatusEffects.Ignited, Duration, Damage);
    }
}