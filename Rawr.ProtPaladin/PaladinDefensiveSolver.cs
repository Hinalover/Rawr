using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.ProtPaladin
{
    public class PaladinDefensiveSolver
    {
        public Dictionary<Trigger, float> TriggerIntervals { get; set; }
        public Dictionary<Trigger, float> TriggerChances { get; set; }

        private SpecialEffect _sotrSpecialEffect = new SpecialEffect { Duration = 3f, Chance = 1f, Cooldown = 0f, Stats = new Stats { DamageTakenReductionMultiplier = 0.3f }, Trigger = Trigger.Use };

        public void Solve(Character character, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts, ref CharacterCalculationsProtPaladin calcs, Attack bossAttack, float SotRInterval)
        {
            TriggerIntervals = new Dictionary<Trigger, float>();
            TriggerChances = new Dictionary<Trigger, float>();
            // Basic calculations: Damage per hit, damage taken per second
            float baseDamagePerSwing = bossAttack.DamagePerHit;
            float baseSwingSpeed = bossAttack.AttackSpeed;

            // SotR buff calculations
            _sotrSpecialEffect.Stats.DamageTakenReductionMultiplier = 0.3f + calcs.Mastery * 0.01f;
            _sotrSpecialEffect.Cooldown = SotRInterval;
            float sotrDamageReduction = (1 - _sotrSpecialEffect.GetAverageStats().DamageTakenReductionMultiplier);

            // Maintain Weakened Blows ourselves - that's the final (1 - 0.1f)
            float armorDamageReduction = (1 - StatConversion.GetArmorDamageReduction(bossOpts.Level, character.Level, calcs.BasicStats.Armor, 0, 0));
            float totalDamageReduction = (1 - calcs.BasicStats.DamageTakenReductionMultiplier) * (1 - calcs.BasicStats.PhysicalDamageTakenReductionMultiplier) * armorDamageReduction
                * (1 - 0.1f) * sotrDamageReduction;
            float damagePerHitAfterArmor = baseDamagePerSwing * totalDamageReduction;
            // First roll: Miss/dodge/parry
            float unavoidedAttacks = Math.Max(0f, 1 - calcs.Miss - calcs.Dodge - calcs.Parry);
            // Second roll: Block
            float attacksHit = unavoidedAttacks * Math.Max(0f, 1 - calcs.Block);
            float attacksBlocked = unavoidedAttacks * Math.Min(1, calcs.Block);
            float averageDamagePerHit = damagePerHitAfterArmor * attacksHit + damagePerHitAfterArmor * (0.7f - calcs.BasicStats.BonusBlockValueMultiplier) * attacksBlocked;

            float finalSwingSpeed = bossAttack.AttackSpeed / (1 - calcs.BasicStats.BossAttackSpeedReductionMultiplier);

            float damageTakenPerSecond = averageDamagePerHit / finalSwingSpeed;

            // Vengeance
            float vengeanceAP = 0.018f * (bossAttack.DamagePerHit / finalSwingSpeed) * 20f;
            calcs.AverageVengeanceAP = vengeanceAP * (1 + calcs.BasicStats.BonusAttackPowerMultiplier);

            // Survivability
            float rawSurvivability = (calcs.BasicStats.Health + calcs.BasicStats.DamageAbsorbed) / totalDamageReduction;
            float cappedSurvivability = SoftCapSurvival(calcOpts, baseDamagePerSwing, rawSurvivability);

            // Mitigation
            float hitWeight = attacksHit * totalDamageReduction;
            float blockWeight = attacksBlocked * (0.7f - calcs.BasicStats.BonusBlockValueMultiplier) * totalDamageReduction;
            float damageTaken = (hitWeight + blockWeight) * (1 - calcs.BasicStats.BossAttackSpeedReductionMultiplier);
            float totalMitigation = 1 - damageTaken;

            // Recovery

            // Output stats
            calcs.AttackerSpeed = finalSwingSpeed;
            calcs.DTPS = damageTakenPerSecond;
            calcs.DamagePerHit = damagePerHitAfterArmor;
            calcs.DamagePerBlock = damagePerHitAfterArmor * (0.7f - calcs.BasicStats.BonusBlockValueMultiplier);
            calcs.GuaranteedReduction = totalDamageReduction;

            calcs.MitigationPoints = StatConversion.getMitigationScaler(character.Level) / (1 - totalMitigation);
            calcs.TotalMitigation = totalMitigation;
            calcs.SurvivabilityPoints = cappedSurvivability;
            calcs.RecoveryPoints = 0;

            // Create triggers
            TriggerIntervals.Add(Trigger.DamageAvoided, finalSwingSpeed);
            TriggerIntervals.Add(Trigger.DamageBlocked, finalSwingSpeed);
            TriggerIntervals.Add(Trigger.DamageDodged, finalSwingSpeed);
            TriggerIntervals.Add(Trigger.DamageParried, finalSwingSpeed);
            TriggerIntervals.Add(Trigger.DamageTaken, finalSwingSpeed);
            TriggerIntervals.Add(Trigger.DamageTakenPhysical, finalSwingSpeed);
            TriggerChances.Add(Trigger.DamageAvoided, 1 - unavoidedAttacks);
            TriggerChances.Add(Trigger.DamageBlocked, attacksBlocked);
            TriggerChances.Add(Trigger.DamageDodged, calcs.Dodge);
            TriggerChances.Add(Trigger.DamageParried, calcs.Parry);
            TriggerChances.Add(Trigger.DamageTaken, unavoidedAttacks);
            TriggerChances.Add(Trigger.DamageTakenPhysical, unavoidedAttacks);
        }

        private float SoftCapSurvival(CalculationOptionsProtPaladin calcOpts, float attackValue, float origValue)
        {
            float cappedValue = origValue;
            //
            double survivalCap = ((double)attackValue * (double)calcOpts.HitsToSurvive) / 1000d;

            double survivalRaw = origValue / 1000f;

            //Implement Survival Soft Cap
            if (survivalRaw <= survivalCap)
            {
                cappedValue = 1000f * (float)survivalRaw;
            }
            else
            {
                double x = survivalRaw;
                double cap = survivalCap;
                double fourToTheNegativeFourThirds = Math.Pow(4d, -4d / 3d);
                double topLeft = Math.Pow(((x - cap) / cap) + fourToTheNegativeFourThirds, 1d / 4d);
                double topRight = Math.Pow(fourToTheNegativeFourThirds, 1d / 4d);
                double fracTop = topLeft - topRight;
                double fraction = fracTop / 2d;
                double y = (cap * fraction + cap);
                cappedValue = (float)y * 1000f;
            }
            return cappedValue;
        }
    }
}
