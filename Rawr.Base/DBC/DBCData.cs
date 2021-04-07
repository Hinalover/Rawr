using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Rawr
{
    abstract public class DBCData
    {
        protected List<DBCSpellData> Spell_Data = new List<DBCSpellData>();
        protected List<DBCSpellEffect> SpellEffect_Data = new List<DBCSpellEffect>();
        protected List<DBCSpellPower> SpellPower_Data = new List<DBCSpellPower>();
        protected List<DBCRPPMModifier> RPPMModifier_Data = new List<DBCRPPMModifier>();
        protected bool PTR { get; set; }
        protected uint buildLevel { get; set; }

        public void Add(DBCSpellData data)
        {
            Spell_Data.Add(data);
        }

        public void Add(DBCSpellEffect data)
        {
            SpellEffect_Data.Add(data);
        }

        public void Add(DBCSpellPower data)
        {
            SpellPower_Data.Add(data);
        }

        public void Add(DBCRPPMModifier data)
        {
            RPPMModifier_Data.Add(data);
        }

        /// <summary>
        /// Returns Base Spell Data information for a given spell id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Base Spell Data</returns>
        private DBCSpellData getSpellData(uint id)
        {
            List<DBCSpellData> SpellData = new List<DBCSpellData>();
            var query = from DBCSpellData spellData in Spell_Data
                        where spellData.id == id
                        select spellData;

            foreach (DBCSpellData data in query)
                SpellData.Add(data);

            if (SpellData.Count > 0 )
                return SpellData[0];
            else
                return null;
        }

        /// <summary>
        /// Returns a list of Spell Effects for a given spell id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<DBCSpellEffect> getSpellEffect (uint id)
        {
            List<DBCSpellEffect> SpellEffect = new List<DBCSpellEffect>();
            var query = from DBCSpellEffect spellEffect in SpellEffect_Data
                        where spellEffect.spell_id == id
                        select spellEffect;

            foreach (DBCSpellEffect data in query)
                SpellEffect.Add(data);

            if (SpellEffect.Count > 0)
                return SpellEffect;
            else
                return null;
        }

        /// <summary>
        /// Returns information on the resource and cost of the spell
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<DBCSpellPower> getSpellCost (uint id)
        {
            List<DBCSpellPower> SpellPower = new List<DBCSpellPower>();
            var query = from DBCSpellPower spellPower in SpellPower_Data
                        where spellPower.spell_id == id
                        select spellPower;

            foreach (DBCSpellPower data in query)
                SpellPower.Add(data);

            if (SpellPower.Count > 0)
                return SpellPower;
            else
                return null;
        }

        /// <summary>
        /// Returns Information if a spell's Real PPM is modified by a specific Specialization
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<DBCRPPMModifier> getRPPMModifier(uint id)
        {
            List<DBCRPPMModifier> RPPM = new List<DBCRPPMModifier>();
            var query = from DBCRPPMModifier rppm in RPPMModifier_Data
                        where rppm.id == id
                        select rppm;

            foreach (DBCRPPMModifier data in query)
                RPPM.Add(data);

            if (RPPM.Count > 0)
                return RPPM;
            else
                return null;
        }

        public SpellCompilation getSpellInfo(uint id)
        {
            return new SpellCompilation(getSpellData(id), getSpellEffect(id), getSpellCost(id), getRPPMModifier(id), buildLevel, PTR);
        }
    }
}
