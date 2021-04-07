using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    public abstract class CombatTable
    {
        protected Character Char;
        protected BossOptions bossOpts;
        //protected CombatFactors combatFactors;
        protected StatsEnhance StatS;
        protected AbilityEnhance_Base Abil;

        /// <summary>The Level Difference between you and the Target. Ranges from 0 to +3</summary>
        public int LevelDif { get { return bossOpts.Level - Char.Level; } }
        public bool isWhite;
        //private float _anyLand = 0f;
        //private float _anyNotLand = 1f;
        ///// <summary>Any attack that lands: 1f - AnyNotLand</summary>
        //public float AnyLand { get { return _anyLand; } }
        ///// <summary>Any attack that does not land: Dodges, Parries or Misses</summary>
        //public float AnyNotLand { get { return _anyNotLand; } }

        public float Miss { get; protected set; }
        public float Dodge { get; protected set; }
        public float Parry { get; protected set; }
        public float Block { get; protected set; }
        public float Glance { get; protected set; }
        public float Crit { get; protected set; }
        public float Hit { get; protected set; }

        protected void Initialize(Character character, BossOptions bo/*, CombatFactors cf*/, StatsEnhance stats, AbilityEnhance_Base ability, bool isWhite)
        {
            Char = character;
            StatS = stats;
            bossOpts = bo;
            //combatFactors = cf;
            Abil = ability;
            this.isWhite = isWhite;

            Calculate();
        }

        protected virtual void Calculate()
        {
            //_anyNotLand = Dodge + Parry + Miss;
            //_anyLand = 1f - _anyNotLand;
        }
    }

    public class AttackTable : CombatTable
    {
        protected override void Calculate()
        {
            float tableSize = 0f;

            // Miss
            if (Abil.IsSpell)
            {
                Miss = Math.Min(1f - tableSize, Math.Max(StatConversion.GetSpellMiss(LevelDif, false)
                     - (StatConversion.GetSpellHitFromRating(StatS.HitRating) + StatS.SpellHit), 0f));  // FIXME
            }
            else
            {
                Miss = Math.Min(1f - tableSize, 0f);  // FIXME
            }
            tableSize += Miss;

            // Dodge
            if (!Abil.IsSpell)
            {
                Dodge = Math.Min(1f - tableSize, 0f);  // FIXME
            }
            else
            {
                Dodge = 0f;
            }
            tableSize += Dodge;

            // Parry
            if (!Abil.IsSpell)
            {
                Parry = Math.Min(1f - tableSize, 0f);  // FIXME
            }
            else
            {
                Parry = 0f;
            }
            tableSize += Parry;

            // Block
            if (!Abil.IsSpell)
            {
                Block = Math.Min(1f - tableSize, 0f);  // FIXME
            }
            else
            {
                Block = 0f;
            }
            tableSize += Block;

            // Glancing Blow
            if (isWhite)
            {
                Glance = Math.Min(1f - tableSize, 0f);  // FIXME
            }
            else
            {
                Glance = 0f;
            }
            tableSize += Glance;

            // Critical Hit
            if (Abil.IsSpell)
            {
                Crit = Math.Min(1f - tableSize, StatConversion.NPC_LEVEL_SPELL_CRIT_MOD[LevelDif]
                     + StatConversion.GetSpellCritFromRating(StatS.CritRating) + Abil.BonusCritChance); // FIXME
            }
            else
            {
                Crit = Math.Min(1f - tableSize, StatConversion.NPC_LEVEL_CRIT_MOD[LevelDif]
                     + StatConversion.GetCritFromRating(StatS.CritRating) + Abil.BonusCritChance);  // FIXME
            }
            tableSize += Crit;

            // Normal Hit
            Hit = Math.Max(0f, 1f - tableSize);
            base.Calculate();
        }

        public AttackTable(Character character, BossOptions bo, StatsEnhance stats, AbilityEnhance_Base ability, bool isWhite)
        {
            Initialize(character, bo, stats, ability, isWhite);
        }
    }
}
