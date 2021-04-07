using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Load_SimC_DBC
{
    class Spell
    {
        private string name;
        private uint id;
        private uint flags;
        private float speed;
        private ItemDamageType school; // School, requires custom thing
        private CharacterClass Class; // Class (spell_class_expr_t)
        private CharacterRace race; // Race (spell_race_expr_t)
        private int scaling;
        private uint max_scaling_level;
        private float extra_coeff;
        private uint level;
        private uint max_level;
        private float min_range;
        private float max_range;
        private uint cooldown; // in Milliseconds
        private uint gcd; // in Milliseconds
        private uint category;
        private float duration; // in Milliseconds
        private DKRune rune_cost; // Runes (spell_rune_expr_t)
        private uint power_gain;
        private uint max_stack;
        private uint proc_chance;
        private uint initial_stack;
        private uint proc_flags; // Proc flags, no support for now
        private uint icd; // in Milliseconds
        private float rppm;
        private uint equip_class;
        private uint equip_imask;
        private uint equip_scmask;
        private int cast_min;
        private int cast_max;
        private int cast_div;
        private float m_scaling;
        private uint scaling_level;
        private uint replace_spellid;
        private uint Attr1;
        private uint Attr2;
        private uint Attr3;
        private uint Attr4;
        private uint Attr5;
        private uint Attr6;
        private uint Attr7;
        private uint Attr8;
        private uint Attr9;
        private uint Attr10;
        private uint Attr11;
        private uint Attr12;
		private uint Flags1;
		private uint Flags2;
		private uint Flags3;
		private uint Flags4;
        private string desc;
        private string tooltip;
        private string desc_vars;
        private string icon;
        private string active_icon;
        private string rank;
        private string effect_2;
        private string effect_3;

        public Spell()
        {
            name = "";
            id = 0;
            flags = 0x00;
            speed = 0;
            school = ItemDamageType.Physical;
            Class = CharacterClass.None;
            race = CharacterRace.None;
            scaling = 0;
            max_scaling_level = 0;
            extra_coeff = 0;
            level = 0;
            max_level = 0;
            min_range = 0;
            max_range = 0;
            cooldown = 0;
            gcd = 0;
            category = 0;
            duration = 0;
            rune_cost = DKRune.None;
            power_gain = 0;
            max_stack = 0;
            proc_chance = 0;
            initial_stack = 0;
            proc_flags = 0;
            icd = 0;
            rppm = 0;
            equip_class = 0;
            equip_imask = 0x00;
            equip_scmask = 0x00;
            cast_min = 0;
            cast_max = 0;
            cast_div = 0;
            m_scaling = 0;
            scaling_level = 0;
            replace_spellid = 0;
            Attr1 = 0x00;
            Attr2 = 0x00;
            Attr3 = 0x00;
            Attr4 = 0x00;
            Attr5 = 0x00;
            Attr6 = 0x00;
            Attr7 = 0x00;
            Attr8 = 0x00;
            Attr9 = 0x00;
            Attr10 = 0x00;
            Attr11 = 0x00;
            Attr12 = 0x00;
			Flags1 = 0x00;
			Flags2 = 0x00;
			Flags3 = 0x00;
			Flags4 = 0x00;
            desc = "";
            tooltip = "";
            desc_vars = "";
            icon = "";
            active_icon = "";
            rank = "";
            effect_2 = "";
            effect_3 = "";
        }

        public Spell(string a, uint b, uint c, float d, ItemDamageType e, CharacterClass f, CharacterRace g, int h,
            uint i, float j, uint k, uint l, float m, float n, uint o, uint p, uint q, float r, DKRune s, uint t,
            uint u, uint v, uint w, uint x, uint y, float z, uint aa, uint ab, uint ac, int ad, int ae, int af,
            float ag, uint ah, uint ai, uint aj, uint ak, uint al, uint am, uint an, uint ao, uint ap, uint aq, uint ar, 
            uint at, uint au, uint av, string aw, string ax, string ay, string az, string ba, string bb, string bc, string bd )
        {
            name = a;
            id = b;
            flags = c;
            speed = d;
            school = e;
            Class = f;
            race = g;
            scaling = h;
            max_scaling_level = i;
            extra_coeff = j;
            level = k;
            max_level = l;
            min_range = m;
            max_range = n;
            cooldown = o;
            gcd = p;
            category = q;
            duration = r;
            rune_cost = s;
            power_gain = t;
            max_stack = u;
            proc_chance = v;
            initial_stack = w;
            proc_flags = x;
            icd = y;
            rppm = z;
            equip_class = aa;
            equip_imask = ab;
            equip_scmask = ac;
            cast_min = ad;
            cast_max = ae;
            cast_div = af;
            m_scaling = ag;
            scaling_level = ah;
            replace_spellid = ai;
            Attr1 = aj;
            Attr2 = ak;
            Attr3 = al;
            Attr4 = am;
            Attr5 = an;
            Attr6 = ao;
            Attr7 = ap;
            Attr8 = aq;
            Attr9 = ar;
            Attr10 = at;
            Attr11 = au;
            Attr12 = av;
            desc = aw;
            tooltip = ax;
            desc_vars = ay;
            icon = az;
            active_icon = ba;
            rank = bb;
            effect_2 = bc;
            effect_3 = bd;
        }

        public Spell(List<string> list)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            name = list[0];
            id = Convert.ToUInt32(list[1]);
            flags = Convert.ToUInt32(list[2], 16);
            speed = float.Parse(list[3], culture);
            school = (ItemDamageType)Convert.ToUInt32(list[4], 16);
            Class = (CharacterClass)Convert.ToUInt32(list[5], 16);
            race = (CharacterRace)Convert.ToUInt32(list[6], 16);
            scaling = Convert.ToInt32(list[7]);
            max_scaling_level = Convert.ToUInt32(list[8]);
            level = Convert.ToUInt32(list[9]);
            max_level = Convert.ToUInt32(list[10]);
            min_range = float.Parse(list[11], culture);
            max_range = float.Parse(list[12], culture);
            cooldown = Convert.ToUInt32(list[13]);
            gcd = Convert.ToUInt32(list[14]);
            category = Convert.ToUInt32(list[15]);
            duration = float.Parse(list[16], culture);
            rune_cost = (DKRune) Convert.ToUInt32(list[17], 16);
            power_gain = Convert.ToUInt32(list[18]);
            max_stack = Convert.ToUInt32(list[19]);
            proc_chance = Convert.ToUInt32(list[20]);
            initial_stack = Convert.ToUInt32(list[21]);
            proc_flags = Convert.ToUInt32(list[22], 16);
            icd = Convert.ToUInt32(list[23]);
            rppm = float.Parse(list[24], culture);
            equip_class = Convert.ToUInt32(list[25]);
            equip_imask = Convert.ToUInt32(list[26], 16);
            equip_scmask = Convert.ToUInt32(list[27], 16);
            cast_min = Convert.ToInt32(list[28]);
            cast_max = Convert.ToInt32(list[29]);
            cast_div = Convert.ToInt32(list[30]);
            m_scaling = float.Parse(list[31], culture);
            scaling_level = Convert.ToUInt32(list[32]);
            replace_spellid = Convert.ToUInt32(list[33]);
            Attr1 = Convert.ToUInt32(list[34], 16);
            Attr2 = Convert.ToUInt32(list[35], 16);
            Attr3 = Convert.ToUInt32(list[36], 16);
            Attr4 = Convert.ToUInt32(list[37], 16);
            Attr5 = Convert.ToUInt32(list[38], 16);
            Attr6 = Convert.ToUInt32(list[39], 16);
            Attr7 = Convert.ToUInt32(list[40], 16);
            Attr8 = Convert.ToUInt32(list[41], 16);
            Attr9 = Convert.ToUInt32(list[42], 16);
            Attr10 = Convert.ToUInt32(list[43], 16);
            Attr11 = Convert.ToUInt32(list[44], 16);
            Attr12 = Convert.ToUInt32(list[45], 16);
			Flags1 = Convert.ToUInt32(list[46], 16);
			Flags2 = Convert.ToUInt32(list[47], 16);
			Flags3 = Convert.ToUInt32(list[48], 16);
			Flags4 = Convert.ToUInt32(list[49], 16);
			//Family = Convert.ToUInt32(list[49]);
            desc = list[50];
            tooltip = list[51];
            desc_vars = list[52];
            icon = list[53];
            active_icon = list[54];
            rank = list[55];
            effect_2 = list[56];
            effect_3 = list[57];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\t\t\t\tthis.Add( new DBCSpellData ( ");
            sb.Append(String.Format("\"{0}\", ", name));
            sb.Append(String.Format("{0}, ", id));
            sb.Append(String.Format("{0}, ", flags));
            sb.Append(String.Format("{0}f, ", speed));
            sb.Append(String.Format("ItemDamageType.{0}, ", school));
			if (Enum.IsDefined(typeof(CharacterClass), Class))
			{
				sb.Append(String.Format("CharacterClass.{0}, ", Class));
			}
			else
			{
				sb.Append("CharacterClass.Unknown, ");
			}
			if (Enum.IsDefined(typeof(CharacterRace), race))
			{
				sb.Append(String.Format("CharacterRace.{0}, ", race));
			}
			else
			{
				sb.Append("CharacterRace.Unknown, ");
			}
            sb.Append(String.Format("{0}, ", scaling));
            sb.Append(String.Format("{0}, ", max_scaling_level));
            sb.Append(String.Format("{0}f, ", extra_coeff));
            sb.Append(String.Format("{0}, ", level));
            sb.Append(String.Format("{0}, ", max_level));
            sb.Append(String.Format("{0}f, ", min_range));
            sb.Append(String.Format("{0}f, ", max_range));
            sb.Append(String.Format("{0}, ", cooldown));
            sb.Append(String.Format("{0}, ", gcd));
            sb.Append(String.Format("{0}, ", category));
            sb.Append(String.Format("{0}f, ", duration));
            sb.Append(String.Format("DKRune.{0}, ", rune_cost));
            sb.Append(String.Format("{0}, ", power_gain));
            sb.Append(String.Format("{0}, ", max_stack));
            sb.Append(String.Format("{0}, ", proc_chance));
            sb.Append(String.Format("{0}, ", initial_stack));
            sb.Append(String.Format("{0}, ", proc_flags));
            sb.Append(String.Format("{0}, ", icd));
            sb.Append(String.Format("{0}f, ", rppm));
            sb.Append(String.Format("{0}, ", equip_class));
            sb.Append(String.Format("{0}, ", equip_imask));
            sb.Append(String.Format("{0}, ", equip_scmask));
            sb.Append(String.Format("{0}, ", cast_min));
            sb.Append(String.Format("{0}, ", cast_max));
            sb.Append(String.Format("{0}, ", cast_div));
            sb.Append(String.Format("{0}f, ", m_scaling));
            sb.Append(String.Format("{0}, ", scaling_level));
            sb.Append(String.Format("{0}, ", replace_spellid));
            sb.Append(String.Format("{0}, ", Attr1));
            sb.Append(String.Format("{0}, ", Attr2));
            sb.Append(String.Format("{0}, ", Attr3));
            sb.Append(String.Format("{0}, ", Attr4));
            sb.Append(String.Format("{0}, ", Attr5));
            sb.Append(String.Format("{0}, ", Attr6));
            sb.Append(String.Format("{0}, ", Attr7));
            sb.Append(String.Format("{0}, ", Attr8));
            sb.Append(String.Format("{0}, ", Attr9));
            sb.Append(String.Format("{0}, ", Attr10));
            sb.Append(String.Format("{0}, ", Attr11));
            sb.Append(String.Format("{0}, ", Attr12));
            sb.Append(String.Format("\"{0}\", ", desc));
            sb.Append(String.Format("\"{0}\", ", tooltip));
            sb.Append(String.Format("\"{0}\", ", desc_vars));
            sb.Append(String.Format("\"{0}\", ", icon));
            sb.Append(String.Format("\"{0}\", ", active_icon));
            sb.Append(String.Format("\"{0}\", ", rank));
            sb.Append(String.Format("\"{0}\", ", effect_2));
            sb.Append(String.Format("\"{0}\"", effect_3));
            sb.Append(" ) );");
            return sb.ToString();
        }
    }
}
