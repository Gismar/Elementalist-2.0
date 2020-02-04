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
            _orbPool = new Dictionary<OrbElement, OrbBase>
            {
                [OrbElement.Water] = CreateOrb(OrbElement.Water, player),
                [OrbElement.Fire] = CreateOrb(OrbElement.Fire, player),
                [OrbElement.Earth] = CreateOrb(OrbElement.Earth, player),
                [OrbElement.Lightning] = CreateOrb(OrbElement.Lightning, player),
                [OrbElement.Air] = CreateOrb(OrbElement.Air, player)
            };

            _swapKeys = new Dictionary<KeyCode, OrbElement>
            {
                [KeyCode.Q] = OrbElement.Water,
                [KeyCode.Alpha1] = OrbElement.Fire,
                [KeyCode.Alpha2] = OrbElement.Earth,
                [KeyCode.Alpha3] = OrbElement.Lightning,
                [KeyCode.E] = OrbElement.Air
            };

            _currentOrb = _orbPool[OrbElement.Water];
            return this;
        }

        private void Start() => KeySwap(OrbElement.Water);

        private void Update()
        {
            if (_currentOrb.OrbState != OrbState.Attacking)
            {
                KeyValuePair<KeyCode, OrbElement> keyPressed = _swapKeys.FirstOrDefault(k => Input.GetKeyDown(k.Key));
                if (!keyPressed.Equals(default(KeyValuePair<KeyCode, OrbElement>)))
                    KeySwap(keyPressed.Value);

                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0)
                    ScrollSwap(Mathf.FloorToInt(Mathf.Sign(scroll)));
            }
        }

        private OrbBase CreateOrb(OrbElement orbElement, Transform player) 
            => Instantiate(_orbPrefabs[(int)orbElement], transform)
                .GetComponent<OrbBase>()
                .Initialize(new OrbSetup(player, orbElement));

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
