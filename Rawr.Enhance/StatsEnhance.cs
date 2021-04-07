using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    public class StatsEnhance : Stats
    {
        public float BonusSearingFlamesModifier { get; set; }

        public float ConcussionMultiplier { get; set; }
        public float ShieldBonus { get; set; }
        public float CallofFlameBonus { get; set; }
        public float BonusWindfuryDamageMultiplier { get; set; }
        public float BonusStormstrikeDamageMultiplier { get; set; }
        public float BonusLavaLashDamageMultiplier { get; set; }
        public float BonusLightningBoltDamageMultiplier { get; set; }
        public float T11BonusLavaLashDamageMultiplier { get; set; }

        public float PhysicalDamageProcs { get; set; }
    }
}
