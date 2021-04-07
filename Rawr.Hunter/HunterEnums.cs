using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    public enum Specialization
    {
        BeastMastery,
        Marksmanship,
        Survival
    }

    public enum Aspect
    {
        Hawk,
        Fox,
        None
    }

    public enum ShotRotation
    {
        AutoShotOnly,
        OneToOne,
        ThreeToTwo
    }

    public enum CDStates_Enum
    {
        FOCUS_LT50 = 0,
        CD_KILLSHOT = 1,
        CD_RAPIDFIRE = 2,
        CD_KILLCOMMAND = 3,
        FOCUSFIRE = 4,
        BM_UNKNOWN = 5,
        UP_IMPSTEADYSHOT = 6,
        PROC_MMM = 7,
        CD_CHIMERASHOT = 8,
        CD_READINESS = 9,
        EXPLOSIVESHOT = 10,
        PROC_LOCKNLOAD = 11,
        CD_BLACKARROW = 12,
    }

    [Flags]
    public enum CDStates_Flags
    {
        NONE = 0,
        /// <summary>
        /// Focus drops below 50.
        /// </summary>
        FOCUS_LT50 = 1 << CDStates_Enum.FOCUS_LT50,
        /// <summary>
        /// KS available off CD.
        /// </summary>
        CD_KILLSHOT = 1 << CDStates_Enum.CD_KILLSHOT,
        /// <summary>
        /// RapidFire available off CD.
        /// </summary>
        CD_RAPIDFIRE = 1 << CDStates_Enum.CD_RAPIDFIRE,
        /// <summary>
        /// Kill Command available off CD.
        /// </summary>
        CD_KILLCOMMAND = 1 << CDStates_Enum.CD_KILLCOMMAND,
        FOCUSFIRE = 1 << CDStates_Enum.FOCUSFIRE,
        BM_UNKNOWN = 1 << CDStates_Enum.BM_UNKNOWN,
        /// <summary>
        /// Buff Imp SteadShot drops off.
        /// </summary>
        UP_IMPSTEADYSHOT = 1 << CDStates_Enum.UP_IMPSTEADYSHOT,
        /// <summary>
        /// MMM procs off the shot.
        /// </summary>
        PROC_MMM = 1 << CDStates_Enum.PROC_MMM,
        /// <summary>
        /// Chimera available off CD.
        /// </summary>
        CD_CHIMERASHOT = 1 << CDStates_Enum.CD_CHIMERASHOT,
        CD_READINESS = 1 << CDStates_Enum.CD_READINESS,
        EXPLOSIVESHOT = 1 << CDStates_Enum.EXPLOSIVESHOT,
        PROC_LOCKNLOAD = 1 << CDStates_Enum.PROC_LOCKNLOAD,
        /// <summary>
        /// BlackArrow available off CD.
        /// </summary>
        CD_BLACKARROW = 1 << CDStates_Enum.CD_BLACKARROW,
    }

    // Updated 4.2 from Wowhead talents, Shots & Abilities.
    public enum Shots
    {
        None,
        AimedShot, // MM
        AimedShot_MMM, // MM
        AimedShot_CA, // MM
        ArcaneShot, //
        BestialWrath, //
        BlackArrow, //
        ChimeraShot, //
        CobraShot, //
        ConcussiveShot, //
        ExplosiveShot, // SV
        ExplosiveTrap, //
        Fervor, //
        FocusFire, // 
        FreezingTrap, //
        DistractingShot, //
        IceTrap, //
        ImmolationTrap, //
        Intimidation, //
        KillCommand, // 
        KillShot, //
        LockNLoad,
        MultiShot, //
        RapidFire, //
        Readiness, //
        ScatterShot, //
        SerpentSting, //
        SilencingShot, //
        SnakeTrap, //
        SteadyShot, //
        TranquilizingShot, //
    }

    // TODO: Setup map to go with Petfamily & PetAttack
    public enum PetAttacks
    {
        AcidSpit,
        BadAttitude,
        BadManner,
        Bite,
        Bullheaded,
        CallOfTheWild,
        Charge,
        Claw,
        //Cornered,
        Cower,
        Dash,
        Dive,
        DemoralizingScreech,
        DustCloud,
        EmbraceoftheShaleSpider,
        FireBreath,
        FroststormBreath,
        FuriousHowl,
        Gore,
        Growl,
        HardenCarapace,
        LastStand,
        LavaBreath,
        LickYourWounds,
        LightningBreath,
        LockJaw,
        MonstrousBite,
        NetherShock,
        None,
        Pin,
        PoisonSpit,
        Prowl,
        Pummel,
        Rabid,
        Rake,
        Ravage,
        RoarOfRecovery,
        RoarOfSacrifice,
        SavageRend,
        ScorpidPoison,
        SerenityDust,
        ShellShield,
        Smack,
        Snatch,
        SonicBlast,
        SpiritStrike,
        SporeCloud,
        Stampede,
        Sting,
        Swipe,
        Swoop,
        Tailspin,
        Taunt,
        TendonRip,
        Thunderstomp,
        VenomWebSpray,
        Warp,
        Web,
        WolverineBite
    }

    public enum PetSkillType
    {
        FocusDump,
        NonDamaging,
        SpecialMelee,
        SpecialSpell,
        SpecialUnique,
        Unimplemented
    }

    public enum AspectUsage
    {
        None,
        FoxToRun
    }

    public enum TriggerState
    {
        TRIGGER = -1,
        NOEFFECT = 0,

    }
}
