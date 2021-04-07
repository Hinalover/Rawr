using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class GuardianCombatState
    {
        public Character Char = new Character();
        public int CharacterLevel = 90;
        public int TargetLevel = 93;
        public StatsBear Stats = new StatsBear();
        public DruidTalents Talents = new DruidTalents();
        public GaurdianWeapon MainHand = new GaurdianWeapon();
        public float NumberOfTargets = 1f;
        /// <summary>
        /// Is the player attacking from behind
        /// </summary>
        public bool AttackingFromBehind = false;
        /// <summary>
        /// Uptime for Enrage which provides 30 total Rage.
        /// </summary>
        public float EnrageUptime = 0;
        /// <summary>
        /// Uptime for Berserk which removes the cooldown from Mangle and causes it to hit up to 3 targets
        /// </summary>
        public float BerserkUptime = 0;
        /// <summary>
        /// Uptime for Incarnation which reduces the cooldown on all melee damage abilities and Growl to 1.5 sec.
        /// </summary>
        public float IncarnationUptime = 0;
        /// <summary>
        /// Uptime for Force of Nature which summons 3 treants to assist the player
        /// </summary>
        public float ForceofNatureUptime = 0;
        /// <summary>
        /// Number of Bleeds on the target
        /// </summary>
        public float NumberOfBleeds = 0;

        public FeralRotationType Spec = FeralRotationType.Guardian;
        public float BossArmor = BaseCombatRating.Get_BossArmor(93);
        /// <summary>
        /// If the player wises to use PTR numbers instead of live numbers.
        /// </summary>
        public bool PTR = false;

        public float AttackPower
        {
            get
            {
                return MainHand.AttackPower;
            }
        }

        public float VengenceAttackPower
        {
            get
            {
                return MainHand.VengenceAttackPower;
            }
            set
            {
                MainHand.VengenceAttackPower = (float)value;
            }
        }

        #region Weapon Damage
        #region Zero Vengence
        public float MinDamage
        {
            get
            {
                return MainHand.MinDamage;
            }
        }

        public float MaxDamage
        {
            get
            {
                return MainHand.MaxDamage;
            }
        }

        public float MinimumDPS
        {
            get
            {
                return MainHand.MinimumDPS;
            }
        }

        public float MaximumDPS
        {
            get
            {
                return MainHand.MaximumDPS;
            }
        }

        public float DPS
        {
            get
            {
                return MainHand.DPS;
            }
        }

        public float WeaponDamage
        {
            get
            {
                return MainHand.WeaponDamage;
            }
        }
        #endregion

        #region Average Vengence
        public float VengenceMinDamage
        {
            get
            {
                return MainHand.VengenceMinDamage;
            }
        }

        public float VengenceMaxDamage
        {
            get
            {
                return MainHand.VengenceMaxDamage;
            }
        }

        public float VengenceMinimumDPS
        {
            get
            {
                return MainHand.VengenceMinimumDPS;
            }
        }

        public float VengenceMaximumDPS
        {
            get
            {
                return MainHand.VengenceMaximumDPS;
            }
        }

        public float VengenceDPS
        {
            get
            {
                return MainHand.VengenceDPS;
            }
        }

        public float VengenceWeaponDamage
        {
            get
            {
                return MainHand.VengenceWeaponDamage;
            }
        }
        #endregion
        #endregion

        public void ResetCombatState()
        {
            AttackingFromBehind = true;
            EnrageUptime = 0;
            BerserkUptime = 0;
            Spec = FeralRotationType.Guardian;
            BossArmor = BaseCombatRating.Get_BossArmor(93);
            PTR = false;
        }
    }
}
