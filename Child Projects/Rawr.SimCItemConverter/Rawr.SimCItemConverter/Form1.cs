using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Rawr.SimCItemConverter
{
	public class GemPropertyData
	{
		public int ID;
		public int EnchantID;
		public int Color;
		public int MinILevel;
	}

	enum ItemEnchantType
	{
		ITEM_ENCHANTMENT_NONE = 0,
		ITEM_ENCHANTMENT_COMBAT_SPELL = 1,
		ITEM_ENCHANTMENT_DAMAGE = 2,
		ITEM_ENCHANTMENT_EQUIP_SPELL = 3,
		ITEM_ENCHANTMENT_RESISTANCE = 4,
		ITEM_ENCHANTMENT_STAT = 5,
		ITEM_ENCHANTMENT_TOTEM = 6,
		ITEM_ENCHANTMENT_USE_SPELL = 7,
		ITEM_ENCHANTMENT_PRISMATIC_SOCKET = 8
	};

	public class ItemEnchantmentData
	{
		public int ID;
		public int Slot;
		public int GemID;
		public int ScalingID;
		public int MinScalingLevel;
		public int MaxScalingLevel;
		public int RequiredSkill;
		public int RequiredSkillLevel;
		public int[] EnchantType = new int[3]; // item_enchantment
		public int[] EnchantAmount = new int[3];
		public int[] EnchantProp = new int[3]; // item_mod_type
		public float[] EnchantCoefficient = new float[3];
		public int SpellID;
	}

	public class ItemUpgrade
	{
		public int ID;
		public int ItemLevel;
	}

	public partial class Form1 : Form
	{
		private Dictionary<int, GemPropertyData> gemProperties = new Dictionary<int, GemPropertyData>();
		private Dictionary<int, ItemEnchantmentData> itemEnchantments = new Dictionary<int, ItemEnchantmentData>();
		private Dictionary<int, ItemUpgrade> itemUpgrades = new Dictionary<int, ItemUpgrade>();
		private Dictionary<int, List<int>> itemUpgradeRules = new Dictionary<int, List<int>>();
		private Dictionary<int, int[]> randomSuffixList = new Dictionary<int, int[]>();
		private DBCData dbc = new DBCLive();

		const int playerLevel = 100;

		public Form1()
		{
			InitializeComponent();
		}

		private void btnLive_Click(object sender, EventArgs e)
		{
			Uri url = new Uri("https://simulationcraft.googlecode.com/git-history/master/engine/dbc/sc_item_data.inc");
			LoadItems(url);
		}

		private void btnBeta_Click(object sender, EventArgs e)
		{
			Uri url = new Uri("https://simulationcraft.googlecode.com/git-history/wod/engine/dbc/sc_item_data.inc");
			LoadItems(url);
		}

		private void LoadItems(Uri url)
		{
			WebClient client = new WebClient();
			Stream stream = null;
			StreamReader sr = null;
			string temp = "";

			randomSuffixList[390] = new int[]{   5,   6,   7,   8,  14,  36,  37,  39,  40,  41,  42,  43,  45,  91, 114, 118, 120, 121, 122, 123, 124, 125, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139 };
			randomSuffixList[391] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[392] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[393] = new int[]{   5,   6,   7,   8,  14,  36,  37,  39,  40,  41,  42,  43,  45,  91, 114, 118, 120, 121, 122, 123, 124, 125, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139 };
			randomSuffixList[394] = new int[]{   6,   8,  36,  37,  39,  41,  42, 114, 129, 130, 131, 132, 138 };
			randomSuffixList[395] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[396] = new int[]{   5,  40,  41,  91, 133, 134, 135, 136, 137 };
			randomSuffixList[397] = new int[]{   5,  40,  41,  91, 133, 134, 135, 136, 137 };
			randomSuffixList[398] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[399] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[400] = new int[]{   6,   8,  36,  37,  39,  42, 130, 131, 132, 138 };
			randomSuffixList[401] = new int[]{   6,   8,  36,  37,  39,  41,  42, 114, 129, 130, 131, 132, 138 };
			randomSuffixList[402] = new int[]{   5,  40,  91, 131, 132, 133, 134, 135, 136, 137 };
			randomSuffixList[403] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[404] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[405] = new int[]{   5,  40,  91, 131, 132, 133, 134, 135, 136, 137 };
			randomSuffixList[406] = new int[]{   6,   7,   8,  14,  36,  37,  39,  41,  42,  43,  45, 118, 120, 121, 122, 123, 124, 125, 127, 128, 131, 132, 139 };
			randomSuffixList[407] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[408] = new int[]{   6,   7,   8,  14,  36,  37,  39,  41,  42,  43,  45, 118, 120, 121, 122, 123, 124, 125, 127, 128, 131, 132, 139 };
			randomSuffixList[409] = new int[]{   6,   8,  36,  37,  39,  41,  42, 114, 129, 130, 131, 132, 138 };
			randomSuffixList[410] = new int[]{   6,   8,  36,  37,  39,  41,  42, 114, 129, 130, 131, 132, 138 };
			randomSuffixList[411] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[412] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[413] = new int[]{   5,   7,  14,  40,  41,  43,  91, 118, 120, 121, 122, 123, 133, 134, 135, 136, 137, 139 };
			randomSuffixList[414] = new int[]{   6,   8,  36,  37,  39,  41,  42, 114, 129, 130, 131, 132, 138 };
			randomSuffixList[415] = new int[]{ 154, 155, 156, 157, 158, 159 };
			randomSuffixList[417] = new int[]{ 169, 170, 171, 172 };
			randomSuffixList[418] = new int[]{ 203, 204, 205, 206 };
			randomSuffixList[419] = new int[]{ 220, 221, 222, 223 };
			randomSuffixList[420] = new int[]{ 173, 174, 175, 176 };
			randomSuffixList[421] = new int[]{ 118, 120, 121, 122 };
			randomSuffixList[422] = new int[]{ 180, 181, 182 };
			randomSuffixList[423] = new int[]{ 224, 225, 226 };
			randomSuffixList[424] = new int[]{ 207, 208, 209 };
			randomSuffixList[425] = new int[]{ 177, 178, 179 };
			randomSuffixList[426] = new int[]{ 125, 127, 128 };
			randomSuffixList[427] = new int[]{ 183, 184, 185, 186, 187, 188 };
			randomSuffixList[429] = new int[]{ 114, 129, 130, 131, 132, 138 };
			randomSuffixList[431] = new int[]{ 189, 190, 191, 192, 193, 194 };
			randomSuffixList[432] = new int[]{ 191, 192, 193, 194 };
			randomSuffixList[433] = new int[]{ 229, 230, 231, 232 };
			randomSuffixList[434] = new int[]{ 212, 213, 214, 215 };
			randomSuffixList[435] = new int[]{ 185, 186, 187, 188 };
			randomSuffixList[437] = new int[]{ 195, 196, 197, 198 };
			randomSuffixList[438] = new int[]{ 216, 217, 218, 219 };
			randomSuffixList[439] = new int[]{ 233, 234, 235, 236 };
			randomSuffixList[440] = new int[]{ 199, 200, 201, 202 };
			randomSuffixList[441] = new int[]{ 133, 135, 136, 137 };
			randomSuffixList[442] = new int[]{ 114, 118, 120, 121, 122, 123, 124, 125, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138 };
			randomSuffixList[450] = new int[]{   5,  40,  91, 131, 132, 133, 134, 135, 136, 137 };
			randomSuffixList[451] = new int[]{   5,  40,  91, 131, 132, 133, 134, 135, 136, 137 };
			randomSuffixList[654] = new int[] { 344, 345, 347, 348, 358, 359, 360, 361 };


			ItemCacheInstance itemCache = new ItemCacheInstance();

			//stream = client.OpenRead(url);
			stream = new FileStream("D:\\Theorycrafting\\simulationcraft\\engine\\dbc\\sc_item_data.inc", FileMode.Open);
			sr = new StreamReader(stream);
			string all = sr.ReadToEnd();
			sr.Close();

			StringReader reader = new StringReader(all);

			bool found = false;
			while ((temp = reader.ReadLine()) != null)
			{
				if (temp == "static struct gem_property_data_t __gem_property_data[] = {")
				{
					found = true;
					continue;
				}
				if (found)
				{
					if (temp.StartsWith("};"))
					{
						break;
					}

					string str = temp.Replace('{', ' ');
					str = str.Replace('}', ' ');
					string[] tokens = str.Split(',');

					GemPropertyData data = new GemPropertyData();
					data.ID = int.Parse(tokens[0].Trim());
					data.EnchantID = int.Parse(tokens[1].Trim());
					data.Color = int.Parse(tokens[2].Trim());
					data.MinILevel = int.Parse(tokens[3].Trim());

					gemProperties.Add(data.ID, data);
				}
			}

			reader = new StringReader(all);

			found = false;
			while ((temp = reader.ReadLine()) != null)
			{
				if (temp == "static struct item_enchantment_data_t __spell_item_ench_data[] = {")
				{
					found = true;
					continue;
				}
				if (found)
				{
					if (temp.StartsWith("};"))
					{
						break;
					}

					string str = temp.Replace('{', ' ');
					str = str.Replace('}', ' ');
					string[] tokens = str.Split(',');

					ItemEnchantmentData data = new ItemEnchantmentData();
					data.ID = int.Parse(tokens[0].Trim());
					data.Slot = int.Parse(tokens[1].Trim());
					data.GemID = int.Parse(tokens[2].Trim());
					data.ScalingID = int.Parse(tokens[3].Trim());
					data.MinScalingLevel = int.Parse(tokens[4].Trim());
					data.MaxScalingLevel = int.Parse(tokens[5].Trim());
					data.RequiredSkill = int.Parse(tokens[6].Trim());
					data.RequiredSkillLevel = int.Parse(tokens[7].Trim());
					for (int i = 0; i < 3; i++)
					{
						data.EnchantType[i] = int.Parse(tokens[8 + i].Trim());
						data.EnchantAmount[i] = int.Parse(tokens[11 + i].Trim());
						data.EnchantProp[i] = int.Parse(tokens[14 + i].Trim());
						data.EnchantCoefficient[i] = float.Parse(tokens[17 + i].Trim());
					}
					data.SpellID = int.Parse(tokens[20].Trim());

					itemEnchantments.Add(data.ID, data);
				}
			}

			reader = new StringReader(all);

			found = false;
			while ((temp = reader.ReadLine()) != null)
			{
				if (temp == "static item_upgrade_t __item_upgrade_data[] = {")
				{
					found = true;
					continue;
				}
				if (found)
				{
					if (temp.StartsWith("};"))
					{
						break;
					}

					string str = temp.Replace('{', ' ');
					str = str.Replace('}', ' ');
					string[] tokens = str.Split(',');

					ItemUpgrade data = new ItemUpgrade();
					data.ID = int.Parse(tokens[0].Trim());
					data.ItemLevel = int.Parse(tokens[1].Trim());

					itemUpgrades.Add(data.ID, data);
				}
			}

			reader = new StringReader(all);

			found = false;
			while ((temp = reader.ReadLine()) != null)
			{
				if (temp == "static item_upgrade_rule_t __item_upgrade_rule_data[] = {")
				{
					found = true;
					continue;
				}
				if (found)
				{
					if (temp.StartsWith("};"))
					{
						break;
					}

					string str = temp.Replace('{', ' ');
					str = str.Replace('}', ' ');
					string[] tokens = str.Split(',');

					int itemID = int.Parse(tokens[3].Trim());
					int upgradeLevel = int.Parse(tokens[1].Trim());
					int upgradeID = int.Parse(tokens[2].Trim());

					List<int> upgrades;
					if (!itemUpgradeRules.TryGetValue(itemID, out upgrades))
					{
						upgrades = new List<int>();
						itemUpgradeRules.Add(itemID, upgrades);
					}
					/*ItemUpgrade upgrade;
					if (itemUpgrades.TryGetValue(upgradeID, out upgrade))
					{
						upgrades.Add(upgrade.ItemLevel);
					}*/
					// i have no idea what to do with the upgrade table, all used entries have value 0
					// all that seems to matter is upgrade level
					upgrades.Add(upgradeLevel * 4);
				}
			}			

			reader = new StringReader(all);

			while ((temp = reader.ReadLine()) != null)
			{
				if (temp.StartsWith("};"))
				{
					break;
				}
				if (temp.StartsWith("  {"))
				{
					string str = temp.Replace('{', ' ');
					str = str.Replace('}', ' ');
					str = str.Replace("\\\"", "*****");
					string[] strTokens = str.Split('"');
					if (strTokens.Length < 3)
					{
						continue;
					}
					string[] tokens = ("," + strTokens[2]).Split(',');

					Item item = new Item();
					item.Id = int.Parse(strTokens[0].Trim(' ', ','));
					item.Name = strTokens[1].Trim().Replace("*****", "\"");
					uint flags1 = Convert.ToUInt32(tokens[2].Trim(), 16);
					uint flags2 = Convert.ToUInt32(tokens[3].Trim(), 16);
					uint typeFlags = Convert.ToUInt32(tokens[4].Trim(), 16);
					item.ItemLevel = int.Parse(tokens[5].Trim());
					int requiredLevel = int.Parse(tokens[6].Trim());
					int requiredSkill = int.Parse(tokens[7].Trim());
					int requiredSkillLevel = int.Parse(tokens[8].Trim());
					item.Quality = (ItemQuality)int.Parse(tokens[9].Trim());
					item.Slot = GetItemSlot(int.Parse(tokens[10].Trim()));
					item.Type = GetItemType(int.Parse(tokens[11].Trim()) + "." + int.Parse(tokens[12].Trim()));
					item.Bind = (BindsOn)int.Parse(tokens[13].Trim());
					float delay = float.Parse(tokens[14].Trim());
					float damageRange = float.Parse(tokens[15].Trim());
					float itemModifier = float.Parse(tokens[16].Trim());
					uint classMask = Convert.ToUInt32(tokens[17].Trim(), 16);
					uint raceMask = Convert.ToUInt32(tokens[18].Trim(), 16);
					for (int i = 0; i < 10; i++)
					{
						int statType = int.Parse(tokens[19 + i].Trim());
						int statValue = int.Parse(tokens[29 + i].Trim());
						int statAlloc = int.Parse(tokens[39 + i].Trim());
						float socketMultiplier = float.Parse(tokens[49 + i].Trim());
						if (statType != -1 && statValue > 0)
						{
							AdditiveStat stat = GetItemStat(statType);
							if ((int)stat >= 0)
							{								
								item.Stats.rawAdditiveData[(int)stat] = statValue;
								if (item.ItemStatAllocations == null)
								{
									item.ItemStatAllocations = new List<ItemStatAllocation>();
								}
								ItemStatAllocation allocation = new ItemStatAllocation() { Stat = stat, Allocation = statAlloc, SocketMultiplier = socketMultiplier };
								item.ItemStatAllocations.Add(allocation);
								// it looks like statValue in dbc can have bogus value, always calculate from allocations
								if (statAlloc > 0)
								{
									int itemBudget = ItemInstance.GetItemBudget(item, 0);
									item.Stats.rawAdditiveData[(int)stat] = ItemInstance.GetItemStatFromAllocation(allocation, itemBudget, item.ItemLevel);
								}
							}
						}
					}
					for (int i = 0; i < 5; i++)
					{
						int itemSpellTriggerType = int.Parse(tokens[59 + i].Trim());
						int spellID = int.Parse(tokens[64 + i].Trim());
						int cooldownCategory = int.Parse(tokens[69 + i].Trim());
						int cooldownCategoryDuration = int.Parse(tokens[74 + i].Trim());
						int cooldownGroup = int.Parse(tokens[79 + i].Trim());
						int cooldownGroupDuration = int.Parse(tokens[84 + i].Trim());
						if(spellID > 0)
						{
							switch(itemSpellTriggerType)
							{
								case 0: // ITEM_SPELLTRIGGER_ON_USE
									break;
								case 1: // ITEM_SPELLTRIGGER_ON_EQUIP
									SpellCompilation spell = dbc.getSpellInfo((uint)spellID);
									ApplySpellEffects(item.Stats, spell, item);
									break;
								case 2: // ITEM_SPELLTRIGGER_CHANCE_ON_HIT
									break;
								case 4: // ITEM_SPELLTRIGGER_SOULSTONE
									break;
								case 5: // ITEM_SPELLTRIGGER_ON_NO_DELAY_USE
									break;
								case 6: // ITEM_SPELLTRIGGER_LEARN_SPELL_ID
									break;
							}
						}
					}
					item.SocketColor1 = GetSocketColor(int.Parse(tokens[89].Trim()));
					item.SocketColor2 = GetSocketColor(int.Parse(tokens[90].Trim()));
					item.SocketColor3 = GetSocketColor(int.Parse(tokens[91].Trim()));
					int gemPropertiesID = int.Parse(tokens[92].Trim());
					if (gemPropertiesID > 0)
					{
						GemPropertyData gemData;
						if (gemProperties.TryGetValue(gemPropertiesID, out gemData))
						{
							ApplyGemProperties(gemData, item);
						}
					}
					int socketBonusID = int.Parse(tokens[93].Trim());
					if (socketBonusID > 0)
					{
						ItemEnchantmentData enchant;
						if (itemEnchantments.TryGetValue(socketBonusID, out enchant))
						{
							item.SocketBonus = new Stats();
							ApplyItemEnchantment(item.SocketBonus, enchant, item);
						}
					}
					int setID = int.Parse(tokens[94].Trim());
					int suffixGroupID = int.Parse(tokens[95].Trim());
					if (suffixGroupID > 0)
					{
						int[] randomSuffixes;
						if (randomSuffixList.TryGetValue(suffixGroupID, out randomSuffixes))
						{
							item.AllowedRandomSuffixes = new List<int>(randomSuffixes);
						}
					}

					List<int> upgrades;
					if (itemUpgradeRules.TryGetValue(item.Id, out upgrades))
					{
						upgrades.Sort();
						item.UpgradeLevels = upgrades;
					}

					itemCache.AddItem(item);
				}
			}

			Stream fileStream = new FileStream(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ItemCache.xml"), FileMode.Create, FileAccess.ReadWrite);
			StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);
			itemCache.Save(writer);
		}

		private static ItemSlot GetItemSlot(int slotId)
		{
			switch (slotId)
			{
				case 1: return ItemSlot.Head;
				case 2: return ItemSlot.Neck;
				case 3: return ItemSlot.Shoulders;
				case 16: return ItemSlot.Back;
				case 5:
				case 20: return ItemSlot.Chest;
				case 4: return ItemSlot.Shirt;
				case 19: return ItemSlot.Tabard;
				case 9: return ItemSlot.Wrist;
				case 10: return ItemSlot.Hands;
				case 6: return ItemSlot.Waist;
				case 7: return ItemSlot.Legs;
				case 8: return ItemSlot.Feet;
				case 11: return ItemSlot.Finger;
				case 12: return ItemSlot.Trinket;
				case 13: return ItemSlot.OneHand;
				case 17: return ItemSlot.TwoHand;
				case 21: return ItemSlot.MainHand;
				case 14:
				case 22:
				case 23: return ItemSlot.OffHand;
				case 15:
				case 25:
				case 26:
				case 28: return ItemSlot.Ranged;
				case 24: return ItemSlot.Projectile;
				case 18:
				case 27: return ItemSlot.ProjectileBag;
				default: return ItemSlot.None;
			}
		}

		private static ItemType GetItemType(string classSubclass)
		{
			switch (classSubclass)
			{
				case "4.1": return ItemType.Cloth;
				case "4.2": return ItemType.Leather;
				case "4.3": return ItemType.Mail;
				case "4.4": return ItemType.Plate;
				case "2.15": return ItemType.Dagger;
				case "2.13": return ItemType.FistWeapon;
				case "2.1": return ItemType.TwoHandAxe;
				case "2.0": return ItemType.OneHandAxe;
				case "2.5": return ItemType.TwoHandMace;
				case "2.4": return ItemType.OneHandMace;
				case "2.8": return ItemType.TwoHandSword;
				case "2.7": return ItemType.OneHandSword;
				case "2.6": return ItemType.Polearm;
				case "2.10": return ItemType.Staff;
				case "4.6": return ItemType.Shield;
				case "2.2": return ItemType.Bow;
				case "2.18": return ItemType.Crossbow;
				case "2.3": return ItemType.Gun;
				case "2.19": return ItemType.Wand;
				case "2.16": return ItemType.Thrown;
				case "6.2": return ItemType.Arrow;
				case "6.3": return ItemType.Bullet;
				case "11.2": return ItemType.Quiver;
				case "11.3": return ItemType.AmmoPouch;
				case "4.8": //return ItemType.Idol;
				case "4.7": //return ItemType.Libram;
				case "4.9": //return ItemType.Totem;
				case "4.10": //return ItemType.Sigil;
				case "4.11": return ItemType.Relic;
				default: return ItemType.None;
			}
		}

		private static AdditiveStat GetItemStat(int itemStat)
		{
			switch (itemStat)
			{
				case 0: return AdditiveStat.Mana;
				case 1: return AdditiveStat.Health;
				case 3: return AdditiveStat.Agility;
				case 4: return AdditiveStat.Strength;
				case 5: return AdditiveStat.Intellect;
				case 6: return AdditiveStat.Spirit;
				case 7: return AdditiveStat.Stamina;
				// case 12: return AdditiveStat.DefenseSkillRating;
				case 13: return AdditiveStat.DodgeRating;
				case 14: return AdditiveStat.ParryRating;
				case 15: return AdditiveStat.BlockRating;
				case 16: return AdditiveStat.HitRating;
				case 17: return AdditiveStat.RangedHitRating;
				case 18: return AdditiveStat.HitRating;
				case 19: return AdditiveStat.CritRating;
				case 20: return AdditiveStat.RangedCritRating;
				case 21: return AdditiveStat.CritRating;
				// case 22: return AdditiveStat.HitTakenMeleeRating;
				// case 23: return AdditiveStat.HitTakenRangedRating;
				// case 24: return AdditiveStat.HitTakenSpellRating;
				// case 25: return AdditiveStat.CritTakenMeleeRating;
				// case 26: return AdditiveStat.CritTakenRangedRating;
				// case 27: return AdditiveStat.CritTakenSpellRating;
				case 28: return AdditiveStat.HasteRating;
				case 29: return AdditiveStat.RangedHasteRating;
				case 30: return AdditiveStat.HasteRating;
				case 31: return AdditiveStat.HitRating;
				case 32: return AdditiveStat.CritRating;
				// case 33: return AdditiveStat.HitTakenRating;
				// case 34: return AdditiveStat.CritTakenRating;
				case 35: return AdditiveStat.Resilience;
				case 36: return AdditiveStat.HasteRating;
				case 37: return AdditiveStat.ExpertiseRating;
				case 38: return AdditiveStat.AttackPower;
				case 39: return AdditiveStat.RangedAttackPower;
				case 40: return AdditiveStat.AttackPower;
				// case 41: return AdditiveStat.SpellHealingDone;
				// case 42: return AdditiveStat.SpellDamageDone;
				case 43: return AdditiveStat.SpellCombatManaRegeneration;
				// case 44: return AdditiveStat.ArmorPenetrationRating;
				case 45: return AdditiveStat.SpellPower;
				case 46: return AdditiveStat.HealthRestore;
				case 47: return AdditiveStat.SpellPenetration;
				// case 48: return AdditiveStat.BlockValue;
				case 49: return AdditiveStat.MasteryRating;
				case 50: return AdditiveStat.BonusArmor;
				case 51: return AdditiveStat.FireResistance;
				case 52: return AdditiveStat.FrostResistance;
				// case 53: return AdditiveStat.HolyResistance;
				case 54: return AdditiveStat.ShadowResistance;
				case 55: return AdditiveStat.NatureResistance;
				case 56: return AdditiveStat.ArcaneResistance;
				case 57: return AdditiveStat.PvPPower;
				default: return (AdditiveStat)(-1);
			}
		}

		private static ItemSlot GetSocketColor(int socketColor)
		{
			switch (socketColor)
			{
				case 0: return ItemSlot.None;
				case 1: return ItemSlot.Meta;
				case 2: return ItemSlot.Red;
				case 4: return ItemSlot.Yellow;
				case 6: return ItemSlot.Orange;
				case 8: return ItemSlot.Blue;
				case 10: return ItemSlot.Purple;
				case 12: return ItemSlot.Green;
				case 14: return ItemSlot.Prismatic;
				case 16: return ItemSlot.Hydraulic; // Sha-Touched
				case 32: return ItemSlot.Cogwheel;
				default: return ItemSlot.None;
			}
		}

		private float GetSpellScaling(int scalingID, int level)
		{
			switch (scalingID)
			{
				case 11: return BaseCombatRating.WarriorSpellScaling(level);
				case 6: return BaseCombatRating.PaladinSpellScaling(level);
				case 3: return BaseCombatRating.HunterSpellScaling(level);
				case 8: return BaseCombatRating.RogueSpellScaling(level);
				case 7: return BaseCombatRating.PriestSpellScaling(level);
				case 1: return BaseCombatRating.DeathKnightSpellScaling(level);
				case 9: return BaseCombatRating.ShamanSpellScaling(level);
				case 4: return BaseCombatRating.MageSpellScaling(level);
				case 10: return BaseCombatRating.WarlockSpellScaling(level);
				case 5: return BaseCombatRating.MonkSpellScaling(level);
				case 2: return BaseCombatRating.DruidSpellScaling(level);
				case -1: return BaseCombatRating.ConstantSpellScaling(level);
				case -2: return BaseCombatRating.ConstantSpellScaling2(level);
				case -3: return BaseCombatRating.ConstantSpellScaling3(level);
				case -4: return BaseCombatRating.ConstantSpellScaling4(level);
				case -5: return BaseCombatRating.ConstantSpellScaling5(level);
				default: return 0;
			}
		}

		void ApplyGemProperties(GemPropertyData data, Item item)
		{
			item.Slot = GetSocketColor(data.Color);
			ItemEnchantmentData enchant;
			if (itemEnchantments.TryGetValue(data.EnchantID, out enchant))
			{
				ApplyItemEnchantment(item.Stats, enchant, item);
			}
		}

		void ApplyItemEnchantment(Stats stats, ItemEnchantmentData enchant, Item item)
		{
			for (int i = 0; i < 3; i++)
			{
				switch ((ItemEnchantType)enchant.EnchantType[i])
				{
					case ItemEnchantType.ITEM_ENCHANTMENT_NONE:
						break;
					case ItemEnchantType.ITEM_ENCHANTMENT_DAMAGE:
						stats.WeaponDamage += enchant.EnchantAmount[i];
						break;
					case ItemEnchantType.ITEM_ENCHANTMENT_EQUIP_SPELL:
						SpellCompilation spell = dbc.getSpellInfo((uint)enchant.EnchantProp[i]);
						ApplySpellEffects(stats, spell, item);
						break;
					case ItemEnchantType.ITEM_ENCHANTMENT_STAT:
						float value = 0;
						if (enchant.ScalingID == 0)
						{
							value = enchant.EnchantAmount[i];
						}
						else
						{
							int level = playerLevel;
							if (playerLevel > enchant.MaxScalingLevel)
							{
								level = enchant.MaxScalingLevel;
							}
							if (level > 0)
							{
								float budget = GetSpellScaling(enchant.ScalingID, level);
								value = (float)Math.Round(budget * enchant.EnchantCoefficient[i]);
							}
						}

						switch (enchant.EnchantProp[i])
						{
							case 3:
								stats.Agility += value;
								break;
							case 4:
								stats.Strength += value;
								break;
							case 5:
								stats.Intellect += value;
								break;
							case 6:
								stats.Spirit += value;
								break;
							case 7:
								stats.Stamina += value;
								break;
							case 13:
								stats.DodgeRating += value;
								break;
							case 14:
								stats.ParryRating += value;
								break;
							case 32:
								stats.CritRating += value;
								break;
							case 35:
								stats.PvPResilience += value;
								break;
							case 1:
							case 36:
								stats.HasteRating += value;
								break;
							case 40:
								stats.VersatilityRating += value;
								break;
							case 49:
								stats.MasteryRating += value;
								break;
							case 57:
								stats.PvPPower += value;
								break;
							case 59:
								stats.MultistrikeRating += value;
								break;
							default:
								break;
						}
						break;
					case ItemEnchantType.ITEM_ENCHANTMENT_RESISTANCE:
						stats.BonusArmor += enchant.EnchantAmount[i];
						break;
					default:
						break;

				}
			}
		}

		void ApplySpellEffects(Stats stats, SpellCompilation spell, Item item)
		{
			if (spell.Effect == null)
				return;
			foreach (DBCSpellEffect effect in spell.Effect)
			{
				switch (effect.type)
				{
					case EffectType.E_APPLY_AURA:
						switch (effect.subtype)
						{
							case EffectSubtype.A_MOD_RESISTANCE:
								stats.ArcaneResistance += effect.base_value;
								stats.FireResistance += effect.base_value;
								stats.FrostResistance += effect.base_value;
								stats.NatureResistance += effect.base_value;
								stats.ShadowResistance += effect.base_value;
								break;
							case EffectSubtype.A_REFLECT_SPELLS_SCHOOL:
								stats.SpellReflectChance += 0.01f * effect.base_value;
								break;
							case EffectSubtype.A_PROC_TRIGGER_SPELL:
								{
									SpecialEffect specialEffect = new SpecialEffect();
									specialEffect.Chance = spell.Data.proc_chance;
									specialEffect.Cooldown = spell.Data.internal_cooldown / 1000.0f;
									if (effect.trigger_spell_id == 0)
										continue;
									SpellCompilation triggerSpell = dbc.getSpellInfo(effect.trigger_spell_id);
									specialEffect.Duration = triggerSpell.Data.duration / 1000.0f;
									if (triggerSpell.Data.max_stack > 0)
									{
										specialEffect.MaxStack = (int)triggerSpell.Data.max_stack;
									}
									switch (spell.Data.proc_flags)
									{
										case 0x00010000: // Your harmful spells
										case 0x00014000:
											specialEffect.Trigger = Trigger.SpellCast;
											break;
										case 0x00011154:
											specialEffect.Trigger = Trigger.SpellCrit;
											break;
										case 0x000002A8:
											specialEffect.Trigger = Trigger.DamageTaken;
											break;
										case 0x00251000: // Your damage dealing spells have a chance
										case 0x00250000:
											specialEffect.Trigger = Trigger.DamageSpellHitorDoTTick;
											break;
										case 0x00000154: // Chance on melee or ranged attacks in Talador
											break;
										case 0x00000014: // damaging an enemy in melee
											specialEffect.Trigger = Trigger.MeleeAttack;
											break;
										case 0x00010114: // Your attacks have a chance
										case 0x00011014: // Each time your attacks hit
											specialEffect.Trigger = Trigger.MeleeHit;
											break;
										case 0x00004400: // Your helpful spells have a chance
										case 0x00244000:
										case 0x00004000: // Your healing spells
										case 0x00255554: // When you heal
											specialEffect.Trigger = Trigger.HealingSpellCast;
											break;
										case 0x00000028: // Melee attacks which reduce you below 35% health
											specialEffect.Trigger = Trigger.DamageTakenPutsMeBelow35PercHealth;
											break;
										case 0x00240000: // you deal periodic spell damage
											specialEffect.Trigger = Trigger.DoTTick;
											break;
										case 0x00251154: // When you deal damage
										case 0x00011110: // Your harmful abilities have a chance
										case 0x00000044: // Thundering Skyfire Diamond
										case 0x00001154: // Your damaging attacks have a chance
											specialEffect.Trigger = Trigger.DamageDone;
											break;
										case 0x00255154: // When you deal damage or heal a target
										case 0x00254000: // Your healing and damaging spells have a chance
											specialEffect.Trigger = Trigger.DamageOrHealingDone;
											break;
										case 0x00000140: // Your ranged attacks have a chance
											specialEffect.Trigger = Trigger.RangedHit;
											break;
										case 0x00015554: // Your melee and ranged attacks have a chance
											break;
										case 0x00000004: // special?
											break;
										case 0x00011010: // Your Chains of Ice ability?
											break;
										case 0x00000002: // whenever you kill a target that yields experience or honor?
											break;
										default:
											break;
									}
									specialEffect.Stats = new Stats();
									ApplySpellEffects(specialEffect.Stats, triggerSpell, item);
									stats.AddSpecialEffect(specialEffect);
								}
								break;
							case EffectSubtype.A_MOD_RATING:
								switch (effect.misc_value1)
								{
									case 0x000E0000:
									case 0x00060000:
										stats.HasteRating += effect.Average(item);
										break;
									case 0x00000800:
										stats.MultistrikeRating += effect.Average(item);
										break;
									case 0x02000000:
										stats.MasteryRating += effect.Average(item);
										break;
									case 0x00000700:
										stats.CritRating += effect.Average(item);
										break;
									case 0x00000004:
										stats.DodgeRating += effect.Average(item);
										break;
									case 0x00000008:
										stats.ParryRating += effect.Average(item);
										break;
									case 0x04000000:
										stats.PvPPower += effect.Average(item);
										break;
									case 0x70000000:
										stats.VersatilityRating += effect.Average(item);
										break;
									default:
										break;
								}
								break;
							case EffectSubtype.A_MOD_SPEED_ALWAYS:
								stats.MovementSpeed += 0.01f * effect.base_value;
								break;
							case EffectSubtype.A_MECHANIC_DURATION_MOD:
								switch (effect.misc_value1)
								{
									case 5:
										stats.FearDurReduc -= 0.01f * effect.base_value;
										break;
									case 11:
										// snare and root will each have one, don't double count
										break;
									case 7:
										stats.SnareRootDurReduc -= 0.01f * effect.base_value;
										break;
									case 9:
										stats.SilenceDurReduc -= 0.01f * effect.base_value;
										break;
									case 12:
										stats.StunDurReduc -= 0.01f * effect.base_value;
										break;
									default:
										break;
								}
								break;
							case EffectSubtype.A_MOD_THREAT:
								if (effect.base_value >= 0)
								{
									stats.ThreatIncreaseMultiplier += 0.01f * effect.base_value;
								}
								else
								{
									stats.ThreatReductionMultiplier -= 0.01f * effect.base_value;
								}
								break;
							case EffectSubtype.A_MOD_STUN:
								// chance to stun
								break;
							case EffectSubtype.A_MOD_CRIT_DAMAGE_BONUS:
								stats.CritBonusDamage += 0.01f * effect.base_value;
								break;
							case EffectSubtype.A_MOD_CRITICAL_HEALING_AMOUNT:
								// already counted in CritBonusDamage?
								break;
							case EffectSubtype.A_MOD_MECHANIC_RESISTANCE:
								// stun resistance?
								break;
							case EffectSubtype.A_MOD_STAT:
								switch (effect.misc_value1)
								{
									case -1:
										stats.Strength += effect.Average(item);
										stats.Agility += effect.Average(item);
										stats.Intellect += effect.Average(item);
										stats.Spirit += effect.Average(item);
										break;
									case 0:
										stats.Strength += effect.Average(item);
										break;
									case 1:
										stats.Agility += effect.Average(item);
										break;
									case 3:
										stats.Intellect += effect.Average(item);
										break;
									case 4:
										stats.Spirit += effect.Average(item);
										break;
									default:
										break;
								}
								break;
							case EffectSubtype.A_MOD_SHIELD_BLOCKVALUE_PCT:
								stats.BonusBlockValueMultiplier += 0.01f * effect.base_value;
								break;
							case EffectSubtype.A_MOD_INCREASE_ENERGY_PERCENT:
								switch (effect.resource_gain_type)
								{
									case ResourceType.Mana:
										stats.BonusManaMultiplier += 0.01f * effect.base_value;
										break;
									default:
										break;
								}
								break;
							case EffectSubtype.A_MOD_DAMAGE_PERCENT_TAKEN:
								switch (effect.misc_value1)
								{
									case 126:
										stats.SpellDamageTakenReductionMultiplier -= 0.01f * effect.base_value;
										break;
									case 127:
										stats.DamageTakenReductionMultiplier -= 0.01f * effect.base_value;
										break;
									default:
										break;
								}
								break;
							case EffectSubtype.A_MOD_BASE_RESISTANCE_PCT:
								// misc value?
								stats.BaseArmorMultiplier += 0.01f * effect.base_value;
								break;
							case EffectSubtype.A_DUMMY:
								// server script
								break;
							case EffectSubtype.A_MOD_CASTING_SPEED_NOT_STACK:
								stats.SpellHaste += 0.01f * effect.base_value;
								break;
							case EffectSubtype.A_MOD_POWER_COST_SCHOOL:
								stats.SpellsManaCostReduction = -effect.base_value;
								break;
							case EffectSubtype.A_MOD_DAMAGE_DONE:
								stats.SpellPower += effect.average;
								break;
							case EffectSubtype.A_MOD_HEALING_DONE:
								// comes together with A_MOD_DAMAGE_DONE, don't duplicate?
								break;
							case EffectSubtype.A_SCHOOL_ABSORB:
								stats.DamageAbsorbed += effect.base_value;
								break;
							case EffectSubtype.A_360:
								// Dragonwrath, Tarecgosa's Rest
								break;
							case EffectSubtype.A_ADD_PCT_MODIFIER:
								break;
							case EffectSubtype.A_ADD_FLAT_MODIFIER:
								break;
							case EffectSubtype.A_MOD_SCALE:
							case EffectSubtype.A_MOD_SCALE_2:
								break;
							case EffectSubtype.A_PERIODIC_TRIGGER_SPELL:
								{
									SpecialEffect specialEffect = new SpecialEffect();
									specialEffect.Chance = 1;
									specialEffect.Cooldown = 0;									
									SpellCompilation triggerSpell = dbc.getSpellInfo(effect.trigger_spell_id);
									if (triggerSpell.Data == null)
										continue;
									specialEffect.Duration = -triggerSpell.Data.duration;
									if (triggerSpell.Data.max_stack > 0)
									{
										specialEffect.MaxStack = (int)triggerSpell.Data.max_stack;
									}
									specialEffect.Trigger = Trigger.Use;
									specialEffect.Stats = new Stats();
									ApplySpellEffects(specialEffect.Stats, triggerSpell, item);
									stats.AddSpecialEffect(specialEffect);
								}
								break;
							case EffectSubtype.A_PERIODIC_DAMAGE:
								switch (spell.Data.spell_damage_type)
								{
									case ItemDamageType.Nature:
										stats.NatureDamage += effect.base_value;
										break;
									case ItemDamageType.Fire:
										stats.FireDamage += effect.base_value;
										break;
									case ItemDamageType.Arcane:
										stats.ArcaneDamage += effect.base_value;
										break;
									case ItemDamageType.Frost:
										stats.FrostDamage += effect.base_value;
										break;
									case ItemDamageType.Holy:
										stats.HolyDamage += effect.base_value;
										break;
									case ItemDamageType.Physical:
										stats.PhysicalDamage += effect.base_value;
										break;
									default:
										break;
								}
								break;
							case EffectSubtype.A_MOD_ATTACK_POWER:
								stats.AttackPower += effect.average;
								break;
							case EffectSubtype.A_MOD_RANGED_ATTACK_POWER:
								stats.RangedAttackPower += effect.average;
								break;
							case EffectSubtype.A_PERIODIC_DUMMY:
								break;
							case EffectSubtype.A_PERIODIC_ENERGIZE:
								switch (effect.resource_gain_type)
								{
									case ResourceType.Mana:
										stats.Mana += effect.average;
										break;
									default:
										break;
								}
								break;
							case EffectSubtype.A_MOD_ALL_CRIT_CHANCE:
								stats.SpellCrit += 0.01f * effect.base_value;;
								stats.PhysicalCrit += 0.01f * effect.base_value;
								break;
							case EffectSubtype.A_MOD_HEALING_DONE_PERCENT:
								stats.BonusHealingDoneMultiplier += 0.01f * effect.base_value;
								break;
							case EffectSubtype.A_421:
								// Jina-Kang, Kindness of Chi-Ji
								break;
							case EffectSubtype.A_405:
								// Purified Bindings of Immerseus
								break;
							case EffectSubtype.A_MOD_TOTAL_STAT_PERCENTAGE:
								break;
							case EffectSubtype.A_233:
								// Zorkra's Hood
								break;
							default:
								break;
						}
						break;
					case EffectType.E_HEAL:
						stats.HealthRestore += effect.average;
						break;
					case EffectType.E_ENERGIZE:
						switch (effect.resource_gain_type)
						{
							case ResourceType.Mana:
								stats.Mana += effect.base_value;
								break;
							case ResourceType.Health:
								stats.Health += effect.base_value;
								break;
							case ResourceType.RunicPower:
								break;
							default:
								break;
						}
						break;
					case EffectType.E_TRIGGER_SPELL:
						break;
					case EffectType.E_SCHOOL_DAMAGE:
						break;
					case EffectType.E_DUMMY:
						break;
					case EffectType.E_FORCE_CAST:
						break;
					case EffectType.E_NONE:
						break;
					case EffectType.E_SUMMON:
						break;
					case EffectType.E_HEAL_PCT:
						stats.HealthRestoreFromMaxHealth += 0.01f * effect.base_value;
						break;
					case EffectType.E_HEALTH_LEECH:
						stats.MaxHealthDamageProc += 0.001f * effect.base_value;
						break;
					case EffectType.E_174:
						// Manipulator's Talisman
						break;
					default:
						break;
				}
			}
		}
	}
}
