using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class GuardianMitigation
    {
        public float DodgeFromAgility = 0;
        public float dodgeFromBonusAgility = 0;
        public float DodgeFromDodgeRating = 0;
        public float dodgeThatsNotAffectedByDR = 0;
        public float dodgeBeforeDRApplied = 0;
        public float dodgeAfterDRApplied = 0;
        public float AvoidancePreDR = 0;
        public float AvoidancePostDR = 0;
        public float Miss = 0;
        public float Dodge = 0;
        public float Armor = 0;
        public float PreDRFromArmor = 0;
        public float PostDRFromArmor = 0;
        public float TotalConstantDamageReduction = 0;

        public float TotalSurvivability = 0;

        public float DamageTaken = 0;
        public float TotalMitigation = 0;
        public float MitigatedFromHaste = 0;

        public float Vengence = 0;

        private Stats baseStats;
        private float FightLength = 0;
        private float levelDifferenceAvoidance = 0;
        private float CappedCritReduction = 0;
        private float PostDamageReductionBossAttackDamage = 0;

        public GuardianMitigation(Character character, ref CharacterCalculationsBear calcs, int CharacterLevel, int TargetLevel, GuardianCombatState CombatState, Attack bossAttack, float fightLength, CalculationOptionsBear calcOpts)
        {
            baseStats = BaseStats.GetBaseStats(character.Level, character.Class, character.Race, BaseStats.DruidForm.Bear);
            float levelDifference = TargetLevel - CharacterLevel;
            FightLength = fightLength;
            levelDifferenceAvoidance = levelDifference * 0.002f;

            #region Calculate Dodge DR
            // This is the Diminishing Returns' "k" value that changes based on what class the user is
            float DR_k = StatConversion.DR_COEFFIENT[11];
            // This is the % cap for dodge 
            float DR_C_d = StatConversion.CAP_DODGE[11]; 
            float DR_miss_cap = 16f;

            float dodgeFromBaseAgility = (baseStats.Agility / BaseCombatRating.DruidAgilityNeededForOnePercentDodge(CharacterLevel)) * 0.01f;
            dodgeFromBonusAgility = ((CombatState.Stats.Agility - baseStats.Agility) / BaseCombatRating.DruidAgilityNeededForOnePercentDodge(CharacterLevel)) * 0.01f;
            DodgeFromAgility = (CombatState.Stats.Agility / BaseCombatRating.DruidAgilityNeededForOnePercentDodge(CharacterLevel)) * 0.01f;
            DodgeFromDodgeRating = StatConversion.GetDodgeFromRating(CombatState.Stats.DodgeRating, CharacterLevel);

            dodgeThatsNotAffectedByDR = (CombatState.Stats.Dodge - (float)Math.Max(0, levelDifference * .015)) + dodgeFromBaseAgility;
            //float missThatsNotAffectedByDR = (float)Math.Max(0, CombatState.Stats.Miss - (float)Math.Max(0, levelDifference * .015));
            dodgeBeforeDRApplied = dodgeFromBonusAgility + DodgeFromDodgeRating;
            float missBeforeDRApplied = 0f;
            // (1/C_d + (k / ((Agility - baseAgility/a) + preDodge)))^-1
            dodgeAfterDRApplied = (float)Math.Pow(((1f / DR_C_d) + (DR_k / dodgeBeforeDRApplied)),-1);
            float missAfterDRApplied = (float)Math.Pow(((1f / DR_miss_cap) + (DR_k / missBeforeDRApplied)),-1);
            float dodgeTotal = dodgeThatsNotAffectedByDR + dodgeAfterDRApplied + calcs.Rotation.SavageDefense.AverageDodge + calcs.Rotation.ElusiveBrew.AverageDodge;
            //float missTotal = missThatsNotAffectedByDR + missAfterDRApplied;

            Armor = CombatState.Stats.Armor * (1 + CombatState.MainHand.Mastery);

            float CooldownDamageReduction = calcs.Rotation.Barkskin.AverageDamageReduction
                                   + calcs.Rotation.SurvivalInstincts.AverageDamageReduction
                                   + calcs.Rotation.BoneShield.AverageDamageReduction;

            Miss = 0;// missTotal;
            Dodge = (float)Math.Min(1 - Miss, dodgeTotal);
            PreDRFromArmor = StatConversion.GetDamageReductionFromArmor(CharacterLevel, Armor);
            PostDRFromArmor = StatConversion.GetDamageReductionFromArmor(TargetLevel, Armor);
            TotalConstantDamageReduction = 1f - (1f - PostDRFromArmor)
                                         * (1f - CombatState.Stats.PhysicalDamageTakenReductionMultiplier)
                                         * (1f - CombatState.Stats.DamageTakenReductionMultiplier)
                                         * (1f - CombatState.Stats.BossPhysicalDamageDealtReductionMultiplier)
                                         * (1f - CooldownDamageReduction);
            PostDamageReductionBossAttackDamage = (1 - TotalConstantDamageReduction) * (calcOpts.UseBossHandler ? bossAttack.DamagePerHit : calcOpts.BossUnmitigatedDamage);
            float absorbs = CombatState.Stats.DamageAbsorbed + (PostDamageReductionBossAttackDamage * CombatState.Stats.DamageAbsorbedFromDamageTaken);
            AvoidancePreDR = dodgeThatsNotAffectedByDR + dodgeBeforeDRApplied + calcs.Rotation.SavageDefense.AverageDodge + calcs.Rotation.ElusiveBrew.AverageDodge;
            AvoidancePostDR = dodgeTotal;
            CappedCritReduction = Math.Min(0.04f + levelDifferenceAvoidance, CombatState.Stats.CritChanceReduction);
            #endregion

            #region Vengeance
            //float critsVeng = Math.Min(Math.Max(0f, 1f - AvoidancePostDR), (0.04f + levelDifferenceAvoidance) - CappedCritReduction);
            //float hitsVeng = Math.Max(0f, 1f - (critsVeng + AvoidancePostDR));
            //Apply armor and multipliers for each attack type...
            //critsVeng *= (1f - TotalConstantDamageReduction) * 2f;
            //hitsVeng *= (1f - TotalConstantDamageReduction);

            float damageTakenPercent = /*(hitsVeng + critsVeng) * */ (1f - CombatState.Stats.BossAttackSpeedReductionMultiplier);
            float damageTakenPerHit = (calcOpts.UseBossHandler ? bossAttack.DamagePerHit : calcOpts.BossUnmitigatedDamage) * damageTakenPercent;
            float damageTakenPerSecond = damageTakenPerHit / ((calcOpts.UseBossHandler ? bossAttack.AttackSpeed : calcOpts.BossSwingSpeed) / (1 - CombatState.Stats.BossAttackSpeedReductionMultiplier));
            float vengeanceAP = 0.018f * damageTakenPerSecond * 20f;
            /*
            // == Evaluate damage taken once ahead of time for vengeance ==
            //Out of 100 attacks, you'll take...
            float critsVeng = Math.Min(Math.Max(0f, 1f - AvoidancePostDR), (0.04f + levelDifferenceAvoidance) - CappedCritReduction);
            float hitsVeng = Math.Max(0f, 1f - (critsVeng + AvoidancePostDR));
            //Apply armor and multipliers for each attack type...
            critsVeng *= (1f - TotalConstantDamageReduction) * 2f;
            hitsVeng *= (1f - TotalConstantDamageReduction);

            float damageTakenPercent = (hitsVeng + critsVeng) * (1f - CombatState.Stats.BossAttackSpeedReductionMultiplier);
            float damageTakenPerHit = bossAttack.DamagePerHit * damageTakenPercent;
            float damageTakenPerSecond = damageTakenPerHit / bossAttack.AttackSpeed;
            float damageTakenPerVengeanceTick = damageTakenPerSecond * 2f;
            float vengeanceCap = CombatState.Stats.Stamina + baseStats.Health * 0.1f;
            float vengeanceAPPreAvoidance = Math.Min(vengeanceCap, damageTakenPerVengeanceTick);

            double chanceHit = 1f - AvoidancePostDR;
            double vengeanceMultiplierFromAvoidance = //Best-fit of results from simulation of avoidance effects on vengeance
                -46.288470839554d * Math.Pow(chanceHit, 6)
                + 143.12528411194400d * Math.Pow(chanceHit, 5)
                - 159.9833254324610000d * Math.Pow(chanceHit, 4)
                + 74.0451030489808d * Math.Pow(chanceHit, 3)
                - 10.8422088672455d * Math.Pow(chanceHit, 2)
                + 0.935157126508557d * chanceHit;

            //A percentage of the ticks will be guaranteed decays for attack speeds longer than 2sec, due to no swings occuring between the current and last tick
            float vengeanceMultiplierFromSwingSpeed = bossAttack.AttackSpeed <= 2f ? 1f : (1f - 0.1f * (1f - 2f / bossAttack.AttackSpeed));

            float vengeanceAP = (float)(vengeanceAPPreAvoidance * vengeanceMultiplierFromAvoidance * vengeanceMultiplierFromSwingSpeed);
            */
            Vengence = CombatState.VengenceAttackPower + (vengeanceAP * (1f + CombatState.Stats.BonusAttackPowerMultiplier));
            CombatState.VengenceAttackPower = Vengence;
            
            calcs.Rotation.updateCombatState(CombatState);
            #endregion

            #region Calculate Survivability
            float DamageAbsorbedFromDamageTaken = CombatState.Stats.DamageAbsorbedFromDamageTaken * PostDamageReductionBossAttackDamage;
            float averagehealth = CombatState.Stats.Health * (1 + calcs.Rotation.MightofUrsoc.AverageIncreasedHealth) 
                                + CombatState.Stats.DamageAbsorbed 
                                + DamageAbsorbedFromDamageTaken 
                                + CombatState.Stats.BattlemasterHealthProc; 
            float SurvivalResults = GetSurvival(CombatState.Stats, averagehealth, PostDRFromArmor);
            TotalSurvivability = SoftCapSurvival(calcOpts, (calcOpts.UseBossHandler ? bossAttack.DamagePerHit : calcOpts.BossUnmitigatedDamage), SurvivalResults);
            #endregion

            #region Calculate Mitigation
            float Crit = (float)Math.Min(Math.Max(0f, 1f - AvoidancePostDR), Math.Max(0, (0.04f + levelDifferenceAvoidance) - CappedCritReduction));
            float Hit = (float)Math.Max(0, 1 - (Crit + AvoidancePostDR));

            Crit *= (1 - TotalConstantDamageReduction) * 2;
            Hit *= (1 - TotalConstantDamageReduction);

            DamageTaken = (Hit + Crit) * (1f - CombatState.Stats.BossAttackSpeedReductionMultiplier);

            TotalMitigation = 1f - DamageTaken;
            #endregion
        }

        private float GetSurvival(StatsBear stats, float averageHealth, float ArmorDamageReduction)
        {
            float PhysicalSurvival = averageHealth;

            PhysicalSurvival = GetEffectiveHealth(averageHealth, ArmorDamageReduction, 1.0f);

            // Since Armor plays a role in Survival, so shall the other damage taken adjusters.
            // Note, it's expected that (at least for tanks) that DamageTakenMultiplier will be Negative.
            // JOTHAY NOTE: The above is no longer true. DamageTakenReductionMultiplier will now be positive, but must be handled multiplicatively and inversely
            // JOTHAY NOTE: YOUR FORMULAS HAVE BEEN UPDATED TO REFLECT THIS CHANGE
            // So the next line should INCREASE survival because 
            // fPhysicalSurvival * (1 - [some negative value] * (1 - [0 or some negative value])
            // will look like:
            // fPhysicalSurvival * 1.xx * 1.xx
            PhysicalSurvival =      PhysicalSurvival / ((1f - stats.DamageTakenReductionMultiplier) * (1f - stats.PhysicalDamageTakenReductionMultiplier));

            return PhysicalSurvival;
        }

        private float SoftCapSurvival(CalculationOptionsBear calcOpts, float attackValue, float origValue)
        {
            float cappedValue = origValue;
            //
            double survivalCap = ((double)attackValue * (double)calcOpts.HitsToLive) / 1000d;
            
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

        /// <summary>Get the value for a sub-component of Survival</summary>
        /// <param name="fHealth">Current HP</param>
        /// <param name="fDR">Dam Reduction rate</param>
        /// <param name="PercValue">% value of the survival rank. valid range 0-1</param>
        /// <returns></returns>
        private float GetEffectiveHealth(float health, float dr, float percValue)
        {
            // TotalSurvival == sum(Survival for each school)
            // Survival = (Health / (1 - DR)) * % damage inc from that school
            if (0f <= percValue && percValue <= 1f && dr < 1f)
                return (health / (1 - dr)) * percValue;
            else
                return 0;
        }
    }
}