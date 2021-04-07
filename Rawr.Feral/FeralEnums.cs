using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    /// <summary>
    /// The cost of abiliest whether it requires Energy, Rage, Mana.
    /// If it adds or removes ComboPoints
    /// Castime, Cooldown, and Duration
    /// </summary>
    public enum FeralCostTypes : int
    { 
        None = 0,
        Energy,
        Rage,
        Mana,
        ComboPoint,
        // TIME is in ms to keep the int structure.
        CastTime, // How long does it cost to activate the ability?
        CooldownTime, // How long until we can use the ability again.  This is ability specific CD. 
        DurationTime, // How long does the ability last?
    }

    /// <summary>
    /// What type of Rotation the user is trying to produce
    /// </summary>
    public enum FeralRotationType
    {
        Custom, Feral, Guardian, Unknown,
    }

    /// <summary>
    /// The Different Talent Trees for Druids
    /// Taking into effect Mist of Panderia
    /// </summary>
    public enum TalentTrees
    {
        Balance, Feral, Guardian, Restoration
    }

    /// <summary>
    /// The different Druid forms. Will mostly deal with Caster, Cat, and Bear form
    /// </summary>
    public enum DruidForm
    {
        Caster, Boomkin, Cat, Bear, Tree, Aquatic, Travel, Flight,
    }

    /// <summary>
    /// All abiliest Feral and Guardian Druids can learn
    /// </summary>
    public enum FeralAbility
    {
        // Basic
        WhiteSwing,
        #region Feral Abilities
        Claw,
        FerociousBite,
        Rake,
        MangleCatForm,
        Ravage,
        RavageProc, // Ravage!
        SkullBashCatForm,
        TigersFury,
        Dash,
        SwipeCatForm,
        Shred,
        Rip,
        Maim,
        FeralChargeCat,
        ThrashCatForm, // Added in MoP
        StampedingRoarCatForm,
        Pounce,
        SavageRoar,
        #endregion

        #region Guardian Abilities
        MangleBearForm,
        Lacerate,
        Maul,
        SwipeBearForm,
        Enrage,
        SkullBashBearForm,
        Bash,
        FrenziedRegeneration,
        SavageDefense,
        FeralChargeBear,
        ThrashBearForm,
        StampedingRoarBearForm,
        SurvivalInstincts,
        #endregion

        #region Feral and Guardian Abilities
        LeaderofthePack,
        FaerieFormFeral,
        OmenOfClarity,
        FurySwipe,
        Barkskin,
        Berserk,
        FieryClaws,
        BearHug, // Added in MoP
        MightofUrsoc, // Added in MoP
        #endregion

        #region Caster Abilities
        Wrath,
        Moonfire,
        Rejuvenation,
        Revive,
        HealingTouch,
        Hurricane,
        Innervate,
        Rebirth,
        Hibernate,
        Soothe,
        Tranquility,
        Cyclone,
        #endregion

        #region Symbiosis - MoP
        #region Feral Abilities gained
        DeathCoil,                      // DK -> Feral Druid
        PlayDead,                       // Hunter -> Feral Duid
        FrostNova,                      // Mage -> Feral Druid
        Clash,                          // Monk -> Feral Druid
        DivineShield,                   // Paladin -> Feral Druid
        Dispersion,                     // Priest -> Feral Druid
        Redirect,                       // Rogue -> Feral Druid
        FeralSpirit,                    // Shaman -> Feral Druid
        SoulSwap,                       // Warlock -> Feral Druid
        ShatteringBlow,                 // Warrior -> Feral Druid
        #endregion

        #region Guardian Abilities gained
        BoneShield,                         // DK -> Guardian Druid
        IceTrap,                            // Hunter -> Guardian Duid
        MageWard,                           // Mage -> Guardian Druid
        ElusiveBrew,                        // Monk -> Guardian Druid
        Consecration,                       // Paladin -> Guardian Druid
        FearWard,                           // Priest -> Guardian Druid
        Feint,                              // Rogue -> Guardian Druid
        LightningShield,                    // Shaman -> Guardian Druid
        LifeTap,                            // Warlock -> Guardian Druid
        SpellReflection,                    // Warrior -> Feral Druid
        #endregion
        #endregion

        #region Talents
        DisplacerBeast,
        WildCharge,
        NaturesSwiftness,
        Renewal,
        CenarionWard,
        MassEntanglement,
        Typhoon,
        Incarnation,
        ForceOfNature,
        DisorientingRoar,
        UrsolsVortex,
        MighyBash,
        HeartOfTheWild,
        DreamOfCenarius,
        NaturesVigil,
        #endregion

        #region Other Abilities
        OtherPhysical,
        OtherHoly,
        OtherArcane,
        OtherFire,
        OtherFrost,
        OtherNature,
        OtherShadow,
        #endregion
    }

    public enum FeralSymbiosis
    {
        None = 0,
        DeathCoil = 1,                       // DK -> Feral Druid
        PlayDead = 2,                        // Hunter -> Feral Duid
        FrostNova = 3,                       // Mage -> Feral Druid
        Clash = 4,                           // Monk -> Feral Druid
        DivineShield = 5,                    // Paladin -> Feral Druid
        Dispersion = 6,                      // Priest -> Feral Druid
        Redirect = 7,                        // Rogue -> Feral Druid
        FeralSpirit = 8,                     // Shaman -> Feral Druid
        SoulSwap = 9,                        // Warlock -> Feral Druid
        ShatteringBlow = 10,                 // Warrior -> Feral Druid
    }

    public enum GuardianSymbiosis
    {
        None = 0,
        BoneShield = 1,                          // DK -> Guardian Druid
        IceTrap = 2,                             // Hunter -> Guardian Duid
        MageWard = 3,                            // Mage -> Guardian Druid
        ElusiveBrew = 4,                         // Monk -> Guardian Druid
        Consecration = 5,                        // Paladin -> Guardian Druid
        FearWard = 6,                            // Priest -> Guardian Druid
        Feint = 7,                               // Rogue -> Guardian Druid
        LightningShield = 8,                     // Shaman -> Guardian Druid
        LifeTap = 9,                             // Warlock -> Guardian Druid
        SpellReflection = 10,                    // Warrior -> Feral Druid
    }
}
