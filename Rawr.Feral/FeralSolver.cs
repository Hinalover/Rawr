using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class FeralSolver
    {
        private static List<FeralRotationList> _feralList = null;
   

        public static List<FeralRotationList> FeralList
        {
            get
            {
                if (_feralList == null)
                {
                    _feralList = new List<FeralRotationList>();
                    //_feralList.Add(new FeralRotationList(true, true, true, 3, true, 5, true, 3, 0));
                    //_feralList.Add(new FeralRotationList(true, true, true, 3, true, 5, true, 3, 35));
                    //_feralList.Add(new FeralRotationList(true, true, true, 3, true, 5, true, 4, 0));
                    //_feralList.Add(new FeralRotationList(true, true, true, 3, true, 5, true, 4, 35));
                    _feralList.Add(new FeralRotationList(true, true, true, 3, true, 5, true, 5, 0));
                    _feralList.Add(new FeralRotationList(true, true, true, 3, true, 5, true, 5, 25));
                    //_feralList.Add(new FeralRotationList(true, true, true, 4, true, 5, true, 3, 0));
                    //_feralList.Add(new FeralRotationList(true, true, true, 4, true, 5, true, 3, 35));
                    //_feralList.Add(new FeralRotationList(true, true, true, 4, true, 5, true, 4, 0));
                    //_feralList.Add(new FeralRotationList(true, true, true, 4, true, 5, true, 4, 35));
                    _feralList.Add(new FeralRotationList(true, true, true, 4, true, 5, true, 5, 0));
                    _feralList.Add(new FeralRotationList(true, true, true, 4, true, 5, true, 5, 25));
                    //_feralList.Add(new FeralRotationList(true, true, true, 5, true, 5, true, 3, 0));
                    //_feralList.Add(new FeralRotationList(true, true, true, 5, true, 5, true, 3, 35));
                    //_feralList.Add(new FeralRotationList(true, true, true, 5, true, 5, true, 4, 0));
                    //_feralList.Add(new FeralRotationList(true, true, true, 5, true, 5, true, 4, 35));
                    _feralList.Add(new FeralRotationList(true, true, true, 5, true, 5, true, 5, 0));
                    _feralList.Add(new FeralRotationList(true, true, true, 5, true, 5, true, 5, 25));
                    
                }
                return _feralList;
            }
        }

        private static List<FeralRotationList> _guardianList = null;
        public static List<FeralRotationList> GuardianList
        {
            get
            {
                if (_guardianList == null)
                {
                    _guardianList = new List<FeralRotationList>();
                    _guardianList.Add(new FeralRotationList(false, false, false, false, true, 5, 0));
                    _guardianList.Add(new FeralRotationList(false, false, false, false, true, 5, 5));
                    _guardianList.Add(new FeralRotationList(false, false, false, false, true, 5, 10));
                    _guardianList.Add(new FeralRotationList(false, false, false, false, true, 5, 15));
                    _guardianList.Add(new FeralRotationList(false, false, false, false, true, 5, 20));
                    _guardianList.Add(new FeralRotationList(false, false, false, false, true, 5, 25));
                    _guardianList.Add(new FeralRotationList(false, false, false, false, true, 5, 30));
                    _guardianList.Add(new FeralRotationList(false, false, false, false, true, 5, 35));
                }
                return _guardianList;
            }
        }

        public void Solve (Character character, BossOptions bossOpts, CalculationOptionsFeral calcOpts, ref CharacterCalculationsFeral calcs)
        {
            DruidTalents talents = character.DruidTalents;

            float trinketDPS = 0.0f;
            float baseAttackPower = calcs.AttackPower;
            float baseHealingPower = calcs.HealingPower;
            float baseHit = Math.Max(0, calcs.PhysicalHitCap - calcs.PhysicalHit);
            float baseDodge = Math.Max(0, calcs.DodgeCap - calcs.Dodge);
            float baseParry = Math.Max(0, calcs.ParryCap - calcs.Parry);
            float baseCrit = calcs.PhysicalCrit;
            float baseHaste = calcs.PhysicalHaste;
            float baseMastery = calcs.Mastery;
            float baseMovementSpeed = calcs.MovementSpeed;

            float fightLength = (calcOpts.UseBossHandler ? bossOpts.BerserkTimer : calcOpts.FightLength);
            float bossArmor = (calcOpts.UseBossHandler ? (float)bossOpts.Armor : BaseCombatRating.Get_BossArmor(calcOpts.TargetLevel));
            float percentBehindBoss = (calcOpts.UseBossHandler ? (float)bossOpts.InBackPerc_Melee : calcOpts.PercentBehindBoss);
            int characterLevel = (calcOpts.UseBossHandler ? character.Level : calcOpts.CharacterLevel);
            int targetLevel = (calcOpts.UseBossHandler ? bossOpts.Level : calcOpts.TargetLevel);

            Item baseMainHand = new Item();
            if (character.MainHand != null)
                baseMainHand = character.MainHand.Item;
            else
            {
                baseMainHand = new Item()
                {
                    Type = ItemType.Polearm,
                    Speed = 1.0f,
                    MinDamage = 1,
                    MaxDamage = 1,
                };
            }
            

            FeralWeapon mainHand = new FeralWeapon(character.MainHand, calcs.BasicStats, calcOpts, bossOpts, 
                            talents, baseDodge, baseParry, baseHit, baseCrit, baseHaste, baseMastery,
                            baseAttackPower, baseHealingPower, baseMovementSpeed);

            FeralCombatState CombatState = new FeralCombatState()
            {
                Char = character,
                CharacterLevel = characterLevel,
                TargetLevel = targetLevel,
                Talents = talents,
                BossArmor = bossArmor,
                AttackingFromBehind = true,
                MainHand = mainHand,
                Prowling = false,
                Spec = FeralRotationType.Feral,
                PTR = calcOpts.PTRMode,
            };
            FeralRotation rotation = new FeralRotation();
            rotation.updateCombatState(CombatState);
            calcs.MaxRotation = rotation;
            FeralRotationList rot = null;
            bool first = true;
            for (int i = 0; i < FeralList.Count; i++)
            {
                CombatState = new FeralCombatState()
                {
                    Char = character,
                    CharacterLevel = characterLevel,
                    TargetLevel = targetLevel,
                    Talents = talents,
                    BossArmor = bossArmor,
                    AttackingFromBehind = true,
                    MainHand = mainHand,
                    Prowling = false,
                    Spec = FeralRotationType.Feral
                };

                float currentAttackPower = baseAttackPower;
                float currentHealingPower = baseHealingPower;
                float currentHit = baseHit;
                float currentDodge = baseDodge;
                float currentParry = baseParry;
                float currentCrit = baseCrit;
                float currentHaste = baseHaste;
                float currentMastery = baseMastery;
                float currentMovementSpeed = baseMovementSpeed;
                float currentTrinketDPS = trinketDPS;

                rot = (FeralRotationList)FeralList[i];
                rotation = new FeralRotation(CombatState, percentBehindBoss, fightLength, rot);
                if (first)
                {
                    calcs.MaxRotation = rotation;
                    first = false;
                }

                // Create the dictionaries to be fed to the special effect processing system:
                // Trigger intervals (how often between potentially triggering events)
                Dictionary<Trigger, float> intervals = new Dictionary<Trigger, float>
                {
                    { Trigger.Use, 0 },
                    { Trigger.MeleeAttack, rotation.HitInterval },
                    { Trigger.MeleeHit, rotation.HitInterval },
                    { Trigger.PhysicalHit, rotation.HitInterval },
                    { Trigger.PhysicalAttack, rotation.HitInterval },
                    { Trigger.MeleeCrit, rotation.CritInterval },
                    { Trigger.PhysicalCrit, rotation.CritInterval },
                    { Trigger.DoTTick, rotation.DotTickInterval },
                    { Trigger.DamageDone, rotation.DotTickInterval },
                    { Trigger.DamageOrHealingDone, rotation.HitInterval },
                    { Trigger.MangleCatHit, rotation.MangleAbilityInterval },
                    { Trigger.MangleCatOrShredHit, rotation.MangleShredAbilityInterval },
                    { Trigger.MangleCatOrShredOrInfectedWoundsHit, rotation.MangleShredAbilityInterval },
                };

                // Trigger chances (how likely the triggering event will occur when the interval elapses)
                Dictionary<Trigger, float> chances = new Dictionary<Trigger, float>
                {
                    { Trigger.Use, 1 },
                    { Trigger.MeleeAttack, rotation.HitChance },
                    { Trigger.MeleeHit, rotation.HitChance },
                    { Trigger.PhysicalHit, rotation.HitChance },
                    { Trigger.PhysicalAttack, rotation.HitChance },
                    { Trigger.MeleeCrit, rotation.CritChance },
                    { Trigger.PhysicalCrit, rotation.CritChance },
                    { Trigger.DoTTick, rotation.HitChance },
                    { Trigger.DamageDone, rotation.HitChance },
                    { Trigger.DamageOrHealingDone, rotation.HitChance },
                    { Trigger.MangleCatHit, rotation.HitChance },
                    { Trigger.MangleCatOrShredHit, rotation.HitChance },
                    { Trigger.MangleCatOrShredOrInfectedWoundsHit, rotation.HitChance },
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
                                    averageStats.Accumulate(inner.GetAverageStats(intervals, chances, 1f, 1f, fightLength, 1f), upTime);
                                }
                            }
                        }
                        else if (effect.Stats.MoteOfAnger > 0)
                        {
                            // When in effect stats, MoteOfAnger is % of melee hits
                            // When in character stats, MoteOfAnger is average procs per second
                            averageStats.MoteOfAnger = effect.Stats.MoteOfAnger * effect.GetAverageProcsPerSecond(intervals[effect.Trigger],
                                chances[effect.Trigger], 1f, 1 + currentHaste, fightLength) / effect.MaxStack;
                        }
                        else
                        {
                            averageStats.Accumulate(effect.GetAverageStats(intervals, chances, 1f, 1 + currentHaste, fightLength, 1f));
                        }
                    } 
                    float procAgi = (float)Math.Floor((averageStats.Agility + averageStats.HighestStat) * (1 + calcs.BasicStats.BonusAgilityMultiplier));
                    float procStr = (float)Math.Floor(averageStats.Strength * (1 + calcs.BasicStats.BonusStrengthMultiplier));
                    float procInt = (float)Math.Floor(averageStats.Intellect * (1 + calcs.BasicStats.BonusIntellectMultiplier));
                    float procAP = (float)Math.Floor((averageStats.AttackPower + (((procAgi > 0) ? ((procAgi - 10) * 2) : 0) + ((procStr > 0) ? (procStr - 10) : 0))) * (1 + calcs.BasicStats.BonusAttackPowerMultiplier));
                    float procCrit = StatConversion.GetPhysicalCritFromRating(averageStats.CritRating) + StatConversion.GetPhysicalCritFromAgility(procAgi, CharacterClass.Druid) + averageStats.PhysicalCrit;
                    float procHaste = StatConversion.GetPhysicalHasteFromRating(averageStats.HasteRating) + averageStats.PhysicalHaste;
                    float procMastery = (StatConversion.GetMasteryFromRating(averageStats.MasteryRating) + averageStats.Mastery) * calcs.MasteryPerRating;
                    float procHealingPower = (float)Math.Floor((procAgi + procInt) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
                    float procMovementSpeed = (float)Math.Floor(averageStats.MovementSpeed);

                    currentAttackPower += procAP;
                    currentHealingPower += procHealingPower;
                    currentCrit += procCrit;
                    currentHaste += procHaste;
                    currentMastery += procMastery;
                    currentMovementSpeed += procMovementSpeed;

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

                // Process damage special effects that scale with spell power here
                foreach(SpecialEffect effect in calcs.BasicStats.SpecialEffects(se => se.AttackPowerScaling > 0))
                {
                    float AttackPowerBoost = currentAttackPower * effect.AttackPowerScaling;
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
                CombatState.MainHand.HealingPower = currentHealingPower;
                CombatState.MainHand.CriticalStrike = currentCrit;
                CombatState.MainHand.Haste = currentHaste;
                CombatState.MainHand.Mastery = currentMastery;
                CombatState.MainHand.MovementSpeed = currentMovementSpeed;

                currentTrinketDPS = (CombatState.MainHand.CritDamageMultiplier * currentTrinketDPS * CombatState.MainHand.CriticalStrike) + (currentTrinketDPS * ((1 - CombatState.MainHand.Hit) - CombatState.MainHand.CriticalStrike));

                rotation.TrinketDPS = currentTrinketDPS;
                rotation.updateCombatState(CombatState);

                if (calcs.MaxRotation.totalDamageDone < rotation.totalDamageDone)
                {
                    calcs.MaxRotation = rotation;
                }
            }

            calcs.SubPoints[0] = calcs.MaxRotation.totalDPS();
            calcs.SubPoints[1] = (calcs.BasicStats.Health * 0.001f) * ( 1 + calcs.MaxRotation.Melee.feralAbility.MainHand.MovementSpeed);
            calcs.OverallPoints = calcs.SubPoints[0] + calcs.SubPoints[1];


        }
    }
}
