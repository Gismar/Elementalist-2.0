using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public abstract class AbilityComponent : MonoBehaviour
    {
        public abstract float Cooldown { get; }
        public abstract float Damage { get; }
        public bool Predicate => Check();
        public float Timer { get; protected set; }

        protected OrbBase _orbBase;
        protected OrbAugment _augment;

        private Transform _projectilesTransform;

        protected virtual bool Check() => Time.time > Timer;
        protected virtual void Start()
        {
            _orbBase = GetComponentInParent<OrbBase>();
            OrbAugment augment = transform.parent.GetComponentInChildren<OrbAugment>();
            _projectilesTransform = GameObject.FindGameObjectWithTag("Projectile Folder").transform;
            if (augment != null)
                _augment = augment;
        }

        public abstract void MouseHeld((float rotation, float distance) mouseInfo);
        public abstract void MouseUp((float rotation, float distance) mouseInfo);
        public abstract void OnTouchEnter(Collider2D collision);
        public abstract void OnTouchStay(Collider2D collision);

        protected Projectile GetProjectileFromPool(ref List<Projectile> list, GameObject prefab)
        {
            Projectile projectile = list.FirstOrDefault(p => !p.gameObject.activeInHierarchy);
            if (projectile == default)
            {
                projectile = Instantiate(prefab, _projectilesTransform).GetComponent<Projectile>();
                list.Add(projectile);
            }
            return projectile;
        }
    }
}