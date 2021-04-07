using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Unholy Blight Ability based on the AbilityDK_Base class.
    /// Unholy Blight
    /// Instant	1.5 min cooldown
    /// Surrounds the Death Knight with a vile swarm of unholy insects for 10 sec, stinging all enemies within 10 yards every 1 sec, infecting them with Blood Plague and Frost Fever.
    /// </summary>
    class AbilityDK_UnholyBlight : AbilityDK_Base
    {
        public AbilityDK_UnholyBlight(CombatState CS)
        {
            this.CState = CS;
            this.szName = "UnholyBlight";
            this.bWeaponRequired = false;
            this.uRange = 0;
            this.uArea = 10;
            this.bTriggersGCD = true;
            this.uDuration = 10 * 1000; // 10 sec duration.
            this.Cooldown = 60 * 1500; // 1.5 min CD.
            this.uTickRate = 1000;
            this.ml_TriggeredAbility = new AbilityDK_Base[2];
            UpdateCombatState(CS);
            AbilityIndex = (int)DKability.UnholyBlight;
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.ml_TriggeredAbility[0] = new AbilityDK_BloodPlague(CS);
            this.ml_TriggeredAbility[1] = new AbilityDK_FrostFever(CS);
        }
    }
}
