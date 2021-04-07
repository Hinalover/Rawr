using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Rawr.Mage
{
    public enum SpellId
    {
        None,
        [Description("Arcane Barrage (0)")]
        ArcaneBarrage,
        [Description("Arcane Barrage (1)")]
        ArcaneBarrage1,
        [Description("Arcane Barrage (2)")]
        ArcaneBarrage2,
        [Description("Arcane Barrage (3)")]
        ArcaneBarrage3,
        [Description("Arcane Barrage (4)")]
        ArcaneBarrage4,
        [Description("Arcane Barrage (5)")]
        ArcaneBarrage5,
        [Description("Arcane Barrage (6)")]
        ArcaneBarrage6,
        [Description("Arcane Missiles (0)")]
        ArcaneMissiles,
        [Description("Arcane Missiles (1)")]
        ArcaneMissiles1,
        [Description("Arcane Missiles (2)")]
        ArcaneMissiles2,
        [Description("Arcane Missiles (3)")]
        ArcaneMissiles3,
        [Description("Arcane Missiles (4)")]
        ArcaneMissiles4,
        [Description("Arcane Missiles (5)")]
        ArcaneMissiles5,
        [Description("Arcane Missiles (6)")]
        ArcaneMissiles6,
        [Description("Frostbolt")]
        Frostbolt,
        [Description("Deep Freeze")]
        DeepFreeze,
        [Description("Fireball")]
        Fireball,
        [Description("POM+Fireball")]
        FireballPOM,
        FireballBF,
        FrostfireBoltBF,
        [Description("Frostfire Bolt")]
        FrostfireBolt,
        FrostfireBoltFC,
        [Description("Pyroblast")]        
        PyroblastSpammed,
        PyroblastDotUptime,
        [Description("Pyroblast!")]
        PyroblastPOM,
        PyroblastPOMSpammed,
        PyroblastPOMDotUptime,
        [Description("Fire Blast")]
        FireBlast,
        [Description("Inferno Blast")]
        InfernoBlast,
        Combustion,
        [Description("Scorch")]
        Scorch,
        [Description("Living Bomb")]
        LivingBomb,
        [Description("Living Bomb AOE")]
        LivingBombAOE,
        [Description("Frost Bomb")]
        FrostBomb,
        [Description("Frost Bomb AOE")]
        FrostBombAOE,
        ArcaneBlastRaw,
        [Description("Arcane Blast (0)")]
        ArcaneBlast0,
        [Description("Arcane Blast (1)")]
        ArcaneBlast1,
        ArcaneBlast1NoCC,
        [Description("Arcane Blast (2)")]
        ArcaneBlast2,
        [Description("Arcane Blast (3)")]
        ArcaneBlast3,
        [Description("Arcane Blast (4)")]
        ArcaneBlast4,
        [Description("Arcane Blast (5)")]
        ArcaneBlast5,
        [Description("Arcane Blast (6)")]
        ArcaneBlast6,
        NetherTempest,
        NetherTempestAOE,
        NetherTempestDOT,
        NetherTempestDOTAOE,
        Slow,
        IceLance,
        FrozenOrb,
        [Description("Arcane Explosion")]
        ArcaneExplosion0,
        ArcaneExplosion1,
        ArcaneExplosion2,
        ArcaneExplosion3,
        ArcaneExplosion4,
        ArcaneExplosion5,
        ArcaneExplosion6,
        FlamestrikeSpammed,
        [Description("Flamestrike")]
        FlamestrikeSingle,
        [Description("Blizzard")]
        Blizzard,
        [Description("Blast Wave")]
        BlastWave,
        [Description("Dragon's Breath")]
        DragonsBreath,
        [Description("Cone of Cold")]
        ConeOfCold,
        IncantersWard,
        Waterbolt,
        MirrorImage,
    }

    // spell id: 31707, scaling id: 650
    public class WaterboltTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Waterbolt";
            InitializeCastTime(false, false, 2.5f, 0);
            InitializeScaledDamage(solver, false, 45, MagicSchool.Frost, 0.01f, 0.5000000000f, 0.2500000000f, 0, 0.5500000119f, 0, 1, 1, 0);
            if (solver.Character.Race == CharacterRace.Orc)
            {
                BaseSpellModifier *= 1.05f;
            }
            // TODO recheck all buffs that apply
            Dirty = false;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (!castingState.Frozen)
            {
                spell.AdditiveSpellModifier += castingState.FrostburnBonus; // waterbolt always benefits from Frostburn (but not shatter)
            }
            //spell.RawSpellDamage *= 0.4f;
            spell.CalculateDerivedStats(castingState);
            //spell.DamagePerSpellPower *= 0.4f;
            return spell;
        }
    }

    // spell id: 88084, scaling id: 674 (Arcane Blast)  0.275000005960464
    // spell id: 88082, scaling id: 673 (Fireball)      0.337999999523163
    // spell id: 59637, scaling id: -   (Fire Blast)    0.150000005960464 (base doesn't scale with level, fixed at 88-98?)
    // spell id: 59638, scaling id: 672 (Frostbolt)     0.25
    public class MirrorImageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Mirror Image";
            if (solver.Specialization == Specialization.Arcane && solver.MageTalents.GlyphOfMirrorImage)
            {
                // TODO should do something about AB debuff stacking
                InitializeCastTime(false, false, 2.5f, 0);
                InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0, 0.25900000333786f, 0.150000005960464f, 0, 0.275000005960464f, 0, 0, 0, 0);
            }
            else if (solver.Specialization == Specialization.Fire && solver.MageTalents.GlyphOfMirrorImage)
            {
                InitializeCastTime(false, false, 2.5f, 0);
                InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0, 0.328000009059906f, 0.241999998688698f, 0, 0.337999999523163f, 0, 0, 0, 0);
            }
            else
            {
                // 10 * Frostbolt + 4 * Fire Blast
                InitializeCastTime(false, false, 10f * 2f + 4f * 1.5f, 0);
                var options = solver.CalculationOptions;
                var average = options.GetSpellValue(0.234999999403954f);
                var halfSpread = (float)Math.Floor(average * 0.100000001490116f / 2f);
                average = (float)Math.Round(average);
                InitializeDamage(solver, false, 20, MagicSchool.Frost, 0, 10f * (average - halfSpread) + 4f * 88f, 10f * (average + halfSpread) + 4f * 98f, 10f * 0.25f + 4f * 0.150000005960464f, 0, 0, 0, 0, 0);
            }
            // TODO recheck all buffs that apply
            Dirty = false;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.RawSpellDamage *= 0.4f;
            spell.CalculateDerivedStats(castingState);
            spell.DamagePerSpellPower *= 0.4f;
            return spell;
        }
    }

    public class WandTemplate : SpellTemplate
    {
        private float speed;

        public WandTemplate()
        {
        }

        public WandTemplate(Solver solver, MagicSchool school, int minDamage, int maxDamage, float speed)
        {
            Initialize(solver, school, minDamage, maxDamage, speed);
        }

        public void Initialize(Solver solver, MagicSchool school, int minDamage, int maxDamage, float speed)
        {
            Name = "Wand";
            // Tested: affected by Arcane Instability, affected by Chaotic meta, not affected by Arcane Power
            InitializeEffectDamage(solver, school, minDamage, maxDamage);
            Range = 30;
            this.speed = speed;
            CritBonus = 1.5f * 1.33f * (1 + solver.BaseStats.BonusSpellCritDamageMultiplier);
            BaseSpellModifier = (1 + solver.BaseStats.BonusDamageMultiplier);
            switch (school)
            {
                case MagicSchool.Arcane:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusArcaneDamageMultiplier);
                    break;
                case MagicSchool.Fire:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusFireDamageMultiplier);
                    break;
                case MagicSchool.Frost:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusFrostDamageMultiplier);
                    break;
                case MagicSchool.Nature:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusNatureDamageMultiplier);
                    break;
                case MagicSchool.Shadow:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusShadowDamageMultiplier);
                    break;
            }
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CastTime = speed;
            spell.CritRate = castingState.CritRate;

            if (spell.CritRate < 0.0f) spell.CritRate = 0.0f;
            if (spell.CritRate > 1.0f) spell.CritRate = 1.0f;

            spell.SpellModifier = BaseSpellModifier;

            spell.HitProcs = HitRate;
            spell.CritProcs = spell.HitProcs * spell.CritRate;
            spell.TargetProcs = spell.HitProcs;

            spell.CalculateAverageDamage(castingState.Solver, 0, false, false);
            spell.DamagePerSpellPower = 0;
            spell.DamagePerMastery = 0;
            spell.DamagePerCrit = 0;
            spell.AverageThreat = spell.AverageDamage * ThreatMultiplier;
            spell.IgniteDamage = 0;
            spell.IgniteDamagePerSpellPower = 0;
            spell.IgniteDamagePerMastery = 0;
            spell.IgniteDamagePerCrit = 0;
            spell.AverageCost = 0;
            spell.OO5SR = 1;
            return spell;
        }
    }

    // spell id: 84721
    public class FrozenOrbTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            // TODO consider splitting explosion out for display purposes
            Name = "Frozen Orb";
            InitializeCastTime(false, true, 0, 60);
            InitializeScaledDamage(solver, false, 40, MagicSchool.Frost, 0.1f, 10 * 0.0000000000f, 0.0000000000f, 0, 10 * 0.3832499981f, 0, 10, 10, 0);
            Dirty = false;
        }
    }

    // spell id: 2948, scaling id: 27
    public class ScorchTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Scorch";
            InitializeCastTime(false, false, 1.5f, 0);
            InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0f, 0.9760000110f, 0.1700000018f, 0, 0.6277499795f, 0, 1, 1, 0);
            CausesIgnite = true;
            if (solver.Specialization == Specialization.Fire) // Critical Mass
            {
                CritRateMultiplier = 1.3f;
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 2120, scaling id: 19
    public class FlamestrikeTemplate : SpellTemplate
    {
        public virtual Spell GetSpell(CastingState castingState, bool spammedDot)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, false, spammedDot);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Flamestrike";
            InitializeCastTime(false, false, 2, 0);
            InitializeScaledDamage(solver, true, 40, MagicSchool.Fire, 0.06f, 0.4560000002f, 0.2020000070f, 4 * 0.1190000027f, 0.3885000050f, 4 * 0.1012500003f, 1, 1, 8f);
            DotTickInterval = 2;
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    public class ConjureManaGemTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Conjure Mana Gem";
            InitializeCastTime(false, false, 3, 0);
            InitializeScaledDamage(solver, false, 0, MagicSchool.Arcane, 0.18f, 0, 0, 0, 0, 0, 0, 1, 0);
            Dirty = false;
        }
    }

    public class IncantersWardTemplate : SpellTemplate
    {
        private const float spellPowerCoefficient = 1f;

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState);
            float absorb = castingState.CalculationOptions.GetSpellValue(1f) + spellPowerCoefficient * castingState.ArcaneSpellPower;
            spell.Absorb = absorb;
            float dps = castingState.Solver.IncomingDamageDps;
            spell.TotalAbsorb = Math.Min(absorb, 8f * dps);
            spell.AverageCost -= (0.18f * castingState.BaseStats.Mana * spell.TotalAbsorb / absorb); // 18% max mana absorb
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Incanter's Ward";
            InitializeCastTime(false, true, 0, 25);
            InitializeScaledDamage(solver, false, 0, MagicSchool.Arcane, 0f, 0, 0, 0, 0, 0, 0, 1, 0);
            Dirty = false;
        }
    }

    // spell id: 122, scaling id: 20
    public class FrostNovaTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Frost Nova";
            InitializeCastTime(false, true, 0, 25);
			InitializeScaledDamage(solver, true, 0, MagicSchool.Frost, 0.02f, 1.0000000000f, 0.0000000000f, 0, 0.1492500007f, 0, 1, 1, 0);
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 116, scaling id: 21
    public class FrostboltTemplate : SpellTemplate
    {
        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            /*if (castingState.Frozen)
            {
                spell.SpellModifier *= 1.2f;
            }*/
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Frostbolt";
            InitializeCastTime(false, false, 2f, 0);
            if (solver.Mage4T11)
            {
                BaseCastTime *= 0.9f;
            }
			InitializeScaledDamage(solver, false, 35, MagicSchool.Frost, 0.04f, 1.0000000000f, 0.0000000000f, 0, 1.3500000238f, 0, 1, 1, 0);
            //BaseCritRate += 0.05f * solver.BaseStats.Mage4T9;
            NukeProcs = 1;
            NukeProcs2 = 1;
            if (solver.Specialization == Specialization.Frost)
            {
                BaseSpellModifier *= 1.15f; // 3 stack Frostbolt debuff
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 71757, scaling id: 408
    // DEPRECATED
    public class DeepFreezeTemplate : SpellTemplate
    {
        //float fingersOfFrostCritRate;

        // 30 sec cooldown!!!
        public void Initialize(Solver solver)
        {
            Name = "Deep Freeze";
            InitializeCastTime(false, true, 0, 30);
            InitializeScaledDamage(solver, false, 35, MagicSchool.Frost, 0.09f, 1.3919999599f, 0.224999994039536f, 0, 2.0580000877f, 0, 1, 1, 0);
            if (solver.MageTalents.GlyphOfDeepFreeze)
            {
                BaseSpellModifier *= 1.2f;
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                // TODO proc not benefitting from shatter?
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 2136, scaling id: 17
    public class FireBlastTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Fire Blast";
            InitializeCastTime(false, true, 0, 8);
			InitializeScaledDamage(solver, false, 30, MagicSchool.Fire, 0.02f, 0.0000000000f, 0.1700000018f, 0, 0.7889999747f, 0, 1, 1, 0);
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 108853
    public class InfernoBlastTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Inferno Blast";
            InitializeCastTime(false, true, 0, 8);
			InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.02f, 0.0000000000f, 0.0000000000f, 0, 1.0130000114f, 0, 1, 1, 0);
            BaseCritRate = 1f;
            CausesIgnite = true;
            if (solver.Specialization == Specialization.Fire)
            {
                BaseSpellModifier *= 1.1f; // Pyromaniac
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 133, scaling id: 18
    public class FireballTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool pom, bool brainFreeze)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (brainFreeze)
            {
                spell.CostAmplifier = 0;
            }
            spell.CalculateDerivedStats(castingState, false, pom || brainFreeze, true);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Fireball";
            InitializeCastTime(false, false, 2.25f, 0);
            if (solver.Mage4T11)
            {
                BaseCastTime *= 0.9f;
            }
			InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.04f, 0.0000000000f, 0.2399999946f, 0, 1.3500000238f, 0, 1, 1, 0);
            NukeProcs = 1;
            NukeProcs2 = 1;
            CausesIgnite = true;
            if (solver.Specialization == Specialization.Fire)
            {
                CritRateMultiplier = 1.3f; // Critical Mass
                BaseSpellModifier *= 1.1f; // Pyromaniac
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 44614, scaling id: 22
    public class FrostfireBoltTemplate : SpellTemplate
    {
        // brain freeze proc chance
        // nether_tempest = 0.09
        // living_bomb = 0.25
        // frost_bomb = 1.00

        public Spell GetSpell(CastingState castingState, bool pom, bool brainFreeze)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (brainFreeze)
            {
                spell.CostAmplifier = 0;
            }
            spell.CalculateDerivedStats(castingState, false, pom || brainFreeze, true);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Frostfire Bolt";
            InitializeCastTime(false, false, 2.75f, 0);
            if (solver.Mage4T11)
            {
                BaseCastTime *= 0.9f;
            }
			InitializeScaledDamage(solver, false, 40, MagicSchool.FrostFire, 0.04f, 0.0000000000f, 0.2399999946f, 0, 1.3500000238f, 0, 1, 1, 0);
            NukeProcs = 1;
            NukeProcs2 = 1;
            CausesIgnite = true;
            if (solver.Specialization == Specialization.Fire)
            {
                CritRateMultiplier = 1.3f; // Critical Mass
                BaseSpellModifier *= 1.1f; // Pyromaniac
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 11366 (92315 for Pyroblast!, it actually has different scaling with spell power), scaling id: 26
    public class PyroblastTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool pom, bool spammedDot)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, pom, spammedDot);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, bool pom)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, pom, false, false, false, false, true);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Pyroblast!";
            InitializeCastTime(false, false, /*3.5f*/0, 0);
            // 5.2: 2.2000000477 * 0.9
			InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0f, 0.0000000000f, 0.2380000055f, 4 * 0.0000000000f, 1.6299999952f, 4 * 0.2969999909f, 1, 1, 12);
            DotDuration = 12;
            DotTickInterval = 3;
            NukeProcs2 = 1;
            CausesIgnite = true;
            BaseSpellModifier *= 1.25f; // Pyroblast! bonus
            if (solver.Specialization == Specialization.Fire)
            {
                CritRateMultiplier = 1.3f; // Critical Mass
                BaseSpellModifier *= 1.1f; // Pyromaniac
            }
            if (solver.Mage4T15)
            {
                BaseCritRate += 0.05f;
            }
            if (solver.Mage2T14)
            {
                BaseSpellModifier *= 1.08f;
            }
            if (solver.Mage2T11)
            {
                BaseCritRate += 0.05f;
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    public class PyroblastHardCastTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool pom, bool spammedDot)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, pom, spammedDot);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, bool pom)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, pom, false, false, false, false, true);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Pyroblast";
            InitializeCastTime(false, false, 3.5f, 0);
            // 5.2: 2.2000000477 * 0.9
            InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.05f, 1.9800000429f, 0.2380000055f, 4 * 0.3600000143f, 1.9800000429f, 4 * 0.3600000143f, 1, 1, 12);
            DotDuration = 12;
            DotTickInterval = 3;
            NukeProcs2 = 1;
            CausesIgnite = true;
            if (solver.Specialization == Specialization.Fire)
            {
                CritRateMultiplier = 1.3f; // Critical Mass
                BaseSpellModifier *= 1.1f; // Pyromaniac
            }
            if (solver.Mage4T15)
            {
                BaseCritRate += 0.05f;
            }
            if (solver.Mage2T14)
            {
                BaseSpellModifier *= 1.08f;
            }
            if (solver.Mage2T11)
            {
                BaseCritRate += 0.05f;
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 114923
    public class NetherTempestTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool aoe)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (aoe)
            {
                spell.AreaEffectDot = true;
            }
            spell.CalculateDerivedStats(castingState, false, false, false);
            return spell;
        }

        public Spell GetSpellDOT(CastingState castingState, bool aoe)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (aoe)
            {
                spell.AreaEffectDot = true;
            }
            spell.CalculateDerivedStats(castingState, false, false, false, false, false, false, true);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Nether Tempest";
            InitializeCastTime(false, true, 0f, 0f);
            // 3.12.2013 hotfix: 0.2230000049 * 1.4, 0.1739999950 * 1.4
			InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0.015f, 0.0000000000f, 0.0000000000f, 12 * 1.0000000000f, 0.0000000000f, 12 * 0.0309999995f, 0, 1, 0);
            MaximumAOETargets = 2;
            AOEMultiplier = 0.5f;
            DotDuration = 12;
            DotTickInterval = 1;
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 44457/44461, scaling id: 24/25
    public class LivingBombTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool aoe)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (aoe)
            {
                spell.AreaEffect = true; // but leave aoe dot at false
            }
            spell.CalculateDerivedStats(castingState, false, false, false);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Living Bomb";
            InitializeCastTime(false, true, 0f, 0f);
            GlobalCooldown = 1.0f;
            // 5.2: 3.12.2013 hotfix: 0.3330000043 * 1.4, 0.2599999905 * 1.4, 1.3400000334 * 1.4, 1.0449999571 * 1.4
            // 5.3: 2.21x dot, 0.22x explosion, no more aoe cap. TODO: make explosion scale with ActualTicks/4, assume 1.25 for now
            InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.015f, 1.25f * 0.412720010296f, 0, 4 * 1.03030201326f, 1.25f * 0.321859986778f, 4 * 0.804439970607f, 1, 1, 0);            
            // MaximumAOETargets = 3;
            AreaEffectDot = false;
            DotDuration = 12;
            DotTickInterval = 3;
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 113092
    public class FrostBombTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool aoe)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (aoe)
            {
                spell.AreaEffect = true; // but leave aoe dot at false
            }
            spell.CalculateDerivedStats(castingState, false, false, false);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Frost Bomb";
            InitializeCastTime(false, true, 1.5f, 10f);
            // 3.12.2013 hotfix: 3.1570000648 * 1.4, 2.4619998932 * 1.4 
            InitializeScaledDamage(solver, false, 40, MagicSchool.Frost, 0.0125f, 4.4198000907f, 0, 0, 3.4467998504f, 0, 1, 1, 0);
            //MaximumAOETargets = 10;
            AOEMultiplier = 0.5f;
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 11129, scaling id: 726
    public class CombustionTemplate : SpellTemplate
    {
        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            // calculate dot part based on spell values in current state
            Spell Pyro = castingState.GetSpell(SpellId.PyroblastDotUptime);
            Spell FB = castingState.GetSpell(SpellId.Fireball);

            // override so dot damage doesn't get any multipliers except crit
            spell.DotDamageModifier = 1 / (spell.SpellModifier * spell.AdditiveSpellModifier);

            // contribution from Pyro dot
            // removed in 5.1
            // spell.BasePeriodicDamage += Pyro.DotAverageDamage / (Pyro.DotDuration + Pyro.DotExtraTicks * Pyro.DotTickInterval);

            // estimate ignite contribution
            float igniteFactor = 0.4f * castingState.IgniteBonus / (1 + castingState.IgniteBonus);
            // TODO probabilistic model for rolling multiplier based on haste
            float rollingMultiplier = 1f;
            spell.BasePeriodicDamage += rollingMultiplier * (FB.AverageDamage + Pyro.AverageDamage) / 4f * igniteFactor;

            // extend to 10 ticks
            spell.BasePeriodicDamage *= 10;

            spell.CalculateDerivedStats(castingState, false, false, false);
            spell.CastTime = 0;
            return spell;
        }

        private float IgniteContributionCoefficient;

        public void Initialize(Solver solver)
        {
            Name = "Combustion";
            InitializeCastTime(false, true, 0f, 45f);
            InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.1f, 1.0000000000f, 0.1700000018f, 0, 1.0000000000f, 0, 1, 1, 0);
            DotDuration = 10;
            DotTickInterval = 1;
            BaseDotDamageModifier = 1 / BaseSpellModifier;
            UseMaxT13 = true;
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }

        public void Initialize(Solver solver, bool pyroDot, float pyroRoll, float fbRoll)
        {
            Name = "Combustion";
            InitializeCastTime(false, true, 0f, 45f);
            InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.1f, 1.0000000000f, 0.1700000018f, 0, 1.0000000000f, 0, 1, 1, 0);
            DotDuration = 10;
            DotTickInterval = 1;
            UseMaxT13 = true;

            // removed in 5.1 (or even earlier)
            // if (pyroDot)
            // {
            //     BasePeriodicDamage += 220.50485436893203883495145631068f / 3f;
            //     DotDamageCoefficient += 0.078058252427184466019417475728155f / 3f;
            // }

            float ignite = 0;

            FireballTemplate FB = solver.FireballTemplate;
            PyroblastTemplate Pyro = solver.PyroblastTemplate;

            IgniteContributionCoefficient = 0;
            BasePeriodicDamage += fbRoll * FB.BaseAverageDamage * solver.BaseFireCritBonus * 0.4f * ignite / 4.0f;
            IgniteContributionCoefficient += fbRoll * FB.SpellDamageCoefficient * solver.BaseFireCritBonus * 0.4f * ignite / 4.0f;
            BasePeriodicDamage += pyroRoll * Pyro.BaseAverageDamage * solver.BaseFireCritBonus * 0.4f * ignite / 4.0f;
            IgniteContributionCoefficient += pyroRoll * Pyro.SpellDamageCoefficient * solver.BaseFireCritBonus * 0.4f * ignite / 4.0f;
            DotDamageCoefficient += IgniteContributionCoefficient;
            // extend to 10 ticks
            BasePeriodicDamage *= 10;
            DotDamageCoefficient *= 10;
            IgniteContributionCoefficient *= 10;
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    public class SlowTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Slow";
            InitializeCastTime(false, true, 0f, 0f);
            InitializeScaledDamage(solver, false, 35, MagicSchool.Arcane, 0.12f, 0, 0, 0, 0, 0, 1, 1, 0);
            Dirty = false;
        }
    }

    // spell id: 120, scaling id: 15
    public class ConeOfColdTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Cone of Cold";
            InitializeCastTime(false, true, 0, 10);
			InitializeScaledDamage(solver, true, 0, MagicSchool.Frost, 0.04f, 0.0000000000f, 0.0000000000f, 0, 0.2384999990f, 0, 1, 1, 0);
            if (solver.MageTalents.GlyphOfConeOfCold)
            {
                BaseSpellModifier *= 1.25f;
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 30455, scaling id: 23
    public class IceLanceTemplate : SpellTemplate
    {
        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (castingState.Frozen)
            {
                spell.SpellModifier *= 4; // base ice lance bonus
                if (castingState.Solver.Specialization == Specialization.Frost)
                {
                    spell.AdditiveSpellModifier += 0.25f; // fingers of frost bonus
                }
            }
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Ice Lance";
            InitializeCastTime(false, true, 0, 0);
			InitializeScaledDamage(solver, false, 35, MagicSchool.Frost, 0.01f, 0.0000000000f, 0.2500000000f, 0, 0.7235999703f, 0, 1, 1, 0);
            if (solver.MageTalents.GlyphOfIceLance)
            {
                BaseSpellModifier *= 1.05f;
            }
            if (solver.Specialization == Specialization.Frost)
            {
                BaseSpellModifier *= 1.15f; // 3 stack Frostbolt debuff
            }
            if (solver.Mage2T11)
            {
                BaseCritRate += 0.05f;
            }
            if (solver.Mage2T14)
            {
                BaseSpellModifier *= 1.12f;
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 44425, scaling id: 8
    public class ArcaneBarrageTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, float arcaneBlastDebuff, bool aoe)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            if (aoe)
            {
                spell.AreaEffect = true;
                spell.MaximumAOETargets = Math.Max(MaximumAOETargets, (int)arcaneBlastDebuff + 1);
            }
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        private float arcaneBlastDamageMultiplier;

        public void Initialize(Solver solver)
        {
            Name = "Arcane Barrage";
            InitializeCastTime(false, true, 0, 3);
			InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0.005f, 0.0000000000f, 0.2000000030f, 0, 0.7500000000f, 0, 1, 1, 0);
            AOEMultiplier = 0.5f;
            arcaneBlastDamageMultiplier = 0.5f;
            if (solver.Mage4T15)
            {
                arcaneBlastDamageMultiplier *= 1.2f;
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 30451, scaling id: 9
    public class ArcaneBlastTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, int debuff)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * debuff;
            spell.CostModifier += arcaneBlastManaMultiplier * debuff;
			if (castingState.CalculationOptions.EnhancedArcaneBlast)
			{
				spell.BaseCastTime *= (1.0f - 0.05f * debuff);
			}
            spell.CalculateDerivedStats(castingState, false, false, false, true, false, false);
            return spell;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, false, false, true, false, false);
            return spell;
        }

        public void AddToCycle(Solver solver, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3)
        {
            MageTalents mageTalents = solver.MageTalents;
            float weight = weight0 + weight1 + weight2 + weight3;
            float hasteMultiplier = (rawSpell.CastTime - rawSpell.Latency) / (rawSpell.BaseCastTime);
            // if some are below gcd then we have to use different calculations
            if (hasteMultiplier * (rawSpell.BaseCastTime - castTimeMultiplier * 0.3f) <= Math.Max(hasteMultiplier * rawSpell.GlobalCooldown, Spell.GlobalCooldownLimit))
            {
                float channelReduction;
                float averageCastingSpeed;
                cycle.CastTime += weight0 * CalculateCastTime(cycle.CastingState, rawSpell.InterruptProtection, rawSpell.CritRate, false, rawSpell.BaseCastTime, out channelReduction, out averageCastingSpeed);
                cycle.CastTime += weight1 * CalculateCastTime(cycle.CastingState, rawSpell.InterruptProtection, rawSpell.CritRate, false, rawSpell.BaseCastTime - castTimeMultiplier * 0.1f, out channelReduction, out averageCastingSpeed);
                cycle.CastTime += weight2 * CalculateCastTime(cycle.CastingState, rawSpell.InterruptProtection, rawSpell.CritRate, false, rawSpell.BaseCastTime - castTimeMultiplier * 0.2f, out channelReduction, out averageCastingSpeed);
                cycle.CastTime += weight3 * CalculateCastTime(cycle.CastingState, rawSpell.InterruptProtection, rawSpell.CritRate, false, rawSpell.BaseCastTime - castTimeMultiplier * 0.3f, out channelReduction, out averageCastingSpeed);
            }
            else
            {
                cycle.CastTime += weight * rawSpell.CastTime - castTimeMultiplier * (weight1 * 0.1f + weight2 * 0.2f + weight3 * 0.3f) * hasteMultiplier;
            }
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.CastProcs2 += weight * rawSpell.CastProcs2;
            cycle.NukeProcs += weight * rawSpell.NukeProcs;
            cycle.NukeProcs2 += weight * rawSpell.NukeProcs2;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.HitPPM += weight * rawSpell.HitPPM;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;
            cycle.damageProcs += weight * rawSpell.HitProcs;

            float multiplier = (weight * rawSpell.AdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3)) / rawSpell.AdditiveSpellModifier;
            cycle.DpsPerSpellPower += multiplier * rawSpell.DamagePerSpellPower;
            cycle.DpsPerCrit += multiplier * rawSpell.DamagePerCrit;
            //cycle.DpsPerMastery += multiplier * rawSpell.DamagePerMastery;
            cycle.damagePerSecond += multiplier * rawSpell.AverageDamage;
            //cycle.threatPerSecond += multiplier * rawSpell.AverageThreat;
        }

        public void AddToCycle(Solver solver, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3, float weight4)
        {
            MageTalents mageTalents = solver.MageTalents;
            float weight = weight0 + weight1 + weight2 + weight3 + weight4;
            float hasteMultiplier = (rawSpell.CastTime - rawSpell.Latency) / (rawSpell.BaseCastTime);
            // if some are below gcd then we have to use different calculations
            if (hasteMultiplier * (rawSpell.BaseCastTime - castTimeMultiplier * 0.4f) <= Math.Max(hasteMultiplier * rawSpell.GlobalCooldown, Spell.GlobalCooldownLimit))
            {
                float channelReduction;
                float averageCastingSpeed;
                cycle.CastTime += weight0 * CalculateCastTime(cycle.CastingState, rawSpell.InterruptProtection, rawSpell.CritRate, false, rawSpell.BaseCastTime, out channelReduction, out averageCastingSpeed);
                cycle.CastTime += weight1 * CalculateCastTime(cycle.CastingState, rawSpell.InterruptProtection, rawSpell.CritRate, false, rawSpell.BaseCastTime - castTimeMultiplier * 0.1f, out channelReduction, out averageCastingSpeed);
                cycle.CastTime += weight2 * CalculateCastTime(cycle.CastingState, rawSpell.InterruptProtection, rawSpell.CritRate, false, rawSpell.BaseCastTime - castTimeMultiplier * 0.2f, out channelReduction, out averageCastingSpeed);
                cycle.CastTime += weight3 * CalculateCastTime(cycle.CastingState, rawSpell.InterruptProtection, rawSpell.CritRate, false, rawSpell.BaseCastTime - castTimeMultiplier * 0.3f, out channelReduction, out averageCastingSpeed);
                cycle.CastTime += weight4 * CalculateCastTime(cycle.CastingState, rawSpell.InterruptProtection, rawSpell.CritRate, false, rawSpell.BaseCastTime - castTimeMultiplier * 0.4f, out channelReduction, out averageCastingSpeed);
            }
            else
            {
                cycle.CastTime += weight * rawSpell.CastTime - castTimeMultiplier * (weight1 * 0.1f + weight2 * 0.2f + weight3 * 0.3f + weight4 * 0.4f) * hasteMultiplier;
            }
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.CastProcs2 += weight * rawSpell.CastProcs2;
            cycle.NukeProcs += weight * rawSpell.NukeProcs;
            cycle.NukeProcs2 += weight * rawSpell.NukeProcs2;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.HitPPM += weight * rawSpell.HitPPM;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;
            cycle.damageProcs += weight * rawSpell.HitProcs;

            float multiplier = (weight * rawSpell.AdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3 + 4 * weight4)) / rawSpell.AdditiveSpellModifier;
            cycle.DpsPerSpellPower += multiplier * rawSpell.DamagePerSpellPower;
            cycle.DpsPerCrit += multiplier * rawSpell.DamagePerCrit;
            //cycle.DpsPerMastery += multiplier * rawSpell.DamagePerMastery;
            cycle.damagePerSecond += multiplier * rawSpell.AverageDamage;
            //cycle.threatPerSecond += multiplier * rawSpell.AverageThreat;
        }

        private float arcaneBlastDamageMultiplier;
        private float arcaneBlastManaMultiplier;
        private float castTimeMultiplier;

        public void Initialize(Solver solver)
        {
            Name = "Arcane Blast";
            InitializeCastTime(false, false, 2.25f, 0);
			float castTimeMultiplier = 1.0f;
            if (solver.Mage4T11)
            {
                castTimeMultiplier = 0.9f;
            }
			if (castTimeMultiplier < 1.0f)
			{
				BaseCastTime *= castTimeMultiplier;
			}
			InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0.0167f, 1.0000000000f, 0.0000000000f, 0, 0.8651999831f, 0, 1, 1, 0);
            Stats baseStats = solver.BaseStats;
            MageTalents mageTalents = solver.MageTalents;
            arcaneBlastDamageMultiplier = 0.5f;
            arcaneBlastManaMultiplier = 1.5f;
            if (solver.Mage4T15)
            {
                arcaneBlastDamageMultiplier *= 1.2f;
                arcaneBlastManaMultiplier *= 1.2f;
            }
            NukeProcs = 1;
            NukeProcs2 = 1;
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 7268, scaling id: 12
    public class ArcaneMissilesTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool barrage, int arcaneBlastDebuff)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (barrage)
            {
                spell.BaseCastTime *= 0.5f;
                spell.CostModifier = Math.Max(spell.CostModifier - 1, 0);
            }
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public void AddToCycle(Solver solver, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3, float weight4)
        {
            MageTalents mageTalents = solver.MageTalents;
            float weight = weight0 + weight1 + weight2 + weight3 + weight4;
            cycle.CastTime += weight * rawSpell.CastTime;
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.CastProcs2 += weight * rawSpell.CastProcs2;
            cycle.NukeProcs += weight * rawSpell.NukeProcs;
            cycle.NukeProcs2 += weight * rawSpell.NukeProcs2;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.HitPPM += weight * rawSpell.HitPPM;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;
            cycle.costPerSecond += weight * rawSpell.AverageCost;
            cycle.damageProcs += weight * rawSpell.HitProcs;
            float multiplier = (weight * rawSpell.AdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3 + 4 * weight4)) / rawSpell.AdditiveSpellModifier;
            cycle.DpsPerSpellPower += multiplier * rawSpell.DamagePerSpellPower;
            //cycle.DpsPerMastery += multiplier * rawSpell.DamagePerMastery;
            cycle.damagePerSecond += multiplier * rawSpell.AverageDamage;
            //cycle.threatPerSecond += multiplier * rawSpell.AverageThreat;
        }

        float arcaneBlastDamageMultiplier;

        public void Initialize(Solver solver)
        {
            Name = "Arcane Missiles";
            float castTime = 0.75f;
            castTime = 0.4f;
            int missiles = 5;
            InitializeCastTime(true, false, castTime * missiles, 0);
			InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0, 1.0000000000f, 0.0000000000f, 0, 0.2196000069f, 0, missiles, missiles + 1, 0);
            BaseAverageDamage *= missiles;
            SpellDamageCoefficient *= missiles;
            CastProcs2 = 1;
            arcaneBlastDamageMultiplier = 0.5f;
            if (solver.Mage4T15)
            {
                arcaneBlastDamageMultiplier *= 1.2f;
            }
            if (solver.Mage2T14)
            {
                BaseSpellModifier *= 1.07f;
            }
            if (solver.Mage2T11)
            {
                BaseCritRate += 0.05f;
            }
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 1449, scaling id: 10
    public class ArcaneExplosionTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, float arcaneBlastDebuff)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            //if (castingState.CalculationOptions.ModePTR)
            //{
            //    spell.CostAmplifier = 1f - 0.25f * castingState.MageTalents.ImprovedArcaneExplosion;
            //}
            //spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        //private float arcaneBlastDamageMultiplier;

        public void Initialize(Solver solver)
        {
            Name = "Arcane Explosion";
            InitializeCastTime(false, true, 0, 0);
			InitializeScaledDamage(solver, true, 0, MagicSchool.Arcane, 0.03f, 0.0000000000f, 0.0799999982f, 0, 0.4950000048f, 0, 1, 1, 0);
            // should we count torment the weak?
            //arcaneBlastDamageMultiplier = solver.MageTalents.GlyphOfArcaneBlast ? 0.13f : 0.1f;
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 11113, scaling id: 13
    public class BlastWaveTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Blast Wave - REMOVE";
            InitializeCastTime(false, true, 0, 15);
            InitializeScaledDamage(solver, true, 0, MagicSchool.Fire, 0.07f, 0.989000022411346f, 0.164000004529953f, 0, 0.14300000667572f, 0, 1, 1, 0);
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 31661, scaling id: 16
    public class DragonsBreathTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Dragon's Breath";
            InitializeCastTime(false, true, 0, 20);
            InitializeScaledDamage(solver, true, 0, MagicSchool.Fire, 0.04f, 1.9670000076f, 0.1500000060f, 0, 0.2150000036f, 0, 1, 1, 0);
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    // spell id: 42208, scaling id: 14
    public class BlizzardTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Blizzard";
            InitializeCastTime(true, false, 8, 0);
            InitializeScaledDamage(solver, true, 30, MagicSchool.Frost, 0.05f, 8 * 0.2310000062f, 0, 0, 8 * 0.2619999945f, 0, 8, 1, 0);
            if (solver.BaseStats.DragonwrathProc > 0)
            {
                BaseSpellModifier *= 1.1f;
                Ticks *= 1.1f;
                CastProcs *= 1.1f;
                CastProcs2 *= 1.1f;
            }
            Dirty = false;
        }
    }

    public class ArcaneDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Arcane Damage";
            InitializeEffectDamage(solver, MagicSchool.Arcane, 1, 1);
            CritBonus = 2.0f * 1.33f * (1 + solver.BaseStats.BonusSpellCritDamageMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusArcaneDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class FireDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Fire Damage";
            InitializeEffectDamage(solver, MagicSchool.Fire, 1, 1);
            CritBonus = 2.0f * 1.33f * (1 + solver.BaseStats.BonusSpellCritDamageMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusFireDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class FrostDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Frost Damage";
            InitializeEffectDamage(solver, MagicSchool.Frost, 1, 1);
            CritBonus = 2.0f * 1.33f * (1 + solver.BaseStats.BonusSpellCritDamageMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusFrostDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class ShadowDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Shadow Damage";
            InitializeEffectDamage(solver, MagicSchool.Shadow, 1, 1);
            CritBonus = 2.0f * 1.33f * (1 + solver.BaseStats.BonusSpellCritDamageMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusShadowDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class NatureDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Nature Damage";
            InitializeEffectDamage(solver, MagicSchool.Nature, 1, 1);
            CritBonus = 2.0f * 1.33f * (1 + solver.BaseStats.BonusSpellCritDamageMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusNatureDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class HolyDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Holy Damage";
            InitializeEffectDamage(solver, MagicSchool.Holy, 1, 1);
            CritBonus = 2.0f * 1.33f * (1 + solver.BaseStats.BonusSpellCritDamageMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusHolyDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class HolySummonedDamageTemplate : SpellTemplate
    {
        public float Multiplier;

        public void Initialize(Solver solver)
        {
            Name = "Holy Damage";
            // TODO recheck all buffs that apply
            float spellCrit = 0.05f + solver.TargetDebuffs.SpellCritOnTarget;
            // summoned always hit
            RealResistance = solver.CalculationOptions.HolyResist;
            PartialResistFactor = (RealResistance == -1) ? 0 : (1 - StatConversion.GetAverageResistance(solver.CalculationOptions.PlayerLevel, solver.CalculationOptions.TargetLevel, RealResistance, 0));
            Multiplier = PartialResistFactor * (1 + solver.TargetDebuffs.BonusDamageMultiplier) * (1 + solver.TargetDebuffs.BonusHolyDamageMultiplier) * (1 + (1.5f * 1.33f - 1) * spellCrit);
            Dirty = false;
        }
    }

    public class FireSummonedDamageTemplate : SpellTemplate
    {
        public float Multiplier;

        public void Initialize(Solver solver)
        {
            Name = "Fire Damage";
            // TODO recheck all buffs that apply
            float spellCrit = 0.05f + solver.TargetDebuffs.SpellCritOnTarget;
            // summoned always hit
            RealResistance = solver.CalculationOptions.FireResist;
            PartialResistFactor = (RealResistance == -1) ? 0 : (1 - StatConversion.GetAverageResistance(solver.CalculationOptions.PlayerLevel, solver.CalculationOptions.TargetLevel, RealResistance, 0));
            Multiplier = PartialResistFactor * (1 + solver.TargetDebuffs.BonusDamageMultiplier) * (1 + solver.TargetDebuffs.BonusFireDamageMultiplier) * (1 + (1.5f * 1.33f - 1) * spellCrit);
            Dirty = false;
        }
    }
}
