using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public static class BaseStats
    {   // Only returns level 85 characters.
        public enum DruidForm { Caster, Bear, Cat, Moonkin }

        #region Cache Variables
        // Caching to reduce load.
        private static Stats _lastStats;
        private static int _lastLevel;
        private static CharacterClass _lastClass;
        private static CharacterRace _lastRace;
        private static DruidForm _lastForm;
        private static readonly object syncLock = new object();
        #endregion

        public static Stats GetBaseStats(Character character) { return GetBaseStats(character.Level, character.Class, character.Race, DruidForm.Caster); }
        public static Stats GetBaseStats(int level, CharacterClass characterClass, CharacterRace characterRace) { return GetBaseStats(level, characterClass, characterRace, DruidForm.Caster); }
        public static Stats GetBaseStats(int level, CharacterClass characterClass, CharacterRace characterRace, DruidForm characterForm)
        {   // Health, Mana and some other things are same for every race.
            lock (syncLock)
            {
                #region Cache
                if (level == _lastLevel
                    && characterClass == _lastClass
                    && characterRace == _lastRace
                    && characterForm == _lastForm)
                    return _lastStats.Clone();
                _lastLevel = level;
                _lastClass = characterClass;
                _lastRace = characterRace;
                _lastForm = characterForm;
                #endregion

                Stats S = new Stats();
                #region Race, not class benefit
                // Most Level 85 Race and Class Stats come from:
                // http://code.google.com/p/simulationcraft/source/browse/branches/cataclysm/engine/sc_rating.cpp?r=6207
                // When they were still at 80 as of Jan 01st, 2011

                // From SimCraft
                Stats race = new Stats();
                BaseCombatStatInfo baseStats = BaseCombatRating.TotalBaseStats(characterRace, characterClass, level);
                /*switch (characterRace)
                {
                    // Alliance
                    case CharacterRace.Human:               race.Strength = 20; race.Agility = 20; race.Stamina = 20; race.Intellect = 20; race.Spirit = 20; break;
                    case CharacterRace.Dwarf:               race.Strength = 25; race.Agility = 16; race.Stamina = 21; race.Intellect = 19; race.Spirit = 19; break;
                    case CharacterRace.NightElf:            race.Strength = 16; race.Agility = 24; race.Stamina = 20; race.Intellect = 20; race.Spirit = 20; break;
                    case CharacterRace.Gnome:               race.Strength = 15; race.Agility = 22; race.Stamina = 20; race.Intellect = 24; race.Spirit = 20; break;
                    case CharacterRace.Draenei:             race.Strength = 21; race.Agility = 17; race.Stamina = 20; race.Intellect = 20; race.Spirit = 22; break;
                    case CharacterRace.Worgen:              race.Strength = 23; race.Agility = 22; race.Stamina = 20; race.Intellect = 16; race.Spirit = 19; break;
                    case CharacterRace.PandarenAlliance:    race.Strength = 21; race.Agility = 19; race.Stamina = 22; race.Intellect = 21; race.Spirit = 22; break;
                    // Horde
                    case CharacterRace.Orc:                 race.Strength = 23; race.Agility = 17; race.Stamina = 21; race.Intellect = 17; race.Spirit = 22; break;
                    case CharacterRace.Undead:              race.Strength = 19; race.Agility = 18; race.Stamina = 20; race.Intellect = 18; race.Spirit = 25; break;
                    case CharacterRace.Tauren:              race.Strength = 25; race.Agility = 16; race.Stamina = 21; race.Intellect = 16; race.Spirit = 22; break;
                    case CharacterRace.Troll:               race.Strength = 21; race.Agility = 22; race.Stamina = 20; race.Intellect = 16; race.Spirit = 21; break;
                    case CharacterRace.BloodElf:            race.Strength = 17; race.Agility = 22; race.Stamina = 20; race.Intellect = 23; race.Spirit = 18; break;
                    case CharacterRace.Goblin:              race.Strength = 17; race.Agility = 22; race.Stamina = 20; race.Intellect = 23; race.Spirit = 20; break;
                    case CharacterRace.PandarenHorde:       race.Strength = 21; race.Agility = 19; race.Stamina = 22; race.Intellect = 21; race.Spirit = 22; break;
                    default: { break; }
                };*/
                // From Chardev (85)
                //Class           Str Agi Sta Int Spi
                //Druid            76  69  86 136 153
                //Shaman          111  60 128 119 136
                //Death Knight    171 101 154  16  44
                //Hunter           60 178 119  77  88
                //Mage             17  26  43 187 175
                //Paladin         144  77 136  86  97
                //Priest           26  34  51 188 183
                //Rogue           102 186  94  26  53
                //Warlock          43  51  76 161 166
                //Warrior         169 103 153  17  44
                #endregion

                #region Base Stats
                #region All Classes
                S.Miss  = 0.03f;
                S.Block = 0.00f;
                S.Parry = 0.00f;
                S.Mastery = (level >= 80 ? 8 : 0);
                S.Health = BaseCombatRating.BaseHP(level);
                S.Strength = baseStats.Strength;
                S.Intellect = baseStats.Intellect;
                S.Agility = baseStats.Agility;
                S.Stamina = baseStats.Stamina;
                S.Spirit = baseStats.Spirit;
                #endregion
                switch (characterClass)
                {
                    #region Death Knight
                    case CharacterClass.DeathKnight:
                        Stats dk = new Stats() {
                            Dodge = 0.05f, Parry = 0.05f, Block = 0.00f,
                            PhysicalCrit = 0.050000000745058f,
                            SpellCrit = 0.050000000745058f,
                            AttackPower = 595f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(dk);
                        break;
                    #endregion
                    #region Druid
                    case CharacterClass.Druid:
                        Stats druid = new Stats() {
                            Mana = BaseCombatRating.DruidBaseMana(level),
                            PhysicalCrit = 0.050000000745058f,
                            AttackPower = (level * 3),
                            SpellCrit = 0.050000000745058f,
                        };
                        druid.Mp5 = druid.Mana * 0.02f;
                        S.Accumulate(race);
                        S.Accumulate(druid);
                        switch (characterForm)
                        {
                            case DruidForm.Moonkin:
                            case DruidForm.Caster:
                                break;
                            case DruidForm.Bear:
                                S.BonusStaminaMultiplier = 0.2f;
                                S.Miss += 0.03f;
                                break;
                            case DruidForm.Cat:
                                break;
                            default:
                                break;
                        }
                        break;
                    #endregion
                    #region Hunter
                    case CharacterClass.Hunter:
                        Stats hun = new Stats() {
                            // Stats updated 8/19/2011 4.2 w/ Troll Hunter: Tsevon @ US-Dragonmaw w/ no spec.
                            Dodge = 0f, Parry = 0.05f, 
                            // This assumes ALL AP from full AP += AGI * 2
                            // So naked Hunter has 31 AP un accounted for.
                            // Naked troll, no gear, no spec, LW & Skinning.
                            PhysicalCrit = 0.050000000745058f, 
                            
                            //OpOv: MoP-attack power base has changed, calc has been moved into the Hunter module for easier management and modification
                            //AttackPower = 31f, RangedAttackPower = 31f, 

                            SpellCrit = 0.050000000745058f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(hun);
                        break;
                    #endregion
                    #region Mage
                    case CharacterClass.Mage:
                        Stats mag = new Stats() {
                            Mana = BaseCombatRating.MageBaseMana(level),
                            Dodge = 0.03758f, Parry = 0.05f,
                            PhysicalCrit = 0.050000000745058f,
                            SpellCrit = 0.050000000745058f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(mag);
                        break;
                    #endregion
                    #region Monk
                    case CharacterClass.Monk:
                        Stats monk = new Stats() {
                            Dodge = 0.0314f, Parry = 0.0801f,
                            PhysicalCrit = 0.050000000745058f,
                            AttackPower = 0f,
                            SpellCrit = 0.050000000745058f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(monk);
                        break;
                    #endregion
                    #region Paladin
                    case CharacterClass.Paladin:
                        Stats pal = new Stats() {
                            Mana = BaseCombatRating.PaladinBaseMana(level),
                            Dodge = 0.03f, Parry = 0.03f, Block = 0.03f,
                            PhysicalCrit = 0.050000000745058f,
                            AttackPower = 235f,
                            SpellCrit = 0.050000000745058f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(pal);
                        break;
                    #endregion
                    #region Priest
                    case CharacterClass.Priest:
                        Stats pri = new Stats() {
                            Mana = BaseCombatRating.PriestBaseMana(level),
                            Dodge = 0f,
                            Parry = 0f,
                            PhysicalCrit = 0.050000000745058f,
                            SpellCrit = 0.050000000745058f,
                        };
                        pri.Mp5 = pri.Mana * 0.05f;     // Always 5% of base mana in regen.
                        S.Accumulate(race);
                        S.Accumulate(pri);
                        break;
                    #endregion
                    #region Rogue
                    case CharacterClass.Rogue:
                        Stats rog = new Stats() {
                            Dodge = 0.03758f, Parry = 0.05f,
                            PhysicalCrit = 0.050000000745058f,
                            SpellCrit = 0.050000000745058f,
                            AttackPower = 613f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(rog);
                        break;
                    #endregion
                    #region Shaman
                    case CharacterClass.Shaman:
                        Stats sha = new Stats() {
                            Mana = BaseCombatRating.ShamanBaseMana(level),
                            Dodge = 0.03f, Parry = 0.00f, Block = 0.03f,
                            PhysicalCrit = 0.050000000745058f,
                            AttackPower = 140f,
                            SpellCrit = 0.050000000745058f,
                            SpellPower = -10,
                        };
                        S.Accumulate(race);
                        S.Accumulate(sha);
                        break;
                    #endregion
                    #region Warlock
                    case CharacterClass.Warlock:
                        Stats warlock = new Stats() {
                            Mana = BaseCombatRating.WarlockBaseMana(level),
                            Dodge = 0f, Parry = 0f, Block = 0f,
                            PhysicalCrit = 0.050000000745058f,
                            SpellCrit = 0.050000000745058f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(warlock);
                        break;
                    #endregion
                    #region Warrior
                    case CharacterClass.Warrior:
                        Stats war = new Stats() {
                            Dodge = 0.03f, Parry = 0.03f, Block = 0.03f,
                            PhysicalCrit = 0.050000000745058f,
                            SpellCrit = 0.050000000745058f,
                            AttackPower = 613f,
                        };
                        S.Accumulate(race);
                        S.Accumulate(war);
                        break;
                    #endregion
                    #region No Class
                    default:
                        break;
                    #endregion
                }
                #endregion

                #region Racials
                if (characterRace == CharacterRace.Gnome)
                {
                    // Arcane Resistance - 
                    S.ArcaneDamageReductionMultiplier = 0.01f;
                    // Expansive Mind - Increases maximum Mana, Rage, Energy, Runic Power by 5%
                    S.BonusManaMultiplier = 0.05f;
                    // Nimble Fingers - Increases haste by 1%
                    S.PhysicalHaste += 0.01f;
                    S.SpellHaste += 0.01f;
                }
                else if (characterRace == CharacterRace.Human)
                {
                    // The human spirit - Increases two secondary stats of your choice by x%
                    //S.HighestSecondaryStat = 0.01f;
                    // Every Man for Himself - PvP trinket
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { PVPTrinket = 1 }, 0f, 120f));
                }
                else if (characterRace == CharacterRace.NightElf)
                {
                    // Nature Resistance - 
                    S.NatureDamageReductionMultiplier = 0.01f;
                    // Quickness - Increases dodge and movement speed by 2%
                    S.Dodge += 0.02f;
                    S.MovementSpeed += 0.02f;
                    // Touch of Elune - Increases haste by 1% at night, increases critical strike chance by 1% during the day
                    S.PhysicalHaste += 0.01f;
                    S.SpellHaste += 0.01f;
                    S.PhysicalCrit += 0.01f;
                    S.SpellCrit += 0.01f;
                }
                else if (characterRace == CharacterRace.Dwarf)
                {
                    // Frost Resistance - 
                    S.FrostDamageReductionMultiplier = 0.01f;
                    // Might of the Mountain - Increases critical strike bonus damage and healing by 2%
                    S.BonusCritDamageMultiplier = 0.02f;
                    S.BonusCritHealMultiplier = 0.02f;
                    // Stoneform - Removes magic, curse, poison, disease, bleed effects, and reduces damage taken by 10% for 8s. Not usable in CC.
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { DamageTakenReductionMultiplier = 0.1f }, 8, 120));
                    // TODO: Add debuff removal.  Doesn't work on all bosses so not sure if we want to.
                }
                else if (characterRace == CharacterRace.Draenei)
                {
                    // Shadow Resistance - 
                    S.ShadowDamageReductionMultiplier = 0.01f;
                    // Heroic Presence - Increases your primary stat by x%
                    //S.HighestStat = 0.01f;
                    // Gift of the Naaru
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HealthRestoreFromMaxHealth = 0.2f / 5f }, 5f, 180f));
                }
                else if (characterRace == CharacterRace.Worgen)
                {
                    // Aberration - 
                    S.NatureDamageReductionMultiplier = 0.01f;
                    S.ShadowDamageReductionMultiplier = 0.01f;
                    // Darkflight - 
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = 0.40f }, 10f, 120f));
                    // Viciousness - 
                    S.PhysicalCrit += 0.01f;
                    S.SpellCrit += 0.01f;
                }
                else if (characterRace == CharacterRace.Tauren)
                {
                    // Nature Resistance - 
                    S.NatureDamageReductionMultiplier = 0.01f;
                    // Brawn - Increases critical strike bonus damage and healing by 2%
                    S.BonusCritDamageMultiplier = 0.02f;
                    S.BonusCritHealMultiplier = 0.02f;
                    // Endurance - Increases stamina by x%
                    //S.Stamina = 0.01f;
                    // War Stomp
                }
                else if (characterRace == CharacterRace.Troll)
                {
                    // Da Voodoo Shuffle - 
                    S.SnareRootDurReduc = 0.15f;
                    // Berserking
                    if (characterClass == CharacterClass.DeathKnight || characterClass == CharacterClass.Warrior || characterClass == CharacterClass.Rogue)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { PhysicalHaste = 0.15f }, 10f, 180f));
                    else
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellHaste = 0.15f, PhysicalHaste = 0.2f }, 10f, 180f));
                    // Regeneration - 
                }
                else if (characterRace == CharacterRace.Undead)
                {
                    // Shadow Resistance - 
                    S.ShadowDamageReductionMultiplier = 0.01f;
                    // Will of the Forsaken - 
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FearDurReduc = 1f }, .1f, 180f));
                    // Touch of the Grave - 
                    float touchOfTheGrave = BaseCombatRating.TouchOfTheGraveScaling(level) * BaseCombatRating.TouchOfTheGraveMultiplier;
                    S.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCast, new Stats() { ShadowDamage = touchOfTheGrave, HealthRestore = touchOfTheGrave }, 0, 0, 0.20f ));
                    S.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalAttack, new Stats() { ShadowDamage = touchOfTheGrave, HealthRestore = touchOfTheGrave }, 0, 0, 0.20f));
                    // Cannibalize - 
                }
                else if (characterRace == CharacterRace.Orc)
                {
                    // Hardiness - 
                    S.StunDurReduc = 0.10f;
                    // Command - 
                    S.BonusPetDamageMultiplier = 0.02f;
                    // Blood Fury - 
                    if (characterClass == CharacterClass.Shaman)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { AttackPower = 65 + (level * 13), SpellPower = 75 + (level * 6) }, 15f, 120f));
                    else if (characterClass == CharacterClass.Warlock || characterClass == CharacterClass.Mage)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { SpellPower = 75 + (level * 6) }, 15f, 120f));
                    else
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { AttackPower = 65 + (level * 13) }, 15f, 120f));
                }
                else if (characterRace == CharacterRace.BloodElf)
                {
                    // Arcane Resistance - 
                    S.ArcaneDamageReductionMultiplier = 0.01f;
                    // Arcane Acuity - Increases Crit Chance by 1%
                    S.PhysicalCrit += 0.01f;
                    S.SpellCrit += 0.01f;
                    // Arcane Torrent - 
                    if (characterClass == CharacterClass.DeathKnight)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaorEquivRestore = .20f }, 0f, 120f));
                    else if (characterClass == CharacterClass.Rogue || characterClass == CharacterClass.Hunter)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaorEquivRestore = .15f }, 0f, 120f));
                    else if (characterClass == CharacterClass.Warrior)
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusRageGen = 15f }, 0f, 120f));
                    else
                        S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { ManaorEquivRestore = .06f }, 0f, 120f));
                }
                else if (characterRace == CharacterRace.Goblin)
                {
                    // Time is Money
                    S.PhysicalHaste += 0.01f;
                    S.SpellHaste += 0.01f;
                    // Rocket Barrage - 
                    S.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 1f + (level * 2) }, 0f, 120f));
                    // Rocket Jump - 
                }
                else if (characterRace == CharacterRace.PandarenAlliance || characterRace == CharacterRace.PandarenHorde)
                {
                    // Epicurean - Your love of food allows you to receive double the stats from Well Fed effects.

                    // Quaking Palm - Strikes the target with lightning speed, incapacitating them for 4 sec, and turns off your attack. 2min CD.

                }
                #endregion

                _lastStats = S.Clone();
                return S;
            }
        }

        [Obsolete("This method is deprecated as all expertise racials have been removed from the game")]
        public static float GetRacialExpertise(Character character, ItemSlot weaponSlot)
        {
            return 0;
        }
    }
}
