using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Elementalist.Orbs
{
    public abstract class Projectile : MonoBehaviour
    {
        public abstract Projectile Initialize(Vector2 position, float duration, float rotation, float damage, float size);
    }
}
