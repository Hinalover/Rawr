using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr
{
    abstract public class Spell
    {
        private SpellCompilation _liveSpellCompilation;
        private SpellCompilation _ptrSpellCompilation;

        private uint _ID = 0;
        public uint ID
        {
            get { return _ID; } 
            set
            {
                _ID = value;
                if (_ID > 0)
                {
                    _liveSpellCompilation = new DBCLive().getSpellInfo(_ID);
                    _ptrSpellCompilation = new DBCPTR().getSpellInfo(_ID);
                }
            }
        }

        private bool _PTR = false;
        public bool PTR { get { return _PTR; } set { _PTR = value; } }

        private DBCSpellData _Data
        {
            get
            {
                if (_PTR)
                    return _ptrSpellCompilation.Data;
                else
                    return _liveSpellCompilation.Data;
            }
        }

        private List<DBCSpellPower> _Resource
        {
            get
            {
                if (_PTR)
                    return _ptrSpellCompilation.Resource;
                else
                    return _liveSpellCompilation.Resource;
            }
        }

        public List<DBCSpellEffect> Effect
        {
            get
            {
                if (_PTR)
                    return _ptrSpellCompilation.Effect;
                else
                    return _liveSpellCompilation.Effect;
            }
        }


    }
}
