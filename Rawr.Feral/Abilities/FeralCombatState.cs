using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class FeralCombatState
    {
        public Character Char = new Character();
        public int CharacterLevel = 90;
        public int TargetLevel = 93;
        public StatsFeral Stats = new StatsFeral();
        public DruidTalents Talents = new DruidTalents();
        public FeralWeapon MainHand = new FeralWeapon();
        public float NumberOfTargets = 1f;
        /// <summary>
        /// Is the player attacking from behind
        /// </summary>
        public bool AttackingFromBehind = true;
        /// <summary>
        /// If player is Prowling
        /// </summary>
        public bool Prowling = false;
        /// <summary>
        /// Uptime for Savage Roar, which ncreases physical damage done by 30%
        /// </summary>
        public float SavageRoarUptime = 0;
        /// <summary>
        /// While true, damage is increased by 15%
        /// </summary>
        public float TigersFuryUptime = 0;
        /// <summary>
        /// Uptime for Berserk which reduces Energy cost by 50%
        /// </summary>
        public float BerserkUptime = 0;
        /// <summary>
        /// Uptime for Incarnation which allows for Ravage to be used outside stealth
        /// </summary>
        public float IncarnationUptime = 0;
        /// <summary>
        /// Uptime for Force of Nature which summons 3 treants to assist the player
        /// </summary>

        public float ForceofNatureUptime = 0;
        public float NumberOfBleeds = 0;

        public readonly float Above80Percent = 0.17f; //TODO: Make this dynamic/customizable
        public readonly float Below25Percent = 0.27f; //TODO: Make this dynamic/customizable
        public readonly float Below60Percent = 0.675f; //TODO: Make this dynamic/customizable


        public FeralRotationType Spec = FeralRotationType.Feral;
        public float BossArmor = BaseCombatRating.Get_BossArmor(93);
        /// <summary>
        /// If the player wises to use PTR numbers instead of live numbers.
        /// </summary>
        public bool PTR = false;

        public void ResetCombatState()
        {
            AttackingFromBehind = true;
            Prowling = false;
            SavageRoarUptime = 0;
            TigersFuryUptime = 0;
            BerserkUptime = 0;
            Spec = FeralRotationType.Feral;
            BossArmor = BaseCombatRating.Get_BossArmor(93);
            PTR = false;
        }
    }
}
