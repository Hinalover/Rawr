using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Hunter
{
    public class SpellEffectProcessor
    {
        static float weapon_multiplier = 1.0f;

        static SpellEffectDataCollection data;

        public static SpellEffectDataCollection getAllSpellEffectData()
        {
            if (data == null)
                data = SpellData.getSpellData();

            return data;
        }

        public static IEnumerable<__spelleffect_data> getSpellEffectData(int spellId)
        {
            if (data == null)
                data = SpellData.getSpellData();

            return data.Where(t => t.SpId == spellId).AsEnumerable();
        }

        public static IEnumerable<SpellEffect> getAbilityEffects(int spellId, Character c, float? attackPower, float? spellPower, float? multiplier)
        {
            var spellEffectListing = new List<SpellEffect>();

            var dataToProcess = getSpellEffectData(spellId);

            foreach (var item in dataToProcess)
            {

            }



            return spellEffectListing;
        }

        public static float calculateWeaponDamage(ItemInstance weapon, float attackPower, bool normalizeWeaponSpeed = false)
        {
            if (weapon == null || weapon_multiplier <= 0) return 0;

            float dmg = averageRange(weapon.MaxDamage, weapon.MinDamage); // + weapon bonus damage?

            float weaponSpeed = normalizeWeaponSpeed ? getNormalizedWeaponSpeed(weapon) : weapon.Speed;

            float power_damage = weaponSpeed * attackPower / 14f;

            float total_dmg = dmg + power_damage;

            // OH penalty
            if (weapon.Slot == ItemSlot.OffHand)
                total_dmg *= 0.5f;

            return total_dmg;
        }

        private static float calculateDirectDamage(ItemInstance weapon, float attackPower, float spellPower, float multiplier, bool normalizeWeaponSpeed = false)
        {
            throw new NotImplementedException("calculateDirectDamage is still in development");

            float dmg = calculateWeaponDamage(weapon, attackPower, normalizeWeaponSpeed);

            //commented following line from SimC
            //dmg = Math.Floor(dmg + 0.5);

            //Original line: if (dmg == 0 && weapon_multiplier == 0 && direct_power_mod == 0) return 0;
            if (dmg == 0 && weapon_multiplier == 0) return 0;

            float base_direct_dmg = dmg;
            float weapon_dmg = 0;

            //this is not yet implemented
            //dmg += bonus_damage();

            //if (weapon_multiplier > 0)
            //{
            //    // x% weapon damage + Y
            //    // e.g. Obliterate, Shred, Backstab
            //    dmg += calculate_weapon_damage(ap);
            //    dmg *= weapon_multiplier;
            //    weapon_dmg = dmg;
            //}
            //dmg += direct_power_mod * (ap + sp);
            //dmg *= multiplier;

            //double init_direct_dmg = dmg;

            //if (r == RESULT_GLANCE)
            //{
            //    double delta_skill = (t->level - player->level) * 5.0;

            //    if (delta_skill < 0.0)
            //        delta_skill = 0.0;

            //    double max_glance = 1.3 - 0.03 * delta_skill;

            //    if (max_glance > 0.99)
            //        max_glance = 0.99;
            //    else if (max_glance < 0.2)
            //        max_glance = 0.20;

            //    double min_glance = 1.4 - 0.05 * delta_skill;

            //    if (min_glance > 0.91)
            //        min_glance = 0.91;
            //    else if (min_glance < 0.01)
            //        min_glance = 0.01;

            //    if (min_glance > max_glance)
            //    {
            //        double temp = min_glance;
            //        min_glance = max_glance;
            //        max_glance = temp;
            //    }

            //    dmg *= sim->averaged_range(min_glance, max_glance); // 0.75 against +3 targets.
            //}
            //else if (r == RESULT_CRIT)
            //{
            //    dmg *= 1.0 + total_crit_bonus();
            //}

            //// AoE with decay per target
            //if (chain_target > 0 && base_add_multiplier != 1.0)
            //    dmg *= pow(base_add_multiplier, chain_target);

            //// AoE with static reduced damage per target
            //if (chain_target > 1 && base_aoe_multiplier != 1.0)
            //    dmg *= base_aoe_multiplier;

            //if (!sim->average_range) dmg = floor(dmg + sim->real());


            return dmg;
        }

        private static float averageRange(float val1, float val2)
        {
            return (val1 + val2) / 2f;
        }

        private static float getNormalizedWeaponSpeed(ItemInstance i)
        {
            if (i.Type == ItemType.Dagger)
                return 1.7f;
            else if (i.Type == ItemType.Bow || i.Type == ItemType.Crossbow || i.Type == ItemType.Gun)
                return 2.8f;
            else if (i.Type == ItemType.OneHandAxe || i.Type == ItemType.OneHandMace || i.Type == ItemType.OneHandSword || i.Type == ItemType.FistWeapon || i.Type == ItemType.Wand)
                return 2.4f;
            else if (i.Type == ItemType.Polearm || i.Type == ItemType.Staff || i.Type == ItemType.TwoHandAxe || i.Type == ItemType.TwoHandMace || i.Type == ItemType.TwoHandSword)
                return 3.3f;
            else
                throw new NotImplementedException("Item type is not implemented for Normalized Speed: " + i.Type.ToString());
        }
    }
}
