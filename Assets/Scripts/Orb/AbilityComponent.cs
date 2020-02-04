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
        protected OrbAugment _passive;

        private Transform _projectilesTransform;

        protected virtual bool Check() => Time.time > Timer;
        protected virtual void Start()
        {
            _orbBase = GetComponentInParent<OrbBase>();
            _augment = (OrbAugment)transform.parent.GetComponentInChildren<IAugmentFlag>();
            _passive = (OrbAugment)transform.parent.GetComponentInChildren<IPassiveFlag>();
            _projectilesTransform = GameObject.FindGameObjectWithTag("Projectile Folder").transform;
        }

        /// <summary>
        /// Called when mouse button is held down.
        /// </summary>
        /// <param name="mouseInfo">Mouse info in terms of distance and angle from player.</param>
        public abstract void MouseHeld((float rotation, float distance) mouseInfo);

        /// <summary>
        /// Called when mouse button is released.
        /// </summary>
        /// <param name="mouseInfo">Mouse info in terms of distance and angle from player.</param>
        public abstract void MouseUp((float rotation, float distance) mouseInfo);

        /// <summary>
        /// Called when an object enters trigger collision with the orb.
        /// </summary>
        public abstract void OnTouchEnter(Collider2D collision);

        /// <summary>
        /// Called when an object is still inside the orb's trigger collider.
        /// </summary>
        public abstract void OnTouchStay(Collider2D collision);

        /// <summary>
        /// Get's an unused projectile from the pool. If none exist, will make a new projectile and add it to the list.
        /// </summary>
        /// <param name="list">The pool to which the projectile will be pooled from. </param>
        /// <param name="prefab">The projectile prefab to instantiate in case of no objects available in the pool.</param>
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