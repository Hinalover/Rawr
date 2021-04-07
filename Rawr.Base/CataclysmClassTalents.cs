using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rawr
{

    public partial class WarriorTalents : TalentsBase
    {
        #region Arms
        /// <summary>
        /// Increases the damage of Mortal Strike, Raging Blow, Devastate, Victory Rush and Slam by [5 * Pts]%.
        /// </summary>
        public int WarAcademy { get; set; }
        /// <summary>
        /// Increases all healing received by [3 * Pts]%, and the effectiveness of your self-healing abilities by an additional [10 * Pts]%.
        /// </summary>
        public int FieldDressing { get; set; }
        /// <summary>
        /// Your Charge generates [5 * Pts] additional rage and stuns an additional 2 nearby targets.
        /// </summary>
        public int Blitz { get; set; }
        /// <summary>
        /// You retain up to an additional [25 * Pts] Rage when you change stances.
        /// </summary>
        public int TacticalMastery { get; set; }
        /// <summary>
        /// Whenever you are struck by a Stun or Immobilize effect you generate [10 * Pts] Rage and [5 / 2 * Pts]% of your total health over 10 sec.
        /// </summary>
        //public int SecondWind { get; set; }
        /// <summary>
        /// Your critical strikes cause the opponent to bleed, dealing [16 * Pts]% of your melee weapon's average damage over 6 sec.
        /// </summary>
        public int DeepWounds { get; set; }
        /// <summary>
        /// Reduces the Rage cost of your Pummel, Demoralizing Shout, Intimidating Shout and Challenging Shout by [50 * Pts]%.
        /// </summary>
        public int DrumsOfWar { get; set; }
        /// <summary>
        /// Increases your Overpower critical strike chance by [20 * Pts]%. In addition, whenever your Rend ability causes damage, you have a [100 / 3 * Pts]% chance of allowing the use of Overpower for 9 sec.  This effect will not occur more than once every 5 sec.
        /// </summary>
        public int TasteForBlood { get; set; }
        /// <summary>
        /// Battle, Berserker Stance - Sweeping Strikes - 30 Rage
        /// 1 min cooldown - Instant
        /// Your melee attacks strike an additional nearby opponent.  Lasts 10 sec.
        /// </summary>
        public int SweepingStrikes { get; set; }
        /// <summary>
        /// Increases the critical strike damage bonus of Mortal Strike, Slam and Overpower by [10 * Pts]%.
        /// </summary>
        public int Impale { get; set; }
        /// <summary>
        /// When reapplying Hamstring, you immobilize the target for 5 sec. This effect cannot occur more than once every [30 / 2 * Pts] sec.  In addition, reduces the global cooldown of your Hamstring by [0.25 * Pts] sec.
        /// </summary>
        public int ImprovedHamstring { get; set; }
        /// <summary>
        /// Decreases the swing time of Slam by [0.5 * Pts] sec and increases its damage by [10 * Pts]%.
        /// </summary>
        public int ImprovedSlam { get; set; }
        /// <summary>
        /// Deadly Calm - 2 min cooldown - Instant
        /// For the next 10 sec, none of your abilities cost rage, but you continue to generate rage. Cannot be used during Inner Rage.
        /// </summary>
        //public int DeadlyCalm { get; set; }
        /// <summary>
        /// Your bleed effects cause targets to take an extra [2 * Pts]% physical damage. Applying a bleed effect increases bleed damage taken by the target by [15 * Pts]% for 1 min. In addition, your autoattacks have a [5 * Pts]% chance to generate 20 additional Rage.
        /// </summary>
        public int BloodFrenzy { get; set; }
        /// <summary>
        /// Your Mortal Strike causes the Slaughter effect, which refreshes the duration of Rend on the target and increases the damage of your Execute, Overpower, Slam and Mortal Strike by 10%.  Lasts 15 sec.  Stacks up to 3 times.
        /// </summary>
        public int LambsToTheSlaughter { get; set; }
        /// <summary>
        /// Your Charge ability is now usable while in combat and in all stances, and the cooldown of your Charge is reduced by 2 sec.  Following a Charge, your next Slam or Mortal Strike has an additional 25% chance to critically hit if used within 10 sec.  However, Charge and Intercept now share a cooldown.
        /// </summary>
        //public int Juggernaut { get; set; }
        /// <summary>
        /// Your melee hits have a [3 * Pts]% chance of resetting the cooldown on your Colossus Smash, and you keep [5 * Pts] rage after using Execute.
        /// </summary>
        public int SuddenDeath { get; set; }
        /// <summary>
        /// Your Mortal Strike critical hits have a [50 * Pts]% chance to Enrage you, increasing physical damage caused by [5 * Pts]% for 12 sec.
        /// </summary>
        public int WreckingCrew { get; set; }
        /// <summary>
        /// Battle Stance - Throwdown - Melee Range - 15 Rage
        /// 45 sec cooldown - Instant cast
        /// Requires Melee Weapon - Knocks the target to the ground and stuns it for 5 sec.
        /// </summary>
        public int Throwdown { get; set; }
        /// <summary>
        /// Bladestorm - 25 Rage
        /// 1 min cooldown - Instant cast
        /// Requires Melee Weapon - You become a whirling storm of destructive force, instantly striking all nearby targets for 150% weapon damage and continuing to perform a whirlwind attack every 1 sec for 6 sec.  While under the effects of Bladestorm, you do not feel pity or remorse or fear and you cannot be stopped unless killed or disarmed, but you cannot perform any other abilities.
        /// </summary>
        //public int Bladestorm { get; set; }
        #endregion
        #region Fury
        /// <summary>
        /// After taking any damage, you have a 10% chance to regenerate [1 * Pts]% of your total Health over 5 sec.
        /// </summary>
        public int BloodCraze { get; set; }
        /// <summary>
        /// Your Bloodthirst, Mortal Strike and Shield Slam hits have a [5 * Pts]% chance to make your next special attack that costs more than 5 Rage consume no Rage.
        /// </summary>
        public int BattleTrance { get; set; }
        /// <summary>
        /// Increases the critical strike chance of Bloodthirst, Mortal Strike and Shield Slam by [5 * Pts]%.
        /// </summary>
        public int Cruelty { get; set; }
        /// <summary>
        /// Your Execute hits have a [50 * Pts]% chance to improve your melee attack speed by 5% for 9 sec.  This effect stacks up to 0 times.
        /// </summary>
        public int Executioner { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Battle Shout and Commanding Shout by [15 * Pts] sec and causes those abilities to generate [5 * Pts] additional Rage.
        /// </summary>
        public int BoomingVoice { get; set; }
        /// <summary>
        /// Successfully interrupting a spell with Pummel increases your damage by 5% for [15 * Pts] sec.
        /// </summary>
        public int RudeInterruption { get; set; }
        /// <summary>
        /// Piercing Howl - 10 Rage
        /// Instant cast
        /// Causes all enemies within 10 yards to be Dazed, reducing movement speed by 50% for 6 sec.
        /// </summary>
        //public int PiercingHowl { get; set; }
        /// <summary>
        /// Increases your attack speed by [25 / 3 * Pts]% for your next 3 swings after dealing a melee critical strike.
        /// </summary>
        public int Flurry { get; set; }
        /// <summary>
        /// Death Wish - 10 Rage
        /// 3 min cooldown - Instant
        /// When activated you become Enraged, increasing your physical damage by 20% but increasing all damage taken by 5%.  Lasts 30 sec.
        /// </summary>
        public int DeathWish { get; set; }
        /// <summary>
        /// Your melee hits have a [3 * Pts]% chance to Enrage you, giving you a [10 / 3 * Pts]% physical damage bonus for 9 sec.
        /// </summary>
        public int Enrage { get; set; }
        /// <summary>
        /// Increases your parry chance by 100% for [4 * Pts] sec whenever you are brought to 20% health or less.  This effect cannot occur more often than once every 2 min.
        /// </summary>
        public int DieByTheSword { get; set; }
        /// <summary>
        /// Berserker Stance - Raging Blow - Melee Range - 20 Rage
        /// 6 sec cooldown - Instant cast
        /// Requires Melee Weapon - A mighty blow that deals 100% weapon damage from both melee weapons.  Can only be used while Enraged.
        /// </summary>
        public int RagingBlow { get; set; }
        /// <summary>
        /// Increases the critical strike chance of all party and raid members within 12 yds by 5%.  In addition, improves your critical strike chance by an additional 2%.
        /// </summary>
        public int Rampage { get; set; }
        /// <summary>
        /// Heroic Fury - 30 sec cooldown - Instant cast
        /// Removes any Immobilization effects and refreshes the cooldown of your Intercept ability.
        /// </summary>
        public int HeroicFury { get; set; }
        /// <summary>
        /// Your autoattacks have a chance to reduce all healing done to the target by 25% for 10 sec.
        /// </summary>
        public int FuriousAttacks { get; set; }
        /// <summary>
        /// Dealing damage with Cleave or Whirlwind increases the damage of Cleave and Whirlwind by [5 * Pts]% for 10 sec.  This effect stacks up to 0 times.
        /// </summary>
        public int MeatCleaver { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Berserker Rage, Recklessness and Death Wish abilities by [10 * Pts]%.
        /// </summary>
        public int IntensifyRage { get; set; }
        /// <summary>
        /// Your Bloodthirst hits have a [10 * Pts]% chance of making your next Slam instant, free, and deal 20% more damage for 10 sec.
        /// </summary>
        public int Bloodsurge { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Intercept by [5 * Pts] sec and your Heroic Leap by [10 * Pts] sec.
        /// </summary>
        public int Skirmisher { get; set; }
        /// <summary>
        /// Allows you to equip two-handed axes, maces and swords in one hand.
        /// </summary>
        public int TitansGrip { get; set; }
        /// <summary>
        /// When you dual-wield one-handed weapons, you deal 20% additional damage and Slam hits with both weapons.
        /// </summary>
        public int SingleMindedFury { get; set; }
        #endregion
        #region Protection
        /// <summary>
        /// Increases the critical strike chance of your Heroic Strike by [5 * Pts]%, and gives your Heroic Strike criticals a [100 / 3 * Pts]% chance to cause the next Heroic Strike to also be a critical strike.  These guaranteed criticals cannot re-trigger the Incite effect.
        /// </summary>
        public int Incite { get; set; }
        /// <summary>
        /// Increases your armor value from items by [10 / 3 * Pts]%.
        /// </summary>
        public int Toughness { get; set; }
        /// <summary>
        /// When you Thunder Clap a target affected by your Rend, you have a [50 * Pts]% chance to affect every target with Rend.
        /// </summary>
        public int BloodAndThunder { get; set; }
        /// <summary>
        /// You generate [5 * Pts] extra Rage when you block an attack. You generate [20 * Pts] extra Rage when you Spell Reflect a magic attack.
        /// </summary>
        public int ShieldSpecialization { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Shield Block by [10 * Pts] sec, your Shield Wall by [60 * Pts] sec and causes your Shield Block to also reduce magic damage taken by [20 / 3 * Pts]% for 6 sec.
        /// </summary>
        public int ShieldMastery { get; set; }
        /// <summary>
        /// Improves your critical strike and critical block chance by [5 * Pts]% for 10 sec following a successful parry.
        /// </summary>
        public int HoldTheLine { get; set; }
        /// <summary>
        /// Gives your Pummel and Heroic Throw a [50 * Pts]% chance to silence the target for 3 sec.  Also lowers the cooldown on Heroic Throw by [15 * Pts] sec.
        /// </summary>
        public int GagOrder { get; set; }
        /// <summary>
        /// Last Stand - 3 min cooldown - Instant
        /// Temporarily grants you 30% of your maximum health for 20 sec.  After the effect expires, the health is lost.
        /// </summary>
        public int LastStand { get; set; }
        /// <summary>
        /// Concussion Blow - Melee Range - 15 Rage
        /// 30 sec cooldown - Instant cast
        /// Requires Melee Weapon - Stuns the opponent for 5 sec and deals [75% of AP] damage (based on attack power).
        /// </summary>
        public int ConcussionBlow { get; set; }
        /// <summary>
        /// Reduces the chance you'll be critically hit by melee attacks by [3 * Pts]% while in Defensive Stance.  In addition, when you block, dodge or parry an attack you have a [10 * Pts]% chance to become Enraged, increasing physical damage done by [5 * Pts]% for 12 sec.
        /// </summary>
        public int BastionOfDefense { get; set; }
        /// <summary>
        /// Your Charge, Intercept and Intervene abilities are now usable while in combat and in any stance.  In addition, your Intervene ability will remove all movement impairing effects.
        /// </summary>
        //public int Warbringer { get; set; }
        /// <summary>
        /// Increases the damage of your Revenge ability by [30 * Pts]% and causes Revenge to strike an additional target.
        /// </summary>
        public int ImprovedRevenge { get; set; }
        /// <summary>
        /// Devastate - Melee Range - 15 Rage
        /// Instant cast
        /// Requires Shields - Sunder the target\'s armor causing the Sunder Armor effect.  In addition, causes 150% of weapon damage plus 854 for each application of Sunder Armor on the target.  The Sunder Armor effect can stack up to 3 times.
        /// </summary>
        public int Devastate { get; set; }
        /// <summary>
        /// Using Devastate on a target with 20% or less health has a [25 * Pts]% chance to allow the use of Victory Rush but that Victory Rush only heals for 5% of your health.
        /// </summary>
        //public int ImpendingVictory { get; set; }
        /// <summary>
        /// Improves the damage of your Rend, Cleave and Thunder Clap by [3 * Pts]%.  In addition, your Thunder Clap improves the damage of your next Shockwave by [5 * Pts]%.  Stacks up to 3 times.
        /// </summary>
        public int Thunderstruck { get; set; }
        /// <summary>
        /// Vigilance - 30 yd range
        /// Instant cast
        /// Focus your protective gaze on a party or raid member.  Each time they are hit by an attack, your Taunt cooldown is refreshed and you gain Vengeance as if 20% of the damage was done to you.  Lasts 30 min.  This effect can only be on one target at a time.
        /// </summary>
        //public int Vigilance { get; set; }
        /// <summary>
        /// While your Shield Block is active, your Shield Slam hits for an additional [50 * Pts]% damage.
        /// </summary>
        public int HeavyRepercussions { get; set; }
        /// <summary>
        /// Reduces damage taken by the target of your Intervene ability by [15 * Pts]% for 6 sec.
        /// </summary>
        //public int Safeguard { get; set; }
        /// <summary>
        /// Increases the critical strike chance of Devastate by [5 * Pts]%.  In addition, when your Devastate or Revenge deal damage, they have a [10 * Pts]% chance of refreshing the cooldown of your Shield Slam and reducing its cost by 100% for 5 sec.
        /// </summary>
        public int SwordAndBoard { get; set; }
        /// <summary>
        /// Shockwave - 15 Rage
        /// 20 sec cooldown - Instant cast
        /// Sends a wave of force in front of you, causing [75% of AP] damage (based on attack power) and stunning all enemy targets within 10 yards in a frontal cone for 4 sec.
        /// </summary>
        //public int Shockwave { get; set; }
        #endregion
    }

    public partial class PaladinTalents : TalentsBase
    {
        #region Holy
        /// <summary>
        /// Increases the critical effect chance of your Judgement and Templar's Verdict by [6 * Pts]%.
        /// </summary>
        public int ArbiterOfTheLight { get; set; }
        /// <summary>
        /// Casting a targeted heal on any target, except yourself, also heals you for [2605 / 3 * Pts] to [999 * Pts]
        /// </summary>
        public int ProtectorOfTheInnocent { get; set; }
        /// <summary>
        /// Your Judgement increases your casting and melee haste by [3 * Pts]% for 1 min  and your mana regeneration from Spirit while in combat by [10 * Pts]%.
        /// </summary>
        public int JudgementsOfThePure { get; set; }
        /// <summary>
        /// Reduces the casting time of your Holy Light, Divine Light and Holy Radiance by [0.15 * Pts] sec.
        /// </summary>
        public int ClarityOfPurpose { get; set; }
        /// <summary>
        /// Gives your Word of Glory a [30 * Pts]% increased critical chance when used on targets with 35% or less health.
        /// </summary>
        public int LastWord { get; set; }
        /// <summary>
        /// Increases the damage of your Holy Shock and Exorcism by [10 * Pts]%.
        /// </summary>
        public int BlazingLight { get; set; }
        /// <summary>
        /// Reduces the mana cost of Exorcism by [75 / 2 * Pts]%. In addition, you have a [50 * Pts]% chance to cause the Denounce effect on the targets of your Exorcism. The Denounce effect prevents targets from causing critical effects for the next 6 sec.
        /// </summary>
        public int Denounce { get; set; }
        /// <summary>
        /// Divine Favor - 3 min cooldown - Instant
        /// Increases your spell casting haste by 20% and spell critical chance by 20% for 20 sec.
        /// </summary>
        public int DivineFavor { get; set; }
        /// <summary>
        /// Increases the critical effect chance of your Holy Shock by [5 * Pts]%.  In addition, your Holy Shock critical effects reduce the cast time of your next Flash of Light, Holy Light, Divine Light or Holy Radiance by [0.75 * Pts] sec.
        /// </summary>
        public int InfusionOfLight { get; set; }
        /// <summary>
        /// Your Flash of Light, Holy Light and Divine Light have a [10 * Pts]% chance to make your next Holy Shock not trigger a cooldown if used within 12 sec.
        /// </summary>
        public int Daybreak { get; set; }
        /// <summary>
        /// Grants hit rating equal to [50 * Pts]% of any Spirit gained from items or effects, and increases the range of your Judgement by [5 * Pts] yards.  In addition, your Judgement instantly heals you for [1240 * Pts] to [1427 * Pts]
        /// </summary>
        public int EnlightenedJudgements { get; set; }
        /// <summary>
        /// Beacon of Light - 60 yd range - 6% of base mana
        /// Instant cast
        /// The target becomes a Beacon of Light to all targets within a 60 yard radius.  Your Word of Glory, Holy Shock, Flash of Light, Divine Light and Light of Dawn will also heal the Beacon for 50% of the amount healed.  Holy Light will heal for 100% of the amount.  Only one target can be the Beacon of Light at a time. Lasts 5 min.
        /// </summary>
        public int BeaconOfLight { get; set; }
        /// <summary>
        /// Grants [1 * Pts]% spell haste.  In addition, casting Divine Protection increases your movement speed by [20 * Pts]% for 4 sec.
        /// </summary>
        //public int SpeedOfLight { get; set; }
        /// <summary>
        /// Your Cleanse spell now also dispels 1 Magic effect.
        /// </summary>
        public int SacredCleansing { get; set; }
        /// <summary>
        /// Gives you a [1 * Pts]% bonus to damage and healing for 15 sec after causing a critical effect from a weapon swing, non-periodic spell, or ability. This effect stacks up to 0 times.
        /// </summary>
        public int Conviction { get; set; }
        /// <summary>
        /// Aura Mastery - 2 min cooldown - Instant
        /// Causes your Concentration Aura to make all affected targets immune to Silence and Interrupt effects and improve the effect of Devotion Aura, Resistance Aura, and Retribution Aura by 100%.  Lasts 6 sec.
        /// </summary>
        public int AuraMastery { get; set; }
        /// <summary>
        /// Reduces the cooldown of Divine Protection by [15 * Pts] sec, Hand of Sacrifice by [15 * Pts] sec and Avenging Wrath by [30 * Pts] sec.
        /// </summary>
        public int ParagonOfVirtue { get; set; }
        /// <summary>
        /// Healing the target of your Beacon of Light with Flash of Light or Divine Light has a [100 / 3 * Pts]% chance to generate a charge of Holy Power.  In addition, casting Holy Radiance will always generate one charge of Holy Power.
        /// </summary>
        public int TowerOfRadiance { get; set; }
        /// <summary>
        /// You have a [50 * Pts]% chance to gain a charge of Holy Power whenever you take direct damage.  This effect cannot occur more than once every 8 seconds.
        /// </summary>
        public int BlessedLife { get; set; }
        /// <summary>
        /// Light of Dawn - 1 Holy Power
        /// Instant cast
        /// Consumes all Holy Power to send a wave of healing energy before you, healing up to 6 of the most injured targets in your party or raid within a 30 yard frontal cone for 605 to 674 per charge of Holy Power.
        /// </summary>
        public int LightOfDawn { get; set; }
        #endregion
        #region Protection
        /// <summary>
        /// Increases all healing done by you and all healing effects on you by [2 * Pts]%.
        /// </summary>
        public int Divinity { get; set; }
        /// <summary>
        /// Increases the damage done by your Seal of Righteousness, Seal of Truth, and Seal of Justice by [6 * Pts]%.
        /// </summary>
        public int SealsOfThePure { get; set; }
        /// <summary>
        /// Your Word of Glory has a [15 * Pts]% chance not to consume Holy Power.
        /// </summary>
        public int EternalGlory { get; set; }
        /// <summary>
        /// Your Judgement reduces the melee and ranged attack speed of the target by [10 * Pts]% for 20 sec.  In addition, increases the duration of your Seal of Justice effect by [0.5 * Pts] sec.
        /// </summary>
        public int JudgementsOfTheJust { get; set; }
        /// <summary>
        /// Increases your armor value from items by [10 / 3 * Pts]%.
        /// </summary>
        public int Toughness { get; set; }
        /// <summary>
        /// Decreases the cooldown of your Hammer of Justice spell by [10 * Pts] sec.
        /// </summary>
        public int ImprovedHammerOfJustice { get; set; }
        /// <summary>
        /// Increases the damage of your Consecration by [20 * Pts]% and decreases its mana cost by [40 * Pts]%.
        /// </summary>
        public int HallowedGround { get; set; }
        /// <summary>
        /// Reduces the chance you'll be critically hit by melee attacks by [2 * Pts]% and reduces all damage taken by [10 / 3 * Pts]%. In addition when you block or dodge a melee attack you gain [1 * Pts]% of maximum mana.
        /// </summary>
        public int Sanctuary { get; set; }
        /// <summary>
        /// Hammer of the Righteous - Melee Range - 10% of base mana
        /// 4.5 sec cooldown - Instant cast
        /// Requires One-Handed Melee Weapon - Hammer the current target for 30% weapon damage, causing a wave of light that hits all targets within 8 yards for 583 to 874 Holy damage. Grants a charge of Holy Power.
        /// </summary>
        public int HammerOfTheRighteous { get; set; }
        /// <summary>
        /// Increases the damage of your Crusader Strike and Judgement abilities by [50 * Pts]%, and increases the critical strike chance of your Holy Wrath and Hammer of Wrath spells by [15 * Pts]%.
        /// </summary>
        public int WrathOfTheLightbringer { get; set; }
        /// <summary>
        /// You have a [10 * Pts]% chance after blocking an attack for your next 4 weapon swings within 8 sec to generate an additional attack.
        /// </summary>
        public int Reckoning { get; set; }
        /// <summary>
        /// Shield of the Righteous - Melee Range - 1 Holy Power
        /// Instant cast
        /// Requires Shields - Slam the target with your shield, causing Holy damage.  Consumes all charges of Holy Power to determine damage dealt:
        /// 
        /// 1 Holy Power: 609 damage
        /// 2 Holy Power: 1827 damage
        /// 3 Holy Power: 3654 damage
        /// </summary>
        public int ShieldOfTheRighteous { get; set; }
        /// <summary>
        /// When your Crusader Strike or Hammer of the Righteous deal damage to your primary target, they have a [10 * Pts]% chance of refreshing the cooldown on your next Avenger's Shield and causing it to generate a charge of Holy Power if used within 6 sec.
        /// </summary>
        public int GrandCrusader { get; set; }
        /// <summary>
        /// Your Crusader Strike and Hammer of the Righteous reduce physical damage done by their primary targets by 10% for 30 sec.
        /// </summary>
        public int Vindication { get; set; }
        /// <summary>
        /// Holy Shield - 3% of base mana
        /// 30 sec cooldown - Instant
        /// Requires Shields - Increases the amount your shield blocks by an additional 20% for 10 sec.
        /// </summary>
        public int HolyShield { get; set; }
        /// <summary>
        /// Increases your Word of Glory by [5 * Pts]% when used to heal yourself.  In addition, any overhealing will create a protective shield equal to the amount of overhealing that lasts for 6 sec.
        /// </summary>
        public int GuardedByTheLight { get; set; }
        /// <summary>
        /// Divine Guardian - 100 yd range
        /// 3 min cooldown - Instant
        /// All party or raid members within 30 yards, excluding the Paladin, take 20% reduced damage for 6 sec.
        /// </summary>
        public int DivineGuardian { get; set; }
        /// <summary>
        /// s Shield have a 50% chance of [25 * Pts] your next Shield of the Righteous a critical strike.  Lasts 10 sec."
        /// </summary>
        public int SacredDuty { get; set; }
        /// <summary>
        /// Reduces the cooldown of Avenging Wrath by [20 * Pts] sec and Guardian of Ancient Kings by [40 * Pts] sec.  In addition, your Divine Plea will generate [1 * Pts] Holy Power.
        /// </summary>
        public int ShieldOfTheTemplar { get; set; }
        /// <summary>
        /// Ardent Defender - 3 min cooldown - Instant
        /// Reduce damage taken by 20% for 10 sec. While Ardent Defender is active, the next attack that would otherwise kill you will instead cause you to be healed for 15% of your maximum health.
        /// </summary>
        public int ArdentDefender { get; set; }
        #endregion
        #region Retribution
        /// <summary>
        /// All magic attacks against you have a [20 * Pts]% chance to cause 30% of the damage taken back to the attacker as well.
        /// </summary>
        public int EyeForAnEye { get; set; }
        /// <summary>
        /// Increases the damage of your Crusader Strike, Hammer of the Righteous, and Templar's Verdict by [10 * Pts]%, and the damage and healing of your Holy Shock by [10 * Pts]%.  In addition, for 15 sec after you kill an enemy that yields experience or honor, your next Holy Light heals for an additional [100 * Pts]%.
        /// </summary>
        public int Crusade { get; set; }
        /// <summary>
        /// Increases the range of your Judgement by [10 * Pts] yards.
        /// </summary>
        public int ImprovedJudgement { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Hand of Protection by [2 / 2 * Pts] min and increases the duration of your Hand of Freedom by [2 * Pts] sec.
        /// </summary>
        public int GuardiansFavor { get; set; }
        /// <summary>
        /// Increases the critical effect chance of your Crusader Strike, Hammer of the Righteous and Word of Glory by [5 * Pts]%.
        /// </summary>
        public int RuleOfLaw { get; set; }
        /// <summary>
        /// You have a [50 * Pts]% chance to gain a charge of Holy Power when struck by a Stun, Fear or Immobilize effect.  In addition, increases your movement and mounted movement speed by [15 / 2 * Pts]%.  This effect does not stack with other movement speed increasing effects.
        /// </summary>
        //public int PursuitOfJustice { get; set; }
        /// <summary>
        /// Your auras increase your party and raid's damage dealt by 3%, and your own damage is increased by an additional 2% at all times.  In addition, your Judgement causes Replenishment.
        /// 
        /// Replenishment - Grants up to 10 party or raid members mana regeneration equal to 1% of their maximum mana per 10 sec. Lasts for 15 sec.
        /// </summary>
        public int Communion { get; set; }
        /// <summary>
        /// Your autoattacks have a [20 / 3 * Pts]% chance to make your next Exorcism instant, free and cause 100% additional damage.
        /// </summary>
        public int TheArtOfWar { get; set; }
        /// <summary>
        /// Your Judgement has a [50 * Pts]% chance to increase your movement speed by 45% for 4 sec when used on targets at or further than 15 yards from you.
        /// </summary>
        //public int LongArmOfTheLaw { get; set; }
        /// <summary>
        /// Divine Storm - 5% of base mana
        /// 4.5 sec cooldown - Instant cast
        /// Requires Melee Weapon - An instant attack that causes 100% weapon damage to all enemies within 8 yards.  The Divine Storm heals up to 3 party or raid members totaling 25% of the damage caused, and will grant a charge of Holy Power if it hits 4 or more targets.
        /// </summary>
        public int DivineStorm { get; set; }
        /// <summary>
        /// When reduced below 30% health, you gain the Sacred Shield effect. The Sacred Shield absorbs [1 + 2.8 * AP] damage and increases healing received by 20%.  Lasts 15 sec.  This effect cannot occur more than once every 60 sec.
        /// </summary>
        //public int SacredShield { get; set; }
        /// <summary>
        /// Haste effects lower the cooldown of your Crusader Strike and Divine Storm abilities.
        /// </summary>
        public int SanctityOfBattle { get; set; }
        /// <summary>
        /// Your Seal of Righteousness, Seal of Truth, and Seal of Justice now also deal 7% weapon damage when triggered. In addition, your Seal of Righteousness now hits all enemy targets within melee range.
        /// </summary>
        public int SealsOfCommand { get; set; }
        /// <summary>
        /// Increases the critical strike chance of Hammer of Wrath by [2 * Pts]%, reduces the cooldown of Avenging Wrath by [20 * Pts] secs and allows the use of Hammer of Wrath at all times during Avenging Wrath.
        /// </summary>
        //public int SanctifiedWrath { get; set; }
        /// <summary>
        /// Reduces the cooldown of Word of Glory by [5 * Pts] sec. In addition, when you heal others with your Word of Glory, it increases the effectiveness of the heal by [25 * Pts]% and increases your damage done by [2 * Pts]% per charge of Holy Power for 10 sec.
        /// </summary>
        //public int SelflessHealer { get; set; }
        /// <summary>
        /// Repentance - 30 yd range - 9% of base mana
        /// 1 min cooldown - Instant cast
        /// Puts the enemy target in a state of meditation, incapacitating them for up to 1 min.  Any damage from sources other than Censure will awaken the target.  Usable against Demons, Dragonkin, Giants, Humanoids and Undead.
        /// </summary>
        //public int Repentance { get; set; }
        /// <summary>
        /// The following attacks have a [15 / 2 * Pts]% chance to cause your next Holy Power ability to consume no Holy Power and to cast as if 3 Holy Power were consumed:
        /// 
        /// - Judgement
        /// - Exorcism
        /// - Templar's Verdict
        /// - Divine Storm
        /// - Inquisition
        /// - Holy Wrath
        /// - Hammer of Wrath
        /// </summary>
        //public int DivinePurpose { get; set; }
        /// <summary>
        /// Increases the periodic damage done by your Seal of Truth by [10 * Pts]%, and the duration of your Inquisition by [200 / 3 * Pts]%.
        /// </summary>
        public int InquiryOfFaith { get; set; }
        /// <summary>
        /// Reduces the cooldown by [10 * Pts]% and mana cost by [10 * Pts]% of your Hand of Freedom, Hand of Salvation and Hand of Sacrifice.  In addition, your Cleanse will remove one movement impairing effect if cast on yourself.
        /// </summary>
        public int ActsOfSacrifice { get; set; }
        /// <summary>
        /// Zealotry - 3 Holy Power
        /// 2 min cooldown - Instant
        /// Your Crusader Strike generates 3 charges of Holy Power per strike for the next 20 sec.  Requires 3 Holy Power to use, but does not consume Holy Power.
        /// </summary>
        public int Zealotry { get; set; }
        #endregion
    }

    public partial class HunterTalents : TalentsBase
    {
        #region Beast Mastery
        /// <summary>
        /// Increases the critical strike chance of your Kill Command by [5 * Pts]%.
        /// </summary>
        public int ImprovedKillCommand { get; set; }
        /// <summary>
        /// Increases the attack power bonus of your Aspect of the Hawk by [10 * Pts]%, and increases the amount of Focus restored by your Aspect of the Fox by [1 * Pts]
        /// </summary>
        public int OneWithNature { get; set; }
        /// <summary>
        /// Increases the Focus regeneration of your pets by [10 * Pts]%.
        /// </summary>
        public int BestialDiscipline { get; set; }
        /// <summary>
        /// Increases the speed bonus of your Aspect of the Cheetah and Aspect of the Pack by [4 * Pts]%, and increases your speed while mounted by [5 * Pts]%. The mounted movement speed increase does not stack with other effects.
        /// </summary>
        public int Pathfinding { get; set; }
        /// <summary>
        /// While your pet is active, you and your pet will regenerate [1 * Pts]% of total health every [5 * Pts] sec., and increases healing done to you and your pet by 10%.
        /// </summary>
        //public int SpiritBond { get; set; }
        /// <summary>
        /// Your pet gains [2 * Pts]% attack speed after attacking with a Basic Attack, lasting for 10 sec and stacking up to 0 times.
        /// </summary>
        public int Frenzy { get; set; }
        /// <summary>
        /// Gives the Mend Pet ability a [25 * Pts]% chance of cleansing 1 Curse, Disease, Magic or Poison effect from the pet each tick.
        /// </summary>
        public int ImprovedMendPet { get; set; }
        /// <summary>
        /// You have a [5 * Pts]% chance when you hit with Arcane Shot to cause your pet's next 2 Basic Attacks to critically hit.
        /// </summary>
        public int CobraStrikes { get; set; }
        /// <summary>
        /// Fervor - 2 min cooldown - Instant cast
        /// Instantly restores 50 Focus to you and your pet.
        /// </summary>
        //public int Fervor { get; set; }
        /// <summary>
        /// Focus Fire - 15 sec cooldown - Instant
        /// Consumes your pet\'s Frenzy Effect stack, restoring 4 Focus to your pet and increasing your ranged haste by 3% for each Frenzy Effect stack consumed. Lasts for 20 sec.
        /// </summary>
        public int FocusFire { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Bestial Wrath, Intimidation and Pet Special Abilities by [10 * Pts]%.
        /// </summary>
        public int Longevity { get; set; }
        /// <summary>
        /// When you score two Kill Command critical hits in a row, your third will deal [10 * Pts]% more damage and have its cost reduced by [5 * Pts] Focus.
        /// </summary>
        public int KillingStreak { get; set; }
        /// <summary>
        /// Whenever you are hit by a melee attack, the cooldown of your Disengage is instantly reduced by [2 * Pts] sec.
        /// 
        /// Whenever you are hit by a ranged attack or spell, the cooldown of your Deterrence is instantly reduced by [4 * Pts] sec.
        /// 
        /// These effects have a 2 sec cooldown.
        /// </summary>
        //public int CrouchingTigerHiddenChimera { get; set; }
        /// <summary>
        /// Bestial Wrath - 100 yd range
        /// 2 min cooldown - Instant
        /// Send your pet into a rage causing 20% additional damage for 10 sec.  The beast does not feel pity or remorse or fear and it cannot be stopped unless killed.
        /// </summary>
        public int BestialWrath { get; set; }
        /// <summary>
        /// All party and raid members have all damage increased by 3% within 12 yards of your pet.
        /// </summary>
        public int FerociousInspiration { get; set; }
        /// <summary>
        /// Increases you and your pet's maximum Focus by [5 * Pts]
        /// </summary>
        public int KindredSpirits { get; set; }
        /// <summary>
        /// While your pet is under the effects of Bestial Wrath, you also go into a rage causing 10% additional damage and reducing the focus cost of all shots and abilities by 50% for 10 sec.
        /// </summary>
        public int TheBeastWithin { get; set; }
        /// <summary>
        /// When your pet scores a critical hit with a Basic Attack, you instantly regenerate [3 * Pts] Focus.
        /// </summary>
        public int Invigoration { get; set; }
        /// <summary>
        /// Beast Mastery - You master the art of beast training, teaching you the ability to tame Exotic pets and increasing your total number of Pet Skill Points by 4.
        /// </summary>
        public int BeastMastery { get; set; }
        #endregion
        #region Marksmanship
        /// <summary>
        /// Your ranged auto-shot critical hits cause your pet to generate [5 * Pts] Focus.
        /// </summary>
        public int GoForTheThroat { get; set; }
        /// <summary>
        /// Reduces the Focus cost of your Arcane Shot by [1 * Pts] and your Explosive Shot and Chimera Shot by [2 * Pts]
        /// </summary>
        public int Efficiency { get; set; }
        /// <summary>
        /// After killing an opponent that yields experience or honor, your next Aimed Shot, Steady Shot or Cobra Shot causes [10 * Pts]% additional damage.  Lasts 20 sec.
        /// </summary>
        public int RapidKilling { get; set; }
        /// <summary>
        /// When you critically hit with your Arcane Shot, Aimed Shot or Explosive Shot the Focus cost of your pet's next Basic Attack is reduced by [50 * Pts]% for 12 sec.
        /// </summary>
        public int SicEm { get; set; }
        /// <summary>
        /// When you Steady Shot twice in a row, your ranged attack speed will be increased by [5 * Pts]% for 8 sec.
        /// </summary>
        public int ImprovedSteadyShot { get; set; }
        /// <summary>
        /// Increases the critical strike chance of your Steady Shot, Cobra Shot and Aimed Shot by [30 * Pts]% on targets who are above 90% health.
        /// </summary>
        public int CarefulAim { get; set; }
        /// <summary>
        /// Silencing Shot - 5-35 yd range
        /// 20 sec cooldown - Instant
        /// Requires Ranged Weapon - A shot that silences the target and interrupts spellcasting for 3 sec.
        /// </summary>
        //public int SilencingShot { get; set; }
        /// <summary>
        /// Your successful Chimera Shot and Multi-Shot attacks have a [50 * Pts]% chance to daze the target for 4 sec.
        /// </summary>
        public int ConcussiveBarrage { get; set; }
        /// <summary>
        /// Your critical Aimed, Steady and Chimera Shots cause the target to bleed for [10 * Pts]% of the damage dealt over 8 sec.
        /// </summary>
        public int PiercingShots { get; set; }
        /// <summary>
        /// When you critically hit with Multi-Shot, the focus cost of Multi-Shot is reduced by [25 * Pts]% for 5 sec.
        /// </summary>
        public int Bombardment { get; set; }
        /// <summary>
        /// Increases melee attack power by 20% and ranged attack power by 10% of party and raid members within 12 yards.
        /// </summary>
        public int TrueshotAura { get; set; }
        /// <summary>
        /// Your Steady Shot and Cobra Shot abilities grant an additional [3 * Pts] Focus when dealt on targets at or below 25% health.
        /// </summary>
        public int Termination { get; set; }
        /// <summary>
        /// When your marked target attempts to run, flee or move, you have a [4 * Pts]% chance to cause your next Kill Command on the marked target within 8 sec to refund the Focus cost.
        /// </summary>
        public int ResistanceIsFutile { get; set; }
        /// <summary>
        /// You gain [6 * Pts] focus every 3 sec while under the effect of Rapid Fire, and you gain [25 * Pts] Focus instantly when you gain Rapid Killing.
        /// </summary>
        public int RapidRecuperation { get; set; }
        /// <summary>
        /// You have a [20 * Pts]% chance when you Steady Shot to gain the Master Marksman effect, lasting 30 sec. After reaching 0 stacks, your next Aimed Shot's cast time and focus cost are reduced by 100% for 10 sec.
        /// </summary>
        public int MasterMarksman { get; set; }
        /// <summary>
        /// Readiness - 3 min cooldown - Instant cast
        /// When activated, this ability immediately finishes the cooldown on all Hunter abilities.
        /// </summary>
        //public int Readiness { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Rapid Fire by [1 * Pts] min, and your movement speed is increased by [15 * Pts]% for 4 sec after you use Disengage.
        /// </summary>
        //public int Posthaste { get; set; }
        /// <summary>
        /// Your Arcane Shot and Chimera Shot have a [50 * Pts]% chance to automatically apply the Marked for Death effect.
        /// 
        /// Marked for Death is the same as Hunter's Mark, but is undispellable, does not grant unerring sight of the target, and lasts for 15 sec.
        /// </summary>
        public int MarkedForDeath { get; set; }
        /// <summary>
        /// Chimera Shot - 5-40 yd range - 50 Focus
        /// 10 sec cooldown - Instant cast
        /// Requires Ranged Weapon - An instant shot that causes ranged weapon Nature damage plus [1620 + 73.2% of RAP], refreshing the duration of your Serpent Sting and healing you for 5% of your total health.
        /// </summary>
        public int ChimeraShot { get; set; }
        #endregion
        #region Survival
        /// <summary>
        /// Increases your total Stamina by [5 * Pts]%.
        /// </summary>
        public int HunterVsWild { get; set; }
        /// <summary>
        /// Increases ranged haste by [1 * Pts]%.
        /// </summary>
        public int Pathing { get; set; }
        /// <summary>
        /// Your Serpent Sting also does instant damage equal to [15 * Pts]% of its total periodic effect.
        /// 
        /// Also increases the periodic critical strike chance of your Serpent Sting by [5 * Pts]%.
        /// </summary>
        public int ImprovedSerpentSting { get; set; }
        /// <summary>
        /// Reduces the chance that your trap spells will be resisted by [2 * Pts]%, and reduces the cooldown of your Disengage ability by [2 * Pts] sec.
        /// </summary>
        public int SurvivalTactics { get; set; }
        /// <summary>
        /// Ice Trap and Freezing Trap - Increases the duration by [10 * Pts]%.
        /// 
        /// Immolation Trap, Explosive Trap and Black Arrow - Increases the periodic damage done by [10 * Pts]%.
        /// 
        /// Snake Trap - Increases the number of snakes summoned by [2 * Pts]
        /// </summary>
        public int TrapMastery { get; set; }
        /// <summary>
        /// When your Ice Trap or Snake Trap are triggered you entrap all afflicted targets, preventing them from moving for [2 * Pts] sec.
        /// </summary>
        public int Entrapment { get; set; }
        /// <summary>
        /// Increases the ranged critical strike chance of all of your attacks on targets affected by your Ice Trap and Freezing Trap by [3 * Pts]%.
        /// </summary>
        public int PointOfNoEscape { get; set; }
        /// <summary>
        /// You have a [5 * Pts]% chance when you use Arcane Shot, Explosive Shot or Black Arrow to instantly regain 40% of the base Focus cost of the shot.
        /// </summary>
        //public int ThrillOfTheHunt { get; set; }
        /// <summary>
        /// Counterattack - Melee Range
        /// 5 sec cooldown - Instant cast
        /// A strike that becomes active after parrying an opponent\'s attack.  This attack deals [320 + 20% of AP] damage and immobilizes the target for 5 sec.  Counterattack cannot be blocked, dodged, or parried.
        /// </summary>
        public int Counterattack { get; set; }
        /// <summary>
        /// You have a [50 * Pts]% chance when you trap a target with Freezing Trap or Ice Trap to cause your next two Explosive Shots to cost no Focus and trigger no cooldown. Effect lasts for 12 sec.
        /// </summary>
        public int LockAndLoad { get; set; }
        /// <summary>
        /// Reduces the cooldown of all traps and Black Arrow by [2 * Pts] sec.
        /// </summary>
        public int Resourcefulness { get; set; }
        /// <summary>
        /// When attacked by a spell while in Deterrence, you have a [50 * Pts]% chance to reflect it back at the attacker.
        /// </summary>
        public int MirroredBlades { get; set; }
        /// <summary>
        /// When you deal periodic damage with your Immolation Trap, Explosive Trap or Black Arrow you have a [10 * Pts]% chance to trigger Lock and Load.
        /// </summary>
        public int TNT { get; set; }
        /// <summary>
        /// Increases the periodic critical damage of your Serpent Sting and Black Arrow by [50 * Pts]%.
        /// </summary>
        public int Toxicology { get; set; }
        /// <summary>
        /// Wyvern Sting - 5-35 yd range - 10 Focus
        /// 1 min cooldown - Instant cast
        /// Requires Ranged Weapon - A stinging shot that puts the target to sleep for 30 sec.  Any damage will cancel the effect.  When the target wakes up, the Sting causes 2736 Nature damage over 6 sec.  Only one Sting per Hunter can be active on the target at a time.
        /// </summary>
        //public int WyvernSting { get; set; }
        /// <summary>
        /// Increases your ranged damage done on targets afflicted by your Serpent Sting by [5 * Pts]%.
        /// 
        /// If Wyvern Sting is dispelled, the dispeller is also afflicted by Wyvern Sting lasting [25 * Pts]% of the duration remaining.
        /// </summary>
        public int NoxiousStings { get; set; }
        /// <summary>
        /// Increases your total Agility by an additional 2%, and increases the ranged and melee attack speed of all party and raid members by 10%.
        /// </summary>
        public int HuntingParty { get; set; }
        /// <summary>
        /// Increases the critical strike chance of your Kill Shot ability by [5 * Pts]%, and after remaining stationary for [2 * Pts] sec, your Steady Shot and Cobra Shot deal 6% more damage for 15 sec.
        /// </summary>
        public int SniperTraining { get; set; }
        /// <summary>
        /// Targets hit by your Multi-Shot are also afflicted by your Serpent Sting equal to [9 / 2 * Pts] sec of its total duration.
        /// </summary>
        public int SerpentSpread { get; set; }
        /// <summary>
        /// Black Arrow - 5-40 yd range - 35 Focus
        /// 30 sec cooldown - Instant cast
        /// Requires Ranged Weapon - Fires a Black Arrow at the target, dealing 2850 Shadow damage over 20 sec. Black Arrow shares a cooldown with other Fire Trap spells.
        /// </summary>
        public int BlackArrow { get; set; }
        #endregion
    }

    public partial class RogueTalents : TalentsBase
    {
        #region Assassination
        /// <summary>
        /// After killing an opponent that yields experience or honor, the critical strike [50 * Pts] of [15 * Pts] next ability within 15 sec is increased by 40% and your Slice and [40 * Pts] and Recuperate abilities are refreshed to their original duration.
        /// </summary>
        public int DeadlyMomentum { get; set; }
        /// <summary>
        /// Increases the damage done by your Eviscerate and Envenom abilities by [20 / 3 * Pts]%.
        /// </summary>
        public int CoupDeGrace { get; set; }
        /// <summary>
        /// Increases the critical strike damage bonus of your Sinister Strike, Backstab, Mutilate and Hemorrhage abilities by [10 * Pts]%.
        /// </summary>
        public int Lethality { get; set; }
        /// <summary>
        /// Gives your melee finishing moves a [20 * Pts]% chance to add a combo point to your target.
        /// </summary>
        public int Ruthlessness { get; set; }
        /// <summary>
        /// All healing effects on you are increased by [10 * Pts]% and your movement speed is increased by [15 / 2 * Pts]%.  This does not stack with most other movement speed increasing effects.
        /// </summary>
        public int Quickening { get; set; }
        /// <summary>
        /// Increases the critical strike chance of your Backstab ability by [10 * Pts]%, and the critical strike chance of your Mutilate ability by [5 * Pts]%.
        /// </summary>
        public int PuncturingWounds { get; set; }
        /// <summary>
        /// Even after your Sap wears off, its effects linger on enemies, reducing their damage done by [35 * Pts]% for 8 sec.
        /// </summary>
        public int Blackjack { get; set; }
        /// <summary>
        /// When you apply Instant, Wound, or Mind-Numbing Poison to a target, you have a [50 * Pts]% chance to apply Crippling Poison.
        /// </summary>
        //public int DeadlyBrew { get; set; }
        /// <summary>
        /// Cold Blood - 2 min cooldown - Instant
        /// When activated, generates 25 Energy and increases the critical strike chance of your next non-periodic offensive ability by 100%.
        /// </summary>
        public int ColdBlood { get; set; }
        /// <summary>
        /// Increases the damage dealt by your poisons by [12 * Pts]% and gives you [100 / 3 * Pts]% of the normal chance of applying poisons from your equipped melee weapons when you use the Fan of Knives ability.
        /// </summary>
        public int VilePoisons { get; set; }
        /// <summary>
        /// Reduces all damage taken by [10 / 3 * Pts]%.
        /// </summary>
        public int DeadenedNerves { get; set; }
        /// <summary>
        /// Your critical strikes from abilities that add combo points have a [50 * Pts]% chance to add an additional combo point.
        /// </summary>
        public int SealFate { get; set; }
        /// <summary>
        /// When you Backstab an enemy that is below 35% health, you instantly recover [15 * Pts] Energy.
        /// </summary>
        public int MurderousIntent { get; set; }
        /// <summary>
        /// While stealthed, and for 20 seconds after breaking stealth, you regenerate 30% additional energy.
        /// </summary>
        public int Overkill { get; set; }
        /// <summary>
        /// Increases the spell damage taken by any target you have poisoned by 8%, causes your Envenom ability to no longer consume Deadly Poison, and reduces the duration of all Poison effects applied to you by 50%.
        /// </summary>
        public int MasterPoisoner { get; set; }
        /// <summary>
        /// Gives a [50 * Pts]% chance to refund all combo points used when performing your Expose Armor ability.
        /// </summary>
        public int ImprovedExposeArmor { get; set; }
        /// <summary>
        /// Your Eviscerate and Envenom abilities have a [100 / 3 * Pts]% chance to refresh your Slice and Dice duration to its 5 combo point maximum.
        /// </summary>
        public int CutToTheChase { get; set; }
        /// <summary>
        /// Each time your Rupture or Garrote deals damage to an enemy that you have poisoned, you have a [30 * Pts]% chance to deal 675 additional Nature damage and to regain 10 Energy.  If an enemy dies while afflicted by your Rupture, you regain energy proportional to the remaining Rupture duration.
        /// </summary>
        public int VenomousWounds { get; set; }
        /// <summary>
        /// Vendetta - 30 yd range
        /// 2 min cooldown - Instant cast
        /// Marks an enemy for death, increasing all damage you deal to the target by 20% and granting you unerring vision of your target, regardless of concealments such as stealth and invisibility.  Lasts 30 sec.
        /// </summary>
        public int Vendetta { get; set; }
        #endregion
        #region Combat
        /// <summary>
        /// Causes your Recuperate ability to restore an additional [0.5 * Pts]% of your maximum health and reduces all damage taken by [3 * Pts]% while your Recuperate ability is active.
        /// </summary>
        public int ImprovedRecuperate { get; set; }
        /// <summary>
        /// Increases the damage dealt by your Sinister Strike ability by [10 * Pts]% and reduces its Energy cost by [2 * Pts]
        /// </summary>
        public int ImprovedSinisterStrike { get; set; }
        /// <summary>
        /// Increases your chance to hit with weapon and poison attacks by [2 * Pts]%.
        /// </summary>
        public int Precision { get; set; }
        /// <summary>
        /// Increases the duration of your Slice and Dice ability by [25 * Pts]%.
        /// </summary>
        public int ImprovedSliceAndDice { get; set; }
        /// <summary>
        /// Gives a [50 * Pts]% chance to remove all movement-impairing effects when you activate your Sprint ability.
        /// </summary>
        public int ImprovedSprint { get; set; }
        /// <summary>
        /// Increases the damage of your Sinister Strike, Backstab, and Eviscerate abilities by [20 / 3 * Pts]%.
        /// </summary>
        public int Aggression { get; set; }
        /// <summary>
        /// Causes your Kick ability to silence the target for 3 [1 * Pts]
        /// </summary>
        public int ImprovedKick { get; set; }
        /// <summary>
        /// Increases your chance to dodge enemy attacks by [3 * Pts]% and your attack speed by [2 * Pts]%.
        /// </summary>
        public int LightningReflexes { get; set; }
        /// <summary>
        /// Revealing Strike - Melee Range - 40 Energy
        /// Instant cast
        /// Requires Melee Weapon - An instant strike that causes 125% of your normal weapon damage and increases the effectiveness of your next offensive finishing move on that target by 35% for 15 sec.  Awards 1 combo point.
        /// </summary>
        public int RevealingStrike { get; set; }
        /// <summary>
        /// Increases your armor contribution from cloth and leather items by [25 * Pts]%.
        /// </summary>
        public int ReinforcedLeather { get; set; }
        /// <summary>
        /// Increases the effect duration of your Gouge ability by [1 * Pts] sec and reduces its Energy cost by [15 * Pts]
        /// </summary>
        public int ImprovedGouge { get; set; }
        /// <summary>
        /// Gives your successful off-hand melee attacks and Main Gauche attacks a 20% chance to generate [5 * Pts] Energy.
        /// </summary>
        public int CombatPotency { get; set; }
        /// <summary>
        /// Gives your damaging melee attacks a [20 * Pts]% chance to daze the target, reducing movement speed by 70% for [4 * Pts] sec.
        /// </summary>
        public int BladeTwisting { get; set; }
        /// <summary>
        /// Increases the range of Throw and Deadly Throw by [5 * Pts] yards and gives your Deadly Throw a [50 * Pts]% chance to interrupt the target for 3 sec.
        /// </summary>
        public int ThrowingSpecialization { get; set; }
        /// <summary>
        /// Adrenaline Rush - 3 min cooldown - Instant
        /// Increases your Energy regeneration rate by 100% and your melee attack speed by 20% for 15 sec.
        /// </summary>
        public int AdrenalineRush { get; set; }
        /// <summary>
        /// Increases your total attack power by [3 * Pts]% and all physical damage caused to enemies you have poisoned is increased by [2 * Pts]%.
        /// </summary>
        public int SavageCombat { get; set; }
        /// <summary>
        /// Your Sinister Strike and Revealing Strike abilities have a [100 / 3 * Pts]% chance to grant you an evolving insight into an opponent's defenses, increasing damage to that target by up to 30%.  Opponents can adapt over time, negating this benefit, and Striking a different opponent will begin the cycle anew.
        /// </summary>
        public int BanditsGuile { get; set; }
        /// <summary>
        /// Your damaging finishing moves reduce the cooldown of your Adrenaline Rush, Killing Spree, Redirect, and Sprint abilities by [1 * Pts] sec per combo point.
        /// </summary>
        public int RestlessBlades { get; set; }
        /// <summary>
        /// Killing Spree - 10 yd range
        /// 2 min cooldown - Instant cast
        /// Requires Melee Weapon - Step through the shadows from enemy to enemy within 10 yards, attacking an enemy every 0.5 sec with both weapons until 5 assaults are made, and increasing all damage done by 20% for the duration.  Can hit the same target multiple times.  Cannot hit invisible or stealthed targets.
        /// </summary>
        public int KillingSpree { get; set; }
        #endregion
        #region Subtlety
        /// <summary>
        /// Increases your speed while stealthed by [5 * Pts]% and reduces the cooldown of your Stealth ability by [2 * Pts] sec.
        /// </summary>
        //public int Nightstalker { get; set; }
        /// <summary>
        /// Increases the critical strike chance of your Ambush ability by [20 * Pts]% and its damage by [5 * Pts]%.
        /// </summary>
        public int ImprovedAmbush { get; set; }
        /// <summary>
        /// Your finishing moves have a [20 / 3 * Pts]% chance per combo point to restore 25 Energy.
        /// </summary>
        public int RelentlessStrikes { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Vanish and Blind abilities by [30 * Pts] sec, and your Cloak of Shadows and Combat Readiness abilities by [15 * Pts] sec.
        /// </summary>
        //public int Elusiveness { get; set; }
        /// <summary>
        /// Your Ambush and Backstab hits have a [50 * Pts]% chance to unbalance a target, increasing the time between their melee and ranged attacks by 20%, and reducing movement speed by 50% for 8 sec.
        /// </summary>
        public int Waylay { get; set; }
        /// <summary>
        /// Increases the damage dealt with your Backstab, Mutilate, Garrote and Ambush abilities by [10 * Pts]%.
        /// </summary>
        public int Opportunity { get; set; }
        /// <summary>
        /// Gives you a [50 * Pts]% chance to add an additional combo point to your target when using your Ambush, Garrote, or Cheap Shot ability.
        /// </summary>
        public int Initiative { get; set; }
        /// <summary>
        /// Empowers your Recuperate ability, causing its periodic effect to also restore [4 * Pts] Energy.
        /// </summary>
        public int EnergeticRecovery { get; set; }
        /// <summary>
        /// Your Ambush, Garrote, and Cheap Shot abilities reveal a flaw in your target's defenses, causing all your attacks to bypass [35 * Pts]% of that enemy's armor for 10 sec.
        /// </summary>
        public int FindWeakness { get; set; }
        /// <summary>
        /// Hemorrhage - Melee Range - 35 Energy
        /// Instant cast
        /// Requires Melee Weapon - An instant strike that deals 155% weapon damage (225% if a dagger is equipped) and causes the target to take 30% additional damage from Bleed effects for 1 min.  Awards 1 combo point.
        /// </summary>
        public int Hemorrhage { get; set; }
        /// <summary>
        /// Increases the critical strike chance of all party and raid members by 5%. When any player in your party or raid critically hits with a spell or ability, you have a [100 / 3 * Pts]% chance to gain a combo point on your current target.  This effect cannot occur more than once every [2 / 3 * Pts] seconds.
        /// </summary>
        public int HonorAmongThieves { get; set; }
        /// <summary>
        /// Premeditation - 30 yd range
        /// 20 sec cooldown - Instant
        /// When used, adds 2 combo points to your target.  You must add to or use those combo points within 20 sec or the combo points are lost.
        /// </summary>
        public int Premeditation { get; set; }
        /// <summary>
        /// Reduces the damage taken by area of effect attacks by [10 * Pts]% and increases the duration of your Feint ability by [1 * Pts] sec.
        /// </summary>
        public int EnvelopingShadows { get; set; }
        /// <summary>
        /// You have a [100 / 3 * Pts]% chance that an attack which would otherwise kill you will instead reduce you to 10% of your maximum health. In addition, all damage taken will be reduced by 80% for 3 sec.  This effect cannot occur more than once per 90 seconds.
        /// </summary>
        //public int CheatDeath { get; set; }
        /// <summary>
        /// Preparation - 5 min cooldown - Instant cast
        /// When activated, this ability immediately finishes the cooldown on your Sprint, Vanish, and Shadowstep abilities.
        /// </summary>
        //public int Preparation { get; set; }
        /// <summary>
        /// Increases your damage dealt to targets with a Bleed effect on them by [8 * Pts]% and gives your Bleed effects a [50 * Pts]% chance to not break your Gouge.
        /// </summary>
        public int SanguinaryVein { get; set; }
        /// <summary>
        /// Reduces the Energy cost of your Backstab and Ambush abilities by [20 / 3 * Pts] and the Energy cost of your Hemorrhage and Fan of Knives abilities by [2 * Pts]
        /// </summary>
        public int SlaughterFromTheShadows { get; set; }
        /// <summary>
        /// Your Eviscerate has a [10 * Pts]% chance per combo point to refresh your Rupture on the target to its original duration.
        /// </summary>
        public int SerratedBlades { get; set; }
        /// <summary>
        /// Shadow Dance - 1 min cooldown - Instant
        /// Enter the Shadow Dance for 6 sec, allowing the use of Sap, Garrote, Ambush, Cheap Shot, Premeditation, Pick Pocket, and Disarm Trap regardless of being stealthed.
        /// </summary>
        public int ShadowDance { get; set; }
        #endregion
    }

    public partial class PriestTalents : TalentsBase
    {
        #region Discipline
        /// <summary>
        /// Increases the damage absorbed by your Power Word: Shield by [10 * Pts]%.
        /// </summary>
        public int ImprovedPowerWordShield { get; set; }
        /// <summary>
        /// Increases your Shadow and Holy spell damage and healing by [2 * Pts]%.
        /// </summary>
        public int TwinDisciplines { get; set; }
        /// <summary>
        /// Reduces the mana cost of your instant cast spells by [10 / 3 * Pts]%.
        /// </summary>
        public int MentalAgility { get; set; }
        /// <summary>
        /// When you cast Smite, Holy Fire or Mind Flay you gain Evangelism. Stacks up to 0 times. Lasts for 20 sec.
        /// 
        /// Evangelism (Smite, Holy Fire)
        /// Increases the damage done by your Smite, Holy Fire, and Penance spells by [2 * Pts]% and reduces the mana cost of those spells by [3 * Pts]%.
        /// 
        /// Dark Evangelism (Mind Flay)
        /// Increases the damage done by your Periodic Shadow spells by [1 * Pts]%.
        /// </summary>
        public int Evangelism { get; set; }
        /// <summary>
        /// Archangel - Instant
        /// Consumes your Evangelism effects, causing an effect depending on what type of Evangelism effect is consumed:
        /// 
        /// Archangel (Evangelism)
        /// Instantly restores 1% of your total mana and increases your healing done by 3% for each stack. Lasts for 18 sec. 30 sec cooldown.
        /// 
        /// Dark Archangel (Dark Evangelism)
        /// Instantly restores 5% of your total mana and increases the damage done by your Mind Flay, Mind Spike, Mind Blast and Shadow Word: Death by 4% for each stack. Lasts for 18 sec. 90 sec cooldown.
        /// </summary>
        //public int Archangel { get; set; }
        /// <summary>
        /// Spell damage taken is reduced by 6% while within Inner [2 * Pts] and the movement speed bonus of your Inner Will is increased by 6%.
        /// </summary>
        public int InnerSanctum { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Power Word: Shield ability by [1 * Pts] sec.
        /// </summary>
        public int SoulWarding { get; set; }
        /// <summary>
        /// Increases the critical effect chance of your Flash Heal, Greater Heal, Heal and Penance (Heal) spells by [5 * Pts]% on targets afflicted by the Weakened Soul effect, or blessed with your Grace effect.
        /// </summary>
        public int RenewedHope { get; set; }
        /// <summary>
        /// Power Infusion - 40 yd range - 16% of base mana
        /// 2 min cooldown - Instant
        /// Infuses the target with power, increasing spell casting speed by 20% and reducing the mana cost of all spells by 20%.  Lasts 15 sec.
        /// </summary>
        //public int PowerInfusion { get; set; }
        /// <summary>
        /// When you deal damage with Smite or Holy Fire, you instantly heal a nearby low health friendly target within 15 yards from the enemy target equal to [50 * Pts]% of the damage dealt.
        /// 
        /// If the Priest is healed through Atonement, the effect is reduced in half.
        /// </summary>
        public int Atonement { get; set; }
        /// <summary>
        /// Inner Focus - 45 sec cooldown - Instant
        /// Reduces the mana cost of your next Flash Heal, Binding Heal, Greater Heal or Prayer of Healing by 100% and increases its critical effect chance by 25%.
        /// </summary>
        public int InnerFocus { get; set; }
        /// <summary>
        /// When your Power Word: Shield is completely absorbed or dispelled you are instantly energized with [7 / 3 * Pts]% of your total mana. This effect can only occur once every 12 sec.
        /// </summary>
        public int Rapture { get; set; }
        /// <summary>
        /// Grants [7 * Pts]% spell haste for your next spell after casting Power Word: Shield. Lasts for 6 sec.
        /// </summary>
        public int BorrowedTime { get; set; }
        /// <summary>
        /// Causes [45 / 2 * Pts]% of the damage you absorb with Power Word: Shield to reflect back at the attacker.  This damage causes no threat.
        /// </summary>
        public int ReflectiveShield { get; set; }
        /// <summary>
        /// When you heal a target with your Heal, Greater Heal or Flash Heal spell, the duration of the Weakened Soul debuff on the target is reduced by [2 * Pts] sec.
        /// 
        /// In addition, when you cast Inner Focus you become immune to Silence, Interrupt and Dispel effects for [5 / 2 * Pts] sec.
        /// </summary>
        public int StrengthOfSoul { get; set; }
        /// <summary>
        /// Critical heals and all heals from Prayer of Healing create a protective shield on the target, absorbing [10 * Pts]% of the amount healed. Lasts 15 sec.
        /// </summary>
        public int DivineAegis { get; set; }
        /// <summary>
        /// Pain Suppression - 40 yd range - 8% of base mana
        /// 3 min cooldown - Instant
        /// Instantly reduces a friendly target\'s threat by 5%, and reduces all damage they take by 40% for 8 sec.
        /// </summary>
        public int PainSuppression { get; set; }
        /// <summary>
        /// You have a [50 * Pts]% chance when you heal with Greater Heal to reduce the cooldown of your Inner Focus by 5 sec.
        /// 
        /// You have a [50 * Pts]% chance when you Smite to reduce the cooldown of your Penance by 0.5 sec.
        /// </summary>
        public int TrainOfThought { get; set; }
        /// <summary>
        /// Whenever you are victim of any damage greater than [5 * Pts]% of your total health or critically hit by any non-periodic attack, you gain Focused Will reducing all damage taken by 10% lasting for 8 sec. Stacks up to 0 times.
        /// </summary>
        public int FocusedWill { get; set; }
        /// <summary>
        /// Your Flash Heal, Greater Heal, Heal and Penance spells bless the target with Grace, increasing all healing received from the Priest by [4 * Pts]%. This effect will stack up to 0 times. Effect lasts 15 sec.
        /// </summary>
        public int Grace { get; set; }
        /// <summary>
        /// Power Word: Barrier - 40 yd range - 1 Unholy
        /// 3 min cooldown - Instant
        /// Summons a holy barrier on the target location that reduces all damage done to friendly targets by 25%. While within the barrier, spellcasting will not be interrupted by damage. The barrier lasts for 10 sec.
        /// </summary>
        public int PowerWordBarrier { get; set; }
        #endregion
        #region Holy
        /// <summary>
        /// Increases the amount healed by your Renew spell by [5 * Pts]%.
        /// </summary>
        public int ImprovedRenew { get; set; }
        /// <summary>
        /// Increases the healing done by your Flash Heal, Heal, Binding Heal and Greater Heal by [5 * Pts]%.
        /// </summary>
        public int EmpoweredHealing { get; set; }
        /// <summary>
        /// Reduces the casting time of your Smite, Holy Fire, Heal and Greater Heal spells by [0.15 * Pts] sec.
        /// </summary>
        public int DivineFury { get; set; }
        /// <summary>
        /// Desperate Prayer - 2 min cooldown - Instant cast
        /// Instantly heals the caster for 30% of their total health.
        /// </summary>
        //public int DesperatePrayer { get; set; }
        /// <summary>
        /// You have a [3 * Pts]% chance when you Smite, Heal, Flash Heal, Binding Heal or Greater Heal to cause your next Flash Heal to be instant cast and cost no mana.
        /// </summary>
        public int SurgeOfLight { get; set; }
        /// <summary>
        /// Reduces your target's physical damage taken by [5 * Pts]% for 15 sec after getting a critical effect from your Flash Heal, Heal, Greater Heal, Binding Heal, Penance, Prayer of Mending, Prayer of Healing, or Circle of Healing spell.
        /// </summary>
        public int Inspiration { get; set; }
        /// <summary>
        /// Your Renew will instantly heal the target for [5 * Pts]% of the total periodic effect.
        /// </summary>
        public int DivineTouch { get; set; }
        /// <summary>
        /// Increases the amount of mana regeneration from Spirit while in combat by an additional [15 * Pts]%.
        /// </summary>
        public int HolyConcentration { get; set; }
        /// <summary>
        /// Lightwell - 40 yd range - 30% of base mana
        /// 3 min cooldown - 0.5 sec cast
        /// Creates a Holy Lightwell.  Friendly players can click the Lightwell to restore [9929 + 1.06 * SP] health over 6 sec.  Attacks done to you equal to 30% of your total health will cancel the effect. Lightwell lasts for 3 min or 10 charges.
        /// </summary>
        public int Lightwell { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Holy Word spells by [15 * Pts]%.
        /// </summary>
        public int TomeOfLight { get; set; }
        /// <summary>
        /// Reduces the global cooldown of your Renew by 0.5 sec.
        /// </summary>
        public int RapidRenewal { get; set; }
        /// <summary>
        /// Upon death, the priest becomes the Spirit of Redemption for 15 sec.  The Spirit of Redemption cannot move, attack, be attacked or targeted by any spells or effects.  While in this form the priest can cast any healing spell free of cost.  When the effect ends, the priest dies.
        /// </summary>
        public int SpiritOfRedemption { get; set; }
        /// <summary>
        /// When you heal with Binding Heal or Flash Heal, the cast time of your next Greater Heal or Prayer of Healing spell is reduced by [10 * Pts]% and mana cost reduced by [5 * Pts]%. Stacks up to 0 times. Lasts 20 sec.
        /// </summary>
        public int Serendipity { get; set; }
        /// <summary>
        /// When you cast Power Word: Shield or Leap of Faith, you increase the target's movement speed by [30 * Pts]% for 4 sec, and you have a [50 * Pts]% chance when you cast Cure Disease on yourself to also cleanse 1 poison effect in addition to diseases.
        /// </summary>
        //public int BodyAndSoul { get; set; }
        /// <summary>
        /// Chakra - 30 sec cooldown - Instant
        /// When activated, your next Heal, Flash Heal, Greater Heal, Binding Heal, Prayer of Healing, Prayer of Mending, Mind Spike or Smite will put you into a Chakra state.
        /// 
        /// Serenity (Heal, Flash Heal, Greater Heal, Binding Heal)
        /// Increases the critical effect chance of your direct healing spells by 10%, and causes your direct heals to refresh the duration of your Renew on the target.
        /// 
        /// Sanctuary (Prayer of Healing, Prayer of Mending)
        /// Increases the healing done by your area of effect spells and Renew by 15% and reduces the cooldown of your Circle of Healing by -2 sec.
        /// 
        /// Chastise (Smite, Mind Spike)
        /// Increases your total damage done by Shadow and Holy spells by 15%.
        /// </summary>
        public int Chakra { get; set; }
        /// <summary>
        /// While within Chakra: Serenity or Chakra: Sanctuary, your Holy Word: Chastise ability will transform into a different ability depending on which state you are in.
        /// 
        /// Holy Word: Serenity
        /// Instantly heals the target for 5197 to 6101, and increases the critical effect chance of your healing spells on the target by 25% for 6 sec. 15 sec cooldown.
        /// 
        /// Holy Word: Sanctuary
        /// Blesses the ground with Divine light, healing all within it for 298 to 355 every 2 sec for 18 sec. Only one Sanctuary can be active at any one time. 40 sec cooldown.
        /// </summary>
        public int Revelations { get; set; }
        /// <summary>
        /// Whenever you are victim of an attack equal to damage greater than 10% of your total health or critically hit by any non-periodic attack, you gain Blessed Resilience increasing all healing received by [15 * Pts]% lasting for 10 sec.
        /// </summary>
        public int BlessedResilience { get; set; }
        /// <summary>
        /// Increases healing by [4 * Pts]% on friendly targets at or below 50% health.
        /// </summary>
        public int TestOfFaith { get; set; }
        /// <summary>
        /// Increases the healing done by your Divine Hymn spell by [50 * Pts]%, and reduces the cooldown by [5 / 2 * Pts] minutes.
        /// </summary>
        public int HeavenlyVoice { get; set; }
        /// <summary>
        /// Circle of Healing - 40 yd range - 21% of base mana
        /// 10 sec cooldown - Instant cast
        /// Heals up to 5 friendly party or raid members within 0 yards of the target for 2308 to 2551.  Prioritizes healing the most injured party members.
        /// </summary>
        public int CircleOfHealing { get; set; }
        /// <summary>
        /// Guardian Spirit - 40 yd range - 6% of base mana
        /// 3 min cooldown - Instant
        /// Calls upon a guardian spirit to watch over the friendly target. The spirit increases the healing received by the target by 60%, and also prevents the target from dying by sacrificing itself.  This sacrifice terminates the effect but heals the target of 50% of their maximum health. Lasts 10 sec.
        /// </summary>
        public int GuardianSpirit { get; set; }
        #endregion
        #region Shadow
        /// <summary>
        /// Spell haste increased by [1 * Pts]%.
        /// </summary>
        public int Darkness { get; set; }
        /// <summary>
        /// Increases the damage of your Shadow Word: Pain spell by [3 * Pts]%.
        /// </summary>
        public int ImprovedShadowWordPain { get; set; }
        /// <summary>
        /// Decreases the cooldown of your Fade ability by [3 * Pts] sec, and reduces the cooldown of your Shadowfiend ability by [30 * Pts] sec.
        /// </summary>
        public int VeiledShadows { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Psychic Scream spell by [2 * Pts] sec.
        /// </summary>
        public int ImprovedPsychicScream { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Mind Blast spell by [0.5 * Pts] sec., and while in Shadowform your Mind Blast also has a [100 / 3 * Pts]% chance to reduce all healing done to the target by 10% for 10 sec.
        /// </summary>
        public int ImprovedMindBlast { get; set; }
        /// <summary>
        /// Your Devouring Plague instantly deals damage equal to [15 * Pts]% of its total periodic effect.
        /// </summary>
        public int ImprovedDevouringPlague { get; set; }
        /// <summary>
        /// Increases your Shadow spell damage by [1 * Pts]%, and grants you spell hit rating equal to [50 * Pts]% of any Spirit gained from items or effects.
        /// </summary>
        public int TwistedFaith { get; set; }
        /// <summary>
        /// Shadowform - 13% of base mana
        /// 1.5 sec cooldown - Instant cast
        /// Assume a Shadowform, increasing your Shadow damage by 15%, reducing all damage done to you by 15%, and increasing all party and raid members spell haste by 5%. However, you may not cast Holy spells while in this form.
        /// </summary>
        public int Shadowform { get; set; }
        /// <summary>
        /// Your Fade ability now has a [50 * Pts]% chance to remove all movement impairing effects.
        /// </summary>
        //public int Phantasm { get; set; }
        /// <summary>
        /// Increases the chance for you to gain a Shadow Orb when dealing damage with your Mind Flay and Shadow Word: Pain by [4 * Pts]%, and you have a [50 * Pts]% chance to gain a Shadow Orb when critically hit by any attack.
        /// </summary>
        public int HarnessedShadows { get; set; }
        /// <summary>
        /// Silence - 30 yd range - 225 Mana
        /// 45 sec cooldown - Instant
        /// Silences the target, preventing them from casting spells for 5 sec.  Non-player victim spellcasting is also interrupted for 3 sec.
        /// </summary>
        public int Silence { get; set; }
        /// <summary>
        /// Vampiric Embrace - Instant cast
        /// Fills you with the embrace of Shadow energy, causing you to be healed for 6% and other party members to be healed for 3% of any single-target Shadow spell damage you deal.
        /// </summary>
        public int VampiricEmbrace { get; set; }
        /// <summary>
        /// When you take a damaging attack equal to or greater than [5 * Pts]% of your total health or damage yourself with your Shadow Word: Death, you instantly gain 10% of your total mana.
        /// </summary>
        public int Masochism { get; set; }
        /// <summary>
        /// Increases the damage done with your Shadow Word: Death by [15 * Pts]% on targets at or below 25% health, and when you deal damage with Mind Spike, the cast time of your next Mind Blast is reduced by [25 * Pts]% lasting 6 sec. Mind Melt can stack up to 2 times.
        /// </summary>
        public int MindMelt { get; set; }
        /// <summary>
        /// Your Mind Flay has a [30 * Pts]% chance to refresh the duration of your Shadow Word: Pain on the target, and reduces the damage you take from your own Shadow Word: Death by [20 * Pts]%.
        /// </summary>
        public int PainAndSuffering { get; set; }
        /// <summary>
        /// Vampiric Touch - 40 yd range - 16% of base mana
        /// 1.5 sec cast
        /// Causes 475 Shadow damage over 15 sec, and when you deal damage with Mind Blast to an affected target you cause up to 10 party or raid members to gain 1% of their maximum mana per 10 sec.
        /// </summary>
        public int VampiricTouch { get; set; }
        /// <summary>
        /// When you critically hit with your Mind Blast, you cause the target to be unable to move for [2 * Pts] sec.
        /// </summary>
        public int Paralysis { get; set; }
        /// <summary>
        /// Psychic Horror - 30 yd range - 16% of base mana
        /// 2 min cooldown - Instant cast
        /// You terrify the target, causing them to tremble in horror for 3 sec and drop their main hand and ranged weapons for 10 sec.
        /// </summary>
        public int PsychicHorror { get; set; }
        /// <summary>
        /// When your Vampiric Touch is dispelled, the dispeller and all nearby enemy targets within 29 yards have a [50 * Pts]% chance to be instantly feared in horror for 3 sec.
        /// 
        /// When your Mind Flay critically hits, the cooldown of your Shadowfiend is reduced by [5 * Pts] sec.
        /// </summary>
        public int SinAndPunishment { get; set; }
        /// <summary>
        /// When you deal periodic damage with your Shadow Word: Pain, you have a [4 * Pts]% chance to summon a shadow version of yourself which will slowly move towards a target which is afflicted by your Shadow Word: Pain. Once reaching the target, it will instantly deal 485 shadow damage.
        ///     [0 * Pts] moving, the chance to summon the shadowy apparation is increased to 60%[20 * Pts] [0 * Pts] can have up to 4 [4 * Pts] Apparitions active at a time.
        /// </summary>
        public int ShadowyApparition { get; set; }
        /// <summary>
        /// Dispersion - 2 min cooldown - Instant cast
        /// You disperse into pure Shadow energy, reducing all damage taken by 90%.  You are unable to attack or cast spells, but you regenerate 6% mana every 1 sec for 6 sec. 
        /// 
        /// Dispersion can be cast while stunned, feared or silenced. Clears all snare and movement impairing effects when cast, and makes you immune to them while dispersed.
        /// </summary>
        public int Dispersion { get; set; }
        #endregion
    }

#if false // Remove the old DK Talents.
    public partial class DeathKnightTalents : TalentsBase
    {
        #region Blood
        /// <summary>
        /// Whenever you kill an enemy that grants experience or honor, you generate up to [10 * Pts] Runic Power.  In addition, you generate [1 * Pts] Runic Power per 5 sec while in combat.
        /// </summary>
        public int Butchery { get; set; }
        /// <summary>
        /// You take [2 * Pts]% less damage from all sources.
        /// </summary>
        public int BladeBarrier { get; set; }
        /// <summary>
        /// Increases your attack power by [2 * Pts] for every 180 armor value you have.
        /// </summary>
        public int BladedArmor { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Blood Tap ability by [15 * Pts] sec.
        /// </summary>
        public int ImprovedBloodTap { get; set; }
        /// <summary>
        /// You have a 15% chance after dodging, parrying or taking  direct damage to gain the Scent of Blood effect, causing your next 3 melee hits to [10 * Pts] 10 Runic Power.
        /// </summary>
        public int ScentOfBlood { get; set; }
        /// <summary>
        /// Causes your Blood Plague to afflict enemies with Scarlet Fever, reducing their physical damage dealt by [5 * Pts]% for 21 sec.
        /// </summary>
        public int ScarletFever { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Strangulate ability by [30 * Pts] sec.
        /// </summary>
        public int HandOfDoom { get; set; }
        /// <summary>
        /// Your autoattacks have a [10 * Pts]% chance to cause a Blood-Caked Strike, which hits for 25% weapon damage plus 12.5% for each of your diseases on the target.
        /// </summary>
        public int BloodCakedBlade { get; set; }
        /// <summary>
        /// Bone Shield - 1 Unholy
        /// 1 min cooldown - Instant cast
        /// Surrounds you with a barrier of whirling bones.  The shield begins with 6 charges, and each damaging attack consumes a charge.  While at least 1 charge remains, you take 20% less damage from all sources and deal 2% more damage with all attacks, spells and abilities.  Lasts 5 min.
        /// </summary>
        public int BoneShield { get; set; }
        /// <summary>
        /// Increases your armor value from items by [10 / 3 * Pts]%.
        /// </summary>
        public int Toughness { get; set; }
        /// <summary>
        /// Increases melee attack power by [10 * Pts]% and ranged attack power by [5 * Pts]% of party and raid members within 1[1 * Pts] yards.  Also increases your total Strength by 2%.
        /// </summary>
        public int AbominationsMight { get; set; }
        /// <summary>
        /// Your Icebound Fortitude reduces damage taken by an additional [15 * Pts]%, and costs [50 * Pts] Runic Power to activate.
        /// </summary>
        public int SanguineFortitude { get; set; }
        /// <summary>
        /// Your melee attacks have a [5 * Pts]% chance to spawn a Bloodworm.  The Bloodworm attacks your enemies, gorging itself with blood until it bursts to heal nearby allies.  Lasts up to 20 sec.
        /// </summary>
        public int BloodParasite { get; set; }
        /// <summary>
        /// Increases your rune regeneration by [10 * Pts]% and reduces the chance that you will be critically hit by melee attacks while in Blood Presence by [3 * Pts]%.  In addition, while in Frost Presence or Unholy Presence, you retain [2 * Pts]% damage reduction from Blood Presence.
        /// </summary>
        public int ImprovedBloodPresence { get; set; }
        /// <summary>
        /// When a damaging attack brings you below 30% of your maximum health, the cooldown on your Rune Tap ability is refreshed and your next Rune Tap has no cost, and all damage taken is reduced by [25 / 3 * Pts]% for 8 sec.  This effect cannot occur more than once every 45 seconds.
        /// </summary>
        public int WillOfTheNecropolis { get; set; }
        /// <summary>
        /// Rune Tap - 1 Blood
        /// 30 sec cooldown - Instant
        /// Converts 1 Blood Rune into 10% of your maximum health.
        /// </summary>
        public int RuneTap { get; set; }
        /// <summary>
        /// Vampiric Blood - 1 min cooldown - Instant
        /// Temporarily grants the Death Knight 15% of maximum health and increases the amount of health received from healing spells and effects by 25% for 10 sec.  After the effect expires, the health is lost.
        /// </summary>
        //public int VampiricBlood { get; set; }
        /// <summary>
        /// Increases the damage done by your Death Strike by [40 * Pts]%, its critical strike chance by [10 * Pts]%, and its amount healed by [15 * Pts]%.
        /// </summary>
        public int ImprovedDeathStrike { get; set; }
        /// <summary>
        /// Increases the damage dealt by your Blood Boil by [20 * Pts]%, and when you land a melee attack on a target that is infected with your Blood Plague, there is a [5 * Pts]% chance that your next Blood Boil will consume no runes.
        /// </summary>
        public int CrimsonScourge { get; set; }
        /// <summary>
        /// Dancing Rune Weapon - 30 yd range - 60 Runic Power
        /// 1 min cooldown - Instant cast
        /// Requires Melee Weapon - Summons a second rune weapon that fights on its own for 12 sec, mirroring the Death Knight\'s attacks.  The rune weapon also assists in defense of its master, granting an additional 20% parry chance while active.
        /// </summary>
        public int DancingRuneWeapon { get; set; }
        #endregion
        #region Frost
        /// <summary>
        /// Increases your maximum Runic Power by [10 * Pts]
        /// </summary>
        public int RunicPowerMastery { get; set; }
        /// <summary>
        /// Increases the range of your Icy Touch, Chains of Ice and Howling Blast by [5 * Pts] yards.
        /// </summary>
        public int IcyReach { get; set; }
        /// <summary>
        /// Increases your chance to hit with one-handed melee weapons by [1 * Pts]% and increases the damage done by your off-hand weapon by [25 / 3 * Pts]%.
        /// </summary>
        public int NervesOfColdSteel { get; set; }
        /// <summary>
        /// Increases the damage dealt by your Obliterate ability by [15 * Pts]%.
        /// </summary>
        public int Annihilation { get; set; }
        /// <summary>
        /// Lichborne - 2 min cooldown - Instant
        /// Draw upon unholy energy to become undead for 10 sec.  While undead, you are immune to Charm, Fear and Sleep effects.
        /// </summary>
        //public int Lichborne { get; set; }
        /// <summary>
        /// You become as hard to stop as death itself.  The duration of movement-slowing effects used against you is reduced by [15 * Pts]%, and your mounted speed is increased by [10 * Pts]%.  This does not stack with other movement speed increasing effects.
        /// </summary>
        public int OnAPaleHorse { get; set; }
        /// <summary>
        /// Your Mind Freeze no longer costs Runic Power.
        /// </summary>
        public int EndlessWinter { get; set; }
        /// <summary>
        /// Your Icy Touch, Howling Blast, Obliterate and Frost Strike do an additional [6 * Pts]% damage when striking targets with less than 35% health.
        /// </summary>
        public int MercilessCombat { get; set; }
        /// <summary>
        /// Your Chains of Ice, Howling Blast, Icy Touch and Obliterate generate [5 * Pts] additional Runic Power.
        /// </summary>
        public int ChillOfTheGrave { get; set; }
        /// <summary>
        /// Your autoattacks have a chance to grant a 100% critical strike bonus to your next Obliterate or Frost Strike.  Effect occurs more often than Killing Machine (Rank 2).
        /// </summary>
        public int KillingMachine { get; set; }
        /// <summary>
        /// Your Obliterate has a [15 * Pts]% chance to cause your next Howling Blast or Icy Touch to consume no runes.
        /// </summary>
        public int Rime { get; set; }
        /// <summary>
        /// Pillar of Frost - 1 Frost
        /// 1 min cooldown - Instant
        /// Calls upon the power of Frost to increase the Death Knight\'s Strength by 20%.  Icy crystals hang heavy upon the Death Knight\'s body, providing immunity against external movement such as knockbacks.  Lasts 20 sec.
        /// </summary>
        public int PillarOfFrost { get; set; }
        /// <summary>
        /// Increases the melee and ranged attack speed of all party and raid members within 12 yards by 10%, and your own attack speed by an additional 5%.
        /// </summary>
        public int ImprovedIcyTalons { get; set; }
        /// <summary>
        /// Your Strength is increased by [2 * Pts]% and your Frost Fever chills the bones of its victims, increasing their physical damage taken by [2 * Pts]%.
        /// </summary>
        public int BrittleBones { get; set; }
        /// <summary>
        /// Victims of your Frost Fever disease are Chilled, reducing movement speed by [25 * Pts]% for 10 sec, and your Chains of Ice immobilizes targets for [1 * Pts] sec.
        /// </summary>
        //public int Chilblains { get; set; }
        /// <summary>
        /// Hungering Cold - 40 Runic Power
        /// 1 min cooldown - 1.5 sec cast
        /// Purges the earth around the Death Knight of all heat.  Enemies within 10 yards are afflicted with Frost Fever and trapped in ice, preventing them from performing any action for 10 sec.  Enemies are considered Frozen, but any damage other than diseases will break the ice.
        /// </summary>
        public int HungeringCold { get; set; }
        /// <summary>
        /// Increases your bonus damage while in Frost Presence by an additional [5 / 2 * Pts]%.  In addition, while in Blood Presence or Unholy Presence, you retain [2 * Pts]% increased Runic Power generation from Frost Presence.
        /// </summary>
        public int ImprovedFrostPresence { get; set; }
        /// <summary>
        /// When dual-wielding, your Death Strikes, Obliterates, Plague Strikes, Rune Strikes, Blood Strikes and Frost Strikes have a [100 / 3 * Pts]% chance to also deal damage with your off-hand weapon.
        /// </summary>
        public int ThreatOfThassarian { get; set; }
        /// <summary>
        /// When wielding a two-handed weapon, your melee attacks deal an additional [10 / 3 * Pts]% damage and your autoattacks have a [15 * Pts]% chance to generate 10 Runic Power.
        /// </summary>
        public int MightOfTheFrozenWastes { get; set; }
        /// <summary>
        /// Howling Blast - 20 yd range - 1 Frost
        /// Instant cast
        /// Blast the target with a frigid wind, dealing [1322 + 44% of AP] Frost damage to that foe, and [661 + 22% of AP] Frost damage to all other enemies within 10 yards.
        /// </summary>
        public int HowlingBlast { get; set; }
        #endregion
        #region Unholy
        /// <summary>
        /// Reduces the cooldown of your Death Grip ability by [5 * Pts] sec, and gives you a [50 * Pts]% chance to refresh its cooldown when dealing a killing blow to a target that grants experience or honor.
        /// </summary>
        public int UnholyCommand { get; set; }
        /// <summary>
        /// Increases the damage done by your diseases by [10 * Pts]%.
        /// </summary>
        public int Virulence { get; set; }
        /// <summary>
        /// Increases the duration of Blood Plague and Frost Fever by [4 * Pts] sec.
        /// </summary>
        public int Epidemic { get; set; }
        /// <summary>
        /// Your Plague, Scourge, and Necrotic Strikes defile the ground within 37 yards of your target. Enemies in the area are slowed by [25 * Pts]% while standing on the unholy ground. Does not trigger against targets that are immune to movement-slowing effects. Lasts 20 sec.
        /// </summary>
        public int Desecration { get; set; }
        /// <summary>
        /// When your diseases are dispelled by an enemy, you have a [50 * Pts]% chance to activate a Frost Rune if Frost Fever was removed, or an Unholy Rune if Blood Plague was removed.
        /// </summary>
        public int ResilientInfection { get; set; }
        /// <summary>
        /// Increases the damage and healing of Death Coil by [5 * Pts]% and Death and Decay by [10 * Pts]%.
        /// </summary>
        public int Morbidity { get; set; }
        /// <summary>
        /// Reduces the cost of your Death Coil by [3 * Pts] and causes your Runic Empowerment ability to no longer refresh a depleted rune, but instead to increase your rune regeneration rate by [50 * Pts]% for 3 sec.
        /// </summary>
        //public int RunicCorruption { get; set; }
        /// <summary>
        /// Unholy Frenzy - 30 yd range
        /// 3 min cooldown - Instant
        /// Incites a friendly party or raid member into a killing frenzy for 30 sec.  The target is Enraged, increasing their melee and ranged haste by 20%, but causes them to lose health equal to 2% of their maximum health every 3 sec.
        /// </summary>
        public int UnholyFrenzy { get; set; }
        /// <summary>
        /// Increases the damage of your diseases spread via Pestilence by [50 * Pts]%.
        /// </summary>
        public int Contagion { get; set; }
        /// <summary>
        /// Grants your successful Death Coils a [100 / 3 * Pts]% chance to empower your active Ghoul, increasing its damage dealt by 6% for 30 sec.  Stacks up to 5 times.
        /// </summary>
        public int ShadowInfusion { get; set; }
        /// <summary>
        /// While your Unholy Runes are both depleted, movement-impairing effects may not reduce you below [75 / 2 * Pts]% of normal movement speed.
        /// </summary>
        //public int DeathsAdvance { get; set; }
        /// <summary>
        /// Increases the spell damage absorption of your Anti-Magic Shell by an additional [25 / 3 * Pts]%, and increases the Runic Power generated when damage is absorbed by Anti-Magic Shell.
        /// </summary>
        public int MagicSuppression { get; set; }
        /// <summary>
        /// Increases the damage of your Plague Strike, Scourge Strike, and Festering Strike abilities by [15 * Pts]%.
        /// </summary>
        public int RageOfRivendare { get; set; }
        /// <summary>
        /// Causes the victims of your Death Coil to be surrounded by a vile swarm of unholy insects, taking 10% of the damage done by the Death Coil over 10 sec, and preventing any diseases on the victim from being dispelled.
        /// </summary>
        //public int UnholyBlight { get; set; }
        /// <summary>
        /// Anti-Magic Zone - 1 Unholy
        /// 2 min cooldown - Instant
        /// Places a large, stationary Anti-Magic Zone that reduces spell damage done to party or raid members inside it by 75%.  The Anti-Magic Zone lasts for 10 sec or until it absorbs [10000 + 2 * AP] spell damage.
        /// </summary>
        //public int AntiMagicZone { get; set; }
        /// <summary>
        /// Grants you an additional [5 / 2 * Pts]% haste while in Unholy Presence.  In addition, while in Blood Presence or Frost Presence, you retain [15 / 2 * Pts]% increased movement speed from Unholy Presence.
        /// </summary>
        public int ImprovedUnholyPresence { get; set; }
        /// <summary>
        /// Dark Transformation - 100 yd range - 1 Unholy
        /// Instant cast
        /// Consume 5 charges of Shadow Infusion on your Ghoul to transform it into a powerful undead monstrosity for 30 sec.  The Ghoul\'s abilities are empowered and take on new functions while the transformation is active.
        /// </summary>
        public int DarkTransformation { get; set; }
        /// <summary>
        /// Your Plague Strike, Icy Touch, Chains of Ice, and Outbreak abilities also infect their target with Ebon Plague, which increases damage taken from your diseases by [15 * Pts]% and all magic damage taken by an additional 8%.
        /// </summary>
        public int EbonPlaguebringer { get; set; }
        /// <summary>
        /// Your main-hand autoattacks have a chance (higher than rank 2) to make your next Death Coil cost no Runic Power.
        /// </summary>
        public int SuddenDoom { get; set; }
        /// <summary>
        /// Summon Gargoyle - 30 yd range - 1 Frost 1 Unholy
        /// 3 min cooldown - Instant cast
        /// A Gargoyle flies into the area and bombards the target with Nature damage modified by the Death Knight\'s attack power.  Persists for 30 sec.
        /// </summary>
        public int SummonGargoyle { get; set; }
        #endregion
    }
#endif

    public partial class ShamanTalents : TalentsBase
    {
        #region Elemental
        /// <summary>
        /// Increases your critical strike chance with all spells and attacks by [1 * Pts]%.
        /// </summary>
        public int Acuity { get; set; }
        /// <summary>
        /// Reduces the mana cost of your damaging offensive spells by [5 * Pts]%.
        /// </summary>
        public int Convection { get; set; }
        /// <summary>
        /// Increases the damage done by your Lightning Bolt, Chain Lightning, Thunderstorm, Lava Burst and Shock spells by [2 * Pts]%.
        /// </summary>
        public int Concussion { get; set; }
        /// <summary>
        /// Increases the damage done by your Fire Totems and Fire Nova by [10 * Pts]%, and damage done by your Lava Burst spell by [5 * Pts]%.
        /// </summary>
        public int CallOfFlame { get; set; }
        /// <summary>
        /// Reduces magical damage taken by [4 * Pts]%.
        /// </summary>
        public int ElementalWarding { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Shock spells by [0.5 * Pts] sec and Wind Shear by [5 * Pts] sec.
        /// </summary>
        public int Reverberation { get; set; }
        /// <summary>
        /// Increases your Fire, Frost and Nature damage by [1 * Pts]% and grants you spell hit rating equal to [100 / 3 * Pts]% of any Spirit gained from items or effects.
        /// </summary>
        public int ElementalPrecision { get; set; }
        /// <summary>
        /// When you deal damage with Lightning Bolt or Chain Lightning while your Lightning Shield ability is active, you have a [30 * Pts]% chance to recover 2% of your mana and to generate an additional Lightning Shield charge, up to a maximum of 9 charges.
        /// </summary>
        public int RollingThunder { get; set; }
        /// <summary>
        /// After landing a non-periodic critical strike with a Fire, Frost, or Nature damage spell, you enter a Clearcasting state.  The Clearcasting state reduces the mana cost of your next 2 damage or healing spells by 40%.
        /// </summary>
        public int ElementalFocus { get; set; }
        /// <summary>
        /// Increases the range of your Lightning Bolt, Chain Lightning, Fire Nova, and Lava Burst spells by [5 * Pts] yards, and increases the range of your Shock spells and Searing Totem by [15 / 2 * Pts] yards.
        /// </summary>
        public int ElementalReach { get; set; }
        /// <summary>
        /// While Clearcasting from Elemental Focus is active, you deal [5 * Pts]% more spell damage. In addition, party and raid members within 12 yards receive a [5 / 2 * Pts]% bonus to their critical strike chance.
        /// </summary>
        public int ElementalOath { get; set; }
        /// <summary>
        /// Increases the critical strike damage bonus of your Lava Burst spell by an additional [8 * Pts]% and the periodic damage of your Flame Shock by [20 * Pts]%.  In addition, when your Flame Shock is dispelled you gain [30 * Pts]% spell haste for 6 sec.
        /// </summary>
        public int LavaFlows { get; set; }
        /// <summary>
        /// When you have more than 3 Lightning Shield charges active, your Earth Shock spell will consume any surplus charges, instantly dealing their total damage to the enemy target.
        /// </summary>
        public int Fulmination { get; set; }
        /// <summary>
        /// Elemental Mastery - 3 min cooldown - Instant
        /// When activated, your next Lightning Bolt, Chain Lightning or Lava Burst spell becomes an instant cast spell. In addition, your Fire, Frost, and Nature damage is increased by 15% and you gain 20% spell haste for 15 sec.
        /// </summary>
        //public int ElementalMastery { get; set; }
        /// <summary>
        /// Grants your Earthbind Totem a [50 * Pts]% chance to root nearby targets for 5 sec when cast.
        /// </summary>
        public int EarthsGrasp { get; set; }
        /// <summary>
        /// Causes your Fire totems to increase the spell power of party and raid members within 12 yards by 10%.
        /// </summary>
        public int TotemicWrath { get; set; }
        /// <summary>
        /// Your Lightning Bolt and Chain Lightning spells reduce the remaining cooldown on your Elemental Mastery talent by [1 * Pts] sec.
        /// </summary>
        public int Feedback { get; set; }
        /// <summary>
        /// Gives your Flame Shock periodic damage ticks a [10 * Pts]% chance to reset the cooldown of your Lava Burst spell.
        /// </summary>
        public int LavaSurge { get; set; }
        /// <summary>
        /// Earthquake - 35 yd range - 60% of base mana
        /// 10 sec cooldown - 2.5 sec cast
        /// You cause the earth at the target location to tremble and break, dealing [325 + 11% of SPN] Physical damage every 1 sec to enemies in an 8 yard radius, with a 10% chance of knocking down affected targets. Lasts 10 sec.
        /// </summary>
        public int Earthquake { get; set; }
        #endregion
        #region Enhancement
        /// <summary>
        /// Increases the passive bonuses granted by your Flametongue Weapon and Earthliving Weapon abilities by [20 * Pts]%, the damage of your extra attacks from Windfury Weapon by [20 * Pts]%, and the effectiveness of the ongoing benefits of your Unleash Elements ability by [25 * Pts]%.
        /// </summary>
        public int ElementalWeapons { get; set; }
        /// <summary>
        /// Increases the damage dealt by your Primal Strike and Stormstrike abilities by [15 * Pts]%.
        /// </summary>
        public int FocusedStrikes { get; set; }
        /// <summary>
        /// Increases the damage done by your Lightning Shield orbs by [5 * Pts]%, increases the amount of mana gained from your Water Shield orbs by [5 * Pts]%, and increases the amount of healing done by your Earth Shield orbs by [5 * Pts]%.
        /// </summary>
        public int ImprovedShields { get; set; }
        /// <summary>
        /// When you deal critical damage with a non-periodic spell, your chance to get a critical strike with melee attacks increases by [3 * Pts]% for 10 sec.
        /// </summary>
        public int ElementalDevastation { get; set; }
        /// <summary>
        /// Increases your attack speed by [10 * Pts]% for your next 3 swings after dealing a critical strike.
        /// </summary>
        public int Flurry { get; set; }
        /// <summary>
        /// Reduces the cast time of your Ghost Wolf spell by [1 * Pts] sec and increases movement speed by [15 / 2 * Pts]%.  This does not stack with other movement speed increasing effects.
        /// </summary>
        //public int AncestralSwiftness { get; set; }
        /// <summary>
        /// Increases the range of your totems' effects by [15 * Pts]%.
        /// </summary>
        public int TotemicReach { get; set; }
        /// <summary>
        /// Increases your Stamina by [10 / 3 * Pts]%, and reduces the duration of movement slowing effects on you by [10 * Pts]%.
        /// </summary>
        public int Toughness { get; set; }
        /// <summary>
        /// Stormstrike - Melee Range - 8% of base mana
        /// 8 sec cooldown - Instant cast
        /// Requires Melee Weapon - Instantly strike an enemy with both weapons, dealing 225% weapon damage and granting you an additional 25% chance to critically strike that enemy with your Lightning Bolt, Chain Lightning, Lightning Shield, and Earth Shock spells for 15 sec.
        /// </summary>
        public int Stormstrike { get; set; }
        /// <summary>
        /// When you use your Primal Strike, Stormstrike, or Lava Lash abilities while having Lightning Shield active, you have a [15 * Pts]% chance to deal damage equal to a Lightning Shield orb without consuming a charge.
        /// </summary>
        public int StaticShock { get; set; }
        /// <summary>
        /// Increases the damage done by your Lightning Bolt, Chain Lightning, Lava Lash, and Shock spells by [5 * Pts]% on targets afflicted by your Frostbrand Attack effect, and your Frost Shock has a [50 * Pts]% chance to root the target in ice for 5 sec. when used on targets at or further than 15 yards from you.
        /// </summary>
        //public int FrozenPower { get; set; }
        /// <summary>
        /// When you successfully interrupt an enemy spellcast with Wind Shear, you gain [if (PL<=70) then PL else if (PL<=80) then PL+(PL-70)*5 else PL+(PL-70)*5+(PL-80)*7] resistance to that spell's magical school for 10 sec.
        /// </summary>
        public int SeasonedWinds { get; set; }
        /// <summary>
        /// Causes the Searing Bolts from your Searing Totem to have a [100 / 3 * Pts]% chance to set their targets aflame, dealing damage equal to the Searing Bolt's impact damage over 15 sec. Stacks up to 5 times.
        /// </summary>
        public int SearingFlames { get; set; }
        /// <summary>
        /// Your Earthbind Totem's pulses have a [50 * Pts]% chance to also remove all snare effects from you and nearby friendly targets.
        /// </summary>
        public int EarthenPower { get; set; }
        /// <summary>
        /// Shamanistic Rage - 1 min cooldown - Instant cast
        /// Reduces all damage taken by 30% and causes your skills, totems, and offensive spells to consume no mana for 15 sec. This spell is usable while stunned.
        /// </summary>
        public int ShamanisticRage { get; set; }
        /// <summary>
        /// Increases your expertise by [4 * Pts] and increases all party and raid members' melee attack power by [10 * Pts]% and ranged attack power by [5 * Pts]% while within 12 yards of the Shaman.
        /// </summary>
        public int UnleashedRage { get; set; }
        /// <summary>
        /// When you deal damage with a melee weapon, you have a chance (higher than rank 2) to reduce the cast time and mana cost of your next Lightning Bolt, Chain Lightning, Hex, or [20 * Pts] [0 * Pts] spell by 20%[5 * Pts] Stacks up [30 * Pts] 5 times. Lasts 30 sec.
        /// </summary>
        public int MaelstromWeapon { get; set; }
        /// <summary>
        /// Increases the damage of your Lava Lash ability by [10 * Pts]% for each application of your Searing Flames on the target, consuming them in the process, and causes your Lava Lash to spread your Flame Shock from the target to up to four enemies within 12 yards.
        /// </summary>
        public int ImprovedLavaLash { get; set; }
        /// <summary>
        /// Feral Spirit - 30 yd range - 12% of base mana
        /// 2 min cooldown - Instant cast
        /// Summons two Spirit Wolves under the command of the Shaman, lasting 30 sec.
        /// </summary>
        public int FeralSpirit { get; set; }
        #endregion
        #region Restoration
        /// <summary>
        /// Reduces damage taken while casting spells by [5 * Pts]%.
        /// </summary>
        public int AncestralResolve { get; set; }
        /// <summary>
        /// Reduces the mana cost of your healing spells by [2 * Pts]%.
        /// </summary>
        public int TidalFocus { get; set; }
        /// <summary>
        /// Increases your healing done by [2 * Pts]% and your healing received by [5 * Pts]%.
        /// </summary>
        public int SparkOfLife { get; set; }
        /// <summary>
        /// While Water Shield is active, you recover mana when your direct healing spells have a critical effect.  You regain [1146 * Pts] mana from a Healing Wave or Greater Healing Wave critical, [1375 / 2 * Pts] mana from a Healing Surge, Riptide, or Unleash Life critical, and [763 / 2 * Pts] mana from a Chain Heal critical.
        /// </summary>
        public int Resurgence { get; set; }
        /// <summary>
        /// Reduces the mana cost of your totems by [15 * Pts]% and increases their duration by [20 * Pts]%.
        /// </summary>
        public int TotemicFocus { get; set; }
        /// <summary>
        /// After casting any Shock spell, your next heal's mana cost is reduced by [25 * Pts]% of the cost of the Shock spell, and its healing effectiveness is increased by [10 * Pts]%.
        /// </summary>
        public int FocusedInsight { get; set; }
        /// <summary>
        /// Whenever a damaging attack brings you below 30% health, your maximum health is increased by [5 * Pts]% for 10 sec and your threat level towards the attacker is reduced.  30 second cooldown.
        /// </summary>
        //public int NaturesGuardian { get; set; }
        /// <summary>
        /// Your critical heals reduce physical damage taken by [5 * Pts]% for 15 sec, and heals you cast increase allies' maximum health by [5 * Pts]% of the amount healed, up to a maximum of 10% of their health.
        /// </summary>
        public int AncestralHealing { get; set; }
        /// <summary>
        /// Nature's Swiftness - 2 min cooldown - Instant
        /// When activated, your next Nature spell with a base casting time less than 10 sec. becomes an instant cast spell.
        /// </summary>
        public int NaturesSwiftness { get; set; }
        /// <summary>
        /// Increases the effectiveness of your direct heals on Earth Shielded targets by [6 * Pts]%.
        /// </summary>
        public int NaturesBlessing { get; set; }
        /// <summary>
        /// Increases the amount healed by your Healing Stream Totem by [25 * Pts]%, and your Healing Rain spell by [15 * Pts]%.
        /// </summary>
        public int SoothingRains { get; set; }
        /// <summary>
        /// Empowers your Cleanse Spirit spell to also remove a magic effect from a friendly target.
        /// </summary>
        public int ImprovedCleanseSpirit { get; set; }
        /// <summary>
        /// Reduces the cost of Cleanse Spirit by [20 * Pts]%, and when your Cleanse Spirit successfully removes a harmful effect, you also heal the target for [1317 * Pts] to [1485 * Pts]  The heal effect can only occur once every 6 seconds.
        /// </summary>
        public int CleansingWaters { get; set; }
        /// <summary>
        /// When you critically heal with a single-target direct heal, you summon an Ancestral spirit to aid you, instantly healing the lowest percentage health friendly party or raid target within 40 yards for [10 * Pts]% of the amount healed.
        /// </summary>
        public int AncestralAwakening { get; set; }
        /// <summary>
        /// Mana Tide Totem - 3 min cooldown - Instant cast
        /// Tools: Water Totem - Summons a Mana Tide Totem with 10% of the caster\'s health at the feet of the caster for 12 sec.  Party and raid members within 40 yards of the totem gain 200% of the caster\'s Spirit (excluding short-duration Spirit bonuses).
        /// </summary>
        public int ManaTideTotem { get; set; }
        /// <summary>
        /// Your attunement to natural energies causes your Lightning Bolt spell to restore mana equal to [20 * Pts]% of damage dealt.
        /// </summary>
        public int TelluricCurrents { get; set; }
        /// <summary>
        /// Spirit Link Totem - 11% of base mana
        /// 3 min cooldown - Instant cast
        /// Summons a Spirit Link Totem with 5 health at the feet of the caster. The totem reduces damage taken by all party and raid members within 10 yards by 10%. Every 1 sec, the health of all affected players is redistributed, such that each player ends up with the same percentage of their maximum health. Lasts 6 sec.
        /// </summary>
        public int SpiritLinkTotem { get; set; }
        /// <summary>
        /// When you cast Chain Heal or Riptide, you gain the Tidal Waves effect, which reduces the cast time of your Healing Wave and Greater Healing Wave spells by [10 * Pts]% and increases the critical effect chance of your Healing Surge spell by [10 * Pts]%. 2 charges.
        /// </summary>
        public int TidalWaves { get; set; }
        /// <summary>
        /// Grants an additional [40 * Pts]% chance to trigger your Earthliving heal over time effect when you heal an ally who is below 35% of total health.
        /// </summary>
        public int BlessingOfTheEternals { get; set; }
        /// <summary>
        /// Riptide - 40 yd range - 10% of base mana
        /// 6 sec cooldown - Instant cast
        /// Heals a friendly target for 2363 and another 5585 over 15 sec.  Your next Chain Heal cast on that primary target within 15 sec will consume the healing over time effect and increase the amount of the Chain Heal by 25%.
        /// </summary>
        public int Riptide { get; set; }
        #endregion
    }

    /*public partial class MageTalents : TalentsBase
    {
        #region Arcane
        /// <summary>
        /// Gives you a [10 / 3 * Pts]% chance of entering a Clearcasting state after any damage spell hits a target.  The Clearcasting state reduces the mana cost of your next damage spell by 100%.
        /// </summary>
        public int ArcaneConcentration { get; set; }
        /// <summary>
        /// Your Counterspell also silences the target for [2 * Pts] sec.
        /// </summary>
        public int ImprovedCounterspell { get; set; }
        /// <summary>
        /// Increases your spell haste by [1 * Pts]%.
        /// </summary>
        public int NetherwindPresence { get; set; }
        /// <summary>
        /// Your Arcane damage spells deal [2 * Pts]% more damage to snared or slowed targets.
        /// </summary>
        public int TormentTheWeak { get; set; }
        /// <summary>
        /// You gain a [5 * Pts]% damage bonus for 8 sec after successfully interrupting a spell.
        /// </summary>
        //public int Invocation { get; set; }
        /// <summary>
        /// Increases the number of missiles fired by your Arcane Missiles spell by [1 * Pts]
        /// </summary>
        public int ImprovedArcaneMissiles { get; set; }
        /// <summary>
        /// Increases your speed by [35 * Pts]% for 3 sec after casting the Blink spell.
        /// </summary>
        public int ImprovedBlink { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Presence of Mind, Arcane Power and Invisibility spells by [25 / 2 * Pts]% and the cooldown of your Evocation spell by [1 * Pts] min.
        /// </summary>
        public int ArcaneFlows { get; set; }
        /// <summary>
        /// Presence of Mind - 2 min cooldown - Instant
        /// When activated, your next Mage spell with a casting time less than 10 sec becomes an instant cast spell.
        /// </summary>
        //public int PresenceOfMind { get; set; }
        /// <summary>
        /// Your Arcane Missiles spell will fire its missiles every [0.6 * Pts] sec.
        /// </summary>
        public int MissileBarrage { get; set; }
        /// <summary>
        /// Reduces all damage taken by [2 * Pts]% and reduces the fade time of your Invisibility spell by [1 * Pts] sec.
        /// </summary>
        public int PrismaticCloak { get; set; }
        /// <summary>
        /// When a target you've polymorphed is damaged, that target is stunned for [3 / 2 * Pts] sec.  This effect cannot occur more often than once every 10 sec.
        /// </summary>
        public int ImprovedPolymorph { get; set; }
        /// <summary>
        /// Increases the damage of all party and raid members within 12 yards by 3%.
        /// </summary>
        public int ArcaneTactics { get; set; }
        /// <summary>
        /// When your Mana Shield or Mage Ward absorbs damage your spell damage is increased by [10 * Pts]% of the amount absorbed for 10 sec.  In addition, when your Mana Shield is destroyed, all enemies within 6 yards are knocked back 12 yards.
        /// </summary>
        public int IncantersAbsorption { get; set; }
        /// <summary>
        /// Reduces the global cooldown of your Arcane Explosion spell by [0.3 * Pts] sec, reduces the threat generated by [40 * Pts]%, and reduces the mana cost by [25 * Pts]%.
        /// </summary>
        public int ImprovedArcaneExplosion { get; set; }
        /// <summary>
        /// Increases the critical strike chance of your next two damaging spells by [15 / 2 * Pts]% after gaining Clearcasting or Presence of Mind.
        /// </summary>
        public int ArcanePotency { get; set; }
        /// <summary>
        /// Slow - 35 yd range - 12% of base mana
        /// Instant cast
        /// Reduces target\'s movement speed by 60%, increases the time between ranged attacks by 60% and increases casting time by 30%.  Lasts 15 sec.  Slow can only affect one target at a time.
        /// </summary>
        public int Slow { get; set; }
        /// <summary>
        /// Gives your Arcane Blast spell a [50 * Pts]% chance to apply the Slow spell to any target it damages if no target is currently affected by Slow.
        /// </summary>
        public int NetherVortex { get; set; }
        /// <summary>
        /// Focus Magic - 30 yd range - 6% of base mana
        /// Instant cast
        /// Increases the target\'s chance to critically hit with spells by 3% for 30 min.  When the target critically hits your chance to critically hit with spells is increased by 3% for 10 sec.  Cannot be cast on self.  Limit 1 target.
        /// </summary>
        public int FocusMagic { get; set; }
        /// <summary>
        /// Mana gained from your Mana Gem also increases your spell power by [1 * Pts]% of your maximum mana for 15 sec.
        /// </summary>
        public int ImprovedManaGem { get; set; }
        /// <summary>
        /// Arcane Power - 2 min cooldown - Instant
        /// When activated, you deal 20% more spell damage and damaging spells cost 10% more mana to cast. This effect lasts ???.
        /// </summary>
        public int ArcanePower { get; set; }
        #endregion
        #region Fire
        /// <summary>
        /// Your spell criticals will refund [15 * Pts]% of their base mana cost.
        /// </summary>
        public int MasterOfElements { get; set; }
        /// <summary>
        /// Reduces the casting time lost from taking damaging attacks by [70 / 3 * Pts]%.
        /// </summary>
        public int BurningSoul { get; set; }
        /// <summary>
        /// Increases the critical strike chance of your Fire Blast spell by [4 * Pts]% and increases its range by [5 * Pts] yards.
        /// </summary>
        public int ImprovedFireBlast { get; set; }
        /// <summary>
        /// Your critical strikes from non-periodic Fire damage spells cause the target to burn for an additional [40 / 3 * Pts]% of your spell's damage over 4 sec.
        /// </summary>
        public int Ignite { get; set; }
        /// <summary>
        /// Increases the damage of your Fire spells by [1 * Pts]% and gives your Flame Orb a [100 / 3 * Pts]% chance to explode for 1134 to 1336 damage at the end of its duration.
        /// </summary>
        public int FirePower { get; set; }
        /// <summary>
        /// Gives you a [5 * Pts]% chance when hit by a melee or ranged attack to increase your movement speed by 50% and dispel all effects that prevent movement.  This effect lasts 8 sec.
        /// </summary>
        //public int BlazingSpeed { get; set; }
        /// <summary>
        /// Gives your damaging spells a [5 * Pts]% chance to reset the cooldown on Fire Blast and to cause the next Fire Blast you cast to stun the target for 2 sec and spread any Fire damage over time effects to nearby enemy targets within 32 yards.
        /// </summary>
        public int Impact { get; set; }
        /// <summary>
        /// You have a [50 * Pts]% chance that an attack which would otherwise kill you will instead bring you to 40% of your maximum health.  However, you will burn for 12% of your maximum health every 1.5 sec for the next 6 sec.  This effect cannot occur more than once per minute.
        /// </summary>
        //public int Cauterize { get; set; }
        /// <summary>
        /// Blast Wave - 40 yd range - 7% of base mana
        /// 15 sec cooldown - Instant cast
        /// A wave of flame radiates outward from the target location, damaging all enemies caught within the blast for 851 to 1003 Fire damage and are slowed by 70% for 3 sec.
        /// </summary>
        public int BlastWave { get; set; }
        /// <summary>
        /// Your spells no longer trigger Arcane Missiles.  Instead, your critical strikes with Fireball, Frostfire Bolt, Scorch, Pyroblast, or Fire Blast have a chance to cause your next Pyroblast spell cast within 15 sec to be instant cast and cost no mana.
        /// </summary>
        public int HotStreak { get; set; }
        /// <summary>
        /// Reduces the mana cost of your Scorch spell by [50 * Pts]%.
        /// </summary>
        public int ImprovedScorch { get; set; }
        /// <summary>
        /// Reduces the global cooldown of your Mage Ward spell by 1 sec and your Blazing Speed also removes any movement slowing effects when triggered and is also triggered any time Mage Ward dissipates from absorbing damage.
        /// </summary>
        public int MoltenShields { get; set; }
        /// <summary>
        /// Combustion - 40 yd range
        /// 2 min cooldown - Instant
        /// Combines your damaging periodic Fire effects on an enemy target but does not consume them, instantly dealing 954 to 1131 Fire damage and creating a new periodic effect that lasts 10 sec and deals damage per time equal to the sum of the combined effects.
        /// </summary>
        public int Combustion { get; set; }
        /// <summary>
        /// Any time you score 2 non-periodic critical strikes in a row with your Fireball, Frostfire Bolt, Scorch, Pyroblast, or Fire Blast spells, you have a [50 * Pts]% chance to trigger your Hot Streak effect.
        /// </summary>
        public int ImprovedHotStreak { get; set; }
        /// <summary>
        /// Allows you to cast the Scorch spell while moving.
        /// </summary>
        public int Firestarter { get; set; }
        /// <summary>
        /// Reduces the casting time of your Flamestrike spell by [50 * Pts]% and gives you a [50 * Pts]% chance that your Blast Wave spell will also automatically Flamestrike the same location if two or more targets are affected by the Blast Wave.
        /// </summary>
        public int ImprovedFlamestrike { get; set; }
        /// <summary>
        /// Dragon's Breath - 7% of base mana
        /// 20 sec cooldown - Instant cast
        /// Targets in a cone in front of the caster take 1194 to 1388 Fire damage and are disoriented for 5 sec.  Any direct damaging attack will revive targets.
        /// </summary>
        public int DragonsBreath { get; set; }
        /// <summary>
        /// Increases damage of all spells against targets with less than 35% health by [4 * Pts]%.
        /// </summary>
        public int MoltenFury { get; set; }
        /// <summary>
        /// Increases spell haste by [5 * Pts]% if 3 or more targets are taking Fire damage over time from your spells.
        /// </summary>
        public int Pyromaniac { get; set; }
        /// <summary>
        /// Your Living Bomb and Flame Orb spells deal [5 * Pts]% more damage, and your Pyroblast and Scorch spells have a [100 / 3 * Pts]% chance to cause your target to be vulnerable to spell damage, increasing spell critical strike chance against that target by 5% and lasts 30 sec.
        /// </summary>
        public int CriticalMass { get; set; }
        /// <summary>
        /// Living Bomb - 40 yd range - 17% of base mana
        /// Instant cast
        /// The target becomes a Living Bomb, taking 928 Fire damage over 12 sec.  After 12 sec, the target explodes dealing 465 Fire damage to up to 3 enemies within 10 yards.  Limit 3 targets.
        /// </summary>
        //public int LivingBomb { get; set; }
        #endregion
        #region Frost
        /// <summary>
        /// Reduces the cast time of your Frostbolt spell by [0.3 * Pts] secs.  This effect becomes inactive for 15 sec after use.
        /// </summary>
        public int EarlyFrost { get; set; }
        /// <summary>
        /// Increases the critical strike chance of your spells by [1 * Pts]%.
        /// </summary>
        public int PiercingIce { get; set; }
        /// <summary>
        /// Multiplies the critical strike chance of all your spells against frozen targets by [3 / 2 * Pts] and increases the damage done by Frostbolt against frozen targets by [10 * Pts]%.
        /// </summary>
        public int Shatter { get; set; }
        /// <summary>
        /// Reduces the cooldown of your Frost Nova, Cone of Cold, Ice Block, Cold Snap, Ice Barrier, and Icy Veins spells by [20 / 3 * Pts]%.
        /// </summary>
        public int IceFloes { get; set; }
        /// <summary>
        /// Your Cone of Cold also freezes targets for [2 * Pts] sec.
        /// </summary>
        public int ImprovedConeOfCold { get; set; }
        /// <summary>
        /// Your Frostbolt criticals apply the chill effect to [1 * Pts] additional nearby targets.
        /// </summary>
        public int PiercingChill { get; set; }
        /// <summary>
        /// Your Chill effects reduce the target's speed by an additional [10 / 3 * Pts]%, and the target's healing received.  In addition, whenever you deal spell damage, your Water Elemental is healed for [5 * Pts]% of the amount dealt.
        /// </summary>
        public int Permafrost { get; set; }
        /// <summary>
        /// Adds a chill effect to your Blizzard spell.  This effect lowers the target's movement speed by [40 / 2 * Pts]%.  Lasts 2 sec.  In addition, increases the range of your Ice Lance spell by [5 / 2 * Pts] yards.
        /// </summary>
        public int IceShards { get; set; }
        /// <summary>
        /// Icy Veins - 3% of base mana
        /// 3 min cooldown - Instant
        /// Hastens your spellcasting, increasing spell casting speed by 20% and reduces the pushback suffered from damaging attacks while casting by 100%.  Lasts 20 sec.
        /// </summary>
        public int IcyVeins { get; set; }
        /// <summary>
        /// Gives your offensive Chill effects a [20 / 3 * Pts]% chance to grant you the Fingers of Frost effect, which causes your next Ice Lance or Deep Freeze spell to act as if your target were frozen and increases Ice Lance damage by 25%.  Fingers of Frost can accumulate up to 2 charges and lasts 15 sec.
        /// </summary>
        public int FingersOfFrost { get; set; }
        /// <summary>
        /// Gives your Water Elemental's Freeze spell a [100 / 3 * Pts]% chance to grant 2 charges of Fingers of Frost when it strikes enemy targets.
        /// </summary>
        public int ImprovedFreeze { get; set; }
        /// <summary>
        /// Reduces the mana cost of all spells by [10 / 3 * Pts]%.  In addition, your Frostbolt spell has a [100 / 3 * Pts]% chance to grant up to 10 party or raid members mana regeneration equal to 1% of their maximum mana over 10 sec.
        /// </summary>
        public int EnduringWinter { get; set; }
        /// <summary>
        /// Cold Snap - 8 min cooldown - Instant
        /// When activated, this spell finishes the cooldown on all Frost spells you recently cast.
        /// </summary>
        //public int ColdSnap { get; set; }
        /// <summary>
        /// Your spells no longer trigger Arcane Missiles.  Instead, your Frost damage spells with chilling effects have a [5 * Pts]% chance to cause your next Fireball or Frostfire Bolt spell to be instant cast and cost no mana.  When Frostfire Bolt is instant, it can benefit from Fingers of Frost.  Brain Freeze cannot be triggered by Frostfire Bolt.
        /// </summary>
        public int BrainFreeze { get; set; }
        /// <summary>
        /// Gives your Ice Barrier spell a 100% chance to freeze all enemies within 13 yds for [2 * Pts] sec when it is destroyed.
        /// </summary>
        public int ShatteredBarrier { get; set; }
        /// <summary>
        /// Ice Barrier - 21% of base mana
        /// 30 sec cooldown - Instant cast
        /// Instantly shields you, absorbing [[8069 + 87% of SPFR]] damage.  Lasts 1 min.  While the shield holds, spellcasting will not be delayed by damage.
        /// </summary>
        //public int IceBarrier { get; set; }
        /// <summary>
        /// Gives the caster a [50 * Pts]% chance for the Ice Barrier spell to automatically cast with no mana cost upon taking damage that lowers the caster's life below 50%.  This effect obeys Ice Barrier's cooldown, and will trigger the cooldown when activated.
        /// </summary>
        public int ReactiveBarrier { get; set; }
        /// <summary>
        /// Your Frostfire Orb gains a chill effect, slowing targets damaged by the Frostfire Orb by 40% for 2 sec.   In addition, reduces the speed of targets slowed by your Frostfire Bolt's chill effect by an [10 * Pts] [0 * Pts]%.
        /// </summary>
        public int FrostfireOrb { get; set; }
        /// <summary>
        /// Deep Freeze - 35 yd range - 9% of base mana
        /// 30 sec cooldown - Instant cast
        /// Stuns the target for 5 sec.  Only usable on Frozen targets.  Deals 1144 to 1434 damage to targets that are permanently immune to stuns.
        /// </summary>
        public int DeepFreeze { get; set; }
        #endregion
    }*/

    public partial class WarlockTalents : TalentsBase
    {
        #region Affliction
        /// <summary>
        /// Increases the critical strike chance of your Bane of Agony and Bane of Doom by [4 * Pts]%.
        /// </summary>
        public int DoomAndGloom { get; set; }
        /// <summary>
        /// Increases the amount of Mana awarded by your Life Tap spell by [10 * Pts]%.
        /// </summary>
        public int ImprovedLifeTap { get; set; }
        /// <summary>
        /// Increases the damage done by your Corruption by [4 * Pts]%.
        /// </summary>
        public int ImprovedCorruption { get; set; }
        /// <summary>
        /// Your Curse of the Elements also affects up to 15 nearby enemy targets within [20 * Pts] yards of the cursed target. 
        /// 
        /// Also, your Curse of Weakness increases the target's energy, rage, focus or runic power costs of abilities by [5 * Pts]% while active.
        /// </summary>
        public int Jinx { get; set; }
        /// <summary>
        /// Increases the amount drained by your Drain Life and Drain Soul spells by an additional [3 * Pts]% for each of your Affliction effects on the target, up to a maximum of [9 * Pts]% additional effect.
        /// </summary>
        public int SoulSiphon { get; set; }
        /// <summary>
        /// When you deal damage with your Corruption spell, you have a [25 * Pts]% chance to be healed for 2% of your total health.
        /// </summary>
        public int SiphonLife { get; set; }
        /// <summary>
        /// Curse of Exhaustion - 40 yd range - 6% of base mana
        /// Instant cast
        /// Reduces the target\'s movement speed by 30% for 30 sec.  Only one Curse per Warlock can be active on any one target.
        /// </summary>
        public int CurseOfExhaustion { get; set; }
        /// <summary>
        /// Causes your Fear spell to inflict a Nightmare on the target when the fear effect ends. The Nightmare effect reduces the target's movement speed by [15 * Pts]% for 5 sec.
        /// </summary>
        public int ImprovedFear { get; set; }
        /// <summary>
        /// When you deal damage with Corruption, you have 6% chance to increase your spell casting speed by [20 / 3 * Pts]% for 10 sec.
        /// </summary>
        public int Eradication { get; set; }
        /// <summary>
        /// Reduces the casting time of your Howl of Terror spell by [0.8 * Pts] sec.
        /// </summary>
        public int ImprovedHowlOfTerror { get; set; }
        /// <summary>
        /// Soul Swap - 40 yd range - 18% of base mana
        /// Instant cast
        /// You instantly deal 167 damage, and remove your Shadow damage-over-time effects from the target.
        /// 
        /// For 20 sec afterwards, the next target you cast Soul Swap: Exhale on will be afflicted by the Shadow damage-over-time effects and suffer 167 damage.
        /// 
        /// You cannot Soul Swap to the same target.
        /// </summary>
        public int SoulSwap { get; set; }
        /// <summary>
        /// Your Shadow Bolt and Haunt spells apply the Shadow Embrace effect, increasing all Shadow periodic damage dealt to the target by you by [5 / 3 * Pts]%. Stacks up [12 * Pts] 0 times.
        /// </summary>
        public int ShadowEmbrace { get; set; }
        /// <summary>
        /// While at or below 25% health, your Drain Life heals an additional [1 * Pts]% of your total health.
        /// 
        /// Increases the damage done by your Shadow Spells by [4 * Pts]% when your target is at or below 25% health.
        /// </summary>
        public int DeathsEmbrace { get; set; }
        /// <summary>
        /// Gives your Corruption and Drain Life spells a [2 * Pts]% chance to cause you to enter a Shadow Trance state after damaging the opponent.  The Shadow Trance state reduces the casting time of your next Shadow Bolt spell by 100%.
        /// </summary>
        public int Nightfall { get; set; }
        /// <summary>
        /// Grants the Soulburn empowerment to your Seed of Corruption spell.
        /// 
        /// Your Seed of Corruption detonation effect will afflict Corruption on all enemy targets. The Soul Shard will be refunded if the detonation is successful.
        /// </summary>
        public int SoulburnSeedOfCorruption { get; set; }
        /// <summary>
        /// Increases the critical effect chance of your Corruption, Seed of Corruption and Unstable Affliction by [5 * Pts]%.
        /// 
        /// And your Drain Life, Drain Soul, and Haunt spells have a [100 / 3 * Pts]% chance to reset the duration of your Corruption spell on the target.
        /// </summary>
        public int EverlastingAffliction { get; set; }
        /// <summary>
        /// Reduces the global cooldown of your Bane and Curse spells by [0.25 * Pts] sec.
        /// 
        /// Your Drain Soul has a [50 * Pts]% chance to refresh the duration of your Unstable Affliction spell when dealing damage on targets below 25% health.
        /// </summary>
        public int Pandemic { get; set; }
        /// <summary>
        /// Haunt - 40 yd range - 12% of base mana
        /// 8 sec cooldown - 1.5 sec cast
        /// You send a ghostly soul into the target, dealing [922 + 69.71% of SP] Shadow damage and increasing all damage done by your Shadow damage-over-time effects on the target by 20% for 12 sec. When the Haunt spell ends or is dispelled, the soul returns to you, healing you for 100% of the damage it did to the target.
        /// </summary>
        public int Haunt { get; set; }
        #endregion
        #region Demonology
        /// <summary>
        /// Increases your total Stamina by [10 / 3 * Pts]%.
        /// </summary>
        public int DemonicEmbrace { get; set; }
        /// <summary>
        /// Reduces the cast time of your Imp's Firebolt spell by [0.25 * Pts] sec, increases the damage done by your Felguard's Legion Strike by [5 * Pts]%, and increases the damage done by your Felhunter's Shadow Bite by [5 * Pts]%.
        /// </summary>
        public int DarkArts { get; set; }
        /// <summary>
        /// You have a [50 * Pts]% chance to heal your pet for 15% of the amount of spell damage done by you.
        /// </summary>
        public int FelSynergy { get; set; }
        /// <summary>
        /// If your summoned demon dies, you gain the Demonic Rebirth effect reducing the cast time of your next summon demon spell by [50 * Pts]%. Lasts for 10 sec. This effect has a 2 min cooldown.
        /// </summary>
        public int DemonicRebirth { get; set; }
        /// <summary>
        /// When your summoned demon critically hits with its Basic Attack, you instantly gain [2 * Pts]% total mana.
        /// 
        /// When you gain mana from Life Tap, your summoned demon gains [30 * Pts]% of the mana you gain.
        /// </summary>
        public int ManaFeed { get; set; }
        /// <summary>
        /// Increases the amount of health generated through spells and effects granted by your Demon Armor by an additional [5 * Pts]%, and increases the amount of health returned by your Fel Armor by [50 * Pts]%.
        /// </summary>
        public int DemonicAegis { get; set; }
        /// <summary>
        /// Reduces the casting time of your Imp, Voidwalker, Succubus, Felhunter and Felguard Summoning spells by [0.5 * Pts] sec and the Mana cost by [50 * Pts]%.
        /// </summary>
        public int MasterSummoner { get; set; }
        /// <summary>
        /// Increases the chance for your Bane of Doom to summon a demon by [10 * Pts]% when it deals damage.
        /// 
        /// Also grants your Shadow Bolt, Hand of Gul'dan, Soul Fire, and Incinerate spells a [5 * Pts]% chance to reduce the cooldown of your Demon Form by 15 sec.
        /// </summary>
        public int ImpendingDoom { get; set; }
        /// <summary>
        /// Demonic Empowerment - 100 yd range - 6% of base mana
        /// 1 min cooldown - Instant
        /// Grants the Warlock\'s summoned demon Empowerment.
        /// 
        /// Imp - Instantly heals the Imp for 30% of its total health.
        /// 
        /// Voidwalker - Increases the Voidwalker\'s health by 20%, and its threat generated from spells and attacks by 20% for 20 sec.
        /// 
        /// Succubus - Instantly vanishes, causing the Succubus to go into an improved Invisibility state. The vanish effect removes all stuns, snares and movement impairing effects from the Succubus.
        /// 
        /// Felhunter - Dispels all magical effects from the Felhunter.
        /// 
        /// Felguard - Instantly removes all stun, snare, fear, banish, or horror and movement impairing effects from your Felguard and makes your Felguard immune to them for 15 sec.
        /// </summary>
        public int DemonicEmpowerment { get; set; }
        /// <summary>
        /// Increases the amount of Health transferred by your Health Funnel spell by [10 * Pts]% and reduces the health cost by [10 * Pts]%. In addition, your summoned demon takes [15 * Pts]% less damage while under the effect of your Health Funnel.
        /// </summary>
        public int ImprovedHealthFunnel { get; set; }
        /// <summary>
        /// You have a [2 * Pts]% chance to gain the Molten Core effect when your Immolate deals damage. The Molten Core effect empowers your next 3 Incinerate spells cast within 15 sec, increasing damage done by [6 * Pts]% and reducing cast time by [10 * Pts]%.
        /// </summary>
        public int MoltenCore { get; set; }
        /// <summary>
        /// Hand of Gul'dan - 40 yd range - 7% of base mana
        /// 12 sec cooldown - 2 sec cast
        /// Summons a falling meteor down upon the enemy target, dealing 1405 to 1660 Shadowflame damage and erupts an aura of magic within 4 yards, causing all targets within it to have a 10% increased  chance to be critically hit by any Warlock demons. The aura lasts for 15 sec.
        /// </summary>
        public int HandOfGuldan { get; set; }
        /// <summary>
        /// When your Hand of Gul'dan lands, all enemies within 4 yards will be rooted for [3 / 2 * Pts] sec and stunned for the same duration if they are still within the Curse of Gul'dan aura 6 sec afterward.
        /// </summary>
        public int AuraOfForeboding { get; set; }
        /// <summary>
        /// Increases the duration of your Infernal and Doomguard summons by [10 * Pts] sec.
        /// </summary>
        public int AncientGrimoire { get; set; }
        /// <summary>
        /// Enables you to channel Hellfire while moving, and increases the duration of your Immolate by [6000/1000)] sec.
        /// </summary>
        public int Inferno { get; set; }
        /// <summary>
        /// When you Shadowbolt, Incinerate or Soul Fire a target that is at or below 25% health, the cast time of your Soul Fire spell is reduced by [20 * Pts]% for 10 sec.
        /// </summary>
        public int Decimation { get; set; }
        /// <summary>
        /// Increases the damage done by your Hellfire by [15 * Pts]%, and your Hand of Gul'dan has a [50 * Pts]% chance to refresh the duration of your Immolate on the target.
        /// </summary>
        public int Cremation { get; set; }
        /// <summary>
        /// Increases your spell damage by 2%, and your summoned demon grants the Demonic Pact effect to all nearby friendly party and raid members.
        /// 
        /// The Demonic Pact effect increases spell power by 10%.
        /// </summary>
        public int DemonicPact { get; set; }
        /// <summary>
        /// Metamorphosis - Instant
        /// You transform into a Demon for 30 sec.  This form increases your armor by 600%, damage by 20%, reduces the chance you\'ll be critically hit by melee attacks by 6% and reduces the duration of stun and snare effects by 50%.  You gain some unique demon abilities in addition to your normal abilities. 3 minute cooldown.
        /// </summary>
        public int Metamorphosis { get; set; }
        #endregion
        #region Destruction
        /// <summary>
        /// Reduces the casting time of your Shadow Bolt, Chaos Bolt and Immolate spells by [0.1 * Pts] sec.
        /// </summary>
        public int Bane { get; set; }
        /// <summary>
        /// Increases the damage done by your Shadow Bolt and Incinerate spells by [4 * Pts]%, and your Shadow Bolt and Incinerate have a [100 / 3 * Pts]% chance to cause the Shadow and Flame effect to the target.
        /// 
        /// The Shadow and Flame effect causes the target to be vulnerable to spell damage, increasing spell critical strike chance against that target by 5% for 30 sec.
        /// </summary>
        public int ShadowAndFlame { get; set; }
        /// <summary>
        /// Increases the damage done by your Immolate spell by [10 * Pts]%.
        /// </summary>
        public int ImprovedImmolate { get; set; }
        /// <summary>
        /// Your Rain of Fire has a [6 * Pts]% chance to Stun targets for 2 sec., and your Conflagrate has a [50 * Pts]% chance to daze the target for 5 sec.
        /// </summary>
        public int Aftermath { get; set; }
        /// <summary>
        /// Reduces the cast time of your Soul Fire by [0.5 * Pts] sec and your Incinerate by [0.13 * Pts] sec.
        /// </summary>
        public int Emberstorm { get; set; }
        /// <summary>
        /// Increases the critical strike chance of your Searing Pain spell by [20 * Pts]% on targets at or below 25% health.
        /// </summary>
        public int ImprovedSearingPain { get; set; }
        /// <summary>
        /// Increases your Fire and Shadow damage done by [4 * Pts]% for 20 sec after you deal damage with Soul Fire.
        /// </summary>
        public int ImprovedSoulFire { get; set; }
        /// <summary>
        /// When you cast Conflagrate, the cast time for your next three Shadow Bolt, Incinerate and Chaos Bolt spells is reduced by [10 * Pts]%. Lasts 15 sec.
        /// </summary>
        public int Backdraft { get; set; }
        /// <summary>
        /// Shadowburn - 40 yd range - 15% of base mana
        /// 15 sec cooldown - Instant cast
        /// Instantly blasts the target for 649 to 724 Shadowflame damage.  If the target dies within 5 sec of Shadowburn, and yields experience or honor, the caster gains 3 Soul Shards. Only usable on enemies that have less than 20% health.
        /// </summary>
        public int Shadowburn { get; set; }
        /// <summary>
        /// Your Soul Fire and your Imp's Firebolt cause a Burning Ember damage-over-time effect on the target equal to [25 * Pts]% of the damage done lasting 7 sec.
        /// 
        /// The Burning Ember effect deals up to [[10 * Pts].1 + 20% of SP] fire damage every 1 sec for 7 sec.
        /// </summary>
        public int BurningEmbers { get; set; }
        /// <summary>
        /// Your Shadowburn, Soul Fire and Chaos Bolt instantly restore [2 * Pts]% of your total health and mana when they deal damage and also grant Replenishment.
        /// 
        /// Replenishment - Grants up to 10 party or raid members mana regeneration equal to 1% of their maximum mana per 10 sec. Lasts for 15 sec.
        /// </summary>
        //public int SoulLeech { get; set; }
        /// <summary>
        /// Gives you a [25 / 3 * Pts]% chance when hit by a physical attack to reduce the cast time of your next Shadow Bolt or Incinerate spell by 100%.  This effect lasts 8 sec and will not occur more than once every 8 seconds.
        /// </summary>
        public int Backlash { get; set; }
        /// <summary>
        /// Transforms your Shadow Ward into Nether Ward. You must be within Demon Armor or Fel Armor in order for the transformation effect to occur.
        /// 
        /// Nether Ward
        /// Absorbs [3551 + 80.7% of SP] spell damage.  Lasts 30 sec. 30 sec cooldown.
        /// </summary>
        public int NetherWard { get; set; }
        /// <summary>
        /// Increases the damage done by your Incinerate and Chaos Bolt spells to targets afflicted by your Immolate by [5 * Pts]%, and the critical strike chance of your Conflagrate spell is increased by [5 * Pts]%.
        /// </summary>
        public int FireAndBrimstone { get; set; }
        /// <summary>
        /// Shadowfury - 30 yd range - 27% of base mana
        /// 20 sec cooldown - Instant cast
        /// Shadowfury is unleashed, causing 687 to 819 Shadow damage and stunning all enemies within 8 yds for 3 sec.
        /// </summary>
        //public int Shadowfury { get; set; }
        /// <summary>
        /// When you absorb damage through Shadow Ward, Nether Ward or other effects, you gain Nether Protection, reducing all damage by that spell school by [15 * Pts]% for 12 sec.
        /// </summary>
        public int NetherProtection { get; set; }
        /// <summary>
        /// Your Imp's Firebolt has a [2 * Pts]% chance to cause your next Soul Fire spell to be instant cast within 8 sec.
        /// </summary>
        public int EmpoweredImp { get; set; }
        /// <summary>
        /// Bane of Havoc - 40 yd range - 10% of base mana
        /// Instant cast
        /// Banes the target for 5 min, causing 15% of all damage done by the Warlock to other targets to also be dealt to the baned target. 
        /// 
        /// Only one target can have Bane of Havoc at a time, and only one Bane per Warlock can be active on any one target.
        /// </summary>
        public int BaneOfHavoc { get; set; }
        /// <summary>
        /// Chaos Bolt - 40 yd range - 7% of base mana
        /// 12 sec cooldown - 2.5 sec cast
        /// Sends a bolt of chaotic fire at the enemy, dealing 1311 to 1665 Fire damage. Chaos Bolt cannot be resisted, and pierces through all absorption effects.
        /// </summary>
        public int ChaosBolt { get; set; }
        #endregion
    }

    public partial class DruidTalents : TalentsBase
    {
        #region Balance
        /// <summary>
        /// You gain [5 * Pts]% spell haste after you cast Moonfire, Regrowth, or Insect Swarm, lasting 15 sec. This effect has a 1 minute cooldown. When you gain Lunar or Solar Eclipse, the cooldown of Nature's Grace is instantly reset.
        /// </summary>
        public int NaturesGrace { get; set; }
        /// <summary>
        /// Reduces the cast time of your Wrath and Starfire spells by [0.15 * Pts] sec.
        /// </summary>
        public int StarlightWrath { get; set; }
        /// <summary>
        /// Increases the critical strike chance with spells by [2 * Pts]%.
        /// </summary>
        public int NaturesMajesty { get; set; }
        /// <summary>
        /// Increases the healing done by your periodic spells and by Swiftmend by [2 * Pts]%, and increases the duration of your Moonfire and Insect Swarm by [2 * Pts] sec.
        /// </summary>
        public int Genesis { get; set; }
        /// <summary>
        /// Reduces the mana cost of your damage and healing spells by [3 * Pts]%.
        /// </summary>
        public int Moonglow { get; set; }
        /// <summary>
        /// Increases your Nature and Arcane spell damage by [1 * Pts]% and increases your spell hit rating by an additional amount equal to [50 * Pts]% of any Spirit gained from items or effects.
        /// </summary>
        public int BalanceOfPower { get; set; }
        /// <summary>
        /// While not in an Eclipse state, you have a [12 * Pts]% chance to double the Solar or Lunar energy generated by your Wrath or Starfire when they deal damage.
        /// 
        /// When you reach a Solar or Lunar eclipse, you instantly are restored [8 * Pts]% of your total mana.
        /// </summary>
        public int Euphoria { get; set; }
        /// <summary>
        /// Shapeshift - Moonkin Form - 13% of base mana
        /// Instant cast
        /// Shapeshift into Moonkin Form, increasing Arcane and Nature spell damage by 10%, reducing all damage taken by 15%, and increases spell haste of all party and raid members by 5%. The Moonkin can not cast healing or resurrection spells while shapeshifted.
        /// 
        /// The act of shapeshifting frees the caster of movement impairing effects.
        /// </summary>
        public int MoonkinForm { get; set; }
        /// <summary>
        /// Typhoon - 30 yd range - 16% of base mana
        /// 20 sec cooldown - Instant cast
        /// You summon a violent Typhoon that does 1298 Nature damage to targets in front of the caster within 30 yards, knocking them back and dazing them for 6 sec.
        /// </summary>
        //public int Typhoon { get; set; }
        /// <summary>
        /// You have a [2 * Pts]% chance when you deal damage with your Moonfire or Insect Swarm to instantly reset the cooldown of your Starsurge and reduce its cast time by 100%. Lasts 12 sec.
        /// </summary>
        public int ShootingStars { get; set; }
        /// <summary>
        /// Attacks done to you while in Moonkin Form have a [5 * Pts]% chance to cause you to go into a Frenzy, increasing your damage by 10% and causing you to be immune to pushback while casting Balance spells. Lasts 10 sec.
        /// </summary>
        public int OwlkinFrenzy { get; set; }
        /// <summary>
        /// Increases damage done by your Hurricane and Typhoon spells by [15 * Pts]%, and increases the range of your Cyclone spell by [2 * Pts] yards.
        /// </summary>
        public int GaleWinds { get; set; }
        /// <summary>
        /// Solar Beam - 40 yd range - 18% of base mana
        /// 1 min cooldown - Instant
        /// You summon a beam of solar light over an enemy target\'s location, interrupting the target and silencing all enemies under the beam while it is active. Solar Beam lasts for 10 sec.
        /// </summary>
        public int SolarBeam { get; set; }
        /// <summary>
        /// When you cast your Innervate on yourself, you regain an additional [15 * Pts]% of your total mana over its duration.
        /// </summary>
        public int Dreamstate { get; set; }
        /// <summary>
        /// Force of Nature - 40 yd range - 12% of base mana
        /// 3 min cooldown - Instant cast
        /// Summons 3 treants to attack enemy targets for 30 sec.
        /// </summary>
        //public int ForceOfNature { get; set; }
        /// <summary>
        /// While in Solar Eclipse, your Moonfire spell will morph into Sunfire.
        /// </summary>
        public int Sunfire { get; set; }
        /// <summary>
        /// Your Wrath, Starfire and Wild Mushroom: Detonate spells apply the Earth and Moon effect, which increases spell damage taken by 8% for 15 sec.  Also increases your spell damage by 2%.
        /// </summary>
        public int EarthAndMoon { get; set; }
        /// <summary>
        /// When your Treants die or your Wild Mushrooms are triggered, you spawn a Fungal Growth at its wake covering the area within 8 yards, slowing all enemy targets by [25 * Pts]%. Lasts 20 sec.
        /// </summary>
        public int FungalGrowth { get; set; }
        /// <summary>
        /// When you cast Moonfire, you gain Lunar Shower. Lunar Shower increases the direct damage done by your Moonfire by [15 * Pts]%, and reduces the mana cost by [10 * Pts]%. This effect stacks up to 3 times and lasts 3 sec.  While you are under the effect of Lunar Shower, Moonfire generates 8 Solar Energy and Sunfire generates 8 Lunar Energy.
        /// </summary>
        public int LunarShower { get; set; }
        /// <summary>
        /// Starfall - 35% of base mana
        /// 1 min cooldown - Instant cast
        /// You summon a flurry of stars from the sky on all targets within 40 yards of the caster that you\'re in combat with, each dealing 368 to 428 Arcane damage. Maximum 20 stars. Lasts 10 sec.  
        /// 
        /// Shapeshifting into an animal form or mounting cancels the effect. Any effect which causes you to lose control of your character will suppress the starfall effect.
        /// </summary>
        public int Starfall { get; set; }
        #endregion
        #region Feral Combat
        /// <summary>
        /// Increases your movement speed by [15 * Pts]% in Cat Form and increases your chance to dodge while in Cat Form or Bear Form by [2 * Pts]%.
        /// 
        /// In addition, your Dash and Stampeding Roar have a [50 * Pts]% chance to remove all movement impairing effects from affected targets when used.
        /// </summary>
        public int FeralSwiftness { get; set; }
        /// <summary>
        /// Grants you a [100 / 3 * Pts]% chance to gain 10 Rage when you shapeshift into Bear Form, allows you to keep up to [100 / 3 * Pts] of your Energy when you shapeshift into Cat Form, and increases your maximum mana by [5 * Pts]%.
        /// </summary>
        public int Furor { get; set; }
        /// <summary>
        /// Increases the critical strike chance of your Ravage by [25 * Pts]% on targets at or above 80% health.
        /// 
        /// Your finishing moves have a [10 * Pts]% chance per combo point to make your next non-instant Nature spell with a base casting time of less than 10 sec. become an instant cast spell and cost no mana.
        /// </summary>
        public int PredatoryStrikes { get; set; }
        /// <summary>
        /// Your Shred, Ravage, Maul, and Mangle attacks cause an Infected Wound in the target. The Infected Wound reduces the movement speed of the target by [25 * Pts]% and the attack speed by [10 * Pts]%. Lasts 12 sec.
        /// </summary>
        public int InfectedWounds { get; set; }
        /// <summary>
        /// When you autoattack while in Cat Form or Bear Form, you have a [5 * Pts]% chance to cause a Fury Swipe dealing 310% weapon damage. This effect cannot occur more than once every 3 sec.
        /// </summary>
        public int FurySwipes { get; set; }
        /// <summary>
        /// Gives you a [50 * Pts]% chance to gain an additional 5 Rage anytime you get a critical strike while in Bear Form and your critical strikes from Cat Form abilities that add combo points  have a [50 * Pts]% chance to add an additional combo point.
        /// </summary>
        public int PrimalFury { get; set; }
        /// <summary>
        /// Increases the damage caused by your Ferocious Bite by [5 * Pts]% and causes Faerie Fire (Feral) to apply [3 / 2 * Pts] stacks of the Faerie Fire effect when cast.
        /// </summary>
        public int FeralAggression { get; set; }
        /// <summary>
        /// While using your Enrage ability in Bear Form, your damage is increased by [5 * Pts]%, and your Tiger's Fury ability also instantly restores [20 * Pts] Energy.
        /// </summary>
        public int KingOfTheJungle { get; set; }
        /// <summary>
        /// Rank 1 - Feral Charge - 100 yd range
        /// Instant
        /// Teaches Feral Charge (Bear) and Feral Charge (Cat).
        /// 
        /// Feral Charge (Bear) - Causes you to charge an enemy, immobilizing them for 4 sec. 15 second cooldown.
        /// 
        /// Feral Charge (Cat) - Causes you to leap behind an enemy, dazing them for 3 sec. 30 second cooldown.
        /// </summary>
        public int FeralCharge { get; set; }
        /// <summary>
        /// Increases your melee haste by [15 * Pts]% after you use Feral Charge (Bear) for 8 sec, and your next Ravage will temporarily not require stealth or have a positioning requirement for 10 sec after you use Feral Charge (Cat), and cost [50 * Pts]% less energy.
        /// </summary>
        public int Stampede { get; set; }
        /// <summary>
        /// Increases your Armor contribution from cloth and leather items by [10 / 3 * Pts]%, increases armor while in Bear Form by an additional [26 * Pts]%, and reduces the chance you'll be critically hit by melee attacks by [2 * Pts]%.
        /// </summary>
        public int ThickHide { get; set; }
        /// <summary>
        /// While in Cat Form or Bear Form, the Leader of the Pack increases critical strike chance of all party and raid members within 12 yards by 5%.  In addition, your melee critical strikes in Cat Form and Bear Form cause you to heal for 4% of your total health and gain 8% of your maximum mana.  This effect cannot occur more than once every 6 sec.
        /// </summary>
        public int LeaderOfThePack { get; set; }
        /// <summary>
        /// Increases the stun duration of your Bash and Pounce abilities by [0.5 * Pts] sec, decreases the cooldown of Bash by [5 * Pts] sec, decreases the cooldown of Skull Bash by [25 * Pts] sec, and causes victims of your Skull Bash ability to have [5 * Pts]% increased mana cost for their spells for 10 sec.
        /// </summary>
        public int BrutalImpact { get; set; }
        /// <summary>
        /// Increases your healing spells by up to [50 * Pts]% of your Agility, and increases healing done to you by [10 * Pts]% while in Cat Form.
        /// </summary>
        public int NurturingInstinct { get; set; }
        /// <summary>
        /// Tiger's Fury and Berserk also increase your current and maximum Energy by [10 * Pts] during their durations, and your Enrage and Berserk abilities instantly generate [6 * Pts] Rage.
        /// </summary>
        public int PrimalMadness { get; set; }
        /// <summary>
        /// Cat or Bear Form - Survival Instincts - 3 min cooldown - Instant
        /// Reduces all damage taken by 50% for 12 sec.  Only usable while in Bear Form or Cat Form.
        /// </summary>
        public int SurvivalInstincts { get; set; }
        /// <summary>
        /// Increases the duration of your Rake by [3 * Pts] sec and your Savage Roar and Pulverize by [4 * Pts] sec.
        /// </summary>
        public int EndlessCarnage { get; set; }
        /// <summary>
        /// Reduces damage taken while in Bear Form by [9 * Pts]%, increases your dodge while in Bear Form by [3 * Pts]%, and you generate [1 * Pts] Rage every time you dodge while in Bear Form.
        /// </summary>
        public int NaturalReaction { get; set; }
        /// <summary>
        /// When you Ferocious Bite a target at or below 25% health, you have a [50 * Pts]% chance to instantly refresh the duration of your Rip on the target.
        /// </summary>
        public int BloodInTheWater { get; set; }
        /// <summary>
        /// Increases damage done by your Maul and Shred attacks on bleeding targets by [20 / 3 * Pts]%, and increases the critical strike chance of your Ferocious Bite ability on bleeding targets by [25 / 3 * Pts]%.
        /// </summary>
        public int RendAndTear { get; set; }
        /// <summary>
        /// Bear Form - Pulverize - Melee Range - 15 Rage
        /// Instant cast
        /// Deals 60% weapon damage plus additional 1624 damage for each of your Lacerate applications on the target, and increases your melee critical strike chance by 3% for each Lacerate application consumed for 10 sec.
        /// </summary>
        public int Pulverize { get; set; }
        /// <summary>
        /// Cat or Bear Form - Berserk - 3 min cooldown - Instant
        /// Your Lacerate periodic damage has a 30% chance to refresh the cooldown of your Mangle (Bear) ability and make it cost no rage.  
        /// 
        /// In addition, when activated this ability causes your Mangle (Bear) ability to hit up to 3 targets and have no cooldown, and reduces the energy cost of all your Cat Form abilities by 50%.  Lasts 15 sec.  You cannot use Tiger\'s Fury while Berserk is active.
        /// </summary>
        public int Berserk { get; set; }
        #endregion
        #region Restoration
        /// <summary>
        /// Increases the healing done by your Rejuvenation by [2 * Pts]% and the direct damage of your Moonfire by [3 * Pts]%.
        /// </summary>
        public int BlessingOfTheGrove { get; set; }
        /// <summary>
        /// Reduces the mana cost of all shapeshifting by [10 * Pts]% and increases the duration of Tree of Life Form by [3 * Pts] sec.
        /// </summary>
        public int NaturalShapeshifter { get; set; }
        /// <summary>
        /// Reduces the cast time of your Healing Touch and Nourish spells by [0.25 * Pts] sec.
        /// </summary>
        public int Naturalist { get; set; }
        /// <summary>
        /// Increases your Intellect by [2 * Pts]%.  In addition, while in Bear Form your Stamina is increased by [2 * Pts]% and while in Cat Form your attack power is increased by [10 / 3 * Pts]%.
        /// </summary>
        //public int HeartOfTheWild { get; set; }
        /// <summary>
        /// Reduces all spell damage taken by [2 * Pts]%.
        /// </summary>
        public int Perseverance { get; set; }
        /// <summary>
        /// Grants an effect which lasts while the Druid is within the respective shapeshift form.
        /// 
        /// Bear Form - Increases physical damage by 4%.
        /// 
        /// Cat Form - Increases critical strike chance by 4%.
        /// 
        /// Moonkin Form - Increases spell damage by 4%.
        /// 
        /// Tree of Life/Caster Form - Increases healing by 4%.
        /// </summary>
        public int MasterShapeshifter { get; set; }
        /// <summary>
        /// Increases the effect of your Rejuvenation and Swiftmend spells by [5 * Pts]%.
        /// </summary>
        public int ImprovedRejuvenation { get; set; }
        /// <summary>
        /// When you critically heal a target with Swiftmend, Regrowth, Nourish or Healing Touch spell you plant a Living Seed on the target for [10 * Pts]% of the amount healed. The Living Seed will bloom when the target is next attacked. Lasts 15 sec.
        /// </summary>
        public int LivingSeed { get; set; }
        /// <summary>
        /// When you periodically heal with your Rejuvenation or Lifebloom spells, you have a [1 * Pts]0% chance to instantly regenerate 2% of your total mana. This effect cannot occur more than once every 12 sec.
        /// 
        /// In addition, you also grant Replenishment when you cast or refresh Lifebloom.
        /// 
        /// Replenishment - Grants up to 10 party or raid members mana regeneration equal to 1% of their maximum mana per 10 sec. Lasts for 15 sec.
        /// </summary>
        public int Revitalize { get; set; }
        /// <summary>
        /// Nature's Swiftness - 3 min cooldown - Instant
        /// When activated, your next Nature spell with a base casting time less than 10 sec. becomes an instant cast spell.  If that spell is a healing spell, the amount healed will be increased by 50%.
        /// </summary>
        //public int NaturesSwiftness { get; set; }
        /// <summary>
        /// Reduces the mana cost of your Wrath spell by [50 * Pts]%, and when you deal damage with your Wrath spell you have a [6 * Pts]% chance to cause your next Starfire to be instant cast within 8 sec.
        /// </summary>
        public int FuryOfStormrage { get; set; }
        /// <summary>
        /// Increases the critical effect chance of your Regrowth spell by [20 * Pts]%.
        /// 
        /// In addition, when you have Rejuvenation active on three or more targets, the cast time of your Nourish spell is reduced by [10 * Pts]%.
        /// </summary>
        public int NaturesBounty { get; set; }
        /// <summary>
        /// Increases the direct healing done by your Healing Touch, Regrowth and Nourish spells by [5 * Pts]%, and grants those spells a [50 * Pts]% chance to refresh the duration of your Lifebloom on targets.
        /// </summary>
        public int EmpoweredTouch { get; set; }
        /// <summary>
        /// Whenever you heal with your Lifebloom spell, you have a [2 * Pts]% chance to cause Omen of Clarity.
        /// 
        /// In addition, the cooldown of your Tranquility is reduced by [5 / 2 * Pts] minutes.
        /// </summary>
        public int MalfurionsGift { get; set; }
        /// <summary>
        /// Your Swiftmend spell causes healing flora to sprout beneath the target, restoring health equal to [4 * Pts]% of the amount healed by your Swiftmend to the three most injured targets within 14 yards, every 1 sec for 7 sec.
        /// </summary>
        public int Efflorescence { get; set; }
        /// <summary>
        /// Wild Growth - 40 yd range - 27% of base mana
        /// 8 sec cooldown - Instant cast
        /// Heals up to 5 friendly party or raid members within 30 yards of the target for 2975 over 7 sec.  Prioritizes healing most injured party members.  The amount healed is applied quickly at first, and slows down as the Wild Growth reaches its full duration.
        /// </summary>
        public int WildGrowth { get; set; }
        /// <summary>
        /// Empowers your Remove Corruption spell to also remove a magic effect from a friendly target.
        /// </summary>
        public int NaturesCure { get; set; }
        /// <summary>
        /// Whenever you take an attack while at or below 50% health, you have a [50 * Pts]% chance to automatically cast Rejuvenation on yourself with no mana cost.
        /// </summary>
        public int NaturesWard { get; set; }
        /// <summary>
        /// Increases the healing done when your Lifebloom expires by [5 * Pts]%, and causes your Rejuvenation spell to also instantly heal for [5 * Pts]% of the total periodic effect.
        /// </summary>
        public int GiftOfTheEarthmother { get; set; }
        /// <summary>
        /// Reduces the global cooldown of your Rejuvenation by 0.5 sec.
        /// </summary>
        public int SwiftRejuvenation { get; set; }
        /// <summary>
        /// Shapeshift - Tree of Life - 6% of base mana
        /// 3 min cooldown - Instant cast
        /// Shapeshift into the tree of life, increasing healing done by 15% and increasing your armor by 120%. Also protects the caster from Polymorph effects. In addition, some of your spells are temporarily enhanced while shapeshifted. Lasts 25 sec.
        /// 
        /// Enhanced spells: Lifebloom, Wild Growth, Regrowth, Entangling Roots, Wrath
        /// </summary>
        public int TreeOfLife { get; set; }
        #endregion
    }

}
