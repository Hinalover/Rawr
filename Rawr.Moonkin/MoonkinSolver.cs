using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    // The interface public class to the rest of Rawr.  Provides a single Solve method that runs all the calculations.
    public class MoonkinSolver
    {
        private const int NUM_SPELL_DETAILS = 17;
        public static float BaseMana = 60000f;
        public static float ECLIPSE_MANA_PERCENT = 0.5f;
        public static float ECLIPSE_BASE_PERCENT = 0.3f;

        // A list of all the damage spells
        private Spell[] _spellData = null;
        private Spell[] SpellData
        {
            get
            {
                if (_spellData == null)
                {
                    _spellData = new Spell[] {
                        new Spell()
                        {
                            Name = "SF",
                            BaseDamage = 0f,
                            SpellDamageModifier = 1.532f,
                            BaseCastTime = 3f,
                            BaseManaCost = (float)(int)(BaseMana * 0.155f),
                            DotEffect = null,
                            School = SpellSchool.Arcane,
                            AllDamageModifier = 1f
                        },
                        new Spell()
                        {
                            Name = "MF",
                            BaseDamage = 0f,
                            SpellDamageModifier = 0.240f,
                            BaseCastTime = 1.5f,
                            BaseManaCost = (float)(int)(BaseMana * 0.084f),
                            DotEffect = new DotEffect()
                                {
                                    BaseDuration = 40.0f,
                                    BaseTickLength = 2.0f,
                                    TickDamage = 0f,
                                    SpellDamageModifierPerTick = 0.172f,
                                    AllDamageModifier = 1f
                                },
                            School = SpellSchool.Arcane,
                            AllDamageModifier = 1f
                        },
                        new Spell()
                        {
                            Name = "SuF",
                            BaseDamage = 0f,
                            SpellDamageModifier = 0.240f,
                            BaseCastTime = 1.5f,
                            BaseManaCost = (float)(int)(BaseMana * 0.084f),
                            DotEffect = new DotEffect()
                            {
                                BaseDuration = 20.0f,
                                BaseTickLength = 2.0f,
                                TickDamage = 0f,
                                SpellDamageModifierPerTick = 0.172f,
                                AllDamageModifier = 1f
                            },
                            School = SpellSchool.Nature,
                            AllDamageModifier = 1f
                        },
                        new Spell()
                        {
                            Name = "W",
                            BaseDamage = 0f,
                            SpellDamageModifier = 0.944f,
                            BaseCastTime = 2.0f,
                            BaseManaCost = (float)(int)(BaseMana * 0.088f),
                            DotEffect = null,
                            School = SpellSchool.Nature,
                            AllDamageModifier = 1f
                        },
                        new Spell()
                        {
                            Name = "SS",
                            BaseDamage = 0f,
                            SpellDamageModifier = 1.68f,
                            BaseCastTime = 2.0f,
                            BaseManaCost = (float)(int)(BaseMana * 0.155f),
                            DotEffect = null,
                            School = SpellSchool.Spellstorm,
                            AllDamageModifier = 1f
                        },
                    };
                }
                return _spellData;
            }
        }
        public Spell Starfire
        {
            get
            {
                return SpellData[0];
            }
        }
        public Spell Moonfire
        {
            get
            {
                return SpellData[1];
            }
        }
        public Spell Sunfire
        {
            get
            {
                return SpellData[2];
            }
        }
        public Spell Wrath
        {
            get
            {
                return SpellData[3];
            }
        }
        public Spell Starsurge
        {
            get
            {
                return SpellData[4];
            }
        }
        private void ResetSpellList()
        {
            // Since the property rebuilding the array is based on this variable being null, this effectively forces a refresh
            _spellData = null;
        }

        // The spell rotations themselves.
        private SpellRotation[] rotations = null;
        public SpellRotation[] Rotations
        {
            get
            {
                if (rotations == null)
                {
                    rotations = new SpellRotation[2]
                    {
                        new SpellRotation() { RotationData = new RotationData() { Name = "None" } },
                        new SpellRotation() { RotationData = new RotationData() { Name = "Moonfire Always Refresh", MoonfireRefreshMode = MoonfireRefreshMode.AlwaysRefresh } },
                    };
                }
                return rotations;
            }
        }

        // Results data from the calculations, which will be sent to the UI.
        RotationData[] cachedResults = new RotationData[1];

        public void Solve(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            DruidTalents talents = character.DruidTalents;
            UpdateSpells(character, ref calcs);

            float trinketDPS = 0.0f;
            float baseSpellPower = calcs.SpellPower;
            float baseCrit = calcs.SpellCrit;
            float baseHaste = calcs.SpellHaste;
            float baseMastery = calcs.Mastery;
            // TODO: These are hard-coded conversions, we'll want to handle these later
            float baseMultistrike = Math.Min(1, Math.Max(0, calcs.BasicStats.MultistrikeRating / 100.0f / 100.0f));
            float baseVersatility = Math.Max(0, calcs.BasicStats.VersatilityRating / 500.0f / 100.0f);
            float sub35PercentTime = (float)(character.BossOptions.Under20Perc + character.BossOptions.Under35Perc);

            float maxDamageDone = 0.0f, maxBurstDamageDone = 0.0f;
            SpellRotation maxBurstRotation = Rotations[0];
            SpellRotation maxRotation = Rotations[0];

            float manaPool = GetEffectiveManaPool(character, calcOpts, calcs);

            float manaGained = manaPool - calcs.BasicStats.Mana;

            float oldArcaneMultiplier = calcs.BasicStats.BonusArcaneDamageMultiplier;
            float oldNatureMultiplier = calcs.BasicStats.BonusNatureDamageMultiplier;
            float oldCritDamageMultiplier = calcs.BasicStats.BonusCritDamageMultiplier;

            int rotationIndex = 1;
            foreach (SpellRotation rot in Rotations)
            {
                if (rot.RotationData.Name == "None") continue;
                rot.Solver = this;

                // Reset variables modified in the pre-loop to base values
                float currentSpellPower = baseSpellPower;
                float currentCrit = baseCrit;
                float currentHaste = baseHaste;
                float currentMastery = baseMastery;
                float currentMultistrike = baseMultistrike;
                float currentVersatility = baseVersatility;
                float currentTrinketDPS = trinketDPS;
                calcs.BasicStats.BonusArcaneDamageMultiplier = oldArcaneMultiplier;
                calcs.BasicStats.BonusNatureDamageMultiplier = oldNatureMultiplier;
                calcs.BasicStats.BonusCritDamageMultiplier = oldCritDamageMultiplier;
                float[] spellDetails = new float[NUM_SPELL_DETAILS];

                float baselineDPS = rot.DamageDone(character, calcs, calcOpts.TreantLifespan, currentSpellPower, currentCrit, currentHaste, currentMastery, currentMultistrike, currentVersatility, calcOpts.Latency);

                float damageSpellInterval = rot.RotationData.Duration / (rot.RotationData.CastCount - rot.RotationData.TreantCasts);
                float damageInterval = rot.RotationData.Duration / (rot.RotationData.CastCount - rot.RotationData.TreantCasts + rot.RotationData.DotTicks);

                // Create the dictionaries to be fed to the special effect processing system:
                // Trigger intervals (how often between potentially triggering events)
                Dictionary<Trigger, float> intervals = new Dictionary<Trigger, float>
                {
                    { Trigger.Use, 0 },
                    { Trigger.SpellCast, rot.RotationData.Duration / rot.RotationData.CastCount },
                    { Trigger.DamageSpellCast, damageSpellInterval },
                    { Trigger.SpellHit, damageSpellInterval },
                    { Trigger.DamageSpellHit, damageSpellInterval },
                    { Trigger.DamageDone, damageInterval },
                    { Trigger.DamageOrHealingDone, damageInterval },
                    { Trigger.SpellCrit, damageSpellInterval },
                    { Trigger.DamageSpellCrit, damageSpellInterval },
                    { Trigger.DoTTick, rot.RotationData.Duration / rot.RotationData.DotTicks },
                    { Trigger.EclipseProc, rot.RotationData.Duration / 2 },
                    { Trigger.DamageSpellHitorDoTTick, damageInterval },
                    { Trigger.DamageSpellOrDoTCrit, damageInterval }
                };

                // Trigger chances (how likely the triggering event will occur when the interval elapses)
                Dictionary<Trigger, float> chances = new Dictionary<Trigger,float>
                {
                    { Trigger.Use, 1 },
                    { Trigger.SpellCast, 1 },
                    { Trigger.DamageSpellCast, 1 },
                    { Trigger.SpellHit, 1 },
                    { Trigger.DamageSpellHit, 1 },
                    { Trigger.DamageDone, 1 },
                    { Trigger.DamageOrHealingDone, 1 },
                    { Trigger.SpellCrit, baseCrit },
                    { Trigger.DamageSpellCrit, baseCrit },
                    { Trigger.DoTTick, 1 },
                    { Trigger.EclipseProc, 1 },
                    { Trigger.DamageSpellHitorDoTTick, 1 },
                    { Trigger.DamageSpellOrDoTCrit, baseCrit }
                };

                // Process all special effects and add their stats to the current stats
                foreach (SpecialEffect effect in calcs.BasicStats.SpecialEffects(se => se.SpellPowerScaling == 0))
                {
                    float durationMultiplier = effect.LimitedToExecutePhase ? (float)(character.BossOptions.Under35Perc + character.BossOptions.Under20Perc) : 1;
                    float fightLength = character.BossOptions.BerserkTimer * 60f * durationMultiplier;
                    Stats averageStats = new Stats();
                    
                    if (effect.Stats._rawSpecialEffectDataSize > 0)
                    {
                        // If there is an effect within the effect, Model it
                        for (int j = 0; j < effect.Stats._rawSpecialEffectDataSize; j++)
                        {
                            if (intervals.ContainsKey(effect.Stats._rawSpecialEffectData[j].Trigger))
                            {
                                SpecialEffect inner = effect.Stats._rawSpecialEffectData[j];
                                float upTime = effect.GetAverageUptime(intervals[effect.Trigger], chances[effect.Trigger], 1f, 1f + currentHaste, fightLength);
                                averageStats.Accumulate(inner.GetAverageStats(intervals, chances, 1f, 1f + currentHaste, fightLength, durationMultiplier), upTime);
                            }
                        }
                    }
                    else
                    {
                        averageStats.Accumulate(effect.GetAverageStats(intervals, chances, 1f, 1f + currentHaste, fightLength, durationMultiplier));
                    }
                    
                    float procInt = (float)Math.Floor((averageStats.Intellect + averageStats.HighestStat) * (1 + calcs.BasicStats.BonusIntellectMultiplier));
                    float procSP = (float)Math.Floor((averageStats.SpellPower + procInt) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
                    float procCrit = StatConversion.GetSpellCritFromRating(averageStats.CritRating) + StatConversion.GetSpellCritFromIntellect(procInt) + averageStats.SpellCrit;
                    float procHaste = StatConversion.GetSpellHasteFromRating(averageStats.HasteRating) * (1 + calcs.BasicStats.SecondaryStatMultiplier) + averageStats.SpellHaste;
                    float procMastery = StatConversion.GetMasteryFromRating(averageStats.MasteryRating) * (1 + calcs.BasicStats.SecondaryStatMultiplier) + averageStats.Mastery;

                    currentSpellPower += procSP;
                    currentCrit += procCrit;
                    currentHaste += procHaste;
                    currentMastery += procMastery;

                    // Highest secondary stat handling
                    if (calcs.BasicStats.CritRating > calcs.BasicStats.HasteRating && calcs.BasicStats.CritRating > calcs.BasicStats.MasteryRating)
                    {
                        currentCrit += StatConversion.GetSpellCritFromRating(averageStats.HighestSecondaryStat);
                    }
                    else if (calcs.BasicStats.HasteRating > calcs.BasicStats.CritRating && calcs.BasicStats.HasteRating > calcs.BasicStats.MasteryRating)
                    {
                        currentHaste += StatConversion.GetSpellHasteFromRating(averageStats.HighestSecondaryStat) * (1 + calcs.BasicStats.SecondaryStatMultiplier);
                    }
                    else
                    {
                        currentMastery += StatConversion.GetMasteryFromRating(averageStats.HighestSecondaryStat) * (1 + calcs.BasicStats.SecondaryStatMultiplier);
                    }

                    if (averageStats.MultistrikeProc > 0)
                        calcs.BasicStats.MultistrikeProc = averageStats.MultistrikeProc;

                    // Skull Banner: +20% critical damage (10 sec, 3 min)
                    calcs.BasicStats.BonusCritDamageMultiplier = (1 + calcs.BasicStats.BonusCritDamageMultiplier) * (1 + averageStats.BonusCritDamageMultiplier) - 1;
                    // Nature's Vigil: +20% spell damage (30 sec, 3 min)
                    calcs.BasicStats.BonusDamageMultiplier = (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + averageStats.BonusDamageMultiplier) - 1;
                    currentTrinketDPS += (averageStats.HolyDamage * (1 + calcs.BasicStats.BonusHolyDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        (averageStats.NatureDamage * (1 + calcs.BasicStats.BonusNatureDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        (averageStats.ArcaneDamage * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        (averageStats.ShadowDamage * (1 + calcs.BasicStats.BonusShadowDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        (averageStats.FireDamage * (1 + calcs.BasicStats.BonusFireDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        (averageStats.FrostDamage * (1 + calcs.BasicStats.BonusFrostDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier));
                };
                // Process damage special effects that scale with spell power here
                foreach (SpecialEffect effect in calcs.BasicStats.SpecialEffects(se => se.SpellPowerScaling > 0))
                {
                    float spellPowerBoost = currentSpellPower * effect.SpellPowerScaling;

                    float eHolyDamge = effect.Stats.HolyDamage;
                    float eNatureDamage = effect.Stats.NatureDamage;
                    float eArcaneDamage = effect.Stats.ArcaneDamage;
                    float eShadowDamage = effect.Stats.ShadowDamage;
                    float eFireDamage = effect.Stats.FireDamage;
                    float eFrostDamage = effect.Stats.FrostDamage;

                    float effectDamage = eHolyDamge + eNatureDamage + eArcaneDamage + eShadowDamage + eFireDamage + eFrostDamage;

                    // Add the spell power to the effect before calculating the stat
                    if (eHolyDamge > 0)
                        eHolyDamge += spellPowerBoost;
                    if (eNatureDamage > 0)
                        eNatureDamage += spellPowerBoost;
                    if (eArcaneDamage > 0)
                        eArcaneDamage += spellPowerBoost;
                    if (eShadowDamage > 0)
                        eShadowDamage += spellPowerBoost;
                    if (eFireDamage > 0)
                        eFireDamage += spellPowerBoost;
                    if (eFrostDamage > 0)
                        eFrostDamage += spellPowerBoost;

                    Stats averageStats = effect.GetAverageStats(intervals, chances, 3f, 1f, character.BossOptions.BerserkTimer * 60f, 1f);
                    float averageDamage = averageStats.HolyDamage + averageStats.NatureDamage + averageStats.ArcaneDamage + averageStats.ShadowDamage +
                        averageStats.FireDamage + averageStats.FrostDamage;

                    // Average out the Spell Power modified values against the averageStats values;
                    float aHolyDamage, aNatureDamage, aArcaneDamage, aShadowDamage, aFireDamage, aFrostDamage = 0;
                    aHolyDamage = (eHolyDamge / (effectDamage / averageDamage));
                    aNatureDamage = (eNatureDamage / (effectDamage / averageDamage));
                    aArcaneDamage = (eArcaneDamage / (effectDamage / averageDamage));
                    aShadowDamage = (eShadowDamage / (effectDamage / averageDamage));
                    aFireDamage = (eFireDamage / (effectDamage / averageDamage));
                    aFrostDamage = (eFrostDamage / (effectDamage / averageDamage));

                    currentTrinketDPS += ((averageStats.HolyDamage + aHolyDamage) * (1 + calcs.BasicStats.BonusHolyDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        ((averageStats.NatureDamage + aNatureDamage) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        ((averageStats.ArcaneDamage + aArcaneDamage) * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        ((averageStats.ShadowDamage + aShadowDamage) * (1 + calcs.BasicStats.BonusShadowDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        ((averageStats.FireDamage + aFireDamage) * (1 + calcs.BasicStats.BonusFireDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier)) +
                        ((averageStats.FrostDamage + aFrostDamage) * (1 + calcs.BasicStats.BonusFrostDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier));
                }

                float accumulatedDPS = 0.0f;
                Array.Clear(spellDetails, 0, spellDetails.Length);
                float damageDone = rot.DamageDone(character, calcs, calcOpts.TreantLifespan, currentSpellPower, currentCrit, currentHaste, currentMastery, currentMultistrike, currentVersatility, calcOpts.Latency);
                float caDamageDone = rot.DoCelestialAlignmentCalcs(calcs, talents, currentSpellPower, currentCrit, currentHaste, currentMastery, calcOpts.Latency);
                accumulatedDPS = damageDone / rot.RotationData.Duration;
                spellDetails[0] = rot.RotationData.StarfireAvgHit;
                spellDetails[1] = rot.RotationData.WrathAvgHit;
                spellDetails[2] = rot.RotationData.MoonfireAvgHit;
                spellDetails[3] = rot.RotationData.StarSurgeAvgHit;
                spellDetails[4] = rot.RotationData.SunfireAvgHit;
                spellDetails[5] = rot.RotationData.StarfireAvgCast;
                spellDetails[6] = rot.RotationData.WrathAvgCast;
                spellDetails[7] = rot.RotationData.MoonfireAvgCast;
                spellDetails[8] = rot.RotationData.StarSurgeAvgCast;
                spellDetails[9] = rot.RotationData.SunfireAvgCast;
                spellDetails[10] = rot.RotationData.StarfireAvgEnergy;
                spellDetails[11] = rot.RotationData.WrathAvgEnergy;
                spellDetails[12] = rot.RotationData.StarSurgeAvgEnergy;
                spellDetails[13] = rot.RotationData.MoonfireAvgEnergy;
                spellDetails[14] = rot.RotationData.SunfireAvgEnergy;
                spellDetails[15] = rot.RotationData.TreantDamage;

                float burstDPS = accumulatedDPS * ((180 - 15) / 180f) + caDamageDone / 180f;
                float sustainedDPS = burstDPS;

                // Mana calcs
                rot.RotationData.ManaGained += manaGained / (character.BossOptions.BerserkTimer * 60.0f) * rot.RotationData.Duration;
                float timeToOOM = manaPool / ((rot.RotationData.ManaUsed - rot.RotationData.ManaGained) / rot.RotationData.Duration);
                if (timeToOOM <= 0) timeToOOM = character.BossOptions.BerserkTimer * 60.0f;   // Happens when ManaUsed is less than 0
                if (timeToOOM < character.BossOptions.BerserkTimer * 60.0f)
                {
                    rot.RotationData.TimeToOOM = new TimeSpan(0, (int)(timeToOOM / 60), (int)(timeToOOM % 60));
                    sustainedDPS = burstDPS * timeToOOM / (character.BossOptions.BerserkTimer * 60.0f);
                }
                
                burstDPS += currentTrinketDPS;
                sustainedDPS += currentTrinketDPS;

                rot.RotationData.SustainedDPS = sustainedDPS;
                rot.RotationData.BurstDPS = burstDPS;
                rot.RotationData.StarfireAvgHit = spellDetails[0];
                rot.RotationData.WrathAvgHit = spellDetails[1];
                rot.RotationData.MoonfireAvgHit = spellDetails[2];
                rot.RotationData.StarSurgeAvgHit = spellDetails[3];
                rot.RotationData.SunfireAvgHit = spellDetails[4];
                rot.RotationData.StarfireAvgCast = spellDetails[5];
                rot.RotationData.WrathAvgCast = spellDetails[6];
                rot.RotationData.MoonfireAvgCast = spellDetails[7];
                rot.RotationData.StarSurgeAvgCast = spellDetails[8];
                rot.RotationData.SunfireAvgCast = spellDetails[9];
                rot.RotationData.StarfireAvgEnergy = spellDetails[10];
                rot.RotationData.WrathAvgEnergy = spellDetails[11];
                rot.RotationData.StarSurgeAvgEnergy = spellDetails[12];
                rot.RotationData.MoonfireAvgEnergy = spellDetails[13];
                rot.RotationData.SunfireAvgEnergy = spellDetails[14];
                rot.RotationData.TreantDamage = spellDetails[15];

                // Update the sustained DPS rotation if any one of the following three cases is true:
                // 1) No user rotation is selected and sustained DPS is maximum
                // 2) A user rotation is selected, Eclipse is not present, and the user rotation matches the current rotation
                // 3) A user rotation is selected, Eclipse is present, and the user rotation's dot spells matches this rotation's
                if ((calcOpts.UserRotation == "None" && sustainedDPS > maxDamageDone) || rot.RotationData.Name == calcOpts.UserRotation)
                {
                    maxDamageDone = sustainedDPS;
                    maxRotation = rot;
                }
                if (burstDPS > maxBurstDamageDone)
                {
                    maxBurstDamageDone = burstDPS;
                    maxBurstRotation = rot;
                }
                cachedResults[rotationIndex - 1] = rot.RotationData;

                ++rotationIndex;
            }
            // Present the findings to the user.
            calcs.SelectedRotation = maxRotation.RotationData;
            calcs.BurstRotation = maxBurstRotation.RotationData;
            calcs.SubPoints = new float[] { maxBurstDamageDone, maxDamageDone };
            calcs.OverallPoints = calcs.SubPoints[0] + calcs.SubPoints[1];
            calcs.Rotations = cachedResults;
        }

        // Non-rotation-specific mana calculations
        private float GetEffectiveManaPool(Character character, CalculationOptionsMoonkin calcOpts, CharacterCalculationsMoonkin calcs)
        {
            float fightLength = character.BossOptions.BerserkTimer * 60.0f;

            float innervateCooldown = 180;

            // Mana/5 calculations
            float totalManaRegen = calcs.ManaRegen * fightLength;

            // Mana pot calculations
            float manaRestoredByPots = 0.0f;
            foreach (Buff b in character.ActiveBuffs)
            {
                if (b.Stats.ManaRestore > 0)
                {
                    manaRestoredByPots = b.Stats.ManaRestore;
                    break;
                }
            }

            // Innervate calculations
            float innervateDelay = calcOpts.InnervateDelay * 60.0f;
            int numInnervates = (calcOpts.Innervate && fightLength - innervateDelay > 0) ? ((int)(fightLength - innervateDelay) / (int)innervateCooldown + 1) : 0;
            float totalInnervateMana = numInnervates * 0.2f * calcs.BasicStats.Mana;

            // Replenishment calculations
            float replenishmentPerTick = calcs.BasicStats.Mana * calcs.BasicStats.ManaRestoreFromMaxManaPerSecond;
            float replenishmentMana = calcOpts.ReplenishmentUptime * replenishmentPerTick * character.BossOptions.BerserkTimer * 60;

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen + manaRestoredByPots + replenishmentMana;
        }

        // Add talented effects to the spells
        private void UpdateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            DruidTalents talents = character.DruidTalents;
            StatsMoonkin stats = calcs.BasicStats;

            // Add spell-specific critical strike damage
            // Burning Shadowspirit Diamond
            float baseCritMultiplier = 2f * (1 + stats.BonusCritDamageMultiplier);
            Starfire.CriticalDamageModifier = Wrath.CriticalDamageModifier = Moonfire.CriticalDamageModifier = Starsurge.CriticalDamageModifier = baseCritMultiplier;

            // Reduce spell-specific mana costs
            // Shard of Woe (Mana cost -405)
            Starfire.BaseManaCost -= calcs.BasicStats.SpellsManaCostReduction;
            Moonfire.BaseManaCost -= calcs.BasicStats.SpellsManaCostReduction;
            Wrath.BaseManaCost -= calcs.BasicStats.SpellsManaCostReduction + calcs.BasicStats.NatureSpellsManaCostReduction;
            Starsurge.BaseManaCost -= calcs.BasicStats.SpellsManaCostReduction;

            // Add set bonuses
            Moonfire.CriticalChanceModifier += stats.BonusCritChanceMoonfire;
            Starfire.AllDamageModifier *= 1 + stats.BonusNukeDamageModifier;
            Wrath.AllDamageModifier *= 1 + stats.BonusNukeDamageModifier;
            Starsurge.AllDamageModifier *= (1 + stats.BonusNukeDamageModifier) * (1 + stats.BonusStarsurgeDamageModifier);

            // PTR changes go here
            if (calcs.PTRMode)
            {
            }
        }
    }
}
