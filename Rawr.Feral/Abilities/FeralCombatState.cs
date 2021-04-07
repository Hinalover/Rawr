using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class FeralCombatState
    {
        public Character Char;
        public StatsFeral Stats;
        public DruidTalents Talents;
        public FeralWeapon MainHand;
        public float NumberOfTargets = 1f;
        public int CurrentComboPoints;
        public int CurrentRage;
        public int CurrentEnergy;
        public float CurrentMana;
        /// <summary>
        /// Is the player attacking from behind
        /// </summary>
        public bool AttackingFromBehind;
        /// <summary>
        /// If player is Prowling
        /// </summary>
        public bool Prowling;
        /// <summary>
        /// While true, White Damage Attacks are increased by 80%
        /// </summary>
        public bool SavageRoarUptime;
        /// <summary>
        /// While true, damage is increased by 15%
        /// </summary>
        public bool TigersFuryUptime;
        /// <summary>
        /// While true, Energy cost is reduced by 50%
        /// </summary>
        public bool BerserkUptime;
        public float NumberOfBleeds;

        public bool Above80Percent;
        public bool Below60Percent;
        public bool Below25Percent;

        public FeralRotationType Spec;
        public float BossArmor;

        public void ResetCombatState()
        {
            CurrentRage = 100;
            CurrentEnergy = 100;
            CurrentMana = Stats.Mana;
            CurrentComboPoints = 0;
        }
    }
}
