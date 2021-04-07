using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    abstract public class FeralTemplate
    {
        public float Count = 0;
        public float nonIncarnationCount = 0;
        public float IncarnationCount = 0;
    }

    /// <summary>
    /// Template for Shred
    /// </summary>
    public class Shred : FeralTemplate
    {
        public AbilityFeral_Shred feralAbility = new AbilityFeral_Shred();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_Shred(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Mangle used by Cats
    /// </summary>
    public class Mangle_Cat : FeralTemplate
    {
        public AbilityFeral_MangleCat feralAbility =  new AbilityFeral_MangleCat();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_MangleCat(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Claw
    /// </summary>
    public class Claw : FeralTemplate
    {
        public AbilityFeral_Claw feralAbility = new AbilityFeral_Claw();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_Claw(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public string byAbility(float count, float percent, float total)
        {
            return feralAbility.byAbility(count, percent, total);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Rake
    /// </summary>
    public class Rake : FeralTemplate
    {
        public AbilityFeral_Rake feralAbility = new AbilityFeral_Rake();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_Rake(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float Duration
        {
            get
            {
                return feralAbility.Duration;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public float BaseTickCount
        {
            get
            {
                return feralAbility.feralDoT.BaseTickCount();
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Ravage
    /// </summary>
    public class Ravage : FeralTemplate
    {
        public AbilityFeral_Ravage feralAbility = new AbilityFeral_Ravage();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_Ravage(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Ravage when procced from Stampede
    /// </summary>
    public class Ravage_Proc : FeralTemplate
    {
        public AbilityFeral_RavageProc feralAbility = new AbilityFeral_RavageProc();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_RavageProc(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Pounce
    /// </summary>
    public class Pounce : FeralTemplate
    {
        public AbilityFeral_Pounce feralAbility = new AbilityFeral_Pounce();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_Pounce(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float Duration
        {
            get
            {
                return feralAbility.Duration;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public float BaseTickCount
        {
            get
            {
                return feralAbility.feralDoT.BaseTickCount();
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Swipe used by cats
    /// </summary>
    public class Swipe_Cat : FeralTemplate
    {
        public AbilityFeral_SwipeCat feralAbility = new AbilityFeral_SwipeCat();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_SwipeCat(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Thrash used by cats
    /// </summary>
    public class Thrash_Cat : FeralTemplate
    {
        public AbilityFeral_ThrashCat feralAbility = new AbilityFeral_ThrashCat();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_ThrashCat(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float Duration
        {
            get
            {
                return feralAbility.Duration;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public float BaseTickCount
        {
            get
            {
                return feralAbility.feralDoT.BaseTickCount();
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for White Attacks
    /// </summary>
    public class White_Attack : FeralTemplate
    {
        public AbilityFeral_WhiteSwing feralAbility = new AbilityFeral_WhiteSwing();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_WhiteSwing(CState);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Savage Roar using Combo Points
    /// </summary>
    public class Savage_Roar : FeralTemplate
    {
        public AbilityFeral_SavageRoar feralAbility = new AbilityFeral_SavageRoar();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_SavageRoar(CState, 1);
        }
        public void Initialize(FeralCombatState CState, float CP)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_SavageRoar(CState, CP);
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Duration
        {
            get
            {
                return feralAbility.Duration;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float FormulaComboPoint
        {
            get
            {
                return feralAbility.Formula_CP;
            }
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Rip using Combo Points
    /// </summary>
    public class Rip : FeralTemplate
    {
        public float BloodInTheWaterCount;
        public float nonIncarnationBitWCount;
        public float IncarnationBitWCount;
        public AbilityFeral_Rip feralAbility = new AbilityFeral_Rip();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            BloodInTheWaterCount = 0;
            nonIncarnationCount = 0;
            nonIncarnationBitWCount = 0;
            IncarnationCount = 0;
            IncarnationBitWCount = 0;
            feralAbility = new AbilityFeral_Rip(CState, 5);
        }
        public void Initialize(FeralCombatState CState, float CP)
        {
            Count = 0;
            BloodInTheWaterCount = 0;
            nonIncarnationCount = 0;
            nonIncarnationBitWCount = 0;
            IncarnationCount = 0;
            IncarnationBitWCount = 0;
            feralAbility = new AbilityFeral_Rip(CState, CP);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return (Count + BloodInTheWaterCount) * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
            BloodInTheWaterCount = (nonIncarnationBitWCount * (1 - incarnationUptime)) + (IncarnationBitWCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float Duration
        {
            get
            {
                return feralAbility.Duration;
            }
        }

        public float FormulaComboPoint
        {
            get
            {
                return feralAbility.Formula_CP;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public float BaseTickCount
        {
            get
            {
                return feralAbility.feralDoT.BaseTickCount();
            }
        }

        public string byAbility(float count, float bloodintheWater, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, bloodintheWater, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for non-Feral Rip using Combo Points
    /// </summary>
    public class Rip_Guardian : FeralTemplate
    {
        public float BloodInTheWaterCount;
        public float nonIncarnationBitWCount;
        public float IncarnationBitWCount;
        public AbilityGuardian_Rip feralAbility = new AbilityGuardian_Rip();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            BloodInTheWaterCount = 0;
            nonIncarnationCount = 0;
            nonIncarnationBitWCount = 0;
            IncarnationCount = 0;
            IncarnationBitWCount = 0;
            feralAbility = new AbilityGuardian_Rip(CState, 5);
        }
        public void Initialize(FeralCombatState CState, float CP)
        {
            Count = 0;
            BloodInTheWaterCount = 0;
            nonIncarnationCount = 0;
            nonIncarnationBitWCount = 0;
            IncarnationCount = 0;
            IncarnationBitWCount = 0;
            feralAbility = new AbilityGuardian_Rip(CState, CP);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return (Count + BloodInTheWaterCount) * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
            BloodInTheWaterCount = (nonIncarnationBitWCount * (1 - incarnationUptime)) + (IncarnationBitWCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float Duration
        {
            get
            {
                return feralAbility.Duration;
            }
        }

        public float FormulaComboPoint
        {
            get
            {
                return feralAbility.Formula_CP;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public float BaseTickCount
        {
            get
            {
                return feralAbility.feralDoT.BaseTickCount();
            }
        }

        public string byAbility(float count, float bloodintheWater, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, bloodintheWater, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Ferocious Bite using Combo Points and extra energy
    /// </summary>
    public class Ferocious_Bite : FeralTemplate
    {
        public AbilityFeral_FerociousBite feralAbility = new AbilityFeral_FerociousBite();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_FerociousBite(CState, 1, 0);
        }
        public void Initialize(FeralCombatState CState, float CP)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_FerociousBite(CState, CP, 35);
        }
        public void Initialize(FeralCombatState CState, float CP, float energy)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_FerociousBite(CState, CP, energy);
        }

        public float AverageDamage()
        {
            return feralAbility.TotalDamage;
        }

        public float DamageDone()
        {
            return Count * feralAbility.TotalDamage;
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy + FormulaEnergy;
            }
        }

        public float FormulaEnergy
        {
            get
            {
                return feralAbility.Formula_Energy;
            }
        }

        public float FormulaComboPoint
        {
            get
            {
                return feralAbility.Formula_CP;
            }
        }

        public float CritChance
        {
            get
            {
                return feralAbility.CritChance;
            }
        }

        public string byAbility(float count, float percent, float total, float damageDone)
        {
            return feralAbility.byAbility(count, percent, total, damageDone);
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Tiger's Fury
    /// </summary>
    public class Tigers_Fury : FeralTemplate
    {
        public AbilityFeral_TigersFury feralAbility = new AbilityFeral_TigersFury();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_TigersFury(CState);
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Energy
        {
            get
            {
                return feralAbility.Energy;
            }
        }

        public float Duration
        {
            get
            {
                return feralAbility.Duration;
            }
        }

        public float Cooldown
        {
            get
            {
                return feralAbility.Cooldown;
            }
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Berserk
    /// </summary>
    public class Berserk : FeralTemplate
    {
        public AbilityFeral_Berserk feralAbility = new AbilityFeral_Berserk();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            feralAbility = new AbilityFeral_Berserk(CState);
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Duration
        {
            get
            {
                return feralAbility.Duration;
            }
        }

        public float Cooldown
        {
            get
            {
                return feralAbility.Cooldown;
            }
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }

    /// <summary>
    /// Template for Incarnation
    /// </summary>
    public class Incarnation : FeralTemplate
    {
        public AbilityFeral_Incarnation feralAbility = new AbilityFeral_Incarnation();

        public void Initialize(FeralCombatState CState)
        {
            Count = 0;
            nonIncarnationCount = 0;
            IncarnationCount = 0;
            feralAbility = new AbilityFeral_Incarnation(CState);
        }

        public void AverageIncarnationCount(float incarnationUptime)
        {
            Count = (nonIncarnationCount * (1 - incarnationUptime)) + (IncarnationCount * incarnationUptime);
        }

        public string Name
        {
            get
            {
                return feralAbility.Name;
            }
        }

        public float Duration
        {
            get
            {
                return feralAbility.Duration;
            }
        }

        public float Cooldown
        {
            get
            {
                return feralAbility.Cooldown;
            }
        }

        public void UpdateCombatState(FeralCombatState cState)
        {
            feralAbility.UpdateCombatState(cState);
        }
    }
}