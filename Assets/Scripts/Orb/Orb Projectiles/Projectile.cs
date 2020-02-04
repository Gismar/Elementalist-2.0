using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Elementalist.Players;

namespace Elementalist.Orbs
{
    public abstract class Projectile : MonoBehaviour
    {
        protected OrbAugment _augment;
        protected OrbAugment _passive;
        protected Player _player;
        public abstract Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float size);

        public Projectile AddAugment(OrbAugment augment)
        {
            _augment = augment;
            return this;
        }

        public Projectile AddPassive(OrbAugment passive)
        {
            _passive = passive;
            return this;
        }

        public Projectile AddPlayer(Player player)
        {
            _player = player;
            return this;
        }
    }
}
