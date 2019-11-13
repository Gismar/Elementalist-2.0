using UnityEngine;

namespace Elementalist.Orbs
{
    public struct OrbSetup
    {
        public Transform Player { get; }
        //Global Dater Handler
        public OrbState OrbState { get; }
        public OrbElement OrbElement { get; }

        public OrbSetup(Transform player, /*Global Data Handler*/ OrbElement orbElement, OrbState orbState = OrbState.Orbiting)
        {
            Player = player;
            OrbState = orbState;
            OrbElement = orbElement;
        }
    }
}