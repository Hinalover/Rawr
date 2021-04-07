using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr
{
    public class SpellCompilation
    {
        public DBCSpellData Data;
        public List<DBCSpellEffect> Effect;
        public List<DBCSpellPower> Resource;
        private List<DBCRPPMModifier> RPPM_Modifier;
        public uint buildLevel = 0;

        private bool _ptr = false;
        public bool PTR
        {
            get { return _ptr; }
            set
            {
                _ptr = value;
				if (Effect != null && (Effect.Count > 0) && (Data != null))
                    for (int i = 0; i < Effect.Count; i++)
                        Effect[i].PTR = value;
                if (Resource != null && (Resource.Count > 0) && (Data != null))
                    for (int i = 0; i < Resource.Count; i++)
                        Resource[i].PTR = value;
            }
        }

        private int _player_level = StatConversion.DEFAULTPLAYERLEVEL;
        public int player_level
        {
            get { return _player_level; }
            set
            {
                if (value <= StatConversion.DEFAULTPLAYERLEVEL)
                    _player_level = value;
                else
                    _player_level = StatConversion.DEFAULTPLAYERLEVEL;
                if ((Effect.Count > 0) && (Data != null))
                {
                    for (int i = 0; i < Effect.Count; i++)
                        Effect[i].player_level = _player_level;
                }
            }
        }

        public SpellCompilation()
        {
            Data = new DBCSpellData();
            Effect = new List<DBCSpellEffect>();
            Resource = new List<DBCSpellPower>();
            RPPM_Modifier = new List<DBCRPPMModifier>();
        }

        public SpellCompilation(DBCSpellData data, List<DBCSpellEffect> effect, List<DBCSpellPower> resource, List<DBCRPPMModifier> rppm, uint build)
        {
            Data = data;
            Effect = effect;
            Resource = resource;
            RPPM_Modifier = rppm;
            buildLevel = build;
            PTR = false;

            if ((Effect.Count > 0) && (Data != null))
            {
                for (int i = 0; i < Effect.Count; i++)
                {
                    Effect[i].spell = Data;
                    Effect[i].player_level = _player_level;
                }
            }

            if ((Resource.Count > 0) && (Data != null))
                for (int i = 0; i < Resource.Count; i++)
                    Resource[i].spell = Data;
        }

        public SpellCompilation(DBCSpellData data, List<DBCSpellEffect> effect, List<DBCSpellPower> resource, List<DBCRPPMModifier> rppm, uint build, bool ptr)
        {
            Data = data;
            Effect = effect;
            Resource = resource;
            RPPM_Modifier = rppm;
            buildLevel = build;
            PTR = ptr;

            if (Effect != null && (Effect.Count > 0) && (Data != null))
            {
                for (int i = 0; i < Effect.Count; i++)
                {
                    Effect[i].spell = Data;
                    Effect[i].player_level = _player_level;
                }
            }

            if (Resource != null && (Resource.Count > 0) && (Data != null))
                for (int i = 0; i < Resource.Count; i++)
                    Resource[i].spell = Data;
        }

        public float getRPPM(Specialization spec)
        {
            if (RPPM_Modifier.Count > 0)
                return Data.rppm + findRPPMBySpec(spec);
            else
                return Data.rppm;
        }

        private float findRPPMBySpec(Specialization spec)
        {
            List<DBCRPPMModifier> RPPM = new List<DBCRPPMModifier>();
            var query = from DBCRPPMModifier rppm in RPPM_Modifier
                        where rppm.specialization == spec
                        select rppm;

            foreach (DBCRPPMModifier data in query)
                RPPM.Add(data);

            if (RPPM.Count > 0)
                return RPPM[0].value;
            else
                return 0;
        }
    }
}
