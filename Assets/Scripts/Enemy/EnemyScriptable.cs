using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Enemies
{
    [CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Enemy Scriptable")]
    public class EnemyScriptable : ScriptableObject
    {
        public Sprite Sprite;
        public float BaseSpeed;
        public float BaseHealth;
        public float SpawnTime;
        public float PointValue;
        public int SpawnAmount;
    }
}
