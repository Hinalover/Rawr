using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
   public static class StatConversion
    {   // Class only works for Level 85 Characters (Level 90 if you are in the Rawr5 projects)
        // Numbers reverse engineered by Whitetooth (hotdogee [at] gmail [dot] com)

        #region Character Constants
        public const int DEFAULTPLAYERLEVEL = 90;

        // These are set based on the values above
        public static float RATING_PER_DODGE            = BaseCombatRating.DodgeRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_PARRY            = BaseCombatRating.ParryRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_BLOCK            = BaseCombatRating.BlockRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_PHYSICALHIT      = BaseCombatRating.MeleeHitRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_RANGEHIT         = BaseCombatRating.RangedHitRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_SPELLHIT         = BaseCombatRating.SpellHitRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_PHYSICALCRIT     = BaseCombatRating.MeleeCritRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_RANGECRIT        = BaseCombatRating.RangeCritRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_SPELLCRIT        = BaseCombatRating.SpellCritRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_PHYSICALHASTE    = BaseCombatRating.MeleeHasteRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_RANGEHASTE       = BaseCombatRating.RangedHasteRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_SPELLHASTE       = BaseCombatRating.SpellHasteRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_EXPERTISE        = BaseCombatRating.ExpertiseRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_MASTERY          = BaseCombatRating.MasteryRatingMultiplier(DEFAULTPLAYERLEVEL); // Not a Perc, so decimal over
        // These shouldn't be changing
        public static float RATING_PER_HEALTH            = BaseCombatRating.HPPerStamina(DEFAULTPLAYERLEVEL);
        public const float RATING_PER_MANA              = 0; // MoP - Mana is static
        public const float BLOCKVALUE_PER_STR           =  2.00f;
        // These have not been provided Cata values yet, some could be removed as no longer valid
        //public const float LEVEL_85_COMBATRATING_MODIFIER      = 3.2789987789987789987789987789988f;
        public static float RATING_PER_RESILIENCE              = BaseCombatRating.PvPResilienceRatingRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public static float RATING_PER_PVP_POWER               = BaseCombatRating.PvPPowerRatingRatingMultiplier(DEFAULTPLAYERLEVEL) * 100;
        public const float RATING_PER_DODGEPARRYREDUC          = 1f; // Expertise is converted directly to % now
        public const float LEVEL_AVOIDANCE_MULTIPLIER          = 0.20f;

        // Updated for WoD in Rawr6
        // Attack Table for players attacking mobs                                            90       91        92      93
        public static readonly float[] WHITE_MISS_CHANCE_CAP                = new float[] { 0.0000f, 0.000f, 0.0000f, 0.000f };
        public static readonly float[] WHITE_MISS_CHANCE_CAP_DW             = new float[] { 0.1700f, 0.170f, 0.1700f, 0.170f }; //  OpOv: Dual wielding imposes 17% miss chance across the board for up to 3 levels higher
        
        public static readonly float[] YELLOW_MISS_CHANCE_CAP               = WHITE_MISS_CHANCE_CAP;

        public static readonly float[] WHITE_DODGE_CHANCE_CAP               = new float[] { 0.0000f, 0.000f, 0.0000f, 0.000f };
        public static readonly float[] YELLOW_DODGE_CHANCE_CAP              = WHITE_DODGE_CHANCE_CAP;

        public static readonly float[] WHITE_PARRY_CHANCE_CAP = new float[] { 0.0300f, 0.0300f, 0.0300f, 0.0300f };
        public static readonly float[] YELLOW_PARRY_CHANCE_CAP              = WHITE_PARRY_CHANCE_CAP;

        public static readonly float[] WHITE_GLANCE_CHANCE_CAP              = new float[] { 0.000f, 0.00f, 0.000f, 0.00f }; // 25%
        public static readonly float[] YELLOW_GLANCE_CHANCE_CAP             = new float[] { 0.0000f, 0.0000f, 0.0000f, 0.0000f }; //  0% Yellows don't glance
        

        //OpOv Note: TODO: Not sure about block chance in WoD
        public static readonly float[] WHITE_BLOCK_CHANCE_CAP               = new float[] { 0.0300f, 0.0450f, 0.0600f, 0.0750f };
        public static readonly float[] YELLOW_BLOCK_CHANCE_CAP              = WHITE_BLOCK_CHANCE_CAP;

        public static readonly float[] SPELL_MISS_CHANCE_CAP                = new float[] { 0.0000f, 0.0000f, 0.000f, 0.000f };

        /// <summary>
        /// You need to *add* this to your current crit value as it's a negative number.
        /// <para>[85: 0, 86: -0.01, 87: -0.02, 88: -0.03]</para>
        /// </summary>
        public static readonly float[] NPC_LEVEL_CRIT_MOD                   = new float[] { -0.0000f, -0.010f, -0.0200f, -0.0300f }; //  -3.0%

        // http://elitistjerks.com/f75/t110187-cataclysm_mage_simulators_formulators/p4/#post1834778
        // Level+3 has now been confirmed as being -1.8% (-0.0180)
        /// <summary>
        /// You need to *add* this to your current crit value as it's a negative number.
        /// <para>[85: 0, 86: -0.002625, 87: -0.00525, 88: -0.0180]</para>
        /// <para>Note: Level+1 and Level+2 values are just guesstimates based on trends
        /// from NPC_LEVEL_CRIT_MOD. We don't currently have solid values for these.</para>
        /// </summary>
        public static readonly float[] NPC_LEVEL_SPELL_CRIT_MOD             = new float[] { -0.0000f, -0.0100f, -0.0200f, -0.0300f }; //  -1.8%
        //public static readonly float[] NPC_LEVEL_SPELL_CRIT_MOD             = new float[] { -0.0000f, -0.002625f, -0.00525f, -0.0180f }; //  -1.8%

        //source: https://code.google.com/p/simulationcraft/source/browse/trunk/engine/sc_target.cpp
        public static readonly float[] NPC_ARMOR                            = new float[] { BaseCombatRating.Get_BossArmor(DEFAULTPLAYERLEVEL),
                                                                                BaseCombatRating.Get_BossArmor(DEFAULTPLAYERLEVEL + 1),
                                                                                BaseCombatRating.Get_BossArmor(DEFAULTPLAYERLEVEL + 2),
                                                                                BaseCombatRating.Get_BossArmor(DEFAULTPLAYERLEVEL + 3) };

        // Same for all classes
        public const float INT_PER_SPELLCRIT = 2533.66f;
//        public const float REGEN_CONSTANT = 0.200000002980232f;  
        public const float REGEN_CONSTANT = 1.1287f;  // Saw a slightly different value for shammy regen on EJ, but for now assuming all classes are identical

        /// <summary>
        /// Source: http://elitistjerks.com/f15/t29453-combat_ratings_level_85_cataclysm/
        /// </summary>
        public static readonly float[] AGI_PER_PHYSICALCRIT = { 0.0f, // CharacterClass starts at 1
            BaseCombatRating.WarriorChanceToMeleeCrit(DEFAULTPLAYERLEVEL),      // Warrior 1
            BaseCombatRating.PaladinChanceToMeleeCrit(DEFAULTPLAYERLEVEL),      // Paladin 2
            BaseCombatRating.HunterChanceToMeleeCrit(DEFAULTPLAYERLEVEL),       // Hunter 3
            BaseCombatRating.RogueChanceToMeleeCrit(DEFAULTPLAYERLEVEL),        // Rogue 4
            BaseCombatRating.PriestChanceToMeleeCrit(DEFAULTPLAYERLEVEL),       // Priest 5
            BaseCombatRating.DeathKnightChanceToMeleeCrit(DEFAULTPLAYERLEVEL),  // Death Knight 6
            BaseCombatRating.ShamanChanceToMeleeCrit(DEFAULTPLAYERLEVEL),       // Shaman 7
            BaseCombatRating.MageChanceToMeleeCrit(DEFAULTPLAYERLEVEL),         // Mage 8
            BaseCombatRating.WarriorChanceToMeleeCrit(DEFAULTPLAYERLEVEL),      // Warlock 9
            BaseCombatRating.MonkChanceToMeleeCrit(DEFAULTPLAYERLEVEL),         // Monk 10
            BaseCombatRating.DruidChanceToMeleeCrit(DEFAULTPLAYERLEVEL),        // Druid 11
        };

        /// <summary>
        /// Source: http://elitistjerks.com/f15/t29453-combat_ratings_level_85_cataclysm/
        /// </summary>
        public static readonly float[] AGI_PER_DODGE = { 0.0f, // Starts at 0
            0, // Patch 4.2 removed Agility to Dodge for Warriors //5.309f * 84.74576271f, // Warrior 1
            0, // Patch 4.2 removed Agility to Dodge for Paladins //5.309f * 59.88023952f, // Paladin 2
            951.158596f, // Hunter 3
            951.158596f, // Rogue 4
            951.158596f, // Priest 5
            0, // Patch 4.2 removed Agility to Dodge for DKs //5.309f * 84.74576271f, // Death Knight 6
            951.158596f, // Shaman 7
            951.158596f, // Mage 8
            951.158596f, // Warlock 9
            951.158596f, // Monk 10
            951.158596f, // Druid 11
        };

        public static readonly float[] PARRY_PER_STR = { 0.0f, // Stats at 0
            243.58281085f, //90 - 951.158596 // Warrior 1
            243.58281085f, //90 - 951.158596 // Paladin 2
              0.0f, // Hunter 3
              0.0f, // Rogue 4
              0.0f, // Priest 5
            243.58281085f, //90 - 951.158596 // Death Knight 6
              0.0f, // Shaman 7
              0.0f, // Mage 8
              0.0f, // Warlock 9
              0.0f, // Monk 10
              0.0f, // Druid 11
        };

        public static readonly float STR_PER_PARRY_PERCENT = 95120f;

        public static readonly float[] MASTERY_PER_BLOCK = { 0.0f, // Starts at 0
            0.015f, // Warrior 1
            0.01125f, // Paladin 2
            0f, // Hunter 3
            0f, // Rogue 4
            0f, // Priest 5
            0f, // Death Knight 6
            0f, // Shaman 7
            0f, // Mage 8
            0f, // Warlock 9
            0f, // Monk 10
            0f, // Druid 11
        };

        public static readonly float[] DR_COEFFIENT = { 0.0f, // Starts at 0
            0.8850f, // Warrior 1
            0.8850f, // Paladin 2
            0.9880f, // Hunter 3
            0.9880f, // Rogue 4
            0.9530f, // Priest 5
            0.8850f, // Death Knight 6
            0.9880f, // Shaman 7
            0.9530f, // Mage 8
            0.9530f, // Warlock 9
            1.4220f, // Monk 10
            1.2220f, // Druid 11
        };

        // This is the cap value for DODGE PERCENTAGE.
        public static readonly float[] CAP_DODGE = { 0.0f, // Starts at 0
             65.631440f, // Warrior 1
             65.631440f, // Paladin 2
            145.560408f, // Hunter 3
            145.560408f, // Rogue 4
            150.375940f, // Priest 5
             65.631440f, // Death Knight 6
            145.560408f, // Shaman 7
            150.375940f, // Mage 8
            150.375940f, // Warlock 9
            505.000000f, // Monk 10
            150.375940f, // Druid 11
        };

        /// <summary>
        /// This is the 1/CAP_DODGE to cut down the ammount of math going on.
        /// </summary>
        public static readonly float[] CAP_DODGE_INV = { 0.0f, // Starts at 0
            0.01523660f, // Warrior 1
            0.01523660f, // Paladin 2
            0.00687000f, // Hunter 3
            0.00687000f, // Rogue 4
            0.00665000f, // Priest 5
            0.01523660f, // Death Knight 6
            0.00687000f, // Shaman 7
            0.00665000f, // Mage 8
            0.00665000f, // Warlock 9
            0.00198020f, // Monk 10
            0.00665336f, // Druid 11
        };

        // This is the cap value for PARRY PERCENTAGE.
        public static readonly float[] CAP_PARRY = { 0.0f, // Starts at 0
            235.500000f, // Warrior 1
            235.500000f, // Paladin 2
            145.560408f, // Hunter 3
            145.560408f, // Rogue 4
              0f,        // Priest 5
            235.500000f, // Death Knight 6
            145.560408f, // Shaman 7
              0f,        // Mage 8
              0f,        // Warlock 9
             91f,        // Monk 10
              0f,        // Druid 11
        };

        /// <summary>
        /// This is the 1/CAP_PARRY to cut down the amount of math going on.
        /// And prevent divide by 0 errors.
        /// </summary>
        public static readonly float[] CAP_PARRY_INV = { 0.0f, // Starts at 0
            0.00424628f, // Warrior 1
            0.00424628f, // Paladin 2
            0.00687000f, // Hunter 3
            0.00687000f, // Rogue 4
            0f,          // Priest 5
            0.00424628f, // Death Knight 6
            0.00687000f, // Shaman 7
            0f,          // Mage 8
            0f,          // Warlock 9
            0.01098901f, // Monk 10
            0f,          // Druid 11
        };

       // This is the cap value for BLOCK PERCENTAGE.
        public static readonly float[] CAP_BLOCK = { 0.0f, // Starts at 0
            135.1f,     // Warrior 1
            135.1f,     // Paladin 2
            0f,         // Hunter 3
            0f,         // Rogue 4
            0f,         // Priest 5
            0f,         // Death Knight 6
            0f,         // Shaman 7
            0f,         // Mage 8
            0f,         // Warlock 9
            0f,         // Monk 10
            0f,         // Druid 11
        };

       /// <summary>
       /// This is 1/CAP_BLOCK to cut down the amount of math going on,
       /// and prevent divide by 0 errors.
       /// </summary>
        public static readonly float[] CAP_BLOCK_INV = { 0.0f, // Starts at 0
            0.00740192f,     // Warrior 1
            0.00740192f,     // Paladin 2
            0f,         // Hunter 3
            0f,         // Rogue 4
            0f,         // Priest 5
            0f,         // Death Knight 6
            0f,         // Shaman 7
            0f,         // Mage 8
            0f,         // Warlock 9
            0f,         // Monk 10
            0f,         // Druid 11
        };

        /// <summary>This is the cap value for MISS PERCENTAGE on NPC attacks against a Player</summary>
        public static readonly float[] CAP_MISSED = { 0.0f, // Starts at 0
            16f, // Warrior 1
            16f, // Paladin 2
             0f, // Hunter 3
             0f, // Rogue 4
             0f, // Priest 5
            16f, // Death Knight 6
             0f, // Shaman 7
             0f, // Mage 8
             0f, // Warlock 9
             0f, // Monk 10
             0f, // Druid 11
        };

        #endregion

        #region Functions for Plain Rating Conversions

        #region Health
        public static float GetHealthFromStamina(float Rating, CharacterClass Class) { return GetHealthFromStamina(Rating); }
        /// <summary>
        /// Returns a Value (1000 = 1000 extra Health)
        /// </summary>
        /// <param name="Rating">Stamina</param>
        /// <returns>A Value (1000 = 1000 extra Health)</returns>
        public static float GetHealthFromStamina(float Rating) {
            return Rating <= 20 ? Rating : (Rating - 20) * RATING_PER_HEALTH + 20; // first 20 stamina is 1 health
        }
        public static float GetHealthFromStamina(float Rating, int Level)
        {
            return Rating <= 20 ? Rating : (Rating - 20) * BaseCombatRating.HPPerStamina(Level) + 20; // first 20 stamina is 1 health
        }
        #endregion

        #region Mana
        public static float GetManaFromIntellect(float Rating, CharacterClass Class) { return GetManaFromIntellect(Rating); }
        /// <summary>
        /// Returns a Value (1000 = 1000 extra Mana)
        /// </summary>
        /// <param name="Rating">Intellect</param>
        /// <returns>A Value (1000 = 1000 extra Mana)</returns>
        public static float GetManaFromIntellect(float Rating)
        {
            return Rating <= 20 ? Rating : (Rating - 20) * RATING_PER_MANA + 20; // first 20 intellect is 1 mana
        }
        #endregion

        #region Block
        public static float GetBlockFromRating(float Rating, CharacterClass Class) { return GetBlockFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% added chance to Block)
        /// </summary>
        /// <param name="Rating">Block Rating</param>
        /// <returns>A Percentage (0.05 = 5% added chance to Block)</returns>
        public static float GetBlockFromRating(float Rating)
        {
            return Rating / RATING_PER_BLOCK;
        }
        public static float GetBlockFromRating(float Rating, int Level)
        {
            return Rating / (BaseCombatRating.BlockRatingMultiplier(Level) * 100);
        }

        public static float GetBlockValueFromStrength(float str, CharacterClass Class) { return GetBlockValueFromStrength(str); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% added chance to Block)
        /// </summary>
        /// <param name="Rating">Block Rating</param>
        /// <returns>A Percentage (0.05 = 5% added chance to Block)</returns>
        public static float GetBlockValueFromStrength(float str) { return str / BLOCKVALUE_PER_STR; }
        #endregion

        #region Dodge
        public static float GetDodgeFromRating(float Rating, CharacterClass Class) { return GetDodgeFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Dodge)
        /// </summary>
        /// <param name="Rating">Dodge Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Dodge)</returns>
        public static float GetDodgeFromRating(float Rating) { return Rating / RATING_PER_DODGE; }
        public static float GetDodgeFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.DodgeRatingMultiplier(Level) * 100); }

        #region Agility
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Dodge)
        /// </summary>
        /// <param name="Agility">Agility</param>
        /// <param name="Class">CharacterClass</param>
        /// <returns>A Percentage (0.05 = 5% extra Dodge)</returns>
        public static float GetDodgeFromAgility(float Agility, CharacterClass Class)
        {
            if (AGI_PER_DODGE[(int)Class] > 0)
                return Agility / AGI_PER_DODGE[(int)Class] * 0.01f;
            return 0;
        }
        #endregion
        #endregion

        #region Mastery
        public static float GetMasteryFromRating(float Rating, CharacterClass Class) { return GetMasteryFromRating(Rating); }
        /// <summary>
        /// Returns a Value (6.34 = 6.34 extra Mastery)
        /// </summary>
        /// <param name="Rating">Mastery Rating</param>
        /// <returns>A Value (6.34 = 6.34 extra Mastery)</returns>
        public static float GetMasteryFromRating(float Rating) { return Rating / RATING_PER_MASTERY; }
        public static float GetMasteryFromRating(float Rating, int Level) { return Rating / BaseCombatRating.MasteryRatingMultiplier(Level); }
        #endregion

        #region Expertise
        public static float GetExpertiseFromRating(float Rating, CharacterClass Class) { return GetExpertiseFromRating(Rating); }
        /// <summary>
        /// Returns a Value (6.34 = 6.34 extra Expertise)
        /// </summary>
        /// <param name="Rating">Expertise Rating</param>
        /// <returns>A Value (6.34 = 6.34 extra Expertise)</returns>
        public static float GetExpertiseFromRating(float Rating) { return Rating / RATING_PER_EXPERTISE; }
        public static float GetExpertiseFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.ExpertiseRatingMultiplier(Level) * 100); }

        public static float GetRatingFromExpertise(float value, CharacterClass Class) { return GetRatingFromExpertise(value); }
        /// <summary>
        /// Returns a Value (6.34 = 6.34 extra Expertise)
        /// </summary>
        /// <param name="Rating">Expertise Rating</param>
        /// <returns>A Value (6.34 = 6.34 extra Expertise)</returns>
        public static float GetRatingFromExpertise(float value) { return value * RATING_PER_EXPERTISE; }
        public static float GetRatingFromExpertise(float value, int Level) { return value * BaseCombatRating.ExpertiseRatingMultiplier(Level); }

        public static float GetDodgeParryReducFromExpertise(float Rating, CharacterClass Class) { return GetDodgeParryReducFromExpertise(Rating); }
        /// <summary>
        /// Returns a Percentage (1.00 = 1% extra Dodge/Parry Reduction)
        /// </summary>
        /// <param name="Rating">Expertise</param>
        /// <returns>A Percentage (1.00 = 1% extra Dodge/Parry Reduction)</returns>
        public static float GetDodgeParryReducFromExpertise(float Rating) { return Rating * RATING_PER_DODGEPARRYREDUC; }
        public static float GetExpertiseFromDodgeParryReduc(float value, CharacterClass Class) { return GetExpertiseFromDodgeParryReduc(value); }
        /// <summary>
        /// Returns a Value (1 = 1 extra Expertise)
        /// </summary>
        /// <param name="Rating">DodgeParryReduc %</param>
        /// <returns>A Value (1 = 1 extra Expertise)</returns>
        public static float GetExpertiseFromDodgeParryReduc(float value) { return value / RATING_PER_DODGEPARRYREDUC; }
        #endregion

        #region Parry
        public static float GetParryFromRating(float Rating, CharacterClass Class) { return GetParryFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Parry)
        /// </summary>
        /// <param name="Rating">Parry Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Parry)</returns>
        public static float GetParryFromRating(float Rating) { return Rating / RATING_PER_PARRY; }
        public static float GetParryFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.ParryRatingMultiplier(Level) * 100); }

        public static float GetParryFromStrength(float Strength) { return Strength / STR_PER_PARRY_PERCENT; }
        #endregion

        #region Crit
        #region Physical Crit
        public static float GetCritFromRating(float Rating, CharacterClass Class) { return GetPhysicalCritFromRating(Rating); }
        public static float GetCritFromRating(float Rating) { return GetPhysicalCritFromRating(Rating); }
        public static float GetPhysicalCritFromRating(float Rating, CharacterClass Class) { return GetPhysicalCritFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Crit)
        /// </summary>
        /// <param name="Rating">Crit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Crit)</returns>
        public static float GetPhysicalCritFromRating(float Rating) { return Rating / RATING_PER_PHYSICALCRIT; }
        public static float GetPhysicalCritFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.MeleeCritRatingMultiplier(Level) * 100); }

        public static float GetRatingFromCrit(float Rating, CharacterClass Class) { return GetRatingFromPhysicalCrit(Rating); }
        public static float GetRatingFromCrit(float Rating) { return GetRatingFromPhysicalCrit(Rating); }
        public static float GetRatingFromPhysicalCrit(float Rating, CharacterClass Class) { return GetRatingFromPhysicalCrit(Rating); }
        /// <summary>
        /// Returns a Value (1 = 1 Crit Rating)
        /// </summary>
        /// <param name="Rating">Crit Percent</param>
        /// <returns>A Value (1 = 1 Crit Rating)</returns>
        public static float GetRatingFromPhysicalCrit(float Rating) { return Rating * RATING_PER_PHYSICALCRIT; }
        public static float GetRatingFromPhysicalCrit(float Rating, int Level) { return Rating * BaseCombatRating.MeleeCritRatingMultiplier(Level); }
        #endregion

        #region Range Crit
        public static float GetRangeCritFromRating(float Rating, CharacterClass Class) { return GetRangeCritFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Crit)
        /// </summary>
        /// <param name="Rating">Crit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Crit)</returns>
        public static float GetRangeCritFromRating(float Rating) { return Rating / RATING_PER_RANGECRIT; }
        public static float GetRangeCritFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.RangeCritRatingMultiplier(Level) * 100); }

        public static float GetRatingFromRangeCrit(float Rating, CharacterClass Class) { return GetRatingFromRangeCrit(Rating); }
        /// <summary>
        /// Returns a Value (1 = 1 Crit Rating)
        /// </summary>
        /// <param name="Rating">Crit Percent</param>
        /// <returns>A Value (1 = 1 Crit Rating)</returns>
        public static float GetRatingFromRangeCrit(float Rating) { return Rating * RATING_PER_RANGECRIT; }
        public static float GetRatingFromRangeCrit(float Rating, int Level) { return Rating * BaseCombatRating.RangeCritRatingMultiplier(Level); }
        #endregion

        #region Spell Crit
        public static float GetSpellCritFromRating(float Rating, CharacterClass Class) { return GetSpellCritFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Crit)
        /// </summary>
        /// <param name="Rating">Crit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Crit)</returns>
        public static float GetSpellCritFromRating(float Rating) { return Rating / RATING_PER_SPELLCRIT; }
        public static float GetSpellCritFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.SpellCritRatingMultiplier(Level) * 100); }

        public static float GetSpellCritFromIntellect(float Intellect, CharacterClass Class, int Level)
        {
            switch (Class)
            {
                case CharacterClass.Warrior:
                    return Intellect / BaseCombatRating.WarriorChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.Paladin:
                    return Intellect / BaseCombatRating.PaladinChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.Hunter:
                    return Intellect / BaseCombatRating.HunterChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.Rogue:
                    return Intellect / BaseCombatRating.RogueChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.Priest:
                    return Intellect / BaseCombatRating.PriestChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.DeathKnight:
                    return Intellect / BaseCombatRating.DeathKnightChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.Shaman:
                    return Intellect / BaseCombatRating.ShamanChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.Mage:
                    return Intellect / BaseCombatRating.MageChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.Warlock:
                    return Intellect / BaseCombatRating.WarlockChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.Monk:
                    return Intellect / BaseCombatRating.MonkChanceToSpellCrit(Level) * 0.01f;
                case CharacterClass.Druid:
                    return Intellect / BaseCombatRating.DruidChanceToSpellCrit(Level) * 0.01f;
                default:
                    return 0;
            }

        }
        public static float GetSpellCritFromIntellect(float Intellect, CharacterClass Class) { return GetSpellCritFromIntellect(Intellect, Class, DEFAULTPLAYERLEVEL); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Crit)
        /// </summary>
        /// <param name="Intellect">Intellect</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Crit)</returns>
        public static float GetSpellCritFromIntellect(float Intellect) { return Intellect / INT_PER_SPELLCRIT * 0.01f; }
        #endregion

        #region Agility
        public static float GetCritFromAgility(float Agility, CharacterClass Class) { return GetPhysicalCritFromAgility(Agility, Class); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Crit)
        /// </summary>
        /// <param name="Agility">Agility</param>
        /// <param name="Class">CharacterClass</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Crit)</returns>
        public static float GetPhysicalCritFromAgility(float Agility, CharacterClass Class)
        {
            return Agility / AGI_PER_PHYSICALCRIT[(int)Class] * 0.01f;
        }
        public static float GetPhysicalCritFromAgility(float Agility, CharacterClass Class, int Level)
        {
            switch (Class)
            {
                case CharacterClass.Warrior:
                    return Agility / BaseCombatRating.WarriorChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.Paladin:
                    return Agility / BaseCombatRating.PaladinChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.Hunter:
                    return Agility / BaseCombatRating.HunterChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.Rogue:
                    return Agility / BaseCombatRating.RogueChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.Priest:
                    return Agility / BaseCombatRating.PriestChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.DeathKnight:
                    return Agility / BaseCombatRating.DeathKnightChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.Shaman:
                    return Agility / BaseCombatRating.ShamanChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.Mage:
                    return Agility / BaseCombatRating.MageChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.Warlock:
                    return Agility / BaseCombatRating.WarlockChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.Monk:
                    return Agility / BaseCombatRating.MonkChanceToMeleeCrit(Level) * 0.01f;
                case CharacterClass.Druid:
                    return Agility / BaseCombatRating.DruidChanceToMeleeCrit(Level) * 0.01f;
                default:
                    return 0;
            }
        }
        #endregion
        #endregion

        #region Haste
        #region Physical Haste
        // Returns a Percentage
        public static float GetHasteFromRating(float Rating, CharacterClass Class) { return GetPhysicalHasteFromRating(Rating); }
        public static float GetPhysicalHasteFromRating(float Rating, CharacterClass Class) { return GetPhysicalHasteFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Haste)
        /// </summary>
        /// <param name="Rating">Haste Rating</param>
        /// <param name="Class">CharacterClass</param>
        /// <returns>A Percentage (0.05 = 5% extra Haste)</returns>
        public static float GetPhysicalHasteFromRating(float Rating) {
            //Removed in Cata (Patch 4.0.3)
            /*if (Class == CharacterClass.DeathKnight
                || Class == CharacterClass.Druid
                || Class == CharacterClass.Paladin
                || Class == CharacterClass.Shaman)
                return Rating / RATING_PER_PHYSICALHASTE * 1.3f;*/    // Patch 3.1: Hybrids gain 30% more Physical Haste from Haste Rating.
            return Rating / RATING_PER_PHYSICALHASTE;
        }
        public static float GetPhysicalHasteFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.MeleeHasteRatingMultiplier(Level) * 100); }
        #endregion

        #region Range Haste
        public static float GetRangeHasteFromRating(float Rating, CharacterClass Class) { return GetRangeHasteFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Haste)
        /// </summary>
        /// <param name="Rating">Haste Rating</param>
        /// <param name="Class">CharacterClass</param>
        /// <returns>A Percentage (0.05 = 5% extra Haste)</returns>
        public static float GetRangeHasteFromRating(float Rating) { return Rating / RATING_PER_RANGEHASTE; }
        public static float GetRangeHasteFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.RangedHasteRatingMultiplier(Level) * 100); }
        #endregion

        #region Spell Haste
        public static float GetSpellHasteFromRating(float Rating, CharacterClass Class) { return GetSpellHasteFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Haste)
        /// </summary>
        /// <param name="Rating">Haste Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Haste)</returns>
        public static float GetSpellHasteFromRating(float Rating) { return Rating / RATING_PER_SPELLHASTE; }
        public static float GetSpellHasteFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.SpellHasteRatingMultiplier(Level) * 100); }
        #endregion
        #endregion

        #region Hit
        #region Physical Hit
        public static float GetHitFromRating(float Rating, CharacterClass Class) { return GetPhysicalHitFromRating(Rating); }
        public static float GetHitFromRating(float Rating) { return GetPhysicalHitFromRating(Rating); }
        public static float GetPhysicalHitFromRating(float Rating, CharacterClass Class) { return GetPhysicalHitFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Hit)
        /// </summary>
        /// <param name="Rating">Hit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Hit)</returns>
        public static float GetPhysicalHitFromRating(float Rating){return Rating / RATING_PER_PHYSICALHIT;}
        public static float GetPhysicalHitFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.MeleeHitRatingMultiplier(Level) * 100); }

        public static float GetRatingFromHit(float value, CharacterClass Class) { return GetRatingFromHit(value); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Hit)
        /// </summary>
        /// <param name="Rating">Hit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Hit)</returns>
        public static float GetRatingFromHit(float value) { return value * RATING_PER_PHYSICALHIT; }
        public static float GetRatingFromHit(float value, int Level) { return value * (BaseCombatRating.MeleeHitRatingMultiplier(Level) * 100); }
        #endregion

        #region Range Hit
        public static float GetRangeHitFromRating(float Rating, CharacterClass Class) { return GetRangeHitFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Hit)
        /// </summary>
        /// <param name="Rating">Hit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Hit)</returns>
        public static float GetRangeHitFromRating(float Rating) { return Rating / RATING_PER_RANGEHIT; }
        public static float GetRangeHitFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.RangedHitRatingMultiplier(Level) * 100); }

        public static float GetRatingFromRangeHit(float value, CharacterClass Class) { return GetRatingFromRangeHit(value); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Hit)
        /// </summary>
        /// <param name="Rating">Hit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Hit)</returns>
        public static float GetRatingFromRangeHit(float value) { return value * RATING_PER_RANGEHIT; }
        public static float GetRatingFromRangeHit(float value, int Level) { return value * BaseCombatRating.RangedHitRatingMultiplier(Level); }
        #endregion

        #region Spell Hit
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Hit)
        /// </summary>
        /// <param name="Rating">Hit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Hit)</returns>
        public static float GetRatingFromSpellHit(float value, CharacterClass Class) { return GetRatingFromSpellHit(value); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Hit)
        /// </summary>
        /// <param name="Rating">Hit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Hit)</returns>
        public static float GetRatingFromSpellHit(float value) { return value * RATING_PER_SPELLHIT; }
        public static float GetRatingFromSpellHit(float value, int Level) { return value * BaseCombatRating.SpellHitRatingMultiplier(Level); }

        public static float GetSpellHitFromRating(float Rating, CharacterClass Class) { return GetSpellHitFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Hit)
        /// </summary>
        /// <param name="Rating">Hit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Hit)</returns>
        public static float GetSpellHitFromRating(float Rating) { return Rating / RATING_PER_SPELLHIT; }
        public static float GetSpellHitFromRating(float Rating, int Level) { return Rating / (BaseCombatRating.SpellHitRatingMultiplier(Level) * 100); }
        #endregion
        #endregion

        #region Spirit
        public static float GetSpiritRegenSec(float Spirit, float Intellect, CharacterClass Class) { return GetSpiritRegenSec(Spirit, Intellect); }
        /// <summary>
        /// Returns a Number, How much mana is gained each Second. (Multiply by 5 to get MP5)
        /// </summary>
        /// <param name="Spirit">Spirit</param>
        /// <param name="Intellect">Intellect</param>
        /// <returns>A Number, How much mana is gained each Second. (Multiply by 5 to get MP5)</returns>
        public static float GetSpiritRegenSec(float Spirit, float Intellect)
        {
//            return 0.001f + Spirit * REGEN_CONSTANT * (float)Math.Sqrt(Intellect);
            // Patch 5.0 doesn't seem to be using Int for mana regen
            return 0.001f + Spirit * REGEN_CONSTANT / 5.0f;
        }
        #endregion

        /// <summary>
        /// Returns the % Damage reduction based on amount of PvP Resilience
        /// </summary>
        /// <param name="resilience">Amount of PvP Resilience</param>
        /// <returns>% Damage reduction on attacks from other players</returns>
        public static float GetDamageReductionFromResilience(float pvpResilience)
        {
            return pvpResilience / RATING_PER_RESILIENCE;
        }

        /// <summary>
        /// Returns the % Damage reduction based on amount of PvP Power
        /// </summary>
        /// <param name="resilience">Amount of PvP Power</param>
        /// <returns>% Damage reduction on attacks to other players</returns>
        public static float GetDamageReductionFromPvPPower(float pvpPower)
        {
            return pvpPower / RATING_PER_PVP_POWER;
        }


        public static float ApplyMultiplier(float baseValue, float multiplier)
        {
            return (baseValue * (1f + multiplier));
        }
        public static float ApplyInverseMultiplier(float baseValue, float multiplier)
        {
            return (baseValue * (1f - multiplier));
        }
        #endregion

        #region Functions for More complex things.

        /// <summary>Originally from Bear. This should be updated once per expansion</summary>
        public const float MitigationScaler         = 78210f; // 90 - 138772.5 // value is 75% Armor Reduction at the character level
        public const double SurvivalScalerBase      = 0.1574901d; // fourToTheNegativeFourThirds = Math.Pow(4d, -4d / 3d)
        public const double SurvivalScalerTopRight  = 0.6299605d; // topRight = Math.Pow(fourToTheNegativeFourThirds, 1d / 4d)

        public static float getMitigationScaler(int characterLevel)
        {
            if (characterLevel > 85)
                return (12112.5f * characterLevel) - 951352.5f;
            else if (characterLevel > 80)
                return (6502.5f * characterLevel) - 474502.5f;
            else if (characterLevel > 59)
                return (1402.5f * characterLevel) - 66502.5f;
            else
                return (255f * characterLevel) - 1200f;
        }

        /// <summary>
        /// Returns how much physical damage is reduced from Armor. (0.095 = 9.5% reduction)
        /// </summary>
        /// <remarks>
        /// * http://forums.worldofwarcraft.com/thread.html?topicId=16473618356&sid=1&pageNo=4 post 77.<br/>
        /// * Ghostcrawler vs theorycraft.<br/>
        /// * http://elitistjerks.com/f15/t29453-combat_ratings_level_85_cataclysm/p24/#post1841717<br/>
        /// </remarks>
        /// <param name="AttackerLevel">Level of Attacker</param>
        /// <param name="TargetArmor">Armor of Target</param>
        /// <param name="ArmorIgnoreDebuffs">Armor reduction on target as result of Debuffs (Sunder/Fearie Fire) These are Multiplied.</param>
        /// <param name="ArmorIgnoreBuffs">Armor reduction buffs on player (Mace Spec, Battle Stance, etc) These are Added.</param>
        /// <returns>How much physical damage is reduced from Armor. (0.095 = 9.5% reduction)</returns>
        public static float GetArmorDamageReduction(int AttackerLevel, float TargetArmor, float ArmorIgnoreDebuffs, float ArmorIgnoreBuffs)
        {
            return GetArmorDamageReduction(AttackerLevel, (int)POSSIBLE_LEVELS.LVLP3, TargetArmor, ArmorIgnoreDebuffs, ArmorIgnoreBuffs);
        }

        /// <summary>
        /// Returns how much physical damage is reduced from Armor. (0.095 = 9.5% reduction)
        /// <para>This function used to take in ArP Rating but that was removed from the game in Cata</para>
        /// </summary>
        /// <remarks>
        /// * http://forums.worldofwarcraft.com/thread.html?topicId=16473618356&sid=1&pageNo=4 post 77.<br/>
        /// * Ghostcrawler vs theorycraft.<br/>
        /// * http://elitistjerks.com/f15/t29453-combat_ratings_level_85_cataclysm/p24/#post1841717<br/>
        /// </remarks>
        /// <param name="AttackerLevel">Level of Attacker</param>
        /// <param name="TargetLevel">Level of Target</param>
        /// <param name="TargetArmor">Armor of Target</param>
        /// <param name="ArmorIgnoreDebuffs">Armor reduction on target as result of Debuffs (Sunder/Fearie Fire) These are Multiplied.</param>
        /// <param name="ArmorIgnoreBuffs">Armor reduction buffs on player (Mace Spec, Battle Stance, etc) These are Added.</param>
        /// <returns>How much physical damage is reduced from Armor. (0.095 = 9.5% reduction)</returns>
        public static float GetArmorDamageReduction(int AttackerLevel, int TargetLevel, float TargetArmor, float ArmorIgnoreDebuffs, float ArmorIgnoreBuffs)
        {
            /*
             * float ArmorConstant = 0;
             * if ( AttackerLevel > 85 )
		     *    ArmorConstant = AttackerLevel + (4.5f * (AttackerLevel - 59)) + (20 * (AttackerLevel - 80)) + (22 * (AttackerLevel - 85));
	         *else if ( AttackerLevel > 80 ) 
        	 *	ArmorConstant = AttackerLevel + (4.5f * (AttackerLevel - 59)) + (20 * (AttackerLevel - 80));
	         *else if ( AttackerLevel > 59 )
             *    ArmorConstant = AttackerLevel + (4.5f * (AttackerLevel - 59));
             *else
             *    ArmorConstant = AttackerLevel;
             */
            TargetArmor *= (1f - ArmorIgnoreDebuffs);
            //float ArPCap = Math.Min((TargetArmor + ArmorConstant) / 3f, TargetArmor);
            //TargetArmor -= ArPCap * Math.Min(1f, ArmorIgnoreBuffs);
            //float temp = TargetArmor / (85 * ArmorConstant + 400);
            //float armorReduction = temp / (1f + temp);
            float armorReduction = GetDamageReductionFromArmor(AttackerLevel, TargetArmor);
            //if (armorReduction > 0.75f) { armorReduction = 0.75f; }
            return armorReduction;
        }

        /// <summary>
        /// Gets the amount of physical damage reduction provided by armor, from an attacker.
        /// In the case of modeling a tank's damage reduction, Attacker would be the boss, Target would be the tank.
        /// </summary>
        /// <param name="AttackerLevel">The level of the creature making the attack.</param>
        /// <param name="TargetArmor">The armor of the creature being attacked.</param>
        /// <returns>How much physical damage is reduced from Armor. (0.095 = 9.5% reduction)</returns>
        public static float GetDamageReductionFromArmor(int AttackerLevel, float TargetArmor)
        {
            if (AttackerLevel > 85)
                return Math.Min(0.75f, (TargetArmor) / (TargetArmor + 4037.5f * AttackerLevel - 317117.5f));
            else if (AttackerLevel > 80)
                return Math.Min(0.75f, (TargetArmor) / (TargetArmor + 2167.5f * AttackerLevel - 158167.5f));
            else if (AttackerLevel > 59)
                return Math.Min(0.75f, (TargetArmor) / (TargetArmor + 467.5f * AttackerLevel - 22167.5f));
            else
                return Math.Min(0.75f, (TargetArmor) / (TargetArmor + 85f * AttackerLevel - 400f));
            #region LibStatLogic-1.2.lua file in Rating Buster
            // Above is a simplified version of what is in LUA code
            /*
            local levelModifier = attackerLevel
            if ( levelModifier > 85 ) then
                levelModifier = levelModifier + (4.5 * (levelModifier - 59)) + (20 * (levelModifier - 80)) + (22 * (levelModifier - 85));
            elseif ( levelModifier > 80 ) then
                levelModifier = levelModifier + (4.5 * (levelModifier - 59)) + (20 * (levelModifier - 80));
            elseif ( levelModifier > 59 ) thenf
                levelModifier = levelModifier + (4.5 * (levelModifier - 59))
            end
            local temp = armor / (85 * levelModifier + 400)
            local armorReduction = temp / (1 + temp)
            -- caps at 0.75
            if armorReduction > 0.75 then
                armorReduction = 0.75
            end
             */
            #endregion
        }

        /// <summary>
        /// Returns the chance to miss (0.09 = 9% chance to miss)<br/>
        /// http://www.wow-dark-destiny.com/images/spellhit.png
        /// </summary>
        /// <param name="LevelDelta">Attacker Level - Defender Level</param>
        /// <param name="bPvP">Set to True if Player vs Player combat</param>
        /// <returns>The chance to miss (0.09 = 9% chance to miss)</returns>
        public static float GetSpellMiss(int LevelDelta, bool bPvP)
        {
            /*
            if (-LevelDelta <= 2)
                return (float)Math.Min(0.0f, (-LevelDelta + 4) * 0.01f);

            if (-LevelDelta > 2)
                if (bPvP)
                    return (float)Math.Min(0.62f, (-LevelDelta * 7 - 8) * 0.01f);

            return (float)Math.Min(0.94f, (-LevelDelta * 11 - 16) * 0.01f);
             */
            if (LevelDelta > 3 || LevelDelta < 0)
                return Math.Max(0, (LevelDelta * 0.06f) + 0.06f);
            else
                return SPELL_MISS_CHANCE_CAP[LevelDelta];
        }

        private static float AttackerResistancePenalty(int LevelDelta)
        {
            if (LevelDelta == 1)
                return 0f;
            else if (LevelDelta == 2)
                return 0f;
            else if (LevelDelta == 3)
                return 95f;
            return 0f;
        }

        /// <summary>
        /// Returns a Percent giving Average Magical Damage Resisted (0.16 = 16% Resisted)
        /// </summary>
        /// <param name="AttackerLevel">Level of the Attacker</param>
        /// <param name="TargetLevel">Level of the Target</param>
        /// <param name="TargetResistance">Targets Resistance</param>
        /// <param name="AttackerSpellPenetration">Attackers Spell Penetration</param>
        /// <returns>A Percent giving Average Magical Damage Resisted (0.16 = 16% Resisted)</returns>
        public static float GetAverageResistance(int AttackerLevel, int TargetLevel,
            float TargetResistance, float AttackerSpellPenetration)
        {
            float ActualResistance = (float)Math.Max(0f, TargetResistance - AttackerSpellPenetration);
            return ActualResistance / (AttackerLevel * 5f + AttackerResistancePenalty(AttackerLevel - TargetLevel) + ActualResistance)
                   /*+ 0.02f * (float)Math.Max(0, TargetLevel - AttackerLevel)*/; // apparently level-based partial resists were removed in Cataclysm
        }

        /// <summary>
        /// Returns a Table giving the chance to fall within a resistance slice cutoff.
        /// The table is float[11] Table, where Table[0] is 0% resisted, and Table[10] is 100% resisted.
        /// Each Table entry gives how much chance to roll into that slice.
        /// So if Table[1] contains 0.165, that means you have a 16.5% chance to resist 10% damage.
        /// </summary>
        /// <param name="AttackerLevel">Level of the Attacker</param>
        /// <param name="TargetLevel">Level of the Target</param>
        /// <param name="TargetResistance">Targets Resistance</param>
        /// <param name="AttackerSpellPenetration">Attackers Spell Penetration</param>
        /// <returns>A Table giving the chance to fall within a resistance slice cutoff.</returns>
        public static float[] GetResistanceTable(int AttackerLevel, int TargetLevel,
            float TargetResistance, float AttackerSpellPenetration)
        {                      //   00% 10% 20% 30% 40% 50% 60% 70% 80% 90% 100%
            float[] ResistTable = { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
            float AverageResistance = GetAverageResistance(AttackerLevel, TargetLevel, TargetResistance, AttackerSpellPenetration);

            for (int x = -1; x < 11; x++)
            {   // Build Table
                float ResistSlice = (float)Math.Max(0f, 0.5f - 2.5f * (float)Math.Abs(0.1f * x - AverageResistance));
                if (x == -1)
                {   // Adjust 0% and 10% for "negative" resists.
                    ResistTable[0] += 2f * ResistSlice;
                    ResistTable[1] -= 1f * ResistSlice;
                }
                else
                    ResistTable[x] += ResistSlice;
            }
            return ResistTable;
        }

        /// <summary>
        /// Returns a String version of the Table giving the chance to fall within a resistance slice cutoff.
        /// Useful as part of a tooltip
        /// </summary>
        /// <param name="AttackerLevel">Level of the Attacker</param>
        /// <param name="TargetLevel">Level of the Target</param>
        /// <param name="TargetResistance">Targets Resistance</param>
        /// <param name="AttackerSpellPenetration">Attackers Spell Penetration</param>
        /// <returns>A string version of a Table giving the chance to fall within a resistance slice cutoff.</returns>
        public static string GetResistanceTableString(int AttackerLevel, int TargetLevel,
            float TargetResistance, float AttackerSpellPenetration)
        {
            int count;
            string tipResist = string.Empty;
            tipResist = Math.Round(StatConversion.GetAverageResistance(AttackerLevel, TargetLevel, TargetResistance, AttackerSpellPenetration) * 100.0f, 2).ToString() + "% average resistance \n";
            tipResist += "% Resisted     Occurance";

            float[] ResistTable = StatConversion.GetResistanceTable(AttackerLevel, TargetLevel, TargetResistance, AttackerSpellPenetration);
            for (count = 0; count < 10; count++)
            {
                if (ResistTable[count] > 0)
                {
                    tipResist += "\n" + Math.Round(count * 10.0f, 1) + " % resisted   " + Math.Round(ResistTable[count] * 100.0f, 2) + "%";
                }
            }

            return tipResist;
        }

        /// <summary>
        /// Returns the Minimum amount of Spell Damage will be resisted. (0.2 = Anything below 20% is always resisted)
        /// If this returns 0.0, that means you will always have a chance to take full damage from spells.
        /// If this returns 0.1, that means you will never take full damage from spells, and minumum you will take is 10% reduction.
        /// </summary>
        /// <param name="AttackerLevel">Level of Attacker</param>
        /// <param name="TargetLevel">Level of Target</param>
        /// <param name="TargetResistance">Target Resistance</param>
        /// <param name="AttackerSpellPenetration">Attacker Spell Penetration</param>
        /// <returns>The Minimum amount of Spell Damage will be resisted. (0.2 = Anything below 20% is always resisted)</returns>
        public static float GetMinimumResistance(int AttackerLevel, int TargetLevel,
            float TargetResistance, float AttackerSpellPenetration)
        {
            float[] ResistTable = GetResistanceTable(AttackerLevel, TargetLevel, TargetResistance, AttackerSpellPenetration);

            for (int x = 0; x < 11; x++)
                if (ResistTable[x] > 0f)
                    return 0.1f * x;
            return 0f;
        }

        // Initial function taken from the ProtWarrior Module.
        // Then using table found on EJ:
        // http://elitistjerks.com/f31/t29453-combat_ratings_level_80_a/
        // creating updated Avoidance Chance w/ DR build in formula.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character">Character in question.</param>
        /// <param name="stats">Stats object... total stats of the character.</param>
        /// <param name="avoidanceType">What type of hit is the target doing on the character?</param>
        /// <param name="TargetLevel">Level of the target being fought</param>
        /// <returns>A % value where .50 == 50%</returns>
        public static float GetDRAvoidanceChance(Character character, Stats stats, HitResult avoidanceType, int TargetLevel) { return GetDRAvoidanceChance(character, stats, avoidanceType, (uint)TargetLevel); }
        public static float GetDRAvoidanceChance(Character character, Stats stats, HitResult avoidanceType, uint TargetLevel)
        {
            /*
            float defSkill = stats.Defense;
            // Let's make sure we don't run off the bottom w/ a negative defense rating.
            stats.DefenseRating = Math.Max(stats.DefenseRating, 0f);
            float defSkillMod = (GetDefenseFromRating(stats.DefenseRating, character.Class) * DEFENSE_RATING_AVOIDANCE_MULTIPLIER);
            float baseAvoid = (defSkill - (TargetLevel * 5)) * DEFENSE_RATING_AVOIDANCE_MULTIPLIER;
            float modifiedAvoid = defSkillMod;
            float finalAvoid = 0f; // I know it breaks my lack of redundancy rule, but it helps w/ readability.
            int iClass = (int)character.Class;
            */

            float baseAvoid = (character.Level - TargetLevel) * LEVEL_AVOIDANCE_MULTIPLIER;
            float modifiedAvoid = 0.0f;
            float finalAvoid    = 0.0f;
            int iClass          = (int)character.Class;

            switch (avoidanceType)
            {
                case HitResult.Dodge:
                    if ((character.Class == CharacterClass.DeathKnight) || (character.Class == CharacterClass.Paladin) || (character.Class == CharacterClass.Warrior))
                        baseAvoid += (stats.Dodge * 100f);
                    else
                        baseAvoid += ((stats.Dodge + GetDodgeFromAgility(stats.BaseAgility, character.Class)) * 100f);
                    // Assuring we don't run off the bottom w/ negative dodge rating.
                    stats.DodgeRating = Math.Max(stats.DodgeRating, 0f);
                    modifiedAvoid += ((GetDodgeFromAgility((stats.Agility - stats.BaseAgility), character.Class) +
                                    GetDodgeFromRating(stats.DodgeRating)) * 100f);
                    modifiedAvoid = DRMath(CAP_DODGE_INV[iClass], DR_COEFFIENT[iClass], modifiedAvoid);
                    // Don't run off the bottom if we have negative dodge
                    finalAvoid = Math.Max(baseAvoid + modifiedAvoid, 0);
                    finalAvoid = Math.Min(finalAvoid, CAP_DODGE[iClass]);
                    break;
                case HitResult.Parry:
                    baseAvoid += stats.Parry * 100f;
                    baseAvoid += GetParryFromStrength(stats.Strength) * 100f;
                    // Assuring we don't run off the bottom w/ negative parry rating.
                    stats.ParryRating = Math.Max(stats.ParryRating, 0f);
                    //if (character.Class == CharacterClass.DeathKnight) stats.ParryRating += (stats.Strength - BaseStats.GetBaseStats(character).Strength) * 0.25f;
                    modifiedAvoid += (GetParryFromRating(stats.ParryRating) * 100f);
                    modifiedAvoid = DRMath(CAP_PARRY_INV[iClass], DR_COEFFIENT[iClass], modifiedAvoid);
                    finalAvoid = Math.Max(baseAvoid + modifiedAvoid, 0);
                    finalAvoid = Math.Min(finalAvoid, CAP_PARRY[iClass]);
                    break;
                case HitResult.Miss:
                    // Base Miss rate according is 5%
                    // However, this can be talented up (e.g. Frigid Dreadplate, NE racial, etc.) 
                    baseAvoid += stats.Miss * 100f;
                    modifiedAvoid = DRMath( (1f/CAP_MISSED[iClass]), DR_COEFFIENT[iClass], modifiedAvoid );
                    // Factoring in the Miss Cap. 
                    modifiedAvoid = Math.Min(CAP_MISSED[iClass], modifiedAvoid);
                    finalAvoid = Math.Max(baseAvoid + modifiedAvoid, 0);
                    finalAvoid = Math.Min(finalAvoid, CAP_MISSED[iClass]);
                    break;
                case HitResult.Block:
                    // Base Block is 3%
                    baseAvoid += stats.Block * 100f;
                    // Assuring we don't run off the bottom w/ negative block rating.
                    if (MASTERY_PER_BLOCK[iClass] > 0)
                    {
                        stats.MasteryRating = Math.Max(stats.MasteryRating, 0f);
                        modifiedAvoid += (stats.Mastery + GetMasteryFromRating(stats.MasteryRating)) * MASTERY_PER_BLOCK[iClass] * 100f;
                    }
                    modifiedAvoid = DRMath(CAP_BLOCK_INV[iClass], DR_COEFFIENT[iClass], modifiedAvoid);
                    finalAvoid = Math.Max(baseAvoid + modifiedAvoid, 0);
                    finalAvoid = Math.Min(finalAvoid, CAP_BLOCK[iClass]);
                    break;
                case HitResult.Crit:
                    // Resilience doesn't change crit chance anymore.
                    //modifiedAvoid -= (GetCritReductionFromResilience(stats.Resilience) * 100f);
                    finalAvoid = baseAvoid + modifiedAvoid;
                    break;
            }

            // Many of the base values are whole numbers, so need to get it back to decimal. 
            // May want to look at making this more consistant in the future.
            finalAvoid = finalAvoid / 100.0f;
            return finalAvoid;
        }
        /// <summary>
        /// StatPostDR =  1/(CAP_STAT_INV + COEF/StatPreDR)
        /// </summary>
        /// <param name="inv_cap">One of the CAP_STAT_INV values, appropriate for the class.</param>
        /// <param name="coefficient">One of the DR_COEF values, appropriate for the class.</param>
        /// <param name="valuePreDR">The value of the stat before DR are factored in.</param>
        /// <returns></returns>
        private static float DRMath(float inv_cap, float coefficient, float valuePreDR)
        {
            float DRValue = 0f;
            DRValue = 1f / (inv_cap + coefficient / valuePreDR);
            return DRValue;
        }
        #endregion
    }
}
