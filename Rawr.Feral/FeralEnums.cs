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
        #endregion

        #region Guardian Abilities
        MangleBearForm,
        DemoralizingRoar,
        Maul,
        SwipeBearForm,
        Enrage,
        SkullBashBearForm,
        ChallengingRoar,
        Bash,
        FrenziedRegeneration,
        SavageRoar,
        FeralChargeBear,
        ThrashBearForm,
        StampedingRoarBearForm,
        #endregion

        #region Feral and Guardian Abilities
        FaerieFormFeral,
        OmenOfClarity,
        FurySwipe,
        Barkskin,
        Berserk,
        FieryClaws,
        BearHug, // Added in MoP
        MightofUrsoc, // Added in Mop
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
        SymbiosisFeralMonkBrewmaster,   // Brewmaster Monk -> Feral Druid
        SymbiosisFeralMonkMistweaver,   // Mistweaver Monk -> Feral Druid
        SymbiosisFeralMonkWindwalker,   // Windwalker Monk -> Feral Druid
        SymbiosisFeralBloodDK,          // Blood DK -> Feral Druid
        SymbiosisFeralDPSDK,            // DPS DK -> Feral Druid
        FeignDeath,                     // Hunter -> Feral Duid
        FrostNova,                      // Mage -> Feral Druid
        SymbiosisFeralHolyPaladin,      // Holy Paladin -> Feral Druid
        SymbiosisFeralProtPaladin,      // Protection Paladin -> Feral Druid
        SymbiosisFeralRetPaladin,       // Retrebution Paladin -> Feral Druid
        SymbiosisFeralHealingPriest,    // Healing Priest -> Feral Druid
        SymbiosisFeralShadowPriest,     // Shadow Priest -> Feral Druid
        SymbiosisFeralRogue,            // Rogue -> Feral Druid
        SymbiosisFeralEleShaman,        // Elemental Shaman -> Feral Druid
        SymbiosisFeralEnhanceShaman,    // Enhancement Shaman -> Feral Druid
        SymbiosisFeralRestoShaman,      // Restoration Shaman -> Feral Druid
        SoulSwap,                       // Warlock -> Feral Druid
        SymbiosisFeralProtWarrior,      // Protection Warrior -> Feral Druid
        SymbiosisFeralDPSWarrior,       // DPS Warrior -> Feral Druid
        #endregion

        #region Guardian Abilities gained
        SymbiosisGuardianMonkBrewmaster,    // Brewmaster Monk -> Guardian Druid
        SymbiosisGuardianMonkMistweaver,    // Mistweaver Monk -> Guardian Druid
        SymbiosisGuardianMonkWindwalker,    // Windwalker Monk -> Guardian Druid
        SymbiosisGuardianBloodDK,           // Blood DK -> Guardian Druid
        SymbiosisGuardianDPSDK,             // DPS DK -> Guardian Druid
        IceTrap,                            // Hunter -> Guardian Duid
        SymbiosisGuardianMage,              // Mage -> Guardian Druid
        SymbiosisGuardianHolyPaladin,       // Holy Paladin -> Guardian Druid
        Consecration,                       // Protection Paladin -> Guardian Druid
        SymbiosisGuardianRetPaladin,        // Retrebution Paladin -> Guardian Druid
        FearWard,                           // Healing Priest -> Guardian Druid
        SymbiosisGuardianShadowPriest,      // Shadow Priest -> Guardian Druid
        SymbiosisGuardianRogue,             // Rogue -> Guardian Druid
        SymbiosisGuardianEleShaman,         // Elemental Shaman -> Guardian Druid
        SymbiosisGuardianEnhanceShaman,     // Enhancement Shaman -> Guardian Druid
        SymbiosisGuardianRestoShaman,       // Restoration Shaman -> Guardian Druid
        SymbiosisGuardianWarlock,           // Warlock -> Guardian Druid
        SymbiosisGuardianProtWarrior,       // Protection Warrior -> Feral Druid
        SymbiosisGuardianDPSWarrior,        // DPS Warrior -> Guardian Druid
        #endregion
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
}
