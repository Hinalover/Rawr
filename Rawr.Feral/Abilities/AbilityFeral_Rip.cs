using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    class AbilityFeral_Rip : AbilityFeral_Base
    {
        /// <summary>
        /// Finishing move that causes Bleed damage over time.  Damage increases per combo point and by your attack power:
        /// 1 point:  (((56 + 161 * 1) + (0.0207*CP * AP)) * 8)  damage over 16 sec.
        /// 2 points: (((56 + 161 * 2) + (0.0207*CP * AP)) * 8)  damage over 16 sec.
        /// 3 points: (((56 + 161 * 3) + (0.0207*CP * AP)) * 8)  damage over 16 sec.
        /// 4 points: (((56 + 161 * 4) + (0.0207*CP * AP)) * 8)  damage over 16 sec.
        /// 5 points: (((56 + 161 * 5) + (0.0207*CP * AP)) * 8)  damage over 16 sec.
        /// </summary>
        public AbilityFeral_Rip(FeralCombatState CState)
        {
            CombatState = CState;
            Name = "Rip";
            SpellID = 1079;
            SpellIcon = "ability_ghoulfrenzy";
            druidForm = new DruidForm[]{ DruidForm.Cat };

            Energy = 30 * (CombatState.BerserkUptime ? 0.5f : 1f);
            ComboPoint = -1;

            DamageType = ItemDamageType.Physical;
            BaseDamage = 56;
            isDoT = true;
            feralDoT.Interval = 2f * 1000f;
            feralDoT.BaseLength = 16f * 1000f;
            // Assume the Glyph of Bloodletting adds the six extra seconds
            feralDoT.TotalLength = feralDoT.BaseLength + (CombatState.Talents.GlyphOfBloodletting ? (6f * 1000f) : 0);
            Duration = feralDoT.TotalLength;

            Formula_CP_Base_Damage_Modifier = 161f;
            Formula_CP_AP_Modifier = 0.0207f;

            TriggersGCD = true;
            CastTime = 0;
            Cooldown = MIN_GCD_MS;
            AbilityIndex = (int)FeralAbility.Rip;
            Range = MELEE_RANGE;
            UpdateCombatState(CombatState);
        }

        public override void UpdateCombatState(FeralCombatState CState)
        {
            base.UpdateCombatState(CState);
            EnergyRefunded();
            CombatState.NumberOfBleeds += 1;
            if (Formula_CP != 0)
            {
                // Combo Points cannot be over 5 Combo Points
                if (Formula_CP > 5)
                    Formula_CP = 5;
                this.ComboPoint = -Formula_CP;
            }
            this.MainHand = CState.MainHand;
        }

        private float _DamageMultiplierModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        public override float DamageMultiplierModifer
        {
            get
            {
                if (_DamageMultiplierModifer == 0)
                {
                    _DamageMultiplierModifer = (1 + CombatState.Stats.BonusDamageMultiplier)
                                             * (1 + CombatState.Stats.BonusPhysicalDamageMultiplier)
                                             * (1f - StatConversion.GetArmorDamageReduction(CombatState.Char.Level, CombatState.BossArmor, CombatState.Stats.TargetArmorReduction, CombatState.Stats.ArmorPenetration))
                                             * (1 + (CombatState.TigersFuryUptime ? 0.15f : 0f))
                                             * (1 + CombatState.Stats.NonShredBleedDamageMultiplier) // Mastery Multiplier
                                             * (1 + (CombatState.Talents.GlyphOfRip ? 0.15f : 0)); 
                }
                return _DamageMultiplierModifer;
            }
            set
            {
                _DamageMultiplierModifer = value;
            }
        }

        public override float Formula()
        {
            if (Formula_CP == 0)
                return 0f;
            else
                return ((BaseDamage + (Formula_CP_Base_Damage_Modifier * Formula_CP)) + ((Formula_CP_AP_Modifier * Formula_CP) * CombatState.Stats.AttackPower));
        }
    }
}
