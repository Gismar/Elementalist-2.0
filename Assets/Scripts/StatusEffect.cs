using System;

namespace Elementalist
{
    [Flags]
    public enum StatusEffects
    {
        None = 1 << 0,
        Stunned = 1 << 1,
        Slowed = 1 << 2,
        Knockedback = 1 << 3,
        Invicibile = 1 << 4,
        Ignited = 1 << 5,
        Drenched = 1 << 6
    }
}
