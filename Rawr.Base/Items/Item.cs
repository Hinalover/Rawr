using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rawr
{
    #region ItemStatAllocation
    public class ItemStatAllocation
    {
        [XmlElement("Stat")]
        public AdditiveStat Stat { get; set; }

        [XmlElement("Allocation")]
        public int Allocation { get; set; }

        [XmlElement("SocketMultiplier")]
        public float SocketMultiplier { get; set; }
    }
    #endregion

    #region Item
    public class Item : IComparable<Item>
    {
        [XmlElement("ItemLevel")]
        public int _itemLevel;

        [XmlElement("DisplayId")]
        public int _displayId;

        [XmlElement("DisplaySlot")]
        public int _displaySlot;

        [XmlElement("IconPath")]
        public string _iconPath;

        [XmlElement("Stats")]
        public Stats _stats = new Stats();

        [XmlElement("ItemStatAllocations")]
        public List<ItemStatAllocation> ItemStatAllocations { get; set; }

        [XmlElement("Quality")]
        public ItemQuality _quality;

        [DefaultValueAttribute("")]
        [XmlElement("SetName")]
        public string _setName;
        
        [XmlElement("Type")]
        public ItemType _type = ItemType.None;

        [XmlElement("Faction")]
        public ItemFaction _faction = ItemFaction.Neutral;
        
        [DefaultValueAttribute(0)]
        [XmlElement("MinDamage")]
        public int _minDamage = 0;

        [DefaultValueAttribute(0)]
        [XmlElement("MaxDamage")]
        public int _maxDamage = 0;

        [DefaultValueAttribute(0)]
        [XmlElement("DamageType")]
        public ItemDamageType _damageType = ItemDamageType.Physical;

        [DefaultValueAttribute(0)]
        [XmlElement("Speed")]
        public float _speed = 0f;

        [DefaultValueAttribute("")]
        [XmlElement("RequiredClasses")]
        public string _requiredClasses;

        [DefaultValueAttribute(false)]
        [XmlElement("Unique")]
        public bool _unique;

        /// <summary>
        /// List of Ids that cannot be used together with this item (other than this item).
        /// Unique should be set to true if this is not empty.
        /// </summary>
        [XmlElement("UniqueId")]
        public List<int> UniqueId { get; set; }

        [DefaultValueAttribute(BindsOn.None)]
        [XmlElement("Bind")]
        public BindsOn _bind;

        [DefaultValueAttribute(0)]
        [XmlElement("SocketColor1")]
        public ItemSlot _socketColor1;

        [DefaultValueAttribute(0)]
        [XmlElement("SocketColor2")]
        public ItemSlot _socketColor2;

        [DefaultValueAttribute(0)]
        [XmlElement("SocketColor3")]
        public ItemSlot _socketColor3;

        //[DefaultValueAttribute(new Stats())]
        [XmlElement("SocketBonus")]
        public Stats _socketBonus = new Stats();

        [XmlElement("LocalizedName")]
        public string _localizedName;

        [XmlElement("AllowedRandomSuffix")]
        public List<int> AllowedRandomSuffixes { get; set; }

        [DefaultValueAttribute(0)]
        [XmlElement("UpgradeLevels")]
        public List<int> UpgradeLevels { get; set; }

        #region Location Infos
        [XmlIgnore]
        private bool LocaListPurged = false;
        private ItemLocationList LocationInfos = null;
        public ItemLocationList LocationInfo {
            get {
                if (LocationInfos == null  ) { LocationInfos = new ItemLocationList(); }
                if (LocationInfos.Count < 1) { LocationInfos.Add(new UnknownItem()); }
                if (Id != 0 && !LocaListPurged) {
                    // This should only be run once, it's designed to fix bad location lists
                    LocaListPurged = true;
                    int[] counts = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
                    for (int i = 0; i < LocationInfos.Count; ) {
                        /* We don't actually want to do this now
                        if (LocationInfos[i] is NoSource) {
                            // Replace the NoSource with the Unknown Source
                            LocationInfos[i] = new UnknownItem();
                        }*/
                        if (LocationInfos[i] == null) { LocationInfos.RemoveAt(i); continue; } // Don't iterate
                        else if (LocationInfos[i] is StaticDrop     ) { counts[ 0]++; }
                        else if (LocationInfos[i] is NoSource       ) { counts[ 1]++; }
                        else if (LocationInfos[i] is UnknownItem    ) { counts[ 2]++;
                            if (counts[2] > 1) {
                                LocationInfos.RemoveAt(i);
                                counts[2]--;
                                continue;
                                /*Don't iterate*/
                            }
                        }
                        else if (LocationInfos[i] is WorldDrop      ) { counts[ 3]++; }
                        else if (LocationInfos[i] is PvpItem        ) { counts[ 4]++; }
                        else if (LocationInfos[i] is VendorItem     ) { counts[ 5]++; }
                        else if (LocationInfos[i] is FactionItem    ) { counts[ 6]++; }
                        else if (LocationInfos[i] is CraftedItem    ) { counts[ 7]++; }
                        else if (LocationInfos[i] is QuestItem      ) { counts[ 8]++; }
                        else if (LocationInfos[i] is AchievementItem) { counts[ 9]++; }
                        else if (LocationInfos[i] is ContainerItem  ) { counts[10]++; }
                        i++; // Iterate
                    }
                    while (LocationInfos.Count > 1 && (counts[1] + counts[2]) > 0)
                    {
                        for (int i = 0; i < LocationInfos.Count; i++)
                        {
                            bool removeIt = false;
                            if (LocationInfos[i] is NoSource   ) { counts[1]--; removeIt = true; }
                            if (LocationInfos[i] is UnknownItem) { counts[2]--; removeIt = true; }
                            if (removeIt) { LocationInfos.RemoveAt(i); }
                        }
                    }
                }
                return LocationInfos;
            }
            set {
                LocationInfos = value;
                if (LocationInfos == null) { LocationInfos = new ItemLocationList(); }
                if (LocationInfos.Count < 1 && Id != 0) { LocationInfos.Add(new UnknownItem()); }
                if (Id != 0 && !LocaListPurged) {
                    // This should only be run once, it's designed to fix bad location lists
                    LocaListPurged = true;
                    int[] counts = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
                    for (int i = 0; i < LocationInfos.Count; ) {
                        if (LocationInfos[i] is NoSource) {
                            // Replace the NoSource with the Unknown Source
                            LocationInfos[i] = new UnknownItem();
                        }
                        if (LocationInfos[i] == null) { LocationInfos.RemoveAt(i); continue; } // Don't iterate
                        else if (LocationInfos[i] is StaticDrop     ) { counts[ 0]++; }
                        else if (LocationInfos[i] is NoSource       ) { counts[ 1]++; }
                        else if (LocationInfos[i] is UnknownItem    ) { counts[ 2]++;
                            if (counts[2] > 1) {
                                LocationInfos.RemoveAt(i);
                                counts[2]--;
                                continue;
                                /*Don't iterate*/
                            }
                        }
                        else if (LocationInfos[i] is WorldDrop      ) { counts[ 3]++; }
                        else if (LocationInfos[i] is PvpItem        ) { counts[ 4]++; }
                        else if (LocationInfos[i] is VendorItem     ) { counts[ 5]++; }
                        else if (LocationInfos[i] is FactionItem    ) { counts[ 6]++; }
                        else if (LocationInfos[i] is CraftedItem    ) { counts[ 7]++; }
                        else if (LocationInfos[i] is QuestItem      ) { counts[ 8]++; }
                        else if (LocationInfos[i] is AchievementItem) { counts[ 9]++; }
                        else if (LocationInfos[i] is ContainerItem  ) { counts[10]++; }
                        i++; // Iterate
                    }
                    while (LocationInfos.Count > 1 && (counts[1] + counts[2]) > 0) {
                        for (int i = 0; i < LocationInfos.Count; i++) {
                            bool removeIt = false;
                            if (LocationInfos[i] is NoSource   ) { counts[1]--; removeIt = true; }
                            if (LocationInfos[i] is UnknownItem) { counts[2]--; removeIt = true; }
                            if (removeIt) { LocationInfos.RemoveAt(i); }
                        }
                    }
                }
            }
        }
        public string GetFullLocationDesc {
            get {
                string retVal = "";
                if (LocationInfo.Count > 1) {
                    bool first = true;
                    foreach (ItemLocation il in LocationInfo) {
                        if (il == null) { continue; }
                        if (!first) { retVal += " and "; }
                        retVal += il.Description;
                        first = false;
                    }
                } else {
                    retVal = LocationInfo[0].Description;
                }
                return retVal;
            }
        }
        #endregion

        /// <summary>Cost of acquiring the item (i.e. badges, dkp, gold, etc.)</summary>
        [DefaultValueAttribute(0.0f)]
        public float Cost { get; set; }

        [XmlIgnore]
        public DateTime LastChange { get; set; }

        public void InvalidateCachedData()
        {
            Stats.InvalidateSparseData();
            SocketBonus.InvalidateSparseData();
            LastChange = DateTime.Now;
        }

        [XmlIgnore]
        public bool Invalid { get; set; }

        public void Delete()
        {
            Invalid = true;
        }

        private string _name;
        [XmlIgnore]
        public string Name
        {
            get {
                if (_localizedName != null && !Rawr.Properties.GeneralSettings.Default.Locale.Equals("en")) {
                    return _localizedName;
                } else {
                    return _name;
                }
            }
            set { _name = value; UpdateGemInformation(); }
        }

        [XmlElement("Name")]
        public string EnglishName {
            get { return _name; }
            set { 
                _name = value;
                UpdateGemInformation();
            }
        }

        private int _id;
        public int Id {
            get { return _id; }
            set {
                _id = value;
                UpdateGemInformation();
            }
        }

        [XmlIgnore]
        public int ItemLevel
        {
            get { return _itemLevel; }
            set
            {
                _itemLevel = value;
            }
        }
        [XmlIgnore]
        public int DisplayId
        {
            get { return _displayId; }
            set
            {
                _displayId = value;
            }
        }
        [XmlIgnore]
        public int DisplaySlot
        {
            get { return _displaySlot; }
            set
            {
                _displaySlot = value;
            }
        }
        [XmlIgnore]
        public int SlotId
        {
            get
            {
                switch (_slot)
                {
                    case ItemSlot.Head: return 1;
                    case ItemSlot.Neck: return 2;
                    case ItemSlot.Shoulders: return 3;
                    case ItemSlot.Shirt: return 4;
                    case ItemSlot.Chest: return 5;
                    case ItemSlot.Waist: return 6;
                    case ItemSlot.Legs: return 7;
                    case ItemSlot.Feet: return 8;
                    case ItemSlot.Wrist: return 9;
                    case ItemSlot.Hands: return 10;
                    case ItemSlot.Finger: return 11;
                    case ItemSlot.Trinket: return 13;
                    case ItemSlot.Back: return 15;
                    case ItemSlot.TwoHand:
                    case ItemSlot.OneHand:
                    case ItemSlot.Ranged:
                    case ItemSlot.MainHand: return 16;
                    case ItemSlot.OffHand: return 17;
                    case ItemSlot.Tabard: return 19;
                    default: return 0;
                }
            }
        }
        [XmlIgnore]
        public string IconPath
        {
            get { return (_iconPath == null ? null : _iconPath.ToLower(System.Globalization.CultureInfo.InvariantCulture)); }
            set { _iconPath = value == null ? null : value.ToLower(System.Globalization.CultureInfo.InvariantCulture); }
        }

        private ItemSlot _slot;
        public ItemSlot Slot
        {
            get { return _slot; }
            set {
                _slot = value;
                UpdateGemInformation();
            }
        }

        /// <summary>
        /// String version of Slot, to facilitate databinding.
        /// </summary>
        [XmlIgnore]
        public string SlotString
        {
            get { return _slot.ToString(); }
            set 
            { 
                _slot = (ItemSlot)Enum.Parse(typeof(ItemSlot), value, false);
                UpdateGemInformation();
            }
        }

        [XmlIgnore]
        public Stats Stats
        {
            get { return _stats; }
            set { _stats = value; }
        }
        [XmlIgnore]
        public ItemQuality Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }
        [XmlIgnore]
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        [XmlIgnore]
        public ItemFaction Faction
        {
            get { return _faction; }
            set { _faction = value; }
        }
        [XmlIgnore]
        public float DropRate {
            get {
                if (LocationInfo.Count > 0) {
                    foreach (ItemLocation il in LocationInfo) {
                        if (il is StaticDrop && (il as StaticDrop).DropPerc > 0f) {
                            return (il as StaticDrop).DropPerc;
                        }
                    }
                }
                return 0f;
            }
        }

        public bool FitsFaction(CharacterRace race)
        {
            bool fitsFaction = true;
            if (Faction != ItemFaction.Neutral)
            {
                switch (race)
                {
                    case CharacterRace.Draenei:
                    case CharacterRace.Dwarf:
                    case CharacterRace.Gnome:
                    case CharacterRace.Human:
                    case CharacterRace.NightElf:
                    case CharacterRace.Worgen:
                    case CharacterRace.PandarenAlliance:
                        fitsFaction &= Faction == ItemFaction.Alliance;
                        break;

                    default:
                        fitsFaction &= Faction == ItemFaction.Horde;
                        break;
                }
            }
            return fitsFaction;
        }

        /// <summary>
        /// String version of Faction, to facilitate databinding
        /// </summary>
        [XmlIgnore]
        public string FactionString
        {
            get { return _faction.ToString(); }
            set { _faction = (ItemFaction)Enum.Parse(typeof(ItemFaction), value, false); }
        }
        [XmlIgnore]
        public ItemType Type {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// String version of Type, to facilitate databinding
        /// </summary>
        [XmlIgnore]
        public string TypeString {
            get { return _type.ToString(); }
            set { _type = (ItemType)Enum.Parse(typeof(ItemType), value, false); }
        }
        [XmlIgnore]
        public int MinDamage {
            get { return _minDamage; }
            set { _minDamage = value; }
        }
        [XmlIgnore]
        public int MaxDamage {
            get { return _maxDamage; }
            set { _maxDamage = value; }
        }
        [XmlIgnore]
        public ItemDamageType DamageType {
            get { return _damageType; }
            set { _damageType = value; }
        }
        [XmlIgnore]
        public float Speed {
            get { return _speed; }
            set { _speed = value; }
        }
        [XmlIgnore]
        public float DPS {
            get {
                if (Speed == 0f) return 0f;
                else return ((float)(MinDamage + MaxDamage) * 0.5f) / Speed;
            }
        }
        [XmlIgnore]
        public string RequiredClasses {
            get { return _requiredClasses; }
            set { _requiredClasses = value; }
        }
        [XmlIgnore]
        public bool Unique {
            get { return _unique; }
            set { _unique = value; }
        }
        [XmlIgnore]
        public BindsOn Bind
        {
            get { return _bind; }
            set { _bind = value; }
        }
        [XmlIgnore]
        public ItemSlot SocketColor1
        {
            get { return _socketColor1; }
            set { _socketColor1 = value; }
        }
        [XmlIgnore]
        public ItemSlot SocketColor2
        {
            get { return _socketColor2; }
            set { _socketColor2 = value; }
        }
        [XmlIgnore]
        public ItemSlot SocketColor3
        {
            get { return _socketColor3; }
            set { _socketColor3 = value; }
        }
        public ItemSlot GetSocketColor(int index)
        {
            switch (index)
            {
                case 1:
                    return SocketColor1;
                case 2:
                    return SocketColor2;
                case 3:
                    return SocketColor3;
                default:
                    return ItemSlot.None;
            }
        }
        [XmlIgnore]
        public string SocketColor1String
        {
            get { return _socketColor1.ToString(); }
            set { _socketColor1 = (ItemSlot)Enum.Parse(typeof(ItemSlot), value, false); }
        }
        [XmlIgnore]
        public string SocketColor2String
        {
            get { return _socketColor2.ToString(); }
            set { _socketColor2 = (ItemSlot)Enum.Parse(typeof(ItemSlot), value, false); }
        }
        [XmlIgnore]
        public string SocketColor3String
        {
            get { return _socketColor3.ToString(); }
            set { _socketColor3 = (ItemSlot)Enum.Parse(typeof(ItemSlot), value, false); }
        }
        [XmlIgnore]
        public Stats SocketBonus
        {
            get { return _socketBonus; }
            set { _socketBonus = value; }
        }
        [XmlIgnore]
        public string LocalizedName
        {
            get { return _localizedName; }
            set { _localizedName = value; }
        }

        public static int GetSlotIdbyCharacterSlot(CharacterSlot slot)
        {
            switch (slot)
            {
                case CharacterSlot.Head: return 1;
                case CharacterSlot.Neck: return 2;
                case CharacterSlot.Shoulders: return 3;
                case CharacterSlot.Shirt: return 4;
                case CharacterSlot.Chest: return 5;
                case CharacterSlot.Waist: return 6;
                case CharacterSlot.Legs: return 7;
                case CharacterSlot.Feet: return 8;
                case CharacterSlot.Wrist: return 9;
                case CharacterSlot.Hands: return 10;
                case CharacterSlot.Finger1: return 11;
                case CharacterSlot.Finger2: return 12;
                case CharacterSlot.Trinket1: return 13;
                case CharacterSlot.Trinket2: return 14;
                case CharacterSlot.Back: return 15;
                case CharacterSlot.MainHand: return 16;
                case CharacterSlot.OffHand: return 17;
                //case CharacterSlot.Ranged: return 18;
                case CharacterSlot.Tabard: return 19;
                default: return 0;
            }
        }

        public static ItemSlot GetItemSlotByCharacterSlot(CharacterSlot slot)
        {
            switch (slot)
            {
                //case CharacterSlot.Projectile: return ItemSlot.Projectile;
                case CharacterSlot.Head: return ItemSlot.Head;
                case CharacterSlot.Neck: return ItemSlot.Neck;
                case CharacterSlot.Shoulders: return ItemSlot.Shoulders;
                case CharacterSlot.Chest: return ItemSlot.Chest;
                case CharacterSlot.Waist: return ItemSlot.Waist;
                case CharacterSlot.Legs: return ItemSlot.Legs;
                case CharacterSlot.Feet: return ItemSlot.Feet;
                case CharacterSlot.Wrist: return ItemSlot.Wrist;
                case CharacterSlot.Hands: return ItemSlot.Hands;
                case CharacterSlot.Finger1: return ItemSlot.Finger;
                case CharacterSlot.Finger2: return ItemSlot.Finger;
                case CharacterSlot.Trinket1: return ItemSlot.Trinket;
                case CharacterSlot.Trinket2: return ItemSlot.Trinket;
                case CharacterSlot.Back: return ItemSlot.Back;
                case CharacterSlot.MainHand: return ItemSlot.MainHand;
                case CharacterSlot.OffHand: return ItemSlot.OffHand;
                //case CharacterSlot.Ranged: return ItemSlot.Ranged;
                //case CharacterSlot.ProjectileBag: return ItemSlot.ProjectileBag;
                //case CharacterSlot.ExtraWristSocket: return ItemSlot.Prismatic;
                //case CharacterSlot.ExtraHandsSocket: return ItemSlot.Prismatic;
                //case CharacterSlot.ExtraWaistSocket: return ItemSlot.Prismatic;
                case CharacterSlot.Tabard: return ItemSlot.Tabard;
                case CharacterSlot.Shirt: return ItemSlot.Shirt;
                case CharacterSlot.Gems: return ItemSlot.Prismatic;
                case CharacterSlot.Metas: return ItemSlot.Meta;
                case CharacterSlot.Cogwheels: return ItemSlot.Cogwheel;
                case CharacterSlot.Hydraulics: return ItemSlot.Hydraulic;
                default: return ItemSlot.None;
            }
        }

        private bool _isGem;
        public bool IsGem
        {
            get
            {
                return _isGem;
            }
        }

        private bool _isRedGem;
        public bool IsRedGem
        {
            get
            {
                return _isRedGem;
            }
        }

        private bool _isYellowGem;
        public bool IsYellowGem
        {
            get
            {
                return _isYellowGem;
            }
        }

        private bool _isBlueGem;
        public bool IsBlueGem
        {
            get
            {
                return _isBlueGem;
            }
        }

        private bool _isCogwheel;
        public bool IsCogwheel { get { return _isCogwheel; } }

        private bool _isHydraulic;
        public bool IsHydraulic { get { return _isHydraulic; } }

        private bool _isJewelersGem;
        public bool IsJewelersGem
        {
            get
            {
                return _isJewelersGem;
            }
        }

        private bool _isJewelersFacet;
        public bool IsJewelersFacet
        {
            get
            {
                return _isJewelersFacet;
            }
        }

        public bool IsLimitedGem
        {
            get
            {
                return _isGem && (_isJewelersGem || Unique);
            }
        }

        internal static bool IsJewelersGemId(int id)
        {
            return (
            #region Blue
            #region Cataclysm
            id == 52264 || // Rigid Chimera's Eye
            id == 52261 || // Solid Chimera's Eye
            id == 52262 || // Sparkling Chimera's Eye
            id == 52263 || // Stormy Chimera's Eye
            #endregion
            #region Wrath
            id == 42156 || // Rigid Dragon's Eye
            id == 36767 || // Solid Dragon's Eye
            id == 42145 || // Sparkling Dragon's Eye
            id == 42146 || // Sparkling Dragon's Eye
            id == 42155 || // Stormy Dragon's Eye
            #endregion
            #endregion
            #region Red
            #region Cataclysm
            id == 52255 || // Bold Chimera's Eye
            id == 52257 || // Brilliant Chimera's Eye
            id == 52258 || // Delicate Chimera's Eye
            id == 52259 || // Flashing Chimera's Eye
            id == 52260 || // Precise Chimera's Eye
            #endregion
            #region Wrath
            id == 42142 || // Bold Dragon's Eye
            id == 42144 || // Brilliant Dragon's Eye
            id == 42148 || // Brilliant Dragon's Eye
            id == 36766 || // Delicate Dragon's Eye
            id == 42143 || // Delicate Dragon's Eye
            id == 42152 || // Flashing Dragon's Eye
            id == 42154 || // Precise Dragon's Eye
            #endregion
            #endregion
            #region Yellow
            #region Cataclysm
            id == 52269 || // Fractured Chimera's Eye
            id == 52267 || // Mystic Chimera's Eye
            id == 52268 || // Quick Chimera's Eye
            id == 52266 || // Smooth Chimera's Eye
            id == 52265 || // Subtle Chimera's Eye
            #endregion
            #region Wrath
            id == 42158 || // Mystic Dragon's Eye
            id == 42150 || // Quick Dragon's Eye
            id == 42149 || // Smooth Dragon's Eye
            id == 42153 || // Smooth Dragon's Eye
            id == 42151 || // Subtle Dragon's Eye
            id == 42157    // Subtle Dragon's Eye
            #endregion
            #endregion
            );
        }

        internal static bool IsJewelersFacetId(int id)
        {
            return (
            #region Blue
            #region MoP
            id == 83144 || // Rigid Serpent's EyeStormy
            id == 83148 || // Solid Serpent's Eye
            id == 83149 || // Sparkling Serpent's Eye
                //id == 52263 || // Stormy Serpent's Eye
            #endregion
            #endregion
            #region Red
            #region MoP
            id == 83141 || // Bold Serpent's Eye
            id == 83150 || // Brilliant Serpent's Eye
            id == 83151 || // Delicate Serpent's Eye
            id == 83152 || // Flashing Serpent's Eye
            id == 83147 || // Precise Serpent's Eye
            #endregion
            #endregion
            #region Yellow
            #region MoP
            id == 83143 || // Fractured Serpent's Eye
                //id == 52267 || // Mystic Serpent's Eye
            id == 83142 || // Quick Serpent's Eye
            id == 83146 || // Smooth Serpent's Eye
            id == 83145  // Subtle Serpent's Eye
            #endregion
            #endregion
            );
        }

        private void UpdateGemInformation()
        {
            _isGem = Slot == ItemSlot.Meta
                || Slot == ItemSlot.Blue || Slot == ItemSlot.Red || Slot == ItemSlot.Yellow
                || Slot == ItemSlot.Green || Slot == ItemSlot.Orange || Slot == ItemSlot.Purple
                || Slot == ItemSlot.Prismatic
                || Slot == ItemSlot.Cogwheel || Slot == ItemSlot.Hydraulic;
            _isJewelersGem = IsJewelersGemId(Id);
            _isJewelersFacet = IsJewelersFacetId(Id);
            _isRedGem = _isGem && Item.GemMatchesSlot(this, ItemSlot.Red);
            _isYellowGem = _isGem && Item.GemMatchesSlot(this, ItemSlot.Yellow);
            _isBlueGem = _isGem && Item.GemMatchesSlot(this, ItemSlot.Blue);
            _isCogwheel = _isGem && Item.GemMatchesSlot(this, ItemSlot.Cogwheel);
            _isHydraulic = _isGem && Item.GemMatchesSlot(this, ItemSlot.Hydraulic);
        }

        public Item()
        {
            UpgradeLevels = new List<int>();
        }
        public Item(string name, ItemQuality quality, ItemType type, int id, string iconPath, ItemSlot slot, string setName, bool unique, Stats stats, Stats socketBonus, ItemSlot socketColor1, ItemSlot socketColor2, ItemSlot socketColor3, int minDamage, int maxDamage, ItemDamageType damageType, float speed, string requiredClasses)
        {
            _name = name;
            _id = id;
            _iconPath = iconPath;
            _slot = slot;
            _stats = stats;
            _socketBonus = socketBonus;
            _socketColor1 = socketColor1;
            _socketColor2 = socketColor2;
            _socketColor3 = socketColor3;
            _setName = setName;
            _quality = quality;
            _type = type;
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            _damageType = damageType;
            _speed = speed;
            _requiredClasses = requiredClasses;
            _unique = unique;
            UpgradeLevels = new List<int>();
            UpdateGemInformation();
        }

        public Item Clone()
        {
            return new Item()
            {
                Name = this.Name,
                Quality = this.Quality,
                Id = this.Id,
                ItemLevel = this.ItemLevel,
                IconPath = this.IconPath,
                Slot = this.Slot,
                Stats = this.Stats.Clone(),
                SocketBonus = this.SocketBonus.Clone(),
                SocketColor1 = this.SocketColor1,
                SocketColor2 = this.SocketColor2,
                SocketColor3 = this.SocketColor3,
                SetName = this.SetName,
                Type = this.Type,
                MinDamage = this.MinDamage,
                MaxDamage = this.MaxDamage,
                DamageType = this.DamageType,
                Speed = this.Speed,
                RequiredClasses = this.RequiredClasses,
                Unique = this.Unique,
                UniqueId = new List<int>(this.UniqueId ?? (this.UniqueId = new List<int>() { })),
                LocalizedName = this.LocalizedName,
                UpgradeLevels = new List<int>(this.UpgradeLevels ?? (this.UpgradeLevels = new List<int>() { }))
            };
        }

        public override string ToString()
        {
            return (!string.IsNullOrEmpty(this.Name) ? this.Name : this.Id.ToString("00000")) + ": " + 
                (this.Bind != BindsOn.None ? (this.Bind + " ") : string.Empty) +
                this.Stats.ToString();
        }

        public static bool GemMatchesSlot(Item gem, ItemSlot slotColor)
        {
            switch (slotColor)
            {
                case ItemSlot.Red:
                    return gem != null && (gem.Slot == ItemSlot.Red || gem.Slot == ItemSlot.Orange || gem.Slot == ItemSlot.Purple || gem.Slot == ItemSlot.Prismatic);
                case ItemSlot.Yellow:
                    return gem != null && (gem.Slot == ItemSlot.Yellow || gem.Slot == ItemSlot.Orange || gem.Slot == ItemSlot.Green || gem.Slot == ItemSlot.Prismatic);
                case ItemSlot.Blue:
                    return gem != null && (gem.Slot == ItemSlot.Blue || gem.Slot == ItemSlot.Green || gem.Slot == ItemSlot.Purple || gem.Slot == ItemSlot.Prismatic);
                case ItemSlot.Meta:
                    return gem != null && (gem.Slot == ItemSlot.Meta);
                case ItemSlot.Cogwheel:
                    return gem != null && (gem.Slot == ItemSlot.Cogwheel);
                case ItemSlot.Hydraulic:
                    return gem != null && (gem.Slot == ItemSlot.Hydraulic);
                default:
                    return gem == null || gem.Slot != ItemSlot.Meta;
            }
        }

        public static Dictionary<ItemSlot, CharacterSlot> DefaultSlotMap { get; private set; }
        static Item()
        {
            Dictionary<ItemSlot, CharacterSlot> list = new Dictionary<ItemSlot, CharacterSlot>();
            foreach (ItemSlot iSlot in EnumHelper.GetValues(typeof(ItemSlot)))
            {
                list[iSlot] = CharacterSlot.None;
            }
            list[ItemSlot.Head] = CharacterSlot.Head;
            list[ItemSlot.Neck] = CharacterSlot.Neck;
            list[ItemSlot.Shoulders] = CharacterSlot.Shoulders;
            list[ItemSlot.Back] = CharacterSlot.Back;
            list[ItemSlot.Chest] = CharacterSlot.Chest;
            list[ItemSlot.Shirt] = CharacterSlot.Shirt;
            list[ItemSlot.Tabard] = CharacterSlot.Tabard;
            list[ItemSlot.Wrist] = CharacterSlot.Wrist;
            list[ItemSlot.Hands] = CharacterSlot.Hands;
            list[ItemSlot.Waist] = CharacterSlot.Waist;
            list[ItemSlot.Legs] = CharacterSlot.Legs;
            list[ItemSlot.Feet] = CharacterSlot.Feet;
            list[ItemSlot.Finger] = CharacterSlot.Finger1;
            list[ItemSlot.Trinket] = CharacterSlot.Trinket1;
            list[ItemSlot.OneHand] = CharacterSlot.MainHand;
            list[ItemSlot.TwoHand] = CharacterSlot.MainHand;
            list[ItemSlot.MainHand] = CharacterSlot.MainHand;
            list[ItemSlot.Ranged] = CharacterSlot.MainHand;
            list[ItemSlot.OffHand] = CharacterSlot.OffHand;
            list.OrderBy(kvp => (int)kvp.Key);
            DefaultSlotMap = list;
        }

        public bool FitsInSlot(CharacterSlot charSlot)
        {
            //And if I fell with all the strength I held inside...
            switch (charSlot)
            {
                case CharacterSlot.Head:
                    return this.Slot == ItemSlot.Head;
                case CharacterSlot.Neck:
                    return this.Slot == ItemSlot.Neck;
                case CharacterSlot.Shoulders:
                    return this.Slot == ItemSlot.Shoulders;
                case CharacterSlot.Back:
                    return this.Slot == ItemSlot.Back;
                case CharacterSlot.Chest:
                    return this.Slot == ItemSlot.Chest;
                case CharacterSlot.Shirt:
                    return this.Slot == ItemSlot.Shirt;
                case CharacterSlot.Tabard:
                    return this.Slot == ItemSlot.Tabard;
                case CharacterSlot.Wrist:
                    return this.Slot == ItemSlot.Wrist;
                case CharacterSlot.Hands:
                    return this.Slot == ItemSlot.Hands;
                case CharacterSlot.Waist:
                    return this.Slot == ItemSlot.Waist;
                case CharacterSlot.Legs:
                    return this.Slot == ItemSlot.Legs;
                case CharacterSlot.Feet:
                    return this.Slot == ItemSlot.Feet;
                case CharacterSlot.Finger1:
                case CharacterSlot.Finger2:
                    return this.Slot == ItemSlot.Finger;
                case CharacterSlot.Trinket1:
                case CharacterSlot.Trinket2:
                    return this.Slot == ItemSlot.Trinket;
                case CharacterSlot.MainHand:
                    return this.Slot == ItemSlot.TwoHand || this.Slot == ItemSlot.OneHand || this.Slot == ItemSlot.MainHand || this.Slot == ItemSlot.Ranged;
                case CharacterSlot.OffHand:
                    return this.Slot == ItemSlot.OneHand || this.Slot == ItemSlot.OffHand;
                case CharacterSlot.Cogwheels:
                    return this.Slot == ItemSlot.Cogwheel;
                case CharacterSlot.Hydraulics:
                    return this.Slot == ItemSlot.Hydraulic;
                case CharacterSlot.Gems:
                    return this.Slot == ItemSlot.Red || this.Slot == ItemSlot.Blue || this.Slot == ItemSlot.Yellow
                        || this.Slot == ItemSlot.Purple || this.Slot == ItemSlot.Green || this.Slot == ItemSlot.Orange
                        || this.Slot == ItemSlot.Prismatic;
                case CharacterSlot.Metas:
                    return this.Slot == ItemSlot.Meta;
                default:
                    return false;
            }
            //I wouldn't be out here... alone tonight
        }

        public bool FitsInSlot(CharacterSlot charSlot, Character character)
        {
            return Calculations.ItemFitsInSlot(this, character, charSlot, false);
        }

        public bool FitsInSlot(CharacterSlot charSlot, Character character, bool ignoreUnique)
        {
            return Calculations.ItemFitsInSlot(this, character, charSlot, ignoreUnique);
        }

        public bool MeetsRequirements(Character character)
        {
            bool temp;
            return MeetsRequirements(character, out temp);
        }

        public static bool ItemsAreConsideredUniqueEqual(Item itema, Item itemb)
        {
            return (object)itema != null && (object)itemb != null && itema.Unique && (itema.Id == itemb.Id || (itema.UniqueId != null && itema.UniqueId.Contains(itemb.Id)));
        }

        public static bool OptimizerManagedVolatiliy { get; set; }

        /// <summary>
        /// Checks whether item meets the requirements to activate its stats.
        /// </summary>
        /// <param name="character">Character for which we are checking the requirements.</param>
        /// <param name="volatileRequirements">This is set to true for items that depend on requirements not local to the item itself.</param>
        /// <returns>True if the item meets the requirements.</returns>
        public bool MeetsRequirements(Character character, out bool volatileRequirements)
        {
            volatileRequirements = false;
            bool meetsRequirements = true;

            if (this.Slot == ItemSlot.Meta)
            {
				return true;
                #region Metagem Requirements
                volatileRequirements = true;
                if (character == null
                    || !Rawr.Properties.GeneralSettings.Default.EnforceGemRequirements
                    || !Rawr.Properties.GeneralSettings.Default.EnforceGemRequirements_Meta
                    ) { return true; }

                int redGems = 0, yellowGems = 0, blueGems = 0;
                if (character != null)
                {
                    /*redGems = character.GetGemColorCount(ItemSlot.Red);
                    yellowGems = character.GetGemColorCount(ItemSlot.Yellow);
                    blueGems = character.GetGemColorCount(ItemSlot.Blue);*/
                    redGems = character.RedGemCount;
                    yellowGems = character.YellowGemCount;
                    blueGems = character.BlueGemCount;
                }

                //TODO: Make this dynamic, by loading the gem requirements from the armory
                switch (this.Id)
                {
                    case 34220: // Chaotic Skyfire Diamond            (3% Crit Dmg, 12 Crit Rating)
                    case 41285: // Chaotic Skyflare Diamond           (3% Crit Dmg, 21 Crit Rating)
                    case 52291: // Chaotic Shadowspirit Diamond       (3% Crit Dmg, 54 Crit Rating)
                    case 68778: // Agile Shadowspirit Diamond         (3% Crit Dmg, 54 Agility)
                    case 68779: // Reverberating Shadowspirit Diamond (3% Crit Dmg, 54 Strength)
                    case 68780: // Burning Shadowspirit Diamond       (3% Crit Dmg, 54 Intellect)
                    case 32409: // Relentless Earthsiege Diamond (3% Crit Dmg, 12 Agility)
                    case 41398: // Relentless Earthsiege Diamond (3% Crit Dmg, 21 Agility)
                        volatileRequirements = true;
                        meetsRequirements = redGems >= 3;
                        break;
                    case 25899:
                    case 25890:
                    case 25901:
                    case 32410:
                        volatileRequirements = true; //2 of each
                        meetsRequirements = redGems >= 2 && yellowGems >= 2 && blueGems >= 2;
                        break;
                    case 41307:
                    case 41401:
                    case 41400:
                    case 41375:
                    case 44078:
                    case 44089:
                    case 41382:
                        volatileRequirements = true; //1 of each
                        meetsRequirements = redGems >= 1 && yellowGems >= 1 && blueGems >= 1;
                        break;
                    case 25897:
                        volatileRequirements = true; //More reds than blues
                        meetsRequirements = redGems > blueGems;
                        break;
                    case 25895:
                        volatileRequirements = true; //More reds than yellows
                        meetsRequirements = redGems > yellowGems;
                        break;
                    case 25893:
                    case 32640:
                        volatileRequirements = true; //More blues than yellows
                        meetsRequirements = blueGems > yellowGems;
                        break;
                    case 52299: // Powerful Shadowspirit Diamond
                        volatileRequirements = true; //2 blues
                        meetsRequirements = blueGems >= 2;
                        break;
                    case 41376:
                    case 52298: // Destructive Shadowspirit Diamond
                        volatileRequirements = true; //2 reds
                        meetsRequirements = redGems >= 2;
                        break;
                    case 52289: // Fleet Shadowspirit Diamond
                    case 52294: // Austere Shadowspirit Diamond
                    case 52296: // Ember Shadowspirit Diamond
                        volatileRequirements = true; //2 yellows
                        meetsRequirements = yellowGems >= 2;
                        break;
                    case 25896:
                    case 44087:
                    case 52293: // Eternal Shadowspirit Diamond
                        volatileRequirements = true; //3 blues
                        meetsRequirements = blueGems >= 3;
                        break;
                    case 25898:
                        volatileRequirements = true; //5 blues
                        meetsRequirements = blueGems >= 5;
                        break;
                    case 32641:
                        volatileRequirements = true; //exactly 3 yellows
                        meetsRequirements = yellowGems == 3;
                        break;
                    case 41333:
                        volatileRequirements = true; //3 red
                        meetsRequirements = redGems >= 3;
                        break;
                    case 25894:
                    case 28557:
                    case 28556:
                    case 41339:
                    case 44076:
                        volatileRequirements = true; //2 yellows, 1 red
                        meetsRequirements = yellowGems >= 2 && redGems >= 1;
                        break;
                    case 35501:
                    case 44088:
                        volatileRequirements = true; //1 yellow, 2 blue
                        meetsRequirements = yellowGems >= 1 && blueGems >= 2;
                        break;
                    case 41378:
                    case 44084:
                    case 41381:
                        volatileRequirements = true; //2 yellow, 1 blue
                        meetsRequirements = yellowGems >= 2 && blueGems >= 1;
                        break;
                    case 52292: // Bracing Shadowspirit Diamond
                    case 52297: // Revitalizing Shadowspirit Diamond
                    case 52300: // Enigmatic Shadowspirit Diamond
                    case 52301: // Impassive Shadowspirit Diamond
                        volatileRequirements = true; //1 blue, 1 yellow
                        meetsRequirements = blueGems >= 1 && yellowGems >= 1;
                        break;
                    case 52302: // Forlorn Shadowspirit Diamond
                        volatileRequirements = true; //1 blue, 1 red
                        meetsRequirements = blueGems >= 1 && redGems >= 1;
                        break;
                    case 41380:
                    case 41377:
                    case 44082:
                    case 41385:
                        volatileRequirements = true; //2 blue, 1 red
                        meetsRequirements = blueGems >= 2 && redGems >= 1;
                        break;
                    case 52295: // Effulgent Shadowspirit Diamond
                        volatileRequirements = true; //1 red, 1 yellow
                        meetsRequirements = redGems >= 1 && yellowGems >= 1;
                        break;
                    case 41335:
                    case 41389:
                        volatileRequirements = true; //2 red, 1 yellow
                        meetsRequirements = redGems >= 2 && yellowGems >= 1;
                        break;
                    case 41379:
                    case 44081:
                    case 41396:
                    case 41395:
                        volatileRequirements = true; //2 red, 1 blue
                        meetsRequirements = redGems >= 2 && blueGems >= 1;
                        break;
                    default:
                        meetsRequirements = true;
                        break;
                }
                #endregion
            }
            else if (!OptimizerManagedVolatiliy)
            {
                #region Gem Requirements
                /*if (IsJewelersGem)
                {
                    volatileRequirements = true;
                    if (character == null
                    || !Rawr.Properties.GeneralSettings.Default.EnforceGemRequirements
                    || !Rawr.Properties.GeneralSettings.Default.EnforceGemRequirements_JC
                    ) { return true; }
                    meetsRequirements = character.JewelersGemCount <= 3;
                }
                else if (IsJewelersFacet)
                {
                    volatileRequirements = true;
                    if (character == null
                    || !Rawr.Properties.GeneralSettings.Default.EnforceGemRequirements
                    || !Rawr.Properties.GeneralSettings.Default.EnforceGemRequirements_JC
                    ) { return true; }
                    meetsRequirements = character.JewelersFacetCount <= 2;
                }
                else*/ if (Unique || IsCogwheel || IsHydraulic)
                {
                    volatileRequirements = true;
                    if (character == null
                    || !Rawr.Properties.GeneralSettings.Default.EnforceGemRequirements
                    || !Rawr.Properties.GeneralSettings.Default.EnforceGemRequirements_Unique
                    ) { return true; }
                    meetsRequirements = character.GetGemIdCount(Id) <= 1;
                }
                else
                {
                    volatileRequirements = false;
                    meetsRequirements = true;
                }
                #endregion
            }

            return meetsRequirements;
        }

        public static Item LoadFromId(int id) { return LoadFromId(id, false, true, false, false); }
        public static Item LoadFromId(int id, bool forceRefresh, bool raiseEvent, bool useWowhead, bool usePTR) { return LoadFromId(id, forceRefresh, raiseEvent, useWowhead, usePTR, Rawr.Properties.GeneralSettings.Default.Locale); }
        public static Item LoadFromId(int id, bool forceRefresh, bool raiseEvent, bool useWowhead, bool usePTR, string locale) { return LoadFromId(id, forceRefresh, raiseEvent, useWowhead, usePTR, locale, "cata"); }
        public static Item LoadFromId(int id, bool forceRefresh, bool raiseEvent, bool useWowhead, bool usePTR, string locale, string wowheadSite)
        {
            Item cachedItem = ItemCache.FindItemById(id);
            #if DEBUG
            string oldItemStats = "";
            string oldItemSource = "";
            List<ItemLocation> oldItemLoc = null;
            if (cachedItem != null && forceRefresh){
                oldItemStats  = cachedItem.ToString().Split(':')[1];
                oldItemLoc    = cachedItem.LocationInfo;
                oldItemSource = cachedItem.GetFullLocationDesc;
            }
            #endif

            if (cachedItem != null && !forceRefresh) return cachedItem;
            else if (useWowhead)
            {
                WowheadService wowheadService = new WowheadService();
                wowheadService.GetItemCompleted += new EventHandler<EventArgs<Item>>(wowheadService_GetItemCompleted);
                wowheadService.GetItemAsync(id, usePTR);

                if (cachedItem != null) return cachedItem;
                else
                {
                    Item tempItem = new Item();
                    tempItem.Name = "[Downloading from Wowhead]";
                    tempItem.Id = id;
                    ItemCache.AddItem(tempItem, raiseEvent);
                    return tempItem;
                }
            }
            else
            {
                Rawr4ArmoryService armoryService = new Rawr4ArmoryService();
                armoryService.GetItemCompleted += new EventHandler<EventArgs<Item>>(armoryService_GetItemCompleted);
                armoryService.GetItemAsync(id);
                
                if (cachedItem != null) return cachedItem;
                else
                {
                    Item tempItem = new Item();
                    tempItem.Name = "[Item Not Found - Automatic Armory Item Lookups Coming Soon]";
                    tempItem.Id = id;
                    ItemCache.AddItem(tempItem, raiseEvent);
                    return tempItem;
                }
            }
        }

        private static void armoryService_GetItemCompleted(object sender, EventArgs<Item> e)
        {
            if (e.Value != null)
                ItemCache.AddItem(e.Value, true);
            ((Rawr4ArmoryService)sender).GetItemCompleted -= new EventHandler<EventArgs<Item>>(armoryService_GetItemCompleted);
        }

        private static void wowheadService_GetItemCompleted(object sender, EventArgs<Item> e)
        {
            if (e.Value != null)
                ItemCache.AddItem(e.Value, true);
            ((WowheadService)sender).GetItemCompleted -= new EventHandler<EventArgs<Item>>(wowheadService_GetItemCompleted);
        }

        /// <summary>Used by optimizer</summary>
        [XmlIgnore]
        internal List<Optimizer.ItemAvailabilityInformation> AvailabilityInformation;

        #region IComparable<Item> Members

        public int CompareTo(Item other)
        {
            return ToString().CompareTo(other.ToString());
        }

        #endregion
    }
    #endregion

    #region ItemInstance
    // to make our lives more tolerable, ItemInstance is exactly what it implies
    // it is a single instance of an item, it is not to be shared between multiple characters
    // or whatever, at least if you don't know what you are doing
    // if for whatever reason you reuse the same instance treat it as read only
    public class ItemInstance : IComparable<ItemInstance>
    {
        [XmlElement("Id")]
        public int _id;
        [DefaultValueAttribute(0)]
        [XmlElement("Gem1Id")]
        public int _gem1Id;
        [DefaultValueAttribute(0)]
        [XmlElement("Gem2Id")]
        public int _gem2Id;
        [DefaultValueAttribute(0)]
        [XmlElement("Gem3Id")]
        public int _gem3Id;
        [DefaultValueAttribute(0)]
        [XmlElement("EnchantId")]
        public int _enchantId;
        [DefaultValueAttribute(0)]
        [XmlElement("TinkeringId")]
        public int _tinkeringId;
        [DefaultValueAttribute(0)]
        [XmlElement("RandomSuffixId")]
        public int _randomSuffixId;
        [DefaultValueAttribute(0)]
        [XmlElement("UpgradeLevel")]
        public int _upgradeLevel;

        // Used by optimizer
        [XmlIgnore]
        internal Optimizer.ItemAvailabilityInformation ItemAvailabilityInformation { get; set; }

        [XmlIgnore]
        public int Id
        {
            get { return _id; }
            set { _id = value; OnIdsChanged(); }
        }
        [XmlIgnore]
        public int Gem1Id
        {
            get { return _gem1Id; }
            set { _gem1Id = value; OnIdsChanged(); }
        }
        [XmlIgnore]
        public int Gem2Id
        {
            get { return _gem2Id; }
            set { _gem2Id = value; OnIdsChanged(); }
        }
        [XmlIgnore]
        public int Gem3Id
        {
            get { return _gem3Id; }
            set { _gem3Id = value; OnIdsChanged(); }
        }
        [XmlIgnore]
        public int EnchantId
        {
            get { return _enchantId; }
            set { _enchantId = value; OnIdsChanged(); }
        }
        [XmlIgnore]
        public int TinkeringId
        {
            get { return _tinkeringId; }
            set { _tinkeringId = value; OnIdsChanged(); }
        }
        [DefaultValueAttribute(0)]
        public int ReforgeId
        {
            get 
            {
                if (Reforging != null)
                {
                    return Reforging.Id;
                }
                return 0; 
            }
            set
            {
                if (Reforging == null)
                {
                    Reforging = new Reforging();
                }
                Reforging.Id = value;
                OnIdsChanged();
            }
        }
        [XmlIgnore]
        public int RandomSuffixId
        {
            get { return _randomSuffixId; }
            set { _randomSuffixId = value; OnIdsChanged(); }
        }

        [XmlIgnore]
        public int UpgradeLevel
        {
            get { return _upgradeLevel; }
            set { _upgradeLevel = value; OnIdsChanged(); }
        }

        private void UpdateJewelerCount()
        {
            int jewelerCount = 0;
            if (Item.IsJewelersGemId(_gem1Id)) jewelerCount++;
            if (Item.IsJewelersGemId(_gem2Id)) jewelerCount++;
            if (Item.IsJewelersGemId(_gem3Id)) jewelerCount++;
            if (Item.IsJewelersFacetId(_gem1Id)) jewelerCount++;
            if (Item.IsJewelersFacetId(_gem2Id)) jewelerCount++;
            if (Item.IsJewelersFacetId(_gem3Id)) jewelerCount++;
            JewelerCount = jewelerCount;
        }

        [XmlIgnore]
        public int JewelerCount { get; private set; }

        [DefaultValueAttribute(false)]
        public bool ForceDisplay { get; set; }

        public event EventHandler IdsChanged;
        private void OnIdsChanged()
        {
            _gemmedId = string.Empty;
            InvalidateCachedData();
            UpdateJewelerCount();
            if (Reforging != null)
            {
                Reforging.ApplyReforging(Item, RandomSuffixId, UpgradeLevel);
            }
            
            if (IdsChanged != null) IdsChanged(this, null);
        }

        [XmlIgnore]
        private Item _itemCached = null;
        [XmlIgnore]
        public Item Item
        {
            get
            {
                if (Id <= 0) return null;
                if (_itemCached == null || _itemCached.Id != Id || _itemCached.Invalid)
                {
                    // don't need to raise event, it only triggers when item is not present and just for the blank item
                    // the real event when item is loaded from web asynchronously is always triggered
                    _itemCached = Item.LoadFromId(Id, false, false, true, false); // changed to Wowhead until we have battle.net parsing working
                    //_itemCached = Item.LoadFromId(Id);
                    // after a temp item is replaced when we fetch data from wowhead the old one will be marked invalid
                    // make sure to clear cached data and recalculate reforging with new info
                    InvalidateCachedData();
                    if (Reforging != null)
                    {
                        Reforging.ApplyReforging(_itemCached, RandomSuffixId, UpgradeLevel);
                    }
                }
                return _itemCached;
            }
            set
            {
                _itemCached = value;
                if (value == null)
                    Id = 0;
                else
                    Id = value.Id;
            }
        }

        [XmlIgnore]
        private Item _gem1Cached = null;
        [XmlIgnore]
        public Item Gem1
        {
            get
            {
                if (Gem1Id == 0) return null;
                if (_gem1Cached == null || _gem1Cached.Id != Gem1Id || _gem1Cached.Invalid)
                {
                    _gem1Cached = Item.LoadFromId(Gem1Id);
                }
                return _gem1Cached;
            }
            set
            {
                _gem1Cached = value;
                if (value == null)
                    Gem1Id = 0;
                else
                    Gem1Id = value.Id;
            }
        }

        [XmlIgnore]
        private Item _gem2Cached = null;
        [XmlIgnore]
        public Item Gem2
        {
            get
            {
                if (Gem2Id == 0) return null;
                if (_gem2Cached == null || _gem2Cached.Id != Gem2Id || _gem2Cached.Invalid)
                {
                    _gem2Cached = Item.LoadFromId(Gem2Id);
                }
                return _gem2Cached;
            }
            set
            {
                _gem2Cached = value;
                if (value == null)
                    Gem2Id = 0;
                else
                    Gem2Id = value.Id;
            }
        }

        [XmlIgnore]
        private Item _gem3Cached = null;
        [XmlIgnore]
        public Item Gem3
        {
            get
            {
                if (Gem3Id == 0) return null;
                if (_gem3Cached == null || _gem3Cached.Id != Gem3Id || _gem3Cached.Invalid)
                {
                    _gem3Cached = Item.LoadFromId(Gem3Id);
                }
                return _gem3Cached;
            }
            set
            {
                _gem3Cached = value;
                if (value == null)
                    Gem3Id = 0;
                else
                    Gem3Id = value.Id;
            }
        }

        [XmlIgnore]
        private Enchant _enchantCached = null;
        [XmlIgnore]
        public Enchant Enchant
        {
            get
            {
                if (_enchantCached == null || _enchantCached.Id != EnchantId)
                {
                    _enchantCached = Enchant.FindEnchant(EnchantId, Item != null ? Item.Slot : ItemSlot.None, null);
                }
                return _enchantCached;
            }
            set
            {
                _enchantCached = value;
                if (value == null)
                    EnchantId = 0;
                else
                    EnchantId = value.Id;
            }
        }

        [XmlIgnore]
        private Tinkering _tinkeringCached = null;
        [XmlIgnore]
        public Tinkering Tinkering
        {
            get
            {
                if (_tinkeringCached == null || _tinkeringCached.Id != TinkeringId)
                {
                    _tinkeringCached = Tinkering.FindTinkering(TinkeringId, Item != null ? Item.Slot : ItemSlot.None, null);
                }
                return _tinkeringCached;
            }
            set
            {
                _tinkeringCached = value;
                if (value == null)
                    TinkeringId = 0;
                else
                    TinkeringId = value.Id;
            }
        }

        [XmlIgnore]
        private Reforging _reforging;
        [XmlIgnore]
        public Reforging Reforging
        {
            get
            {
                return _reforging;
            }
            set
            {
                _reforging = value;
                OnIdsChanged();
            }
        }

        // 1-based index
        public Item GetGem(int index)
        {
            switch (index)
            {
                case 1:
                    return Gem1;
                case 2:
                    return Gem2;
                case 3:
                    return Gem3;
                default:
                    return null;
            }
        }

        public void SetGem(int index, Item value)
        {
            switch (index)
            {
                case 1:
                    Gem1 = value;
                    break;
                case 2:
                    Gem2 = value;
                    break;
                case 3:
                    Gem3 = value;
                    break;
            }
        }

        [XmlIgnore]
        private string _gemmedId = string.Empty;
        [XmlIgnore]
        public string GemmedId
        {
            get
            {
                if (_gemmedId.Length == 0) // _gemmedId is never null
                {
                    _gemmedId = string.Format("{0}.{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}",
                        Id, RandomSuffixId, Gem1Id, Gem2Id, Gem3Id, EnchantId, ReforgeId, TinkeringId, UpgradeLevel);
                }
                return _gemmedId;
            }
            set
            {
                if (value == null) _gemmedId = string.Empty;
                else _gemmedId = value;
                string[] ids = _gemmedId.Split('.');
                if (ids.Length == 7)
                {
                    // gemmed id without random suffix or upgrade level
                    ids = new string[] { ids[0], "0", ids[1], ids[2], ids[3], ids[4], ids[5], ids[6], "0" };
                }
                if (ids.Length == 8)
                {
                    // gemmed id without upgrade level
                    ids = new string[] { ids[0], ids[1], ids[2], ids[3], ids[4], ids[5], ids[6], ids[7], "0" };
                }
                _id = int.Parse(ids[0]);
                _randomSuffixId = ids.Length > 1 ? int.Parse(ids[1]) : 0;
                _gem1Id = ids.Length > 2 ? int.Parse(ids[2]) : 0;
                _gem2Id = ids.Length > 3 ? int.Parse(ids[3]) : 0;
                _gem3Id = ids.Length > 4 ? int.Parse(ids[4]) : 0;
                _enchantId = ids.Length > 5 ? int.Parse(ids[5]) : 0;
                _tinkeringId = ids.Length > 7 ? int.Parse(ids[7]) : 0;
                ReforgeId = ids.Length > 6 ? int.Parse(ids[6]) : 0;
                UpgradeLevel = ids.Length > 8 ? int.Parse(ids[8]) : 0;
                OnIdsChanged();
            }
        }

        [XmlIgnore]
        public string SuffixId
        {
            get
            {
                return Id + "." + RandomSuffixId;
            }
        }

        public ItemInstance() { }
        public ItemInstance(string gemmedId)
        {
            string[] ids = gemmedId.Split('.');
            if (ids.Length == 7)
            {
                // gemmed id without random suffix or upgrade level
                ids = new string[] { ids[0], "0", ids[1], ids[2], ids[3], ids[4], ids[5], ids[6], "0" };
            }
            if (ids.Length == 8)
            {
                // gemmed id without upgrade level
                ids = new string[] { ids[0], ids[1], ids[2], ids[3], ids[4], ids[5], ids[6], ids[7], "0" };
            }
            _id = int.Parse(ids[0]);
            _randomSuffixId = ids.Length > 1 ? int.Parse(ids[1]) : 0;
            _gem1Id = ids.Length > 2 ? int.Parse(ids[2]) : 0;
            _gem2Id = ids.Length > 3 ? int.Parse(ids[3]) : 0;
            _gem3Id = ids.Length > 4 ? int.Parse(ids[4]) : 0;
            _enchantId = ids.Length > 5 ? int.Parse(ids[5]) : 0;
            _tinkeringId = ids.Length > 7 ? int.Parse(ids[7]) : 0;
            UpdateJewelerCount();
            UpgradeLevel = ids.Length > 8 ? int.Parse(ids[8]) : 0;
            ReforgeId = ids.Length > 6 ? int.Parse(ids[6]) : 0;
            if (Reforging != null)
            {
                Reforging.ApplyReforging(Item, RandomSuffixId, UpgradeLevel);
            }
        }
        public ItemInstance(int id, int randomSuffixId, int gem1Id, int gem2Id, int gem3Id, int enchantId, int reforgeId, int tinkeringId, int upgradeLevel = 0)
        {
            _id = id;
            _gem1Id = gem1Id;
            _gem2Id = gem2Id;
            _gem3Id = gem3Id;
            _enchantId = enchantId;
            _tinkeringId = tinkeringId;
            _randomSuffixId = randomSuffixId;
            UpdateJewelerCount();
            _upgradeLevel = upgradeLevel;
            _reforging = new Reforging(Item, randomSuffixId, _upgradeLevel, reforgeId);
        }
        public ItemInstance(Item item, int randomSuffixId, Item gem1, Item gem2, Item gem3, Enchant enchant, Reforging reforging, Tinkering tinkering, int upgradeLevel = 0)
        {
            // this code path is used a lot, optimize for performance
            _itemCached = item;
            _gem1Cached = gem1;
            _gem2Cached = gem2;
            _gem3Cached = gem3;
            _enchantCached = enchant;
            _id = item != null ? item.Id : 0;
            _gem1Id = gem1 != null ? gem1.Id : 0;
            _gem2Id = gem2 != null ? gem2.Id : 0;
            _gem3Id = gem3 != null ? gem3.Id : 0;
            _enchantId = enchant != null ? enchant.Id : 0;
            _tinkeringId = tinkering != null ? tinkering.Id : 0;
            _randomSuffixId = randomSuffixId;
            _upgradeLevel = upgradeLevel;
            _reforging = reforging;
            OnIdsChanged();
        }

        public ItemInstance Clone()
        {
            return new ItemInstance()
            {
                Item = this.Item,
                Gem1 = this.Gem1,
                Gem2 = this.Gem2,
                Gem3 = this.Gem3,
                Enchant = this.Enchant,
                Reforging = this.Reforging == null ? null : this.Reforging.Clone(),
                Tinkering = this.Tinkering,
                RandomSuffixId = this.RandomSuffixId,
                ItemAvailabilityInformation = this.ItemAvailabilityInformation, // batch tools relies on this
                UpgradeLevel = this.UpgradeLevel,
				// cachedTotalStats are never modified after first calculated, it's safe to reuse them, on any change to the clone we'll recalculate
				cachedTime = this.cachedTime,
				cachedTotalStats = this.cachedTotalStats
            };
        }

        public override string ToString()
        {
            string summary;
            summary = this.Item.Name;
            if (RandomSuffixId != 0)
            {
                summary += " " + RandomSuffix.GetSuffix(RandomSuffixId);
            }
            summary += ": ";
            summary += this.GetTotalStats().ToString();
            //summary += Stats.ToString();
            //summary += Sockets.ToString();
            if (summary.EndsWith(", ")) summary = summary.Substring(0, summary.Length - 2);

            if ((Item.SocketColor1 != ItemSlot.None && Gem1Id == 0) ||
                (Item.SocketColor2 != ItemSlot.None && Gem2Id == 0) ||
                (Item.SocketColor3 != ItemSlot.None && Gem3Id == 0))
                summary += " [EMPTY SOCKETS]";

            return summary;
        }

        public string ToItemString()
        {            
            // Blizzard itemString format is
            // item:itemId:enchantId:jewelId1:jewelId2:jewelId3:jewelId4:suffixId:uniqueId:linkLevel:reforgeId
            int reforge = ReforgeId + 56;
            return "item:" + this.Id + ":" + EnchantId + ":" + 
                GemIDConverter.ConvertGemItemIDToEnchantID(Gem1Id) + ":" + 
                GemIDConverter.ConvertGemItemIDToEnchantID(Gem2Id) + ":" + 
                GemIDConverter.ConvertGemItemIDToEnchantID(Gem3Id) + ":0:" +
                RandomSuffixId +
                ":0:0:" + reforge;
        }

        public bool MatchesSocketBonus
        {
            get
            {
                Item item = Item;
                return Item.GemMatchesSlot(Gem1, item.SocketColor1) &&
                       Item.GemMatchesSlot(Gem2, item.SocketColor2) &&
                       Item.GemMatchesSlot(Gem3, item.SocketColor3);
            }
        }

        public int SlotId
        {
            get
            {
                return Item == null ? 0 : Item.SlotId;
            }
        }

        public int DisplayId
        {
            get
            {
                return Item == null ? 0 : Item.DisplayId;
            }
        }

        public int DisplaySlot
        {
            get
            {
                return Item == null ? 0 : Item.DisplaySlot;
            }
        }

        public static void AccumulateUpgradeStats(Stats stats, Item item, int randomSuffixId, int upgradeItemLevel)
        {
            if (item.ItemStatAllocations != null && item.ItemStatAllocations.Count > 0)
            {
                AccumulateUpgradeStatsDBC(stats, item, randomSuffixId, upgradeItemLevel);
            }
            else
            {
                AccumulateUpgradeStatsSimple(stats, item, randomSuffixId, upgradeItemLevel);
            }
        }

        private static int GetSlotType(ItemSlot itemSlot)
        {
            switch (itemSlot)
            {
                case ItemSlot.TwoHand:
                case ItemSlot.Ranged:
                    return 0;
                case ItemSlot.MainHand:
                case ItemSlot.OneHand:
                    return 3;
                case ItemSlot.Head:
                case ItemSlot.Chest:
                case ItemSlot.Legs:
                    return 0;
                case ItemSlot.Shoulders:
                case ItemSlot.Waist:
                case ItemSlot.Feet:
                case ItemSlot.Hands:
                case ItemSlot.Trinket:
                    return 1;
                case ItemSlot.Neck:
                case ItemSlot.OffHand:
                case ItemSlot.Finger:
                case ItemSlot.Back:
                case ItemSlot.Wrist:
                    return 2;
                default:
                    return -1;
            }
        }

        #region BudgetData
        private static int[,,] BudgetData = new int[1000, 3, 5] {
             { {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 } },
             { {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 } },
             { {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 } },
             { {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 } },
             { {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 } },
             { {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 } },
             { {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 } },
             { {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 } },
             { {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 }, {    0,    0,    0,    0,    0 } },
             { {    9,    7,    5,    4,    3 }, {    5,    4,    3,    2,    2 }, {    1,    1,    1,    1,    1 } },
             { {   10,    8,    6,    4,    3 }, {    6,    5,    3,    3,    2 }, {    2,    2,    1,    1,    1 } },
             { {   10,    8,    6,    4,    3 }, {    6,    5,    3,    3,    2 }, {    2,    2,    1,    1,    1 } },
             { {   11,    8,    6,    5,    3 }, {    7,    5,    4,    3,    2 }, {    3,    2,    2,    1,    1 } },
             { {   11,    8,    6,    5,    3 }, {    7,    5,    4,    3,    2 }, {    3,    2,    2,    1,    1 } },
             { {   12,    9,    7,    5,    4 }, {    8,    6,    5,    3,    3 }, {    4,    3,    2,    2,    1 } },
             { {   12,    9,    7,    5,    4 }, {    8,    6,    5,    3,    3 }, {    4,    3,    2,    2,    1 } },
             { {   13,   10,    7,    5,    4 }, {    9,    7,    5,    4,    3 }, {    5,    4,    3,    2,    2 } },
             { {   13,   10,    7,    5,    4 }, {    9,    7,    5,    4,    3 }, {    5,    4,    3,    2,    2 } },
             { {   14,   11,    8,    6,    4 }, {   10,    8,    6,    4,    3 }, {    6,    5,    3,    3,    2 } },
             { {   14,   11,    8,    6,    4 }, {   10,    8,    6,    4,    3 }, {    6,    5,    3,    3,    2 } },
             { {   15,   11,    8,    6,    5 }, {   11,    8,    6,    5,    3 }, {    7,    5,    4,    3,    2 } },
             { {   15,   11,    8,    6,    5 }, {   11,    8,    6,    5,    3 }, {    7,    5,    4,    3,    2 } },
             { {   16,   12,    9,    7,    5 }, {   12,    9,    7,    5,    4 }, {    8,    6,    5,    3,    3 } },
             { {   16,   12,    9,    7,    5 }, {   12,    9,    7,    5,    4 }, {    8,    6,    5,    3,    3 } },
             { {   17,   13,   10,    7,    5 }, {   13,   10,    7,    5,    4 }, {    9,    7,    5,    4,    3 } },
             { {   17,   13,   10,    7,    5 }, {   13,   10,    7,    5,    4 }, {    9,    7,    5,    4,    3 } },
             { {   18,   14,   10,    8,    6 }, {   14,   11,    8,    6,    4 }, {   10,    8,    6,    4,    3 } },
             { {   18,   14,   10,    8,    6 }, {   14,   11,    8,    6,    4 }, {   10,    8,    6,    4,    3 } },
             { {   19,   14,   11,    8,    6 }, {   15,   11,    8,    6,    5 }, {   11,    8,    6,    5,    3 } },
             { {   19,   14,   11,    8,    6 }, {   15,   11,    8,    6,    5 }, {   11,    8,    6,    5,    3 } },
             { {   20,   15,   11,    8,    6 }, {   16,   12,    9,    7,    5 }, {   12,    9,    7,    5,    4 } },
             { {   20,   15,   11,    8,    6 }, {   16,   12,    9,    7,    5 }, {   12,    9,    7,    5,    4 } },
             { {   21,   16,   12,    9,    7 }, {   17,   13,   10,    7,    5 }, {   13,   10,    7,    5,    4 } },
             { {   21,   16,   12,    9,    7 }, {   17,   13,   10,    7,    5 }, {   13,   10,    7,    5,    4 } },
             { {   22,   17,   12,    9,    7 }, {   18,   14,   10,    8,    6 }, {   14,   11,    8,    6,    4 } },
             { {   22,   17,   12,    9,    7 }, {   18,   14,   10,    8,    6 }, {   14,   11,    8,    6,    4 } },
             { {   23,   17,   13,   10,    7 }, {   19,   14,   11,    8,    6 }, {   15,   11,    8,    6,    5 } },
             { {   23,   17,   13,   10,    7 }, {   19,   14,   11,    8,    6 }, {   15,   11,    8,    6,    5 } },
             { {   24,   18,   14,   10,    8 }, {   20,   15,   11,    8,    6 }, {   16,   12,    9,    7,    5 } },
             { {   24,   18,   14,   10,    8 }, {   20,   15,   11,    8,    6 }, {   16,   12,    9,    7,    5 } },
             { {   25,   19,   14,   11,    8 }, {   21,   16,   12,    9,    7 }, {   17,   13,   10,    7,    5 } },
             { {   25,   19,   14,   11,    8 }, {   21,   16,   12,    9,    7 }, {   17,   13,   10,    7,    5 } },
             { {   26,   20,   15,   11,    8 }, {   22,   17,   12,    9,    7 }, {   18,   14,   10,    8,    6 } },
             { {   26,   20,   15,   11,    8 }, {   22,   17,   12,    9,    7 }, {   18,   14,   10,    8,    6 } },
             { {   27,   20,   15,   11,    9 }, {   23,   17,   13,   10,    7 }, {   19,   14,   11,    8,    6 } },
             { {   27,   20,   15,   11,    9 }, {   23,   17,   13,   10,    7 }, {   19,   14,   11,    8,    6 } },
             { {   28,   21,   16,   12,    9 }, {   24,   18,   14,   10,    8 }, {   20,   15,   11,    8,    6 } },
             { {   28,   21,   16,   12,    9 }, {   24,   18,   14,   10,    8 }, {   20,   15,   11,    8,    6 } },
             { {   29,   22,   16,   12,    9 }, {   25,   19,   14,   11,    8 }, {   21,   16,   12,    9,    7 } },
             { {   29,   22,   16,   12,    9 }, {   25,   19,   14,   11,    8 }, {   21,   16,   12,    9,    7 } },
             { {   30,   23,   17,   13,    9 }, {   26,   20,   15,   11,    8 }, {   22,   17,   12,    9,    7 } },
             { {   30,   23,   17,   13,    9 }, {   26,   20,   15,   11,    8 }, {   22,   17,   12,    9,    7 } },
             { {   31,   23,   17,   13,   10 }, {   27,   20,   15,   11,    9 }, {   23,   17,   13,   10,    7 } },
             { {   31,   23,   17,   13,   10 }, {   27,   20,   15,   11,    9 }, {   23,   17,   13,   10,    7 } },
             { {   32,   24,   18,   14,   10 }, {   28,   21,   16,   12,    9 }, {   24,   18,   14,   10,    8 } },
             { {   32,   24,   18,   14,   10 }, {   28,   21,   16,   12,    9 }, {   24,   18,   14,   10,    8 } },
             { {   33,   25,   19,   14,   10 }, {   29,   22,   16,   12,    9 }, {   25,   19,   14,   11,    8 } },
             { {   33,   25,   19,   14,   10 }, {   29,   22,   16,   12,    9 }, {   25,   19,   14,   11,    8 } },
             { {   34,   26,   19,   14,   11 }, {   30,   23,   17,   13,    9 }, {   26,   20,   15,   11,    8 } },
             { {   34,   26,   19,   14,   11 }, {   30,   23,   17,   13,    9 }, {   26,   20,   15,   11,    8 } },
             { {   35,   26,   20,   15,   11 }, {   31,   23,   17,   13,   10 }, {   27,   20,   15,   11,    9 } },
             { {   35,   26,   20,   15,   11 }, {   31,   23,   17,   13,   10 }, {   27,   20,   15,   11,    9 } },
             { {   36,   27,   20,   15,   11 }, {   32,   24,   18,   14,   10 }, {   28,   21,   16,   12,    9 } },
             { {   36,   27,   20,   15,   11 }, {   32,   24,   18,   14,   10 }, {   28,   21,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   25,   19,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   32,   24,   18,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   33,   25,   19,   14,   10 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   34,   26,   19,   14,   11 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   34,   26,   19,   14,   11 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   35,   26,   20,   15,   11 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   36,   27,   20,   15,   11 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   36,   27,   20,   15,   11 }, {   29,   22,   16,   12,    9 } },
             { {   37,   28,   21,   16,   12 }, {   37,   28,   21,   16,   12 }, {   30,   23,   17,   13,    9 } },
             { {   37,   28,   21,   16,   12 }, {   38,   29,   21,   16,   12 }, {   30,   23,   17,   13,    9 } },
             { {   37,   28,   21,   16,   12 }, {   38,   29,   21,   16,   12 }, {   31,   23,   17,   13,   10 } },
             { {   38,   29,   21,   16,   12 }, {   39,   29,   22,   16,   12 }, {   32,   24,   18,   14,   10 } },
             { {   38,   29,   21,   16,   12 }, {   39,   29,   22,   16,   12 }, {   32,   24,   18,   14,   10 } },
             { {   39,   29,   22,   16,   12 }, {   39,   29,   22,   16,   12 }, {   33,   25,   19,   14,   10 } },
             { {   39,   29,   22,   16,   12 }, {   39,   29,   22,   16,   12 }, {   34,   26,   19,   14,   11 } },
             { {   40,   30,   23,   17,   13 }, {   39,   29,   22,   16,   12 }, {   34,   26,   19,   14,   11 } },
             { {   40,   30,   23,   17,   13 }, {   39,   29,   22,   16,   12 }, {   35,   26,   20,   15,   11 } },
             { {   40,   30,   23,   17,   13 }, {   40,   30,   23,   17,   13 }, {   35,   26,   20,   15,   11 } },
             { {   41,   31,   23,   17,   13 }, {   40,   30,   23,   17,   13 }, {   35,   26,   20,   15,   11 } },
             { {   41,   31,   23,   17,   13 }, {   40,   30,   23,   17,   13 }, {   35,   26,   20,   15,   11 } },
             { {   42,   32,   24,   18,   13 }, {   40,   30,   23,   17,   13 }, {   35,   26,   20,   15,   11 } },
             { {   42,   32,   24,   18,   13 }, {   41,   31,   23,   17,   13 }, {   35,   26,   20,   15,   11 } },
             { {   43,   32,   24,   18,   14 }, {   41,   31,   23,   17,   13 }, {   36,   27,   20,   15,   11 } },
             { {   43,   32,   24,   18,   14 }, {   41,   31,   23,   17,   13 }, {   36,   27,   20,   15,   11 } },
             { {   44,   33,   25,   19,   14 }, {   41,   31,   23,   17,   13 }, {   36,   27,   20,   15,   11 } },
             { {   44,   33,   25,   19,   14 }, {   41,   31,   23,   17,   13 }, {   36,   27,   20,   15,   11 } },
             { {   44,   33,   25,   19,   14 }, {   41,   31,   23,   17,   13 }, {   36,   27,   20,   15,   11 } },
             { {   45,   34,   25,   19,   14 }, {   41,   31,   23,   17,   13 }, {   36,   27,   20,   15,   11 } },
             { {   45,   34,   25,   19,   14 }, {   41,   31,   23,   17,   13 }, {   37,   28,   21,   16,   12 } },
             { {   46,   35,   26,   19,   15 }, {   41,   31,   23,   17,   13 }, {   37,   28,   21,   16,   12 } },
             { {   46,   35,   26,   19,   15 }, {   42,   32,   24,   18,   13 }, {   37,   28,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   42,   32,   24,   18,   13 }, {   37,   28,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   37,   28,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   37,   28,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   37,   28,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   37,   28,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   37,   28,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   37,   28,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   43,   32,   24,   18,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   38,   29,   21,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   39,   29,   22,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   39,   29,   22,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   39,   29,   22,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   39,   29,   22,   16,   12 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   40,   30,   23,   17,   13 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   40,   30,   23,   17,   13 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   40,   30,   23,   17,   13 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   41,   31,   23,   17,   13 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   41,   31,   23,   17,   13 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   42,   32,   24,   18,   13 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   42,   32,   24,   18,   13 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   43,   32,   24,   18,   14 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   43,   32,   24,   18,   14 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   44,   33,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   44,   33,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   45,   34,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   45,   34,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   45,   34,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   45,   34,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   44,   33,   25,   19,   14 }, {   45,   34,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   45,   34,   25,   19,   14 }, {   45,   34,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   45,   34,   25,   19,   14 }, {   45,   34,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   46,   35,   26,   19,   15 }, {   45,   34,   25,   19,   14 } },
             { {   47,   35,   26,   20,   15 }, {   46,   35,   26,   19,   15 }, {   46,   35,   26,   19,   15 } },
             { {   47,   35,   26,   20,   15 }, {   47,   35,   26,   20,   15 }, {   46,   35,   26,   19,   15 } },
             { {   47,   35,   26,   20,   15 }, {   47,   35,   26,   20,   15 }, {   46,   35,   26,   19,   15 } },
             { {   47,   35,   26,   20,   15 }, {   48,   36,   27,   20,   15 }, {   46,   35,   26,   19,   15 } },
             { {   48,   36,   27,   20,   15 }, {   48,   36,   27,   20,   15 }, {   46,   35,   26,   19,   15 } },
             { {   48,   36,   27,   20,   15 }, {   49,   37,   28,   21,   16 }, {   46,   35,   26,   19,   15 } },
             { {   48,   36,   27,   20,   15 }, {   49,   37,   28,   21,   16 }, {   46,   35,   26,   19,   15 } },
             { {   48,   36,   27,   20,   15 }, {   49,   37,   28,   21,   16 }, {   46,   35,   26,   19,   15 } },
             { {   48,   36,   27,   20,   15 }, {   49,   37,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   48,   36,   27,   20,   15 }, {   49,   37,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   48,   36,   27,   20,   15 }, {   49,   37,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   49,   37,   28,   21,   16 }, {   49,   37,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   49,   37,   28,   21,   16 }, {   49,   37,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   49,   37,   28,   21,   16 }, {   50,   38,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   49,   37,   28,   21,   16 }, {   50,   38,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   49,   37,   28,   21,   16 }, {   50,   38,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   49,   37,   28,   21,   16 }, {   50,   38,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   49,   37,   28,   21,   16 }, {   50,   38,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   50,   38,   28,   21,   16 }, {   50,   38,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   50,   38,   28,   21,   16 }, {   50,   38,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   50,   38,   28,   21,   16 }, {   50,   38,   28,   21,   16 }, {   47,   35,   26,   20,   15 } },
             { {   50,   38,   28,   21,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   50,   38,   28,   21,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   50,   38,   28,   21,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   50,   38,   28,   21,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   51,   38,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   51,   38,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   51,   38,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   51,   38,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   51,   38,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   51,   38,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   51,   38,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   52,   39,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   52,   39,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   52,   39,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   52,   39,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   52,   39,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   52,   39,   29,   22,   16 }, {   51,   38,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   52,   39,   29,   22,   16 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   53,   40,   30,   22,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   53,   40,   30,   22,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   53,   40,   30,   22,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   53,   40,   30,   22,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   53,   40,   30,   22,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   53,   40,   30,   22,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   54,   41,   30,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   54,   41,   30,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   54,   41,   30,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   54,   41,   30,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   54,   41,   30,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   54,   41,   30,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   47,   35,   26,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   48,   36,   27,   20,   15 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   49,   37,   28,   21,   16 } },
             { {   55,   41,   31,   23,   17 }, {   52,   39,   29,   22,   16 }, {   49,   37,   28,   21,   16 } },
             { {   56,   42,   32,   24,   18 }, {   52,   39,   29,   22,   16 }, {   49,   37,   28,   21,   16 } },
             { {   56,   42,   32,   24,   18 }, {   52,   39,   29,   22,   16 }, {   50,   38,   28,   21,   16 } },
             { {   56,   42,   32,   24,   18 }, {   52,   39,   29,   22,   16 }, {   50,   38,   28,   21,   16 } },
             { {   56,   42,   32,   24,   18 }, {   52,   39,   29,   22,   16 }, {   50,   38,   28,   21,   16 } },
             { {   56,   42,   32,   24,   18 }, {   52,   39,   29,   22,   16 }, {   51,   38,   29,   22,   16 } },
             { {   56,   42,   32,   24,   18 }, {   52,   39,   29,   22,   16 }, {   51,   38,   29,   22,   16 } },
             { {   56,   42,   32,   24,   18 }, {   52,   39,   29,   22,   16 }, {   52,   39,   29,   22,   16 } },
             { {   56,   42,   32,   24,   18 }, {   52,   39,   29,   22,   16 }, {   52,   39,   29,   22,   16 } },
             { {   56,   42,   32,   24,   18 }, {   52,   39,   29,   22,   16 }, {   52,   39,   29,   22,   16 } },
             { {   56,   42,   32,   24,   18 }, {   53,   40,   30,   22,   17 }, {   53,   40,   30,   22,   17 } },
             { {   56,   42,   32,   24,   18 }, {   53,   40,   30,   22,   17 }, {   53,   40,   30,   22,   17 } },
             { {   56,   42,   32,   24,   18 }, {   53,   40,   30,   22,   17 }, {   53,   40,   30,   22,   17 } },
             { {   56,   42,   32,   24,   18 }, {   53,   40,   30,   22,   17 }, {   53,   40,   30,   22,   17 } },
             { {   56,   42,   32,   24,   18 }, {   53,   40,   30,   22,   17 }, {   53,   40,   30,   22,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   55,   41,   31,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   57,   43,   32,   24,   18 }, {   55,   41,   31,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   55,   41,   31,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   55,   41,   31,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   55,   41,   31,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   55,   41,   31,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   55,   41,   31,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   55,   41,   31,   23,   17 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   56,   42,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   56,   42,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   56,   42,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   56,   42,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   56,   42,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   56,   42,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   58,   44,   33,   24,   18 }, {   56,   42,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   56,   42,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   59,   44,   33,   25,   19 }, {   57,   43,   32,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   60,   45,   34,   25,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   54,   41,   30,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   55,   41,   31,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   55,   41,   31,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   55,   41,   31,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 }, {   55,   41,   31,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   55,   41,   31,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   55,   41,   31,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   55,   41,   31,   23,   17 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   56,   42,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   60,   45,   34,   25,   19 }, {   57,   43,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   57,   43,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   57,   43,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   57,   43,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   57,   43,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   57,   43,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   57,   43,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   57,   43,   32,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   58,   44,   33,   24,   18 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 } },
             { {   61,   46,   34,   26,   19 }, {   61,   46,   34,   26,   19 }, {   59,   44,   33,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   59,   44,   33,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   59,   44,   33,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   59,   44,   33,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   59,   44,   33,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   59,   44,   33,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   60,   45,   34,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   60,   45,   34,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   60,   45,   34,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   60,   45,   34,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   60,   45,   34,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   60,   45,   34,   25,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   61,   46,   34,   26,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   61,   46,   34,   26,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   61,   46,   34,   26,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   61,   46,   34,   26,   19 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 } },
             { {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 }, {   62,   47,   35,   26,   20 } },
             { {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 }, {   62,   47,   35,   26,   20 } },
             { {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 }, {   62,   47,   35,   26,   20 } },
             { {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 } },
             { {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 } },
             { {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 } },
             { {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 } },
             { {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 } },
             { {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 } },
             { {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 }, {   63,   47,   35,   27,   20 } },
             { {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 } },
             { {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 } },
             { {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 } },
             { {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 } },
             { {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 } },
             { {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 }, {   64,   48,   36,   27,   20 } },
             { {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 } },
             { {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 } },
             { {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 } },
             { {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 } },
             { {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 } },
             { {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 } },
             { {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 }, {   65,   49,   37,   27,   21 } },
             { {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 } },
             { {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 } },
             { {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 } },
             { {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 } },
             { {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 } },
             { {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 }, {   66,   50,   37,   28,   21 } },
             { {   67,   50,   38,   28,   21 }, {   67,   50,   38,   28,   21 }, {   67,   50,   38,   28,   21 } },
             { {   68,   51,   38,   29,   22 }, {   68,   51,   38,   29,   22 }, {   68,   51,   38,   29,   22 } },
             { {   68,   51,   38,   29,   22 }, {   68,   51,   38,   29,   22 }, {   68,   51,   38,   29,   22 } },
             { {   69,   52,   39,   29,   22 }, {   69,   52,   39,   29,   22 }, {   69,   52,   39,   29,   22 } },
             { {   70,   53,   39,   30,   22 }, {   70,   53,   39,   30,   22 }, {   70,   53,   39,   30,   22 } },
             { {   70,   53,   39,   30,   22 }, {   70,   53,   39,   30,   22 }, {   70,   53,   39,   30,   22 } },
             { {   71,   53,   40,   30,   22 }, {   71,   53,   40,   30,   22 }, {   71,   53,   40,   30,   22 } },
             { {   72,   54,   41,   30,   23 }, {   72,   54,   41,   30,   23 }, {   72,   54,   41,   30,   23 } },
             { {   72,   54,   41,   30,   23 }, {   72,   54,   41,   30,   23 }, {   72,   54,   41,   30,   23 } },
             { {   73,   55,   41,   31,   23 }, {   73,   55,   41,   31,   23 }, {   73,   55,   41,   31,   23 } },
             { {   74,   56,   42,   31,   23 }, {   74,   56,   42,   31,   23 }, {   74,   56,   42,   31,   23 } },
             { {   74,   56,   42,   31,   23 }, {   74,   56,   42,   31,   23 }, {   74,   56,   42,   31,   23 } },
             { {   75,   56,   42,   32,   24 }, {   75,   56,   42,   32,   24 }, {   75,   56,   42,   32,   24 } },
             { {   76,   57,   43,   32,   24 }, {   76,   57,   43,   32,   24 }, {   76,   57,   43,   32,   24 } },
             { {   76,   57,   43,   32,   24 }, {   76,   57,   43,   32,   24 }, {   76,   57,   43,   32,   24 } },
             { {   77,   58,   43,   32,   24 }, {   77,   58,   43,   32,   24 }, {   77,   58,   43,   32,   24 } },
             { {   78,   59,   44,   33,   25 }, {   78,   59,   44,   33,   25 }, {   78,   59,   44,   33,   25 } },
             { {   78,   59,   44,   33,   25 }, {   78,   59,   44,   33,   25 }, {   78,   59,   44,   33,   25 } },
             { {   79,   59,   44,   33,   25 }, {   79,   59,   44,   33,   25 }, {   79,   59,   44,   33,   25 } },
             { {   80,   60,   45,   34,   25 }, {   80,   60,   45,   34,   25 }, {   80,   60,   45,   34,   25 } },
             { {   81,   61,   46,   34,   26 }, {   81,   61,   46,   34,   26 }, {   81,   61,   46,   34,   26 } },
             { {   81,   61,   46,   34,   26 }, {   81,   61,   46,   34,   26 }, {   81,   61,   46,   34,   26 } },
             { {   82,   62,   46,   35,   26 }, {   82,   62,   46,   35,   26 }, {   82,   62,   46,   35,   26 } },
             { {   83,   62,   47,   35,   26 }, {   83,   62,   47,   35,   26 }, {   83,   62,   47,   35,   26 } },
             { {   84,   63,   47,   35,   27 }, {   84,   63,   47,   35,   27 }, {   84,   63,   47,   35,   27 } },
             { {   85,   64,   48,   36,   27 }, {   85,   64,   48,   36,   27 }, {   85,   64,   48,   36,   27 } },
             { {   85,   64,   48,   36,   27 }, {   85,   64,   48,   36,   27 }, {   85,   64,   48,   36,   27 } },
             { {   86,   65,   48,   36,   27 }, {   86,   65,   48,   36,   27 }, {   86,   65,   48,   36,   27 } },
             { {   87,   65,   49,   37,   28 }, {   87,   65,   49,   37,   28 }, {   87,   65,   49,   37,   28 } },
             { {   88,   66,   50,   37,   28 }, {   88,   66,   50,   37,   28 }, {   88,   66,   50,   37,   28 } },
             { {   89,   67,   50,   38,   28 }, {   89,   67,   50,   38,   28 }, {   89,   67,   50,   38,   28 } },
             { {   89,   67,   50,   38,   28 }, {   89,   67,   50,   38,   28 }, {   89,   67,   50,   38,   28 } },
             { {   90,   68,   51,   38,   28 }, {   90,   68,   51,   38,   28 }, {   90,   68,   51,   38,   28 } },
             { {   91,   68,   51,   38,   29 }, {   91,   68,   51,   38,   29 }, {   91,   68,   51,   38,   29 } },
             { {   92,   69,   52,   39,   29 }, {   92,   69,   52,   39,   29 }, {   92,   69,   52,   39,   29 } },
             { {   93,   70,   52,   39,   29 }, {   93,   70,   52,   39,   29 }, {   93,   70,   52,   39,   29 } },
             { {   94,   71,   53,   40,   30 }, {   94,   71,   53,   40,   30 }, {   94,   71,   53,   40,   30 } },
             { {   95,   71,   53,   40,   30 }, {   95,   71,   53,   40,   30 }, {   95,   71,   53,   40,   30 } },
             { {   95,   71,   53,   40,   30 }, {   95,   71,   53,   40,   30 }, {   95,   71,   53,   40,   30 } },
             { {   96,   72,   54,   41,   30 }, {   96,   72,   54,   41,   30 }, {   96,   72,   54,   41,   30 } },
             { {   97,   73,   55,   41,   31 }, {   97,   73,   55,   41,   31 }, {   97,   73,   55,   41,   31 } },
             { {   98,   74,   55,   41,   31 }, {   98,   74,   55,   41,   31 }, {   98,   74,   55,   41,   31 } },
             { {   99,   74,   56,   42,   31 }, {   99,   74,   56,   42,   31 }, {   99,   74,   56,   42,   31 } },
             { {  100,   75,   56,   42,   32 }, {  100,   75,   56,   42,   32 }, {  100,   75,   56,   42,   32 } },
             { {  101,   76,   57,   43,   32 }, {  101,   76,   57,   43,   32 }, {  101,   76,   57,   43,   32 } },
             { {  102,   77,   57,   43,   32 }, {  102,   77,   57,   43,   32 }, {  102,   77,   57,   43,   32 } },
             { {  103,   77,   58,   43,   33 }, {  103,   77,   58,   43,   33 }, {  103,   77,   58,   43,   33 } },
             { {  104,   78,   59,   44,   33 }, {  104,   78,   59,   44,   33 }, {  104,   78,   59,   44,   33 } },
             { {  105,   79,   59,   44,   33 }, {  105,   79,   59,   44,   33 }, {  105,   79,   59,   44,   33 } },
             { {  106,   80,   60,   45,   34 }, {  106,   80,   60,   45,   34 }, {  106,   80,   60,   45,   34 } },
             { {  107,   80,   60,   45,   34 }, {  107,   80,   60,   45,   34 }, {  107,   80,   60,   45,   34 } },
             { {  108,   81,   61,   46,   34 }, {  108,   81,   61,   46,   34 }, {  108,   81,   61,   46,   34 } },
             { {  109,   82,   61,   46,   34 }, {  109,   82,   61,   46,   34 }, {  109,   82,   61,   46,   34 } },
             { {  110,   83,   62,   46,   35 }, {  110,   83,   62,   46,   35 }, {  110,   83,   62,   46,   35 } },
             { {  111,   83,   62,   47,   35 }, {  111,   83,   62,   47,   35 }, {  111,   83,   62,   47,   35 } },
             { {  112,   84,   63,   47,   35 }, {  112,   84,   63,   47,   35 }, {  112,   84,   63,   47,   35 } },
             { {  113,   85,   64,   48,   36 }, {  113,   85,   64,   48,   36 }, {  113,   85,   64,   48,   36 } },
             { {  114,   86,   64,   48,   36 }, {  114,   86,   64,   48,   36 }, {  114,   86,   64,   48,   36 } },
             { {  115,   86,   65,   49,   36 }, {  115,   86,   65,   49,   36 }, {  115,   86,   65,   49,   36 } },
             { {  116,   87,   65,   49,   37 }, {  116,   87,   65,   49,   37 }, {  116,   87,   65,   49,   37 } },
             { {  117,   88,   66,   49,   37 }, {  117,   88,   66,   49,   37 }, {  117,   88,   66,   49,   37 } },
             { {  118,   89,   66,   50,   37 }, {  118,   89,   66,   50,   37 }, {  118,   89,   66,   50,   37 } },
             { {  119,   89,   67,   50,   38 }, {  119,   89,   67,   50,   38 }, {  119,   89,   67,   50,   38 } },
             { {  121,   91,   68,   51,   38 }, {  121,   91,   68,   51,   38 }, {  121,   91,   68,   51,   38 } },
             { {  122,   92,   69,   51,   39 }, {  122,   92,   69,   51,   39 }, {  122,   92,   69,   51,   39 } },
             { {  123,   92,   69,   52,   39 }, {  123,   92,   69,   52,   39 }, {  123,   92,   69,   52,   39 } },
             { {  124,   93,   70,   52,   39 }, {  124,   93,   70,   52,   39 }, {  124,   93,   70,   52,   39 } },
             { {  125,   94,   70,   53,   40 }, {  125,   94,   70,   53,   40 }, {  125,   94,   70,   53,   40 } },
             { {  126,   95,   71,   53,   40 }, {  126,   95,   71,   53,   40 }, {  126,   95,   71,   53,   40 } },
             { {  127,   95,   71,   54,   40 }, {  127,   95,   71,   54,   40 }, {  127,   95,   71,   54,   40 } },
             { {  129,   97,   73,   54,   41 }, {  129,   97,   73,   54,   41 }, {  129,   97,   73,   54,   41 } },
             { {  130,   98,   73,   55,   41 }, {  130,   98,   73,   55,   41 }, {  130,   98,   73,   55,   41 } },
             { {  131,   98,   74,   55,   41 }, {  131,   98,   74,   55,   41 }, {  131,   98,   74,   55,   41 } },
             { {  132,   99,   74,   56,   42 }, {  132,   99,   74,   56,   42 }, {  132,   99,   74,   56,   42 } },
             { {  134,  101,   75,   57,   42 }, {  134,  101,   75,   57,   42 }, {  134,  101,   75,   57,   42 } },
             { {  135,  101,   76,   57,   43 }, {  135,  101,   76,   57,   43 }, {  135,  101,   76,   57,   43 } },
             { {  136,  102,   77,   57,   43 }, {  136,  102,   77,   57,   43 }, {  136,  102,   77,   57,   43 } },
             { {  137,  103,   77,   58,   43 }, {  137,  103,   77,   58,   43 }, {  137,  103,   77,   58,   43 } },
             { {  139,  104,   78,   59,   44 }, {  139,  104,   78,   59,   44 }, {  139,  104,   78,   59,   44 } },
             { {  140,  105,   79,   59,   44 }, {  140,  105,   79,   59,   44 }, {  140,  105,   79,   59,   44 } },
             { {  141,  106,   79,   59,   45 }, {  141,  106,   79,   59,   45 }, {  141,  106,   79,   59,   45 } },
             { {  143,  107,   80,   60,   45 }, {  143,  107,   80,   60,   45 }, {  143,  107,   80,   60,   45 } },
             { {  144,  108,   81,   61,   46 }, {  144,  108,   81,   61,   46 }, {  144,  108,   81,   61,   46 } },
             { {  145,  109,   82,   61,   46 }, {  145,  109,   82,   61,   46 }, {  145,  109,   82,   61,   46 } },
             { {  147,  110,   83,   62,   47 }, {  147,  110,   83,   62,   47 }, {  147,  110,   83,   62,   47 } },
             { {  148,  111,   83,   62,   47 }, {  148,  111,   83,   62,   47 }, {  148,  111,   83,   62,   47 } },
             { {  149,  112,   84,   63,   47 }, {  149,  112,   84,   63,   47 }, {  149,  112,   84,   63,   47 } },
             { {  151,  113,   85,   64,   48 }, {  151,  113,   85,   64,   48 }, {  151,  113,   85,   64,   48 } },
             { {  152,  114,   86,   64,   48 }, {  152,  114,   86,   64,   48 }, {  152,  114,   86,   64,   48 } },
             { {  154,  116,   87,   65,   49 }, {  154,  116,   87,   65,   49 }, {  154,  116,   87,   65,   49 } },
             { {  155,  116,   87,   65,   49 }, {  155,  116,   87,   65,   49 }, {  155,  116,   87,   65,   49 } },
             { {  156,  117,   88,   66,   49 }, {  156,  117,   88,   66,   49 }, {  156,  117,   88,   66,   49 } },
             { {  158,  119,   89,   67,   50 }, {  158,  119,   89,   67,   50 }, {  158,  119,   89,   67,   50 } },
             { {  159,  119,   89,   67,   50 }, {  159,  119,   89,   67,   50 }, {  159,  119,   89,   67,   50 } },
             { {  161,  121,   91,   68,   51 }, {  161,  121,   91,   68,   51 }, {  161,  121,   91,   68,   51 } },
             { {  162,  122,   91,   68,   51 }, {  162,  122,   91,   68,   51 }, {  162,  122,   91,   68,   51 } },
             { {  164,  123,   92,   69,   52 }, {  164,  123,   92,   69,   52 }, {  164,  123,   92,   69,   52 } },
             { {  165,  124,   93,   70,   52 }, {  165,  124,   93,   70,   52 }, {  165,  124,   93,   70,   52 } },
             { {  167,  125,   94,   70,   53 }, {  167,  125,   94,   70,   53 }, {  167,  125,   94,   70,   53 } },
             { {  169,  127,   95,   71,   53 }, {  169,  127,   95,   71,   53 }, {  169,  127,   95,   71,   53 } },
             { {  170,  128,   96,   72,   54 }, {  170,  128,   96,   72,   54 }, {  170,  128,   96,   72,   54 } },
             { {  172,  129,   97,   73,   54 }, {  172,  129,   97,   73,   54 }, {  172,  129,   97,   73,   54 } },
             { {  173,  130,   97,   73,   55 }, {  173,  130,   97,   73,   55 }, {  173,  130,   97,   73,   55 } },
             { {  175,  131,   98,   74,   55 }, {  175,  131,   98,   74,   55 }, {  175,  131,   98,   74,   55 } },
             { {  177,  133,  100,   75,   56 }, {  177,  133,  100,   75,   56 }, {  177,  133,  100,   75,   56 } },
             { {  178,  134,  100,   75,   56 }, {  178,  134,  100,   75,   56 }, {  178,  134,  100,   75,   56 } },
             { {  180,  135,  101,   76,   57 }, {  180,  135,  101,   76,   57 }, {  180,  135,  101,   76,   57 } },
             { {  182,  137,  102,   77,   58 }, {  182,  137,  102,   77,   58 }, {  182,  137,  102,   77,   58 } },
             { {  183,  137,  103,   77,   58 }, {  183,  137,  103,   77,   58 }, {  183,  137,  103,   77,   58 } },
             { {  185,  139,  104,   78,   59 }, {  185,  139,  104,   78,   59 }, {  185,  139,  104,   78,   59 } },
             { {  187,  140,  105,   79,   59 }, {  187,  140,  105,   79,   59 }, {  187,  140,  105,   79,   59 } },
             { {  188,  141,  106,   79,   59 }, {  188,  141,  106,   79,   59 }, {  188,  141,  106,   79,   59 } },
             { {  190,  143,  107,   80,   60 }, {  190,  143,  107,   80,   60 }, {  190,  143,  107,   80,   60 } },
             { {  192,  144,  108,   81,   61 }, {  192,  144,  108,   81,   61 }, {  192,  144,  108,   81,   61 } },
             { {  194,  146,  109,   82,   61 }, {  194,  146,  109,   82,   61 }, {  194,  146,  109,   82,   61 } },
             { {  196,  147,  110,   83,   62 }, {  196,  147,  110,   83,   62 }, {  196,  147,  110,   83,   62 } },
             { {  197,  148,  111,   83,   62 }, {  197,  148,  111,   83,   62 }, {  197,  148,  111,   83,   62 } },
             { {  199,  149,  112,   84,   63 }, {  199,  149,  112,   84,   63 }, {  199,  149,  112,   84,   63 } },
             { {  201,  151,  113,   85,   64 }, {  201,  151,  113,   85,   64 }, {  201,  151,  113,   85,   64 } },
             { {  203,  152,  114,   86,   64 }, {  203,  152,  114,   86,   64 }, {  203,  152,  114,   86,   64 } },
             { {  205,  154,  115,   86,   65 }, {  205,  154,  115,   86,   65 }, {  205,  154,  115,   86,   65 } },
             { {  207,  155,  116,   87,   65 }, {  207,  155,  116,   87,   65 }, {  207,  155,  116,   87,   65 } },
             { {  209,  157,  118,   88,   66 }, {  209,  157,  118,   88,   66 }, {  209,  157,  118,   88,   66 } },
             { {  211,  158,  119,   89,   67 }, {  211,  158,  119,   89,   67 }, {  211,  158,  119,   89,   67 } },
             { {  213,  160,  120,   90,   67 }, {  213,  160,  120,   90,   67 }, {  213,  160,  120,   90,   67 } },
             { {  215,  161,  121,   91,   68 }, {  215,  161,  121,   91,   68 }, {  215,  161,  121,   91,   68 } },
             { {  217,  163,  122,   92,   69 }, {  217,  163,  122,   92,   69 }, {  217,  163,  122,   92,   69 } },
             { {  219,  164,  123,   92,   69 }, {  219,  164,  123,   92,   69 }, {  219,  164,  123,   92,   69 } },
             { {  221,  166,  124,   93,   70 }, {  221,  166,  124,   93,   70 }, {  221,  166,  124,   93,   70 } },
             { {  223,  167,  125,   94,   71 }, {  223,  167,  125,   94,   71 }, {  223,  167,  125,   94,   71 } },
             { {  225,  169,  127,   95,   71 }, {  225,  169,  127,   95,   71 }, {  225,  169,  127,   95,   71 } },
             { {  227,  170,  128,   96,   72 }, {  227,  170,  128,   96,   72 }, {  227,  170,  128,   96,   72 } },
             { {  229,  172,  129,   97,   72 }, {  229,  172,  129,   97,   72 }, {  229,  172,  129,   97,   72 } },
             { {  231,  173,  130,   97,   73 }, {  231,  173,  130,   97,   73 }, {  231,  173,  130,   97,   73 } },
             { {  234,  176,  132,   99,   74 }, {  234,  176,  132,   99,   74 }, {  234,  176,  132,   99,   74 } },
             { {  236,  177,  133,  100,   75 }, {  236,  177,  133,  100,   75 }, {  236,  177,  133,  100,   75 } },
             { {  238,  179,  134,  100,   75 }, {  238,  179,  134,  100,   75 }, {  238,  179,  134,  100,   75 } },
             { {  240,  180,  135,  101,   76 }, {  240,  180,  135,  101,   76 }, {  240,  180,  135,  101,   76 } },
             { {  242,  182,  136,  102,   77 }, {  242,  182,  136,  102,   77 }, {  242,  182,  136,  102,   77 } },
             { {  245,  184,  138,  103,   78 }, {  245,  184,  138,  103,   78 }, {  245,  184,  138,  103,   78 } },
             { {  247,  185,  139,  104,   78 }, {  247,  185,  139,  104,   78 }, {  247,  185,  139,  104,   78 } },
             { {  249,  187,  140,  105,   79 }, {  249,  187,  140,  105,   79 }, {  249,  187,  140,  105,   79 } },
             { {  252,  189,  142,  106,   80 }, {  252,  189,  142,  106,   80 }, {  252,  189,  142,  106,   80 } },
             { {  254,  191,  143,  107,   80 }, {  254,  191,  143,  107,   80 }, {  254,  191,  143,  107,   80 } },
             { {  256,  192,  144,  108,   81 }, {  256,  192,  144,  108,   81 }, {  256,  192,  144,  108,   81 } },
             { {  259,  194,  146,  109,   82 }, {  259,  194,  146,  109,   82 }, {  259,  194,  146,  109,   82 } },
             { {  261,  196,  147,  110,   83 }, {  261,  196,  147,  110,   83 }, {  261,  196,  147,  110,   83 } },
             { {  264,  198,  149,  111,   84 }, {  264,  198,  149,  111,   84 }, {  264,  198,  149,  111,   84 } },
             { {  266,  200,  150,  112,   84 }, {  266,  200,  150,  112,   84 }, {  266,  200,  150,  112,   84 } },
             { {  269,  202,  151,  113,   85 }, {  269,  202,  151,  113,   85 }, {  269,  202,  151,  113,   85 } },
             { {  271,  203,  152,  114,   86 }, {  271,  203,  152,  114,   86 }, {  271,  203,  152,  114,   86 } },
             { {  274,  206,  154,  116,   87 }, {  274,  206,  154,  116,   87 }, {  274,  206,  154,  116,   87 } },
             { {  276,  207,  155,  116,   87 }, {  276,  207,  155,  116,   87 }, {  276,  207,  155,  116,   87 } },
             { {  279,  209,  157,  118,   88 }, {  279,  209,  157,  118,   88 }, {  279,  209,  157,  118,   88 } },
             { {  281,  211,  158,  119,   89 }, {  281,  211,  158,  119,   89 }, {  281,  211,  158,  119,   89 } },
             { {  284,  213,  160,  120,   90 }, {  284,  213,  160,  120,   90 }, {  284,  213,  160,  120,   90 } },
             { {  287,  215,  161,  121,   91 }, {  287,  215,  161,  121,   91 }, {  287,  215,  161,  121,   91 } },
             { {  289,  217,  163,  122,   91 }, {  289,  217,  163,  122,   91 }, {  289,  217,  163,  122,   91 } },
             { {  292,  219,  164,  123,   92 }, {  292,  219,  164,  123,   92 }, {  292,  219,  164,  123,   92 } },
             { {  295,  221,  166,  124,   93 }, {  295,  221,  166,  124,   93 }, {  295,  221,  166,  124,   93 } },
             { {  298,  224,  168,  126,   94 }, {  298,  224,  168,  126,   94 }, {  298,  224,  168,  126,   94 } },
             { {  300,  225,  169,  127,   95 }, {  300,  225,  169,  127,   95 }, {  300,  225,  169,  127,   95 } },
             { {  303,  227,  170,  128,   96 }, {  303,  227,  170,  128,   96 }, {  303,  227,  170,  128,   96 } },
             { {  306,  230,  172,  129,   97 }, {  306,  230,  172,  129,   97 }, {  306,  230,  172,  129,   97 } },
             { {  309,  232,  174,  130,   98 }, {  309,  232,  174,  130,   98 }, {  309,  232,  174,  130,   98 } },
             { {  312,  234,  176,  132,   99 }, {  312,  234,  176,  132,   99 }, {  312,  234,  176,  132,   99 } },
             { {  315,  236,  177,  133,  100 }, {  315,  236,  177,  133,  100 }, {  315,  236,  177,  133,  100 } },
             { {  318,  239,  179,  134,  101 }, {  318,  239,  179,  134,  101 }, {  318,  239,  179,  134,  101 } },
             { {  321,  241,  181,  135,  102 }, {  321,  241,  181,  135,  102 }, {  321,  241,  181,  135,  102 } },
             { {  324,  243,  182,  137,  103 }, {  324,  243,  182,  137,  103 }, {  324,  243,  182,  137,  103 } },
             { {  327,  245,  184,  138,  103 }, {  327,  245,  184,  138,  103 }, {  327,  245,  184,  138,  103 } },
             { {  330,  248,  186,  139,  104 }, {  330,  248,  186,  139,  104 }, {  330,  248,  186,  139,  104 } },
             { {  333,  250,  187,  140,  105 }, {  333,  250,  187,  140,  105 }, {  333,  250,  187,  140,  105 } },
             { {  336,  252,  189,  142,  106 }, {  336,  252,  189,  142,  106 }, {  336,  252,  189,  142,  106 } },
             { {  339,  254,  191,  143,  107 }, {  339,  254,  191,  143,  107 }, {  339,  254,  191,  143,  107 } },
             { {  342,  257,  192,  144,  108 }, {  342,  257,  192,  144,  108 }, {  342,  257,  192,  144,  108 } },
             { {  345,  259,  194,  146,  109 }, {  345,  259,  194,  146,  109 }, {  345,  259,  194,  146,  109 } },
             { {  349,  262,  196,  147,  110 }, {  349,  262,  196,  147,  110 }, {  349,  262,  196,  147,  110 } },
             { {  352,  264,  198,  149,  111 }, {  352,  264,  198,  149,  111 }, {  352,  264,  198,  149,  111 } },
             { {  355,  266,  200,  150,  112 }, {  355,  266,  200,  150,  112 }, {  355,  266,  200,  150,  112 } },
             { {  358,  269,  201,  151,  113 }, {  358,  269,  201,  151,  113 }, {  358,  269,  201,  151,  113 } },
             { {  362,  272,  204,  153,  115 }, {  362,  272,  204,  153,  115 }, {  362,  272,  204,  153,  115 } },
             { {  365,  274,  205,  154,  115 }, {  365,  274,  205,  154,  115 }, {  365,  274,  205,  154,  115 } },
             { {  369,  277,  208,  156,  117 }, {  369,  277,  208,  156,  117 }, {  369,  277,  208,  156,  117 } },
             { {  372,  279,  209,  157,  118 }, {  372,  279,  209,  157,  118 }, {  372,  279,  209,  157,  118 } },
             { {  376,  282,  212,  159,  119 }, {  376,  282,  212,  159,  119 }, {  376,  282,  212,  159,  119 } },
             { {  379,  284,  213,  160,  120 }, {  379,  284,  213,  160,  120 }, {  379,  284,  213,  160,  120 } },
             { {  383,  287,  215,  162,  121 }, {  383,  287,  215,  162,  121 }, {  383,  287,  215,  162,  121 } },
             { {  386,  290,  217,  163,  122 }, {  386,  290,  217,  163,  122 }, {  386,  290,  217,  163,  122 } },
             { {  390,  293,  219,  165,  123 }, {  390,  293,  219,  165,  123 }, {  390,  293,  219,  165,  123 } },
             { {  393,  295,  221,  166,  124 }, {  393,  295,  221,  166,  124 }, {  393,  295,  221,  166,  124 } },
             { {  397,  298,  223,  167,  126 }, {  397,  298,  223,  167,  126 }, {  397,  298,  223,  167,  126 } },
             { {  401,  301,  226,  169,  127 }, {  401,  301,  226,  169,  127 }, {  401,  301,  226,  169,  127 } },
             { {  405,  304,  228,  171,  128 }, {  405,  304,  228,  171,  128 }, {  405,  304,  228,  171,  128 } },
             { {  408,  306,  230,  172,  129 }, {  408,  306,  230,  172,  129 }, {  408,  306,  230,  172,  129 } },
             { {  412,  309,  232,  174,  130 }, {  412,  309,  232,  174,  130 }, {  412,  309,  232,  174,  130 } },
             { {  416,  312,  234,  176,  132 }, {  416,  312,  234,  176,  132 }, {  416,  312,  234,  176,  132 } },
             { {  420,  315,  236,  177,  133 }, {  420,  315,  236,  177,  133 }, {  420,  315,  236,  177,  133 } },
             { {  424,  318,  239,  179,  134 }, {  424,  318,  239,  179,  134 }, {  424,  318,  239,  179,  134 } },
             { {  428,  321,  241,  181,  135 }, {  428,  321,  241,  181,  135 }, {  428,  321,  241,  181,  135 } },
             { {  432,  324,  243,  182,  137 }, {  432,  324,  243,  182,  137 }, {  432,  324,  243,  182,  137 } },
             { {  436,  327,  245,  184,  138 }, {  436,  327,  245,  184,  138 }, {  436,  327,  245,  184,  138 } },
             { {  440,  330,  248,  186,  139 }, {  440,  330,  248,  186,  139 }, {  440,  330,  248,  186,  139 } },
             { {  444,  333,  250,  187,  140 }, {  444,  333,  250,  187,  140 }, {  444,  333,  250,  187,  140 } },
             { {  448,  336,  252,  189,  142 }, {  448,  336,  252,  189,  142 }, {  448,  336,  252,  189,  142 } },
             { {  452,  339,  254,  191,  143 }, {  452,  339,  254,  191,  143 }, {  452,  339,  254,  191,  143 } },
             { {  457,  343,  257,  193,  145 }, {  457,  343,  257,  193,  145 }, {  457,  343,  257,  193,  145 } },
             { {  461,  346,  259,  194,  146 }, {  461,  346,  259,  194,  146 }, {  461,  346,  259,  194,  146 } },
             { {  465,  349,  262,  196,  147 }, {  465,  349,  262,  196,  147 }, {  465,  349,  262,  196,  147 } },
             { {  470,  353,  264,  198,  149 }, {  470,  353,  264,  198,  149 }, {  470,  353,  264,  198,  149 } },
             { {  474,  356,  267,  200,  150 }, {  474,  356,  267,  200,  150 }, {  474,  356,  267,  200,  150 } },
             { {  479,  359,  269,  202,  152 }, {  479,  359,  269,  202,  152 }, {  479,  359,  269,  202,  152 } },
             { {  483,  362,  272,  204,  153 }, {  483,  362,  272,  204,  153 }, {  483,  362,  272,  204,  153 } },
             { {  488,  366,  275,  206,  154 }, {  488,  366,  275,  206,  154 }, {  488,  366,  275,  206,  154 } },
             { {  492,  369,  277,  208,  156 }, {  492,  369,  277,  208,  156 }, {  492,  369,  277,  208,  156 } },
             { {  497,  373,  280,  210,  157 }, {  497,  373,  280,  210,  157 }, {  497,  373,  280,  210,  157 } },
             { {  501,  376,  282,  211,  159 }, {  501,  376,  282,  211,  159 }, {  501,  376,  282,  211,  159 } },
             { {  506,  380,  285,  213,  160 }, {  506,  380,  285,  213,  160 }, {  506,  380,  285,  213,  160 } },
             { {  511,  383,  287,  216,  162 }, {  511,  383,  287,  216,  162 }, {  511,  383,  287,  216,  162 } },
             { {  516,  387,  290,  218,  163 }, {  516,  387,  290,  218,  163 }, {  516,  387,  290,  218,  163 } },
             { {  520,  390,  293,  219,  165 }, {  520,  390,  293,  219,  165 }, {  520,  390,  293,  219,  165 } },
             { {  525,  394,  295,  221,  166 }, {  525,  394,  295,  221,  166 }, {  525,  394,  295,  221,  166 } },
             { {  530,  398,  298,  224,  168 }, {  530,  398,  298,  224,  168 }, {  530,  398,  298,  224,  168 } },
             { {  535,  401,  301,  226,  169 }, {  535,  401,  301,  226,  169 }, {  535,  401,  301,  226,  169 } },
             { {  540,  405,  304,  228,  171 }, {  540,  405,  304,  228,  171 }, {  540,  405,  304,  228,  171 } },
             { {  545,  409,  307,  230,  172 }, {  545,  409,  307,  230,  172 }, {  545,  409,  307,  230,  172 } },
             { {  550,  413,  309,  232,  174 }, {  550,  413,  309,  232,  174 }, {  550,  413,  309,  232,  174 } },
             { {  555,  416,  312,  234,  176 }, {  555,  416,  312,  234,  176 }, {  555,  416,  312,  234,  176 } },
             { {  561,  421,  316,  237,  178 }, {  561,  421,  316,  237,  178 }, {  561,  421,  316,  237,  178 } },
             { {  566,  425,  318,  239,  179 }, {  566,  425,  318,  239,  179 }, {  566,  425,  318,  239,  179 } },
             { {  571,  428,  321,  241,  181 }, {  571,  428,  321,  241,  181 }, {  571,  428,  321,  241,  181 } },
             { {  577,  433,  325,  243,  183 }, {  577,  433,  325,  243,  183 }, {  577,  433,  325,  243,  183 } },
             { {  582,  437,  327,  246,  184 }, {  582,  437,  327,  246,  184 }, {  582,  437,  327,  246,  184 } },
             { {  587,  440,  330,  248,  186 }, {  587,  440,  330,  248,  186 }, {  587,  440,  330,  248,  186 } },
             { {  593,  445,  334,  250,  188 }, {  593,  445,  334,  250,  188 }, {  593,  445,  334,  250,  188 } },
             { {  598,  449,  336,  252,  189 }, {  598,  449,  336,  252,  189 }, {  598,  449,  336,  252,  189 } },
             { {  604,  453,  340,  255,  191 }, {  604,  453,  340,  255,  191 }, {  604,  453,  340,  255,  191 } },
             { {  610,  458,  343,  257,  193 }, {  610,  458,  343,  257,  193 }, {  610,  458,  343,  257,  193 } },
             { {  615,  461,  346,  259,  195 }, {  615,  461,  346,  259,  195 }, {  615,  461,  346,  259,  195 } },
             { {  621,  466,  349,  262,  196 }, {  621,  466,  349,  262,  196 }, {  621,  466,  349,  262,  196 } },
             { {  627,  470,  353,  265,  198 }, {  627,  470,  353,  265,  198 }, {  627,  470,  353,  265,  198 } },
             { {  633,  475,  356,  267,  200 }, {  633,  475,  356,  267,  200 }, {  633,  475,  356,  267,  200 } },
             { {  639,  479,  359,  270,  202 }, {  639,  479,  359,  270,  202 }, {  639,  479,  359,  270,  202 } },
             { {  645,  484,  363,  272,  204 }, {  645,  484,  363,  272,  204 }, {  645,  484,  363,  272,  204 } },
             { {  651,  488,  366,  275,  206 }, {  651,  488,  366,  275,  206 }, {  651,  488,  366,  275,  206 } },
             { {  657,  493,  370,  277,  208 }, {  657,  493,  370,  277,  208 }, {  657,  493,  370,  277,  208 } },
             { {  663,  497,  373,  280,  210 }, {  663,  497,  373,  280,  210 }, {  663,  497,  373,  280,  210 } },
             { {  669,  502,  376,  282,  212 }, {  669,  502,  376,  282,  212 }, {  669,  502,  376,  282,  212 } },
             { {  675,  506,  380,  285,  214 }, {  675,  506,  380,  285,  214 }, {  675,  506,  380,  285,  214 } },
             { {  682,  512,  384,  288,  216 }, {  682,  512,  384,  288,  216 }, {  682,  512,  384,  288,  216 } },
             { {  688,  516,  387,  290,  218 }, {  688,  516,  387,  290,  218 }, {  688,  516,  387,  290,  218 } },
             { {  695,  521,  391,  293,  220 }, {  695,  521,  391,  293,  220 }, {  695,  521,  391,  293,  220 } },
             { {  701,  526,  394,  296,  222 }, {  701,  526,  394,  296,  222 }, {  701,  526,  394,  296,  222 } },
             { {  708,  531,  398,  299,  224 }, {  708,  531,  398,  299,  224 }, {  708,  531,  398,  299,  224 } },
             { {  714,  536,  402,  301,  226 }, {  714,  536,  402,  301,  226 }, {  714,  536,  402,  301,  226 } },
             { {  721,  541,  406,  304,  228 }, {  721,  541,  406,  304,  228 }, {  721,  541,  406,  304,  228 } },
             { {  728,  546,  410,  307,  230 }, {  728,  546,  410,  307,  230 }, {  728,  546,  410,  307,  230 } },
             { {  735,  551,  413,  310,  233 }, {  735,  551,  413,  310,  233 }, {  735,  551,  413,  310,  233 } },
             { {  741,  556,  417,  313,  234 }, {  741,  556,  417,  313,  234 }, {  741,  556,  417,  313,  234 } },
             { {  748,  561,  421,  316,  237 }, {  748,  561,  421,  316,  237 }, {  748,  561,  421,  316,  237 } },
             { {  755,  566,  425,  319,  239 }, {  755,  566,  425,  319,  239 }, {  755,  566,  425,  319,  239 } },
             { {  762,  572,  429,  321,  241 }, {  762,  572,  429,  321,  241 }, {  762,  572,  429,  321,  241 } },
             { {  770,  578,  433,  325,  244 }, {  770,  578,  433,  325,  244 }, {  770,  578,  433,  325,  244 } },
             { {  777,  583,  437,  328,  246 }, {  777,  583,  437,  328,  246 }, {  777,  583,  437,  328,  246 } },
             { {  784,  588,  441,  331,  248 }, {  784,  588,  441,  331,  248 }, {  784,  588,  441,  331,  248 } },
             { {  791,  593,  445,  334,  250 }, {  791,  593,  445,  334,  250 }, {  791,  593,  445,  334,  250 } },
             { {  799,  599,  449,  337,  253 }, {  799,  599,  449,  337,  253 }, {  799,  599,  449,  337,  253 } },
             { {  806,  605,  453,  340,  255 }, {  806,  605,  453,  340,  255 }, {  806,  605,  453,  340,  255 } },
             { {  814,  611,  458,  343,  258 }, {  814,  611,  458,  343,  258 }, {  814,  611,  458,  343,  258 } },
             { {  821,  616,  462,  346,  260 }, {  821,  616,  462,  346,  260 }, {  821,  616,  462,  346,  260 } },
             { {  829,  622,  466,  350,  262 }, {  829,  622,  466,  350,  262 }, {  829,  622,  466,  350,  262 } },
             { {  837,  628,  471,  353,  265 }, {  837,  628,  471,  353,  265 }, {  837,  628,  471,  353,  265 } },
             { {  845,  634,  475,  356,  267 }, {  845,  634,  475,  356,  267 }, {  845,  634,  475,  356,  267 } },
             { {  853,  640,  480,  360,  270 }, {  853,  640,  480,  360,  270 }, {  853,  640,  480,  360,  270 } },
             { {  861,  646,  484,  363,  272 }, {  861,  646,  484,  363,  272 }, {  861,  646,  484,  363,  272 } },
             { {  869,  652,  489,  367,  275 }, {  869,  652,  489,  367,  275 }, {  869,  652,  489,  367,  275 } },
             { {  877,  658,  493,  370,  277 }, {  877,  658,  493,  370,  277 }, {  877,  658,  493,  370,  277 } },
             { {  885,  664,  498,  373,  280 }, {  885,  664,  498,  373,  280 }, {  885,  664,  498,  373,  280 } },
             { {  893,  670,  502,  377,  283 }, {  893,  670,  502,  377,  283 }, {  893,  670,  502,  377,  283 } },
             { {  902,  677,  507,  381,  285 }, {  902,  677,  507,  381,  285 }, {  902,  677,  507,  381,  285 } },
             { {  910,  683,  512,  384,  288 }, {  910,  683,  512,  384,  288 }, {  910,  683,  512,  384,  288 } },
             { {  919,  689,  517,  388,  291 }, {  919,  689,  517,  388,  291 }, {  919,  689,  517,  388,  291 } },
             { {  927,  695,  521,  391,  293 }, {  927,  695,  521,  391,  293 }, {  927,  695,  521,  391,  293 } },
             { {  936,  702,  527,  395,  296 }, {  936,  702,  527,  395,  296 }, {  936,  702,  527,  395,  296 } },
             { {  945,  709,  532,  399,  299 }, {  945,  709,  532,  399,  299 }, {  945,  709,  532,  399,  299 } },
             { {  954,  716,  537,  402,  302 }, {  954,  716,  537,  402,  302 }, {  954,  716,  537,  402,  302 } },
             { {  962,  722,  541,  406,  304 }, {  962,  722,  541,  406,  304 }, {  962,  722,  541,  406,  304 } },
             { {  971,  728,  546,  410,  307 }, {  971,  728,  546,  410,  307 }, {  971,  728,  546,  410,  307 } },
             { {  981,  736,  552,  414,  310 }, {  981,  736,  552,  414,  310 }, {  981,  736,  552,  414,  310 } },
             { {  990,  743,  557,  418,  313 }, {  990,  743,  557,  418,  313 }, {  990,  743,  557,  418,  313 } },
             { {  999,  749,  562,  421,  316 }, {  999,  749,  562,  421,  316 }, {  999,  749,  562,  421,  316 } },
             { { 1008,  756,  567,  425,  319 }, { 1008,  756,  567,  425,  319 }, { 1008,  756,  567,  425,  319 } },
             { { 1018,  764,  573,  429,  322 }, { 1018,  764,  573,  429,  322 }, { 1018,  764,  573,  429,  322 } },
             { { 1027,  770,  578,  433,  325 }, { 1027,  770,  578,  433,  325 }, { 1027,  770,  578,  433,  325 } },
             { { 1037,  778,  583,  437,  328 }, { 1037,  778,  583,  437,  328 }, { 1037,  778,  583,  437,  328 } },
             { { 1047,  785,  589,  442,  331 }, { 1047,  785,  589,  442,  331 }, { 1047,  785,  589,  442,  331 } },
             { { 1056,  792,  594,  446,  334 }, { 1056,  792,  594,  446,  334 }, { 1056,  792,  594,  446,  334 } },
             { { 1066,  800,  600,  450,  337 }, { 1066,  800,  600,  450,  337 }, { 1066,  800,  600,  450,  337 } },
             { { 1076,  807,  605,  454,  340 }, { 1076,  807,  605,  454,  340 }, { 1076,  807,  605,  454,  340 } },
             { { 1086,  815,  611,  458,  344 }, { 1086,  815,  611,  458,  344 }, { 1086,  815,  611,  458,  344 } },
             { { 1097,  823,  617,  463,  347 }, { 1097,  823,  617,  463,  347 }, { 1097,  823,  617,  463,  347 } },
             { { 1107,  830,  623,  467,  350 }, { 1107,  830,  623,  467,  350 }, { 1107,  830,  623,  467,  350 } },
             { { 1117,  838,  628,  471,  353 }, { 1117,  838,  628,  471,  353 }, { 1117,  838,  628,  471,  353 } },
             { { 1128,  846,  635,  476,  357 }, { 1128,  846,  635,  476,  357 }, { 1128,  846,  635,  476,  357 } },
             { { 1138,  854,  640,  480,  360 }, { 1138,  854,  640,  480,  360 }, { 1138,  854,  640,  480,  360 } },
             { { 1149,  862,  646,  485,  364 }, { 1149,  862,  646,  485,  364 }, { 1149,  862,  646,  485,  364 } },
             { { 1160,  870,  653,  489,  367 }, { 1160,  870,  653,  489,  367 }, { 1160,  870,  653,  489,  367 } },
             { { 1170,  878,  658,  494,  370 }, { 1170,  878,  658,  494,  370 }, { 1170,  878,  658,  494,  370 } },
             { { 1181,  886,  664,  498,  374 }, { 1181,  886,  664,  498,  374 }, { 1181,  886,  664,  498,  374 } },
             { { 1192,  894,  671,  503,  377 }, { 1192,  894,  671,  503,  377 }, { 1192,  894,  671,  503,  377 } },
             { { 1204,  903,  677,  508,  381 }, { 1204,  903,  677,  508,  381 }, { 1204,  903,  677,  508,  381 } },
             { { 1215,  911,  683,  513,  384 }, { 1215,  911,  683,  513,  384 }, { 1215,  911,  683,  513,  384 } },
             { { 1226,  920,  690,  517,  388 }, { 1226,  920,  690,  517,  388 }, { 1226,  920,  690,  517,  388 } },
             { { 1238,  929,  696,  522,  392 }, { 1238,  929,  696,  522,  392 }, { 1238,  929,  696,  522,  392 } },
             { { 1249,  937,  703,  527,  395 }, { 1249,  937,  703,  527,  395 }, { 1249,  937,  703,  527,  395 } },
             { { 1261,  946,  709,  532,  399 }, { 1261,  946,  709,  532,  399 }, { 1261,  946,  709,  532,  399 } },
             { { 1273,  955,  716,  537,  403 }, { 1273,  955,  716,  537,  403 }, { 1273,  955,  716,  537,  403 } },
             { { 1285,  964,  723,  542,  407 }, { 1285,  964,  723,  542,  407 }, { 1285,  964,  723,  542,  407 } },
             { { 1297,  973,  730,  547,  410 }, { 1297,  973,  730,  547,  410 }, { 1297,  973,  730,  547,  410 } },
             { { 1309,  982,  736,  552,  414 }, { 1309,  982,  736,  552,  414 }, { 1309,  982,  736,  552,  414 } },
             { { 1321,  991,  743,  557,  418 }, { 1321,  991,  743,  557,  418 }, { 1321,  991,  743,  557,  418 } },
             { { 1334, 1001,  750,  563,  422 }, { 1334, 1001,  750,  563,  422 }, { 1334, 1001,  750,  563,  422 } },
             { { 1346, 1010,  757,  568,  426 }, { 1346, 1010,  757,  568,  426 }, { 1346, 1010,  757,  568,  426 } },
             { { 1359, 1019,  764,  573,  430 }, { 1359, 1019,  764,  573,  430 }, { 1359, 1019,  764,  573,  430 } },
             { { 1371, 1028,  771,  578,  434 }, { 1371, 1028,  771,  578,  434 }, { 1371, 1028,  771,  578,  434 } },
             { { 1384, 1038,  779,  584,  438 }, { 1384, 1038,  779,  584,  438 }, { 1384, 1038,  779,  584,  438 } },
             { { 1397, 1048,  786,  589,  442 }, { 1397, 1048,  786,  589,  442 }, { 1397, 1048,  786,  589,  442 } },
             { { 1410, 1058,  793,  595,  446 }, { 1410, 1058,  793,  595,  446 }, { 1410, 1058,  793,  595,  446 } },
             { { 1423, 1067,  800,  600,  450 }, { 1423, 1067,  800,  600,  450 }, { 1423, 1067,  800,  600,  450 } },
             { { 1437, 1078,  808,  606,  455 }, { 1437, 1078,  808,  606,  455 }, { 1437, 1078,  808,  606,  455 } },
             { { 1450, 1088,  816,  612,  459 }, { 1450, 1088,  816,  612,  459 }, { 1450, 1088,  816,  612,  459 } },
             { { 1464, 1098,  824,  618,  463 }, { 1464, 1098,  824,  618,  463 }, { 1464, 1098,  824,  618,  463 } },
             { { 1477, 1108,  831,  623,  467 }, { 1477, 1108,  831,  623,  467 }, { 1477, 1108,  831,  623,  467 } },
             { { 1491, 1118,  839,  629,  472 }, { 1491, 1118,  839,  629,  472 }, { 1491, 1118,  839,  629,  472 } },
             { { 1505, 1129,  847,  635,  476 }, { 1505, 1129,  847,  635,  476 }, { 1505, 1129,  847,  635,  476 } },
             { { 1519, 1139,  854,  641,  481 }, { 1519, 1139,  854,  641,  481 }, { 1519, 1139,  854,  641,  481 } },
             { { 1534, 1151,  863,  647,  485 }, { 1534, 1151,  863,  647,  485 }, { 1534, 1151,  863,  647,  485 } },
             { { 1548, 1161,  871,  653,  490 }, { 1548, 1161,  871,  653,  490 }, { 1548, 1161,  871,  653,  490 } },
             { { 1562, 1172,  879,  659,  494 }, { 1562, 1172,  879,  659,  494 }, { 1562, 1172,  879,  659,  494 } },
             { { 1577, 1183,  887,  665,  499 }, { 1577, 1183,  887,  665,  499 }, { 1577, 1183,  887,  665,  499 } },
             { { 1592, 1194,  896,  672,  504 }, { 1592, 1194,  896,  672,  504 }, { 1592, 1194,  896,  672,  504 } },
             { { 1607, 1205,  904,  678,  508 }, { 1607, 1205,  904,  678,  508 }, { 1607, 1205,  904,  678,  508 } },
             { { 1622, 1217,  912,  684,  513 }, { 1622, 1217,  912,  684,  513 }, { 1622, 1217,  912,  684,  513 } },
             { { 1637, 1228,  921,  691,  518 }, { 1637, 1228,  921,  691,  518 }, { 1637, 1228,  921,  691,  518 } },
             { { 1652, 1239,  929,  697,  523 }, { 1652, 1239,  929,  697,  523 }, { 1652, 1239,  929,  697,  523 } },
             { { 1668, 1251,  938,  704,  528 }, { 1668, 1251,  938,  704,  528 }, { 1668, 1251,  938,  704,  528 } },
             { { 1683, 1262,  947,  710,  533 }, { 1683, 1262,  947,  710,  533 }, { 1683, 1262,  947,  710,  533 } },
             { { 1699, 1274,  956,  717,  538 }, { 1699, 1274,  956,  717,  538 }, { 1699, 1274,  956,  717,  538 } },
             { { 1715, 1286,  965,  724,  543 }, { 1715, 1286,  965,  724,  543 }, { 1715, 1286,  965,  724,  543 } },
             { { 1731, 1298,  974,  730,  548 }, { 1731, 1298,  974,  730,  548 }, { 1731, 1298,  974,  730,  548 } },
             { { 1747, 1310,  983,  737,  553 }, { 1747, 1310,  983,  737,  553 }, { 1747, 1310,  983,  737,  553 } },
             { { 1764, 1323,  992,  744,  558 }, { 1764, 1323,  992,  744,  558 }, { 1764, 1323,  992,  744,  558 } },
             { { 1780, 1335, 1001,  751,  563 }, { 1780, 1335, 1001,  751,  563 }, { 1780, 1335, 1001,  751,  563 } },
             { { 1797, 1348, 1011,  758,  569 }, { 1797, 1348, 1011,  758,  569 }, { 1797, 1348, 1011,  758,  569 } },
             { { 1814, 1361, 1020,  765,  574 }, { 1814, 1361, 1020,  765,  574 }, { 1814, 1361, 1020,  765,  574 } },
             { { 1831, 1373, 1030,  772,  579 }, { 1831, 1373, 1030,  772,  579 }, { 1831, 1373, 1030,  772,  579 } },
             { { 1848, 1386, 1040,  780,  585 }, { 1848, 1386, 1040,  780,  585 }, { 1848, 1386, 1040,  780,  585 } },
             { { 1865, 1399, 1049,  787,  590 }, { 1865, 1399, 1049,  787,  590 }, { 1865, 1399, 1049,  787,  590 } },
             { { 1882, 1412, 1059,  794,  595 }, { 1882, 1412, 1059,  794,  595 }, { 1882, 1412, 1059,  794,  595 } },
             { { 1900, 1425, 1069,  802,  601 }, { 1900, 1425, 1069,  802,  601 }, { 1900, 1425, 1069,  802,  601 } },
             { { 1918, 1439, 1079,  809,  607 }, { 1918, 1439, 1079,  809,  607 }, { 1918, 1439, 1079,  809,  607 } },
             { { 1936, 1452, 1089,  817,  613 }, { 1936, 1452, 1089,  817,  613 }, { 1936, 1452, 1089,  817,  613 } },
             { { 1954, 1466, 1099,  824,  618 }, { 1954, 1466, 1099,  824,  618 }, { 1954, 1466, 1099,  824,  618 } },
             { { 1972, 1479, 1109,  832,  624 }, { 1972, 1479, 1109,  832,  624 }, { 1972, 1479, 1109,  832,  624 } },
             { { 1991, 1493, 1120,  840,  630 }, { 1991, 1493, 1120,  840,  630 }, { 1991, 1493, 1120,  840,  630 } },
             { { 2009, 1507, 1130,  848,  636 }, { 2009, 1507, 1130,  848,  636 }, { 2009, 1507, 1130,  848,  636 } },
             { { 2028, 1521, 1141,  856,  642 }, { 2028, 1521, 1141,  856,  642 }, { 2028, 1521, 1141,  856,  642 } },
             { { 2047, 1535, 1151,  864,  648 }, { 2047, 1535, 1151,  864,  648 }, { 2047, 1535, 1151,  864,  648 } },
             { { 2066, 1550, 1162,  872,  654 }, { 2066, 1550, 1162,  872,  654 }, { 2066, 1550, 1162,  872,  654 } },
             { { 2086, 1565, 1173,  880,  660 }, { 2086, 1565, 1173,  880,  660 }, { 2086, 1565, 1173,  880,  660 } },
             { { 2105, 1579, 1184,  888,  666 }, { 2105, 1579, 1184,  888,  666 }, { 2105, 1579, 1184,  888,  666 } },
             { { 2125, 1594, 1195,  896,  672 }, { 2125, 1594, 1195,  896,  672 }, { 2125, 1594, 1195,  896,  672 } },
             { { 2145, 1609, 1207,  905,  679 }, { 2145, 1609, 1207,  905,  679 }, { 2145, 1609, 1207,  905,  679 } },
             { { 2165, 1624, 1218,  913,  685 }, { 2165, 1624, 1218,  913,  685 }, { 2165, 1624, 1218,  913,  685 } },
             { { 2185, 1639, 1229,  922,  691 }, { 2185, 1639, 1229,  922,  691 }, { 2185, 1639, 1229,  922,  691 } },
             { { 2206, 1655, 1241,  931,  698 }, { 2206, 1655, 1241,  931,  698 }, { 2206, 1655, 1241,  931,  698 } },
             { { 2226, 1670, 1252,  939,  704 }, { 2226, 1670, 1252,  939,  704 }, { 2226, 1670, 1252,  939,  704 } },
             { { 2247, 1685, 1264,  948,  711 }, { 2247, 1685, 1264,  948,  711 }, { 2247, 1685, 1264,  948,  711 } },
             { { 2268, 1701, 1276,  957,  718 }, { 2268, 1701, 1276,  957,  718 }, { 2268, 1701, 1276,  957,  718 } },
             { { 2289, 1717, 1288,  966,  724 }, { 2289, 1717, 1288,  966,  724 }, { 2289, 1717, 1288,  966,  724 } },
             { { 2311, 1733, 1300,  975,  731 }, { 2311, 1733, 1300,  975,  731 }, { 2311, 1733, 1300,  975,  731 } },
             { { 2332, 1749, 1312,  984,  738 }, { 2332, 1749, 1312,  984,  738 }, { 2332, 1749, 1312,  984,  738 } },
             { { 2354, 1766, 1324,  993,  745 }, { 2354, 1766, 1324,  993,  745 }, { 2354, 1766, 1324,  993,  745 } },
             { { 2376, 1782, 1337, 1002,  752 }, { 2376, 1782, 1337, 1002,  752 }, { 2376, 1782, 1337, 1002,  752 } },
             { { 2398, 1799, 1349, 1012,  759 }, { 2398, 1799, 1349, 1012,  759 }, { 2398, 1799, 1349, 1012,  759 } },
             { { 2421, 1816, 1362, 1021,  766 }, { 2421, 1816, 1362, 1021,  766 }, { 2421, 1816, 1362, 1021,  766 } },
             { { 2444, 1833, 1375, 1031,  773 }, { 2444, 1833, 1375, 1031,  773 }, { 2444, 1833, 1375, 1031,  773 } },
             { { 2466, 1850, 1387, 1040,  780 }, { 2466, 1850, 1387, 1040,  780 }, { 2466, 1850, 1387, 1040,  780 } },
             { { 2490, 1868, 1401, 1050,  788 }, { 2490, 1868, 1401, 1050,  788 }, { 2490, 1868, 1401, 1050,  788 } },
             { { 2513, 1885, 1414, 1060,  795 }, { 2513, 1885, 1414, 1060,  795 }, { 2513, 1885, 1414, 1060,  795 } },
             { { 2536, 1902, 1427, 1070,  802 }, { 2536, 1902, 1427, 1070,  802 }, { 2536, 1902, 1427, 1070,  802 } },
             { { 2560, 1920, 1440, 1080,  810 }, { 2560, 1920, 1440, 1080,  810 }, { 2560, 1920, 1440, 1080,  810 } },
             { { 2584, 1938, 1454, 1090,  818 }, { 2584, 1938, 1454, 1090,  818 }, { 2584, 1938, 1454, 1090,  818 } },
             { { 2608, 1956, 1467, 1100,  825 }, { 2608, 1956, 1467, 1100,  825 }, { 2608, 1956, 1467, 1100,  825 } },
             { { 2633, 1975, 1481, 1111,  833 }, { 2633, 1975, 1481, 1111,  833 }, { 2633, 1975, 1481, 1111,  833 } },
             { { 2657, 1993, 1495, 1121,  841 }, { 2657, 1993, 1495, 1121,  841 }, { 2657, 1993, 1495, 1121,  841 } },
             { { 2682, 2012, 1509, 1131,  849 }, { 2682, 2012, 1509, 1131,  849 }, { 2682, 2012, 1509, 1131,  849 } },
             { { 2707, 2030, 1523, 1142,  857 }, { 2707, 2030, 1523, 1142,  857 }, { 2707, 2030, 1523, 1142,  857 } },
             { { 2733, 2050, 1537, 1153,  865 }, { 2733, 2050, 1537, 1153,  865 }, { 2733, 2050, 1537, 1153,  865 } },
             { { 2758, 2069, 1551, 1164,  873 }, { 2758, 2069, 1551, 1164,  873 }, { 2758, 2069, 1551, 1164,  873 } },
             { { 2784, 2088, 1566, 1175,  881 }, { 2784, 2088, 1566, 1175,  881 }, { 2784, 2088, 1566, 1175,  881 } },
             { { 2810, 2108, 1581, 1185,  889 }, { 2810, 2108, 1581, 1185,  889 }, { 2810, 2108, 1581, 1185,  889 } },
             { { 2836, 2127, 1595, 1196,  897 }, { 2836, 2127, 1595, 1196,  897 }, { 2836, 2127, 1595, 1196,  897 } },
             { { 2863, 2147, 1610, 1208,  906 }, { 2863, 2147, 1610, 1208,  906 }, { 2863, 2147, 1610, 1208,  906 } },
             { { 2890, 2168, 1626, 1219,  914 }, { 2890, 2168, 1626, 1219,  914 }, { 2890, 2168, 1626, 1219,  914 } },
             { { 2917, 2188, 1641, 1231,  923 }, { 2917, 2188, 1641, 1231,  923 }, { 2917, 2188, 1641, 1231,  923 } },
             { { 2944, 2208, 1656, 1242,  932 }, { 2944, 2208, 1656, 1242,  932 }, { 2944, 2208, 1656, 1242,  932 } },
             { { 2972, 2229, 1672, 1254,  940 }, { 2972, 2229, 1672, 1254,  940 }, { 2972, 2229, 1672, 1254,  940 } },
             { { 3000, 2250, 1688, 1266,  949 }, { 3000, 2250, 1688, 1266,  949 }, { 3000, 2250, 1688, 1266,  949 } },
             { { 3028, 2271, 1703, 1277,  958 }, { 3028, 2271, 1703, 1277,  958 }, { 3028, 2271, 1703, 1277,  958 } },
             { { 3056, 2292, 1719, 1289,  967 }, { 3056, 2292, 1719, 1289,  967 }, { 3056, 2292, 1719, 1289,  967 } },
             { { 3085, 2314, 1735, 1301,  976 }, { 3085, 2314, 1735, 1301,  976 }, { 3085, 2314, 1735, 1301,  976 } },
             { { 3113, 2335, 1751, 1313,  985 }, { 3113, 2335, 1751, 1313,  985 }, { 3113, 2335, 1751, 1313,  985 } },
             { { 3143, 2357, 1768, 1326,  994 }, { 3143, 2357, 1768, 1326,  994 }, { 3143, 2357, 1768, 1326,  994 } },
             { { 3172, 2379, 1784, 1338, 1004 }, { 3172, 2379, 1784, 1338, 1004 }, { 3172, 2379, 1784, 1338, 1004 } },
             { { 3202, 2402, 1801, 1351, 1013 }, { 3202, 2402, 1801, 1351, 1013 }, { 3202, 2402, 1801, 1351, 1013 } },
             { { 3232, 2424, 1818, 1364, 1023 }, { 3232, 2424, 1818, 1364, 1023 }, { 3232, 2424, 1818, 1364, 1023 } },
             { { 3262, 2447, 1835, 1376, 1032 }, { 3262, 2447, 1835, 1376, 1032 }, { 3262, 2447, 1835, 1376, 1032 } },
             { { 3292, 2469, 1852, 1389, 1042 }, { 3292, 2469, 1852, 1389, 1042 }, { 3292, 2469, 1852, 1389, 1042 } },
             { { 3323, 2492, 1869, 1402, 1051 }, { 3323, 2492, 1869, 1402, 1051 }, { 3323, 2492, 1869, 1402, 1051 } },
             { { 3354, 2516, 1887, 1415, 1061 }, { 3354, 2516, 1887, 1415, 1061 }, { 3354, 2516, 1887, 1415, 1061 } },
             { { 3386, 2540, 1905, 1428, 1071 }, { 3386, 2540, 1905, 1428, 1071 }, { 3386, 2540, 1905, 1428, 1071 } },
             { { 3417, 2563, 1922, 1442, 1081 }, { 3417, 2563, 1922, 1442, 1081 }, { 3417, 2563, 1922, 1442, 1081 } },
             { { 3449, 2587, 1940, 1455, 1091 }, { 3449, 2587, 1940, 1455, 1091 }, { 3449, 2587, 1940, 1455, 1091 } },
             { { 3482, 2612, 1959, 1469, 1102 }, { 3482, 2612, 1959, 1469, 1102 }, { 3482, 2612, 1959, 1469, 1102 } },
             { { 3514, 2636, 1977, 1482, 1112 }, { 3514, 2636, 1977, 1482, 1112 }, { 3514, 2636, 1977, 1482, 1112 } },
             { { 3547, 2660, 1995, 1496, 1122 }, { 3547, 2660, 1995, 1496, 1122 }, { 3547, 2660, 1995, 1496, 1122 } },
             { { 3580, 2685, 2014, 1510, 1133 }, { 3580, 2685, 2014, 1510, 1133 }, { 3580, 2685, 2014, 1510, 1133 } },
             { { 3614, 2711, 2033, 1525, 1143 }, { 3614, 2711, 2033, 1525, 1143 }, { 3614, 2711, 2033, 1525, 1143 } },
             { { 3648, 2736, 2052, 1539, 1154 }, { 3648, 2736, 2052, 1539, 1154 }, { 3648, 2736, 2052, 1539, 1154 } },
             { { 3682, 2762, 2071, 1553, 1165 }, { 3682, 2762, 2071, 1553, 1165 }, { 3682, 2762, 2071, 1553, 1165 } },
             { { 3716, 2787, 2090, 1568, 1176 }, { 3716, 2787, 2090, 1568, 1176 }, { 3716, 2787, 2090, 1568, 1176 } },
             { { 3751, 2813, 2110, 1582, 1187 }, { 3751, 2813, 2110, 1582, 1187 }, { 3751, 2813, 2110, 1582, 1187 } },
             { { 3786, 2840, 2130, 1597, 1198 }, { 3786, 2840, 2130, 1597, 1198 }, { 3786, 2840, 2130, 1597, 1198 } },
             { { 3822, 2867, 2150, 1612, 1209 }, { 3822, 2867, 2150, 1612, 1209 }, { 3822, 2867, 2150, 1612, 1209 } },
             { { 3858, 2894, 2170, 1628, 1221 }, { 3858, 2894, 2170, 1628, 1221 }, { 3858, 2894, 2170, 1628, 1221 } },
             { { 3894, 2921, 2190, 1643, 1232 }, { 3894, 2921, 2190, 1643, 1232 }, { 3894, 2921, 2190, 1643, 1232 } },
             { { 3930, 2948, 2211, 1658, 1243 }, { 3930, 2948, 2211, 1658, 1243 }, { 3930, 2948, 2211, 1658, 1243 } },
             { { 3967, 2975, 2231, 1674, 1255 }, { 3967, 2975, 2231, 1674, 1255 }, { 3967, 2975, 2231, 1674, 1255 } },
             { { 4004, 3003, 2252, 1689, 1267 }, { 4004, 3003, 2252, 1689, 1267 }, { 4004, 3003, 2252, 1689, 1267 } },
             { { 4042, 3032, 2274, 1705, 1279 }, { 4042, 3032, 2274, 1705, 1279 }, { 4042, 3032, 2274, 1705, 1279 } },
             { { 4079, 3059, 2294, 1721, 1291 }, { 4079, 3059, 2294, 1721, 1291 }, { 4079, 3059, 2294, 1721, 1291 } },
             { { 4118, 3089, 2316, 1737, 1303 }, { 4118, 3089, 2316, 1737, 1303 }, { 4118, 3089, 2316, 1737, 1303 } },
             { { 4156, 3117, 2338, 1753, 1315 }, { 4156, 3117, 2338, 1753, 1315 }, { 4156, 3117, 2338, 1753, 1315 } },
             { { 4195, 3146, 2360, 1770, 1327 }, { 4195, 3146, 2360, 1770, 1327 }, { 4195, 3146, 2360, 1770, 1327 } },
             { { 4234, 3176, 2382, 1786, 1340 }, { 4234, 3176, 2382, 1786, 1340 }, { 4234, 3176, 2382, 1786, 1340 } },
             { { 4274, 3206, 2404, 1803, 1352 }, { 4274, 3206, 2404, 1803, 1352 }, { 4274, 3206, 2404, 1803, 1352 } },
             { { 4314, 3236, 2427, 1820, 1365 }, { 4314, 3236, 2427, 1820, 1365 }, { 4314, 3236, 2427, 1820, 1365 } },
             { { 4354, 3266, 2449, 1837, 1378 }, { 4354, 3266, 2449, 1837, 1378 }, { 4354, 3266, 2449, 1837, 1378 } },
             { { 4395, 3296, 2472, 1854, 1391 }, { 4395, 3296, 2472, 1854, 1391 }, { 4395, 3296, 2472, 1854, 1391 } },
             { { 4436, 3327, 2495, 1871, 1404 }, { 4436, 3327, 2495, 1871, 1404 }, { 4436, 3327, 2495, 1871, 1404 } },
             { { 4478, 3359, 2519, 1889, 1417 }, { 4478, 3359, 2519, 1889, 1417 }, { 4478, 3359, 2519, 1889, 1417 } },
             { { 4520, 3390, 2543, 1907, 1430 }, { 4520, 3390, 2543, 1907, 1430 }, { 4520, 3390, 2543, 1907, 1430 } },
             { { 4562, 3422, 2566, 1925, 1443 }, { 4562, 3422, 2566, 1925, 1443 }, { 4562, 3422, 2566, 1925, 1443 } },
             { { 4605, 3454, 2590, 1943, 1457 }, { 4605, 3454, 2590, 1943, 1457 }, { 4605, 3454, 2590, 1943, 1457 } },
             { { 4648, 3486, 2615, 1961, 1471 }, { 4648, 3486, 2615, 1961, 1471 }, { 4648, 3486, 2615, 1961, 1471 } },
             { { 4691, 3518, 2639, 1979, 1484 }, { 4691, 3518, 2639, 1979, 1484 }, { 4691, 3518, 2639, 1979, 1484 } },
             { { 4735, 3551, 2663, 1998, 1498 }, { 4735, 3551, 2663, 1998, 1498 }, { 4735, 3551, 2663, 1998, 1498 } },
             { { 4779, 3584, 2688, 2016, 1512 }, { 4779, 3584, 2688, 2016, 1512 }, { 4779, 3584, 2688, 2016, 1512 } },
             { { 4824, 3618, 2714, 2035, 1526 }, { 4824, 3618, 2714, 2035, 1526 }, { 4824, 3618, 2714, 2035, 1526 } },
             { { 4869, 3652, 2739, 2054, 1541 }, { 4869, 3652, 2739, 2054, 1541 }, { 4869, 3652, 2739, 2054, 1541 } },
             { { 4915, 3686, 2765, 2074, 1555 }, { 4915, 3686, 2765, 2074, 1555 }, { 4915, 3686, 2765, 2074, 1555 } },
             { { 4961, 3721, 2791, 2093, 1570 }, { 4961, 3721, 2791, 2093, 1570 }, { 4961, 3721, 2791, 2093, 1570 } },
             { { 5007, 3755, 2816, 2112, 1584 }, { 5007, 3755, 2816, 2112, 1584 }, { 5007, 3755, 2816, 2112, 1584 } },
             { { 5054, 3791, 2843, 2132, 1599 }, { 5054, 3791, 2843, 2132, 1599 }, { 5054, 3791, 2843, 2132, 1599 } },
             { { 5102, 3827, 2870, 2152, 1614 }, { 5102, 3827, 2870, 2152, 1614 }, { 5102, 3827, 2870, 2152, 1614 } },
             { { 5149, 3862, 2896, 2172, 1629 }, { 5149, 3862, 2896, 2172, 1629 }, { 5149, 3862, 2896, 2172, 1629 } },
             { { 5198, 3899, 2924, 2193, 1645 }, { 5198, 3899, 2924, 2193, 1645 }, { 5198, 3899, 2924, 2193, 1645 } },
             { { 5246, 3935, 2951, 2213, 1660 }, { 5246, 3935, 2951, 2213, 1660 }, { 5246, 3935, 2951, 2213, 1660 } },
             { { 5295, 3971, 2978, 2234, 1675 }, { 5295, 3971, 2978, 2234, 1675 }, { 5295, 3971, 2978, 2234, 1675 } },
             { { 5345, 4009, 3007, 2255, 1691 }, { 5345, 4009, 3007, 2255, 1691 }, { 5345, 4009, 3007, 2255, 1691 } },
             { { 5395, 4046, 3035, 2276, 1707 }, { 5395, 4046, 3035, 2276, 1707 }, { 5395, 4046, 3035, 2276, 1707 } },
             { { 5445, 4084, 3063, 2297, 1723 }, { 5445, 4084, 3063, 2297, 1723 }, { 5445, 4084, 3063, 2297, 1723 } },
             { { 5496, 4122, 3092, 2319, 1739 }, { 5496, 4122, 3092, 2319, 1739 }, { 5496, 4122, 3092, 2319, 1739 } },
             { { 5548, 4161, 3121, 2341, 1755 }, { 5548, 4161, 3121, 2341, 1755 }, { 5548, 4161, 3121, 2341, 1755 } },
             { { 5600, 4200, 3150, 2363, 1772 }, { 5600, 4200, 3150, 2363, 1772 }, { 5600, 4200, 3150, 2363, 1772 } },
             { { 5652, 4239, 3179, 2384, 1788 }, { 5652, 4239, 3179, 2384, 1788 }, { 5652, 4239, 3179, 2384, 1788 } },
             { { 5705, 4279, 3209, 2407, 1805 }, { 5705, 4279, 3209, 2407, 1805 }, { 5705, 4279, 3209, 2407, 1805 } },
             { { 5759, 4319, 3239, 2430, 1822 }, { 5759, 4319, 3239, 2430, 1822 }, { 5759, 4319, 3239, 2430, 1822 } },
             { { 5812, 4359, 3269, 2452, 1839 }, { 5812, 4359, 3269, 2452, 1839 }, { 5812, 4359, 3269, 2452, 1839 } },
             { { 5867, 4400, 3300, 2475, 1856 }, { 5867, 4400, 3300, 2475, 1856 }, { 5867, 4400, 3300, 2475, 1856 } },
             { { 5922, 4442, 3331, 2498, 1874 }, { 5922, 4442, 3331, 2498, 1874 }, { 5922, 4442, 3331, 2498, 1874 } },
             { { 5977, 4483, 3362, 2522, 1891 }, { 5977, 4483, 3362, 2522, 1891 }, { 5977, 4483, 3362, 2522, 1891 } },
             { { 6033, 4525, 3394, 2545, 1909 }, { 6033, 4525, 3394, 2545, 1909 }, { 6033, 4525, 3394, 2545, 1909 } },
             { { 6090, 4568, 3426, 2569, 1927 }, { 6090, 4568, 3426, 2569, 1927 }, { 6090, 4568, 3426, 2569, 1927 } },
             { { 6147, 4610, 3458, 2593, 1945 }, { 6147, 4610, 3458, 2593, 1945 }, { 6147, 4610, 3458, 2593, 1945 } },
             { { 6204, 4653, 3490, 2617, 1963 }, { 6204, 4653, 3490, 2617, 1963 }, { 6204, 4653, 3490, 2617, 1963 } },
             { { 6262, 4697, 3522, 2642, 1981 }, { 6262, 4697, 3522, 2642, 1981 }, { 6262, 4697, 3522, 2642, 1981 } },
             { { 6321, 4741, 3556, 2667, 2000 }, { 6321, 4741, 3556, 2667, 2000 }, { 6321, 4741, 3556, 2667, 2000 } },
             { { 6380, 4785, 3589, 2692, 2019 }, { 6380, 4785, 3589, 2692, 2019 }, { 6380, 4785, 3589, 2692, 2019 } },
             { { 6440, 4830, 3623, 2717, 2038 }, { 6440, 4830, 3623, 2717, 2038 }, { 6440, 4830, 3623, 2717, 2038 } },
             { { 6500, 4875, 3656, 2742, 2057 }, { 6500, 4875, 3656, 2742, 2057 }, { 6500, 4875, 3656, 2742, 2057 } },
             { { 6561, 4921, 3691, 2768, 2076 }, { 6561, 4921, 3691, 2768, 2076 }, { 6561, 4921, 3691, 2768, 2076 } },
             { { 6622, 4967, 3725, 2794, 2095 }, { 6622, 4967, 3725, 2794, 2095 }, { 6622, 4967, 3725, 2794, 2095 } },
             { { 6684, 5013, 3760, 2820, 2115 }, { 6684, 5013, 3760, 2820, 2115 }, { 6684, 5013, 3760, 2820, 2115 } },
             { { 6747, 5060, 3795, 2846, 2135 }, { 6747, 5060, 3795, 2846, 2135 }, { 6747, 5060, 3795, 2846, 2135 } },
             { { 6810, 5108, 3831, 2873, 2155 }, { 6810, 5108, 3831, 2873, 2155 }, { 6810, 5108, 3831, 2873, 2155 } },
             { { 6874, 5156, 3867, 2900, 2175 }, { 6874, 5156, 3867, 2900, 2175 }, { 6874, 5156, 3867, 2900, 2175 } },
             { { 6938, 5204, 3903, 2927, 2195 }, { 6938, 5204, 3903, 2927, 2195 }, { 6938, 5204, 3903, 2927, 2195 } },
             { { 7003, 5252, 3939, 2954, 2216 }, { 7003, 5252, 3939, 2954, 2216 }, { 7003, 5252, 3939, 2954, 2216 } },
             { { 7069, 5302, 3976, 2982, 2237 }, { 7069, 5302, 3976, 2982, 2237 }, { 7069, 5302, 3976, 2982, 2237 } },
             { { 7135, 5351, 4013, 3010, 2258 }, { 7135, 5351, 4013, 3010, 2258 }, { 7135, 5351, 4013, 3010, 2258 } },
             { { 7202, 5402, 4051, 3038, 2279 }, { 7202, 5402, 4051, 3038, 2279 }, { 7202, 5402, 4051, 3038, 2279 } },
             { { 7269, 5452, 4089, 3067, 2300 }, { 7269, 5452, 4089, 3067, 2300 }, { 7269, 5452, 4089, 3067, 2300 } },
             { { 7337, 5503, 4127, 3095, 2321 }, { 7337, 5503, 4127, 3095, 2321 }, { 7337, 5503, 4127, 3095, 2321 } },
             { { 7406, 5555, 4166, 3124, 2343 }, { 7406, 5555, 4166, 3124, 2343 }, { 7406, 5555, 4166, 3124, 2343 } },
             { { 7475, 5606, 4205, 3154, 2365 }, { 7475, 5606, 4205, 3154, 2365 }, { 7475, 5606, 4205, 3154, 2365 } },
             { { 7545, 5659, 4244, 3183, 2387 }, { 7545, 5659, 4244, 3183, 2387 }, { 7545, 5659, 4244, 3183, 2387 } },
             { { 7616, 5712, 4284, 3213, 2410 }, { 7616, 5712, 4284, 3213, 2410 }, { 7616, 5712, 4284, 3213, 2410 } },
             { { 7687, 5765, 4324, 3243, 2432 }, { 7687, 5765, 4324, 3243, 2432 }, { 7687, 5765, 4324, 3243, 2432 } },
             { { 7759, 5819, 4364, 3273, 2455 }, { 7759, 5819, 4364, 3273, 2455 }, { 7759, 5819, 4364, 3273, 2455 } },
             { { 7832, 5874, 4406, 3304, 2478 }, { 7832, 5874, 4406, 3304, 2478 }, { 7832, 5874, 4406, 3304, 2478 } },
             { { 7905, 5929, 4447, 3335, 2501 }, { 7905, 5929, 4447, 3335, 2501 }, { 7905, 5929, 4447, 3335, 2501 } },
             { { 7979, 5984, 4488, 3366, 2525 }, { 7979, 5984, 4488, 3366, 2525 }, { 7979, 5984, 4488, 3366, 2525 } },
             { { 8054, 6041, 4530, 3398, 2548 }, { 8054, 6041, 4530, 3398, 2548 }, { 8054, 6041, 4530, 3398, 2548 } },
             { { 8129, 6097, 4573, 3429, 2572 }, { 8129, 6097, 4573, 3429, 2572 }, { 8129, 6097, 4573, 3429, 2572 } },
             { { 8205, 6154, 4615, 3461, 2596 }, { 8205, 6154, 4615, 3461, 2596 }, { 8205, 6154, 4615, 3461, 2596 } },
             { { 8282, 6212, 4659, 3494, 2620 }, { 8282, 6212, 4659, 3494, 2620 }, { 8282, 6212, 4659, 3494, 2620 } },
             { { 8359, 6269, 4702, 3526, 2645 }, { 8359, 6269, 4702, 3526, 2645 }, { 8359, 6269, 4702, 3526, 2645 } },
             { { 8438, 6329, 4746, 3560, 2670 }, { 8438, 6329, 4746, 3560, 2670 }, { 8438, 6329, 4746, 3560, 2670 } },
             { { 8517, 6388, 4791, 3593, 2695 }, { 8517, 6388, 4791, 3593, 2695 }, { 8517, 6388, 4791, 3593, 2695 } },
             { { 8596, 6447, 4835, 3626, 2720 }, { 8596, 6447, 4835, 3626, 2720 }, { 8596, 6447, 4835, 3626, 2720 } },
             { { 8677, 6508, 4881, 3661, 2745 }, { 8677, 6508, 4881, 3661, 2745 }, { 8677, 6508, 4881, 3661, 2745 } },
             { { 8758, 6569, 4926, 3695, 2771 }, { 8758, 6569, 4926, 3695, 2771 }, { 8758, 6569, 4926, 3695, 2771 } },
             { { 8840, 6630, 4973, 3729, 2797 }, { 8840, 6630, 4973, 3729, 2797 }, { 8840, 6630, 4973, 3729, 2797 } },
             { { 8923, 6692, 5019, 3764, 2823 }, { 8923, 6692, 5019, 3764, 2823 }, { 8923, 6692, 5019, 3764, 2823 } },
             { { 9006, 6755, 5066, 3799, 2850 }, { 9006, 6755, 5066, 3799, 2850 }, { 9006, 6755, 5066, 3799, 2850 } },
             { { 9091, 6818, 5114, 3835, 2876 }, { 9091, 6818, 5114, 3835, 2876 }, { 9091, 6818, 5114, 3835, 2876 } },
             { { 9176, 6882, 5162, 3871, 2903 }, { 9176, 6882, 5162, 3871, 2903 }, { 9176, 6882, 5162, 3871, 2903 } },
             { { 9262, 6947, 5210, 3907, 2931 }, { 9262, 6947, 5210, 3907, 2931 }, { 9262, 6947, 5210, 3907, 2931 } },
             { { 9348, 7011, 5258, 3944, 2958 }, { 9348, 7011, 5258, 3944, 2958 }, { 9348, 7011, 5258, 3944, 2958 } },
             { { 9436, 7077, 5308, 3981, 2986 }, { 9436, 7077, 5308, 3981, 2986 }, { 9436, 7077, 5308, 3981, 2986 } },
             { { 9524, 7143, 5357, 4018, 3013 }, { 9524, 7143, 5357, 4018, 3013 }, { 9524, 7143, 5357, 4018, 3013 } },
             { { 9613, 7210, 5407, 4055, 3042 }, { 9613, 7210, 5407, 4055, 3042 }, { 9613, 7210, 5407, 4055, 3042 } },
             { { 9703, 7277, 5458, 4093, 3070 }, { 9703, 7277, 5458, 4093, 3070 }, { 9703, 7277, 5458, 4093, 3070 } },
             { { 9794, 7346, 5509, 4132, 3099 }, { 9794, 7346, 5509, 4132, 3099 }, { 9794, 7346, 5509, 4132, 3099 } },
             { { 9886, 7415, 5561, 4171, 3128 }, { 9886, 7415, 5561, 4171, 3128 }, { 9886, 7415, 5561, 4171, 3128 } },
             { { 9978, 7484, 5613, 4209, 3157 }, { 9978, 7484, 5613, 4209, 3157 }, { 9978, 7484, 5613, 4209, 3157 } },
        };

        private static double[] SocketCostData = new double[1000] {
               0.000000000000000,    0.000000000000000,    0.000000000000000,    0.000000000000000,    0.000000000000000, 
               0.000000000000000,    0.000000000000000,    0.000000000000000,    0.000000000000000,    0.000000000000000, 
               0.000000000000000,    0.000000000000000,    0.000000000000000,    0.000000000000000,    0.000000000000000, 
               0.000000000000000,    0.000000000000000,    0.000000000000000,    1.000000000000000,    1.000000000000000, 
               1.000000000000000,    1.000000000000000,    1.000000000000000,    1.000000000000000,    1.000000000000000, 
               1.000000000000000,    1.000000000000000,    1.000000000000000,    1.000000000000000,    1.000000000000000, 
               2.000000000000000,    2.000000000000000,    2.000000000000000,    2.000000000000000,    2.000000000000000, 
               2.000000000000000,    2.000000000000000,    2.000000000000000,    2.000000000000000,    2.000000000000000, 
               2.000000000000000,    2.000000000000000,    3.000000000000000,    3.000000000000000,    3.000000000000000, 
               3.000000000000000,    3.000000000000000,    3.000000000000000,    3.000000000000000,    3.000000000000000, 
               3.000000000000000,    3.000000000000000,    3.000000000000000,    3.000000000000000,    4.000000000000000, 
               4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000, 
               4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000, 
               4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000, 
               4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000, 
               4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000, 
               4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000,    4.000000000000000, 
               4.000000000000000,    4.000000000000000,    4.000000000000000,    5.000000000000000,    5.000000000000000, 
               5.000000000000000,    5.000000000000000,    5.000000000000000,    5.000000000000000,    5.000000000000000, 
               5.000000000000000,    5.000000000000000,    5.000000000000000,    5.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000,    6.000000000000000, 
               6.000000000000000,    6.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
               8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000,    8.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000, 
              30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,   30.000000000000000,
        };
#endregion

		public static int GetEffectItemBudget(Item item, int upgradeItemLevel, int maxItemLevel)
		{
			int slotType = 0;
			int newItemLevel = item.ItemLevel + upgradeItemLevel;
			if (maxItemLevel > 0 && newItemLevel > maxItemLevel)
			{
				newItemLevel = maxItemLevel;
			}

			if (slotType != -1 && item.Quality > ItemQuality.Poor)
			{
				if (item.Quality == ItemQuality.Epic)
				{
					return BudgetData[newItemLevel - 1, 0, slotType];
				}
				else if (item.Quality == ItemQuality.Rare)
				{
					return BudgetData[newItemLevel - 1, 1, slotType];
				}
				else
				{
					return BudgetData[newItemLevel - 1, 2, slotType];
				}
			}
			return 0;
		}

		public static int GetItemBudget(Item item, int upgradeItemLevel)
		{
			int slotType = GetSlotType(item.Slot);
			int newItemLevel = item.ItemLevel + upgradeItemLevel;

			if (slotType != -1 && item.Quality > ItemQuality.Poor)
			{
				if (item.Quality == ItemQuality.Epic)
				{
					return BudgetData[newItemLevel - 1, 0, slotType];
				}
				else if (item.Quality == ItemQuality.Rare)
				{
					return BudgetData[newItemLevel - 1, 1, slotType];
				}
				else
				{
					return BudgetData[newItemLevel - 1, 2, slotType];
				}
			}
			return 0;
		}

		public static float GetItemStatFromAllocation(ItemStatAllocation allocation, int itemBudget, int itemLevel)
		{
			double rawValue = Math.Round(allocation.Allocation * itemBudget / 10000.0);
			double socketPenalty = Math.Round(allocation.SocketMultiplier * SocketCostData[itemLevel - 1]);
			return (float)Math.Floor(rawValue - socketPenalty);
		}

        private static void AccumulateUpgradeStatsDBC(Stats stats, Item item, int randomSuffixId, int upgradeItemLevel)
        {
			int itemBudget = GetItemBudget(item, upgradeItemLevel);

            foreach (var allocation in item.ItemStatAllocations)
            {
                stats._rawAdditiveData[(int)allocation.Stat] = GetItemStatFromAllocation(allocation, itemBudget, item.ItemLevel);
            }
			if (randomSuffixId > 0)
			{
				RandomSuffix.AccumulateUpgradeStatsDBC(stats, randomSuffixId, itemBudget);
			}
			// Trinket procs
			if (item.Slot == ItemSlot.Trinket && item.Stats._rawSpecialEffectData != null)
			{
				float multiplier = (float)GetEffectItemBudget(item, upgradeItemLevel, 0) / (float)GetEffectItemBudget(item, 0, 0);

				foreach (SpecialEffect effect in item.Stats._rawSpecialEffectData)
				{
					if (effect == null) continue;
					SpecialEffect newEffect = stats.SpecialEffects(se => se.Equals(effect)).First();
					stats.RemoveSpecialEffect(newEffect);
					newEffect = new SpecialEffect(newEffect, true);
					newEffect.Stats.GenerateSparseData();
					for (int i = 0; i < newEffect.Stats.sparseAdditiveCount; i++)
					{
						int index = newEffect.Stats.sparseIndices[i];
						newEffect.Stats._rawAdditiveData[index] = newEffect.Stats._rawAdditiveData[index] * multiplier;
					}
					stats.AddSpecialEffect(newEffect);
				}
			}
		}

        // This function applies upgrading to the item instance's stats.
        private static void AccumulateUpgradeStatsSimple(Stats stats, Item item, int randomSuffixId, int upgradeItemLevel)
        {
            float adjustment = (float)Math.Pow(1.15, upgradeItemLevel / 15.0);
            // Apply upgrade formula based on socket count
            // Calculate the "socket count" (meta counts as 2)
            int socketCount = 0;
            for (int index = 1; index <= 3; ++index)
            {
                ItemSlot socket = item.GetSocketColor(index);
                if (socket != ItemSlot.None)
                    ++socketCount;
                if (socket == ItemSlot.Meta)
                    ++socketCount;
            }
            // Primary stats get a base adjustment of socket * 80, secondary stats half that
            int primaryBaseAdjustment = socketCount * 80;
            int secondaryBaseAdjustment = socketCount * 40;
            // Stam, SP, Amplifier, Multistrike: Not affected by base adjustments
            List<AdditiveStat> NoMultiplierRoundedStats = new List<AdditiveStat>()
            {
                AdditiveStat.Stamina,
                AdditiveStat.SpellPower
            };
            List<AdditiveStat> NoMultiplierNoRoundingStats = new List<AdditiveStat>()
            {
                AdditiveStat.SecondaryStatMultiplier
            };
            // Agi, Int, Str: Primary base adjustment
            List<AdditiveStat> PrimaryStats = new List<AdditiveStat>()
            {
                AdditiveStat.Agility,
                AdditiveStat.Intellect,
                AdditiveStat.Strength
            };
            // Everything else: Secondary base adjustment
            List<AdditiveStat> SecondaryStats = new List<AdditiveStat>()
            {
                AdditiveStat.HasteRating,
                AdditiveStat.CritRating,
                AdditiveStat.MasteryRating,
                AdditiveStat.Spirit,
                AdditiveStat.HitRating,
                AdditiveStat.DodgeRating,
                AdditiveStat.ParryRating,
                AdditiveStat.ExpertiseRating
            };
            // Perform stat accumulation
            foreach (AdditiveStat stat in NoMultiplierRoundedStats)
            {
                if (item.Stats._rawAdditiveData[(int)stat] > 0)
                    stats._rawAdditiveData[(int)stat] += (float)Math.Floor(item.Stats._rawAdditiveData[(int)stat] * adjustment) - item.Stats._rawAdditiveData[(int)stat];
            }
            foreach (AdditiveStat stat in NoMultiplierNoRoundingStats)
            {
                if (item.Stats._rawAdditiveData[(int)stat] > 0)
                    stats._rawAdditiveData[(int)stat] += item.Stats._rawAdditiveData[(int)stat] * adjustment - item.Stats._rawAdditiveData[(int)stat];
            }
            foreach (AdditiveStat stat in PrimaryStats)
            {
                float currentVal = item.Stats._rawAdditiveData[(int)stat];
                if (currentVal > 0)
                    stats._rawAdditiveData[(int)stat] += (float)Math.Round((currentVal + primaryBaseAdjustment) * adjustment) - primaryBaseAdjustment - currentVal;
            }
            foreach (AdditiveStat stat in SecondaryStats)
            {
                float currentVal = item.Stats._rawAdditiveData[(int)stat];
                if (randomSuffixId != 0)
                {
                    currentVal = RandomSuffix.GetStatValue(item, randomSuffixId, stat);
                }
                if (currentVal > 0)
                    stats._rawAdditiveData[(int)stat] += (float)Math.Round((currentVal + secondaryBaseAdjustment) * adjustment) - secondaryBaseAdjustment - currentVal;
            }
            // Armor has its own adjustment
            if (item.Stats._rawAdditiveData[(int)AdditiveStat.Armor] > 0)
            {
                float currentVal = item.Stats._rawAdditiveData[(int)AdditiveStat.Armor];
                stats._rawAdditiveData[(int)AdditiveStat.Armor] += (float)Math.Round(currentVal * (float)Math.Pow(1.042, upgradeItemLevel / 15.0)) - currentVal;
            }
            // Trinket procs
            if (item.Slot == ItemSlot.Trinket && item.Stats._rawSpecialEffectData != null)
            {
                foreach (SpecialEffect effect in item.Stats._rawSpecialEffectData)
                {
                    if (effect == null) continue;
                    if (!string.IsNullOrEmpty(effect.ModifiedBy) && effect.ModifiedBy.Contains("Budget"))
                    {
                        float currentVal = effect.Chance;
                        SpecialEffect newEffect = stats.SpecialEffects(se => se.Equals(effect)).First();
                        stats.RemoveSpecialEffect(newEffect);
                        newEffect = new SpecialEffect(newEffect, false);
                        newEffect.Chance += effect.Chance * adjustment - currentVal;
                        stats.AddSpecialEffect(newEffect);
                    }
                    foreach (AdditiveStat stat in NoMultiplierNoRoundingStats)
                    {
                        float currentVal = effect.Stats._rawAdditiveData[(int)stat];
                        if (currentVal > 0)
                        {
                            SpecialEffect newEffect = stats.SpecialEffects(se => se.Equals(effect)).First();
                            stats.RemoveSpecialEffect(newEffect);
                            newEffect = new SpecialEffect(newEffect, false);
                            newEffect.Stats._rawAdditiveData[(int)stat] +=
                                (float)Math.Floor(effect.Stats._rawAdditiveData[(int)stat] * adjustment) - currentVal;
                            stats.AddSpecialEffect(newEffect);
                        }
                    }
                    foreach (AdditiveStat stat in PrimaryStats)
                    {
                        float currentVal = effect.Stats._rawAdditiveData[(int)stat];
                        if (currentVal > 0)
                        {
                            SpecialEffect newEffect = stats.SpecialEffects(se => se.Equals(effect)).First();
                            stats.RemoveSpecialEffect(newEffect);
                            newEffect = new SpecialEffect(newEffect, false);
                            newEffect.Stats._rawAdditiveData[(int)stat] +=
                                (float)Math.Floor(effect.Stats._rawAdditiveData[(int)stat] * adjustment) - currentVal;
                            stats.AddSpecialEffect(newEffect);
                        }
                    }
                    foreach (AdditiveStat stat in SecondaryStats)
                    {
                        float currentVal = effect.Stats._rawAdditiveData[(int)stat];
                        if (currentVal > 0)
                        {
                            SpecialEffect newEffect = stats.SpecialEffects(se => se.Equals(effect)).First();
                            stats.RemoveSpecialEffect(newEffect);
                            newEffect = new SpecialEffect(newEffect, false);
                            newEffect.Stats._rawAdditiveData[(int)stat] +=
                                (float)Math.Floor(effect.Stats._rawAdditiveData[(int)stat] * adjustment) - currentVal;
                            stats.AddSpecialEffect(newEffect);
                        }
                    }
                }
            }
        }

        // caching policy: cache total stats only for items that don't have global requirements
        // value should not change if it relies on data other than from this item
        // assume there is no stat editing happening in code other than in item editor
        // invalidate on id changes, invalidate when item is opened for editing
        // invalidate all items when any gem is opened for editing
        // invalidate 
        private Stats cachedTotalStats = null;
        private DateTime cachedTime; 
        public void InvalidateCachedData()
        {
            cachedTotalStats = null;
        }

        public Stats GetTotalStats() { return AccumulateTotalStats(null, null); }
        public Stats GetTotalStats(Character character) { return AccumulateTotalStats(character, null); }
#if SILVERLIGHT
        public Stats AccumulateTotalStats(Character character, Stats unsafeStatsAccumulator)
#else
        public unsafe Stats AccumulateTotalStats(Character character, Stats unsafeStatsAccumulator)
#endif
        {
            Item item = Item;
            if ((object)cachedTotalStats != null && item.LastChange <= cachedTime)
            {
                if ((object)unsafeStatsAccumulator != null)
                {
                    unsafeStatsAccumulator.AccumulateUnsafe(cachedTotalStats);
                }
                return cachedTotalStats;
            }
            if (item == null)
            {
                return null;
            }
            Item g1 = Gem1;
            Item g2 = Gem2;
            Item g3 = Gem3;
            Enchant enchant = Enchant;
            Tinkering tinkering = Tinkering;
            bool volatileGem = false, volatileItem = false;
            bool gem1 = false;
            bool gem2 = false;
            bool gem3 = false;
            bool eligibleForSocketBonus = Item.GemMatchesSlot(g1, item.SocketColor1) &&
                                            Item.GemMatchesSlot(g2, item.SocketColor2) &&
                                            Item.GemMatchesSlot(g3, item.SocketColor3);
            if (g1 != null && g1.MeetsRequirements(character, out volatileGem)) gem1 = true;
            volatileItem = volatileItem || volatileGem;
            if (g2 != null && g2.MeetsRequirements(character, out volatileGem)) gem2 = true;
            volatileItem = volatileItem || volatileGem;
            if (g3 != null && g3.MeetsRequirements(character, out volatileGem)) gem3 = true;
            volatileItem = volatileItem || volatileGem;
			// the way AccumulateUpgradeStats works is by direct modifying base (not just delta), so we should always use a temp Stats for this item only
			// this isn't much of a problem since JC gems and meta gems no longer have global restrictions
            /*if (volatileItem && unsafeStatsAccumulator != null)
            {
                unsafeStatsAccumulator.AccumulateUnsafe(item.Stats, true);
                if (RandomSuffixId != 0)
                {
                    RandomSuffix.AccumulateStats(unsafeStatsAccumulator, item, RandomSuffixId);
                }
                if (UpgradeLevel > 0)
                {
                    AccumulateUpgradeStats(unsafeStatsAccumulator, item, RandomSuffixId, UpgradeLevel);
                }
                if (Reforging != null && Reforging.Validate)
                {
                    unsafeStatsAccumulator._rawAdditiveData[(int)Reforging.ReforgeFrom] -= Reforging.ReforgeAmount;
                    unsafeStatsAccumulator._rawAdditiveData[(int)Reforging.ReforgeTo] += Reforging.ReforgeAmount;
                }
                if (gem1) unsafeStatsAccumulator.AccumulateUnsafe(g1.Stats, true);
                if (gem2) unsafeStatsAccumulator.AccumulateUnsafe(g2.Stats, true);
                if (gem3) unsafeStatsAccumulator.AccumulateUnsafe(g3.Stats, true);
                if (eligibleForSocketBonus) unsafeStatsAccumulator.AccumulateUnsafe(item.SocketBonus, true);
                bool eligibleForEnchant = Calculations.IsItemEligibleForEnchant(enchant, item);
                if (eligibleForEnchant) unsafeStatsAccumulator.AccumulateUnsafe(enchant.Stats, true);
                bool eligibleForTinkering = Calculations.IsItemEligibleForTinkering(tinkering, item);
                if (eligibleForTinkering) unsafeStatsAccumulator.AccumulateUnsafe(tinkering.Stats, true);
                return null;
            }
            else*/
            {
                Stats totalItemStats = new Stats();
#if !SILVERLIGHT
                fixed (float* pRawAdditiveData = totalItemStats._rawAdditiveData, pRawMultiplicativeData = totalItemStats._rawMultiplicativeData, pRawNoStackData = totalItemStats._rawNoStackData)
                {
                    totalItemStats.BeginUnsafe(pRawAdditiveData, pRawMultiplicativeData, pRawNoStackData);
#endif
                    totalItemStats.AccumulateUnsafe(item.Stats, true);
                    if (RandomSuffixId != 0)
                    {
                        RandomSuffix.AccumulateStats(totalItemStats, item, RandomSuffixId);
                    }
                    if (UpgradeLevel > 0)
                    {
                        AccumulateUpgradeStats(totalItemStats, item, RandomSuffixId, UpgradeLevel);
                    }
                    if (Reforging != null && Reforging.Validate)
                    {
                        totalItemStats._rawAdditiveData[(int)Reforging.ReforgeFrom] -= Reforging.ReforgeAmount;
                        totalItemStats._rawAdditiveData[(int)Reforging.ReforgeTo] += Reforging.ReforgeAmount;
                    }
                    if (gem1) totalItemStats.AccumulateUnsafe(g1.Stats, true);
                    if (gem2) totalItemStats.AccumulateUnsafe(g2.Stats, true);
                    if (gem3) totalItemStats.AccumulateUnsafe(g3.Stats, true);
                    if (eligibleForSocketBonus) totalItemStats.AccumulateUnsafe(item.SocketBonus, true);
                    bool eligibleForEnchant = Calculations.IsItemEligibleForEnchant(enchant, item);
                    if (eligibleForEnchant) totalItemStats.AccumulateUnsafe(enchant.Stats, true);
                    bool eligibleForTinkering = Calculations.IsItemEligibleForTinkering(tinkering, item);
                    if (eligibleForTinkering) totalItemStats.AccumulateUnsafe(tinkering.Stats, true);
                    if (!volatileItem)
                    {
                        cachedTime = DateTime.Now;
                        cachedTotalStats = totalItemStats;
                        cachedTotalStats.GenerateSparseData();
                        if (unsafeStatsAccumulator != null)
                        {
                            unsafeStatsAccumulator.AccumulateUnsafe(cachedTotalStats);
                        }
                    }
#if !SILVERLIGHT

                    totalItemStats.EndUnsafe();
                }
#endif
                return totalItemStats;
            }
        }

        public static ItemInstance LoadFromId(string gemmedId)
        {
            if (string.IsNullOrEmpty(gemmedId))
                return null;
            return new ItemInstance(gemmedId);
        }

        #region IComparable<Item> Members

        public int CompareTo(ItemInstance other)
        {
            return GemmedId.CompareTo(other.GemmedId);
        }

        #endregion

        public bool FitsInSlot(CharacterSlot characterSlot)
        {
            return Item.FitsInSlot(characterSlot);
        }

        // helper functions to minimize fixing of models
        [XmlIgnore]
        public ItemSlot Slot
        {
            get
            {
                if (Item == null) return ItemSlot.None;
                return Item.Slot;
            }
        }

        [XmlIgnore]
        public ItemType Type
        {
            get
            {
                if (Item == null) return ItemType.None;
                return Item.Type;
            }
        }

        [XmlIgnore]
        public int MinDamage
        {
            get
            {
                if (Item == null) return 0;
                return (int)Math.Floor(Item.MinDamage * Math.Pow(1.15, UpgradeLevel / 15.0));
            }
        }

        [XmlIgnore]
        public int MaxDamage
        {
            get
            {
                if (Item == null) return 0;
                return (int)Math.Floor(Item.MaxDamage * Math.Pow(1.15, UpgradeLevel / 15.0));
            }
        }

        [XmlIgnore]
        public ItemDamageType DamageType
        {
            get
            {
                if (Item == null) return ItemDamageType.Physical;
                return Item.DamageType;
            }
        }

        [XmlIgnore]
        public float Speed
        {
            get
            {
                if (Item == null) return 0;
                return Item.Speed;
            }
        }
        [XmlIgnore]
        public float DPS
        {
            get
            {
                if (Speed == 0f) return 0f;
                else return ((float)(MinDamage + MaxDamage) * 0.5f) / Speed;
            }
        }

        public static bool operator ==(ItemInstance a, ItemInstance b)
        {
            if ((object)b == null || (object)a == null) return (object)a == (object)b;
            return a.GemmedId == b.GemmedId;
        }

        public static bool operator !=(ItemInstance a, ItemInstance b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            ItemInstance o = obj as ItemInstance;
            if ((object)o != null)
            {
                return GemmedId == o.GemmedId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GemmedId.GetHashCode();
        }

        public string Name 
        {
            get
            {
                if (Item == null) return null;
                if (RandomSuffixId != 0)
                {
                    return Item.Name + " " + RandomSuffix.GetSuffix(RandomSuffixId);
                }
                else
                {
                    return Item.Name;
                }
            }
        }
    }
    #endregion

    [GenerateSerializer]
    public class ItemList : List<Item>
    {
        public ItemList() : base() { }
        public ItemList(IEnumerable<Item> collection) : base(collection) { }
    }

    [GenerateSerializer]
    public class ItemSet : List<ItemInstance>
    {
        public ItemSet() : base() { }
        public ItemSet(String name, IEnumerable<ItemInstance> collection) : base(collection) { Name = name; }
        private String _name = "Unnamed Set";
        public String Name { get { return _name; } set { _name = value; } }
        public string ToGemmedIDList() {
            List<string> list = new List<string>();
            foreach (ItemInstance ii in this) {
                if (ii == null) {
                    list.Add("");
                } else {
                    list.Add(ii.GemmedId);
                }
            }
            string retVal = "";
            int i = 0;
            foreach (string s in list) {
                retVal += string.Format("{0}:{1}|", (CharacterSlot)i, s);
                i++;
            }
            retVal = "\"" + Name.Replace("|","-") + "\"|" + retVal.Trim('|');
            return retVal;
        }
        public override string ToString()
        {
            string list = "";
            foreach (ItemInstance i in this) {
                list += string.Format("{0}, ", i);
            }
            list = list.Trim(',').Trim(' ');
            return Name + ": " + ListAsDesc;
        }
        public ItemInstance this[CharacterSlot cs] {
            get {
                if (this.Count <= 0) { return null; }
                if (this.Count < (int)cs + 1) { return null; }
                if ((int)cs < 0) { return null; }
                return this[(int)cs];
            }
            set { this[(int)cs] = value; }
        }
        public String ListAsDesc {
            get {
                string list = "";
                foreach (CharacterSlot cs in Character.EquippableCharacterSlots) {
                    if (this[cs] == null) { list += string.Format("{0}: {1}\r\n", cs.ToString(), "Empty"); }
                    else { list += string.Format("{0}: {1}\r\n", cs.ToString(), this[cs]); }
                }
                list = list.Trim('\r').Trim('\n');
                return list != "" ? list : "Empty List";
            }
        }
        public static ItemSet GenerateItemSetFromSavedString(string source) {
            ItemSet retVal = new ItemSet();
            //
            foreach (CharacterSlot cs in Character.EquippableCharacterSlots) { retVal.Add(null); }
            //
            List<String> sources = source.Split('|').ToList<String>();
            string name = sources[0].Replace("\"",""); // Read the Name
            sources.RemoveAt(0); // Pull the Name out of the list
            retVal.Name = name;
            foreach (String src in sources)
            {
                string[] srcs = src.Split(':');
                if (!String.IsNullOrEmpty(srcs[1])) {
                    retVal[(CharacterSlot)Enum.Parse(typeof(CharacterSlot), srcs[0], true)] = ItemInstance.LoadFromId(srcs[1]);
                } //else { retVal[(CharacterSlot)Enum.Parse(typeof(CharacterSlot), srcs[0], true)] = null; }
            }
            //
            return retVal;
        }
        /*public override bool Equals(object obj)
        {
            if (obj == null) { return false; } // fail on null object
            ItemSet other = (obj as ItemSet);
            if (other.Name != Name) { return false; } // fail on name mismatch
            if (other.Count != this.Count) { return false; } // fail on count mismatch
            foreach (CharacterSlot cs in Character.EquippableCharacterSlots) {
                if (this[cs] == null && other[cs] != null) {
                    return false; // fail on one slot being null and not the other
                } else if (this[cs] != null && other[cs] == null) {
                    return false; // fail on one slot being null and not the other
                } else if (this[cs] != other[cs]) {
                    return false; // fail on not matching in that slot
                }
            }
            return true;
            //return base.Equals(obj);
        }*/
    }

    [GenerateSerializer]
    public class ItemSetList : List<ItemSet>
    {
        public ItemSetList() : base() { }
        public ItemSetList(IEnumerable<ItemSet> collection) : base(collection) { }
    }
}
