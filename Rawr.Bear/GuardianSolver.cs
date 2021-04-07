using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class GuardianSolver
    {
        public void Solve(Character character, BossOptions bossOpts, CalculationOptionsBear calcOpts, ref CharacterCalculationsBear calcs, Attack bossAttack)
        {
            DruidTalents talents = character.DruidTalents;

            float trinketDPS = 0.0f;
            float baseAttackPower = calcs.AttackPower;
            float baseHealingPower = calcs.HealingPower;
            float baseSpellPower = calcs.SpellPower;
            float baseHit = Math.Max(0, calcs.PhysicalHitCap - calcs.PhysicalHit);
            float baseDodge = Math.Max(0, calcs.DodgeCap - calcs.PhysicalDodge);
            float baseParry = Math.Max(0, calcs.ParryCap - calcs.PhysicalParry);
            float baseCrit = calcs.PhysicalCrit;
            float baseHaste = calcs.PhysicalHaste;
            float baseMastery = calcs.Mastery;
            float baseMovementSpeed = calcs.MovementSpeed;
            StatsBear baseStats = calcs.BasicStats;

            float fightLength = (calcOpts.UseBossHandler ? bossOpts.BerserkTimer : calcOpts.FightLength);
            float bossArmor = (calcOpts.UseBossHandler ? (float)bossOpts.Armor : BaseCombatRating.Get_BossArmor(calcOpts.TargetLevel));
            int characterLevel = (calcOpts.UseBossHandler ? character.Level : calcOpts.CharacterLevel);
            int targetLevel = (calcOpts.UseBossHandler ? bossOpts.Level : calcOpts.TargetLevel);

            GaurdianWeapon mainHand = new GaurdianWeapon(character.MainHand, baseStats, calcOpts, bossOpts, talents, baseDodge, baseParry,
                baseHit, baseCrit, baseHaste, baseMastery, baseAttackPower, baseAttackPower, baseSpellPower, baseHealingPower, baseMovementSpeed);

            GuardianCombatState CombatState = new GuardianCombatState()
            {
                Char = character,
                CharacterLevel = characterLevel,
                TargetLevel = targetLevel,
                Talents = talents,
                BossArmor = bossArmor,
                AttackingFromBehind = false,
                MainHand = mainHand,
                Spec = FeralRotationType.Guardian,
                Stats = calcs.BasicStats,
                PTR = calcOpts.PTRMode,
            };

            calcs.Rotation = new GuardianRotation(CombatState, fightLength, calcOpts.GuardianSymbiosis);
            
            #region Proc calculations
            float currentTrinketDPS = trinketDPS;
            float currentAttackPower = baseAttackPower;
            float currentHealingPower = baseHealingPower;
            float currentSpellPower = baseSpellPower;
            float currentHit = baseHit;
            float currentDodge = baseDodge;
            float currentParry = baseParry;
            float currentCrit = baseCrit;
            float currentHaste = baseHaste;
            float currentMastery = baseMastery;
            float currentMovementSpeed = baseMovementSpeed;
            StatsBear currentStats = baseStats;

            // Create the dictionaries to be fed to the special effect processing system:
            // Trigger intervals (how often between potentially triggering events)
            Dictionary<Trigger, float> intervals = new Dictionary<Trigger, float>
                {
                    { Trigger.Use, 0 },
                    { Trigger.MeleeAttack, 1.5f },
                    { Trigger.MeleeHit, calcs.Rotation.HitInterval },
                    { Trigger.PhysicalHit, calcs.Rotation.HitInterval },
                    { Trigger.PhysicalAttack, 1.5f },
                    { Trigger.MeleeCrit, calcs.Rotation.CritInterval },
                    { Trigger.PhysicalCrit, calcs.Rotation.CritInterval },
                    { Trigger.DoTTick, calcs.Rotation.DotTickInterval },
                    { Trigger.PhysicalHitorDoTTick, calcs.Rotation.HitorDoTTickInterval },
                    { Trigger.MeleeHitorDoTTick, calcs.Rotation.HitorDoTTickInterval },
                    { Trigger.DamageDone, calcs.Rotation.HitInterval },
                    { Trigger.DamageOrHealingDone, calcs.Rotation.HitInterval },
                    { Trigger.WhiteAttack, CombatState.MainHand.hastedSpeed },
                    { Trigger.WhiteHit, CombatState.MainHand.hastedSpeed / calcs.Rotation.Melee.HitChance },
                    { Trigger.WhiteCrit, CombatState.MainHand.hastedSpeed / calcs.Rotation.Melee.CritChance },
                    { Trigger.MangleBearHit, calcs.Rotation.MangleAbilityInterval },
                    { Trigger.MangleCatOrShredOrInfectedWoundsHit, calcs.Rotation.MangleAbilityInterval },
                    { Trigger.DamageTakenPutsMeBelow35PercHealth, (calcOpts.HitsToLive * 0.65f * bossAttack.AttackSpeed) },
                    { Trigger.DamageTakenPutsMeBelow50PercHealth, (calcOpts.HitsToLive * 0.50f * bossAttack.AttackSpeed) },
                };

            // Trigger chances (how likely the triggering event will occur when the interval elapses)
            Dictionary<Trigger, float> chances = new Dictionary<Trigger, float>
                {
                    { Trigger.Use, 1f },
                    { Trigger.MeleeAttack, 1f },
                    { Trigger.MeleeHit, calcs.Rotation.HitChance },
                    { Trigger.PhysicalHit, calcs.Rotation.HitChance },
                    { Trigger.PhysicalAttack, 1f },
                    { Trigger.MeleeCrit, calcs.Rotation.CritChance },
                    { Trigger.PhysicalCrit, calcs.Rotation.CritChance },
                    { Trigger.DoTTick, calcs.Rotation.HitChance },
                    { Trigger.PhysicalHitorDoTTick, calcs.Rotation.HitChance },
                    { Trigger.MeleeHitorDoTTick, calcs.Rotation.HitChance },
                    { Trigger.DamageDone, calcs.Rotation.HitChance },
                    { Trigger.DamageOrHealingDone, calcs.Rotation.HitChance },
                    { Trigger.WhiteAttack, 1f },
                    { Trigger.WhiteHit, calcs.Rotation.Melee.HitChance },
                    { Trigger.WhiteCrit, calcs.Rotation.Melee.CritChance },
                    { Trigger.MangleBearHit, calcs.Rotation.HitChance },
                    { Trigger.MangleCatOrShredOrInfectedWoundsHit, calcs.Rotation.HitChance },
                    { Trigger.DamageTakenPutsMeBelow35PercHealth, 1f },
                    { Trigger.DamageTakenPutsMeBelow50PercHealth, 1f },
                };

            // Process all special effects and add their stats to the current stats
            foreach (SpecialEffect effect in calcs.BasicStats.SpecialEffects(se => se.AttackPowerScaling == 0))
            {
                Stats averageStats = new Stats();
                {
                    if (effect.Stats._rawSpecialEffectDataSize > 0)
                    {
                        // If there is an effect within the effect, Model it
                        for (int j = 0; j < effect.Stats._rawSpecialEffectDataSize; j++)
                        {
                            if (intervals.ContainsKey(effect.Stats._rawSpecialEffectData[j].Trigger))
                            {
                                SpecialEffect inner = effect.Stats._rawSpecialEffectData[j];
                                float upTime = effect.GetAverageUptime(intervals[effect.Trigger], chances[effect.Trigger], 1f, 1f, fightLength);
                                averageStats.Accumulate(inner.GetAverageStats(intervals, chances, 1f, 1 + currentHaste, fightLength, 1f), upTime);
                            }
                        }
                    }
                    else if (effect.Stats.MoteOfAnger > 0)
                    {
                        // When in effect stats, MoteOfAnger is % of melee hits
                        // When in character stats, MoteOfAnger is average procs per second
                        averageStats.MoteOfAnger = effect.Stats.MoteOfAnger * effect.GetAverageProcsPerSecond(intervals[effect.Trigger],
                            chances[effect.Trigger], 1f, currentHaste, fightLength) / effect.MaxStack;
                    }
                    else
                    {
                        averageStats.Accumulate(effect.GetAverageStats(intervals, chances, 1f, 1 + currentHaste, fightLength, 1f));
                    }
                }
                float procStamina = (float)Math.Floor(averageStats.Stamina * (1 + averageStats.BonusStaminaMultiplier));
                float procAgi = (float)Math.Floor((averageStats.Agility + averageStats.HighestStat) * (1 + calcs.BasicStats.BonusAgilityMultiplier));
                float procStr = (float)Math.Floor(averageStats.Strength * (1 + calcs.BasicStats.BonusStrengthMultiplier));
                float procInt = (float)Math.Floor(averageStats.Intellect * (1 + calcs.BasicStats.BonusIntellectMultiplier));
                float procHealth = (float)Math.Round(averageStats.Health + StatConversion.GetHealthFromStamina(procStamina, characterLevel));
                procHealth = (float)Math.Floor(procHealth * (1f + averageStats.BonusHealthMultiplier));
                float procAP = (float)Math.Floor((averageStats.AttackPower + (((procAgi > 0) ? ((procAgi - 10) * 2) : 0) + ((procStr > 0) ? (procStr - 10) : 0))) * (1 + calcs.BasicStats.BonusAttackPowerMultiplier));
                float procCrit = StatConversion.GetPhysicalCritFromRating(averageStats.CritRating) + StatConversion.GetPhysicalCritFromAgility(procAgi, CharacterClass.Druid) + averageStats.PhysicalCrit;
                float procHaste = StatConversion.GetPhysicalHasteFromRating(averageStats.HasteRating) + averageStats.PhysicalHaste;
                float procMastery = (StatConversion.GetMasteryFromRating(averageStats.MasteryRating) + averageStats.Mastery) * calcs.MasteryPerRating;
                float procHealingPower = (float)Math.Floor((procAgi + procInt) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
                float procSpellPower = (float)Math.Floor((procInt) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
                float procMovementSpeed = (float)Math.Floor(averageStats.MovementSpeed);
                float procHit = StatConversion.GetPhysicalHitFromRating(averageStats.HitRating, calcs.CharacterLevel) + averageStats.PhysicalHit;
                float procExp = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(averageStats.ExpertiseRating, calcs.CharacterLevel) + averageStats.Expertise, CharacterClass.Druid);
                float procDodge = (calcs.PhysicalDodge + procExp > calcs.DodgeCap ? calcs.DodgeCap - calcs.PhysicalDodge : procExp);
                procExp -= procDodge;
                float procParry = (procExp != 0 ? (calcs.PhysicalParry + procExp > calcs.ParryCap ? calcs.ParryCap - calcs.PhysicalParry : procExp) : 0f);

                averageStats.Agility = procAgi;
                averageStats.Strength = procStr;
                averageStats.Intellect = procInt;
                averageStats.Stamina = procStamina;
                averageStats.Health = procHealth;
                currentAttackPower += procAP;
                currentHealingPower += procHealingPower;
                currentSpellPower += procSpellPower;
                currentCrit += procCrit;
                currentHaste += procHaste;
                currentMastery += procMastery;
                currentMovementSpeed += procMovementSpeed;
                currentHit = (float)Math.Max(0, currentHit - procHit);
                currentDodge = (float)Math.Max(0, currentDodge - procDodge);
                currentParry = (float)Math.Max(0, currentParry - procParry);
                currentStats.Accumulate(averageStats);

                // Highest secondary stat handling
                if (calcs.BasicStats.CritRating > calcs.BasicStats.HasteRating && calcs.BasicStats.CritRating > calcs.BasicStats.MasteryRating)
                {
                    currentCrit += StatConversion.GetPhysicalCritFromRating(averageStats.HighestSecondaryStat);
                }
                else if (calcs.BasicStats.HasteRating > calcs.BasicStats.CritRating && calcs.BasicStats.HasteRating > calcs.BasicStats.MasteryRating)
                {
                    currentHaste += StatConversion.GetPhysicalHasteFromRating(averageStats.HighestSecondaryStat);
                }
                else
                {
                    currentMastery += StatConversion.GetMasteryFromRating(averageStats.HighestSecondaryStat) * calcs.MasteryPerRating;
                }

                // Skull Banner: +20% critical damage (10 sec, 3 min)
                calcs.BasicStats.BonusCritDamageMultiplier = (1 + calcs.BasicStats.BonusCritDamageMultiplier) * (1 + averageStats.BonusCritDamageMultiplier) - 1;
                currentTrinketDPS += (averageStats.PhysicalDamage * (1 + calcs.BasicStats.BonusPhysicalDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) +
                    (averageStats.HolyDamage * (1 + calcs.BasicStats.BonusHolyDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    (averageStats.NatureDamage * (1 + calcs.BasicStats.BonusNatureDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    (averageStats.ArcaneDamage * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    (averageStats.ShadowDamage * (1 + calcs.BasicStats.BonusShadowDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    (averageStats.FireDamage * (1 + calcs.BasicStats.BonusFireDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    (averageStats.FrostDamage * (1 + calcs.BasicStats.BonusFrostDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)));
            }

            // Process damage special effects that scale with attack power here
            foreach (SpecialEffect effect in calcs.BasicStats.SpecialEffects(se => se.AttackPowerScaling > 0))
            {
                float estimateVengengeAP = currentAttackPower * 1.9f;
                float AttackPowerBoost = estimateVengengeAP * effect.AttackPowerScaling;
                float ePhysicalDamage = effect.Stats.PhysicalDamage;
                float eHolyDamge = effect.Stats.HolyDamage;
                float eNatureDamage = effect.Stats.NatureDamage;
                float eArcaneDamage = effect.Stats.ArcaneDamage;
                float eShadowDamage = effect.Stats.ShadowDamage;
                float eFireDamage = effect.Stats.FireDamage;
                float eFrostDamage = effect.Stats.FrostDamage;

                // Add the attack power to the effect before calculating the stat
                if (ePhysicalDamage > 0)
                    ePhysicalDamage += AttackPowerBoost;
                if (eHolyDamge > 0)
                    eHolyDamge += AttackPowerBoost;
                if (eNatureDamage > 0)
                    eNatureDamage += AttackPowerBoost;
                if (eArcaneDamage > 0)
                    eArcaneDamage += AttackPowerBoost;
                if (eShadowDamage > 0)
                    eShadowDamage += AttackPowerBoost;
                if (eFireDamage > 0)
                    eFireDamage += AttackPowerBoost;
                if (eFrostDamage > 0)
                    eFrostDamage += AttackPowerBoost;

                Stats averageStats = effect.GetAverageStats(intervals, chances, 1f, currentHaste, fightLength, 1f);

                // Average out the Attack Power modified values against the averageStats values;
                float aPhysicalDamage, aHolyDamage, aNatureDamage, aArcaneDamage, aShadowDamage, aFireDamage, aFrostDamage = 0;
                aPhysicalDamage = (ePhysicalDamage / (effect.Stats.PhysicalDamage / averageStats.PhysicalDamage));
                aHolyDamage = (eHolyDamge / (effect.Stats.PhysicalDamage / averageStats.PhysicalDamage));
                aNatureDamage = (eNatureDamage / (effect.Stats.PhysicalDamage / averageStats.PhysicalDamage));
                aArcaneDamage = (eArcaneDamage / (effect.Stats.PhysicalDamage / averageStats.PhysicalDamage));
                aShadowDamage = (eShadowDamage / (effect.Stats.PhysicalDamage / averageStats.PhysicalDamage));
                aFireDamage = (eFireDamage / (effect.Stats.PhysicalDamage / averageStats.PhysicalDamage));
                aFrostDamage = (eFrostDamage / (effect.Stats.PhysicalDamage / averageStats.PhysicalDamage));

                currentTrinketDPS += ((averageStats.PhysicalDamage + aPhysicalDamage) * (1 + calcs.BasicStats.BonusPhysicalDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) +
                    ((averageStats.HolyDamage + aHolyDamage) * (1 + calcs.BasicStats.BonusHolyDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    ((averageStats.NatureDamage + aNatureDamage) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    ((averageStats.ArcaneDamage + aArcaneDamage) * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    ((averageStats.ShadowDamage + aShadowDamage) * (1 + calcs.BasicStats.BonusShadowDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    ((averageStats.FireDamage + aFireDamage) * (1 + calcs.BasicStats.BonusFireDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                    ((averageStats.FrostDamage + aFrostDamage) * (1 + calcs.BasicStats.BonusFrostDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)));
            }


            CombatState.MainHand.AttackPower = currentAttackPower;
            CombatState.MainHand.VengenceAttackPower = currentAttackPower;
            CombatState.MainHand.HealingPower = currentHealingPower;
            CombatState.MainHand.CriticalStrike = currentCrit;
            CombatState.MainHand.Haste = currentHaste;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
            CombatState.MainHand.Mastery = currentMastery;
            CombatState.MainHand.MovementSpeed = currentMovementSpeed;
            CombatState.MainHand.Hit = currentHit;
            CombatState.MainHand.chanceDodged = currentDodge;
            CombatState.MainHand.chanceParried = currentParry;
            CombatState.Stats = currentStats;

            currentTrinketDPS = (CombatState.MainHand.CritDamageMultiplier * currentTrinketDPS * CombatState.MainHand.CriticalStrike) + (currentTrinketDPS * ((1 - CombatState.MainHand.Hit) - CombatState.MainHand.CriticalStrike));

            calcs.Rotation.TrinketDPS = currentTrinketDPS;
            calcs.Rotation.updateCombatState(CombatState);
            #endregion

            calcs.Rotation.generateDefensiveCooldownInfo();

            calcs.Mitigation = new GuardianMitigation(character, ref calcs, characterLevel, targetLevel, CombatState, bossAttack, fightLength, calcOpts);

            calcs.MitigationPoints = StatConversion.getMitigationScaler(characterLevel) / (1 - calcs.Mitigation.TotalMitigation);

            calcs.SurvivabilityPoints = calcs.Mitigation.TotalSurvivability;

            calcs.ThreatPoints = calcs.Rotation.totalDPS() * (1 + (((calcs.Rotation.totalTPS() / calcs.Rotation.totalDPS()) - 1) * calcOpts.MitigationOrDPS));

            calcs.Recovery = new GuardianRecovery(ref calcs, bossAttack, fightLength, CombatState, calcOpts);

            calcs.RecoveryPoints = (StatConversion.getMitigationScaler(characterLevel) * calcs.Recovery.ThreatPoints) * calcOpts.HitsToLive;
        }
    }
}