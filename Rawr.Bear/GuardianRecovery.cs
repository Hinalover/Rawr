using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class GuardianRecovery
    {
        public float TotalHealingReceived = 0;
        public float TotalHPS = 0;
        public float ThreatPoints = 0;

        public GuardianRecovery(ref CharacterCalculationsBear calcs, Attack bossAttack, float fightlength, GuardianCombatState CombatState, CalculationOptionsBear calcOpts)
        {
            TotalHealingReceived = calcs.Rotation.FrenziedRegen.getTotalHealthRestore
                                 + calcs.Rotation.LeaderOfThePack.getTotalHealthRestore
                                 + calcs.Rotation.Renewal.getTotalHealthRestore
                                 + calcs.Rotation.CenarionWard.getTotalHealthRestore
                                 + calcs.Rotation.HealingTouchWithNaturesSwifteness.getTotalHealthRestore;

            TotalHPS = TotalHealingReceived / fightlength;
            float damageTaken = (bossAttack.DamagePerHit / (bossAttack.AttackSpeed / (1f - CombatState.Stats.BossAttackSpeedReductionMultiplier))) * calcs.Mitigation.DamageTaken;
            ThreatPoints = (TotalHPS / damageTaken);
        }
    }
}
