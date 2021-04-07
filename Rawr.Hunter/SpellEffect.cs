using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Hunter
{
    public class SpellEffect
    {
        EffectType type;
        EffectSubType subType;
    }

    public class WeaponDamageEffect : SpellEffect
    {
        float damageLow;

        public float DamageLow
        {
            get { return damageLow; }
            set { damageLow = value; }
        }

        float damageHigh;

        public float DamageHigh
        {
            get { return damageHigh; }
            set { damageHigh = value; }
        }

        public float DamageAvg
        {
            get { return (DamageHigh + DamageLow) / 2f; }
        }

    }

    public class DoTEffect : SpellEffect
    {
        int duration;

    }
}
