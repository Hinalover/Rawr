using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using System.Xml.Serialization;

namespace Rawr
{
    //[GenerateSerializer]
    public class EnchantList : List<Enchant> { }

    /// <summary>An object representing an Enchantment to be placed on a slot on a character.</summary>
    public class Enchant
    {
        #region Variables
        /// <summary>
        /// The ID of the enchant. This is determined by viewing the enchant spell on Wowhead, and
        /// noting the Enchant Item Permenant ID in the spell effect data.
        /// 
        /// EXAMPLE:
        /// Enchant Gloves - Superior Agility. This enchant is applied by spell 25080, which you can find
        /// by searching on Wowhead (http://www.wowhead.com/?spell=25080). In the spell Effect section, it
        /// says "Enchant Item Permanent (2564)". The Enchant ID is 2564.
        /// </summary>
        public int Id;
        /// <summary>The name of the enchant.</summary>
        public string Name;
        /// <summary>A shortened version of the enchant's Name.</summary>
        public string ShortName
        {
            get
            {
                string shortName = Name.Replace("Arcanum of the ", "").Replace("Arcanum of ", "").Replace("Inscription of the ", "")
                    .Replace("Inscription of ", "").Replace("Greater", "Great").Replace("Exceptional", "Excep").Replace("Defense", "Def")
                    .Replace("Armor Kit", "").Replace("Arcanum of ", "").Replace(" Leg Armor", "").Replace(" Scope", "")
                    .Replace(" Spellthread", "").Replace("Lining - ", "").Replace("Strength", "Str").Replace("Agility", "Agi")
                    .Replace("Stamina", "Sta").Replace("Intellect", "Int").Replace("Spirit", "Spr").Replace("Heavy", "Hvy")
                    .Replace("Jormungar Leg Reinforcements", "Jorm Reinf").Replace(" Leg Reinforcements", "")
                    .Replace("Powerful", "Power").Replace("Swiftness", "Swift").Replace("Knothide", "Knot")
                    .Replace("Savage", "Sav").Replace("Mighty Armor", "Mighty Arm").Replace("Shadow Armor", "Shadow Arm")
                    .Replace("Attack Power", "AP").Replace("Rune of the ", "").Replace(" Gargoyle", "")
                    .Replace("speed Accelerators", "").Replace(" Mysteries", "").Replace(" Embroidery", "")
                    .Replace("Mana Restoration", "Mp5").Replace("Restore Mana", "Mp5").Replace("Vengeance", "Veng.")
                    .Replace("Reticulated Armor ", "").Replace("Titanium Weapon Chain", "Titnm.W.Chn");
                return shortName.Substring(0, Math.Min(shortName.Length, 12));
            }
        }
        /// <summary>A REALLY shortened version of the enchant's Name. For those names that are still too long to fit in the space on screen</summary>
        public string ReallyShortName
        {
            get
            {
                if (Id == 0) return "None";
                string[] split = Name.Split(' ');
                if (split.Length > 1)
                {
                    string ret = "";
                    foreach (string s in split) { ret += s.Substring(0, 1); }
                    return ret;
                }
                return Name.Substring(0, 5);
            }
        }
        /// <summary>
        /// The slot that the enchant is applied to. If the enchant is available on multiple slots,
        /// define the enchant multiple times, once for each slot.
        /// <para>IMPORTANT: Shield enchants should be defined as Off-Hand</para>
        /// </summary>
        public ItemSlot Slot = ItemSlot.Head;
        /// <summary>Whether this enchant applies to Shields only.</summary>
        public bool ShieldsOnly = false;
        /// <summary>The stats that the enchant gives the character.</summary>
        public Stats Stats = new Stats();
        private static EnchantList _allEnchants;
        /// <summary>A List<Enchant> containing all known enchants relevant to all models.</summary>
        public static List<Enchant> AllEnchants { get { return _allEnchants; } }
        /// <summary>If set, will attempt to pull this icon from wow.com or wowhead.com for the comparison list</summary>
        public string IconSource { get; set; }
        #endregion

        public Enchant() { }
        /// <summary>Creates a new Enchant, representing an enchant to a single slot.</summary>
        /// <example>new Enchant(2564, "Superior Agility", ItemSlot.Hands, new Stats() { Agility = 15 })</example>
        /// <param name="id">The Enchant ID for the enchant. See the Id property for details of how to find this.</param>
        /// <param name="name">The name of the enchant.</param>
        /// <param name="slot">The slot that this instance of the enchant applies to. (Create multiple Enchant
        /// objects for enchants which may be applied to multiple slots)</param>
        /// <param name="icon">The Icon name (eg- "spell_fire_masterofelements"). Defaults to null for no Icon</param>
        /// <param name="stats">The stats that the enchant gives the character.</param>
        public Enchant(int id, string name, ItemSlot slot, Stats stats, string icon, bool shieldsonly = false)
        {
            Id = id;
            Name = name;
            Slot = slot;
            Stats = stats;
            IconSource = icon;
            ShieldsOnly = shieldsonly;
        }

        public override string ToString()
        {
            string summary = Name + ": ";
            summary += Stats.ToString();
            summary = summary.TrimEnd(',', ' ', ':');
            return summary;
        }

        public override bool Equals(object obj)
        {
            Enchant temp = obj as Enchant;
            if (temp != null)
            {
                return Name.Equals(temp.Name) && Id == temp.Id && Slot == temp.Slot && Stats.Equals(temp.Stats);
            }
            else { return false; }
        }

        public override int GetHashCode() { return (Id << 5) | (int)Slot; }

        public bool FitsInSlot(ItemSlot slot)
        {
            return (Slot == slot ||
                (Slot == ItemSlot.OneHand && (slot == ItemSlot.OffHand || slot == ItemSlot.MainHand || slot == ItemSlot.TwoHand || slot == ItemSlot.Ranged)) ||
                (Slot == ItemSlot.TwoHand && (slot == ItemSlot.MainHand)) ||
                (Slot == ItemSlot.Ranged && (slot == ItemSlot.MainHand)));
        }

        public bool FitsInSlot(ItemSlot slot, Character character) { return Calculations.EnchantFitsInSlot(this, character, slot); }

        public static Enchant FindEnchant(int id, ItemSlot slot, Character character)
        {
            //&UT&
            // Chance for null
            if (AllEnchants == null) return null;

            return AllEnchants.Find(new Predicate<Enchant>(delegate(Enchant enchant)
            {
                return (enchant.Id == id) && (enchant.FitsInSlot(slot, character) ||
                  (enchant.Slot == ItemSlot.TwoHand && slot == ItemSlot.OneHand));
            })) ?? AllEnchants[0];
        }

        private static List<Enchant> _cachedEnchantingOptions = null;
        private static ItemSlot _cachedEnchantingOptions_slot = ItemSlot.None;
        private static List<string> _cachedEnchantingOptions_avail = null;
        public static List<Enchant> GetEnchantingOptions(Item baseItem, Character character)
        {
            // Try to use caching to save us time
            List<Enchant> options;
            if (_cachedEnchantingOptions_slot == baseItem.Slot
                && _cachedEnchantingOptions_avail == character.AvailableItems)
            {
                options = _cachedEnchantingOptions;
            }
            else
            {
                options = FindEnchants(baseItem.Slot, character);
                // Look for Enchants marked available (automatically includes "No Enchant")
                if (options.Count > 1) { options = options.FindAll(e => (character.GetItemAvailability(e) != ItemAvailability.NotAvailable || e.Id == 0)); }
                _cachedEnchantingOptions_slot = baseItem.Slot;
                _cachedEnchantingOptions_avail = new List<string>(character.AvailableItems.ToArray());
                _cachedEnchantingOptions = options;
            }
            //
            return options;
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character character)
        {
            return FindEnchants(slot, character, Calculations.Instance);
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character character, CalculationsBase model)
        {
            //List<ItemSlot> validSlots = new List<ItemSlot>();
            //if (slot != ItemSlot.MainHand)
            //    validSlots.Add(slot);
            //if (slot == ItemSlot.OffHand || slot == ItemSlot.MainHand || slot == ItemSlot.TwoHand)
            //    validSlots.Add(ItemSlot.OneHand);
            //if (slot == ItemSlot.MainHand)
            //    validSlots.Add(ItemSlot.TwoHand);
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    return enchant.Slot == ItemSlot.None ||
                        model.IsEnchantRelevant(enchant, character) &&
                        (slot == ItemSlot.None || enchant.FitsInSlot(slot, character));
                }
            ));
        }

        public static List<Enchant> FindAllEnchants(ItemSlot slot, Character character)
        {
            //List<ItemSlot> validSlots = new List<ItemSlot>();
            //if (slot != ItemSlot.MainHand)
            //    validSlots.Add(slot);
            //if (slot == ItemSlot.OffHand || slot == ItemSlot.MainHand || slot == ItemSlot.TwoHand)
            //    validSlots.Add(ItemSlot.OneHand);
            //if (slot == ItemSlot.MainHand)
            //    validSlots.Add(ItemSlot.TwoHand);
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    return enchant.Slot == ItemSlot.None ||
                        (slot == ItemSlot.None || enchant.FitsInSlot(slot, character));
                }
            ));
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character character, List<string> availableIds)
        {
            return FindEnchants(slot, character, availableIds, Calculations.Instance);
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character character, List<string> availableIds, CalculationsBase model)
        {
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    return enchant.Id == 0 ||
                        ((enchant.Slot == ItemSlot.None ||
                        model.IsEnchantRelevant(enchant, character) && (slot == ItemSlot.None || enchant.FitsInSlot(slot, character)))
                        && availableIds.Contains((-1 * (enchant.Id + ((int)AvailableItemIDModifiers.Enchants * (int)enchant.Slot))).ToString(System.Globalization.CultureInfo.InvariantCulture)));
                }
            ));
        }

        public static List<Enchant> FindEnchants(ItemSlot slot, Character[] characters, List<string> availableIds, CalculationsBase[] models)
        {
            return AllEnchants.FindAll(new Predicate<Enchant>(
                delegate(Enchant enchant)
                {
                    bool isRelevant = false;
                    for (int i = 0; i < models.Length; i++)
                    {
                        if (models[i].IsEnchantRelevant(enchant, characters[i]) && (enchant.FitsInSlot(slot, characters[i]) || slot == ItemSlot.None))
                        {
                            isRelevant = true;
                            break;
                        }
                    }
                    return ((isRelevant || enchant.Slot == ItemSlot.None)
                        && availableIds.Contains((-1 * (enchant.Id + ((int)AvailableItemIDModifiers.Enchants * (int)enchant.Slot))).ToString(System.Globalization.CultureInfo.InvariantCulture)))
                        || enchant.Id == 0;
                }
            ));
        }

        /*public static void Save(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EnchantList));
            serializer.Serialize(writer, _allEnchants);
            writer.Close();
        }*/

        /*public static void Load(TextReader reader)
        {
            _allEnchants = null;
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(EnchantList));
                _allEnchants = (EnchantList)serializer.Deserialize(reader);
                reader.Close();
            } catch {
            } finally {
                reader.Close();
                _allEnchants = _allEnchants ?? new EnchantList();
            }
            List<Enchant> defaultEnchants = GetDefaultEnchants();
            for (int defaultEnchantIndex = 0; defaultEnchantIndex < defaultEnchants.Count; defaultEnchantIndex++)
            {
                bool found = false;
                for (int allEnchantIndex = 0; allEnchantIndex < _allEnchants.Count; allEnchantIndex++)
                {
                    if (defaultEnchants[defaultEnchantIndex].Id == _allEnchants[allEnchantIndex].Id &&
                        defaultEnchants[defaultEnchantIndex].Slot == _allEnchants[allEnchantIndex].Slot &&
                        defaultEnchants[defaultEnchantIndex].Name == _allEnchants[allEnchantIndex].Name
                        )
                    {
                        if (defaultEnchants[defaultEnchantIndex].Stats != _allEnchants[allEnchantIndex].Stats)
                        {
                            if (defaultEnchants[defaultEnchantIndex].Stats == null)
                            {
                                _allEnchants.RemoveAt(allEnchantIndex);
                            } else {
                                _allEnchants[allEnchantIndex].Stats = defaultEnchants[defaultEnchantIndex].Stats;
                            }
                        }
                        found = true;
                        break;
                    }
                }
                if (!found && defaultEnchants[defaultEnchantIndex].Stats != null)
                {
                    _allEnchants.Add(defaultEnchants[defaultEnchantIndex]);
                }
            }
        }*/
        public static void LoadDefaultEnchants()
        {
            _allEnchants = new EnchantList();
            _allEnchants.AddRange(GetDefaultEnchants());
        }

        private static List<Enchant> GetDefaultEnchants()
        {
            List<Enchant> defaultEnchants = new List<Enchant>();
            Stats enchantTemp = new Stats();
            // The All Important No Enchant, works in all slots
            defaultEnchants.Add(new Enchant(0, "No Enchant", ItemSlot.None, new Stats(), ""));
            #region Head
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4207, "Arcanum of Hyjal", ItemSlot.Head, new Stats() { Intellect = 60, CritRating = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4208, "Arcanum of the Dragonmaw", ItemSlot.Head, new Stats() { Strength = 60, MasteryRating = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4206, "Arcanum of the Earthen Ring", ItemSlot.Head, new Stats() { Stamina = 90, DodgeRating = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4209, "Arcanum of the Ramkahen", ItemSlot.Head, new Stats() { Agility = 60, HasteRating = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4208, "Arcanum of the Wildhammer", ItemSlot.Head, new Stats() { Strength = 60, MasteryRating = 35 }, "spell_fire_masterofelements"));
            // Patch 4.0.6+ added new Agility, Strength, and Intellect PvP Helm Arcanums
            defaultEnchants.Add(new Enchant(4246, "Arcanum of Vicious Agility", ItemSlot.Head, new Stats() { Agility = 60, PvPResilience = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4247, "Arcanum of Vicious Strength", ItemSlot.Head, new Stats() { Strength = 60, PvPResilience = 35 }, "spell_fire_masterofelements"));
            defaultEnchants.Add(new Enchant(4245, "Arcanum of Vicious Intellect", ItemSlot.Head, new Stats() { Intellect = 60, PvPResilience = 35 }, "spell_fire_masterofelements"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3819, "Arcanum of Blissful Mending", ItemSlot.Head, new Stats() { Intellect = 26, Spirit = 20 }, "ability_warrior_shieldmastery"));
            defaultEnchants.Add(new Enchant(3820, "Arcanum of Burning Mysteries", ItemSlot.Head, new Stats() { Intellect = 30, CritRating = 20 }, "spell_fire_masterofelements")); // Will probably be vamped again
            defaultEnchants.Add(new Enchant(3842, "Arcanum of the Savage Gladiator", ItemSlot.Head, new Stats() { Stamina = 30, PvPResilience = 25 }, "ability_warrior_shieldmastery"));
            defaultEnchants.Add(new Enchant(3818, "Arcanum of the Stalwart Protector", ItemSlot.Head, new Stats() { Stamina = 37, DodgeRating = 20 }, "ability_warrior_swordandboard"));
            defaultEnchants.Add(new Enchant(3817, "Arcanum of Torment", ItemSlot.Head, new Stats() { AttackPower = 50, CritRating = 20 }, "ability_warrior_rampage")); // Will probably be vamped again
            defaultEnchants.Add(new Enchant(3797, "Arcanum of Dominance", ItemSlot.Head, new Stats() { SpellPower = 29, PvPResilience = 20 }, "spell_arcane_arcaneresilience")); // Will probably be vamped again
            //defaultEnchants.Add(new Enchant(3815, "Arcanum of the Eclipsed Moon", ItemSlot.Head, new Stats() { ArcaneResistance = 25, Stamina = 30 }, "ability_druid_eclipse"));
            //defaultEnchants.Add(new Enchant(3816, "Arcanum of the Flame's Soul", ItemSlot.Head, new Stats() { FireResistance = 25, Stamina = 30 }, "spell_fire_burnout"));
            //defaultEnchants.Add(new Enchant(3814, "Arcanum of the Fleeing Shadow", ItemSlot.Head, new Stats() { ShadowResistance = 25, Stamina = 30 }, "ability_paladin_gaurdedbythelight"));
            //defaultEnchants.Add(new Enchant(3812, "Arcanum of the Frosty Soul", ItemSlot.Head, new Stats() { FrostResistance = 25, Stamina = 30 }, "spell_frost_frozencore"));
            //defaultEnchants.Add(new Enchant(3813, "Arcanum of Toxic Warding", ItemSlot.Head, new Stats() { NatureResistance = 25, Stamina = 30 }, "trade_brewpoison"));
            defaultEnchants.Add(new Enchant(3795, "Arcanum of Triumph", ItemSlot.Head, new Stats() { AttackPower = 50, PvPResilience = 20 }, "ability_warrior_shieldmastery")); // Will probably be vamped again
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(3001, "Arcanum of Renewal", ItemSlot.Head, new Stats() { Intellect = 16, Spirit = 18 }, "spell_holy_healingaura"));
            defaultEnchants.Add(new Enchant(2999, "Arcanum of the Defender", ItemSlot.Head, new Stats() { ParryRating = 16, DodgeRating = 17 }, "ability_warrior_victoryrush"));
            defaultEnchants.Add(new Enchant(3004, "Arcanum of the Gladiator", ItemSlot.Head, new Stats() { Stamina = 18, PvPResilience = 20 }, "inv_misc_statue_04"));
            defaultEnchants.Add(new Enchant(3096, "Arcanum of the Outcast", ItemSlot.Head, new Stats() { Strength = 17, Intellect = 16 }, "ability_rogue_masterofsubtlety"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Shoulders
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4806, "Greater Crane Wing Inscription", ItemSlot.Shoulders, new Stats() { Intellect = 12, CritRating = 6 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4805, "Greater Ox Horn Inscription", ItemSlot.Shoulders, new Stats() { Stamina = 19, DodgeRating = 6 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4803, "Greater Tiger Fang Inscription", ItemSlot.Shoulders, new Stats() { Strength = 13, CritRating = 6 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4804, "Greater Tiger Claw Inscription", ItemSlot.Shoulders, new Stats() { Agility = 13, CritRating = 6 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4909, "Crane Wing Inscription", ItemSlot.Shoulders, new Stats() { Intellect = 8, CritRating = 5 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4910, "Ox Horn Inscription", ItemSlot.Shoulders, new Stats() { Stamina = 11, DodgeRating = 5 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4907, "Tiger Fang Inscription", ItemSlot.Shoulders, new Stats() { Strength = 8, CritRating = 5 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4908, "Tiger Claw Inscription", ItemSlot.Shoulders, new Stats() { Agility = 8, CritRating = 5 }, "inv_misc_mastersinscription"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4200, "Greater Inscription of Charged Lodestone", ItemSlot.Shoulders, new Stats() { Intellect = 10, HasteRating = 5 }, "inv_misc_gem_bloodstone_02"));
            defaultEnchants.Add(new Enchant(4202, "Greater Inscription of Jagged Stone", ItemSlot.Shoulders, new Stats() { Strength = 10, CritRating = 5 }, "inv_misc_gem_emeraldrough_02"));
            defaultEnchants.Add(new Enchant(4204, "Greater Inscription of Shattered Crystal", ItemSlot.Shoulders, new Stats() { Agility = 10, MasteryRating = 5 }, "inv_misc_gem_goldendraenite_01"));
            defaultEnchants.Add(new Enchant(4198, "Greater Inscription of Unbreakable Quartz", ItemSlot.Shoulders, new Stats() { Stamina = 15, DodgeRating = 5 }, "inv_misc_gem_crystal_01"));
            defaultEnchants.Add(new Enchant(4199, "Lesser Inscription of Charged Lodestone", ItemSlot.Shoulders, new Stats() { Intellect = 6, HasteRating = 4 }, "inv_misc_gem_bloodstone_02"));
            defaultEnchants.Add(new Enchant(4201, "Lesser Inscription of Jagged Stone", ItemSlot.Shoulders, new Stats() { Strength = 6, CritRating = 4 }, "inv_misc_gem_emeraldrough_02"));
            defaultEnchants.Add(new Enchant(4205, "Lesser Inscription of Shattered Crystal", ItemSlot.Shoulders, new Stats() { Agility = 6, MasteryRating = 4 }, "inv_misc_gem_goldendraenite_01"));
            defaultEnchants.Add(new Enchant(4197, "Lesser Inscription of Unbreakable Quartz", ItemSlot.Shoulders, new Stats() { Stamina = 9, DodgeRating = 4 }, "inv_misc_gem_crystal_01"));
            // Patch 4.0.6+ added new Agility, Strength, and Intellect PvP Shoulder enchants
            defaultEnchants.Add(new Enchant(4250, "Greater Inscription of Vicious Agility", ItemSlot.Shoulders, new Stats() { Agility = 10, PvPResilience = 5 }, "inv_misc_gem_goldendraenite_01"));
            defaultEnchants.Add(new Enchant(4249, "Greater Inscription of Vicious Strength", ItemSlot.Shoulders, new Stats() { Strength = 10, PvPResilience = 5 }, "inv_misc_gem_emeraldrough_02"));
            defaultEnchants.Add(new Enchant(4248, "Greater Inscription of Vicious Intellect", ItemSlot.Shoulders, new Stats() { Intellect = 10, PvPResilience = 5 }, "inv_misc_gem_bloodstone_02"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3808, "Greater Inscription of the Axe", ItemSlot.Shoulders, new Stats() { AttackPower = 10, CritRating = 8 }, "inv_axe_85"));
            defaultEnchants.Add(new Enchant(3809, "Greater Inscription of the Crag", ItemSlot.Shoulders, new Stats() { Intellect = 10, Spirit = 8 }, "spell_arcane_teleportorgrimmar"));
            defaultEnchants.Add(new Enchant(3811, "Greater Inscription of the Pinnacle", ItemSlot.Shoulders, new Stats() { DodgeRating = 10, Stamina = 11 }, "spell_holy_divinepurpose"));
            defaultEnchants.Add(new Enchant(3810, "Greater Inscription of the Storm", ItemSlot.Shoulders, new Stats() { SpellPower = 12, CritRating = 8 }, "spell_nature_lightningoverload"));
            defaultEnchants.Add(new Enchant(3875, "Lesser Inscription of the Axe", ItemSlot.Shoulders, new Stats() { AttackPower = 15, CritRating = 5 }, "inv_axe_82"));
            defaultEnchants.Add(new Enchant(3807, "Lesser Inscription of the Crag", ItemSlot.Shoulders, new Stats() { Intellect = 8, Spirit = 5 }, "spell_nature_farsight"));
            defaultEnchants.Add(new Enchant(3876, "Lesser Inscription of the Pinnacle", ItemSlot.Shoulders, new Stats() { DodgeRating = 8, ParryRating = 5 }, "spell_holy_divinepurpose"));
            defaultEnchants.Add(new Enchant(3806, "Lesser Inscription of the Storm", ItemSlot.Shoulders, new Stats() { SpellPower = 9, CritRating = 5 }, "spell_nature_lightning"));
            defaultEnchants.Add(new Enchant(3852, "Greater Inscription of the Gladiator", ItemSlot.Shoulders, new Stats() { Stamina = 15, PvPResilience = 8 }, "inv_shoulder_61"));
            defaultEnchants.Add(new Enchant(3793, "Inscription of Triumph", ItemSlot.Shoulders, new Stats() { AttackPower = 20, PvPResilience = 8 }, "spell_holy_weaponmastery"));
            defaultEnchants.Add(new Enchant(3794, "Inscription of Dominance", ItemSlot.Shoulders, new Stats() { SpellPower = 12, PvPResilience = 8 }, "spell_holy_powerinfusion"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2982, "Greater Inscription of Discipline", ItemSlot.Shoulders, new Stats() { SpellPower = 18, CritRating = 10 }, "spell_holy_sealofwisdom"));
            defaultEnchants.Add(new Enchant(2980, "Greater Inscription of Faith", ItemSlot.Shoulders, new Stats() { Intellect = 15, Spirit = 10 }, "spell_holy_greaterblessingofsalvation"));
            defaultEnchants.Add(new Enchant(2997, "Greater Inscription of the Blade", ItemSlot.Shoulders, new Stats() { AttackPower = 20, CritRating = 15 }, "spell_holy_weaponmastery"));
            defaultEnchants.Add(new Enchant(2991, "Greater Inscription of the Knight", ItemSlot.Shoulders, new Stats() { ParryRating = 15, DodgeRating = 10 }, "spell_holy_championsgrace"));
            defaultEnchants.Add(new Enchant(2993, "Greater Inscription of the Oracle", ItemSlot.Shoulders, new Stats() { Intellect = 10, Spirit = 16 }, "spell_holy_powerinfusion"));
            defaultEnchants.Add(new Enchant(2995, "Greater Inscription of the Orb", ItemSlot.Shoulders, new Stats() { SpellPower = 12, CritRating = 15 }, "inv_misc_orb_03"));
            defaultEnchants.Add(new Enchant(2986, "Greater Inscription of Vengeance", ItemSlot.Shoulders, new Stats() { AttackPower = 30, CritRating = 10 }, "spell_holy_greaterblessingofkings"));
            defaultEnchants.Add(new Enchant(2978, "Greater Inscription of Warding", ItemSlot.Shoulders, new Stats() { DodgeRating = 15, Stamina = 15 }, "spell_holy_blessingofprotection"));
            //defaultEnchants.Add(new Enchant(2998, "Inscription of Endurance", ItemSlot.Shoulders, new Stats() { NatureResistance = 7, ArcaneResistance = 7, FireResistance = 7, FrostResistance = 7, ShadowResistance = 7 }, "Inscription of Endurance"));
            defaultEnchants.Add(new Enchant(2990, "Inscription of the Knight", ItemSlot.Shoulders, new Stats() { DodgeRating = 13 }, "spell_holy_championsbond"));
            defaultEnchants.Add(new Enchant(2977, "Inscription of Warding", ItemSlot.Shoulders, new Stats() { DodgeRating = 13 }, "spell_holy_greaterblessingofsanctuary"));
            defaultEnchants.Add(new Enchant(2996, "Inscription of the Blade", ItemSlot.Shoulders, new Stats() { CritRating = 13 }, "ability_dualwield"));
            defaultEnchants.Add(new Enchant(2983, "Inscription of Vengeance", ItemSlot.Shoulders, new Stats() { AttackPower = 26 }, "spell_holy_fistofjustice"));
            defaultEnchants.Add(new Enchant(2981, "Inscription of Discipline", ItemSlot.Shoulders, new Stats() { SpellPower = 15 }, "spell_holy_sealofwisdom"));
            defaultEnchants.Add(new Enchant(2994, "Inscription of the Orb", ItemSlot.Shoulders, new Stats() { CritRating = 13 }, "inv_misc_orb_04"));
            defaultEnchants.Add(new Enchant(2979, "Inscription of Faith", ItemSlot.Shoulders, new Stats() { SpellPower = 15 }, "spell_holy_sealofsalvation"));
            defaultEnchants.Add(new Enchant(2992, "Inscription of the Oracle", ItemSlot.Shoulders, new Stats() { Spirit = 12 }, "spell_holy_spiritualguidence"));
            #endregion
            #region Level 60 (Vanilla)
            // defaultEnchants.Add(new Enchant(2716, "Fortitude of the Scourge", ItemSlot.Shoulders, new Stats() { Stamina = 16, BonusArmor = 100 }, "spell_shadow_antishadow")); // No longer in the game
            // defaultEnchants.Add(new Enchant(2717, "Might of the Scourge", ItemSlot.Shoulders, new Stats() { AttackPower = 26, CritRating = 14 }, "spell_shadow_deathpact")); // No longer in the game
            // defaultEnchants.Add(new Enchant(2721, "Power of the Scourge", ItemSlot.Shoulders, new Stats() { CritRating = 14, SpellPower = 15 }, "spell_shadow_darkritual")); // No longer in the game
            // defaultEnchants.Add(new Enchant(2715, "Resilience of the Scourge", ItemSlot.Shoulders, new Stats() { SpellPower = 16, Mp5 = 5 }, "spell_shadow_deadofnight")); // No longer in the game
            defaultEnchants.Add(new Enchant(2606, "Zandalar Signet of Might", ItemSlot.Shoulders, new Stats() { AttackPower = 30 }, "inv_misc_armorkit_08"));
            defaultEnchants.Add(new Enchant(2605, "Zandalar Signet of Mojo", ItemSlot.Shoulders, new Stats() { SpellPower = 18 }, "inv_jewelry_ring_46"));
            defaultEnchants.Add(new Enchant(2604, "Zandalar Signet of Serenity", ItemSlot.Shoulders, new Stats() { SpellPower = 18 }, "spell_holy_powerwordshield"));
            #endregion
            #endregion
            #region Back
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4422, "Greater Protection", ItemSlot.Back, new Stats() { Stamina = 13 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4423, "Superior Intellect", ItemSlot.Back, new Stats() { Intellect = 11 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4424, "Superior Critical Strike", ItemSlot.Back, new Stats() { CritRating = 11 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4100, "Greater Critical Strike", ItemSlot.Back, new Stats() { CritRating = 13 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4096, "Greater Intellect", ItemSlot.Back, new Stats() { Intellect = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4064, "Greater Spell Piercing", ItemSlot.Back, new Stats() { PvPPower = 56 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4090, "Protection", ItemSlot.Back, new Stats() { Stamina = 6 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4087, "Critical Strike", ItemSlot.Back, new Stats() { CritRating = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4072, "Intellect", ItemSlot.Back, new Stats() { Intellect = 6 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3243, "Spell Piercing", ItemSlot.Back, new Stats() { PvPPower = 28 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(983, "Superior Agility", ItemSlot.Back, new Stats() { Agility = 8 }, "inv_enchant_formulagood_01"));
            //defaultEnchants.Add(new Enchant(1262, "Superior Arcane Resistance", ItemSlot.Back, new Stats() { ArcaneResistance = 20 }, "inv_enchant_formulagood_01"));
            //defaultEnchants.Add(new Enchant(1446, "Superior Shadow Resistance", ItemSlot.Back, new Stats() { ShadowResistance = 20 }, "inv_enchant_formulagood_01"));
            //defaultEnchants.Add(new Enchant(1354, "Superior Fire Resistance", ItemSlot.Back, new Stats() { FireResistance = 20 }, "inv_enchant_formulagood_01"));
            //defaultEnchants.Add(new Enchant(3230, "Superior Frost Resistance", ItemSlot.Back, new Stats() { FrostResistance = 20 }, "inv_enchant_formulagood_01"));
            //defaultEnchants.Add(new Enchant(1400, "Superior Nature Resistance", ItemSlot.Back, new Stats() { NatureResistance = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1099, "Major Agility", ItemSlot.Back, new Stats() { Agility = 11 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3831, "Greater Speed", ItemSlot.Back, new Stats() { HasteRating = 12 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3294, "Mighty Stamina", ItemSlot.Back, new Stats() { Stamina = 13 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1951, "Titanweave", ItemSlot.Back, new Stats() { DodgeRating = 16 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3256, "Shadow Armor", ItemSlot.Back, new Stats() { Agility = 5, BonusArmor = 20 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3296, "Wisdom", ItemSlot.Back, new Stats() { ThreatReductionMultiplier = 0.02f, Spirit = 5 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(368, "Greater Agility", ItemSlot.Back, new Stats() { Agility = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2662, "Major Armor", ItemSlot.Back, new Stats() { BonusArmor = 120 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2938, "Spell Penetration", ItemSlot.Back, new Stats() { PvPPower = 16 }, "inv_enchant_formulagood_01"));
            //defaultEnchants.Add(new Enchant(2664, "Major Resistance", ItemSlot.Back, new Stats() { NatureResistance = 7, ArcaneResistance = 7, FireResistance = 7, FrostResistance = 7, ShadowResistance = 7 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3825, "Speed", ItemSlot.Back, new Stats() { HasteRating = 15 }, "spell_holy_greaterheal"));
            //defaultEnchants.Add(new Enchant(1257, "Greater Arcane Resistance", ItemSlot.Back, new Stats() { ArcaneResistance = 15 }, "inv_enchant_formulasuperior_01"));
            //defaultEnchants.Add(new Enchant(1441, "Greater Shadow Resistance", ItemSlot.Back, new Stats() { ShadowResistance = 15 }, "inv_enchant_formulasuperior_01"));
            //defaultEnchants.Add(new Enchant(2619, "Greater Fire Resistance", ItemSlot.Back, new Stats() { FireResistance = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2648, "Steelweave", ItemSlot.Back, new Stats() { DodgeRating = 12 }, "inv_enchant_formulasuperior_01"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(2622, "Dodge", ItemSlot.Back, new Stats() { DodgeRating = 12 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2621, "Subtlety", ItemSlot.Back, new Stats() { ThreatReductionMultiplier = 0.02f }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(744, "Lesser Protection", ItemSlot.Back, new Stats() { BonusArmor = 20 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(783, "Minor Protection", ItemSlot.Back, new Stats() { BonusArmor = 10 }, "inv_enchant_formulagood_01"));
            #endregion
            #endregion
            #region Chest
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4417, "Super Resilience", ItemSlot.Chest, new Stats() { PvPResilience = 13 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4418, "Mighty Spirit", ItemSlot.Chest, new Stats() { Spirit = 13 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4419, "Glorious Stats", ItemSlot.Chest, new Stats() { Agility = 3, Strength = 3, Stamina = 3, Intellect = 3, Spirit = 3 }, "inv_enchant_formulagood_01")); // display doesn't agree with actual stats given
            defaultEnchants.Add(new Enchant(4420, "Superior Stamina", ItemSlot.Chest, new Stats() { Stamina = 19 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4088, "Exceptional Spirit", ItemSlot.Chest, new Stats() { Spirit = 8 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4103, "Greater Stamina", ItemSlot.Chest, new Stats() { Stamina = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4102, "Peerless Stats", ItemSlot.Chest, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4077, "Mighty Resilience", ItemSlot.Chest, new Stats() { PvPResilience = 8 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4063, "Mighty Stats", ItemSlot.Chest, new Stats() { Agility = 3, Strength = 3, Stamina = 3, Intellect = 3, Spirit = 3 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4070, "Stamina", ItemSlot.Chest, new Stats() { Stamina = 11 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(1953, "Greater Dodge", ItemSlot.Chest, new Stats() { DodgeRating = 11 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3252, "Super Stats", ItemSlot.Chest, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3832, "Powerful Stats", ItemSlot.Chest, new Stats() { Agility = 5, Strength = 5, Stamina = 5, Intellect = 5, Spirit = 5 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2659, "Exceptional Health", ItemSlot.Chest, new Stats() { Health = 150 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3236, "Mighty Health", ItemSlot.Chest, new Stats() { Health = 100 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3297, "Super Health", ItemSlot.Chest, new Stats() { Health = 138 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2381, "Greater Mana Restoration", ItemSlot.Chest, new Stats() { Spirit = 10 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(1951, "Defense", ItemSlot.Chest, new Stats() { DodgeRating = 18 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2661, "Exceptional Stats", ItemSlot.Chest, new Stats() { Agility = 6, Strength = 6, Stamina = 6, Intellect = 6, Spirit = 6 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2933, "Major Resilience", ItemSlot.Chest, new Stats() { PvPResilience = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3233, "Exceptional Mana", ItemSlot.Chest, new Stats() { Mana = 250 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(1144, "Major Spirit", ItemSlot.Chest, new Stats() { Spirit = 15 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3245, "Exceptional Resilience", ItemSlot.Chest, new Stats() { PvPResilience = 10 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3150, "Restore Mana Prime", ItemSlot.Chest, new Stats() { Spirit = 14 }, "spell_holy_greaterheal"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Wrist
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4411, "Mastery", ItemSlot.Wrist, new Stats() { MasteryRating = 11 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4412, "Superior Dodge", ItemSlot.Wrist, new Stats() { DodgeRating = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4414, "Super Intellect", ItemSlot.Wrist, new Stats() { Intellect = 11 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4415, "Exceptional Strength", ItemSlot.Wrist, new Stats() { Strength = 11 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4416, "Greater Agility", ItemSlot.Wrist, new Stats() { Agility = 11 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4071, "Critical Strike", ItemSlot.Wrist, new Stats() { CritRating = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4086, "Dodge", ItemSlot.Wrist, new Stats() { DodgeRating = 5 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4093, "Exceptional Spirit", ItemSlot.Wrist, new Stats() { Spirit = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4101, "Greater Critical Strike", ItemSlot.Wrist, new Stats() { CritRating = 13 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4108, "Greater Speed", ItemSlot.Wrist, new Stats() { HasteRating = 13 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4065, "Speed", ItemSlot.Wrist, new Stats() { HasteRating = 10 }, "spell_holy_greaterheal"));
            // Patch 4.0.6+ added Agility, Strength, and Intellect to bracers
            defaultEnchants.Add(new Enchant(4258, "Agility", ItemSlot.Wrist, new Stats() { Agility = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4256, "Major Strength", ItemSlot.Wrist, new Stats() { Strength = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4257, "Mighty Intellect", ItemSlot.Wrist, new Stats() { Intellect = 10 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(2326, "Greater Spellpower", ItemSlot.Wrist, new Stats() { SpellPower = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2661, "Greater Stats", ItemSlot.Wrist, new Stats() { Agility = 3, Strength = 3, Stamina = 3, Intellect = 3, Spirit = 3 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1147, "Major Spirit", ItemSlot.Wrist, new Stats() { Spirit = 9 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3845, "Greater Assault", ItemSlot.Wrist, new Stats() { AttackPower = 25 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2332, "Superior Spellpower", ItemSlot.Wrist, new Stats() { SpellPower = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3850, "Major Stamina", ItemSlot.Wrist, new Stats() { Stamina = 20 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(1119, "Exceptional Intellect", ItemSlot.Wrist, new Stats() { Intellect = 8 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2647, "Brawn", ItemSlot.Wrist, new Stats() { Strength = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(369, "Major Intellect", ItemSlot.Wrist, new Stats() { Intellect = 12 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1891, "Stats", ItemSlot.Wrist, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2648, "Greater Dodge", ItemSlot.Wrist, new Stats() { DodgeRating = 14 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2617, "Superior Healing", ItemSlot.Wrist, new Stats() { SpellPower = 15 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2679, "Restore Mana Prime", ItemSlot.Wrist, new Stats() { Spirit = 12 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2649, "Fortitude", ItemSlot.Wrist, new Stats() { Stamina = 12 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1600, "Striking", ItemSlot.Wrist, new Stats() { AttackPower = 38 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2650, "Spellpower", ItemSlot.Wrist, new Stats() { SpellPower = 15 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1593, "Assault", ItemSlot.Wrist, new Stats() { AttackPower = 24 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(1886, "Superior Stamina", ItemSlot.Wrist, new Stats() { Stamina = 9 }, "inv_enchant_formulagood_01"));
            #endregion
            // Unsorted
            #endregion
            #region Hands
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4430, "Greater Haste", ItemSlot.Hands, new Stats() { HasteRating = 11 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4432, "Super Strength", ItemSlot.Hands, new Stats() { Strength = 11 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4433, "Superior Mastery", ItemSlot.Hands, new Stats() { MasteryRating = 11 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4068, "Haste", ItemSlot.Hands, new Stats() { HasteRating = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4075, "Exceptional Strength", ItemSlot.Hands, new Stats() { Strength = 7 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4107, "Greater Mastery", ItemSlot.Hands, new Stats() { MasteryRating = 13 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4106, "Mighty Strength", ItemSlot.Hands, new Stats() { Strength = 10 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4061, "Mastery", ItemSlot.Hands, new Stats() { MasteryRating = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3253, "Armsman", ItemSlot.Hands, new Stats() { ThreatIncreaseMultiplier = 0.02f, ParryRating = 5 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3222, "Major Agility", ItemSlot.Hands, new Stats() { Agility = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3829, "Greater Assault", ItemSlot.Hands, new Stats() { AttackPower = 9 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1603, "Crusher", ItemSlot.Hands, new Stats() { AttackPower = 11 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3246, "Exceptional Spellpower", ItemSlot.Hands, new Stats() { SpellPower = 14 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2934, "Blasting", ItemSlot.Hands, new Stats() { CritRating = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1594, "Assault", ItemSlot.Hands, new Stats() { AttackPower = 13 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3260, "Glove Reinforcements", ItemSlot.Hands, new Stats() { Stamina = 24 }, "inv_misc_armorkit_08"));
            defaultEnchants.Add(new Enchant(684, "Major Strength", ItemSlot.Hands, new Stats() { Strength = 15 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2322, "Major Healing", ItemSlot.Hands, new Stats() { SpellPower = 19 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2937, "Major Spellpower", ItemSlot.Hands, new Stats() { SpellPower = 20 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(931, "Minor Haste", ItemSlot.Hands, new Stats() { HasteRating = 10 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(2564, "Superior Agility", ItemSlot.Hands, new Stats() { Agility = 15 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2613, "Threat", ItemSlot.Hands, new Stats() { ThreatIncreaseMultiplier = 0.02f }, "inv_enchant_formulasuperior_01"));
            #endregion
            // Unsorted
            #endregion
            #region Legs
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4824, "Ironscale Leg Armor", ItemSlot.Legs, new Stats() { Stamina = 430, DodgeRating = 165 }, "inv_misc_monsterscales_20"));
            defaultEnchants.Add(new Enchant(4822, "Shadowleather Leg Armor", ItemSlot.Legs, new Stats() { Agility = 170, CritRating = 165 }, "inv_misc_monsterscales_14"));
            defaultEnchants.Add(new Enchant(4823, "Angerhide Leg Armor", ItemSlot.Legs, new Stats() { Strength = 285, CritRating = 165 }, "inv_misc_armorkit_29"));
            defaultEnchants.Add(new Enchant(4870, "Toughened Leg Armor", ItemSlot.Legs, new Stats() { Stamina = 250, DodgeRating = 100 }, "inv_misc_monsterscales_20"));
            defaultEnchants.Add(new Enchant(4871, "Sha-Infested Leg Armor", ItemSlot.Legs, new Stats() { Agility = 170, CritRating = 100 }, "inv_misc_monsterscales_14"));
            defaultEnchants.Add(new Enchant(4872, "Brutal Leg Armor", ItemSlot.Legs, new Stats() { Strength = 170, CritRating = 100 }, "inv_misc_armorkit_29"));
            defaultEnchants.Add(new Enchant(5004, "Pearlescent Spellthread", ItemSlot.Legs, new Stats() { Intellect = 170, Spirit = 100 }, "inv_bracer_69"));
            defaultEnchants.Add(new Enchant(5003, "Cerulean Spellthread", ItemSlot.Legs, new Stats() { Intellect = 170, CritRating = 100 }, "inv_misc_thread_eternium"));
            defaultEnchants.Add(new Enchant(4826, "Greater Pearlescent Spellthread", ItemSlot.Legs, new Stats() { Intellect = 285, Spirit = 165 }, "inv_belt_42c"));
            defaultEnchants.Add(new Enchant(4825, "Greater Cerulean Spellthread", ItemSlot.Legs, new Stats() { Intellect = 18, CritRating = 10 }, "inv_belt_42"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4127, "Charscale Leg Armor", ItemSlot.Legs, new Stats() { Agility = 55, Stamina = 145 }, "inv_misc_monsterscales_20"));
            defaultEnchants.Add(new Enchant(4126, "Dragonscale Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 190, CritRating = 55 }, "inv_misc_monsterscales_14"));
            defaultEnchants.Add(new Enchant(4124, "Twilight Leg Armor", ItemSlot.Legs, new Stats() { Agility = 45, Stamina = 85 }, "inv_misc_armorkit_29"));
            defaultEnchants.Add(new Enchant(4122, "Scorched Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 110, CritRating = 45 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(4112, "Powerful Enchanted Spellthread", ItemSlot.Legs, new Stats() { Intellect = 95, Stamina = 80 }, "inv_misc_thread_eternium"));
            defaultEnchants.Add(new Enchant(4110, "Powerful Ghostly Spellthread", ItemSlot.Legs, new Stats() { Intellect = 95, Spirit = 55 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(4111, "Enchanted Spellthread", ItemSlot.Legs, new Stats() { Intellect = 55, Stamina = 65 }, "item_spellcloththread"));
            defaultEnchants.Add(new Enchant(4109, "Ghostly Spellthread", ItemSlot.Legs, new Stats() { Intellect = 55, Spirit = 45 }, "spell_nature_astralrecal"));
            defaultEnchants.Add(new Enchant(4270, "Drakehide Leg Armor", ItemSlot.Legs, new Stats() { DodgeRating = 55, Stamina = 145 }, "inv_misc_cataclysmarmorkit_08"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3853, "Earthen Leg Armor", ItemSlot.Legs, new Stats() { PvPResilience = 40, Stamina = 28 }, "inv_misc_armorkit_18"));
            defaultEnchants.Add(new Enchant(3822, "Frosthide Leg Armor", ItemSlot.Legs, new Stats() { Agility = 22, Stamina = 55 }, "inv_misc_armorkit_32"));
            defaultEnchants.Add(new Enchant(3823, "Icescale Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 75, CritRating = 22 }, "inv_misc_armorkit_33"));
            defaultEnchants.Add(new Enchant(3325, "Jormungar Leg Armor", ItemSlot.Legs, new Stats() { Agility = 15, Stamina = 45 }, "inv_misc_armorkit_31"));
            defaultEnchants.Add(new Enchant(3326, "Nerubian Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 55, CritRating = 15 }, "inv_misc_armorkit_29"));
            defaultEnchants.Add(new Enchant(3719, "Brilliant Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 50, Spirit = 20 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(3721, "Sapphire Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 50, Stamina = 30 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(3720, "Azure Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 35, Stamina = 20 }, "spell_nature_astralrecal"));
            defaultEnchants.Add(new Enchant(3718, "Shining Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 35, Spirit = 12 }, "Shining Spellthread"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(3013, "Nethercleft Leg Armor", ItemSlot.Legs, new Stats() { Agility = 12, Stamina = 40 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(3012, "Nethercobra Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 50, CritRating = 12 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(3011, "Clefthide Leg Armor", ItemSlot.Legs, new Stats() { Agility = 10, Stamina = 30 }, "inv_misc_armorkit_23"));
            defaultEnchants.Add(new Enchant(3010, "Cobrahide Leg Armor", ItemSlot.Legs, new Stats() { AttackPower = 40, CritRating = 10 }, "inv_misc_armorkit_21"));
            defaultEnchants.Add(new Enchant(2746, "Golden Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 35, Stamina = 20 }, "spell_holy_restoration"));
            defaultEnchants.Add(new Enchant(2748, "Runic Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 35, Stamina = 20 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(2747, "Mystic Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 25, Stamina = 15 }, "spell_nature_astralrecal"));
            defaultEnchants.Add(new Enchant(2745, "Silver Spellthread", ItemSlot.Legs, new Stats() { SpellPower = 25, Stamina = 15 }, "spell_nature_lightning"));
            #endregion
            #region Level 60 (Vanilla)
            #endregion
            // Unsorted
            #endregion
            #region Feet
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4426, "Greater Haste", ItemSlot.Feet, new Stats() { HasteRating = 175 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4428, "Blurred Speed", ItemSlot.Feet, new Stats() { Agility = 140, MovementSpeed = 0.1f }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4429, "Pandaren's Step", ItemSlot.Feet, new Stats() { MasteryRating = 9, MovementSpeed = 0.1f }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4069, "Haste", ItemSlot.Feet, new Stats() { HasteRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4076, "Major Agility", ItemSlot.Feet, new Stats() { Agility = 35 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4094, "Mastery", ItemSlot.Feet, new Stats() { MasteryRating = 50 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4105, "Assassin's Step", ItemSlot.Feet, new Stats() { Agility = 25, MovementSpeed = 0.08f }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4104, "Lavawalker", ItemSlot.Feet, new Stats() { MasteryRating = 35, MovementSpeed = 0.08f }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(4062, "Earthen Vitality", ItemSlot.Feet, new Stats() { Stamina = 30, MovementSpeed = 0.08f }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(1075, "Greater Fortitude", ItemSlot.Feet, new Stats() { Stamina = 22 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3244, "Greater Vitality", ItemSlot.Feet, new Stats() { Stamina = 14f, Spirit = 14f }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1147, "Greater Spirit", ItemSlot.Feet, new Stats() { Spirit = 18 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(983, "Superior Agility", ItemSlot.Feet, new Stats() { Agility = 16 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1597, "Greater Assault", ItemSlot.Feet, new Stats() { AttackPower = 32 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3232, "Tuskarr's Vitality", ItemSlot.Feet, new Stats() { Stamina = 15, MovementSpeed = 0.08f }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2656, "Vitality", ItemSlot.Feet, new Stats() { Stamina = 10f, Spirit = 10f }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2649, "Fortitude", ItemSlot.Feet, new Stats() { Stamina = 12 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2657, "Dexterity", ItemSlot.Feet, new Stats() { Agility = 12 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2940, "Boar's Speed", ItemSlot.Feet, new Stats() { Stamina = 9, MovementSpeed = 0.08f }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2939, "Cat's Swiftness", ItemSlot.Feet, new Stats() { Agility = 6, MovementSpeed = 0.08f }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(3824, "Assault", ItemSlot.Feet, new Stats() { AttackPower = 24 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(851, "Spirit", ItemSlot.Feet, new Stats() { Spirit = 5 }, "inv_enchant_formulasuperior_01"));
            #endregion
            // Unsorted
            #endregion
            #region Weapons
            #region Two Handers
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4227, "Mighty Agility", ItemSlot.TwoHand, new Stats() { Agility = 26 }, "inv_potion_162"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3855, "Spellpower (Staff)", ItemSlot.TwoHand, new Stats() { SpellPower = 35 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3828, "Greater Savagery", ItemSlot.TwoHand, new Stats() { AttackPower = 43 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(3827, "Massacre", ItemSlot.TwoHand, new Stats() { AttackPower = 55 }, "inv_enchant_formulasuperior_01"));
            //defaultEnchants.Add(new Enchant(3827, "Scourgebane", ItemSlot.TwoHand, new Stats() { AttackPowerAgainstUndead = 70 }, "inv_enchant_formulasuperior_01")); // No modelling
            defaultEnchants.Add(new Enchant(3854, "Greater Spellpower (Staff)", ItemSlot.TwoHand, new Stats() { SpellPower = 41 }, "inv_enchant_formulasuperior_01"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2667, "Savagery", ItemSlot.TwoHand, new Stats() { AttackPower = 70 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2670, "Major Agility", ItemSlot.TwoHand, new Stats() { Agility = 35 }, "inv_enchant_formulagood_01"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(241, "Minor Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 2 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(943, "Lesser Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 3 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1897, "Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 5 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(963, "Greater Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 7 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(1896, "Superior Impact", ItemSlot.TwoHand, new Stats() { WeaponDamage = 9 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2646, "Agility", ItemSlot.TwoHand, new Stats() { Agility = 25 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(1903, "Major Spirit", ItemSlot.TwoHand, new Stats() { Spirit = 9 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1904, "Major Intellect", ItemSlot.TwoHand, new Stats() { Intellect = 9 }, "inv_enchant_formulagood_01"));
            #endregion
            // Unsorted
            #endregion
            #region One Handers (Any of these can also go on TwoHanders)
            #region Level 90 (MoP)
            {
                // http://mop.wowhead.com/item=79061fl
                // Proc spell - http://mop.wowhead.com/spell=110761
                //enchantTemp = new Stats() { };
                //enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = 1000, }, 12, 0, -1f));
                //defaultEnchants.Add(new Enchant(4717, "Pandamonium", ItemSlot.OneHand, enchantTemp, "pandarenracial_innerpeace"));
            }
            {
                // http://mop.wowhead.com/item=74723
                // Haste - http://mop.wowhead.com/spell=104423
                // Crit - http://mop.wowhead.com/spell=104509
                // Mastery - http://mop.wowhead.com/spell=104510
                float dur = 12f;
                float chance = -2.2f / 3f;
                float proc = 75f;
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { CritRating = proc }, dur, 1, chance));
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { HasteRating = proc }, dur, 1, chance));
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { MasteryRating = proc }, dur, 1, chance));
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHitorDoTTick, new Stats() { CritRating = proc }, dur, 1, chance));
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHitorDoTTick, new Stats() { HasteRating = proc }, dur, 1, chance));
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHitorDoTTick, new Stats() { MasteryRating = proc }, dur, 1, chance));
                defaultEnchants.Add(new Enchant(4441, "Windsong", ItemSlot.OneHand, enchantTemp, "spell_frost_arcticwinds"));
            }
            {
                // http://mop.wowhead.com/item=74724
                // Proc spell - http://mop.wowhead.com/spell=104993
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellHit, new Stats() { Intellect = 103, JadeSpiritProcSpirit = 47 }, 12, 50, -2.2f)); //, 12, 0, -2) { RealPPM = true });
                defaultEnchants.Add(new Enchant(4442, "Jade Spirit", ItemSlot.OneHand, enchantTemp, "spell_shaman_spiritlink"));
            }
            {
                // http://mop.wowhead.com/item=74725
                // Proc spell - http://mop.wowhead.com/spell=116616
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { FireDamage = 174, }, 0, 0.1f, -10f) { RealPPM = true });
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { FireDamage = 174, }, 0, 0.1f, -10f) { RealPPM = true });
                defaultEnchants.Add(new Enchant(4443, "Elemental Force", ItemSlot.OneHand, enchantTemp, "ability_mage_firestarter"));
            }
            {
                // http://mop.wowhead.com/item=74726
                // Strength - http://mop.wowhead.com/spell=118335
                // Agility - http://mop.wowhead.com/spell=118334
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { HighestStat = 103, }, 12, 0, -1f)); //, 12, 0, -2) { RealPPM = true });
                defaultEnchants.Add(new Enchant(4444, "Dancing Steel", ItemSlot.OneHand, enchantTemp, "trade_archaeology_zinrokh-sword"));
            }
            {
                // http://mop.wowhead.com/item=74727
                // Proc spell - http://mop.wowhead.com/spell=116631
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeAttack, new Stats() { DamageAbsorbed = 469, }, 10, 3, -3f));//, 10, 0, -6) { RealPPM = true });
                defaultEnchants.Add(new Enchant(4445, "Colossus", ItemSlot.OneHand, enchantTemp, "ability_vehicle_shellshieldgenerator"));
            }
            {
                // http://mop.wowhead.com/item=74728
                // Proc spell - http://mop.wowhead.com/spell=116660
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeAttack, new Stats() { DodgeRating = 83, }, 7, 0, -2f)); //, 7, 0, -4) { RealPPM = true });
                defaultEnchants.Add(new Enchant(4446, "River's Song", ItemSlot.OneHand, enchantTemp, "inv_misc_volatilewater"));
            }
            #endregion
            #region Level 85 (Cataclysm)
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { HasteRating = 90, }, 12, 0, -1f)); // read info off of cata.wowhead.com
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = 90, }, 12, 45, 0.15f)); // read info off of cata.wowhead.com
                defaultEnchants.Add(new Enchant(4083, "Hurricane", ItemSlot.OneHand, enchantTemp, "spell_nature_cyclone"));
            }
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Spirit = 40, }, 15, 20, 0.25f)); // read info off of cata.wowhead.com
                defaultEnchants.Add(new Enchant(4084, "Heartsong", ItemSlot.OneHand, enchantTemp, "ability_paladin_sacredcleansing"));
            }
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = 100, }, 12, 0f, -1f)); // read info off of cata.wowhead.com, and based proc times off of berserking
                defaultEnchants.Add(new Enchant(4099, "Landslide", ItemSlot.OneHand, enchantTemp, "ability_paladin_sacredcleansing"));
            }
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageOrHealingDone, new Stats() { Intellect = 100, }, 12, 45f, 0.33f)); // read info off of cata.wowhead.com
                defaultEnchants.Add(new Enchant(4097, "Power Torrent", ItemSlot.OneHand, enchantTemp, "ability_paladin_sacredcleansing"));
            }
            {
                enchantTemp = new Stats() { };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { DodgeRating = 120, MovementSpeed = 0.15f }, 10, 0f, -1f)); // 2 PPM
                defaultEnchants.Add(new Enchant(4098, "Windwalk", ItemSlot.OneHand, enchantTemp, "ability_paladin_sacredcleansing"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { NatureDamage = 100, }, 0f, 0f, -5f)); // 5 PPM
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { NatureDamage = 100, }, 0f, 10f, .2f)); // 20% chance with 10 sec ICD (http://elitistjerks.com/f76/t110342-retribution_concordance_4_0_6_compliant/p31/#post1872409)
                defaultEnchants.Add(new Enchant(4067, "Avalanche", ItemSlot.OneHand, enchantTemp, "spell_fire_burnout"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { HealthRestore = 800f, }, 0f, 0f, -4.6f)); // 4.6 PPM
                defaultEnchants.Add(new Enchant(4066, "Mending", ItemSlot.OneHand, enchantTemp, "spell_nature_healingway"));
            }
            // Not modelling Elemental Slayer
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(1606, "Greater Potency", ItemSlot.OneHand, new Stats() { AttackPower = 25 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3830, "Exceptional Spellpower", ItemSlot.OneHand, new Stats() { SpellPower = 25 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3844, "Exceptional Spirit", ItemSlot.OneHand, new Stats() { Spirit = 23 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(1103, "Exceptional Agility", ItemSlot.OneHand, new Stats() { Agility = 13 }, "inv_enchant_formulagood_01"));
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { FireDamage = 200f, }, 0f, 0f, -3)); // 3 PPM = 9% Chance to proc
                defaultEnchants.Add(new Enchant(3239, "Icebreaker", ItemSlot.OneHand, enchantTemp, "spell_fire_burnout"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { HealthRestore = 333f, }, 0f, 0f, -4.6f)); // 4.6 PPM
                defaultEnchants.Add(new Enchant(3241, "Lifeward", ItemSlot.OneHand, enchantTemp, "spell_nature_healingway"));
            }
            defaultEnchants.Add(new Enchant(3834, "Mighty Spellpower", ItemSlot.OneHand, new Stats() { SpellPower = 32 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(3833, "Superior Potency", ItemSlot.OneHand, new Stats() { AttackPower = 33 }, "inv_enchant_formulasuperior_01"));
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { AttackPower = 100, BonusArmorMultiplier = -.05f }, 15f, 0f, -1f));
                defaultEnchants.Add(new Enchant(3789, "Berserking", ItemSlot.OneHand, enchantTemp, "spell_nature_strength"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellHit, new Stats() { HasteRating = 125 }, 10f, 35f, 0.35f));
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MangleCatOrShredOrInfectedWoundsHit, new Stats() { HasteRating = 125 }, 10f, 35f, 0.35f));
                defaultEnchants.Add(new Enchant(3790, "Black Magic", ItemSlot.OneHand, enchantTemp, "spell_shadow_unstableaffliction_1"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { ParryRating = 100, }, 10f, 0f, -1f)); // Need to add the 600-800 Physical Damage on next Parry
                defaultEnchants.Add(new Enchant(3869, "Blade Ward", ItemSlot.OneHand, enchantTemp, "inv_sword_121"));
            }
            {
                Stats blood_drain_a = new Stats(); // Once at 35% Health, your melee Hits restores 400 health per stack
                blood_drain_a.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { HealthRestore = 200 }, 20f, 0f, 1f, 5));
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageTaken, blood_drain_a, 0f, 0f, .35f));
                defaultEnchants.Add(new Enchant(3870, "Blood Draining", ItemSlot.OneHand, enchantTemp, "inv_misc_gem_bloodstone_03"));
            }
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2666, "Major Intellect", ItemSlot.OneHand, new Stats() { Intellect = 30 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(963, "Major Striking", ItemSlot.OneHand, new Stats() { WeaponDamage = 7 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(3222, "Greater Agility", ItemSlot.OneHand, new Stats() { Agility = 20 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2343, "Major Healing", ItemSlot.OneHand, new Stats() { SpellPower = 40 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2669, "Major Spellpower", ItemSlot.OneHand, new Stats() { SpellPower = 40 }, "inv_enchant_formulagood_01"));
            defaultEnchants.Add(new Enchant(2668, "Potency", ItemSlot.OneHand, new Stats() { Strength = 20 }, "inv_enchant_formulagood_01"));
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { CritRating = 120f }, 15f, 0f, -1f));
                defaultEnchants.Add(new Enchant(3225, "Executioner", ItemSlot.OneHand, enchantTemp, "inv_2h_auchindoun_01"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { Agility = 120, HasteRating = 30f }, 15f, 0f, -1f));
                defaultEnchants.Add(new Enchant(2673, "Mongoose", ItemSlot.OneHand, enchantTemp, "spell_nature_unrelentingstorm"));
            }
            defaultEnchants.Add(new Enchant(2672, "Soulfrost", ItemSlot.OneHand, new Stats() { SpellFrostDamageRating = 54, SpellShadowDamageRating = 54 }, "inv_enchant_formulasuperior_01"));
            defaultEnchants.Add(new Enchant(2671, "Sunfire", ItemSlot.OneHand, new Stats() { SpellFireDamageRating = 50, SpellArcaneDamageRating = 50 }, "inv_enchant_formulasuperior_01"));
            // Patch 4.0.6+ Weapon Chains now reduce disarm duration by 60%, up from 50%.
            defaultEnchants.Add(new Enchant(3223, "Adamantite Weapon Chain", ItemSlot.OneHand, new Stats() { ParryRating = 15, DisarmDurReduc = 0.60f }, "spell_frost_chainsofice"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Off Handers Only (Usually Means Shields)
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4993, "Greater Parry", ItemSlot.OffHand, new Stats() { ParryRating = 11 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4434, "Major Intellect", ItemSlot.OffHand, new Stats() { Intellect = 10 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 85 (Cataclysm)
            // Patch 4.0.6+ lowered the Off-hand enchant from 100 Intellect to 40 Intellect
            defaultEnchants.Add(new Enchant(4091, "Superior Intellect", ItemSlot.OffHand, new Stats() { Intellect = 8 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4085, "Mastery", ItemSlot.OffHand, new Stats() { MasteryRating = 10 }, "spell_holy_greaterheal", true));
            defaultEnchants.Add(new Enchant(4073, "Protection", ItemSlot.OffHand, new Stats() { Stamina = 3 }, "spell_holy_greaterheal", true));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3849, "Titanium Plating", ItemSlot.OffHand, new Stats() { ParryRating = 26, DisarmDurReduc = 0.50f }, "inv_shield_19", true));
            defaultEnchants.Add(new Enchant(1952, "Dodge", ItemSlot.OffHand, new Stats() { DodgeRating = 10 }, "spell_holy_greaterheal", true));
            defaultEnchants.Add(new Enchant(1128, "Greater Intellect", ItemSlot.OffHand, new Stats() { Intellect = 13 }, "spell_holy_greaterheal", true));
            #endregion
            #region Level 70 (BC)
            //defaultEnchants.Add(new Enchant(1888, "Resistance", ItemSlot.OffHand, new Stats() { ShadowResistance = 5, ArcaneResistance = 5, NatureResistance = 5, FireResistance = 5, FrostResistance = 5 }, "inv_enchant_formulagood_01", true));
            defaultEnchants.Add(new Enchant(2655, "Shield Block", ItemSlot.OffHand, new Stats() { ParryRating = 15 }, "inv_enchant_formulagood_01", true));
            defaultEnchants.Add(new Enchant(3229, "Resilience", ItemSlot.OffHand, new Stats() { PvPResilience = 12 }, "spell_holy_greaterheal", true));
            defaultEnchants.Add(new Enchant(2654, "Intellect", ItemSlot.OffHand, new Stats() { Intellect = 12 }, "inv_misc_note_01", true));
            defaultEnchants.Add(new Enchant(1071, "Major Stamina", ItemSlot.OffHand, new Stats() { Stamina = 18 }, "inv_misc_note_01", true));
            defaultEnchants.Add(new Enchant(2653, "Tough Shield", ItemSlot.OffHand, new Stats() { DodgeRating = 12 }, "spell_holy_greaterheal", true));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Ranged
            #region Level 90 (MoP)
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.RangedHit, new Stats() { CritRating = 900 }, 10, 45, 0.10f));
                defaultEnchants.Add(new Enchant(4700, "Mirror Scope", ItemSlot.Ranged, enchantTemp, "inv_misc_scopea"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.RangedHit, new Stats() { Agility = 1800 }, 10, 0, -1f));
                defaultEnchants.Add(new Enchant(4699, "Lord Blastington's Scope of Doom", ItemSlot.Ranged, enchantTemp, "inv_misc_scopea"));
            }
            #endregion
            #region Level 85 (Cataclysm)
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.RangedHit, new Stats() { PhysicalDamage = (550f + 1650f) / 2f, Agility = 300, }, 10, 45, 0.10f));
                defaultEnchants.Add(new Enchant(4267, "Flintlocke's Woodchucker", ItemSlot.Ranged, enchantTemp, "inv_misc_scopeb"));
            }
            defaultEnchants.Add(new Enchant(4177, "Safety Catch Removal Kit", ItemSlot.Ranged, new Stats() { HasteRating = 88 }, "inv_misc_enggizmos_37"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3608, "Heartseeker Scope", ItemSlot.Ranged, new Stats() { RangedCritRating = 40 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(2724, "Stabilized Eternium Scope", ItemSlot.Ranged, new Stats() { RangedCritRating = 28 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(3607, "Sun Scope", ItemSlot.Ranged, new Stats() { RangedHasteRating = 40 }, "inv_misc_spyglass_03"));
            defaultEnchants.Add(new Enchant(3843, "Diamond-cut Refractor Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 15 }, "ability_hunter_rapidregeneration"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2723, "Khorium Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 12 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(2722, "Adamantite Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 10 }, "inv_misc_spyglass_02"));
            #endregion
            #region Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(2523, "Biznicks 247x128 Accurascope", ItemSlot.Ranged, new Stats() { RangedHitRating = 30 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(664, "Sniper Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 7 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(663, "Deadly Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 5 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(33, "Accurate Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 3 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(32, "Standard Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 2 }, "inv_misc_spyglass_02"));
            defaultEnchants.Add(new Enchant(30, "Crude Scope", ItemSlot.Ranged, new Stats() { ScopeDamage = 1 }, "inv_misc_spyglass_02"));
            #endregion
            // Unsorted
            #endregion
            #endregion

            #region Engineering
            #region Level 85 (Cataclysm)
            #region Hands
            {
                //enchantTemp = new Stats();
                //enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = 1500f }, 12f, 60f));
                //defaultEnchants.Add(new Enchant(4180, "Quickflip Deflection Plates", ItemSlot.Hands, enchantTemp, "inv_misc_desecrated_leatherglove"));
            }
            {
                //enchantTemp = new Stats();
                //enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { Intellect = 480f }, 12f, 60f));
                //defaultEnchants.Add(new Enchant(4179, "Synapse Springs", ItemSlot.Hands, enchantTemp, "spell_shaman_elementaloath"));
            }
            {
                //enchantTemp = new Stats();
                //enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { NatureDamage = 4800f, }, 0f, 120f));
                //defaultEnchants.Add(new Enchant(4181, "Tazik Shocker", ItemSlot.Hands, enchantTemp, "spell_nature_abolishmagic"));
            }
            #endregion
            #region Waist
            {
                //enchantTemp = new Stats();
                //enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { DamageAbsorbed = 18000f }, 8f, 5 * 60f));
                //defaultEnchants.Add(new Enchant(4188, "Grounded Plasma Shield", ItemSlot.Waist, enchantTemp, "inv_boots_plate_13"));
            }
            #endregion
            #region Cogwheel
            defaultEnchants.Add(new Enchant(59491, "Flashing Cogwheel", ItemSlot.Cogwheel, new Stats() { ParryRating = 208 }, "inv_misc_enggizmos_30"));
            defaultEnchants.Add(new Enchant(59480, "Fractured Cogwheel", ItemSlot.Cogwheel, new Stats() { MasteryRating = 208 }, "inv_misc_enggizmos_30"));
            defaultEnchants.Add(new Enchant(59479, "Quick Cogwheel", ItemSlot.Cogwheel, new Stats() { HasteRating = 208 }, "inv_misc_enggizmos_30"));
            defaultEnchants.Add(new Enchant(59478, "Smooth Cogwheel", ItemSlot.Cogwheel, new Stats() { CritRating = 208 }, "inv_misc_enggizmos_30"));
            defaultEnchants.Add(new Enchant(59496, "Sparkling Cogwheel", ItemSlot.Cogwheel, new Stats() { Spirit = 208 }, "inv_misc_enggizmos_30"));
            defaultEnchants.Add(new Enchant(59477, "Subtle Cogwheel", ItemSlot.Cogwheel, new Stats() { DodgeRating = 208 }, "inv_misc_enggizmos_30"));
            #endregion
            #endregion
            #region Level 80 (WotLK)
            #region Back
            // defaultEnchants.Add(new Enchant(3859, "Springy Arachnoweave", ItemSlot.Back, new Stats() { SpellPower = 27f })); // No longer supplies any stats, only the parachute
            // defaultEnchants.Add(new Enchant(3605, "Flexweave Underlay", ItemSlot.Back, new Stats() { Agility = 23f })); // No longer supplies any stats, only the parachute
            #endregion
            #region Hands
            {
                //enchantTemp = new Stats();
                //enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusArmor = 700f }, 14f, 60f));
                //defaultEnchants.Add(new Enchant(3860, "Reticulated Armor Webbing", ItemSlot.Hands, enchantTemp, "inv_misc_desecrated_leatherglove"));
            }
            {
                //enchantTemp = new Stats();
                //enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { HasteRating = 340f }, 12f, 60f));
                //defaultEnchants.Add(new Enchant(3604, "Hyperspeed Accelerators", ItemSlot.Hands, enchantTemp, "spell_shaman_elementaloath"));
            }
            {
                //enchantTemp = new Stats();
                //enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 1837f, }, 0f, 45f));
                //defaultEnchants.Add(new Enchant(3603, "Hand-Mounted Pyro Rocket", ItemSlot.Hands, enchantTemp, "spell_fire_burnout"));
            }
            #endregion
            #region Waist
            {
                //enchantTemp = new Stats();
                //enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 875 }, 0, 360, 1f));
                //defaultEnchants.Add(new Enchant(3601, "Frag Belt", ItemSlot.Waist, enchantTemp, "spell_fire_selfdestruct")); defaultEnchants.Add(new Enchant(3260, "Glove Reinforcements", ItemSlot.Hands, new Stats() { BonusArmor = 240 }));
            }
            #endregion
            #region Feet
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { MovementSpeed = 1.50f, }, 5f, 3 * 60f));
                defaultEnchants.Add(new Enchant(3606, "Nitro Boosts", ItemSlot.Feet, enchantTemp, "spell_fire_burningspeed"));
            }
            #endregion
            #endregion
            #endregion
            #region Enchanting
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4359, "Greater Agility", ItemSlot.Finger, new Stats() { Agility = 160 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4361, "Greater Stamina", ItemSlot.Finger, new Stats() { Stamina = 240 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4360, "Greater Intellect", ItemSlot.Finger, new Stats() { Intellect = 160 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4358, "Greater Strength", ItemSlot.Finger, new Stats() { Strength = 160 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4079, "Agility", ItemSlot.Finger, new Stats() { Agility = 40 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4081, "Stamina", ItemSlot.Finger, new Stats() { Stamina = 60 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4080, "Intellect", ItemSlot.Finger, new Stats() { Intellect = 40 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(4078, "Strength", ItemSlot.Finger, new Stats() { Strength = 40 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3839, "Assault", ItemSlot.Finger, new Stats() { AttackPower = 40 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3840, "Lesser Intellect", ItemSlot.Finger, new Stats() { Intellect = 20 }, "spell_holy_greaterheal"));
            defaultEnchants.Add(new Enchant(3791, "Lesser Stamina", ItemSlot.Finger, new Stats() { Stamina = 30 }, "spell_holy_greaterheal"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2928, "Minor Intellect", ItemSlot.Finger, new Stats() { Intellect = 10 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2929, "Striking", ItemSlot.Finger, new Stats() { WeaponDamage = 2 }, "inv_misc_note_01"));
            defaultEnchants.Add(new Enchant(2931, "Stats", ItemSlot.Finger, new Stats() { Agility = 4, Strength = 4, Stamina = 4, Intellect = 4, Spirit = 4 }, "inv_misc_note_01"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Inscription
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4915, "Secret Crane Wing Inscription", ItemSlot.Shoulders, new Stats() { Intellect = 520, CritRating = 100 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4912, "Secret Ox Horn Inscription", ItemSlot.Shoulders, new Stats() { Stamina = 750, DodgeRating = 100 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4913, "Secret Tiger Claw Inscription", ItemSlot.Shoulders, new Stats() { Strength = 520, CritRating = 100 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4914, "Secret Tiger Fang Inscription", ItemSlot.Shoulders, new Stats() { Agility = 520, CritRating = 100 }, "inv_misc_mastersinscription"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4196, "Felfire Inscription", ItemSlot.Shoulders, new Stats() { Intellect = 130, HasteRating = 25 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4195, "Inscription of the Earth Prince", ItemSlot.Shoulders, new Stats() { Stamina = 195, DodgeRating = 25 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4194, "Lionsmane Inscription", ItemSlot.Shoulders, new Stats() { Strength = 130, CritRating = 25 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(4193, "Swiftsteel Inscription", ItemSlot.Shoulders, new Stats() { Agility = 130, MasteryRating = 25 }, "inv_misc_mastersinscription"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3835, "Master's Inscription of the Axe", ItemSlot.Shoulders, new Stats() { CritRating = 15, AttackPower = 120 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(3836, "Master's Inscription of the Crag", ItemSlot.Shoulders, new Stats() { Spirit = 15, Intellect = 60 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(3837, "Master's Inscription of the Pinnacle", ItemSlot.Shoulders, new Stats() { DodgeRating = 60, ParryRating = 15 }, "inv_misc_mastersinscription"));
            defaultEnchants.Add(new Enchant(3838, "Master's Inscription of the Storm", ItemSlot.Shoulders, new Stats() { CritRating = 15, SpellPower = 70 }, "inv_misc_mastersinscription"));
            #endregion
            #endregion
            #region Leatherworking
            #region Level 90 (MoP)
            #region Wrist
            defaultEnchants.Add(new Enchant(4875, "Fur Lining - Agility (Rank 3)", ItemSlot.Wrist, new Stats() { Agility = 500 }, "inv_misc_pelt_13"));
            defaultEnchants.Add(new Enchant(4877, "Fur Lining - Intellect (Rank 3)", ItemSlot.Wrist, new Stats() { Intellect = 500 }, "inv_misc_pelt_11"));
            defaultEnchants.Add(new Enchant(4878, "Fur Lining - Stamina (Rank 3)", ItemSlot.Wrist, new Stats() { Stamina = 750 }, "inv_misc_pelt_10"));
            defaultEnchants.Add(new Enchant(4879, "Fur Lining - Strength (Rank 3)", ItemSlot.Wrist, new Stats() { Strength = 500 }, "inv_misc_pelt_12"));
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(4880, "Primal Leg Reinforcements (Rank 3)", ItemSlot.Legs, new Stats() { Agility = 285, CritRating = 165 }, "inv_misc_cataclysmarmorkit_12"));
            defaultEnchants.Add(new Enchant(4881, "Draconic Leg Reinforcements (Rank 3)", ItemSlot.Legs, new Stats() { Strength = 285, CritRating = 165 }, "inv_misc_cataclysmarmorkit_11"));
            defaultEnchants.Add(new Enchant(4882, "Heavy Leg Reinforcements (Rank 3)", ItemSlot.Legs, new Stats() { DodgeRating = 165, Stamina = 430 }, "inv_misc_cataclysmarmorkit_10"));
            #endregion
            #endregion
            #region Level 85 (Cataclysm)
            #region Wrist
            defaultEnchants.Add(new Enchant(4190, "Fur Lining - Agility (Rank 2)", ItemSlot.Wrist, new Stats() { Agility = 130 }, "inv_misc_pelt_13"));
            defaultEnchants.Add(new Enchant(4192, "Fur Lining - Intellect (Rank 2)", ItemSlot.Wrist, new Stats() { Intellect = 130 }, "inv_misc_pelt_11"));
            defaultEnchants.Add(new Enchant(4189, "Fur Lining - Stamina (Rank 2)", ItemSlot.Wrist, new Stats() { Stamina = 195 }, "inv_misc_pelt_10"));
            defaultEnchants.Add(new Enchant(4191, "Fur Lining - Strength (Rank 2)", ItemSlot.Wrist, new Stats() { Strength = 130 }, "inv_misc_pelt_12"));
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(4883, "Primal Leg Reinforcements (Rank 2)", ItemSlot.Legs, new Stats() { Agility = 95, CritRating = 55 }, "inv_misc_cataclysmarmorkit_12"));
            defaultEnchants.Add(new Enchant(4885, "Draconic Leg Reinforcements (Rank 2)", ItemSlot.Legs, new Stats() { Strength = 95, CritRating = 55 }, "inv_misc_cataclysmarmorkit_11"));
            defaultEnchants.Add(new Enchant(4884, "Heavy Leg Reinforcements (Rank 2)", ItemSlot.Legs, new Stats() { DodgeRating = 55, Stamina = 143 }, "inv_misc_cataclysmarmorkit_10"));
            #endregion
            #endregion
            #region Level 80 (WotLK)
            #region Wrist
            defaultEnchants.Add(new Enchant(3756, "Fur Lining - Agility (Rank 1)", ItemSlot.Wrist, new Stats() { Agility = 57 }, "inv_misc_pelt_13"));
            defaultEnchants.Add(new Enchant(3758, "Fur Lining - Intellect (Rank 1)", ItemSlot.Wrist, new Stats() { Intellect = 57 }, "inv_misc_pelt_11"));
            defaultEnchants.Add(new Enchant(3757, "Fur Lining - Stamina (Rank 1)", ItemSlot.Wrist, new Stats() { Stamina = 102 }, "inv_misc_pelt_10"));
            defaultEnchants.Add(new Enchant(4874, "Fur Lining - Strength (Rank 1)", ItemSlot.Wrist, new Stats() { Strength = 57 }, "inv_misc_pelt_12"));
            //defaultEnchants.Add(new Enchant(3759, "Fur Lining - Fire Resist", ItemSlot.Wrist, new Stats() { FireResistance = 70 }, "trade_leatherworking"));
            //defaultEnchants.Add(new Enchant(3760, "Fur Lining - Frost Resist", ItemSlot.Wrist, new Stats() { FrostResistance = 70 }, "trade_leatherworking"));
            //defaultEnchants.Add(new Enchant(3761, "Fur Lining - Shadow Resist", ItemSlot.Wrist, new Stats() { ShadowResistance = 70 }, "trade_leatherworking"));
            //defaultEnchants.Add(new Enchant(3762, "Fur Lining - Nature Resist", ItemSlot.Wrist, new Stats() { NatureResistance = 70 }, "trade_leatherworking"));
            //defaultEnchants.Add(new Enchant(3763, "Fur Lining - Arcane Resist", ItemSlot.Wrist, new Stats() { ArcaneResistance = 70 }, "trade_leatherworking"));
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(4886, "Primal Leg Reinforcements (Rank 1)", ItemSlot.Legs, new Stats() { Agility = 37, CritRating = 22 }, "inv_misc_cataclysmarmorkit_12"));
            defaultEnchants.Add(new Enchant(4888, "Draconic Leg Reinforcements (Rank 1)", ItemSlot.Legs, new Stats() { Strength = 37, CritRating = 22 }, "inv_misc_cataclysmarmorkit_11"));
            defaultEnchants.Add(new Enchant(4887, "Heavy Leg Reinforcements (Rank 1)", ItemSlot.Legs, new Stats() { DodgeRating = 22, Stamina = 55 }, "inv_misc_cataclysmarmorkit_10"));
            #endregion
            #endregion
            #endregion
            #region Tailoring
            #region Level 90 (MoP)
            #region Back
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Intellect = 2000 }, 15, 64, 0.25f));
                defaultEnchants.Add(new Enchant(4892, "Lightweave Embroidery (Rank 3)", ItemSlot.Back, enchantTemp, "spell_arcane_prismaticcloak"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 4000 }, 15, 55, 0.15f));
                defaultEnchants.Add(new Enchant(4894, "Swordguard Embroidery (Rank 3)", ItemSlot.Back, enchantTemp, "ability_rogue_throwingspecialization"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Spirit = 3000 }, 15, 60, 0.35f));
                defaultEnchants.Add(new Enchant(4893, "Darkglow Embroidery (Rank 3)", ItemSlot.Back, enchantTemp, "spell_nature_giftofthewaterspirit"));
            }
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(4896, "Sanctified Spellthread (Rank 3)", ItemSlot.Legs, new Stats() { Intellect = 285f, Spirit = 165f }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(4895, "Master's Spellthread (Rank 3)", ItemSlot.Legs, new Stats() { Intellect = 285f, CritRating = 165f }, "spell_nature_astralrecalgroup"));
            #endregion
            #endregion
            #region Level 85 (Cataclysm)
            #region Back
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Intellect = 580 }, 15, 64, 0.25f));
                defaultEnchants.Add(new Enchant(4115, "Lightweave Embroidery (Rank 2)", ItemSlot.Back, enchantTemp, "spell_arcane_prismaticcloak"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 1000 }, 15, 55, 0.15f));
                defaultEnchants.Add(new Enchant(4118, "Swordguard Embroidery (Rank 2)", ItemSlot.Back, enchantTemp, "ability_rogue_throwingspecialization"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Spirit = 580 }, 15, 60, 0.35f));
                defaultEnchants.Add(new Enchant(4116, "Darkglow Embroidery (Rank 2)", ItemSlot.Back, enchantTemp, "spell_nature_giftofthewaterspirit"));
            }
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(4114, "Sanctified Spellthread (Rank 2)", ItemSlot.Legs, new Stats() { Intellect = 95, Spirit = 55 }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(4113, "Master's Spellthread (Rank 2)", ItemSlot.Legs, new Stats() { Intellect = 95f, Stamina = 80f }, "spell_nature_astralrecalgroup"));
            #endregion
            #endregion
            #region Level 80 (WotLK)
            #region Back
            {
                enchantTemp = new Stats() { Spirit = 1 };
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Intellect = 295 }, 15, 60, 0.35f));
                defaultEnchants.Add(new Enchant(3722, "Lightweave Embroidery", ItemSlot.Back, enchantTemp, "spell_arcane_prismaticcloak"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 400 }, 15, 55, 0.15f));
                defaultEnchants.Add(new Enchant(3730, "Swordguard Embroidery", ItemSlot.Back, enchantTemp, "ability_rogue_throwingspecialization"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { Spirit = 250 }, 1f, 60f, 0.35f));
                defaultEnchants.Add(new Enchant(3728, "Darkglow Embroidery", ItemSlot.Back, enchantTemp, "spell_nature_giftofthewaterspirit"));
            }
            #endregion
            #region Legs
            defaultEnchants.Add(new Enchant(3872, "Sanctified Spellthread (Rank 1)", ItemSlot.Legs, new Stats() { SpellPower = 50f, Spirit = 20f }, "spell_nature_astralrecalgroup"));
            defaultEnchants.Add(new Enchant(3873, "Master's Spellthread (Rank 1)", ItemSlot.Legs, new Stats() { SpellPower = 50f, Stamina = 30f }, "spell_nature_astralrecalgroup"));
            #endregion
            #endregion
            #endregion

            #region Death Knight Rune Enchants
            {
                enchantTemp = new Stats() { BonusFrostWeaponDamage = .02f };
                // Updated Razorice for patch 3.3.3
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.MeleeHit, new Stats() { BonusFrostDamageMultiplier = 0.02f }, 20f, 0f, 1f, 5));
                defaultEnchants.Add(new Enchant(3370, "Rune of Razorice", ItemSlot.OneHand, enchantTemp, "spell_frost_frostarmor"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { BonusStrengthMultiplier = .15f }, 15f, 0f, -2f, 1));
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { HealthRestoreFromMaxHealth = .03f }, 0, 0f, -2f, 1));
                defaultEnchants.Add(new Enchant(3368, "Rune of the Fallen Crusader", ItemSlot.OneHand, enchantTemp, "spell_holy_retributionaura"));
            }
            {
                enchantTemp = new Stats();
                enchantTemp.AddSpecialEffect(new SpecialEffect(Trigger.DamageDone, new Stats() { CinderglacierProc = 2f }, 0f, 0f, -1.5f));
                defaultEnchants.Add(new Enchant(3369, "Rune of Cinderglacier", ItemSlot.OneHand, enchantTemp, "spell_shadow_chilltouch"));
            }
            // Patch 4.0.6+  now reduce disarm duration by 60%, up from 50%.
            defaultEnchants.Add(new Enchant(3365, "Rune of Swordshattering", ItemSlot.TwoHand, new Stats() { Parry = 0.04f, DisarmDurReduc = 0.60f }, "ability_parry"));
            // Patch 4.0.6+  now reduce disarm duration by 60%, up from 50%.
            defaultEnchants.Add(new Enchant(3365, "Rune of Swordbreaking", ItemSlot.OneHand, new Stats() { Parry = 0.02f, DisarmDurReduc = 0.60f }, "ability_parry"));
            defaultEnchants.Add(new Enchant(3847, "Rune of the Stoneskin Gargoyle", ItemSlot.TwoHand, new Stats() { BaseArmorMultiplier = .04f, BonusArmorMultiplier = .04f, BonusStaminaMultiplier = 0.02f }, "inv_sword_130"));
            defaultEnchants.Add(new Enchant(3883, "Rune of the Nerubian Carapace", ItemSlot.OneHand, new Stats() { BaseArmorMultiplier = .02f, BonusArmorMultiplier = .02f, BonusStaminaMultiplier = 0.01f }, "inv_sword_61"));
            #endregion

            #region Armor Kits (Chest, Hands, Legs, Feet)
            #region Level 90 (MoP)
            defaultEnchants.Add(new Enchant(4869, "Sha Armor Kit", ItemSlot.Head, new Stats() { Stamina = 150 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4869, "Sha Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 150 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4869, "Sha Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 150 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4869, "Sha Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 150 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4869, "Sha Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 150 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4869, "Sha Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 150 }, "inv_misc_armorkit_28"));
            #endregion
            #region Level 85 (Cataclysm)
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Head, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4120, "Savage Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 36 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Head, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(4121, "Heavy Savage Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 44 }, "inv_misc_armorkit_30"));
            #endregion
            #region Level 80 (WotLK)
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Head, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3329, "Borean Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 12 }, "inv_misc_armorkit_28"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Head, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            defaultEnchants.Add(new Enchant(3330, "Heavy Borean Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 18 }, "inv_misc_armorkit_30"));
            #endregion
            #region Level 70 (BC)
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Head, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2792, "Knothide Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 8 }, "inv_misc_armorkit_24"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Head, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Chest, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Shoulders, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Legs, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Hands, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2841, "Heavy Knothide Armor Kit", ItemSlot.Feet, new Stats() { Stamina = 10 }, "inv_misc_armorkit_25"));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", ItemSlot.Chest, new Stats() { DodgeRating = 8 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", ItemSlot.Legs, new Stats() { DodgeRating = 8 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", ItemSlot.Hands, new Stats() { DodgeRating = 8 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(2793, "Vindicator's Armor Kit", ItemSlot.Feet, new Stats() { DodgeRating = 8 }, "inv_misc_armorkit_26"));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", ItemSlot.Chest, new Stats() { Spirit = 8 }, "inv_misc_armorkit_22"));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", ItemSlot.Legs, new Stats() { Spirit = 8 }, "inv_misc_armorkit_22"));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", ItemSlot.Hands, new Stats() { Spirit = 8 }, "inv_misc_armorkit_22"));
            defaultEnchants.Add(new Enchant(2794, "Magister's Armor Kit", ItemSlot.Feet, new Stats() { Spirit = 8 }, "inv_misc_armorkit_22"));
            //defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", ItemSlot.Chest, new Stats() { ShadowResistance = 8 }, "spell_shadow_antishadow"));
            //defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", ItemSlot.Legs, new Stats() { ShadowResistance = 8 }, "spell_shadow_antishadow"));
            //defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", ItemSlot.Hands, new Stats() { ShadowResistance = 8 }, "spell_shadow_antishadow"));
            //defaultEnchants.Add(new Enchant(2984, "Shadow Armor Kit", ItemSlot.Feet, new Stats() { ShadowResistance = 8 }, "spell_shadow_antishadow"));
            //defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", ItemSlot.Chest, new Stats() { FireResistance = 8 }, "spell_fire_sealoffire"));
            //defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", ItemSlot.Legs, new Stats() { FireResistance = 8 }, "spell_fire_sealoffire"));
            //defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", ItemSlot.Hands, new Stats() { FireResistance = 8 }, "spell_fire_sealoffire"));
            //defaultEnchants.Add(new Enchant(2985, "Flame Armor Kit", ItemSlot.Feet, new Stats() { FireResistance = 8 }, "spell_fire_sealoffire"));
            //defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", ItemSlot.Chest, new Stats() { FrostResistance = 8 }, "spell_frost_wizardmark"));
            //defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", ItemSlot.Legs, new Stats() { FrostResistance = 8 }, "spell_frost_wizardmark"));
            //defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", ItemSlot.Hands, new Stats() { FrostResistance = 8 }, "spell_frost_wizardmark"));
            //defaultEnchants.Add(new Enchant(2987, "Frost Armor Kit", ItemSlot.Feet, new Stats() { FrostResistance = 8 }, "spell_frost_wizardmark"));
            //defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", ItemSlot.Chest, new Stats() { NatureResistance = 8 }, "spell_nature_spiritarmor"));
            //defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", ItemSlot.Legs, new Stats() { NatureResistance = 8 }, "spell_nature_spiritarmor"));
            //defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", ItemSlot.Hands, new Stats() { NatureResistance = 8 }, "spell_nature_spiritarmor"));
            //defaultEnchants.Add(new Enchant(2988, "Nature Armor Kit", ItemSlot.Feet, new Stats() { NatureResistance = 8 }, "spell_nature_spiritarmor"));
            //defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", ItemSlot.Chest, new Stats() { ArcaneResistance = 8 }, "spell_shadow_sealofkings"));
            //defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", ItemSlot.Legs, new Stats() { ArcaneResistance = 8 }, "spell_shadow_sealofkings"));
            //defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", ItemSlot.Hands, new Stats() { ArcaneResistance = 8 }, "spell_shadow_sealofkings"));
            //defaultEnchants.Add(new Enchant(2989, "Arcane Armor Kit", ItemSlot.Feet, new Stats() { ArcaneResistance = 8 }, "spell_shadow_sealofkings"));
            #endregion
            // Level 60 (Vanilla)
            // Unsorted
            #endregion
            #region Vanilla & BC Enchants for both Head & Legs
            // Level 85 (Cataclysm)
            // Level 80 (WotLK)
            // Level 70 (BC)
            // Level 60 (Vanilla)
            defaultEnchants.Add(new Enchant(2583, "Presence of Might", ItemSlot.Head, new Stats() { Stamina = 10, DodgeRating = 10, ParryRating = 10 }, "spell_holy_sealofwrath"));
            defaultEnchants.Add(new Enchant(2583, "Presence of Might", ItemSlot.Legs, new Stats() { Stamina = 10, DodgeRating = 10, ParryRating = 10 }, "spell_holy_sealofwrath"));
            //defaultEnchants.Add(new Enchant(2543, "Arcanum of Rapidity", ItemSlot.Head, new Stats() { HasteRating = 10 }, "inv_misc_gem_02")); // Removed from game
            //defaultEnchants.Add(new Enchant(2543, "Arcanum of Rapidity", ItemSlot.Legs, new Stats() { HasteRating = 10 }, "inv_misc_gem_02")); // Removed from game
            //defaultEnchants.Add(new Enchant(1505, "Lesser Arcanum of Resilience", ItemSlot.Head, new Stats() { FireResistance = 20 }, "inv_misc_gem_03"));
            //defaultEnchants.Add(new Enchant(1505, "Lesser Arcanum of Resilience", ItemSlot.Legs, new Stats() { FireResistance = 20 }, "inv_misc_gem_03"));
            // Unsorted
            #endregion

            return defaultEnchants;
        }
    }
}
