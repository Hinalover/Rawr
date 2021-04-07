using System;
using System.Collections.Generic;
using System.Text;

/* Antivyris Notes for stuff implemented for 5.4
 * CURRENT - Implemented a holy power tracking for total HoPo
 * CURRENT - Removing the melee attacks for mana
 * CURRENT - Rebuild rotation model to reflect Sequences 
 * CURRENT - Update all spells to current SoO Variants
 * 
*/
/* Molotok's notes on stuff implemented for 4.3
 * 
 * DONE # Holy Radiance now has a 3.0-second cast time, 
 * NOT done  no cooldown, and requires a player target. 
 *       That target is imbued with Holy Radiance, which heals them and all group members within 10 yards instantly, 
 *       and continues to heal them by a smaller amount every 1 second for 3 seconds.
 *       (check if spellpower coefficient has changed)
 * DONE # Seal of Insight, when Judged, no longer returns 15% base mana to the paladin. 
 *       Judging Seal of Insight still causes damage, and melee attacks will still restore 4% of base mana.
 * 
 * DONE * Clarity of Purpose now also reduces the cast time of Holy Radiance.
 * DONE * Illuminated Healing (mastery) now also applies to Holy Radiance.
 *  * Infusion of Light now applies its cast time reduction from Holy Shock critical effects to Holy Radiance, in addition to its current effects.
 * DONE (assumed 100% uptime) * In addition to providing haste, the effect from Judgements of the Pure now increases mana regeneration from Spirit by 10/20/30% for 60 seconds.
 * DONE * Light of Dawn now affects 6 targets (base effect), up from 5.
 *  * Paragon of Virtue now lowers the cooldown of Divine Protection by 15/30 seconds, up from 10/20 seconds.
 *  * Speed of Light no longer triggers from Holy Radiance (20% speed buff) and no longer lowers the Holy Radiance cooldown. 
 *       Speed of Light now only triggers from Divine Protection.
 * DONE * Tower of Radiance, in addition to its current effects, now also causes Holy Radiance to always generate 1 charge of Holy Power at all times.
 * 
 * DONE * Beacon of Light is triggered by Word of Glory, Holy Shock, Flash of Light, Divine Light and Light of Dawn at 50% transference and 
 *       Holy Light at 100% transference. It does not transfer Holy Radiance, Protector of the Innocent or other sources of healing.
 * 
 * DONE * Glyph of Light of Dawn now lowers the number of targets to 4, instead of increasing targets to 6, but increases healing by 25%.
 * 
 * 
 * 
 *Left to do: (in no particular order)
 *  Last Word(0-2) - 30% extra WoG crit per point, when target below 35% health
 *  Divine Favor(0-1) - increase haste/crit 20% for 20 secs.  3 min CD
 *  Daybreak(0-2) - FoL, DL, HL have 10% chance per point to make next HS not trigger its 6 sec CD.
 *  Tower of Radiance(0-3) - healing beacon target with FoL or DL has 33% chance per point of giving a Holy Power
 *  Glyph of Divine Favor
 *  Avenging Wrath
 *  Guardian of Ancient Kings
 *  T12 set bonuses
 *  Divine Plea: causes 50% heals
 *  
 * 
 *Done, with assumptions (which can't be changed in the options tab):
 *  Speed of Light - assumes 3 points for Holy Radiance CD reduction.
 *  Assumes you are holy spec, so you get:
 *   - Illuminated Healing (shield on heals, Mastery ability)
 *   - Meditation (50% spirit regen in combat)
 *   - Walk in the light (+10% to all heals)
 *  Seal of Insight is always up
 *  Infusion of Light -0.75 cast time reduction is applied after haste.  Also a HL or DL can be cast before this procs again.
 *  
 * 
 *Not done, but no current plans to do:
 *  Crusade (0-3) - 2nd part - after killing something, next HL heals for 100% extra per point, for 15 sec.
 *  Blessed Life(0-2) - 50% chance to gain holy power when taking direct damage.  8 sec CD.
 *  figure hit into melee / Judgement mana regen - leaning towards not bothering to do this... *  
 *  Enlightened Judgements(0-2) - 1st part - gives +50% spirit to hit per point 
 * 
 * 
 *Known shortcomings of the model:
 *  Crits often cause overheals.  Model does not take this into account.
 * 
 * 
 *DONE  Gemming template (except jewelcrafting gems)
 *DONE  add cleansing to options tab, make default 10 casts.  Then model it in rotation and do Glyph of Cleansing
 *DONE  add melee based mana regen.  add something in options tab for how much melee is done.  Available time to melee will = total instant cast "cast time".
 *
 *DONE Illuminated Healing - Your direct healing spells also place an absorb shield on your target for 12% of the amount healed lasting 15 sec. 
 *DONE                       Each point of Mastery increases the absorb amount by an additional 1.50%.
 * 
 *Talents:
 *DONE(assumes Holy Spec always) Walk in the Light (for selecting Holy specialization)- 10% heal bonus
 *DONE Protector of the Innocent(0-3) - additional self heal when casting a targeted heal not on yourself
 *DONE Judgement of the Pure(0-3) - increases haste 3% per point
 *DONE Clarity of Purpose(0-3) - reduce cast time of HL and DL by .15 per point
 *  Last Word(0-2) - 30% extra WoG crit per point, when target below 35% health
 *  Divine Favor(0-1) - increase haste/crit 20% for 20 secs.  3 min CD
 *DONE Infusion of Light(0-2) - 5% holy shock crit per point
 *DONE                        - HS crit = -0.75 sec per point from next DL/HL
 *  Daybreak(0-2) - FoL, DL, HL have 10% chance per point to make next HS not trigger its 6 sec CD.
 *  Enlightened Judgements(0-2) - gives +50% spirit to hit per point
 *DONE                             - Judgement self heal
 *DONE Beacon of Light(0-1) - 50% of heals to beacon target
 *DONE Speed of Light(0-3) - 1% haste per point
 *Currently assuming 3 points for HR CD reduction. - reduce HR CD by 10 sec per point
 *DONE  Conviction(0-3) - 1% heal bonus per point, for 15 secs after a crit from non-periodic spell.  (or weapon swing)
 *  Tower of Radiance(0-3) - healing beacon target with FoL or DL has 33% chance per point of giving a Holy Power
 *  Blessed Life(0-2) - 50% chance to gain holy power when taking direct damage.  8 sec CD.
 *DONE Light of Dawn(0-1) - gives the spell.
 * 
 *DONE Divinity(0-3) - 2% healing increase per point
 *DONE Crusade(0-3) - 10% per point increase HS heals
 *              - after killing something, next HL heals for 100% extra per point, for 15 sec.
 * 
 *Glyphs
 *Prime                                            
 *DONE Glyph of Word of Glory
 *DONE Glyph of Seal of Insight  
 *DONE Glyph of Holy Shock      
 *  Glyph of Divine Favor
 *Major
 *DONE  Glyph of Beacon of Light - 0 mana cost for casting
 *DONE Glyph of Divine Plea
 *DONE  Glyph of Cleansing
 *DONE Glyph of Divinity - 10% mana when casting LoH
 *DONE-not going to model.  Glyph of Salvation - thread reducing mechanic, don't bother modeling
 *DONE-not going to model.  Glyph of Long Word - changes half WoG heals to HoT.
 *DONE Glyph of Light of Dawn  
 *DONE Glyph of Lay on Hands - reduced CD by 3 min.  (from 10 to 7)
 * 
 *Other CDs:
 *  Avenging Wrath
 *  Guardian of Ancient Kings
 *  
 */

namespace Rawr.Healadin
{

    public static class HealadinConstants
    {
        public static float basemana = 60000;

        // Spell coeficients - Updated for 5.4 - AV
        // source:  http://www.wowhead.com - Datamined Coefficients
        // Flash of Light
        public static float fol_coef = 1.12f;
        public static float fol_mana = basemana * 0.378f;
        public static float fol_min = 12607;
        public static float fol_max = 12607;

        // Holy Light
        public static float hl_coef = 0.785f;
        public static float hl_mana = basemana * 0.126f;
        public static float hl_min = 8868f;
        public static float hl_max = 8868f;

        // Divine Light
        public static float dl_coef = 1.49f;
        public static float dl_mana = basemana * 0.36f;
        public static float dl_min = 16817f;
        public static float dl_max = 16817;

        // Holy Shock
        public static float hs_coef = 0.833f;
        public static float hs_mana = basemana * 0.08f;
        public static float hs_min = 9014f;
        public static float hs_max = 9764f;

        // Word of Glory
        // Stats for 1 holy power.  Scales linearly with more holy power. (so just * by 2 or 3 when needed)
        public static float wog_coef_sp = 0.49f;
        public static float wog_coef_ap = 0f;  // Attack Power coef to be removed, only active while Ret - AV
        public static float wog_min = 4803f;
        public static float wog_max = 5350f;

        // Eternal Flame
        // Stats for 1 holy power.  Scales linearly with more holy power. (so just * by 2 or 3 when needed)
        public static float ef_coef_sp = 0.49f;
        public static float ef_min = 5239f;
        public static float ef_max = 5837f;
        public static float ef_coef_hot_sp = 0.0819f;
        public static float ef_hot = 711f;

        // Stats for 1 holy power, 1 target.  Hits up to 6 targets, 4 glyphed.
        public static float lod_coef = 0.152f;
        public static float lod_min = 1627f;
        public static float lod_max = 1812f;

        // Holy Radiance.  
        public static float hr_coef = 0.675f;
        public static float hr_mana = basemana * 0.36f;
        public static float hr_direct = (5098f + 6230f) / 2f;
        public static float hr_coef_hot = 0f;  //HOT Removed in MoP
        public static float hr_hot = 0f;  //HOT Removed in MoP

        // Protector of the Innocent section, will be removed
        public static float poti_coef = 0.039233f; 
        public static float poti_min = 2481f;
        public static float poti_max = 2853f;

        //Judgement self heals removed in MoP, prepare to remove
        public static float ej_coef = 0f;  
        public static float ej_min = 0f;
        public static float ej_max = 0f;

    }

    public abstract class Heal
    {
        private Rotation _rotation;
        public Rotation Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _stats = _rotation.Stats;
                _talents = _rotation.Talents;
            }
        }

        private Stats _stats;
        protected Stats Stats { get { return _stats; } }

        private PaladinTalents _talents;
        protected PaladinTalents Talents { get { return _talents; } }

        public Heal(Rotation rotation)
        {
            Rotation = rotation;
        }

        public float HPS() { return AverageHealed() / CastTime(); }
        public float MPS() { return AverageCost() / CastTime(); }
        public float HPM() 
        {
            if (BaseMana == 0)
                return 0f;
            else
                return AverageHealed() / AverageCost(); 
        }

        public float CastTime() { return (float)Math.Max(1f, (BaseCastTime - AbilityCastTimeReduction()) / (1f + Stats.SpellHaste));}

        public float AverageCost()
        {
            return (float)Math.Floor((BaseMana - CostReduction()) * (AbilityCostMultiplier()));
        }

        public float AverageHealed()
        {
            return BaseHealed() * (1f - ChanceToCrit()) + CritHealed() * ChanceToCrit();
        }

        // Illuminated Healing
        public float AverageShielded()
        {
            return AverageHealed() * (1f + StatConversion.GetMasteryFromRating(Stats.MasteryRating) * 0.1f);
        }

        public float CritHealed() 
        {
            float critmultiplier = 2f; // *(1f - Stats.BonusCritChanceObliterate);  // had been temporarily using BonusCritChanceObliterate to track option for crit overheals (since removed)
            return BaseHealed() * critmultiplier * (1f + Stats.BonusCritHealMultiplier) * AbilityCritMultiplier(); 
        }

        public float BaseHealed()
        {
            float heal = AbilityHealed();

            heal *= Talents.GlyphOfSealOfInsight ? 1.05f : 1;
            heal *= 1f + Stats.HealingReceivedMultiplier;
            // heal *= 1f - Rotation.DivinePleas * 15f / Rotation.FightLength * .5f; - Divine plea no longer reduces healing
            heal *= AbilityHealedMultiplier();
            heal *= (1f + Stats.BonusHealingDoneMultiplier);

            // Walk in the Light
            // heal *= (1f + 0.1f); // Removed in MoP

            return heal;
        }

        public float ChanceToCrit()
        {
            // TODO: figure out how to add Talents.DivineFavor into crit chance, when do we proc it, which casts are proced, etc.
            return (float)Math.Max(0f, (float)Math.Min(1f, Stats.SpellCrit + AbilityCritChance() + ExtraCritChance ));
        }
        public float CostReduction() { return Stats.SpellsManaCostReduction + Stats.HolySpellsManaCostReduction + AbilityCostReduction(); }

        public float ExtraCritChance { get; set; }
        public bool DivineIllumination { get; set; }

        protected abstract float AbilityHealed();
        protected virtual float AbilityCritChance() { return 0f; }
        protected virtual float AbilityCostReduction() { return 0f; }
        protected virtual float AbilityCastTimeReduction() { return 0f; }
        protected virtual float AbilityCostMultiplier() { return 1f; }
        protected virtual float AbilityHealedMultiplier() { return 1f; }
        protected virtual float AbilityCritMultiplier() { return 1f; }

        public abstract float BaseCastTime { get; }
        public abstract float BaseMana { get; }

        public override string ToString()
        {
            return string.Format("Average Heal: {0:N0}\nAverage Cost: {1:N0}\nHPS: {2:N0}\nHPM: {3:N2}\nCast Time: {4:N2} sec\nCrit Chance: {5:N2}%",
                AverageHealed(),
                AverageCost(),
                HPS(),
                HPM(),
                CastTime(),
                ChanceToCrit() * 100);
        }

        protected static float ClarityOfPurpose(int talentPoints) {
            switch (talentPoints) {
                case 1:
                    return 0.15f;
                case 2:
                    return 0.35f;
                case 3:
                    return 0.5f;
                default:
                    return 0f;
            }
        }
    }

    public class FlashOfLight : Heal
    {
        public FlashOfLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return HealadinConstants.fol_mana; } } 

        protected override float AbilityHealed()
        {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (HealadinConstants.fol_min + HealadinConstants.fol_max) / 2f + ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.fol_coef);
        }
    }

    public class HolyLight : Heal
    {
        public HolyLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 2.5f; } }
        public override float BaseMana { get { return HealadinConstants.hl_mana; } } 

        public float CastTimeReduction { get; set; }

        protected override float AbilityHealed()
        {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (HealadinConstants.hl_min + HealadinConstants.hl_max) / 2f + ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.hl_coef);
        }

    }

    public class DivineLight : Heal
    {
        public DivineLight(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 2.5f; } }
        public override float BaseMana { get { return HealadinConstants.dl_mana; } } 

        protected override float AbilityHealed() {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (HealadinConstants.dl_min + HealadinConstants.dl_max) / 2f + ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.dl_coef);
        }
    }

    public class HolyShock : Heal
    {
        public HolyShock(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return HealadinConstants.hs_mana; } }
        protected override float AbilityCritChance()
        {
            return (0.25f);
        }


        protected override float AbilityHealed()
        {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (((HealadinConstants.hs_min + HealadinConstants.hs_max) / 2f +
                     ((Stats.SpellPower+ Stats.Intellect) * HealadinConstants.hs_coef)) * (1f + Talents.Crusade * 0.1f));
        }

        public float Usage()
        {
            return Casts() * AverageCost();
        }

        public float Casts()
        {
            // if this was set to an invadid value, set it to a default value
            // the could have been from an issue where I used to track the cast time as a % of the max number of times it could be cast
            // thus loading a saved character file resulted in bad results
            // however I changed these things on the options tab to be seconds between casts
            if (Rotation.CalcOpts.HolyShock < 6)
            {
                Rotation.CalcOpts.HolyShock = 7.5f;
                Rotation.CalcOpts.HRCasts = 60f;
                Rotation.CalcOpts.JudgementCasts = 10f;
            }
            return Rotation.ActiveTime / Rotation.CalcOpts.HolyShock;
        }

        public float Time()
        {
            return Casts() * CastTime();
        }

        public float Healed()
        {
            return Casts() * AverageHealed();
        }

        public float Cooldown()
        {
            return 6f; // TODO: Account for talent Daybreak
        }

        /* this must have been for an outdated version of IoL talent.  It no longer effects crit multiplier. (patch 4.1)
        protected override float AbilityCritMultiplier()
        {
            return 1f + (Talents.GlyphOfHolyShock ? 0.05f : 0f) + (Talents.InfusionOfLight * 0.05f);
        }*/

    }

    public class WordofGlory : Heal
    {
        public WordofGlory(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return 0f; } }
        /* public override float ExtraCritChance { get { return (Talents.LastWord * 0.3f); } } how do I figure if the target is below 35% health? */ 
    

        protected override float AbilityHealed()
        {
            float holypower = 3f;  // assume 3 holypower for now
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return holypower * ( // 1.3 is for patch 4.2 update, Walk in the Light now gives that bonus 
                                (HealadinConstants.wog_min + HealadinConstants.wog_max) / 2f + 
                                ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.wog_coef_sp));
        }
    }

    public class LightofDawn : Heal
    {
        public LightofDawn(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return 0f; } }

        protected override float AbilityHealed()
        {
            //TODO: find if we are glyphed 
            float targets_healed = 6f - (Talents.GlyphOfLightOfDawn ? 2f : 0f);
            float holypower = 3f; // assume 3 holy power for now
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (Talents.GlyphOfLightOfDawn ? 1.25f : 1f) * holypower * targets_healed * 
                     ((HealadinConstants.lod_min + HealadinConstants.lod_max) / 2f + ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.lod_coef));
        }
    }

    public class HolyRadiance : Heal
    {
        public HolyRadiance(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 2.5f; } }
        public override float BaseMana { get { return HealadinConstants.hr_mana; } }

        protected override float AbilityHealed()
        {
            float targets_healed = 6f;

            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return targets_healed * (HealadinConstants.hr_direct   + ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.hr_coef));
        }
    }

    public class LayonHands : Heal
    {
        public LayonHands(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return 0f; } }

        protected override float AbilityHealed()
        {
            return Stats.Health;
        }

        public float Cooldown()
        {
            return (Talents.GlyphOfLayOnHands ? 420f : 600f);
        }

        public float Casts()
        {
            return (1f + (float)Math.Floor(Rotation.ActiveTime / Cooldown()));
        }

    }

    public class ProtectoroftheInnocent : Heal
    {
        public ProtectoroftheInnocent(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } } // not cast really, but dont want to cause div by 0 errors(potential HPS calculations), so leaving this for now
        public override float BaseMana { get { return 0f; } }

        protected override float AbilityHealed()
        {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (((HealadinConstants.poti_min + HealadinConstants.poti_max) / 2f +
                    ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.poti_coef)) *
                      (Talents.ProtectorOfTheInnocent / 3));
        }
    }

    public class EnlightenedJudgements : Heal
    {
        public EnlightenedJudgements(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } } // not cast really, but dont want to cause div by 0 errors(potential HPS calculations), so leaving this for now
        public override float BaseMana { get { return 0f; } }

        protected override float AbilityHealed()
        {
            // TODO: calculate real spellpower somewhere in Healadin module, and use that instead of Stats.SpellPower + Stats.Intellect
            return (((HealadinConstants.ej_min + HealadinConstants.ej_max) / 2f +
                    ((Stats.SpellPower + Stats.Intellect) * HealadinConstants.ej_coef)) *
                      (Talents.EnlightenedJudgements / 2));
        }
    }

    public class Cleanse : Heal
    {
        public Cleanse(Rotation rotation)
            : base(rotation)
        { }

        public override float BaseCastTime { get { return 1.5f; } }
        public override float BaseMana { get { return ( HealadinConstants.basemana * 0.164f); } }

        protected override float AbilityHealed()
        {
            return (0f);
        }
    }


    public abstract class Spell
    {

        private Rotation _rotation;
        public Rotation Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _stats = _rotation.Stats;
                _talents = _rotation.Talents;
            }
        }

        private Stats _stats;
        protected Stats Stats { get { return _stats; } }

        private PaladinTalents _talents;
        protected PaladinTalents Talents { get { return _talents; } }

        public Spell(Rotation rotation)
        {
            Rotation = rotation;
        }

        public float Uptime { get; set; }
        public float Duration { get; set; }
        public float BaseCost { get; set; }

        public float Cost()
        {
            return (BaseCost - Stats.SpellsManaCostReduction - Stats.HolySpellsManaCostReduction);
        }

        public float Casts()
        {
            return (float)Math.Ceiling(Uptime / Duration);
        }

        public virtual float CastTime()
        {
            return (float)Math.Max(1f, 1.5f / (1f + Stats.SpellHaste));
        }

        public virtual float Time()
        {
            return Casts() * CastTime();
        }

        public virtual float Usage()
        {
            return Casts() * Cost();
        }

    }

    public class BeaconOfLight : Spell
    {
        public BeaconOfLight(Rotation rotation)
            : base(rotation)
        {
            Duration = 300f;
            Uptime = Rotation.FightLength * Rotation.CalcOpts.BoLUp;
            BaseCost = 0f; 
        }

        public float HealingDone(float procableHealing)
        {
            return procableHealing * Rotation.CalcOpts.BoLUp * 0.5f;
        }

    }

    public class JudgementsOfThePure : Spell
    {
        public JudgementsOfThePure(Rotation rotation, float JudgementCasts)
            : base(rotation)
        {
            Duration = JudgementCasts;
            Uptime = Rotation.CalcOpts.Activity * Rotation.FightLength;
            BaseCost = HealadinConstants.basemana * 0.05f;
        }

        public override float CastTime()
        {
            return 1.5f;
        }

    }

    public class DivineIllumination : Spell
    {

        public HolyLight HL_DI { get; set; }

        public DivineIllumination(Rotation rotation)
            : base(rotation)
        {
            Duration = 180f;
            Uptime = Rotation.FightLength;
            BaseCost = 0f;
            HL_DI = new HolyLight(rotation) { DivineIllumination = true };
        }

        public float Healed()
        {
            return HL_DI.HPS() * Time();
        }

        public override float Time()
        {
            return 15f * Casts() * Rotation.CalcOpts.Activity;
        }

        public override float Usage()
        {
            return Time() * HL_DI.MPS();
        }

    }

    public class DivineFavor : Spell
    {

        public HolyLight HL_DF { get; set; }

        public DivineFavor(Rotation rotation)
            : base(rotation)
        {
            Duration = 120f;
            Uptime = Rotation.FightLength;
            BaseCost = 130f;
            HL_DF = new HolyLight(rotation) { ExtraCritChance = 1f };
        }

        public float Healed()
        {
            return HL_DF.AverageHealed() * Casts();
        }

        public override float Time()
        {
            return HL_DF.CastTime() * Casts();
        }

        public override float Usage()
        {
            return Casts() * (Cost() + HL_DF.AverageCost());
        }

    }
}
