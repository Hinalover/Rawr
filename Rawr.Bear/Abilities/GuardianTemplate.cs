using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    abstract public class GuardianTemplate
    {
        public float Count = 0;
    }

    #region Base Damage Templates
    /// <summary>
    /// Template for Mangle used by Guardians
    /// </summary>
    public class Mangle_Bear : GuardianTemplate
    {
        public AbilityGuardian_Mangle guardianAbility = new AbilityGuardian_Mangle();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Mangle(CState);
        }

        public float AverageDamage()
        {
            return guardianAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * AverageDamage();
        }

        public float AverageThreat()
        {
            return guardianAbility.GetTotalThreat();
        }

        public float ThreatDone()
        {
            return Count * AverageThreat();
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Rage
        {
            get
            {
                return guardianAbility.Rage;
            }
        }

        public float CritChance
        {
            get
            {
                return guardianAbility.CritChance;
            }
        }

        public float HitChance
        {
            get
            {
                return guardianAbility.HitChance;
            }
        }

        public float RageFromHit
        {
            get
            {
                return guardianAbility.getRageFromHit(Count);
            }
        }

        public float RageFromCrit
        {
            get
            {
                return guardianAbility.getRageFromCrit(Count);
            }
        }

        public float TotalRageGenerated
        {
            get
            {
                return guardianAbility.getTotalRageGenerated(Count);
            }
        }

        public float RagePerSecond(float time)
        {
            return TotalRageGenerated / time;
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return guardianAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for White Attacks used by Guardians
    /// </summary>
    public class White_Attack : GuardianTemplate
    {
        public AbilityGuardian_WhiteSwing guardianAbility = new AbilityGuardian_WhiteSwing();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_WhiteSwing(CState);
        }

        public float AverageDamage()
        {
            return guardianAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * AverageDamage();
        }

        public float AverageThreat()
        {
            return guardianAbility.GetTotalThreat();
        }

        public float ThreatDone()
        {
            return Count * AverageThreat();
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Rage
        {
            get
            {
                return guardianAbility.Rage;
            }
        }

        public float CritChance
        {
            get
            {
                return guardianAbility.CritChance;
            }
        }

        public float HitChance
        {
            get
            {
                return guardianAbility.HitChance;
            }
        }

        public float RageFromHit
        {
            get
            {
                return guardianAbility.getRageFromHit(Count);
            }
        }

        public float RageFromCrit
        {
            get
            {
                return guardianAbility.getRageFromCrit(Count);
            }
        }

        public float TotalRageGenerated
        {
            get
            {
                return guardianAbility.getTotalRageGenerated(Count);
            }
        }

        public float RagePerSecond(float time)
        {
            return TotalRageGenerated / time;
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return guardianAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for Enrage used by Guardians
    /// </summary>
    public class Enrage : GuardianTemplate
    {
        public AbilityGuardian_Enrage guardianAbility = new AbilityGuardian_Enrage();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Enrage(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Rage
        {
            get
            {
                return guardianAbility.Rage;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float TotalRageGenerated
        {
            get
            {
                return guardianAbility.getTotalRageGenerated(Count);
            }
        }

        public float RagePerSecond(float time)
        {
            return TotalRageGenerated / time;
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Lacerate used by Guardians
    /// </summary>
    public class Lacerate : GuardianTemplate
    {
        public AbilityGuardian_Lacerate guardianAbility = new AbilityGuardian_Lacerate();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Lacerate(CState);
        }

        public void Initialize(GuardianCombatState CState, float stacks)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Lacerate(CState, stacks);
        }

        public float AverageDamage()
        {
            return guardianAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * AverageDamage();
        }

        public float AverageThreat()
        {
            return guardianAbility.GetTotalThreat();
        }

        public float ThreatDone()
        {
            return Count * AverageThreat();
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float CritChance
        {
            get
            {
                return guardianAbility.CritChance;
            }
        }

        public float HitChance
        {
            get
            {
                return guardianAbility.HitChance;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float BaseTickCount
        {
            get
            {
                return guardianAbility.feralDoT.BaseTickCount();
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return guardianAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for Thrash used by Guardians
    /// </summary>
    public class Thrash_Bear : GuardianTemplate
    {
        public AbilityGuardian_Thrash guardianAbility = new AbilityGuardian_Thrash();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Thrash(CState);
        }

        public float AverageDamage()
        {
            return guardianAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * AverageDamage();
        }

        public float AverageThreat()
        {
            return guardianAbility.GetTotalThreat();
        }

        public float ThreatDone()
        {
            return Count * AverageThreat();
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float CritChance
        {
            get
            {
                return guardianAbility.CritChance;
            }
        }

        public float HitChance
        {
            get
            {
                return guardianAbility.HitChance;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float BaseTickCount
        {
            get
            {
                return guardianAbility.feralDoT.BaseTickCount();
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return guardianAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for Swipe used by Guardians
    /// </summary>
    public class Swipe_Bear : GuardianTemplate
    {
        public AbilityGuardian_Swipe guardianAbility = new AbilityGuardian_Swipe();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Swipe(CState);
        }

        public float AverageDamage()
        {
            return guardianAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * AverageDamage();
        }

        public float AverageThreat()
        {
            return guardianAbility.GetTotalThreat();
        }

        public float ThreatDone()
        {
            return Count * AverageThreat();
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float CritChance
        {
            get
            {
                return guardianAbility.CritChance;
            }
        }

        public float HitChance
        {
            get
            {
                return guardianAbility.HitChance;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return guardianAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for Faerie Fire used by Guardians
    /// </summary>
    public class Faerie_Fire : GuardianTemplate
    {
        public AbilityGuardian_FaerieFire guardianAbility = new AbilityGuardian_FaerieFire();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_FaerieFire(CState);
        }

        public float AverageDamage()
        {
            return guardianAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * AverageDamage();
        }

        public float AverageThreat()
        {
            return guardianAbility.GetTotalThreat();
        }

        public float ThreatDone()
        {
            return Count * AverageThreat();
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float CritChance
        {
            get
            {
                return guardianAbility.CritChance;
            }
        }

        public float HitChance
        {
            get
            {
                return guardianAbility.HitChance;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return guardianAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }
    
    /// <summary>
    /// Template for Maul used by Guardians
    /// </summary>
    public class Maul : GuardianTemplate
    {
        public AbilityGuardian_Maul guardianAbility = new AbilityGuardian_Maul();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Maul(CState);
        }

        public float AverageDamage()
        {
            return guardianAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * AverageDamage();
        }

        public float AverageThreat()
        {
            return guardianAbility.GetTotalThreat();
        }

        public float ThreatDone()
        {
            return Count * AverageThreat();
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Rage
        {
            get
            {
                return guardianAbility.Rage;
            }
        }

        public float CritChance
        {
            get
            {
                return guardianAbility.CritChance;
            }
        }

        public float HitChance
        {
            get
            {
                return guardianAbility.HitChance;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return guardianAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for Berserk used by Guardians
    /// </summary>
    public class Berserk_Bear : GuardianTemplate
    {
        public AbilityGuardian_Berserk guardianAbility = new AbilityGuardian_Berserk();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Berserk(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Incarnation used by Guardians
    /// </summary>
    public class Incarnation_Bear : GuardianTemplate
    {
        public AbilityGuardian_Incarnation guardianAbility = new AbilityGuardian_Incarnation();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Incarnation(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }
    #endregion

    #region Base Defensive Cooldowns
    /// <summary>
    /// Template for Barkskin used by Guardians
    /// </summary>
    public class Barkskin : GuardianTemplate
    {
        public AbilityGuardian_Barkskin guardianAbility = new AbilityGuardian_Barkskin();
        public float Uptime;
        public float TotalDamageReduction;

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            Uptime = 0;
            TotalDamageReduction = 0;
            guardianAbility = new AbilityGuardian_Barkskin(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float DamageReduction
        {
            get
            {
                return guardianAbility.DamageReduction;
            }
        }

        public float AverageDamageReduction
        {
            get
            {
                return Uptime * DamageReduction;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Savage Defense used by Guardians
    /// </summary>
    public class Savage_Defense : GuardianTemplate
    {
        public AbilityGuardian_SavageDefense guardianAbility = new AbilityGuardian_SavageDefense();
        public float Uptime;
        public float AverageDodge;

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_SavageDefense(CState);
            Uptime = 0;
            AverageDodge = 0;
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float Rage
        {
            get
            {
                return guardianAbility.Rage;
            }
        }

        public float getUptime(float rage, float duration)
        {
            return guardianAbility.getUptime(rage, duration);
        }

        public float getAverageDodge(float rage, float duration)
        {
            return guardianAbility.getDodgeFromAbility() * getUptime(rage, duration);
        }

        public float getTotalRageUsed(float rage, float lengthOfFight)
        {
            float fightlength = (lengthOfFight / Cooldown + 3) * Duration / lengthOfFight;
            float totalrage = 0;
            if (Uptime == fightlength)
            {
                totalrage = (Uptime * lengthOfFight * guardianAbility.Rage) / Duration;
            }
            else
                totalrage = rage;
            return totalrage;
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return string.Format("{0}*Uptime: {1}\nDodge Gained: {2}\nRage Cost: {3}\nDuration: {4}\nCharge Cooldown: {5}",
                AverageDodge.ToString("p4"), Uptime.ToString("p2"), guardianAbility.getDodgeFromAbility().ToString("p2"), 
                Rage.ToString("n0"), Duration.ToString("n0"), Cooldown.ToString("n0"));
        }
    }

    /// <summary>
    /// Template for Frenzied Regeneration used by Guardians
    /// </summary>
    public class Frenzied_Regeneration : GuardianTemplate
    {
        public AbilityGuardian_FrenziedRegen guardianAbility = new AbilityGuardian_FrenziedRegen();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_FrenziedRegen(CState);
        }

        public void Initialize(GuardianCombatState CState, float rage)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_FrenziedRegen(CState, rage);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float Rage
        {
            get
            {
                return guardianAbility.Rage;
            }
        }

        public float getTotalHealthRestore
        {
            get
            {
                return Count * guardianAbility.getAmountHealed;
            }
        }

        public float getIncreasedHealed()
        {
            return guardianAbility.getIncreaseHealed();
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for Survival Instincts used by Guardians
    /// </summary>
    public class Survival_Instincts : GuardianTemplate
    {
        public AbilityGuardian_SurvivalInstincts guardianAbility = new AbilityGuardian_SurvivalInstincts();
        public float Uptime;
        public float TotalDamageReduction;

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            Uptime = 0;
            TotalDamageReduction = 0;
            guardianAbility = new AbilityGuardian_SurvivalInstincts(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float DamageReduction
        {
            get
            {
                return guardianAbility.DamageReduction;
            }
        }

        public float AverageDamageReduction
        {
            get
            {
                return DamageReduction * Uptime;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Might of Ursoc used by Guardians
    /// </summary>
    public class Might_of_Ursoc : GuardianTemplate
    {
        public AbilityGuardian_MightofUrsoc guardianAbility = new AbilityGuardian_MightofUrsoc();
        public float Uptime;

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            Uptime = 0;
            guardianAbility = new AbilityGuardian_MightofUrsoc(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float IncreasedMaximumHealth
        {
            get
            {
                return guardianAbility.IncreasedMaximumHealth;
            }
        }

        public float AverageIncreasedHealth
        {
            get
            {
                return IncreasedMaximumHealth * Uptime;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Leader Of the Pack used by Guardians
    /// </summary>
    public class Leader_Of_The_Pack : GuardianTemplate
    {
        public AbilityGuardian_LeaderOfThePack guardianAbility = new AbilityGuardian_LeaderOfThePack();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_LeaderOfThePack(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float BaseCritInterval
        {
            get { return guardianAbility.BaseCritInterval; }
            set { guardianAbility.BaseCritInterval = value; }
        }

        public float CritInterval
        {
            get
            {
                return guardianAbility.CritInterval;
            }
        }

        public float getTotalHealthRestore
        {
            get
            {
                return guardianAbility.getTotalHealthRestored * Count;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }
    #endregion

    #region Symbiosis Damaging Abilities
    /// <summary>
    /// Template for Consecration used by Guardians
    /// </summary>
    public class Consecration : GuardianTemplate
    {
        public AbilityGuardian_Consecration guardianAbility = new AbilityGuardian_Consecration();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Consecration(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float AverageDamage()
        {
            return guardianAbility.GetTotalThreat();
        }

        public float DamageDone()
        {
            return Count * AverageDamage();
        }

        public float AverageThreat()
        {
            return guardianAbility.TotalThreat;
        }

        public float ThreatDone()
        {
            return Count * AverageThreat();
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Lightning Shield used by Guardians
    /// </summary>
    public class Lightning_Shield : GuardianTemplate
    {
        public AbilityGuardian_LightningShield guardianAbility = new AbilityGuardian_LightningShield();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_LightningShield(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float AverageDamage()
        {
            return guardianAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * AverageDamage();
        }

        public float AverageThreat()
        {
            return guardianAbility.GetTotalThreat();
        }

        public float ThreatDone()
        {
            return Count * AverageThreat();
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Life Tap used by Guardians
    /// </summary>
    public class Life_Tap : GuardianTemplate
    {
        public AbilityGuardian_LifeTap guardianAbility = new AbilityGuardian_LifeTap();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_LifeTap(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float TotalRageGenerated
        {
            get
            {
                return guardianAbility.getTotalRageGenerated(Count);
            }
        }

        public float RagePerSecond(float time)
        {
            return TotalRageGenerated / time;
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }
    #endregion

    #region Symbiosis Defensive Cooldowns
    /// <summary>
    /// Template for Bone Shield used by Guardians
    /// </summary>
    public class Bone_Shield : GuardianTemplate
    {
        public AbilityGuardian_BoneShield guardianAbility = new AbilityGuardian_BoneShield();
        public float Uptime;
        public float TotalDamageReduction;

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            Uptime = 0;
            TotalDamageReduction = 0;
            guardianAbility = new AbilityGuardian_BoneShield(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float DamageReduction
        {
            get
            {
                return guardianAbility.DamageReduction;
            }
        }

        public float AverageDamageReduction
        {
            get
            {
                return Uptime * DamageReduction;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Elusive Brew used by Guardians
    /// </summary>
    public class Elusive_Brew : GuardianTemplate
    {
        public AbilityGuardian_ElusiveBrew guardianAbility = new AbilityGuardian_ElusiveBrew();
        public float Uptime;

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            Uptime = 0;
            guardianAbility = new AbilityGuardian_ElusiveBrew(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float Duration
        {
            get
            {
                return guardianAbility.Duration;
            }
        }

        public float AverageDodge
        {
            get
            {
                return Uptime * guardianAbility.IncreasesDodge;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Feint used by Guardians
    /// </summary>
    public class Feint : GuardianTemplate
    {
        public AbilityGuardian_Feint guardianAbility = new AbilityGuardian_Feint();
        public float Uptime;

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            Uptime = 0;
            guardianAbility = new AbilityGuardian_Feint(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float AoEDamageReduction
        {
            get
            {
                return guardianAbility.AoEDamageReduction;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }
    }
    #endregion

    #region Healing Talents
    /// <summary>
    /// Template for Renewal used by Guardians
    /// </summary>
    public class Renewal : GuardianTemplate
    {
        public AbilityGuardian_Renewal guardianAbility = new AbilityGuardian_Renewal();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_Renewal(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float getTotalHealthRestore
        {
            get
            {
                return Count * guardianAbility.HealingFormula;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for Cenarion Ward used by Guardians
    /// </summary>
    public class Cenarion_Ward : GuardianTemplate
    {
        public AbilityGuardian_CenarionWard guardianAbility = new AbilityGuardian_CenarionWard();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_CenarionWard(CState);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float getTotalHealthRestore
        {
            get
            {
                return Count * guardianAbility.HealingFormula;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for Healing Touch without Nature's Swiftness used by Guardians
    /// </summary>
    public class Healing_Touch_wo_NS : GuardianTemplate
    {
        public AbilityGuardian_HealingTouch guardianAbility = new AbilityGuardian_HealingTouch();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_HealingTouch(CState, false);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float getTotalHealthRestore
        {
            get
            {
                return Count * guardianAbility.HealingFormula;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }

    /// <summary>
    /// Template for Healing Touch with Nature's Swiftness used by Guardians
    /// </summary>
    public class Healing_Touch_w_NS : GuardianTemplate
    {
        public AbilityGuardian_HealingTouch guardianAbility = new AbilityGuardian_HealingTouch();

        public void Initialize(GuardianCombatState CState)
        {
            Count = 0;
            guardianAbility = new AbilityGuardian_HealingTouch(CState, true);
        }

        public string Name
        {
            get
            {
                return guardianAbility.Name;
            }
        }

        public float Cooldown
        {
            get
            {
                return guardianAbility.Cooldown;
            }
        }

        public float getTotalHealthRestore
        {
            get
            {
                return Count * guardianAbility.HealingFormula;
            }
        }

        public void UpdateCombatState(GuardianCombatState cState)
        {
            guardianAbility.UpdateCombatState(cState);
        }

        public override string ToString()
        {
            return guardianAbility.ToString();
        }
    }
    #endregion
}
