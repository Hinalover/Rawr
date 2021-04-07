using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public enum GlyphType { Minor = 0, Major, Prime, }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class GlyphDataAttribute : Attribute
    {
        public GlyphDataAttribute(int index, int spellid, int effectid, string name, GlyphType type, string description)
        {
            _index = index;
            _spellid = spellid;
            _effectid = effectid;
            _name = name;
            _type = type;
            _description = description;
        }

        private readonly int _index;
        private readonly int _spellid;
        private readonly int _effectid;
        private readonly string _name;
        private readonly GlyphType _type;
        private readonly string _description;

        public int Index { get { return _index; } }
        /// <summary>The Spell that applies the Glyph:<br/>E.g.- Glyph of Judgement: http://www.wowhead.com/item=41092 </summary>
        public int SpellID { get { return _spellid; } }
        /// <summary>The Glyph Effect itself:<br/>E.g.- Glyph of Judgement: http://www.wowhead.com/spell=54922 </summary>
        public int EffectID { get { return _effectid; } }
        public string Name { get { return _name; } }
        public GlyphType Type { get { return _type; } }
        public string Description { get { return _description; } }
    }

    public partial class WarriorTalents
    {
        private bool[] _glyphData = new bool[34];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Reduces the cooldown on Bladestorm by 15 sec.</summary>
        [GlyphData(0, 45790, 63324, "Glyph of Bladestorm", GlyphType.Prime,
            @"Reduces the cooldown on Bladestorm by 15 sec.")]
        public bool GlyphOfBladestorm { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the damage of Bloodthirst by 10%.</summary>
        [GlyphData(1, 43416, 58367, "Glyph of Bloodthirst", GlyphType.Prime,
            @"Increases the damage of Bloodthirst by 10%.")]
        public bool GlyphOfBloodthirst { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases the critical strike chance of Devastate by 5%.</summary>
        [GlyphData(2, 43415, 58388, "Glyph of Devastate", GlyphType.Prime,
            @"Increases the critical strike chance of Devastate by 5%.")]
        public bool GlyphOfDevastate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the damage of Mortal Strike by 10%.</summary>
        [GlyphData(3, 43421, 58368, "Glyph of Mortal Strike", GlyphType.Prime,
            @"Increases the damage of Mortal Strike by 10%.")]
        public bool GlyphOfMortalStrike { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage of Overpower by 10%.</summary>
        [GlyphData(4, 43422, 58386, "Glyph of Overpower", GlyphType.Prime,
            @"Increases the damage of Overpower by 10%.")]
        public bool GlyphOfOverpower { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Increases the critical strike chance of Raging Blow by 5%.</summary>
        [GlyphData(5, 43432, 58370, "Glyph of Raging Blow", GlyphType.Prime,
            @"Increases the critical strike chance of Raging Blow by 5%.")]
        public bool GlyphOfRagingBlow { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases the damage of Revenge by 10%.</summary>
        [GlyphData(6, 43424, 58364, "Glyph of Revenge", GlyphType.Prime,
            @"Increases the damage of Revenge by 10%.")]
        public bool GlyphOfRevenge { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage of Shield Slam by 10%.</summary>
        [GlyphData(7, 43425, 58375, "Glyph of Shield Slam", GlyphType.Prime,
            @"Increases the damage of Shield Slam by 10%.")]
        public bool GlyphOfShieldSlam { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the critical strike chance of Slam by 5%.</summary>
        [GlyphData(8, 43423, 58385, "Glyph of Slam", GlyphType.Prime,
            @"Increases the critical strike chance of Slam by 5%.")]
        public bool GlyphOfSlam { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 9; } }
        /// <summary>Increases the number of targets your Cleave hits by 1.</summary>
        [GlyphData( 9, 43414, 58366, "Glyph of Cleaving", GlyphType.Major,
            @"Increases the number of targets your Cleave hits by 1.")]
        public bool GlyphOfCleaving { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Your Colossus Smash refreshes the duration of Sunder Armor stacks on a target.</summary>
        [GlyphData(10, 63481, 89003, "Glyph of Colossus Smash", GlyphType.Major,
            @"Your Colossus Smash refreshes the duration of Sunder Armor stacks on a target.")]
        public bool GlyphOfColossusSmash { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Death Wish no longer increases damage taken.</summary>
        [GlyphData(11, 67483, 94374, "Glyph of Death Wish", GlyphType.Major,
            @"Death Wish no longer increases damage taken.")]
        public bool GlyphOfDeathWish { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Your Heroic Throw applies a stack of Sunder Armor.</summary>
        [GlyphData(12, 43418, 58357, "Glyph of Heroic Throw", GlyphType.Major,
            @"Your Heroic Throw applies a stack of Sunder Armor.")]
        public bool GlyphOfHeroicThrow { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases the duration of your Intercept stun by 1 sec.</summary>
        [GlyphData(13, 67482, 94372, "Glyph of Intercept", GlyphType.Major,
            @"Increases the duration of your Intercept stun by 1 sec.")]
        public bool GlyphOfIntercept { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Increases the number of attacks you intercept for your Intervene target by 1.</summary>
        [GlyphData(14, 43419, 58377, "Glyph of Intervene", GlyphType.Major,
            @"Increases the number of attacks you intercept for your Intervene target by 1.")]
        public bool GlyphOfIntervene { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Increases the range of your Charge ability by 5 yards.</summary>
        [GlyphData(15, 43397, 58097, "Glyph of Long Charge", GlyphType.Major,
            @"Increases the range of your Charge ability by 5 yards.")]
        public bool GlyphOfLongCharge { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Increases the radius of Piercing Howl by 50%.</summary>
        [GlyphData(16, 43417, 58372, "Glyph of Piercing Howl", GlyphType.Major,
            @"Increases the radius of Piercing Howl by 50%.")]
        public bool GlyphOfPiercingHowl { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Reduces the cooldown of your Charge ability by 1 sec.</summary>
        [GlyphData(17, 43413, 58355, "Glyph of Rapid Charge", GlyphType.Major,
            @"Reduces the cooldown of your Charge ability by 1 sec.")]
        public bool GlyphOfRapidCharge { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Reduces the rage cost of your Thunder Clap ability by 5.</summary>
        [GlyphData(18, 43430, 58356, "Glyph of Resonating Power", GlyphType.Major,
            @"Reduces the rage cost of your Thunder Clap ability by 5.")]
        public bool GlyphOfResonatingPower { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Shield wall now reduces damage taken by 20%, but increases its cooldown by 2 min.</summary>
        [GlyphData(19, 45797, 63329, "Glyph of Shield Wall", GlyphType.Major,
            @"Shield wall now reduces damage taken by 20%, but increases its cooldown by 2 min.")]
        public bool GlyphOfShieldWall { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Reduces the cooldown on Shockwave by 3 sec.</summary>
        [GlyphData(20, 45792, 63325, "Glyph of Shockwave", GlyphType.Major,
            @"Reduces the cooldown on Shockwave by 3 sec.")]
        public bool GlyphOfShockwave { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Reduces the cooldown on Spell Reflection by 1 sec.</summary>
        [GlyphData(21, 45795, 63328, "Glyph of Spell Reflection", GlyphType.Major,
            @"Reduces the cooldown on Spell Reflection by 1 sec.")]
        public bool GlyphOfSpellReflection { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>When you apply or refresh Sunder Armor, a second nearby target also receives athe Sunder Armor effect.</summary>
        [GlyphData(22, 43427, 58387, "Glyph of Sunder Armor", GlyphType.Major,
            @"When you apply or refresh Sunder Armor, a second nearby target also receives athe Sunder Armor effect.")]
        public bool GlyphOfSunderArmor { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Reduces the rage cost of Sweeping Strikes ability by 100%.</summary>
        [GlyphData(23, 43428, 58384, "Glyph of Sweeping Strikes", GlyphType.Major,
            @"Reduces the rage cost of Sweeping Strikes ability by 100%.")]
        public bool GlyphOfSweepingStrikes { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Increases the radius of your Thunder Clap ability by 2 yards.</summary>
        [GlyphData(24, 43399, 58098, "Glyph of Thunder Clap", GlyphType.Major,
            @"Increases the radius of your Thunder Clap ability by 2 yards.")]
        public bool GlyphOfThunderClap { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Increases the total healing provided by your Victory Rush by 50%.</summary>
        [GlyphData(25, 43431, 58382, "Glyph of Victory Rush", GlyphType.Major,
            @"Increases the total healing provided by your Victory Rush by 50%.")]
        public bool GlyphOfVictoryRush { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 26; } }
        /// <summary>Increases the duration by 2 min and area of effect by 50% of your Battle Shout.</summary>
        [GlyphData(26, 43395, 58095, "Glyph of Battle", GlyphType.Minor,
            @"Increases the duration by 2 min and area of effect by 50% of your Battle Shout.")]
        public bool GlyphOfBattle { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Berserker Rage generates 5 rage when used.</summary>
        [GlyphData(27, 43396, 58096, "Glyph of Berserker Rage", GlyphType.Minor,
            @"Berserker Rage generates 5 rage when used.")]
        public bool GlyphOfBerserkerRage { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Increases the healing your receive from your Bloodthirst ability by 40%.</summary>
        [GlyphData(28, 43412, 58369, "Glyph of Bloody Healing", GlyphType.Minor,
            @"Increases the healing your receive from your Bloodthirst ability by 40%.")]
        public bool GlyphOfBloodyHealing { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Increases the duration by 2 min and the area of effect by 50% of your Commanding Shout.</summary>
        [GlyphData(29, 49084, 68164, "Glyph of Command", GlyphType.Minor,
            @"Increases the duration by 2 min and the area of effect by 50% of your Commanding Shout.")]
        public bool GlyphOfCommand { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Increases the duration by 15 sec and area of effect 50% of your Demoralizing Shout.</summary>
        [GlyphData(30, 43398, 58099, "Glyph of Demoralizing Shout", GlyphType.Minor,
            @"Increases the duration by 15 sec and area of effect 50% of your Demoralizing Shout.")]
        public bool GlyphOfDemoralizingShout { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Increases the window of opportunity in which you can use Victory Rush by 5 sec.</summary>
        [GlyphData(31, 43400, 58104, "Glyph of Enduring Victory", GlyphType.Minor,
            @"Increases the window of opportunity in which you can use Victory Rush by 5 sec.")]
        public bool GlyphOfEnduringVictory { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Reduces the cost of Sunder Armor by 50%.</summary>
        [GlyphData(32, 45793, 63326, "Glyph of Furious Sundering", GlyphType.Minor,
            @"Reduces the cost of Sunder Armor by 50%.")]
        public bool GlyphOfFuriousSundering { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Targets of your Intimidating Shout now tremble in place instead of fleeing in fear.</summary>
        [GlyphData(33, 45794, 63327, "Glyph of Intimidating Shout", GlyphType.Minor,
            @"Targets of your Intimidating Shout now tremble in place instead of fleeing in fear.")]
        public bool GlyphOfIntimidatingShout { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        #endregion
    }

    public partial class MageTalents
    {
        private bool[] _glyphData = new bool[34];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 0; } }
        /// <summary>Increases the radius of your Arcane Explosion by 5 yards.</summary>
        [GlyphData(0, 42736, 115718, "Glyph of Arcane Explosion", GlyphType.Major,
            @"Increases the radius of your Arcane Explosion by 5 yards.")]
        public bool GlyphOfArcaneExplosion { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the duration and cooldown of Arcane Power by 100%.</summary>
        [GlyphData(1, 44955, 62210, "Glyph of Arcane Power", GlyphType.Major,
            @"Increases the duration and cooldown of Arcane Power by 100%.")]
        public bool GlyphOfArcanePower { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Reduces the cast time of your Frost Armor, Mage Armor, and Molten Armor spells by 1.5 sec, and increases the defensive effect of each Armor by an additional 10%.</summary>
        [GlyphData(2, 69773, 98397, "Glyph of Armors", GlyphType.Major,
            @"Reduces the cast time of your Frost Armor, Mage Armor, and Molten Armor spells by 1.5 sec, and increases the defensive effect of each Armor by an additional 10%.")]
        public bool GlyphOfArmors { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the distance you travel with the Blink spell by 5 yards.</summary>
        [GlyphData(3, 42737, 56365, "Glyph of Blink", GlyphType.Major,
            @"Increases the distance you travel with the Blink spell by 5 yards.")]
        public bool GlyphOfBlink { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the direct damage, the duration of the damage over time effect and the cooldown of Combustion by 100%.</summary>
        [GlyphData(4, 42739, 56368, "Glyph of Combustion", GlyphType.Major,
            @"Increases the direct damage, the duration of the damage over time effect and the cooldown of Combustion by 100%.")]
        public bool GlyphOfCombustion { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Increases the damage done by Cone of Cold by 200%.</summary>
        [GlyphData(5, 42746, 115705, "Glyph of Cone of Cold", GlyphType.Major,
            @"Increases the damage done by Cone of Cold by 200%.")]
        public bool GlyphOfConeOfCold { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Your Counterspell can now be cast while casting or channeling other spells, but its cooldown is increased by 4 sec.</summary>
        [GlyphData(6, 50045, 115703, "Glyph of Cone of Cold", GlyphType.Major,
            @"Your Counterspell can now be cast while casting or channeling other spells, but its cooldown is increased by 4 sec.")]
        public bool GlyphOfCounterspell { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Your Deep Freeze spell is no longer on the global cooldown.</summary>
        [GlyphData(7, 45740, 115710, "Glyph of Deep Freeze", GlyphType.Major,
            @"Your Deep Freeze spell is no longer on the global cooldown.")]
        public bool GlyphOfDeepFreeze { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Your Evocation ability also causes you to regain 60% of your health over its duration. With the Invocation talent, you instead gain 40% of your health upon completing an Evocation. With the Rune of Power talent, you gain 1% of your health per second while standing in your own Rune of Power.</summary>
        [GlyphData(8, 42738, 56380, "Glyph of Evocation", GlyphType.Major,
            @"Your Evocation ability also causes you to regain 60% of your health over its duration. 

With the Invocation talent, you instead gain 40% of your health upon completing an Evocation.

With the Rune of Power talent, you gain 1% of your health per second while standing in your own Rune of Power.")]
        public bool GlyphOfEvocation { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Your Fire Blast and Inferno Blast spells now also spread Living Bomb, cause Frost Bomb to instantly explode, and cause Nether Tempest to instantly fire its secondary damage at all nearby targets.</summary>
        [GlyphData(9, 63539, 89926, "Glyph of Fire Blast", GlyphType.Major,
            @"Your Fire Blast and Inferno Blast spells now also spread Living Bomb, cause Frost Bomb to instantly explode, and cause Nether Tempest to instantly fire its secondary damage at all nearby targets.")]
        public bool GlyphOfFireBlast { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Reduces the cooldown of Frost Nova by 5 sec.</summary>
        [GlyphData(10, 42741, 56376, "Glyph of Frost Nova", GlyphType.Major,
            @"Reduces the cooldown of Frost Nova by 5 sec.")]
        public bool GlyphOfFrostNova { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Reduces the cast time of Frostfire Bolt by 0.5 sec.</summary>
        [GlyphData(11, 44684, 61205, "Glyph of Frostfire Bolt", GlyphType.Major,
            @"Reduces the cast time of Frostfire Bolt by 0.5 sec.")]
        public bool GlyphOfFrostfireBolt { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>When Ice Block terminates, it triggers an instant free Frost Nova and makes you immune to all spells for 3 sec.</summary>
        [GlyphData(12, 42744, 115723, "Glyph of Ice Block", GlyphType.Major,
            @"When Ice Block terminates, it triggers an instant free Frost Nova and makes you immune to all spells for 3 sec.")]
        public bool GlyphOfIceBlock { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Your Ice Lance now hits 1 additional target for 50% damage.</summary>
        [GlyphData(13, 42745, 56377, "Glyph of Ice Lance", GlyphType.Major,
            @"Your Ice Lance now hits 1 additional target for 50% damage.")]
        public bool GlyphOfIceLance { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Your Icy Veins causes your Frostbolt, Frostfire Bolt, Ice Lance, and your Water Elemental's Waterbolt spells to split into 3 smaller bolts that each do 160% damage, instead of increasing spell casting speed.</summary>
        [GlyphData(14, 42753, 56364, "Glyph of Icy Veins", GlyphType.Major,
            @"Your Icy Veins causes your Frostbolt, Frostfire Bolt, Ice Lance, and your Water Elemental's Waterbolt spells to split into 3 smaller bolts that each do 160% damage, instead of increasing spell casting speed.")]
        public bool GlyphOfIcyVeins { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Increases your movement speed while Invisible by 40%.</summary>
        [GlyphData(15, 42748, 56366, "Glyph of Invisibility", GlyphType.Major,
            @"Increases your movement speed while Invisible by 40%.")]
        public bool GlyphOfInvisibility { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Your Conjure Mana Gem spell now creates a Brilliant Mana Gem, which holds up to 10 charges.</summary>
        [GlyphData(16, 42749, 56383, "Glyph of Mana Gem", GlyphType.Major,
            @"Your Conjure Mana Gem spell now creates a Brilliant Mana Gem, which holds up to 10 charges.")]
        public bool GlyphOfManaGem { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Your Polymorph spell also removes all damage over time effects from the target.</summary>
        [GlyphData(17, 42752, 56375, "Glyph of Polymorph", GlyphType.Major,
            @"Your Polymorph spell also removes all damage over time effects from the target.")]
        public bool GlyphOfPolymorph { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the damage you deal by 10% for 10 sec after you successfully remove a curse.</summary>
        [GlyphData(18, 44920, 115700, "Glyph of Remove Curse", GlyphType.Major,
            @"Increases the damage you deal by 10% for 10 sec after you successfully remove a curse.")]
        public bool GlyphOfRemoveCurse { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Your Arcane Blast spell applies the Slow spell to any target it damages if no target is currently affected by your Slow.</summary>
        [GlyphData(19, 45737, 86209, "Glyph of Slow", GlyphType.Major,
            @"Your Arcane Blast spell applies the Slow spell to any target it damages if no target is currently affected by your Slow.")]
        public bool GlyphOfSlow { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Spellsteal now also heals you for 3% of your maximum health when it successfully steals a spell.</summary>
        [GlyphData(20, 42754, 115713, "Glyph of Spellsteal", GlyphType.Major,
            @"Spellsteal now also heals you for 3% of your maximum health when it successfully steals a spell.")]
        public bool GlyphOfSpellsteal { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Increases the health of your Water Elemental by 40%, and allows it to cast while moving. When in Assist mode and in combat, commanding your Water Elemental to Follow will cause it to stay near you and autocast Waterbolt when your target is in range.</summary>
        [GlyphData(21, 45736, 63090, "Glyph of Water Elemental", GlyphType.Major,
            @"Increases the health of your Water Elemental by 40%, and allows it to cast while moving. When in Assist mode and in combat, commanding your Water Elemental to Follow will cause it to stay near you and autocast Waterbolt when your target is in range.")]
        public bool GlyphOfWaterElemental { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 22; } }
        /// <summary>Your Arcane Brilliance spell allows you to comprehend your allies' racial languages.</summary>
        [GlyphData(22, 43364, 57925, "Glyph of Arcane Language", GlyphType.Minor,
            @"Your Arcane Brilliance spell allows you to comprehend your allies' racial languages.")]
        public bool GlyphOfArcaneLanguage { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Teaches you the ability Conjure Familiar. Conjures a familiar stone, containing either an Arcane, Fiery, or Icy Familiar.</summary>
        [GlyphData(23, 43359, 126748, "Glyph of Conjure Familiar", GlyphType.Minor,
            @"Teaches you the ability Conjure Familiar.

Conjures a familiar stone, containing either an Arcane, Fiery, or Icy Familiar.")]
        public bool GlyphOfConjureFamiliar { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>When cast on critters, your Polymorph spells now last 24 hrs and can be cast on multiple targets.</summary>
        [GlyphData(24, 42751, 56382, "Glyph of Crittermorph", GlyphType.Minor,
            @"When cast on critters, your Polymorph spells now last 24 hrs and can be cast on multiple targets.")]
        public bool GlyphOfCrittermorph { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Teaches you the ability Illusion. Transforms the Mage to look like someone else for 2 min.</summary>
        [GlyphData(25, 45738, 63092, "Glyph of Illusion", GlyphType.Minor,
            @"Teaches you the ability Illusion.

Transforms the Mage to look like someone else for 2 min.")]
        public bool GlyphOfIllusion { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Your Mana Gem now restores mana over 6 sec, rather than instantly.</summary>
        [GlyphData(26, 42735, 56363, "Glyph of Loose Mana", GlyphType.Minor,
            @"Your Mana Gem now restores mana over 6 sec, rather than instantly.")]
        public bool GlyphOfLooseMana { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Your Mirror Images cast Arcane Blast or Fireball instead of Frostbolt depending on your primary talent tree.</summary>
        [GlyphData(27, 45739, 63093, "Glyph of Mirror Image", GlyphType.Minor,
            @"Your Mirror Images cast Arcane Blast or Fireball instead of Frostbolt depending on your primary talent tree.")]
        public bool GlyphOfMirrorImage { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Your Blink spell teleports you in the direction you are moving instead of the direction you are facing.</summary>
        [GlyphData(28, 42743, 56384, "Glyph of Momentum", GlyphType.Minor,
            @"Your Blink spell teleports you in the direction you are moving instead of the direction you are facing.")]
        public bool GlyphOfMomentum { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>After casting a Mage Teleport spell, or entering a Mage Portal, your movement speed is increased by 70% for 1 min.</summary>
        [GlyphData(29, 63416, 89749, "Glyph of Rapid Teleportation", GlyphType.Minor,
            @"After casting a Mage Teleport spell, or entering a Mage Portal, your movement speed is increased by 70% for 1 min.")]
        public bool GlyphOfRapidTeleportation { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Your Polymorph: Sheep spell polymorphs the target into a polar bear cub instead.</summary>
        [GlyphData(30, 0, 58136, "Glyph of the Bear Cub", GlyphType.Minor,
            @"Your Polymorph: Sheep spell polymorphs the target into a polar bear cub instead.")]
        public bool GlyphOfTheBearCub { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Your Polymorph: Sheep spell polymorphs the target into a monkey instead.</summary>
        [GlyphData(31, 43360, 57927, "Glyph of the Monkey", GlyphType.Minor,
            @"Your Polymorph: Sheep spell polymorphs the target into a monkey instead.")]
        public bool GlyphOfTheMonkey { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Your Polymorph: Sheep spell polymorphs the target into a penguin instead.</summary>
        [GlyphData(32, 43361, 52648, "Glyph of the Penquin", GlyphType.Minor,
            @"Your Polymorph: Sheep spell polymorphs the target into a penguin instead.")]
        public bool GlyphOfThePenquin { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Your Polymorph: Sheep spell polymorphs the target into a penguin instead.</summary>
        [GlyphData(33, 43339, 57924, "Glyph of the Porcupine", GlyphType.Minor,
            @"Your Polymorph: Sheep spell polymorphs the target into a penguin instead.")]
        public bool GlyphOfThePorcupine { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        #endregion
    }

    public partial class DruidTalents
    {
        private bool[] _glyphData = new bool[41];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Major
        public override int GlyphTreeStartingIndexes_0 { get { return -1; } }
        public override int GlyphTreeStartingIndexes_1 { get { return 0; } }
        /// <summary>Reduces the chance you'll be critically hit by 25% while Barkskin is active.</summary>
        [GlyphData(0, 45623, 63057, "Glyph of Barkskin", GlyphType.Major,
            @"Reduces the chance you'll be critically hit by 25% while Barkskin is active.")]
        public bool GlyphofBarkskin { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the bloom heal of your Lifebloom when it expires by 20%, but your Healing Touch, Nourish, and Regrowth abilities no longer refresh the duration of Lifebloom.</summary>
        [GlyphData(1, 43331, 121840, "Glyph of Blooming", GlyphType.Major,
            @"Increases the bloom heal of your Lifebloom when it expires by 20%, but your Healing Touch, Nourish, and Regrowth abilities no longer refresh the duration of Lifebloom.")]
        public bool GlyphofBlooming { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases healing done to you by 20% while in Cat Form.</summary>
        [GlyphData(2, 67487, 47180, "Glyph of Cat Form", GlyphType.Major,
            @"Increases healing done to you by 20% while in Cat Form.")]
        public bool GlyphofCatForm { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the range of your Cyclone spell by 4 yards.</summary>
        [GlyphData(3, 45622, 48514, "Glyph of Cyclone", GlyphType.Major,
            @"Increases the range of your Cyclone spell by 4 yards.")]
        public bool GlyphofCyclone { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Reduces the cooldown of your Dash ability by 60 sec.</summary>
        [GlyphData(4, 43674, 59219, "Glyph of Dash", GlyphType.Major,
            @"Reduces the cooldown of your Dash ability by 60 sec.")]
        public bool GlyphofDash { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Reduces the cast time of your Entangling Roots by 0.2 sec.</summary>
        [GlyphData(5, 40924, 54760, "Glyph of Entangling Roots", GlyphType.Major,
            @"Reduces the cast time of your Entangling Roots by 0.2 sec.")]
        public bool GlyphofEntanglingRoots { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Faerie Fire used in Bear Form also silences the target for 3 sec, but triggers a 15 sec cooldown.</summary>
        [GlyphData(6, 67484, 114237, "Glyph of Fae Silence", GlyphType.Major,
            @"Faerie Fire used in Bear Form also silences the target for 3 sec, but triggers a 15 sec cooldown.")]
        public bool GlyphofFaeSilence { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the range of your Faerie Fire by 10 yds.</summary>
        [GlyphData(7, 67485, 94386, "Glyph of Faerie Fire", GlyphType.Major,
            @"Increases the range of your Faerie Fire by 10 yds.")]
        public bool GlyphOfFaerieFire { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Your Ferocious Bite ability heals you for 1% of your maximum health for each 10 Energy used.</summary>
        [GlyphData(8, 48720, 67598, "Glyph of Ferocious Bite", GlyphType.Major,
            @"Your Ferocious Bite ability heals you for 1% of your maximum health for each 10 Energy used.")]
        public bool GlyphofFerociousBite { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>For 6 sec after activating Frenzied Regeneration, healing effects on you are 40% more powerful.  However, your Frenzied Regeneration now always costs 60 Rage and no longer converts Rage into health.</summary>
        [GlyphData(9, 40896, 54810, "Glyph of Frenzied Regeneration", GlyphType.Major,
            @"For 6 sec after activating Frenzied Regeneration, healing effects on you are 40% more powerful.  However, your Frenzied Regeneration now always costs 60 Rage and no longer converts Rage into health.")]
        public bool GlyphofFrenziedRegeneration { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>When you cast Healing Touch, the cooldown on your Swiftmend is reduced by 1 sec.</summary>
        [GlyphData(10, 40914, 54825, "Glyph of Healing Touch", GlyphType.Major,
            @"When you cast Healing Touch, the cooldown on your Swiftmend is reduced by 1 sec.")]
        public bool GlyphofHealingTouch { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Your Hurricane and Astral Storm abilities now also slow the movement speed of their victims by 50%.</summary>
        [GlyphData(11, 40920, 54831, "Glyph of Hurricane", GlyphType.Major,
            @"Your Hurricane and Astral Storm abilities now also slow the movement speed of their victims by 50%.")]
        public bool GlyphofHurricane { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>When Innervate is cast on a friendly target other than the caster, the caster will gain 10% of maximum mana over 10 sec.</summary>
        [GlyphData(12, 40908, 54832, "Glyph of Innervate", GlyphType.Major,
            @"When Innervate is cast on a friendly target other than the caster, the caster will gain 10% of maximum mana over 10 sec.")]
        public bool GlyphofInnervate { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>While not in Tree of Life Form, casting Lifebloom on a new target grants that target as many applications as the old target had.</summary>
        [GlyphData(13, 40915, 114228, "Glyph of Lifebloom", GlyphType.Major,
            @"While not in Tree of Life Form, casting Lifebloom on a new target grants that target as many applications as the old target had.")]
        public bool GlyphofLifebloom { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Reduces the mana cost of all shapeshifts by 90%.</summary>
        [GlyphData(14, 44928, 116172, "Glyph of Master Shapeshifter", GlyphType.Major,
            @"Reduces the mana cost of all shapeshifts by 90%.")]
        public bool GlyphofMasterShapeshifter { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Your Maul ability now hits 1 additional target for 50% damage.</summary>
        [GlyphData(15, 40897, 54811, "Glyph of Maul", GlyphType.Major,
            @"Your Maul ability now hits 1 additional target for 50% damage.")]
        public bool GlyphofMaul { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Increases the health gain from Might of Ursoc by 20%, but increases the cooldown by 2.0 min.</summary>
        [GlyphData(16, 45603, 116238, "Glyph of Might of Ursoc", GlyphType.Major,
            @"Increases the health gain from Might of Ursoc by 20%, but increases the cooldown by 2.0 min.")]
        public bool GlyphofMightofUrsoc { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Reduces the cooldown of Nature's Grasp by 30 sec.</summary>
        [GlyphData(17, 40922, 116203, "Glyph of Nature's Grasp", GlyphType.Major,
            @"Reduces the cooldown of Nature's Grasp by 30 sec.")]
        public bool GlyphofNaturesGrasp { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the range of your Pounce by 3 yards.</summary>
        [GlyphData(18, 40903, 54821, "Glyph of Pounce", GlyphType.Major,
            @"Increases the range of your Pounce by 3 yards.")]
        public bool GlyphofPounce { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Reduces the movement penalty of Prowl by 100%.</summary>
        [GlyphData(19, 40902, 116186, "Glyph of Prowl", GlyphType.Major,
            @"Reduces the movement penalty of Prowl by 100%.")]
        public bool GlyphofProwl { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Players resurrected by Rebirth are returned to life with 100% health.</summary>
        [GlyphData(20, 40909, 54733, "Glyph of Rebirth", GlyphType.Major,
            @"Players resurrected by Rebirth are returned to life with 100% health.")]
        public bool GlyphofRebirth { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Increases the critical strike chance of your Regrowth by 40%, but removes the periodic component of the spell.</summary>
        [GlyphData(21, 40912, 116218, "Glyph of Regrowth", GlyphType.Major,
            @"Increases the critical strike chance of your Regrowth by 40%, but removes the periodic component of the spell.")]
        public bool GlyphofRegrowth { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>/// <summary>Increases the damage done by Starfall by 10%, but decreases its radius by 50%.</summary>
        [GlyphData(22, 40913, 17076, "Glyph of Rejuvenation", GlyphType.Major,
            @"When you have Rejuvenation active on three or more targets, the cast time of your Nourish spell is reduced by 30%.")]
        public bool GlyphofRejuvenation { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Savage Roar can now be used with 0 combo points, resulting in a 12 sec duration.</summary>
        [GlyphData(23, 40923, 127540, "Glyph of Savagery", GlyphType.Major,
            @"Savage Roar can now be used with 0 combo points, resulting in a 12 sec duration.")]
        public bool GlyphofSavagery { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>While Berserk or Tiger's Fury is active, Shred has no positional requirement.</summary>
        [GlyphData(24, 40901, 114234, "Glyph of Shred", GlyphType.Major,
            @"While Berserk or Tiger's Fury is active, Shred has no positional requirement.")]
        public bool GlyphofShred { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Increases the duration of your Skull Bash interrupt by 4 sec, but increases the cooldown by 10 sec.</summary>
        [GlyphData(25, 40921, 116216, "Glyph of Skull Bash", GlyphType.Major,
            @"Increases the duration of your Skull Bash interrupt by 4 sec, but increases the cooldown by 10 sec.")]
        public bool GlyphofSkullBash { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Increases the duration of your Solar Beam silence effect by 5 sec.</summary>
        [GlyphData(26, 40899, 54812, "Glyph of Solar Beam", GlyphType.Major,
            @"Increases the duration of your Solar Beam silence effect by 5 sec.")]
        public bool GlyphofSolarBeam { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>You can now cast Stampeding Roar without being in Bear Form or Cat Form.</summary>
        [GlyphData(27, 40906, 114300, "Glyph of Stampede", GlyphType.Major,
            @"You can now cast Stampeding Roar without being in Bear Form or Cat Form.")]
        public bool GlyphofStampede { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Increases the radius of Stampeding Roar by 30 yards.</summary>
        [GlyphData(28, 45604, 114222, "Glyph of Stampeding Roar", GlyphType.Major,
            @"Increases the radius of Stampeding Roar by 30 yards.")]
        public bool GlyphofStampedingRoar { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Reduces the cooldown of Survival Instincts by 60 sec, but reduces its duration by 50%.</summary>
        [GlyphData(29, 45601, 114223, "Glyph of Survival Instincts", GlyphType.Major,
            @"Reduces the cooldown of Survival Instincts by 60 sec, but reduces its duration by 50%.")]
        public bool GlyphofSurvivalInstincts { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>You can now cast Healing Touch, Rejuvenation, Rebirth, and Tranquility without cancelling Moonkin Form.</summary>
        [GlyphData(30, 40916, 116209, "Glyph of the Moonbeast", GlyphType.Major,
            @"You can now cast Healing Touch, Rejuvenation, Rebirth, and Tranquility without cancelling Moonkin Form.")]
        public bool GlyphofTheMoonbeast { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Wild Growth can affect 1 additional target, but its cooldown is increased by 2 sec.</summary>
        [GlyphData(31, 45602, 45602, "Glyph of Wild Growth", GlyphType.Major,
            @"Wild Growth can affect 1 additional target, but its cooldown is increased by 2 sec.")]
        public bool GlyphofWildGrowth { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 32; } }
        /// <summary>Increases your swim speed by 50% while in Aquatic Form.</summary>
        [GlyphData(32, 43316, 57856, "Glyph of Aquatic Form", GlyphType.Minor,
            @"Increases your swim speed by 50% while in Aquatic Form.")]
        public bool GlyphofAquaticForm { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Teaches you the ability Charm Woodland Creature.\n\n
        /// Allows the Druid to befriend an ambient creature, which will follow the Druid for 1 hour.</summary>
        [GlyphData(33, 43335, 57855, "Glyph of Charm Woodland Creature", GlyphType.Minor,
            @"Teaches you the ability Charm Woodland Creature.\n\n
              Allows the Druid to befriend an ambient creature, which will follow the Druid for 1 hour.")]
        public bool GlyphofCharmWoodlandCreature { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        /// <summary>Feline Grace reduces falling damage even while not in Cat Form.</summary>
        [GlyphData(34, 43332, 114295, "Glyph of Grace", GlyphType.Minor,
            @"Feline Grace reduces falling damage even while not in Cat Form.")]
        public bool GlyphofGrace { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        /// <summary>Your Moonkin Form now appears as Astral Form, conferring all the same benefits, but appearing as an astrally enhanced version of your normal humanoid form.</summary>
        [GlyphData(35, 44922, 57858, "Glyph of Stars", GlyphType.Minor,
            @"Your Moonkin Form now appears as Astral Form, conferring all the same benefits, but appearing as an astrally enhanced version of your normal humanoid form.")]
        public bool GlyphofStars { get { return _glyphData[35]; } set { _glyphData[35] = value; } }
        /// <summary>Each time you shapeshift into Cat Form or Bear Form, your shapeshifted form will have a random hair color.</summary>
        [GlyphData(36, 43334, 107059, "Glyph of the Chameleon", GlyphType.Minor,
            @"Each time you shapeshift into Cat Form or Bear Form, your shapeshifted form will have a random hair color.")]
        public bool GlyphoftheChameleon { get { return _glyphData[36]; } set { _glyphData[36] = value; } }
        /// <summary>Your Aquatic Form now appears as an Orca.</summary>
        [GlyphData(37, 40919, 114333, "Glyph of the Orca", GlyphType.Minor,
            @"Your Aquatic Form now appears as an Orca.")]
        public bool GlyphoftheOrca { get { return _glyphData[37]; } set { _glyphData[37] = value; } }
        /// <summary>Your Track Humanoids ability now also tracks beasts.</summary>
        [GlyphData(38, 67486, 114280, "Glyph of the Predator", GlyphType.Minor,
            @"Your Track Humanoids ability now also tracks beasts.")]
        public bool GlyphofthePredator { get { return _glyphData[38]; } set { _glyphData[38] = value; } }
        /// <summary>Your Travel Form now appears as a Stag and can be used as a mount by party members.</summary>
        [GlyphData(39, 40900, 114338, "Glyph of the Stag", GlyphType.Minor,
            @"Your Travel Form now appears as a Stag and can be used as a mount by party members.")]
        public bool GlyphoftheStag { get { return _glyphData[39]; } set { _glyphData[39] = value; } }
        /// <summary>Teaches you the ability Treant Form.\n\n
        /// Shapeshift into Treant Form.\n\n
        /// The act of shapeshifting frees the caster of movement impairing effects.</summary>
        [GlyphData(40, 68039, 57857, "Glyph of the Treant", GlyphType.Minor,
            @"Teaches you the ability Treant Form.\n\n
              Shapeshift into Treant Form.\n\n
              The act of shapeshifting frees the caster of movement impairing effects.")]
        public bool GlyphoftheTreant { get { return _glyphData[40]; } set { _glyphData[40] = value; } }
        #endregion
        #region Discontinued
        public bool GlyphOfTigersFury { get; set; }
        public bool GlyphOfBarkskin { get; set; }
        public bool GlyphOfMangle { get; set; }
        public bool GlyphOfLacerate { get; set; }
        public bool GlyphOfBerserk { get; set; }
        public bool GlyphOfFrenziedRegeneration { get; set; }
        public bool GlyphOfFeralCharge { get; set; }
        public bool GlyphOfSavageRoar { get; set; }
        public bool GlyphOfBloodletting { get; set; }
        public bool GlyphOfRip { get; set; }
        #endregion
    }

    public partial class PaladinTalents
    {
        private bool[] _glyphData = new bool[40];
        public override bool[] GlyphData { get { return _glyphData; } }

        public override int GlyphTreeStartingIndexes_0 { get { return -1; } }
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 0; } }
        [GlyphData(0, 41092, 54922, "Glyph of Double Jeopardy", GlyphType.Major, @"Judging a target increases the damage of your next Judgment by 20%, but only if used on a different second target.")]
        public bool GlyphOfDoubleJeopardy { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(1, 41094, 54925, "Glyph of Rebuke", GlyphType.Major, @"Increases the cooldown by 5 sec and lockout by 2 sec of Rebuke.")]
        public bool GlyphOfRebuke { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(2, 41095, 54923, "Glyph of Holy Wrath", GlyphType.Major, @"Your Holy Wrath now also stuns Elementals and Dragonkin.")]
        public bool GlyphOfHolyWrath { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData(3, 41096, 54924, "Glyph of Divine Protection", GlyphType.Major, @"Reduces the magical damage reduction of your Divine Protection to 20% but adds 20% physical damage reduction.")]
        public bool GlyphOfDivineProtection { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(4, 41097, 54926, "Glyph of Templar's Verdict", GlyphType.Major, @"You take 10% less damage for 6 sec after dealing damage with Templar's Verdict.")]
        public bool GlyphOfTemplarsVerdict { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(5, 41098, 54927, "Glyph of Avenging Wrath", GlyphType.Major, @"While Avenging Wrath is active, you are healed for 1% of your maximum health every 2 sec.")]
        public bool GlyphOfAvengingWrath { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData(6, 41099, 54928, "Glyph of Consecration", GlyphType.Major, @"You can now target Consecration anywhere within 25 yards.")]
        public bool GlyphOfConsecration { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(7, 41101, 54930, "Glyph of Focused Shield", GlyphType.Major, @"Your Avenger's Shield hits 2 fewer targets, but for 30% more damage.")]
        public bool GlyphOfFocusedShield { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, 41102, 54931, "Glyph of Turn Evil", GlyphType.Major, @"Reduces the casting time of your Turn Evil spell by 100%, but increases the cooldown by 8 sec.")]
        public bool GlyphOfTurnEvil { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData(9, 41103, 54934, "Glyph of Blinding Light", GlyphType.Major, @"Your Blinding Light now knocks down targets for 3 sec instead of Blinding them.")]
        public bool GlyphOfBlindingLight { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(10, 41104, 54935, "Glyph of Final Wrath", GlyphType.Major, @"Your Holy Wrath does an additional 50% damage to targets with less than 20% health.")]
        public bool GlyphOfFinalWrath { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(11, 41105, 54936, "Glyph of Word of Glory", GlyphType.Major, @"Increases your damage by 3% per Holy Power spent after you cast Word of Glory or Eternal Flame. Lasts 6 sec.")]
        public bool GlyphOfWordOfGlory { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(12, 41106, 54937, "Glyph of Illumination", GlyphType.Major, @"Your Holy Shock criticals grant 1% mana return, but Holy Insight returns 10% less mana.")]
        public bool GlyphOfIllumination { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(13, 41107, 54938, "Glyph of Harsh Words", GlyphType.Major, @"Your Word of Glory can now also be used on enemy targets, causing Holy damage approximately equal to the amount it would have healed.")]
        public bool GlyphOfHarshWords { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, 41108, 54939, "Glyph of Divinity", GlyphType.Major, @"Increases the cooldown of your Lay on Hands by 2 min but causes it to give you 10% of your maximum mana.")]
        public bool GlyphOfDivinity { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(15, 41109, 54940, "Glyph of Light of Dawn", GlyphType.Major, @"Light of Dawn affects 2 fewer targets, but heals each target for 25% more.")]
        public bool GlyphOfLightOfDawn { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(16, 41110, 54943, "Glyph of Blessed Life", GlyphType.Major, @"While Seal of Insight is active, you have a 50% chance to gain a charge of Holy Power whenever you are affected by a Stun, Fear or Immobilize effect.")]
        public bool GlyphOfBlessedLife { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(17, 43367, 57955, "Glyph of Flash of Light", GlyphType.Major, @"When you Flash of Light a target, it increases your next heal done to that target within 7 sec by 10%.")]
        public bool GlyphOfFlashOfLight { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(18, 43867, 56420, "Glyph of Denounce", GlyphType.Major, @"Your Holy Shocks have a 50% chance to reduce the cast time of your next Denounce by 1.0 sec.")]
        public bool GlyphOfDenounce { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(19, 43868, 56414, "Glyph of Dazing Shield", GlyphType.Major, @"Your Avenger's Shield now also dazes targets for 10 sec.")]
        public bool GlyphOfDazingShield { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        [GlyphData(20, 43869, 56416, "Glyph of Immediate Truth", GlyphType.Major, @"Increases the instant damage done by Seal of Truth by 30%, but decreases the damage done by Censure by 50%.")]
        public bool GlyphOfImmediateTruth { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(21, 45741, 63218, "Glyph of Beacon of Light", GlyphType.Major, @"Removes the global cooldown on Beacon of Light.")]
        public bool GlyphOfBeaconOfLight { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(22, 45742, 63219, "Glyph of Hammer of the Righteous", GlyphType.Major, @"The physical damage reduction caused by Hammer of the Righteous now lasts 50% longer.")]
        public bool GlyphOfHammerOfTheRighteous { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        [GlyphData(23, 45743, 63220, "Glyph of Divine Storm", GlyphType.Major, @"Your Divine Storm also heals you for 5% of your maximum health.")]
        public bool GlyphOfDivineStorm { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(24, 45744, 63222, "Glyph of the Alabaster Shield", GlyphType.Major, @"Your successful blocks increase the damage of your next Shield of the Righteous by 20%. Stacks up to 3 times.")]
        public bool GlyphOfTheAlabasterShield { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        [GlyphData(25, 45745, 63223, "Glyph of Divine Plea", GlyphType.Major, @"Divine Plea now has a 5 sec cast time, but you receive 12% of your total mana instantly and your healing is not reduced.")]
        public bool GlyphOfDivinePlea { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(26, 45746, 63224, "Glyph of Holy Shock", GlyphType.Major, @"Decreases the healing of Holy Shock by 50% but increases its damage by 50%.")]
        public bool GlyphOfHolyShock { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        [GlyphData(27, 45747, 63225, "Glyph of Inquisition", GlyphType.Major, @"Reduces the damage bonus of Inquisition by 15% but increases its duration by 100%.")]
        public bool GlyphOfInquisition { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        [GlyphData(28, 66918, 93466, "Glyph of Protector of the Innocent", GlyphType.Major, @"When you use Word of Glory to heal another target, it also heals you for 20% of the amount.")]
        public bool GlyphOfProtectorOfTheInnocent { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        [GlyphData(29, 81956, 119477, "Glyph of the Battle Healer", GlyphType.Major, @"Using melee attacks while using Seal of Insight heals a nearby injured friendly target, excluding the Paladin, within 30 yards for 30% of damage dealt.")]
        public bool GlyphOfTheBattleHealer { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        [GlyphData(30, 83107, 122028, "Glyph of Mass Exorcism", GlyphType.Major, @"Reduces the range of Exorcism to melee range, but causes 25% damage to all enemies within 8 yards of the primary target.")]
        public bool GlyphOfMassExorcism { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 31; } }
        [GlyphData(31, 41100, 89401, "Glyph of the Luminous Charger", GlyphType.Minor, @"Your Paladin class mounts glow with holy light.")]
        public bool GlyphOfTheLuminousCharger { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        [GlyphData(32, 43340, 57958, "Glyph of the Mounted King", GlyphType.Minor, @"Mounting one of your Paladin class mounts automatically casts Blessing of Kings on you.")]
        public bool GlyphOfTheMountedKing { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        [GlyphData(33, 43365, 125043, "Glyph of Contemplation", GlyphType.Minor, @"Teaches you the ability Contemplation.")]
        public bool GlyphOfContemplation { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        [GlyphData(34, 43366, 57979, "Glyph of Winged Vengeance", GlyphType.Minor, @"Your Avenging Wrath depicts 4 wings.")]
        public bool GlyphOfWingedVengeance { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        [GlyphData(35, 43368, 57947, "Glyph of Seal of Blood", GlyphType.Minor, @"Your Seal of Truth now uses the Seal of Blood visual.")]
        public bool GlyphOfSealOfBlood { get { return _glyphData[35]; } set { _glyphData[35] = value; } }
        [GlyphData(36, 43369, 57954, "Glyph of Fire From the Heavens", GlyphType.Minor, @"Your Judgment and Hammer of Wrath criticals call down fire from the sky.")]
        public bool GlyphOfFireFromTheHeavens { get { return _glyphData[36]; } set { _glyphData[36] = value; } }
        [GlyphData(37, 80584, 115931, "Glyph of the Falling Avenger", GlyphType.Minor, @"You slow fall during Avenging Wrath.")]
        public bool GlyphOfTheFallingAvenger { get { return _glyphData[37]; } set { _glyphData[37] = value; } }
        [GlyphData(38, 80585, 115933, "Glyph of the Righteous Retreat", GlyphType.Minor, @"During Divine Shield, you can invoke your Hearthstone 50% faster.")]
        public bool GlyphOfTheRighteousRetreat { get { return _glyphData[38]; } set { _glyphData[38] = value; } }
        [GlyphData(39, 80586, 115934, "Glyph of Bladed Judgment", GlyphType.Minor, @"Your Judgment spell depicts an axe or sword instead of a hammer, if you have an axe or sword equipped.")]
        public bool GlyphOfBladedJudgment { get { return _glyphData[39]; } set { _glyphData[39] = value; } }
        #endregion
        #region Discontinued
        public bool GlyphOfCrusaderStrike { get; set; }
        public bool GlyphOfDivineFavor { get; set; }
        public bool GlyphOfExorcism { get; set; }
        public bool GlyphOfJudgement { get; set; }
        public bool GlyphOfSealOfInsight { get; set; }
        public bool GlyphOfSealOfTruth { get; set; }
        public bool GlyphOfShieldOfTheRighteous { get; set; }
        public bool GlyphOfCleansing { get; set; }
        public bool GlyphOfHammerOfJustice { get; set; }
        public bool GlyphOfHammerOfWrath { get; set; }
        public bool GlyphOfLayOnHands { get; set; }
        public bool GlyphOfSalvation { get; set; }
        public bool GlyphOfAsceticCrusader { get; set; }
        public bool GlyphOfTheLongWord { get; set; }
        public bool GlyphOfBlessingOfKings { get; set; }
        public bool GlyphOfBlessingOfMight { get; set; }
        public bool GlyphOfInsight { get; set; }
        public bool GlyphOfJustice { get; set; }
        public bool GlyphOfRighteousness { get; set; }
        public bool GlyphOfTruth { get; set; }
        #endregion
    }

    public partial class ShamanTalents
    {
        private bool[] _glyphData = new bool[35];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the amount healed by your Earth Shield by 20%.</summary>
        [GlyphData(0, 45775, 63279, "Glyph of Earth Shield", GlyphType.Prime,
            @"Increases the amount healed by your Earth Shield by 20%.")]
        public bool GlyphofEarthShield { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the effectiveness of your Earthliving Weapon's periodic healing by 20%.</summary>
        [GlyphData(1, 41527, 55439, "Glyph of Earthliving Weapon", GlyphType.Prime,
            @"Increases the effectiveness of your Earthliving Weapon's periodic healing by 20%.")]
        public bool GlyphofEarthlivingWeapon { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Your spirit wolves gain an additional 30% of your attack power.</summary>
        [GlyphData(2, 45771, 63271, "Glyph of Feral Spirit", GlyphType.Prime,
            @"Your spirit wolves gain an additional 30% of your attack power.")]
        public bool GlyphofFeralSpirit { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Reduces the cooldown of your Fire Elemental Totem by 5 min.</summary>
        [GlyphData(3, 41529, 55455, "Glyph of Fire Elemental Totem", GlyphType.Prime,
            @"Reduces the cooldown of your Fire Elemental Totem by 5 min.")]
        public bool GlyphofFireElementalTotem { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the duration of your Flame Shock by 50%.</summary>
        [GlyphData(4, 41531, 55447, "Glyph of Flame Shock", GlyphType.Prime,
            @"Increases the duration of your Flame Shock by 50%.")]
        public bool GlyphofFlameShock { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Increases spell critical strike chance by 2% while Flametongue Weapon is active.</summary>
        [GlyphData(5, 41532, 55451, "Glyph of Flametongue Weapon", GlyphType.Prime,
            @"Increases spell critical strike chance by 2% while Flametongue Weapon is active.")]
        public bool GlyphofFlametongueWeapon { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Your Lava Burst spell deals 10% more damage.</summary>
        [GlyphData(6, 41524, 55454, "Glyph of Lava Burst", GlyphType.Prime,
            @"Your Lava Burst spell deals 10% more damage.")]
        public bool GlyphofLavaBurst { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage dealt by your Lava Lash ability by 20%.</summary>
        [GlyphData(7, 41540, 55444, "Glyph of Lava Lash", GlyphType.Prime,
            @"Increases the damage dealt by your Lava Lash ability by 20%.")]
        public bool GlyphofLavaLash { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the damage dealt by Lightning Bolt by 4%.</summary>
        [GlyphData(8, 41536, 55453, "Glyph of Lightning Bolt", GlyphType.Prime,
            @"Increases the damage dealt by Lightning Bolt by 4%.")]
        public bool GlyphofLightningBolt { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the duration of Riptide by 40%.</summary>
        [GlyphData(9, 45772, 63273, "Glyph of Riptide", GlyphType.Prime,
            @"Increases the duration of Riptide by 40%.")]
        public bool GlyphofRiptide { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Reduces the global cooldown triggered by your shock spells to 1 sec.</summary>
        [GlyphData(10, 41526, 55442, "Glyph of Shocking", GlyphType.Prime,
            @"Reduces the global cooldown triggered by your shock spells to 1 sec.")]
        public bool GlyphofShocking { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Increases the critical strike chance bonus from your Stormstrike ability by an additional 10%.</summary>
        [GlyphData(11, 41539, 55446, "Glyph of Stormstrike", GlyphType.Prime,
            @"Increases the critical strike chance bonus from your Stormstrike ability by an additional 10%.")]
        public bool GlyphofStormstrike { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Increases the passive mana regeneration of your Water Shield spell by 50%.</summary>
        [GlyphData(12, 41541, 55436, "Glyph of Water Shield", GlyphType.Prime,
            @"Increases the passive mana regeneration of your Water Shield spell by 50%.")]
        public bool GlyphofWaterShield { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases the chance per swing for Windfury Weapon to trigger by 2%.</summary>
        [GlyphData(13, 41542, 55445, "Glyph of Windfury Weapon", GlyphType.Prime,
            @"Increases the chance per swing for Windfury Weapon to trigger by 2%.")]
        public bool GlyphofWindfuryWeapon { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 14; } }

        /// <summary>Increases healing done by your Chain Heal to targets beyond the first by 15%, but decreases the amount received by the initial target by 10%.</summary>
        [GlyphData(14, 41517, 55437, "Glyph of Chain Heal", GlyphType.Major,
            @"Increases healing done by your Chain Heal to targets beyond the first by 15%, but decreases the amount received by the initial target by 10%.")]
        public bool GlyphofChainHeal { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Your Chain Lightning spell now strikes 2 additional targets, but deals 10% less initial damage.</summary>
        [GlyphData(15, 41518, 55449, "Glyph of Chain Lightning", GlyphType.Major,
            @"Your Chain Lightning spell now strikes 2 additional targets, but deals 10% less initial damage.")]
        public bool GlyphofChainLightning { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>While your Elemental Mastery ability is active, you take 20% less damage from all sources.</summary>
        [GlyphData(16, 41552, 55452, "Glyph of Elemental Mastery", GlyphType.Major,
            @"While your Elemental Mastery ability is active, you take 20% less damage from all sources.")]
        public bool GlyphofElementalMastery { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases the radius of your Fire Nova spell by 5 yards.</summary>
        [GlyphData(17, 41530, 55450, "Glyph of Fire Nova", GlyphType.Major,
            @"Increases the radius of your Fire Nova spell by 5 yards.")]
        public bool GlyphofFireNova { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the duration of your Frost Shock by 2 sec.</summary>
        [GlyphData(18, 41547, 55443, "Glyph of Frost Shock", GlyphType.Major,
            @"Increases the duration of your Frost Shock by 2 sec.")]
        public bool GlyphofFrostShock { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Your Ghost Wolf form grants an additional 5% movement speed.</summary>
        [GlyphData(19, 43725, 59289, "Glyph of Ghost Wolf", GlyphType.Major,
            @"Your Ghost Wolf form grants an additional 5% movement speed.")]
        public bool GlyphofGhostWolf { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Instead of absorbing a spell, your Grounding Totem reflects the next harmful spell back at its caster, but the cooldown of your Grounding Totem is increased by 35 sec.</summary>
        [GlyphData(20, 41538, 55441, "Glyph of Grounding Totem", GlyphType.Major,
            @"Instead of absorbing a spell, your Grounding Totem reflects the next harmful spell back at its caster, but the cooldown of your Grounding Totem is increased by 35 sec.")]
        public bool GlyphofGroundingTotem { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Your Healing Stream Totem increases all the resistances of nearby party and raid members within 30 yards  by 130.</summary>
        [GlyphData(21, 41533, 55456, "Glyph of Healing Stream Totem", GlyphType.Major,
            @"Your Healing Stream Totem increases all the resistances of nearby party and raid members within 30 yards  by 130.")]
        public bool GlyphofHealingStreamTotem { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Your Healing Wave also heals you for 20% of the healing effect when you heal someone else.</summary>
        [GlyphData(22, 41534, 55440, "Glyph of Healing Wave", GlyphType.Major,
            @"Your Healing Wave also heals you for 20% of the healing effect when you heal someone else.")]
        public bool GlyphofHealingWave { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Reduces the cooldown on your Hex spell by 10 seconds.</summary>
        [GlyphData(23, 45777, 63291, "Glyph of Hex", GlyphType.Major,
            @"Reduces the cooldown on your Hex spell by 10 seconds.")]
        public bool GlyphofHex { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Your Lightning Shield can no longer drop below 3 charges from dealing damage to attackers.</summary>
        [GlyphData(24, 41537, 55448, "Glyph of Lightning Shield", GlyphType.Major,
            @"Your Lightning Shield can no longer drop below 3 charges from dealing damage to attackers.")]
        public bool GlyphofLightningShield { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Activating your Shamanistic Rage ability also cleanses you of all dispellable Magic debuffs.</summary>
        [GlyphData(25, 45776, 63280, "Glyph of Shamanistic Rage", GlyphType.Major,
            @"Activating your Shamanistic Rage ability also cleanses you of all dispellable Magic debuffs.")]
        public bool GlyphofShamanisticRage { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Your Stoneclaw Totem also places a damage absorb shield on you, equal to 4 times the strength of the shield it places on your totems.</summary>
        [GlyphData(26, 45778, 63298, "Glyph of Stoneclaw Totem", GlyphType.Major,
            @"Your Stoneclaw Totem also places a damage absorb shield on you, equal to 4 times the strength of the shield it places on your totems.")]
        public bool GlyphofStoneclawTotem { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Reduces the cooldown on Thunderstorm by 10 sec.</summary>
        [GlyphData(27, 45770, 63270, "Glyph of Thunder", GlyphType.Major,
            @"Reduces the cooldown on Thunderstorm by 10 sec.")]
        public bool GlyphofThunder { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Causes your Totemic Recall ability to return an additional 50% of the mana cost of any recalled totems.</summary>
        [GlyphData(28, 41535, 55438, "Glyph of Totemic Recall", GlyphType.Major,
            @"Causes your Totemic Recall ability to return an additional 50% of the mana cost of any recalled totems.")]
        public bool GlyphofTotemicRecall { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 29; } }
        /// <summary>Reduces the cooldown of your Astral Recall spell by 7.5 min.</summary>
        [GlyphData(29, 43381, 58058, "Glyph of Astral Recall", GlyphType.Minor,
            @"Reduces the cooldown of your Astral Recall spell by 7.5 min.")]
        public bool GlyphofAstralRecall { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Your Reincarnation spell no longer requires a reagent.</summary>
        [GlyphData(30, 43385, 58059, "Glyph of Renewed Life", GlyphType.Minor,
            @"Your Reincarnation spell no longer requires a reagent.")]
        public bool GlyphofRenewedLife { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Alters the appearance of your Ghost Wolf transformation, causing it to resemble an arctic wolf.</summary>
        [GlyphData(31, 43386, 58135, "Glyph of the Arctic Wolf", GlyphType.Minor,
            @"Alters the appearance of your Ghost Wolf transformation, causing it to resemble an arctic wolf.")]
        public bool GlyphofArcticWolf { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Increases the mana you recieve from your Thunderstorm spell by 2%, but it no longer knocks enemies back.</summary>
        [GlyphData(32, 44923, 62132, "Glyph of Thunderstorm", GlyphType.Minor,
            @"Increases the mana you recieve from your Thunderstorm spell by 2%, but it no longer knocks enemies back.")]
        public bool GlyphofThunderstorm { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Your Water Breathing Spell no longer reuires a reagent.</summary>
        [GlyphData(33, 43344, 89646, "Glyph of Water Breathing", GlyphType.Minor,
            @"Your Water Breathing Spell no longer reuires a reagent.")]
        public bool GlyphofWaterBreathing { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        /// <summary>Your Water Walking spell no longer quires a reagent.</summary>
        [GlyphData(34, 43388, 58057, "Glyph of Water Walking", GlyphType.Minor,
            @"Your Water Walking spell no longer quires a reagent.")]
        public bool GlyphofWaterWalking { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        #endregion
    }

    public partial class PriestTalents
    {
        private bool[] _glyphData = new bool[35];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Reduces the cooldown of your Dispersion by 45 sec.</summary>
        [GlyphData(0, 45753, 63229, "Glyph of Dispersion", GlyphType.Prime,
            @"Reduces the cooldown of your Dispersion by 45 sec.")]
        public bool GlyphofDispersion { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the critical chance of your Flash Heal on targets below 25% health by 10%.</summary>
        [GlyphData(1, 42400, 55679, "Glyph of Flash Heal", GlyphType.Prime,
            @"Increases the critical chance of your Flash Heal on targets below 25% health by 10%.")]
        public bool GlyphofFlashHeal { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Reduces the cooldown of Guardian Spirit by 30 sec.</summary>
        [GlyphData(2, 45755, 63231, "Glyph of Guardian Spirit", GlyphType.Prime,
            @"Reduces the cooldown of Guardian Spirit by 30 sec.")]
        public bool GlyphofGuardianSpirit { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the total amount of charges on your Lightwell by 5.</summary>
        [GlyphData(3, 42403, 55673, "Glyph of Lightwell", GlyphType.Prime,
            @"Increases the total amount of charges on your Lightwell by 5.")]
        public bool GlyphofLightwell { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage done by your Mind Flay spell by 10%.</summary>
        [GlyphData(4, 42415, 55687, "Glyph of Mind Flay", GlyphType.Prime,
            @"Increases the damage done by your Mind Flay spell by 10%.")]
        public bool GlyphofMindFlay { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Reduces the cooldown of Penance by 2 sec.</summary>
        [GlyphData(5, 45756, 63235, "Glyph of Penance", GlyphType.Prime,
            @"Reduces the cooldown of Penance by 2 sec.")]
        public bool GlyphofPenance { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases the healing received while under the Barrier by 10%.</summary>
        [GlyphData(6, 42407, 55689, "Glyph of Power Word: Barrier", GlyphType.Prime,
            @"Increases the healing received while under the Barrier by 10%.")]
        public bool GlyphofPowerWordBarrier { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Your Power Word: Shield also heals the target for 20% of the absorption amount.</summary>
        [GlyphData(8, 42408, 55672, "Glyph of Power Word: Shield", GlyphType.Prime,
            @"Your Power Word: Shield also heals the target for 20% of the absorption amount.")]
        public bool GlyphofPowerWordShield { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Your Prayer of Healing spell also heals an additional 20% of its initial heal over 6 sec.</summary>
        [GlyphData(9, 42409, 55680, "Glyph of Prayer of Healing", GlyphType.Prime,
            @"Your Prayer of Healing spell also heals an additional 20% of its initial heal over 6 sec.")]
        public bool GlyphofPrayerofHealing { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the amount healed by your Renew by an additional 10%.</summary>
        [GlyphData(10, 42411, 55674, "Glyph of Renew", GlyphType.Prime,
            @"Increases the amount healed by your Renew by an additional 10%.")]
        public bool GlyphofRenew { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>If your Shadow Word: Death target fails to kill the target at 25% or less, your Shadow Word: Death cooldown is reset. This effect can not occur more than once every 6 seconds.</summary>
        [GlyphData(12, 42414, 55682, "Glyph of Shadow Word: Death", GlyphType.Prime,
            @"If your Shadow Word: Death target fails to kill the target at 25% or less, your Shadow Word: Death cooldown is reset. This effect can not occur more than once every 6 seconds.")]
        public bool GlyphofShadowWordDeath { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases the periodic damage of your Shadow Word: Pain by 10%.</summary>
        [GlyphData(13, 42406, 55681, "Glyph of Shadow Word: Pain", GlyphType.Prime,
            @"Increases the periodic damage of your Shadow Word: Pain by 10%.")]
        public bool GlyphofShadowWordPain { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 14; } }
        /// <summary></summary>
        [GlyphData(14, 42396, 55675, "Glyph of Circle of Healing", GlyphType.Major,
            @"Your Circle of Healing spell heals 1 additional target.")]
        public bool GlyphofCircleOfHealing { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Allows Pain Suppression and Guardian Spirit to be cast while stunned.</summary>
        [GlyphData(15, 45760, 63248, "Glyph of Desperation", GlyphType.Major,
            @"Allows Pain Suppression and Guardian Spirit to be cast while stunned.")]
        public bool GlyphofDesperation { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Your Dispel Magic spell also heals your target for 3% maximum health when you successfully dispel a magical effect.</summary>
        [GlyphData(16, 42397, 55677, "Glyph of Dispel Magic", GlyphType.Major,
            @"Your Dispel Magic spell also heals your target for 3% maximum health when you successfully dispel a magical effect.")]
        public bool GlyphofDispelMagic { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases your chance to hit with your Smite and Holy Fire by 18%.</summary>
        [GlyphData(17, 45758, 63246, "Glyph of Divine Accuracy", GlyphType.Major,
            @"Increases your chance to hit with your Smite and Holy Fire by 18%.")]
        public bool GlyphofDivineAccuracy { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Reduces the cooldown of your Fade spell by 9 sec.</summary>
        [GlyphData(18, 42398, 55684, "Glyph of Fade", GlyphType.Major,
            @"Reduces the cooldown of your Fade spell by 9 sec.")]
        public bool GlyphofFade { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Reduces the cooldown and duration of Fear Ward by 60 sec.</summary>
        [GlyphData(19, 42399, 55678, "Glyph of Fear Ward", GlyphType.Major,
            @"Reduces the cooldown and duration of Fear Ward by 60 sec.")]
        public bool GlyphofFearWard { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Reduces the global cooldown of your Holy Nova by 0.5 sec.</summary>
        [GlyphData(20, 42401, 55683, "Glyph of Holy Nova", GlyphType.Major,
            @"Reduces the global cooldown of your Holy Nova by 0.5 sec.")]
        public bool GlyphofHolyNova { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Increases the armor from your Inner Fire spell by 50%.</summary>
        [GlyphData(21, 42402, 55686, "Glyph of Inner Fire", GlyphType.Major,
            @"Increases the armor from your Inner Fire spell by 50%.")]
        public bool GlyphofInnerFire { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Reduces the cast time of Mass Dispel by 1 second.</summary>
        [GlyphData(22, 42404, 55691, "Glyph of Mass Dispel", GlyphType.Major,
            @"Reduces the cast time of Mass Dispel by 1 second.")]
        public bool GlyphofMassDispel { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Your first charge of your Prayer of Mending heals for an additional 60%.</summary>
        [GlyphData(23, 42417, 55685, "Glyph of Prayer of Mending", GlyphType.Major,
            @"Your first charge of your Prayer of Mending heals for an additional 60%.")]
        public bool GlyphofPrayerOfMending { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Reduces the cooldown of your Psychic Horror by 30 sec.</summary>
        [GlyphData(24, 42405, 55688, "Glyph of Psychic Horror", GlyphType.Major,
            @"Reduces the cooldown of your Psychic Horror by 30 sec.")]
        public bool GlyphofPsychicHorror { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Targets of your Psychic Scream tremble in place instead of fleeing in fear, but the cooldown of Psychic Scream is increased by 3 sec.</summary>
        [GlyphData(25, 42410, 55676, "Glyph of Psychic Scream", GlyphType.Major,
            @"Targets of your Psychic Scream tremble in place instead of fleeing in fear, but the cooldown of Psychic Scream is increased by 3 sec.")]
        public bool GlyphofPsychicScream { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Reduces the cast time of your Shackle Undead by 1.0 sec.</summary>
        [GlyphData(26, 42412, 55690, "Glyph of Scourge Imprisonment", GlyphType.Major,
            @"Reduces the cast time of your Shackle Undead by 1.0 sec.")]
        public bool GlyphofScourgeImprisonment { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Your Smite spell inflicts an additional 20% damage against targets afflicted by Holy Fire.</summary>
        [GlyphData(27, 42416, 55692, "Glyph of Smite", GlyphType.Major,
            @"Your Smite spell inflicts an additional 20% damage against targets afflicted by Holy Fire.")]
        public bool GlyphofSmite { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>When you kill a target with your Shadow Word: Death and yield experience or honor, you instantly receive 12% of your total mana over 12 sec.</summary>
        [GlyphData(28, 45757, 63237, "Glyph of Spirit Tap", GlyphType.Major,
            @"When you kill a target with your Shadow Word: Death and yield experience or honor, you instantly receive 12% of your total mana over 12 sec.")]
        public bool GlyphofSpiritTap { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 29; } }
        /// <summary>Reduces the mana cost of your Fade spell by 30%.</summary>
        [GlyphData(29, 43342, 57985, "Glyph of Fading", GlyphType.Minor,
            @"Reduces the mana cost of your Fade spell by 30%.")]
        public bool GlyphofFading { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Reduces the mana cost of your Power Word: Fortitude by 50%.</summary>
        [GlyphData(30, 43371, 58009, "Glyph of Fortitude", GlyphType.Minor,
            @"Reduces the mana cost of your Power Word: Fortitude by 50%.")]
        public bool GlyphofFortitude { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Your Levitate spell no longer requires a reagent.</summary>
        [GlyphData(31, 43370, 57987, "Glyph of Levitate", GlyphType.Minor,
            @"Your Levitate spell no longer requires a reagent.")]
        public bool GlyphofLevitate { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Increases the range of your Shackle Undead spell by 5 yards.</summary>
        [GlyphData(32, 43373, 57986, "Glyph of Shackle Undead", GlyphType.Minor,
            @"Increases the range of your Shackle Undead spell by 5 yards.")]
        public bool GlyphofShackleUndead { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Increases the duration of your Shadow Protection and Prayer of Shadow Protection spells by 10 min.</summary>
        [GlyphData(33, 43372, 58015, "Glyph of Shadow Protection", GlyphType.Minor,
            @"Increases the duration of your Shadow Protection and Prayer of Shadow Protection spells by 10 min.")]
        public bool GlyphofShadowProtection { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        /// <summary>Receive 5% of your maximum mana if your Shadowfiend dies from damage.</summary>
        [GlyphData(34, 43374, 58228, "Glyph of Shadowfiend", GlyphType.Minor,
            @"Receive 5% of your maximum mana if your Shadowfiend dies from damage.")]
        public bool GlyphofShadowfiend { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        #endregion

    }

    public partial class DeathKnightTalents
    {
        private bool[] _glyphData = new bool[30];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the duration of your Death and Decay spell by 50%.</summary>
        [GlyphData(0, 43542, 58629, "Glyph of Death and Decay", GlyphType.Prime,
            @"Increases the duration of your Death and Decay spell by 50%.")]
        public bool GlyphofDeathandDecay { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the damage or healing done by Death Coil by 15%.</summary>
        [GlyphData(1, 45804, 63333, "Glyph of Death Coil", GlyphType.Prime,
            @"Increases the damage or healing done by Death Coil by 15%.")]
        public bool GlyphofDeathCoil { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases your Death Strike's damage by 2% for every 5 runic power you currently have (up to a maximum of 40%). The runic power is not consumed by this effect.</summary>
        [GlyphData(2, 43827, 59336, "Glyph of Death Strike", GlyphType.Prime,
            @"Increases your Death Strike's damage by 2% for every 5 runic power you currently have (up to a maximum of 40%). The runic power is not consumed by this effect.")]
        public bool GlyphofDeathStrike { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Reduces the cost of your Frost Strike by 8 Runic Power.</summary>
        [GlyphData(3, 43543, 58647, "Glyph of Frost Strike", GlyphType.Prime,
            @"Reduces the cost of your Frost Strike by 8 Runic Power.")]
        public bool GlyphofFrostStrike { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage of your Heart Strike ability by 30%.</summary>
        [GlyphData(4, 43534, 58616, "Glyph of Heart Strike", GlyphType.Prime,
            @"Increases the damage of your Heart Strike ability by 30%.")]
        public bool GlyphofHeartStrike { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Your Howling Blast ability now infects your targets with Frost Fever.</summary>
        [GlyphData(5, 45806, 63335, "Glyph of Howling Blast", GlyphType.Prime,
            @"Your Howling Blast ability now infects your targets with Frost Fever.")]
        public bool GlyphofHowlingBlast { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Your Frost Fever disease deals 20% additional damage.</summary>
        [GlyphData(6, 43546, 58631, "Glyph of Icy Touch", GlyphType.Prime,
            @"Your Frost Fever disease deals 20% additional damage.")]
        public bool GlyphofIcyTouch { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage of your Obliterate ability by 20%.</summary>
        [GlyphData(7, 43547, 58671, "Glyph of Obliterate", GlyphType.Prime,
            @"Increases the damage of your Obliterate ability by 20%.")]
        public bool GlyphofObliterate { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Your Ghoul receives an additional 40% of your Strength and 40% of your Stamina.</summary>
        [GlyphData(8, 43549, 58686, "Glyph of Raise Dead", GlyphType.Prime,
            @"Your Ghoul receives an additional 40% of your Strength and 40% of your Stamina.")]
        public bool GlyphofRaiseDead { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the critical strike chance of your Rune Strike by 10%.</summary>
        [GlyphData(9, 43550, 58669, "Glyph of Rune Strike", GlyphType.Prime,
            @"Increases the critical strike chance of your Rune Strike by 10%.")]
        public bool GlyphofRuneStrike { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the Shadow damage portion of your Scourge Strike by 30%.</summary>
        [GlyphData(10, 43551, 58642, "Glyph of Scourge Strike", GlyphType.Prime,
            @"Increases the Shadow damage portion of your Scourge Strike by 30%.")]
        public bool GlyphofScourgeStrike { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 11; } }
        /// <summary>Increases the duration of your Anti-Magic Shell by 2 sec.</summary>
        [GlyphData(11, 43533, 58623, "Glyph of Anti-Magic Shell", GlyphType.Major,
            @"Increases the duration of your Anti-Magic Shell by 2 sec.")]
        public bool GlyphofAntiMagicShell { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Increases the radius of your Blood Boil ability by 50%.</summary>
        [GlyphData(12, 43826, 59332, "Glyph of Blood Boil", GlyphType.Major,
            @"Increases the radius of your Blood Boil ability by 50%.")]
        public bool GlyphofBloodBoil { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases your movement speed by 15% while Bone Shield is active. This does not stack with other movement-speed increasing effects.</summary>
        [GlyphData(13, 43536, 58673, "Glyph of Bone Shield", GlyphType.Major,
            @"Increases your movement speed by 15% while Bone Shield is active. This does not stack with other movement-speed increasing effects.")]
        public bool GlyphofBoneShield { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Your Chains of Ice also causes 144 to 156 Frost damage, increased by your attack power.</summary>
        [GlyphData(14, 43537, 58620, "Glyph of Chains of Ice", GlyphType.Major,
            @"Your Chains of Ice also causes 144 to 156 Frost damage, increased by your attack power.")]
        public bool GlyphofChainsofIce { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Increases your threat generation by 50% while your Dancing Rune Weapon is active.</summary>
        [GlyphData(15, 45799, 63330, "Glyph of Dancing Rune Weapon", GlyphType.Major,
            @"Increases your threat generation by 50% while your Dancing Rune Weapon is active.")]
        public bool GlyphofDancingRuneWeapon { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Causes your Death Strike ability to always restore at least 15% of your maximum health when used while in Frost or Unholy Presence.</summary>
        [GlyphData(16, 68793, 96279, "Glyph of Dark Succor", GlyphType.Major,
            @"Causes your Death Strike ability to always restore at least 15% of your maximum health when used while in Frost or Unholy Presence.")]
        public bool GlyphofDarkSuccor { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases the range of your Death Grip ability by 5 yards.</summary>
        [GlyphData(17, 43541, 62259, "Glyph of Death Grip", GlyphType.Major,
            @"Increases the range of your Death Grip ability by 5 yards.")]
        public bool GlyphofDeathGrip { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Your Hungering Cold ability no longer costs runic power.</summary>
        [GlyphData(18, 45800, 63331, "Glyph of Hungering Cold", GlyphType.Major,
            @"Your Hungering Cold ability no longer costs runic power.")]
        public bool GlyphofHungeringCold { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Increases the radius of your Pestilence effect by 5 yards.</summary>
        [GlyphData(19, 43548, 58657, "Glyph of Pestilence", GlyphType.Major,
            @"Increases the radius of your Pestilence effect by 5 yards.")]
        public bool GlyphofPestilence { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Empowers your Pillar of Frost, making you immune to all effects that cause loss of control of your character, but also freezing you in place while the ability is active.</summary>
        [GlyphData(20, 43553, 58635, "Glyph of Pillar of Frost", GlyphType.Major,
            @"Empowers your Pillar of Frost, making you immune to all effects that cause loss of control of your character, but also freezing you in place while the ability is active.")]
        public bool GlyphofPillarofFrost { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Your Rune Tap also heals your party for 5% of their maximum health.</summary>
        [GlyphData(21, 43825, 59327, "Glyph of Rune Tap", GlyphType.Major,
            @"Your Rune Tap also heals your party for 5% of their maximum health.")]
        public bool GlyphofRuneTap { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Increases the Silence duration of your Strangulate ability by 2 sec when used on a target who is casting a spell.</summary>
        [GlyphData(22, 43552, 58618, "Glyph of Strangulate", GlyphType.Major,
            @"Increases the Silence duration of your Strangulate ability by 2 sec when used on a target who is casting a spell.")]
        public bool GlyphofStrangulate { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Increases the bonus healing received while your Vampiric Blood is active by an additional 15%, but your Vampiric Blood no longer grants you health.</summary>
        [GlyphData(23, 43554, 58676, "Glyph of Vampiric Blood", GlyphType.Major,
            @"Increases the bonus healing received while your Vampiric Blood is active by an additional 15%, but your Vampiric Blood no longer grants you health.")]
        public bool GlyphofVampiricBlood { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 24; } }
        /// <summary>Your Blood Tap no longer causes damage to you.</summary>
        [GlyphData(24, 43535, 58640, "Glyph of Blood Tap", GlyphType.Minor,
            @"Your Blood Tap no longer causes damage to you.")]
        public bool GlyphofBloodTap { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Reduces the cast time of your Death Gate spell by 60%.</summary>
        [GlyphData(25, 43673, 60200, "Glyph of Death Gate", GlyphType.Minor,
            @"Reduces the cast time of your Death Gate spell by 60%.")]
        public bool GlyphofDeathGate { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Your Death Coil refunds 20 runic power when used to heal.</summary>
        [GlyphData(26, 43539, 58677, "Glyph of Death's Embrace", GlyphType.Minor,
            @"Your Death Coil refunds 20 runic power when used to heal.")]
        public bool GlyphofDeathsEmbrace { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Increases the duration of your Horn of Winter ability by 1 min.</summary>
        [GlyphData(27, 43544, 58680, "Glyph of Horn of Winter", GlyphType.Minor,
            @"Increases the duration of your Horn of Winter ability by 1 min.")]
        public bool GlyphofHornofWinter { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Your Path of Frost ability allows you to fall from a greater distance without suffering damage.</summary>
        [GlyphData(28, 43671, 59307, "Glyph of Path of Frost", GlyphType.Minor,
            @"Your Path of Frost ability allows you to fall from a greater distance without suffering damage.")]
        public bool GlyphofPathofFrost { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>When your Death Grip ability fails because its target is immune, its cooldown is reset.</summary>
        [GlyphData(29, 43672, 59309, "Glyph of Resilient Grip", GlyphType.Minor,
            @"When your Death Grip ability fails because its target is immune, its cooldown is reset.")]
        public bool GlyphofResilientGrip { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        #endregion
    } 

    public partial class WarlockTalents
    {
        private bool[] _glyphData = new bool[34];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the duration of your Bane of Agony by 4 sec.</summary>
        [GlyphData(0, 42456, 56241, "Glyph of Bane of Agony", GlyphType.Prime,
            @"Increases the duration of your Bane of Agony by 4 sec.")]
        public bool GlyphOfBaneOfAgony { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Reduces the cooldown on Chaos Bolt by 2 sec.</summary>
        [GlyphData(1, 45781, 63304, "Glyph of Chaos Bolt", GlyphType.Prime,
            @"Reduces the cooldown on Chaos Bolt by 2 sec.")]
        public bool GlyphOfChaosBolt { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Reduces the cooldown of your Conflagrate by 2 sec.</summary>
        [GlyphData(2, 42454, 56235, "Glyph of Conflagrate", GlyphType.Prime,
            @"Reduces the cooldown of your Conflagrate by 2 sec.")]
        public bool GlyphOfConflagrate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Your Corruption spell has a 4% chance to cause you to enter a Shadow Trance state after damaging the opponent.  The Shadow Trance state reduces the casting time of your next Shadow Bolt spell by 100%.</summary>
        [GlyphData(3, 42455, 56218, "Glyph of Corruption", GlyphType.Prime,
            @"Your Corruption spell has a 4% chance to cause you to enter a Shadow Trance state after damaging the opponent.  The Shadow Trance state reduces the casting time of your next Shadow Bolt spell by 100%.")]
        public bool GlyphOfCorruption { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage done by your Felguard's Legion Strike by 5%.</summary>
        [GlyphData(4, 42459, 56246, "Glyph of Felguard", GlyphType.Prime,
            @"Increases the damage done by your Felguard's Legion Strike by 5%.")]
        public bool GlyphOfFelguard { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>The bonus damage granted by your Haunt spell is increased by an additional 3%.</summary>
        [GlyphData(5, 45779, 63302, "Glyph of Haunt", GlyphType.Prime,
            @"The bonus damage granted by your Haunt spell is increased by an additional 3%.")]
        public bool GlyphOfHaunt { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases the periodic damage of your Immolate by 10%.</summary>
        [GlyphData(6, 42464, 56228, "Glyph of Immolate", GlyphType.Prime,
            @"Increases the periodic damage of your Immolate by 10%.")]
        public bool GlyphOfImmolate { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage done by your Imp's Firebolt spell by 20%.</summary>
        [GlyphData(7, 42465, 56248, "Glyph of Imp", GlyphType.Prime,
            @"Increases the damage done by your Imp's Firebolt spell by 20%.")]
        public bool GlyphOfImp { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the damage done by Incinerate by 5%.</summary>
        [GlyphData(8, 42453, 56242, "Glyph of Incinerate", GlyphType.Prime,
            @"Increases the damage done by Incinerate by 5%.")]
        public bool GlyphOfIncinerate { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the damage done by your Succubus' Lash of Pain by 25%.</summary>
        [GlyphData(9, 50077, 79047, "Glyph of Lash of Pain", GlyphType.Prime,
            @"Increases the damage done by your Succubus' Lash of Pain by 25%.")]
        public bool GlyphOfLashPain { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the duration of your Metamorphosis by 6 sec.</summary>
        [GlyphData(10, 45780, 63303, "Glyph of Metamorphosis", GlyphType.Prime,
            @"Increases the duration of your Metamorphosis by 6 sec.")]
        public bool GlyphOfMetamorphosis { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>If your Shadowburn fails to kill the target at or below 20% health, your Shadowburn's cooldown is instantly reset. This effect has a 6 sec cooldown.</summary>
        [GlyphData(11, 42468, 56629, "Glyph of Shadowburn", GlyphType.Prime,
            @"If your Shadowburn fails to kill the target at or below 20% health, your Shadowburn's cooldown is instantly reset. This effect has a 6 sec cooldown.")]
        public bool GlyphOfShadowburn { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Decreases the casting time of your Unstable Affliction by 0.2 sec.</summary>
        [GlyphData(12, 42472, 56233, "Glyph of Unstable Affliction", GlyphType.Prime,
            @"Decreases the casting time of your Unstable Affliction by 0.2 sec.")]
        public bool GlyphOfUnstableAffliction { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 13; } }
        /// <summary>Increases the duration of your Death Coil by 0.5 sec.</summary>
        [GlyphData(13, 42457, 56232, "Glyph of Death Coil", GlyphType.Major,
            @"Increases the duration of your Death Coil by 0.5 sec.")]
        public bool GlyphOfDeathCoil { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary></summary>
        [GlyphData(14, 45782, 63309, "Glyph of Demonic Circle", GlyphType.Major,
            @"Reduces the cooldown on Demonic Circle by 4 sec.")]
        public bool GlyphOfDemonicCircle { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Your Fear causes the target to tremble in place instead of fleeing in fear, but now causes Fear to have a 5 sec cooldown.</summary>
        [GlyphData(15, 42458, 56244, "Glyph of Fear", GlyphType.Major,
            @"Your Fear causes the target to tremble in place instead of fleeing in fear, but now causes Fear to have a 5 sec cooldown.")]
        public bool GlyphOfFear { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>When your Felhunter uses Devour Magic, you will also be healed for that amount.</summary>
        [GlyphData(16, 42450, 56249, "Glyph of Felhunter", GlyphType.Major,
            @"When your Felhunter uses Devour Magic, you will also be healed for that amount.")]
        public bool GlyphOfFelhunter { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>You receive 30% more healing from using a healthstone.</summary>
        [GlyphData(17, 42462, 56224, "Glyph of Healthstone", GlyphType.Major,
            @"You receive 30% more healing from using a healthstone.")]
        public bool GlyphOfHealthstone { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Reduces the cooldown on your Howl of Terror spell by 8 sec.</summary>
        [GlyphData(18, 42463, 56217, "Glyph of Howl of Terror", GlyphType.Major,
            @"Reduces the cooldown on your Howl of Terror spell by 8 sec.")]
        public bool GlyphHowlTerror { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Reduces the global cooldown of your Life Tap by .5 sec.</summary>
        [GlyphData(19, 45785, 63320, "Glyph of Life Tap", GlyphType.Major,
            @"Reduces the global cooldown of your Life Tap by .5 sec.")]
        public bool GlyphOfLifeTap { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Your Succubus's Seduction ability also removes all damage over time effects from the target.</summary>
        [GlyphData(20, 42471, 56250, "Glyph of Seduction", GlyphType.Major,
            @"Your Succubus's Seduction ability also removes all damage over time effects from the target.")]
        public bool GlyphOfSeduction { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Reduces the mana cost of your Shadow Bolt by 15%.</summary>
        [GlyphData(21, 42467, 56240, "Glyph of Shadow Bolt", GlyphType.Major,
            @"Reduces the mana cost of your Shadow Bolt by 15%.")]
        public bool GlyphOfShadowBolt { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Your Shadowflame also applies a 70% movement speed slow to its victims.</summary>
        [GlyphData(22, 45783, 63310, "Glyph of Shadowflame", GlyphType.Major,
            @"Your Shadowflame also applies a 70% movement speed slow to its victims.")]
        public bool GlyphOfShadowflame { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Increases the percentage of damage shared via your Soul Link by an additional 5%.</summary>
        [GlyphData(23, 45789, 63312, "Glyph of Soul Link", GlyphType.Major,
            @"Increases the percentage of damage shared via your Soul Link by an additional 5%.")]
        public bool GlyphOfSoulLink { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Your Soul Swap leaves your damage-over-time spells behind on the target you Soul Swapped from, but gives Soul Swap a 15 sec cooldown.</summary>
        [GlyphData(24, 42466, 56226, "Glyph of Soul Swap", GlyphType.Major,
            @"Your Soul Swap leaves your damage-over-time spells behind on the target you Soul Swapped from, but gives Soul Swap a 15 sec cooldown.")]
        public bool GlyphOfSoulSwap { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Increases the amount of health you gain from resurrecting via a Soulstone by an additional 40%.</summary>
        [GlyphData(25, 42470, 56231, "Glyph of Soulstone", GlyphType.Major,
            @"Increases the amount of health you gain from resurrecting via a Soulstone by an additional 40%.")]
        public bool GlyphOfSoulstone { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary></summary>
        [GlyphData(26, 42473, 56247, "Glyph of Voidwalker", GlyphType.Major,
            @"Increases your Voidwalker's total health by 20%.")]
        public bool GlyphOfVoidwalker { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 27; } }
        /// <summary>Increases the range of your Curse of Exhaustion spell by 5 yards.</summary>
        [GlyphData(27, 43392, 58080, "Glyph of Curse of Exhaustion", GlyphType.Minor,
            @"Increases the range of your Curse of Exhaustion spell by 5 yards.")]
        public bool GlyphCurseOfExhaustion { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Your Drain Soul restores 10% of your total mana after you kill a target that yields experience or honor.</summary>
        [GlyphData(28, 43390, 58070, "Glyph of Drain Soul", GlyphType.Minor,
            @"Your Drain Soul restores 10% of your total mana after you kill a target that yields experience or honor.")]
        public bool GlyphDrainSoul { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Reduces the cast time of your Enslave Demon spell by 50%.</summary>
        [GlyphData(29, 43393, 58107, "Glyph of Enslave Demon", GlyphType.Minor,
            @"Reduces the cast time of your Enslave Demon spell by 50%.")]
        public bool GlyphEnslaveDemon { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Increases the movement speed of your Eye of Kilrogg by 50% and allows it to fly in areas where flying mounts are enabled.</summary>
        [GlyphData(30, 43391, 58081, "Glyph of Eye of Kilrogg", GlyphType.Minor,
            @"Increases the movement speed of your Eye of Kilrogg by 50% and allows it to fly in areas where flying mounts are enabled.")]
        public bool GlyphEyeKilrogg { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Reduces the pushback suffered from damaging attacks while channeling your Health Funnel spell by 100%.</summary>
        [GlyphData(31, 42461, 56238, "Glyph of Health Funnel", GlyphType.Minor,
            @"Reduces the pushback suffered from damaging attacks while channeling your Health Funnel spell by 100%.")]
        public bool GlyphHealthFunnel { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Reduces the mana cost of your Ritual of Souls spell by 70%.</summary>
        [GlyphData(32, 43394, 58094, "Glyph of Ritual of Souls", GlyphType.Minor,
            @"Reduces the mana cost of your Ritual of Souls spell by 70%.")]
        public bool GlyphRitualSouls { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Increases the swim speed of targets affected by your Unending Breath spell by 20%.</summary>
        [GlyphData(33, 43389, 58079, "Glyph of Unending Breath", GlyphType.Minor,
            @"Increases the swim speed of targets affected by your Unending Breath spell by 20%.")]
        public bool GlyphUnendingBreath { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        #endregion       
    }

    public partial class RogueTalents
    {
        private bool[] _glyphData = new bool[36];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the duration of Adrenaline Rush by 5 sec.</summary>
        [GlyphData(0, 42954, 56808, "Glyph of Adrenaline Rush", GlyphType.Prime,
            @"Increases the duration of Adrenaline Rush by 5 sec.")]
        public bool GlyphOfAdrenalineRush { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Your Backstab critical strikes grant you 5 energy.</summary>
        [GlyphData(1, 42956, 56800, "Glyph of Backstab", GlyphType.Prime,
            @"Your Backstab critical strikes grant you 5 energy.")]
        public bool GlyphOfBackstab { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases the critical strike chance of Eviscerate by 10%.</summary>
        [GlyphData(2, 42961, 56802, "Glyph of Eviscerate", GlyphType.Prime,
            @"Increases the critical strike chance of Eviscerate by 10%.")]
        public bool GlyphOfEviscerate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Your Hemorrhage ability also causes the target to bleed, dealing 40% of the direct strike's damage over 24 sec.</summary>
        [GlyphData(3, 42967, 56807, "Glyph of Hemorrhage", GlyphType.Prime,
            @"Your Hemorrhage ability also causes the target to bleed, dealing 40% of the direct strike's damage over 24 sec.")]
        public bool GlyphOfHemorrhage { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the bonus to your damage while Killing Spree is active by an additional 10%.</summary>
        [GlyphData(4, 45762, 63252, "Glyph of Killing Spree", GlyphType.Prime,
            @"Increases the bonus to your damage while Killing Spree is active by an additional 10%.")]
        public bool GlyphOfKillingSpree { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Reduces the cost of Mutilate by 5 energy.</summary>
        [GlyphData(5, 45768, 63268, "Glyph of Mutilate", GlyphType.Prime,
            @"Reduces the cost of Mutilate by 5 energy.")]
        public bool GlyphOfMutilate { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases Revealing Strike's bonus effectiveness to your finishing moves by an additional 10%.</summary>
        [GlyphData(6, 42965, 56814, "Glyph of Revealing Strike", GlyphType.Prime,
            @"Increases Revealing Strike's bonus effectiveness to your finishing moves by an additional 10%.")]
        public bool GlyphOfRevealingStrike { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the duration of Rupture by 4 sec.</summary>
        [GlyphData(7, 42969, 56801, "Glyph of Rupture", GlyphType.Prime,
            @"Increases the duration of Rupture by 4 sec.")]
        public bool GlyphOfRupture { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the duration of Shadow Dance by 2 sec.</summary>
        [GlyphData(8, 45764, 63253, "Glyph of Shadow Dance", GlyphType.Prime,
            @"Increases the duration of Shadow Dance by 2 sec.")]
        public bool GlyphOfShadowDance { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Your Sinister Strikes have a 20% chance to add an additional combo point.</summary>
        [GlyphData(9, 42972, 56821, "Glyph of Sinister Strike", GlyphType.Prime,
            @"Your Sinister Strikes have a 20% chance to add an additional combo point.")]
        public bool GlyphOfSinisterStrike { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the duration of Slice and Dice by 6 sec.</summary>
        [GlyphData(10, 42973, 56810, "Glyph of Slice and Dice", GlyphType.Prime,
            @"Increases the duration of Slice and Dice by 6 sec.")]
        public bool GlyphOfSliceandDice { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Increases the duration of your Vendetta ability by 20%.</summary>
        [GlyphData(11, 45761, 63249, "Glyph of Vendetta", GlyphType.Prime,
            @"Increases the duration of your Vendetta ability by 20%.")]
        public bool GlyphOfVendetta { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 12; } }

        /// <summary>Increases the range on Ambush by 5 yards.</summary>
        [GlyphData(12, 42955, 56813, "Glyph of Ambush", GlyphType.Major,
            @"Increases the range on Ambush by 5 yards.")]
        public bool GlyphOfAmbush { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Reduces the penalty to energy generation while Blade Flurry is active by 50%.</summary>
        [GlyphData(13, 42957, 56818, "Glyph of Blade Flurry", GlyphType.Major,
            @"Reduces the penalty to energy generation while Blade Flurry is active by 50%.")]
        public bool GlyphOfBladeFlurry { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Your Blind ability also removes all damage over time effects from the target.</summary>
        [GlyphData(14, 64493, 91299, "Glyph of Blind", GlyphType.Major,
            @"Your Blind ability also removes all damage over time effects from the target.")]
        public bool GlyphOfBlind { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>While Cloak of Shadows is active, you take 40% less physical damage.</summary>
        [GlyphData(15, 45769, 63269, "Glyph of Cloak of Shadows", GlyphType.Major,
            @"While Cloak of Shadows is active, you take 40% less physical damage.")]
        public bool GlyphOfCloakOfShadows { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Increases the chance to inflict your target with Crippling Poison by an additional 20%.</summary>
        [GlyphData(16, 42958, 56820, "Glyph of Crippling Poison", GlyphType.Major,
            @"Increases the chance to inflict your target with Crippling Poison by an additional 20%.")]
        public bool GlyphOfCripplingPoison { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases the slowing effect on Deadly Throw by 20%.</summary>
        [GlyphData(17, 42959, 56806, "Glyph of Deadly Throw", GlyphType.Major,
            @"Increases the slowing effect on Deadly Throw by 20%.")]
        public bool GlyphOfDeadlyThrow { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the duration of Evasion by 5 sec.</summary>
        [GlyphData(18, 42960, 56799, "Glyph of Evasion", GlyphType.Major,
            @"Increases the duration of Evasion by 5 sec.")]
        public bool GlyphOfEvasion { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Increases the duration of Expose Armor by 12 sec.</summary>
        [GlyphData(19, 42962, 56803, "Glyph of Expose Armor", GlyphType.Major,
            @"Increases the duration of Expose Armor by 12 sec.")]
        public bool GlyphOfExposeArmor { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Increases the radius of your Fan of Knives ability by 50%.</summary>
        [GlyphData(20, 45766, 63254, "Glyph of Fan of Knives", GlyphType.Major,
            @"Increases the radius of your Fan of Knives ability by 50%.")]
        public bool GlyphOfFanOfKnives { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Reduces the energy cost of Feint by 20.</summary>
        [GlyphData(21, 42963, 56804, "Glyph of Feint", GlyphType.Major,
            @"Reduces the energy cost of Feint by 20.")]
        public bool GlyphOfFeint { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Increases the duration of your Garrote ability's silence effect by 1.5 sec.</summary>
        [GlyphData(22, 42964, 56812, "Glyph of Garrote", GlyphType.Major,
            @"Increases the duration of your Garrote ability's silence effect by 1.5 sec.")]
        public bool GlyphOfGarrote { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Your Gouge ability no longer requires that the target be facing you.</summary>
        [GlyphData(23, 42966, 56809, "Glyph of Gouge", GlyphType.Major,
            @"Your Gouge ability no longer requires that the target be facing you.")]
        public bool GlyphOfGouge { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Increases the cooldown of your Kick ability by 4 sec, but this cooldown is reduced by 6 sec when your Kick successfully interrupts a spell.</summary>
        [GlyphData(24, 42971, 56805, "Glyph of Kick", GlyphType.Major,
            @"Increases the cooldown of your Kick ability by 4 sec, but this cooldown is reduced by 6 sec when your Kick successfully interrupts a spell.")]
        public bool GlyphOfKick { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Your Preparation ability also instantly resets the cooldown of Kick, Dismantle, and Smoke Bomb.</summary>
        [GlyphData(25, 42968, 56819, "Glyph of Preparation", GlyphType.Major,
            @"Your Preparation ability also instantly resets the cooldown of Kick, Dismantle, and Smoke Bomb.")]
        public bool GlyphOfPreparation { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Increases the duration of Sap against non-player targets by 80 sec.</summary>
        [GlyphData(26, 42970, 56798, "Glyph of Sap", GlyphType.Major,
            @"Increases the duration of Sap against non-player targets by 80 sec.")]
        public bool GlyphOfSap { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Increases the movement speed of your Sprint ability by an additional 30%.</summary>
        [GlyphData(27, 42974, 56811, "Glyph of Sprint", GlyphType.Major,
            @"Increases the movement speed of your Sprint ability by an additional 30%.")]
        public bool GlyphOfSprint { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Removes the energy cost of your Tricks of the Trade ability but reduces the recipient's damage bonus by 5%.</summary>
        [GlyphData(28, 45767, 63256, "Glyph of Tricks of the Trade", GlyphType.Major,
            @"Removes the energy cost of your Tricks of the Trade ability but reduces the recipient's damage bonus by 5%.")]
        public bool GlyphOfTricksOfTheTrade { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Increases the duration of your Vanish effect by 2 sec.</summary>
        [GlyphData(29, 63420, 89758, "Glyph of Vanish", GlyphType.Major,
            @"Increases the duration of your Vanish effect by 2 sec.")]
        public bool GlyphOfVanish { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 30; } }
        /// <summary>You gain the ability to walk on water while your Sprint ability is active.</summary>
        [GlyphData(30, 43379, 58039, "Glyph of Blurred Speed", GlyphType.Minor,
            @"You gain the ability to walk on water while your Sprint ability is active.")]
        public bool GlyphOfBlurredSpeed { get { return _glyphData[30]; } set{ _glyphData[30] = value;} }
        /// <summary>Increases the range of your Distract ability by 5 yards.</summary>
        [GlyphData(31, 43376, 58032, "Glyph of Distract", GlyphType.Minor,
            @"Increases the range of your Distract ability by 5 yards.")]
        public bool GlyphOfDistrict { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Reduces the cast time of your Pick Lock ability by 100%.</summary>
        [GlyphData(32, 43377, 58027, "Glyph of Pick Lock", GlyphType.Minor,
            @"Reduces the cast time of your Pick Lock ability by 100%.")]
        public bool GlyphOfPickLock { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Increases the range of your Pick Pocket ability by 5 yards.</summary>
        [GlyphData(33, 43343, 58017, "Glyph of Pick Pocket", GlyphType.Minor,
            @"Increases the range of your Pick Pocket ability by 5 yards.")]
        public bool GlyphOfPickPocket { get { return _glyphData[33]; } set{ _glyphData[33] = value;} }
        /// <summary>You apply poisons to your weapons 50% faster.</summary>
        [GlyphData(34, 43380, 58038, "Glyph of Poisons", GlyphType.Minor,
            @"You apply poisons to your weapons 50% faster.")]
        public bool GlyphOfPoisons { get { return _glyphData[34]; } set{ _glyphData[34] = value;} }
        /// <summary>Increases the distance your Safe Fall ability allows you to fall without taking damage.</summary>
        [GlyphData(35, 43378, 58033, "Glyph of Safe Fall", GlyphType.Minor,
            @"Increases the distance your Safe Fall ability allows you to fall without taking damage.")]
        public bool GlyphOfSafeFall { get { return _glyphData[35]; } set{ _glyphData[35] = value;} }
        #endregion
    }

    public partial class HunterTalents
    {
        private bool[] _glyphData = new bool[31];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>When you critically hit with Aimed Shot, you instantly gain 5 Focus.</summary>
        [GlyphData(0, 42897, 56824, "Glyph of Aimed Shot", GlyphType.Prime,
            @"When you critically hit with Aimed Shot, you instantly gain 5 Focus.")]
        public bool GlyphOfAimedShot { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Your Arcane Shot deals 12% more damage.</summary>
        [GlyphData(1, 42898, 56841, "Glyph of Arcane Shot", GlyphType.Prime,
            @"Your Arcane Shot deals 12% more damage.")]
        public bool GlyphOfArcaneShot { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Reduces the cooldown of Chimera Shot by 1 sec.</summary>
        [GlyphData(2, 45625, 63065, "Glyph of Chimera Shot", GlyphType.Prime,
            @"Reduces the cooldown of Chimera Shot by 1 sec.")]
        public bool GlyphOfChimeraShot { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Your Steady Shot generates an additional 2 Focus on targets afflicted by a daze effect.</summary>
        [GlyphData(3, 42909, 56856, "Glyph of Dazzled Prey", GlyphType.Prime,
            @"Your Steady Shot generates an additional 2 Focus on targets afflicted by a daze effect.")]
        public bool GlyphOfDazzledPrey { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the critical strike chance of Explosive Shot by 6%.</summary>
        [GlyphData(4, 45731, 63066, "Glyph of Explosive Shot", GlyphType.Prime,
            @"Increases the critical strike chance of Explosive Shot by 6%.")]
        public bool GlyphOfExplosiveShot { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Reduces the Focus cost of your Kill Command by 3.</summary>
        [GlyphData(5, 42915, 56842, "Glyph of Kill Command", GlyphType.Prime,
            @"Reduces the Focus cost of your Kill Command by 3.")]
        public bool GlyphOfKillCommand { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>If the damage from your Kill Shot fails to kill a target at or below 20% health, your Kill Shot's cooldown is instantly reset. This effect has a 6 sec cooldown.</summary>
        [GlyphData(6, 45732, 63067, "Glyph of Kill Shot", GlyphType.Prime,
            @"If the damage from your Kill Shot fails to kill a target at or below 20% health, your Kill Shot's cooldown is instantly reset. This effect has a 6 sec cooldown.")]
        public bool GlyphOfKillShot { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the haste from Rapid Fire by an additional 10%.</summary>
        [GlyphData(7, 42911, 56828, "Glyph of Rapid Fire", GlyphType.Prime,
            @"Increases the haste from Rapid Fire by an additional 10%.")]
        public bool GlyphOfRapidFire { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the periodic critical strike chance of your Serpent Sting by 6%.</summary>
        [GlyphData(8, 42912, 56832, "Glyph of Serpent Sting", GlyphType.Prime,
            @"Increases the periodic critical strike chance of your Serpent Sting by 6%.")]
        public bool GlyphOfSerpentSting { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the damage dealt by Steady Shot by 10%.</summary>
        [GlyphData(9, 42914, 56826, "Glyph of Steady Shot", GlyphType.Prime,
            @"Increases the damage dealt by Steady Shot by 10%.")]
        public bool GlyphOfSteadyShot { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 10; } }
        /// <summary>Decreases the cooldown of Bestial Wrath by 20 sec.</summary>
        [GlyphData(10, 42902, 56830, "Glyph of Bestial Wrath", GlyphType.Major,
            @"Decreases the cooldown of Bestial Wrath by 20 sec.")]
        public bool GlyphOfBestialWrath { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Your Concussive Shot also limits the maximum run speed of your target.</summary>
        [GlyphData(11, 42901, 56851, "Glyph of Concussive Shot", GlyphType.Major,
            @"Your Concussive Shot also limits the maximum run speed of your target.")]
        public bool GlyphOfConcussiveShot { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Decreases the cooldown of Deterrence by 10 sec.</summary>
        [GlyphData(12, 42903, 56850, "Glyph of Deterrence", GlyphType.Major,
            @"Decreases the cooldown of Deterrence by 10 sec.")]
        public bool GlyphOfDeterrence { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Decreases the cooldown of Disengage by 5 sec.</summary>
        [GlyphData(13, 42904, 56844, "Glyph of Disengage", GlyphType.Major,
            @"Decreases the cooldown of Disengage by 5 sec.")]
        public bool GlyphOfDisengage { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>When your Freezing Trap breaks, the victim's movement speed is reduced by 70% for 4 sec.</summary>
        [GlyphData(14, 42905, 56845, "Glyph of Freezing Trap", GlyphType.Major,
            @"When your Freezing Trap breaks, the victim's movement speed is reduced by 70% for 4 sec.")]
        public bool GlyphOfFreezingTrap { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the radius of the effect from your Ice Trap by 2 yards.</summary>
        [GlyphData(15, 42906, 56847, "Glyph of Ice Trap", GlyphType.Major,
            @"Increases the radius of the effect from your Ice Trap by 2 yards.")]
        public bool GlyphOfFrostTrap { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Decreases the duration of the effect from your Immolation Trap by 6 sec., but damage while active is increased by 100%.</summary>
        [GlyphData(16, 42908, 56846, "Glyph of Immolation Trap", GlyphType.Major,
            @"Decreases the duration of the effect from your Immolation Trap by 6 sec., but damage while active is increased by 100%.")]
        public bool GlyphOfImmolationTrap { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases the duration of your Master's Call by 4 sec.</summary>
        [GlyphData(17, 45733, 63068, "Glyph of Master's Call", GlyphType.Major,
            @"Increases the duration of your Master's Call by 4 sec.")]
        public bool GlyphOfMastersCall { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the total amount of healing done by your Mend Pet ability by 60%.</summary>
        [GlyphData(18, 42900, 56833, "Glyph of Mending", GlyphType.Major,
            @"Increases the total amount of healing done by your Mend Pet ability by 60%.")]
        public bool GlyphOfMending { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>When you use Misdirection on your pet, the cooldown on your Misdirection is reset.</summary>
        [GlyphData(19, 42907, 56829, "Glyph of Misdirection", GlyphType.Major,
            @"When you use Misdirection on your pet, the cooldown on your Misdirection is reset.")]
        public bool GlyphOfMisdirection { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Reduces damage taken by 20% for 5 sec after using Raptor Strike.</summary>
        [GlyphData(20, 45735, 63086, "Glyph of Raptor Strike", GlyphType.Major,
            @"Reduces damage taken by 20% for 5 sec after using Raptor Strike.")]
        public bool GlyphOfRaptorStrike { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Increases the range of Scatter Shot by 3 yards.</summary>
        [GlyphData(21, 45734, 63069, "Glyph of Scatter Shot", GlyphType.Major,
            @"Increases the range of Scatter Shot by 3 yards.")]
        public bool GlyphOfScatterShot { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>When you successfully silence an enemy's spell cast with Silencing Shot, you instantly gain 10 focus.</summary>
        [GlyphData(22, 42910, 56836, "Glyph of Silencing Shot", GlyphType.Major,
            @"When you successfully silence an enemy's spell cast with Silencing Shot, you instantly gain 10 focus.")]
        public bool GlyphOfSilencingShot { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Snakes from your Snake Trap take 90% reduced damage from area of effect spells.</summary>
        [GlyphData(23, 42913, 56849, "Glyph of Snake Trap", GlyphType.Major,
            @"Snakes from your Snake Trap take 90% reduced damage from area of effect spells.")]
        public bool GlyphOfSnakeTrap { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Reduces the focus cost of Trap Launcher by 10.</summary>
        [GlyphData(24, 42899, 56857, "Glyph of Trap Launcher", GlyphType.Major,
            @"Reduces the focus cost of Trap Launcher by 10.")]
        public bool GlyphOfTrueshotAura { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Decreases the cooldown of your Wyvern Sting by 6 sec.</summary>
        [GlyphData(25, 42917, 56848, "Glyph of Wyvern Sting", GlyphType.Major,
            @"Decreases the cooldown of your Wyvern Sting by 6 sec.")]
        public bool GlyphOfWyvernSting { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 26; } }
        /// <summary>Increases the range of your Aspect of the Pack ability by 15 yards.</summary>
        [GlyphData(26, 43355, 57904, "Glyph of Aspect of the Pack", GlyphType.Minor,
            @"Increases the range of your Aspect of the Pack ability by 15 yards.")]
        public bool GlyphOfAspectofthePack { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Reduces the cooldown of your Feign Death spell by 5 sec.</summary>
        [GlyphData(27, 43351, 57903, "Glyph of Feign Death", GlyphType.Minor,
            @"Reduces the cooldown of your Feign Death spell by 5 sec.")]
        public bool GlyphOfFeignDeath { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Slightly reduces the size of your Pet.</summary>
        [GlyphData(28, 43350, 57870, "Glyph of Lesser Proportion", GlyphType.Minor,
            @"Slightly reduces the size of your Pet.")]
        public bool GlyphOfLesserProportion { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Reduces the pushback suffered from damaging attacks while casting Revive Pet by 100%.</summary>
        [GlyphData(29, 43338, 57866, "Glyph of Revive Pet", GlyphType.Minor,
            @"Reduces the pushback suffered from damaging attacks while casting Revive Pet by 100%.")]
        public bool GlyphOfRevivePet { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Reduces the pushback suffered from damaging attacks while casting Scare Beast by 75%.</summary>
        [GlyphData(30, 43356, 57902, "Glyph of Scare Beast", GlyphType.Minor,
            @"Reduces the pushback suffered from damaging attacks while casting Scare Beast by 75%.")]
        public bool GlyphOfScareBeast { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        #endregion
    }

    public partial class MonkTalents
    {
        private bool[] _glyphData = new bool[35];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Major
        public override int GlyphTreeStartingIndexes_0 { get { return -1; } }
        public override int GlyphTreeStartingIndexes_1 { get { return 0; } }
        /// <summary>Increases the chance to summon a Healing Sphere when you kill an enemy while gaining experience or honor by 25%.</summary>
        [GlyphData(0, 87891, 125676, "Glyph of Afterlife", GlyphType.Major,
            @"Increases the chance to summon a Healing Sphere when you kill an enemy while gaining experience or honor by 25%.")]
        public bool GlyphofAfterlife { get { return _glyphData[0]; } set { _glyphData[0] = value; } }

        /// <summary>When you use Breath of Fire on targets afflicted with your Dizzying Haze, they become Disoriented for 3 sec.</summary>
        [GlyphData(1, 85685, 123394, "Glyph of Breath of Fire", GlyphType.Major,
            @"When you use Breath of Fire on targets afflicted with your Dizzying Haze, they become Disoriented for 3 sec.")]
        public bool GlyphofBreathofFire { get { return _glyphData[1]; } set { _glyphData[1] = value; } }

        /// <summary>Increases the range of your Clash ability by 10 yards.</summary>
        [GlyphData(2, 85687, 123399, "Glyph of Clash", GlyphType.Major,
            @"Increases the range of your Clash ability by 10 yards.")]
        public bool GlyphofClash { get { return _glyphData[2]; } set { _glyphData[2] = value; } }

        /// <summary>Your Crackling Jade Lightning knocks the target back a further distance, and after being knocked back increases the damage the target takes from your Crackling Jade Lightning by 25% for 8 sec.</summary>
        [GlyphData(3, 87880, 125648, "Glyph of Crackling Jade Lightning", GlyphType.Major,
            @"Your Crackling Jade Lightning knocks the target back a further distance, and after being knocked back increases the damage the target takes from your Crackling Jade Lightning by 25% for 8 sec.")]
        public bool GlyphofCracklingJadeLightning { get { return _glyphData[3]; } set { _glyphData[3] = value; } }

        /// <summary>Increases the duration of your Healing Spheres by 3 minutes.</summary>
        [GlyphData(4, 85689, 125648, "Glyph of Enduring Healing Sphere", GlyphType.Major,
            @"Increases the duration of your Healing Spheres by 3 minutes.")]
        public bool GlyphofEnduringHealingSphere { get { return _glyphData[4]; } set { _glyphData[4] = value; } }

        /// <summary>Increases the range of your Expel Harm by 10 yards.</summary>
        [GlyphData(5, 82345, 119724, "Glyph of Expel Harm", GlyphType.Major,
            @"Increases the range of your Expel Harm by 10 yards.")]
        public bool GlyphofExpelHarm { get { return _glyphData[5]; } set { _glyphData[5] = value; } }

        /// <summary>When channeling Fists of Fury, your parry chance is increased by 100%.</summary>
        [GlyphData(6, 87892, 125671, "Glyph of Fists of Fury", GlyphType.Major,
            @"When channeling Fists of Fury, your parry chance is increased by 100%.")]
        public bool GlyphofFistsofFury { get { return _glyphData[6]; } set { _glyphData[6] = value; } }

        /// <summary>Your Fortifying Brew reduces damage taken by an additional 5%, but increases your health by 10% rather than 20%.</summary>
        [GlyphData(7, 87893, 124997, "Glyph of Fortifying Brew", GlyphType.Major,
            @"Your Fortifying Brew reduces damage taken by an additional 5%, but increases your health by 10% rather than 20%.")]
        public bool GlyphofFortifyingBrew { get { return _glyphData[7]; } set { _glyphData[7] = value; } }

        /// <summary>Increases the amount your Guard absorbs by 10%, but your Guard can only absorb magical damage.</summary>
        [GlyphData(8, 85691, 123401, "Glyph of Guard", GlyphType.Major,
            @"Increases the amount your Guard absorbs by 10%, but your Guard can only absorb magical damage.")]
        public bool GlyphofGuard { get { return _glyphData[8]; } set { _glyphData[8] = value; } }

        /// <summary>Teaches you the spell Leer of the Ox.\n\n
        /// Leer of the Ox\n
        /// Your Ox Statue stirs hatred in the target, reducing their movement speed by 50% and causing them to attack your Black Ox Statue for 8 sec. The statue must be within 40 yards of the target.\n\n
        /// Requires Black Ox Statue to be active.</summary>
        [GlyphData(9, 87894, 125648, "Glyph of Leer of the Ox", GlyphType.Major,
            @"Teaches you the spell Leer of the Ox.

Leer of the Ox
Your Ox Statue stirs hatred in the target, reducing their movement speed by 50% and causing them to attack your Black Ox Statue for 8 sec. The statue must be within 40 yards of the target.

Requires Black Ox Statue to be active.")]
        public bool GlyphofLeeroftheOx { get { return _glyphData[9]; } set { _glyphData[9] = value; } }

        /// <summary>Life Cocoon can now be cast while stunned.</summary>
        [GlyphData(10, 87895, 124989, "Glyph of Life Cocoon", GlyphType.Major,
            @"Life Cocoon can now be cast while stunned.")]
        public bool GlyphofLifeCocoon { get { return _glyphData[10]; } set { _glyphData[10] = value; } }

        /// <summary>Your Mana Tea is instant instead of channeled and consumes two stacks when used, but causes a 10 sec cooldown.</summary>
        [GlyphData(11, 85692, 123763, "Glyph of Mana Tea", GlyphType.Major,
            @"Your Mana Tea is instant instead of channeled and consumes two stacks when used, but causes a 10 sec cooldown.")]
        public bool GlyphofManaTea { get { return _glyphData[11]; } set { _glyphData[11] = value; } }

        /// <summary>Increases the number of Fire Blossoms you create by 1.</summary>
        [GlyphData(12, 87897, 125755, "Glyph of Path of Blossoms", GlyphType.Major,
            @"Increases the number of Fire Blossoms you create by 1.")]
        public bool GlyphofPathofBlossoms { get { return _glyphData[12]; } set { _glyphData[12] = value; } }

        /// <summary>Your Renewing Mist travels to the furthest injured target within 40 yards rather than the closest injured target within 20 yards.</summary>
        [GlyphData(13, 85696, 123334, "Glyph of Renewing Mists", GlyphType.Major,
            @"Your Renewing Mist travels to the furthest injured target within 40 yards rather than the closest injured target within 20 yards.")]
        public bool GlyphofRenewingMists { get { return _glyphData[13]; } set { _glyphData[13] = value; } }

        /// <summary>When you Roll or Chi Torpedo, all threat is temporarily reduced for 10 sec.</summary>
        [GlyphData(14, 87880, 124969, "Glyph of Retreat", GlyphType.Major,
            @"When you Roll or Chi Torpedo, all threat is temporarily reduced for 10 sec.")]
        public bool GlyphofRetreat { get { return _glyphData[14]; } set { _glyphData[14] = value; } }

        /// <summary>While Sparring, you also have a 5% chance to deflect spells from attackers in front of you, stacking up to 3 times.</summary>
        [GlyphData(15, 87898, 125673, "Glyph of Sparring", GlyphType.Major,
            @"While Sparring, you also have a 5% chance to deflect spells from attackers in front of you, stacking up to 3 times.")]
        public bool GlyphofSparring { get { return _glyphData[15]; } set { _glyphData[15] = value; } }

        /// <summary>You move at full speed while channeling Spinning Crane Kick.</summary>
        [GlyphData(16, 85697, 120479, "Glyph of Spinning Crane Kick", GlyphType.Major,
            @"You move at full speed while channeling Spinning Crane Kick.")]
        public bool GlyphofSpinningCraneKick { get { return _glyphData[16]; } set { _glyphData[16] = value; } }

        /// <summary>When you use Fortifying Brew, all bleed damage taken is reduced by 20% while active.</summary>
        [GlyphData(17, 87899, 125169, "Glyph of Stoneskin", GlyphType.Major,
            @"When you use Fortifying Brew, all bleed damage taken is reduced by 20% while active.")]
        public bool GlyphofStoneskin { get { return _glyphData[17]; } set { _glyphData[17] = value; } }

        /// <summary>Your Surging Mist no longer requires a target, and instead heals the lowest health target within 40 yards.</summary>
        [GlyphData(18, 85699, 120483, "Glyph of Surging Mist", GlyphType.Major,
            @"Your Surging Mist no longer requires a target, and instead heals the lowest health target within 40 yards.")]
        public bool GlyphofSurgingMist { get { return _glyphData[18]; } set { _glyphData[18] = value; } }

        /// <summary>Your Touch of Death no longer has a Chi cost, but the cooldown is increased by 2 minutes.</summary>
        [GlyphData(19, 85700, 123391, "Glyph of Touch of Death", GlyphType.Major,
            @"Your Touch of Death no longer has a Chi cost, but the cooldown is increased by 2 minutes.")]
        public bool GlyphofTouchofDeath { get { return _glyphData[19]; } set { _glyphData[19] = value; } }

        /// <summary>Your Touch of Karma now has a 20 yard range.</summary>
        [GlyphData(20, 87900, 125678, "Glyph of Touch of Karma", GlyphType.Major,
            @"Your Touch of Karma now has a 20 yard range.")]
        public bool GlyphofTouchofKarma { get { return _glyphData[20]; } set { _glyphData[20] = value; } }

        /// <summary>Increases the range of your Transcendence: Transfer spell by 10 yards.</summary>
        [GlyphData(21, 84652, 123023, "Glyph of Transcendence", GlyphType.Major,
            @"Increases the range of your Transcendence: Transfer spell by 10 yards.")]
        public bool GlyphofTranscendence { get { return _glyphData[21]; } set { _glyphData[21] = value; } }

        /// <summary>Your Uplift no longer costs Chi, but instead costs 6% Mana.</summary>
        [GlyphData(22, 87901, 125669, "Glyph of Uplift", GlyphType.Major,
            @"Your Uplift no longer costs Chi, but instead costs 6% Mana.")]
        public bool GlyphofUplift { get { return _glyphData[22]; } set { _glyphData[22] = value; } }

        /// <summary>You can now channel Zen Meditation while moving.</summary>
        [GlyphData(23, 85695, 120477, "Glyph of Zen Meditation", GlyphType.Major,
            @"You can now channel Zen Meditation while moving.")]
        public bool GlyphofZenMeditation { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 24; } }
        /// <summary>Your Blackout Kick always deals 20% additional damage over 4 sec regardless of positioning but you're unable to trigger the healing effect.</summary>
        [GlyphData(24, 90715, 132005, "Glyph of Blackout Kick", GlyphType.Minor,
            @"Your Blackout Kick always deals 20% additional damage over 4 sec regardless of positioning but you're unable to trigger the healing effect.")]
        public bool GlyphofBlackoutKick { get { return _glyphData[24]; } set { _glyphData[24] = value; } }

        /// <summary>Your Crackling Jade Lightning visual is altered to the color of the White Tiger celestial.</summary>
        [GlyphData(25, 87881, 125931, "Glyph of Crackling Tiger Lightning", GlyphType.Minor,
            @"Your Crackling Jade Lightning visual is altered to the color of the White Tiger celestial.")]
        public bool GlyphofCracklingTigerLightning { get { return _glyphData[25]; } set { _glyphData[25] = value; } }

        /// <summary>Your spirit now appears in a fighting pose when using Transcendence.</summary>
        [GlyphData(26, 87888, 125872, "Glyph of Fighting Pose", GlyphType.Minor,
            @"Your spirit now appears in a fighting pose when using Transcendence.")]
        public bool GlyphofFightingPose { get { return _glyphData[26]; } set { _glyphData[26] = value; } }

        /// <summary>Your Flying Serpent Kick automatically ends when you fly into an enemy, triggering the area of effect damage and snare.</summary>
        [GlyphData(27, 87882, 123403, "Glyph of Flying Serpent Kick", GlyphType.Minor,
            @"Your Flying Serpent Kick automatically ends when you fly into an enemy, triggering the area of effect damage and snare.")]
        public bool GlyphofFlyingSerpentKick { get { return _glyphData[27]; } set { _glyphData[27] = value; } }

        /// <summary>You honorably bow after each successful Touch of Death.</summary>
        [GlyphData(28, 87883, 125732, "Glyph of Honor", GlyphType.Minor,
            @"You honorably bow after each successful Touch of Death.")]
        public bool GlyphofHonor { get { return _glyphData[28]; } set { _glyphData[28] = value; } }

        /// <summary>You always will attack with hands and fist with Jab, even with non-fist weapons equipped.</summary>
        [GlyphData(29, 87884, 125660, "Glyph of Jab", GlyphType.Minor,
            @"You always will attack with hands and fist with Jab, even with non-fist weapons equipped.")]
        public bool GlyphofJab { get { return _glyphData[29]; } set { _glyphData[29] = value; } }

        /// <summary>Your Rising Sun Kick's visual is altered to the color of the White Tiger.</summary>
        [GlyphData(30, 87885, 125151, "Glyph of Rising Tiger Kick", GlyphType.Minor,
            @"Your Rising Sun Kick's visual is altered to the color of the White Tiger.")]
        public bool GlyphofRisingTigerKick { get { return _glyphData[30]; } set { _glyphData[30] = value; } }

        /// <summary>Your Spinning Fire Blossom requires an enemy target rather than traveling in front of you, but is no longer capable of rooting targets.</summary>
        [GlyphData(31, 85698, 123405, "Glyph of Spinning Fire Blossom", GlyphType.Minor,
            @"Your Spinning Fire Blossom requires an enemy target rather than traveling in front of you, but is no longer capable of rooting targets.")]
        public bool GlyphofSpinningFireBlossom { get { return _glyphData[31]; } set { _glyphData[31] = value; } }

        /// <summary>You can cast Roll or Chi Torpedo while dead as a spirit.</summary>
        [GlyphData(32, 87887, 125154, "Glyph of Spirit Roll", GlyphType.Minor,
            @"You can cast Roll or Chi Torpedo while dead as a spirit.")]
        public bool GlyphofSpiritRoll { get { return _glyphData[32]; } set { _glyphData[32] = value; } }

        /// <summary>You can Roll or Chi Torpedo over water.</summary>
        [GlyphData(33, 87889, 125901, "Glyph of Water Roll", GlyphType.Minor,
            @"You can Roll or Chi Torpedo over water.")]
        public bool GlyphofWaterRoll { get { return _glyphData[33]; } set { _glyphData[33] = value; } }

        /// <summary>Teaches you the spell Zen Flight. Zen Flight requires a Flight Master's License in order to be cast.\n\n
        /// Zen Flight\n
        /// You fly through the air at a quick speed on a meditative cloud.</summary>
        [GlyphData(34, 87890, 125893, "Glyph of Zen Flight", GlyphType.Minor,
            @"Teaches you the spell Zen Flight. Zen Flight requires a Flight Master's License in order to be cast.

Zen Flight
You fly through the air at a quick speed on a meditative cloud.")]
        public bool GlyphofZenFlight { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        #endregion
    }

}
