using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin {
    public class AbilityModel {
        private Ability Ability;
        private Character Character;
        private StatsProtPaladin Stats;
        private PaladinTalents Talents;
        private CalculationOptionsProtPaladin CalcOpts;
        private BossOptions BossOpts;

        public readonly AttackTable AttackTable;

        public string Name { get; private set; }
        public float Damage { get; private set; }
        public float Threat { get; private set; }
        public float ArmorReduction { get; private set; }
        public float CritPercentage { get { return AttackTable.Critical; } }

        private void CalculateDamage() {
            float baseDamage = 0.0f;
            float critMultiplier = 0.0f;
            float duration = 0.0f;
            float AP = Stats.AttackPower + Stats.AverageVengeanceAP;
            float SP = Stats.SpellPower + (float)Math.Floor(0.5f * Stats.AverageVengeanceAP);

            int targetLevel = BossOpts.Level;

            #region Ability Base Damage
            switch (Ability) {

                #region Spells

                case Ability.AvengersShield:
                    if (Character.OffHand == null || Character.OffHand.Type != ItemType.Shield) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = 4488f + (0.545f * AP) + (0.21f * SP);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfWrath:
                    baseDamage = 1838f + (1.61f * SP);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HolyWrath:
                    baseDamage = 4300.5f + (0.61f * SP);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfTheRighteousProc:
                    if (Character.MainHand == null || (Character.MainHand.Type != ItemType.OneHandAxe && Character.MainHand.Type != ItemType.OneHandMace && Character.MainHand.Type != ItemType.OneHandSword))
                    {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = 1050f + 0.234f * AP;

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.Judgment:
                    baseDamage = 702f + (0.397f * AP) + (0.635f * SP);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;

                #endregion

                #region Melee

                case Ability.CrusaderStrike:
                    if (Character.MainHand == null) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = Lookup.WeaponDamage(Character, AP, true) * 1.6f + 1123f;
                    baseDamage *= (1.0f + Stats.BonusPhysicalDamageMultiplier)
                                * (1.0f + Stats.BonusDamageMultiplierCrusaderStrike)
                                * (1.0f - ArmorReduction);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfTheRighteous:
                    if (Character.MainHand == null || (Character.MainHand.Type != ItemType.OneHandAxe && Character.MainHand.Type != ItemType.OneHandMace && Character.MainHand.Type != ItemType.OneHandSword)) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = Lookup.WeaponDamage(Character, AP, true) * 0.2f;

                    baseDamage *= (1.0f + Stats.BonusPhysicalDamageMultiplier)
                                * (1.0f - ArmorReduction);

                    critMultiplier = 1.0f;
                    break;
                case Ability.MeleeSwing:
                    baseDamage = Lookup.WeaponDamage(Character, AP, false);

                    baseDamage *= (1.0f + Stats.BonusPhysicalDamageMultiplier)
                                * (1.0f - (Lookup.GlancingReduction(Character.Level, targetLevel) * AttackTable.Glance))
                                * (1.0f - ArmorReduction);

                    baseDamage *= (1f + Stats.BonusWhiteDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.SealOfRighteousness:
                    baseDamage = 0.05f * Lookup.WeaponDamage(Character, AP, false);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.SealOfTruth:
                    {
                        baseDamage = Lookup.WeaponDamage(Character, AP, false) * 0.14f;

                        baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                        critMultiplier = 1.0f;
                        break;
                    }
                case Ability.ShieldOfTheRighteous:
                    if (Character.OffHand == null || Character.OffHand.Type != ItemType.Shield) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = 4063f + (0.6f * AP);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier)
                                * (1.0f + Stats.BonusDamageShieldofRighteous);

                    critMultiplier = 1.0f;
                    break;

                #endregion

                #region DoTs

                case Ability.CensureTick:
                    {
                        float censureStacks = 5;

                        baseDamage = 158f + (0.138f * SP) * censureStacks;

                        baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                        critMultiplier = 1.0f;
                        break;
                    }
                case Ability.Consecration:
                    baseDamage = 8 * (0.04f * SP + 0.04f * AP);

                    duration = 9f;

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;

                #endregion

            }

            baseDamage *= (1.0f + Stats.BonusDamageMultiplier);
            
            #endregion
            
            #region Miss Chance, Avoidance Chance
            if (Lookup.IsSpell(Ability))
            {
                if (Ability == Ability.Consecration)
                    // Probability calculation, since each tick can be resisted individually.
                    baseDamage = Lookup.GetConsecrationTickChances(duration, baseDamage, AttackTable.Miss);
                else if (Lookup.IsAvoidable(Ability))
                    // Missed spell attacks
                    baseDamage *= (1.0f - AttackTable.Miss);
            } else {
                // Avoidable attacks
                if (Lookup.IsAvoidable(Ability))
                    baseDamage *= (1.0f - AttackTable.AnyMiss);
            }
            #endregion

            // Average critical strike bonuses
            if (Lookup.CanCrit(Ability))
                baseDamage += baseDamage * critMultiplier * AttackTable.Critical;

            // Final Damage the Ability deals
            Damage = baseDamage;
        }

        private void CalculateThreat() {
            // With Righteous Fury, threat is Damage plus 300%, or quadruple the damage.
            Threat = Damage * 4f;
        }

        public AbilityModel(Character character, StatsProtPaladin stats, Ability ability, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
        {
            Character   = character;
            Stats       = stats;
            Ability     = ability;
            CalcOpts    = calcOpts;
            BossOpts    = bossOpts;

            Talents     = Character.PaladinTalents;
            AttackTable = new AttackTable(character, stats, ability, CalcOpts, BossOpts);

            if (!Lookup.IsSpell(Ability))
                ArmorReduction = StatConversion.GetArmorDamageReduction(character.Level, bossOpts.Level, bossOpts.Armor, Stats.TargetArmorReduction, 0);

            Name        = Lookup.Name(Ability);

            CalculateDamage();
            CalculateThreat();
        }
    }

    public class AbilityModelList : Dictionary<Ability, AbilityModel>
    {
        public void Add(Ability ability, Character character, StatsProtPaladin stats, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
        {
            this.Add(ability, new AbilityModel(character, stats, ability, calcOpts, bossOpts));
        }
    }
}
