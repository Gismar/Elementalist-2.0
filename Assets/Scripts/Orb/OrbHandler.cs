using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elementalist.Orbs
{
    public class OrbHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] _orbPrefabs;

        //Global Data Handler
        private OrbBase _currentOrb;
        private Dictionary<OrbElement, OrbBase> _orbPool; // Acts as a readonly dictionary.
        private Dictionary<KeyCode, OrbElement> _swapKeys; // Acts as a readonly dictionary.

        public OrbHandler Initialize(Transform player)
        {
            Transform projectileFolder = new GameObject("Projectiles").transform;
            _orbPool = new Dictionary<OrbElement, OrbBase>
            {
                [OrbElement.Water] = CreateOrb(OrbElement.Water, player, projectileFolder),
                [OrbElement.Fire] = CreateOrb(OrbElement.Fire, player, projectileFolder),
                [OrbElement.Earth] = CreateOrb(OrbElement.Earth, player, projectileFolder),
                [OrbElement.Lightning] = CreateOrb(OrbElement.Lightning, player, projectileFolder),
                [OrbElement.Air] = CreateOrb(OrbElement.Air, player, projectileFolder)
            };

            _swapKeys = new Dictionary<KeyCode, OrbElement>
            {
                [KeyCode.Q] = OrbElement.Water,
                [KeyCode.Alpha3] = OrbElement.Fire,
                [KeyCode.E] = OrbElement.Lightning,
                [KeyCode.F] = OrbElement.Earth,
                [KeyCode.V] = OrbElement.Air
            };

            _currentOrb = _orbPool[OrbElement.Water];
            return this;
        }

        private void Update()
        {
            if (_currentOrb.OrbState != OrbState.Attacking)
            {
                var keyPressed = _swapKeys.FirstOrDefault(k => Input.GetKeyDown(k.Key));
                if (!keyPressed.Equals(default(KeyValuePair<KeyCode, OrbElement>)))
                    KeySwap(keyPressed.Value);

                var scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0)
                    ScrollSwap(Mathf.FloorToInt(Mathf.Sign(scroll)));
            }
        }

        private OrbBase CreateOrb(OrbElement orbElement, Transform player, Transform projectileFolder)
        {
            return Instantiate(_orbPrefabs[(int)orbElement], transform)
                    .GetComponent<OrbBase>()
                    .Initialize(new OrbSetup(player, orbElement), projectileFolder);
        }

        private void KeySwap(OrbElement orbElement)
        {
            _currentOrb.gameObject.SetActive(false);
            _currentOrb = _orbPool[orbElement].Enable(_currentOrb.transform.position, _currentOrb.OrbState);
        }

        private void ScrollSwap(int scroll)
        {
            int element = (int)_currentOrb.OrbElement + scroll;
            KeySwap(
                element == _orbPrefabs.Length 
                    ? 0
                    : element == -1 
                        ? (OrbElement)_orbPrefabs.Length - 1
                        : (OrbElement)element
            );
        }
    }
}
