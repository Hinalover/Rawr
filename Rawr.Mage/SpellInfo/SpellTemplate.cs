using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public abstract class SpellTemplate
    {
        public string Name;

        public bool Channeled;
        public bool Instant;
        public float BaseCastTime;
        public float BaseUntalentedCastTime;
        public float BaseCooldown;
        public float Cooldown;
        public float GlobalCooldown;

        public bool AreaEffect;
        public bool AreaEffectDot;
        public int MaximumAOETargets;
        public float AOEMultiplier;
        public int BaseCost;
        public MagicSchool MagicSchool;
        public float Ticks;
        public float CastProcs;
        public float CastProcs2;
        public float BaseAverageDamage;
        public float BaseDelta;
        public float SpellDamageCoefficient;
        public float BasePeriodicDamage;
        public float DotDamageCoefficient;
        public float DotDuration;

        public float BaseDirectDamageModifier;
        public float BaseDotDamageModifier;
        public float BaseCostModifier;
        public float BaseCostAmplifier;
        public float BaseInterruptProtection;

        public float BaseSpellModifier;
        public float BaseAdditiveSpellModifier;
        public float BaseCritRate;
        public float NonHSCritRate; // crit rate that doesn't apply to hot streak
        public float CritRateMultiplier; // used for critical mass
        public float CritBonus;
        public float HitRate;
        public float ThreatMultiplier;
        public float Range;

        public float RealResistance;
        public float PartialResistFactor;

        public bool CausesIgnite;

        // not initialized, but never changed
        public float DotTickInterval;
        public float NukeProcs;
        public float NukeProcs2;
        public bool UseMaxT13;

        public bool Dirty = true;

        public virtual Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public float GetEffectAverageDamage(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateAverageDamage(castingState.Solver, 0, false, false);
            return spell.AverageDamage;
        }

        protected SpellTemplate() { }

        public void InitializeCastTime(bool channeled, bool instant, float castTime, float cooldown)
        {
            Channeled = channeled;
            Instant = instant;
            BaseCastTime = castTime;
            BaseUntalentedCastTime = instant ? 1.5f : castTime;
            BaseCooldown = cooldown;
            Cooldown = cooldown;
            GlobalCooldown = 1.5f;
            CausesIgnite = false;
        }

        public void InitializeDamage(Solver solver, bool areaEffect, int range, MagicSchool magicSchool, SpellData spellData) 
        {
            InitializeDamage(solver, areaEffect, range, magicSchool, spellData, 1, 1, 0);
        }

        public void InitializeScaledDamage(Solver solver, bool areaEffect, int range, MagicSchool magicSchool, float baseCost, float baseAverage, float delta, float basePeriodic, float spellDamageCoefficient, float dotDamageCoefficient, float hitProcs, float castProcs, float dotDuration)
        {
            var options = solver.CalculationOptions;
			InitializeDamage(solver, areaEffect, range, magicSchool, (int)(baseCost * options.BaseMana), baseAverage, delta, spellDamageCoefficient, options.GetSpellValue(basePeriodic), dotDamageCoefficient, hitProcs, castProcs, dotDuration);
        }

        public void InitializeDamage(Solver solver, bool areaEffect, int range, MagicSchool magicSchool, SpellData spellData, float hitProcs, float castProcs, float dotDuration)
        {
            InitializeDamage(solver, areaEffect, range, magicSchool, spellData.Cost, spellData.AverageDamage, spellData.Delta, spellData.SpellDamageCoefficient, spellData.PeriodicDamage, spellData.DotDamageCoefficient, hitProcs, castProcs, dotDuration);
        }

        public void InitializeDamage(Solver solver, bool areaEffect, int range, MagicSchool magicSchool, int cost, float averageDamage, float delta, float spellDamageCoefficient, float periodicDamage, float dotDamageCoefficient, float hitProcs, float castProcs, float dotDuration)
        {
            Stats baseStats = solver.BaseStats;
            MageTalents mageTalents = solver.MageTalents;
            CalculationOptionsMage calculationOptions = solver.CalculationOptions;

            AreaEffect = areaEffect;
            AreaEffectDot = areaEffect;
            MaximumAOETargets = 10;
            AOEMultiplier = 1;
            int manaReduction = (int)baseStats.SpellsManaCostReduction;
            BaseCost = Math.Max(cost - manaReduction, 0);
            MagicSchool = magicSchool;
            Ticks = hitProcs;
            CastProcs = castProcs;
            CastProcs2 = castProcs;
            BaseAverageDamage = averageDamage;
            BaseDelta = delta;
            SpellDamageCoefficient = spellDamageCoefficient;
            BasePeriodicDamage = periodicDamage;
            DotDamageCoefficient = dotDamageCoefficient;
            DotDuration = dotDuration;

            BaseDirectDamageModifier = 1.0f;
            BaseDotDamageModifier = 1.0f;
            BaseCostModifier = 1.0f;
            CritRateMultiplier = 1.0f;

            float baseCostAmplifier = calculationOptions.EffectCostMultiplier;
            BaseCostAmplifier = baseCostAmplifier;

            float baseInterruptProtection = baseStats.InterruptProtection;
            if (calculationOptions.PlayerLevel >= 82)
            {
                baseInterruptProtection += 0.7f;
            }
            BaseInterruptProtection = baseInterruptProtection;

            float realResistance;
            switch (MagicSchool)
            {
                case MagicSchool.Arcane:
                    BaseSpellModifier = solver.BaseArcaneSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseArcaneAdditiveSpellModifier;
                    BaseCritRate = solver.BaseArcaneCritRate;
                    CritBonus = solver.BaseArcaneCritBonus;
                    HitRate = solver.BaseArcaneHitRate;
                    ThreatMultiplier = solver.ArcaneThreatMultiplier;
                    realResistance = calculationOptions.ArcaneResist;
                    break;
                case MagicSchool.Fire:
                    BaseSpellModifier = solver.BaseFireSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseFireAdditiveSpellModifier;
                    BaseCritRate = solver.BaseFireCritRate;
                    CritBonus = solver.BaseFireCritBonus;
                    HitRate = solver.BaseFireHitRate;
                    ThreatMultiplier = solver.FireThreatMultiplier;
                    realResistance = calculationOptions.FireResist;
                    break;
                case MagicSchool.FrostFire:
                    BaseSpellModifier = solver.BaseFrostFireSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseFrostFireAdditiveSpellModifier;
                    BaseCritRate = solver.BaseFrostFireCritRate;
                    CritBonus = solver.BaseFrostFireCritBonus;
                    HitRate = solver.BaseFrostFireHitRate;
                    ThreatMultiplier = solver.FrostFireThreatMultiplier;
                    if (calculationOptions.FireResist == -1)
                    {
                        realResistance = calculationOptions.FrostResist;
                    }
                    else if (calculationOptions.FrostResist == -1)
                    {
                        realResistance = calculationOptions.FireResist;
                    }
                    else
                    {
                        realResistance = Math.Min(calculationOptions.FireResist, calculationOptions.FrostResist);
                    }
                    Range = range;
                    break;
                case MagicSchool.Frost:
                    BaseSpellModifier = solver.BaseFrostSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseFrostAdditiveSpellModifier;
                    BaseCritRate = solver.BaseFrostCritRate;
                    CritBonus = solver.BaseFrostCritBonus;
                    HitRate = solver.BaseFrostHitRate;
                    ThreatMultiplier = solver.FrostThreatMultiplier;
                    realResistance = calculationOptions.FrostResist;
                    break;
                case MagicSchool.Nature:
                    BaseSpellModifier = solver.BaseNatureSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseNatureAdditiveSpellModifier;
                    BaseCritRate = solver.BaseNatureCritRate;
                    CritBonus = solver.BaseNatureCritBonus;
                    HitRate = solver.BaseNatureHitRate;
                    ThreatMultiplier = solver.NatureThreatMultiplier;
                    realResistance = calculationOptions.NatureResist;
                    Range = range;
                    break;
                case MagicSchool.Shadow:
                    BaseSpellModifier = solver.BaseShadowSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseShadowAdditiveSpellModifier;
                    BaseCritRate = solver.BaseShadowCritRate;
                    CritBonus = solver.BaseShadowCritBonus;
                    HitRate = solver.BaseShadowHitRate;
                    ThreatMultiplier = solver.ShadowThreatMultiplier;
                    realResistance = calculationOptions.ShadowResist;
                    Range = range;
                    break;
                case MagicSchool.Holy:
                default:
                    BaseSpellModifier = solver.BaseHolySpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseHolyAdditiveSpellModifier;
                    BaseCritRate = solver.BaseHolyCritRate;
                    CritBonus = solver.BaseHolyCritBonus;
                    HitRate = solver.BaseHolyHitRate;
                    ThreatMultiplier = solver.HolyThreatMultiplier;
                    realResistance = calculationOptions.HolyResist;
                    Range = range;
                    break;
            }

            NonHSCritRate = baseStats.SpellCritOnTarget;

            int playerLevel = calculationOptions.PlayerLevel;
            int targetLevel;

            if (areaEffect)
            {
                targetLevel = calculationOptions.AoeTargetLevel;
                float hitRate = ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + solver.BaseSpellHit;
                if (hitRate > Spell.MaxHitRate) hitRate = Spell.MaxHitRate;
                HitRate = hitRate;
            }
            else
            {
                targetLevel = calculationOptions.TargetLevel;
            }

            RealResistance = realResistance;
            PartialResistFactor = (realResistance == -1) ? 0 : (1 - StatConversion.GetAverageResistance(playerLevel, targetLevel, realResistance, baseStats.SpellPenetration));
        }

        public void InitializeEffectDamage(Solver solver, MagicSchool magicSchool, float averageDamage, float delta)
        {
            Stats baseStats = solver.BaseStats;
            MageTalents mageTalents = solver.MageTalents;
            CalculationOptionsMage calculationOptions = solver.CalculationOptions;

            //AreaEffect = areaEffect;
            //BaseCost = cost - (int)baseStats.SpellsManaReduction;
            MagicSchool = magicSchool;
            BaseAverageDamage = averageDamage;
            BaseDelta = delta;
            //BasePeriodicDamage = periodicDamage;
            //SpellDamageCoefficient = spellDamageCoefficient;
            //Ticks = 1;
            //CastProcs = 0;
            //CastProcs2 = 0;
            //DotDamageCoefficient = dotDamageCoefficient;
            //DotDuration = dotDuration;

            BaseDirectDamageModifier = 1.0f;
            BaseDotDamageModifier = 1.0f;
            BaseCostModifier = 1.0f;

            //Range = range;
            
            /*float baseCostAmplifier = calculationOptions.EffectCostMultiplier;
            baseCostAmplifier *= (1.0f - 0.01f * mageTalents.Precision);
            if (mageTalents.FrostChanneling > 0) baseCostAmplifier *= (1.0f - 0.01f - 0.03f * mageTalents.FrostChanneling);
            if (MagicSchool == MagicSchool.Arcane) baseCostAmplifier *= (1.0f - 0.01f * mageTalents.ArcaneFocus);
            BaseCostAmplifier = baseCostAmplifier;*/

            /*float baseInterruptProtection = baseStats.InterruptProtection;
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire)
            {
                baseInterruptProtection += 0.35f * mageTalents.BurningSoul;
                AffectedByFlameCap = true;
            }
            BaseInterruptProtection = baseInterruptProtection;*/

            float realResistance;
            switch (MagicSchool)
            {
                case MagicSchool.Arcane:
                    BaseSpellModifier = solver.BaseArcaneSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseArcaneAdditiveSpellModifier;
                    BaseCritRate = solver.BaseArcaneCritRate;
                    CritBonus = solver.BaseArcaneCritBonus;
                    HitRate = solver.BaseArcaneHitRate;
                    ThreatMultiplier = solver.ArcaneThreatMultiplier;
                    realResistance = calculationOptions.ArcaneResist;
                    break;
                case MagicSchool.Fire:
                    BaseSpellModifier = solver.BaseFireSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseFireAdditiveSpellModifier;
                    BaseCritRate = solver.BaseFireCritRate;
                    CritBonus = solver.BaseFireCritBonus;
                    HitRate = solver.BaseFireHitRate;
                    ThreatMultiplier = solver.FireThreatMultiplier;
                    realResistance = calculationOptions.FireResist;
                    break;
                case MagicSchool.FrostFire:
                    BaseSpellModifier = solver.BaseFrostFireSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseFrostFireAdditiveSpellModifier;
                    BaseCritRate = solver.BaseFrostFireCritRate;
                    CritBonus = solver.BaseFrostFireCritBonus;
                    HitRate = solver.BaseFrostFireHitRate;
                    ThreatMultiplier = solver.FrostFireThreatMultiplier;
                    if (calculationOptions.FireResist == -1)
                    {
                        realResistance = calculationOptions.FrostResist;
                    }
                    else if (calculationOptions.FrostResist == -1)
                    {
                        realResistance = calculationOptions.FireResist;
                    }
                    else
                    {
                        realResistance = Math.Min(calculationOptions.FireResist, calculationOptions.FrostResist);
                    }
                    break;
                case MagicSchool.Frost:
                    BaseSpellModifier = solver.BaseFrostSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseFrostAdditiveSpellModifier;
                    BaseCritRate = solver.BaseFrostCritRate;
                    CritBonus = solver.BaseFrostCritBonus;
                    HitRate = solver.BaseFrostHitRate;
                    ThreatMultiplier = solver.FrostThreatMultiplier;
                    realResistance = calculationOptions.FrostResist;
                    break;
                case MagicSchool.Nature:
                    BaseSpellModifier = solver.BaseNatureSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseNatureAdditiveSpellModifier;
                    BaseCritRate = solver.BaseNatureCritRate;
                    CritBonus = solver.BaseNatureCritBonus;
                    HitRate = solver.BaseNatureHitRate;
                    ThreatMultiplier = solver.NatureThreatMultiplier;
                    realResistance = calculationOptions.NatureResist;
                    break;
                case MagicSchool.Shadow:
                    BaseSpellModifier = solver.BaseShadowSpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseShadowAdditiveSpellModifier;
                    BaseCritRate = solver.BaseShadowCritRate;
                    CritBonus = solver.BaseShadowCritBonus;
                    HitRate = solver.BaseShadowHitRate;
                    ThreatMultiplier = solver.ShadowThreatMultiplier;
                    realResistance = calculationOptions.ShadowResist;
                    break;
                case MagicSchool.Holy:
                default:
                    BaseSpellModifier = solver.BaseHolySpellModifier;
                    BaseAdditiveSpellModifier = solver.BaseHolyAdditiveSpellModifier;
                    BaseCritRate = solver.BaseHolyCritRate;
                    CritBonus = solver.BaseHolyCritBonus;
                    HitRate = solver.BaseHolyHitRate;
                    ThreatMultiplier = solver.HolyThreatMultiplier;
                    realResistance = calculationOptions.HolyResist;
                    break;
            }

            NonHSCritRate = baseStats.SpellCritOnTarget;

            int playerLevel = calculationOptions.PlayerLevel;
            int targetLevel = calculationOptions.TargetLevel;

            /*if (areaEffect)
            {
                targetLevel = calculationOptions.AoeTargetLevel;
                float hitRate = ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculations.BaseSpellHit;
                if (MagicSchool == MagicSchool.Arcane) hitRate += 0.01f * mageTalents.ArcaneFocus;
                if (hitRate > Spell.MaxHitRate) hitRate = Spell.MaxHitRate;
                HitRate = hitRate;
            }
            else
            {
                targetLevel = calculationOptions.TargetLevel;
            }*/

            RealResistance = realResistance;
            PartialResistFactor = (realResistance == -1) ? 0 : (1 - StatConversion.GetAverageResistance(playerLevel, targetLevel, realResistance, baseStats.SpellPenetration));
        }

        public static float ProcBuffUp(float procChance, float buffDuration, float triggerInterval)
        {
            if (triggerInterval <= 0)
                return 0;
            else
                return 1 - (float)Math.Pow(1 - procChance, buffDuration / triggerInterval);
        }

        public float CalculateCastTime(CastingState castingState, float interruptProtection, float critRate, bool pom, float baseCastTime, out float channelReduction, out float castingSpeed)
        {
            CalculationOptionsMage calculationOptions = castingState.CalculationOptions;
            castingSpeed = castingState.CastingSpeed;
            float spellHasteRating = castingState.SpellHasteRating;
            float hasteFactor = 0.01f / calculationOptions.HasteRatingMultiplier;
            float rootCastingSpeed = castingSpeed / (1 + spellHasteRating * hasteFactor);

            float InterruptFactor = 0f;
            float maxPushback = 0f;
            if (calculationOptions.InterruptFrequency > 0f)
            {
                // interrupt factors of more than once per spell are not supported, so put a limit on it (up to twice is probably approximately correct)
                InterruptFactor = Math.Min(calculationOptions.InterruptFrequency, 2 * castingSpeed / baseCastTime);
                if (castingState.IcyVeins) interruptProtection = 1;
                maxPushback = 0.5f * Math.Max(0, 1 - interruptProtection);
                if (Channeled) maxPushback = 0.0f;
            }

            if (pom) baseCastTime = 0.0f;

            float globalCooldown = Math.Max(Spell.GlobalCooldownLimit, GlobalCooldown / castingSpeed);
            float latency;
            if (baseCastTime <= GlobalCooldown || Instant)
            {
                latency = calculationOptions.LatencyGCD;
            }
            else if (Channeled)
            {
                latency = calculationOptions.LatencyChannel;
            }
            else
            {
                latency = calculationOptions.LatencyCast;
            }
            float averageTicks = Ticks;
            float castTime = baseCastTime / castingSpeed;
            /*if (calculationOptions.Beta && Channeled)
            {
                float tickCastTime = castTime / Ticks;
                averageTicks = (float)Math.Floor(baseCastTime / tickCastTime);
                castTime = averageTicks * tickCastTime;
            }*/
            if (InterruptFactor > 0)
            {
                castTime = castTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f) * maxPushback * InterruptFactor;
            }
            castTime += latency;
            float gcdcap = globalCooldown + calculationOptions.LatencyGCD;
            if (castTime < gcdcap) castTime = gcdcap;
            channelReduction = 0.0f;

            if (calculationOptions.AdvancedHasteProcs && castingState.ForceT13 && UseMaxT13)
            {
                spellHasteRating += 500;
                castingSpeed = rootCastingSpeed * (1 + spellHasteRating * hasteFactor);

                globalCooldown = Math.Max(Spell.GlobalCooldownLimit, GlobalCooldown / castingSpeed);
                castTime = baseCastTime / castingSpeed;
                if (InterruptFactor > 0)
                {
                    castTime = castTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f) * maxPushback * InterruptFactor;
                }
                castTime += latency;
                gcdcap = globalCooldown + calculationOptions.LatencyGCD;
                if (castTime < gcdcap) castTime = gcdcap;
            }
            if (!calculationOptions.AdvancedHasteProcs)
            {
                if (castingState.Solver.Mage2T13)
                {
                    // very rough and very ugly
                    SpecialEffect effect = Solver.SpecialEffect2T13;
                    float chance = (castingState.Solver.Specialization == Specialization.Arcane) ? 1.0f : 0.5f;
                    float triggers = 0f;
                    int resets = 1;
                    switch (castingState.Solver.Specialization) // guesstimates
                    {
                        case Specialization.Arcane:
                            triggers = 0.47f;
                            resets = 1 + (int)(castingState.CalculationOptions.FightDuration / castingState.Solver.ArcanePowerCooldown);
                            break;
                        case Specialization.Fire:
                            triggers = 0.45f;
                            resets = 1 + (int)(castingState.CalculationOptions.FightDuration / castingState.Solver.CombustionCooldown);
                            break;
                        case Specialization.Frost:
                            triggers = 0.52f;
                            resets = 1 + (int)(castingState.CalculationOptions.FightDuration / castingState.Solver.IcyVeinsCooldown);
                            break;
                    }

                    if (UseMaxT13)
                    {
                        spellHasteRating += 500;
                    }
                    else
                    {
                        spellHasteRating += 50 * effect.GetAverageStackSize(1 / triggers, chance, 3.0f, calculationOptions.FightDuration, resets);
                    }
                    castingSpeed = rootCastingSpeed * (1 + spellHasteRating * hasteFactor);

                    globalCooldown = Math.Max(Spell.GlobalCooldownLimit, GlobalCooldown / castingSpeed);
                    castTime = baseCastTime / castingSpeed;
                    if (InterruptFactor > 0)
                    {
                        castTime = castTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f) * maxPushback * InterruptFactor;
                    }
                    castTime += latency;
                    gcdcap = globalCooldown + calculationOptions.LatencyGCD;
                    if (castTime < gcdcap) castTime = gcdcap;
                }
                for (int i = 0; i < castingState.Solver.HasteRatingEffectsCount; i++)
                {
                    SpecialEffect effect = castingState.Solver.HasteRatingEffects[i];
                    float procs = 0.0f;
                    int triggers = 0;
                    switch (effect.Trigger)
                    {
                        case Trigger.DamageSpellCast:
                        case Trigger.SpellCast:
                            procs = CastProcs;
                            triggers = (int)procs;
                            break;
                        case Trigger.DamageSpellCrit:
                        case Trigger.DamageSpellOrDoTCrit:
                        case Trigger.SpellCrit:
                            procs = critRate * averageTicks;
                            triggers = (int)averageTicks;
                            break;
                        case Trigger.DamageSpellHit:
                        case Trigger.SpellHit:
                        case Trigger.DamageSpellHitorDoTTick: // FIXME: add dots in
                            procs = HitRate * averageTicks;
                            triggers = (int)averageTicks;
                            break;
                    }
                    if (procs == 0.0f) continue;
                    float procHaste = effect.Stats.HasteRating;
                    if (effect.Cooldown >= effect.Duration)
                    {
                        // hasted casttime
                        float speed = rootCastingSpeed * (1 + (spellHasteRating + procHaste) * hasteFactor);
                        float gcd = Math.Max(Spell.GlobalCooldownLimit, GlobalCooldown / speed);
                        float cast = baseCastTime / speed;
                        /*if (calculationOptions.Beta && Channeled)
                        {
                            float tickCastTime = cast / Ticks;
                            cast = (float)Math.Floor(baseCastTime / tickCastTime) * tickCastTime;
                        }*/
                        if (InterruptFactor > 0)
                        {
                            cast = cast * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f) * maxPushback * InterruptFactor;
                        }
                        cast += latency;
                        gcdcap = gcd + calculationOptions.LatencyGCD;
                        if (cast < gcdcap) cast = gcdcap;

                        float castsAffected = 0;
                        if (triggers > 1)
                        {
                            // multi tick spell (i.e. AM)
                            for (int c = 0; c < triggers; c++) castsAffected += (float)Math.Ceiling((effect.Duration - c * castTime / triggers) / cast);
                            castsAffected /= triggers;
                        }
                        else
                        {
                            // single tick spell
                            castsAffected = (float)Math.Ceiling(effect.Duration / cast); // should the first proc be already hasted?
                        }
                        float effectiveDuration = castsAffected * cast;
                        // this isn't completely accurate, we should have made a separate SpecialEffect and change the actual duration
                        // but performance would hurt so this'll have to do
                        spellHasteRating += procHaste * (effectiveDuration / effect.Duration) * effect.GetAverageUptime(castTime / triggers, procs / triggers, 3.0f, castingSpeed, calculationOptions.FightDuration);
                        //spellHasteRating += procHaste * castsAffected * cast / (effect.Cooldown + castTime / procs / effect.Chance);
                        //Haste += castingState.BasicStats.SpellHasteFor6SecOnCast_15_45 * 6f / (45f + CastTime / CastProcs / 0.15f);
                        castingSpeed = rootCastingSpeed * (1 + spellHasteRating * hasteFactor);

                        globalCooldown = Math.Max(Spell.GlobalCooldownLimit, GlobalCooldown / castingSpeed);
                        castTime = baseCastTime / castingSpeed;
                        /*if (calculationOptions.Beta && Channeled)
                        {
                            float tickCastTime = castTime / Ticks;
                            averageTicks = (float)Math.Floor(baseCastTime / tickCastTime);
                            castTime = averageTicks * tickCastTime;
                        }*/
                        if (InterruptFactor > 0)
                        {
                            castTime = castTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f) * maxPushback * InterruptFactor;
                        }
                        castTime += latency;
                        gcdcap = globalCooldown + calculationOptions.LatencyGCD;
                        if (castTime < gcdcap) castTime = gcdcap;
                    }
                    else if (effect.Cooldown <= 1 && (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.DamageSpellOrDoTCrit || effect.Trigger == Trigger.SpellHit || effect.Trigger == Trigger.DamageSpellHit))
                    {
                        float rate = 1;
                        if (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.DamageSpellOrDoTCrit)
                        {
                            rate = critRate;
                        }

                        float rawHaste = spellHasteRating;

                        castingSpeed /= (1 + spellHasteRating * hasteFactor);
                        float proccedSpeed = castingSpeed * (1 + (rawHaste + procHaste) * hasteFactor);
                        float proccedGcd = Math.Max(Spell.GlobalCooldownLimit, GlobalCooldown / proccedSpeed);
                        float proccedCastTime = baseCastTime / proccedSpeed;
                        float proccedTicks = averageTicks;
                        /*if (calculationOptions.Beta && Channeled)
                        {
                            float tickCastTime = proccedCastTime / Ticks;
                            proccedTicks = (float)Math.Floor(baseCastTime / tickCastTime);
                            castTime = proccedTicks * tickCastTime;
                        }*/
                        if (InterruptFactor > 0)
                        {
                            proccedCastTime = proccedCastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f) * maxPushback * InterruptFactor;
                        }
                        proccedCastTime += latency;
                        if (proccedCastTime < proccedGcd + calculationOptions.LatencyGCD) proccedCastTime = proccedGcd + calculationOptions.LatencyGCD;
                        int chancesToProc = (int)(((int)Math.Floor(effect.Duration / proccedCastTime) + 1) * proccedTicks);
                        if (!(Instant || pom)) chancesToProc -= 1;
                        if (AreaEffect) chancesToProc *= calculationOptions.AoeTargets;
                        float chance = effect.GetChance(baseCastTime / proccedSpeed, proccedCastTime, castingSpeed);
                        spellHasteRating = rawHaste + procHaste * (1 - (float)Math.Pow(1 - chance * rate, chancesToProc));
                        //Haste = rawHaste + castingState.BasicStats.SpellHasteFor5SecOnCrit_50 * ProcBuffUp(1 - (float)Math.Pow(1 - 0.5f * CritRate, HitProcs), 5, CastTime);
                        castingSpeed *= (1 + spellHasteRating * hasteFactor);
                        globalCooldown = Math.Max(Spell.GlobalCooldownLimit, GlobalCooldown / castingSpeed);
                        castTime = baseCastTime / castingSpeed;
                        /*if (calculationOptions.Beta && Channeled)
                        {
                            float tickCastTime = castTime / Ticks;
                            averageTicks = (float)Math.Floor(baseCastTime / tickCastTime);
                            castTime = averageTicks * tickCastTime;
                        }*/
                        if (InterruptFactor > 0)
                        {
                            castTime = castTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + latency) * maxPushback * InterruptFactor;
                        }
                        castTime += latency;
                        if (castTime < globalCooldown + calculationOptions.LatencyGCD) castTime = globalCooldown + calculationOptions.LatencyGCD;
                    }
                }
                // on use stacking items
                for (int i = 0; i < castingState.Solver.StackingHasteEffectCooldownsCount; i++)
                {
                    EffectCooldown effectCooldown = castingState.Solver.StackingHasteEffectCooldowns[i];
                    if (castingState.EffectsActive(effectCooldown.Mask))
                    {
                        Stats stats = effectCooldown.SpecialEffect.Stats;
                        for (int j = 0; j < stats._rawSpecialEffectDataSize; j++)
                        {
                            SpecialEffect effect = stats._rawSpecialEffectData[j];
                            float procHaste = effect.Stats.HasteRating;
                            if (procHaste > 0)
                            {                                
                                float procs = 0.0f;
                                switch (effect.Trigger)
                                {
                                    case Trigger.DamageSpellCast:
                                    case Trigger.SpellCast:
                                        procs = CastProcs;
                                        break;
                                    case Trigger.DamageSpellCrit:
                                    case Trigger.DamageSpellOrDoTCrit:
                                    case Trigger.SpellCrit:
                                        procs = critRate * averageTicks;
                                        break;
                                    case Trigger.DamageSpellHit:
                                    case Trigger.SpellHit:
                                        procs = HitRate * averageTicks;
                                        break;
                                    case Trigger.MageNukeCast:
                                        procs = NukeProcs;
                                        break;
                                    case Trigger.MageNukeCast2:
                                        procs = NukeProcs2;
                                        break;
                                }
                                if (procs == 0.0f) continue;
                                // until they put in some good trinkets with such effects just do a quick dirty calculation
                                float effectHasteRating;
                                if (procs > averageTicks)
                                {
                                    // some 100% on cast procs, happens because AM has 6 cast procs and only 5 ticks
                                    effectHasteRating = effect.GetAverageStackSize(castTime / procs, 1.0f, 3.0f, castingSpeed, effectCooldown.SpecialEffect.Duration) * procHaste;
                                }
                                else
                                {
                                    effectHasteRating = effect.GetAverageStackSize(castTime / averageTicks, procs / averageTicks, 3.0f, castingSpeed, effectCooldown.SpecialEffect.Duration) * procHaste;
                                }

                                castingSpeed /= (1 + spellHasteRating * hasteFactor);
                                spellHasteRating += effectHasteRating;
                                castingSpeed *= (1 + spellHasteRating * hasteFactor);

                                globalCooldown = Math.Max(Spell.GlobalCooldownLimit, GlobalCooldown / castingSpeed);
                                castTime = baseCastTime / castingSpeed;
                                /*if (calculationOptions.Beta && Channeled)
                                {
                                    float tickCastTime = castTime / Ticks;
                                    averageTicks = (float)Math.Floor(baseCastTime / tickCastTime);
                                    castTime = averageTicks * tickCastTime;
                                }*/
                                if (InterruptFactor > 0)
                                {
                                    castTime = castTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f) * maxPushback * InterruptFactor;
                                }
                                castTime += latency;
                                if (castTime < globalCooldown + calculationOptions.LatencyGCD) castTime = globalCooldown + calculationOptions.LatencyGCD;
                            }
                        }
                    }
                }
            }

            /*if (Channeled && calculationOptions.Beta)
            {
                channelReduction = 1 - averageTicks / Ticks;
            }*/

            // channeled pushback
            if (Channeled && InterruptFactor > 0)
            {
                int maxLostTicks = (int)Math.Ceiling(averageTicks * 0.25f * Math.Max(0, 1 - interruptProtection));
                // pushbacks that happen up to pushbackCastTime cut the cast time to pushbackCastTime
                // pushbacks that happen after just terminate the channel
                // [---|---X---|---|---]
                castTime -= latency;
                float tickFactor = 0;
                for (int i = 0; i < maxLostTicks; i++)
                {
                    tickFactor += InterruptFactor * castTime / averageTicks * (i + 1) / averageTicks;
                }
                tickFactor += InterruptFactor * (averageTicks - maxLostTicks) * castTime / averageTicks * maxLostTicks / averageTicks;
                /*if (calculationOptions.Beta)
                {
                    channelReduction = 1 - averageTicks * (1 - tickFactor) / Ticks;
                }
                else*/
                {
                    channelReduction = tickFactor;
                }
                castTime = castTime * (1 - tickFactor) + latency;
            }

            return castTime;
        }
    }
}
