using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.ProtPaladin
{
    public class ProtPaladinSolver
    {
        public Dictionary<Trigger, float> TriggerIntervals { get; set; }
        public Dictionary<Trigger, float> TriggerChances { get; set; }

        public void Solve(Character character, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts, ref CharacterCalculationsProtPaladin calcs, Attack bossAttack)
        {
            PaladinOffensiveSolver offensiveSolver = new PaladinOffensiveSolver();
            PaladinDefensiveSolver defensiveSolver = new PaladinDefensiveSolver();
            if (character.MainHand != null)
            {
                offensiveSolver.Solve(character, calcOpts, bossOpts, ref calcs);
            }
            defensiveSolver.Solve(character, calcOpts, bossOpts, ref calcs, bossAttack, offensiveSolver.SotRInterval);
            TriggerIntervals = new Dictionary<Trigger, float>(defensiveSolver.TriggerIntervals);
            TriggerChances = new Dictionary<Trigger, float>(defensiveSolver.TriggerChances);
            TriggerIntervals.Add(Trigger.Use, 0);
            TriggerChances.Add(Trigger.Use, 1);
            // Factor Vengeance into the dps calculations
            if (character.MainHand != null)
            {
                TriggerIntervals = TriggerIntervals.Concat(offensiveSolver.TriggerIntervals).ToDictionary(x => x.Key, x => x.Value);
                TriggerChances = TriggerChances.Concat(offensiveSolver.TriggerChances).ToDictionary(x => x.Key, x => x.Value);
                offensiveSolver.Solve(character, calcOpts, bossOpts, ref calcs);
            }
        }

        private bool IsDefensiveTrigger(Trigger trigger)
        {
            return trigger == Trigger.DamageAvoided ||
                trigger == Trigger.DamageBlocked ||
                trigger == Trigger.DamageDodged ||
                trigger == Trigger.DamageParried ||
                trigger == Trigger.DamageTaken ||
                trigger == Trigger.DamageTakenPutsMeBelow35PercHealth ||
                trigger == Trigger.DamageTakenPutsMeBelow50PercHealth;
        }

        private bool IsDefensiveStat(Stats stats)
        {
            return stats.Armor > 0 ||
                stats.BonusArmor > 0 ||
                stats.DamageAbsorbed > 0 ||
                stats.DamageAbsorbedFromDamageTaken > 0 ||
                stats.DamageTakenReductionMultiplier > 0 ||
                stats.Dodge > 0 ||
                stats.DodgeRating > 0 ||
                stats.Health > 0 ||
                stats.HealthRestore > 0 ||
                stats.Mastery > 0 ||
                stats.MasteryRating > 0 ||
                stats.Parry > 0 ||
                stats.ParryRating > 0 ||
                stats.PhysicalDamageTakenReductionMultiplier > 0 ||
                stats.Stamina > 0;
        }
    }
}
