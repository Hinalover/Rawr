using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public class CharacterCalculationsProtPaladin : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }
        public List<Buff> ActiveBuffs { get; set; }
        private Dictionary<string, float> _abilities;
        public Dictionary<string, float> Abilities
        {
            get
            {
                return _abilities ?? (_abilities = new Dictionary<string,float>());
            }
            set
            {
                _abilities = value;
            }
        }

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float SurvivabilityPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float MitigationPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float ThreatPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }

        public float RecoveryPoints
        {
            get { return _subPoints[3]; }
            set { _subPoints[3] = value; }
        }
        public int Level { get; set; }
        // Target Info
        public int TargetLevel { get; set; }

        //Basic Tank Defensive Stats
        public float Dodge { get; set; }
        public float Parry { get; set; }
        public float Miss { get; set; }

        // Shield Tank Defensive Stats
        public float Block { get; set; }
        public float Mastery { get; set; }

        // Basic Offensive Stats
        public float MissedAttacks { get; set; }
        public float AvoidedAttacks { get; set; }
        public float DodgedAttacks { get; set; }
        public float ParriedAttacks { get; set; }
        public float GlancingAttacks { get; set; }
        public float GlancingReduction { get; set; }
        public float BlockedAttacks { get; set; }
        public float Hit { get; set; }
        public float SpellHit { get; set; }
        public float Crit { get; set; }
        public float SpellCrit { get; set; }
        public float Expertise { get; set; }
        public float Haste { get; set; }
        public float SpellHaste { get; set; }
        public float WeaponSpeed { get; set; }
        public float DPS { get; set; }
        public float TPS { get; set; }

        // Defensive Stats
        public float GuaranteedReduction { get; set; }
        public float TotalMitigation { get; set; }
        public float AttackerSpeed { get; set; }
        public float DTPS { get; set; }
        public float DamagePerHit { get; set; }
        public float DamagePerBlock { get; set; }

        public float AverageVengeanceAP { get; set; }
        public double AvengingWrathUptime { get; set; }
        public double HolyAvengerUptime { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Strength", BasicStats.Strength.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Stamina", string.Format("{0}*Increases Health by {1}", BasicStats.Stamina, (BasicStats.Stamina - 20f) * 10f + 20f));
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Armor", string.Format("{0}*Reduces physical damage taken by {1:0.00%}" + Environment.NewLine +
                                                  "Armor Damage Reduction depends on Attacker Level.",
                                                  BasicStats.Armor, StatConversion.GetArmorDamageReduction(TargetLevel, Level, BasicStats.Armor, 0, 0)));
            dictValues.Add("Dodge", string.Format("{0:0.00%}*Dodge Rating {1}, {2:0.00%} Dodge before DR", Dodge, BasicStats.DodgeRating, BasicStats.Dodge + StatConversion.GetDodgeFromAgility(BasicStats.Agility, CharacterClass.Paladin) + StatConversion.GetDodgeFromRating(BasicStats.DodgeRating)));
            dictValues.Add("Parry", string.Format("{0:0.00%}*Parry Rating {1}, {2:0.00%} Parry before DR", Parry, BasicStats.ParryRating, BasicStats.Parry + StatConversion.GetParryFromStrength(BasicStats.Strength) + StatConversion.GetParryFromRating(BasicStats.ParryRating)));
            dictValues.Add("Block", string.Format("{0:0.00%}", Block));
            dictValues.Add("Mastery", string.Format("{0:0.00}*Mastery Rating {1}" + Environment.NewLine +
                                                    "Adds {2:0.00%} Block" + Environment.NewLine +
                                                    "Adds {3:0.00%} to Shield of the Righteous DR",
                                                    Mastery,
                                                    BasicStats.MasteryRating,
                                                    Mastery * 0.01f,
                                                    0.3f + Mastery * 0.01f));
            dictValues.Add("Miss", string.Format("{0:0.00%}", Miss));
            dictValues.Add("Guaranteed Reduction", string.Format("{0:0.00%}", GuaranteedReduction));
            dictValues.Add("Total Mitigation", string.Format("{0:0.00%}", TotalMitigation));

            dictValues.Add("Attacker Speed", string.Format("{0:0.00}s", AttackerSpeed));

            dictValues.Add("Damage Taken",
                string.Format("{0:0.0} DPS*{1:0} damage per normal attack" + Environment.NewLine +
                                "{2:0} damage per blocked attack", DTPS, DamagePerHit, DamagePerBlock));


            dictValues.Add("Weapon Speed", string.Format("{0:0.00}*{1:0.00%} Haste", WeaponSpeed, Haste));
            dictValues.Add("Attack Power", string.Format("{0}", BasicStats.AttackPower));
            dictValues.Add("Spell Power", string.Format("{0}", BasicStats.SpellPower));
            dictValues.Add("Hit", string.Format("{0:0.00%}*Hit Rating {1}" + Environment.NewLine + "Against a Target of Level {2}", Hit, BasicStats.HitRating, TargetLevel));
            dictValues.Add("Spell Hit", string.Format("{0:0.00%}*Hit Rating {1}" + Environment.NewLine + "Against a Target of Level {2}",
                                                      SpellHit, BasicStats.HitRating, TargetLevel));
            dictValues.Add("Expertise",
                string.Format("{0:0.00%}*Expertise Rating {1}", Expertise, BasicStats.ExpertiseRating));
            dictValues.Add("Haste", string.Format("{0:0.00%}*Haste Rating {1:0.00}", Haste, BasicStats.HasteRating));
            dictValues.Add("Spell Haste", string.Format("{0:0.00%}*Haste Rating {1:0.00}", SpellHaste, BasicStats.HasteRating));
            
            dictValues.Add("Crit", string.Format("{0:0.00%}*Crit Rating {1}" + Environment.NewLine + "Against a Target of Level {2}",
                                                 Crit, BasicStats.CritRating, TargetLevel));
            dictValues.Add("Spell Crit", string.Format("{0:0.00%}*Crit Rating {1}" + Environment.NewLine + "Against a Target of Level {2}",
                                                       SpellCrit, BasicStats.CritRating, TargetLevel));
            dictValues.Add("Weapon Damage", string.Format("{0:0.00}*As average damage per melee swing before armor", 
                                                          BasicStats.WeaponDamage));
            dictValues.Add("Average Vengeance AP", string.Format("{0:0}", AverageVengeanceAP));
            dictValues.Add("Avoided Attacks",
                string.Format("{0:0.00%}*Attacks Missed: {1:0.00%}" + Environment.NewLine + "Attacks Dodged: {2:0.00%}" + Environment.NewLine +
                                "Attacks Parried: {3:0.00%}", AvoidedAttacks, MissedAttacks, DodgedAttacks, ParriedAttacks));
            dictValues.Add("DPS", string.Format("{0:0.0}", DPS));
            dictValues.Add("TPS", string.Format("{0:0.0}", TPS));

            dictValues.Add("Overall Points", string.Format("{0:0}", OverallPoints));
            dictValues.Add("Mitigation Points", string.Format("{0:0}", MitigationPoints));
            dictValues.Add("Survival Points", string.Format("{0:0}*Effective Health", SurvivabilityPoints));
            dictValues.Add("Threat Points", string.Format("{0:0}", ThreatPoints));
            dictValues.Add("Recovery Points", string.Format("{0:0}", RecoveryPoints));

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
            }
            return 0.0f;
        }
    }
}