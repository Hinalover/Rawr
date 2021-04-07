using System.Collections.Generic;

namespace Rawr.Retribution
{
    public class CharacterCalculationsRetribution : CharacterCalculationsBase
    {
        public override float OverallPoints { get; set; }
        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints { get { return _subPoints; } set { _subPoints = value; } }
        public float DPSPoints { get { return _subPoints[0]; } set { _subPoints[0] = value; } }
        
        public float OtherDPS { get; set; }

        public Skill WhiteSkill { get; set; }
        public Skill SealSkill { get; set; }
        public Skill SealDotSkill { get; set; }
        public Skill CrusaderStrikeSkill { get; set; }
        public Skill TemplarsVerdictSkill { get; set; }
        public Skill CommandSkill { get; set; }
        public Skill JudgementSkill { get; set; }
        public Skill ConsecrationSkill { get; set; }
        public Skill ExorcismSkill { get; set; }
        public Skill HolyWrathSkill { get; set; }
        public Skill HammerOfWrathSkill { get; set; }
        public Skill GoakSkill { get; set; }
        
        public Stats BasicStats { get; set; }
        public Stats CombatStats { get; set; }
        public Character Character { get; set; }
        private RotationCalculation _rotation;
        public RotationCalculation Rotation { get { return _rotation; }
            set { 
                    _rotation = value;
                    CombatStats = _rotation.Stats;
                    Character = _rotation.Character;
                } 
        }

        private const string _normAtt = " Normal Attacks";
        private const string _aWAtt = " Avenging Wrath Attacks";
        private const string _skillOut = "{0:F2}*{1}";

        // Add calculated values to the values dictionary.
        // These values are then available for display via the CharacterDisplayCalculationLabels
        // member defined in CalculationsRetribution.cs
        // While possible, there's little reason to add values to the dictionary that are not being
        // used by the CharacterDisplayCalculationLabels.
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            // Basic stats
            dictValues["Health"] = string.Format("{0:N0}*Base Health: {1:N0}", CombatStats.Health, BasicStats.Health);
            dictValues["Mana"] = string.Format("{0:N0}*Base Mana: {1:N0}", CombatStats.Mana, BasicStats.Mana);
            dictValues["Strength"] = string.Format("{0:N0}*Base Strength: {1:N0}", CombatStats.Strength, BasicStats.Strength);
            dictValues["Agility"] = string.Format("{0:N0}*Base Agility: {1:N0}", CombatStats.Agility, BasicStats.Agility);
            dictValues["Attack Power"] = string.Format("{0:N0}*Base Attack Power: {1:N0}", CombatStats.AttackPower, BasicStats.AttackPower);
            dictValues["Melee Crit"] = string.Format("{0:P}*{1:0} Crit Rating ({2:P})", CombatStats.PhysicalCrit, BasicStats.CritRating, StatConversion.GetCritFromRating(BasicStats.CritRating, CharacterClass.Paladin));
            dictValues["Melee Haste"] = string.Format("{0:P}*{1:0} Haste Rating ({2:P})", CombatStats.PhysicalHaste, BasicStats.HasteRating, StatConversion.GetHasteFromRating(BasicStats.HasteRating, CharacterClass.Paladin));
            dictValues["Chance to Dodge"] = string.Format("{0:P}*{1:0} Expertise Rating ({2:F1})", ((BasePhysicalWhiteCombatTable)WhiteSkill.CT).ChanceToDodge, BasicStats.ExpertiseRating, BasicStats.Expertise);
            dictValues["Mastery"] = string.Format("{0:F2}*{1:0} Mastery Rating ({2:F1})\n{3:P} Hand of Light", (CombatStats.Mastery),
                                                                                                               BasicStats.MasteryRating, StatConversion.GetMasteryFromRating(BasicStats.MasteryRating, CharacterClass.Paladin),
                                                                                                               (CombatStats.Mastery) * PaladinConstants.HOL_COEFF);
            dictValues["Miss Chance"] = string.Format("{0:P}*{1:0} Hit Rating ({2:P})", WhiteSkill.CT.ChanceToMiss, BasicStats.HitRating, StatConversion.GetHitFromRating(BasicStats.HitRating, CharacterClass.Paladin));
            dictValues["Spell Power"] = string.Format("{0:N0}*Base Spell Power: {1:N0}", CombatStats.SpellPower, BasicStats.SpellPower);
            dictValues["Spell Crit"] = string.Format("{0:P}*{1:0} Crit Rating ({2:P})", CombatStats.SpellCrit, BasicStats.CritRating, StatConversion.GetCritFromRating(BasicStats.CritRating, CharacterClass.Paladin));
            dictValues["Spell Haste"] = string.Format("{0:P}*{1:0} Haste Rating ({2:P})", CombatStats.SpellHaste, BasicStats.HasteRating, StatConversion.GetHasteFromRating(BasicStats.HasteRating, CharacterClass.Paladin));
            dictValues["Weapon Damage"] = string.Format("{0:F}*Base Weapon Damage: {1:F}", AbilityHelper.WeaponDamage(Character, CombatStats.AttackPower), AbilityHelper.WeaponDamage(Character, BasicStats.AttackPower));
            dictValues["Weapon Damage @3.3"] = string.Format("{0:F}*Base Weapon Damage: {1:F}", AbilityHelper.WeaponDamage(Character, CombatStats.AttackPower, true), AbilityHelper.WeaponDamage(Character, BasicStats.AttackPower, true));
            dictValues["Attack Speed"] = string.Format("{0:F2}*Base Attack Speed: {1:F2}", AbilityHelper.WeaponSpeed(Character, CombatStats.PhysicalHaste), AbilityHelper.WeaponSpeed(Character, BasicStats.PhysicalHaste));

            // DPS Breakdown
            dictValues["Total DPS"] = OverallPoints.ToString("N0");
            dictValues["White"] = string.Format("{0:N0}*" + WhiteSkill, _rotation.GetDPS(DamageAbility.White));
            dictValues["Seal"] = string.Format("{0:N0}*" + SealSkill, _rotation.GetDPS(DamageAbility.Seal));
            dictValues["Seal (Dot)"] = string.Format("{0:N0}*" + SealDotSkill, _rotation.GetDPS(DamageAbility.SealDot));
            dictValues["Seal of Command"] = string.Format("{0:N0}*" + CommandSkill, _rotation.GetDPS(DamageAbility.SoC));
            dictValues["Crusader Strike"] = string.Format("{0:N0}*" + CrusaderStrikeSkill, _rotation.GetDPS(DamageAbility.CrusaderStrike));
            dictValues["Templars Verdict"] = string.Format("{0:N0}*" + TemplarsVerdictSkill, _rotation.GetDPS(DamageAbility.TemplarsVerdict));
            dictValues["Judgement"] = string.Format("{0:N0}*" + JudgementSkill, _rotation.GetDPS(DamageAbility.Judgement));
            dictValues["Consecration"] = string.Format("{0:N0}*" + ConsecrationSkill, _rotation.GetDPS(DamageAbility.Consecration));
            dictValues["Exorcism"] = string.Format("{0:N0}*" + ExorcismSkill, _rotation.GetDPS(DamageAbility.Exorcism));
            dictValues["Holy Wrath"] = string.Format("{0:N0}*" + HolyWrathSkill, _rotation.GetDPS(DamageAbility.HolyWrath));
            dictValues["Hammer of Wrath"] = string.Format("{0:N0}*" + HammerOfWrathSkill, _rotation.GetDPS(DamageAbility.HammerOfWrath));
            dictValues["GoaK"] = string.Format("{0:N0}*" + GoakSkill, _rotation.GetDPS(DamageAbility.GoaK));
            dictValues["Other"] = OtherDPS.ToString("N0");

            // Rotation Info:
            dictValues["Inqusition Uptime"] = CrusaderStrikeSkill.InqUptime.ToString("P2");
            dictValues["White Usage"] = SkillDesc(_rotation.Casts[DamageAbility.White]);
            dictValues["Seal Usage"] = SkillDesc(_rotation.Casts[DamageAbility.Seal]);
            dictValues["Seal (Dot) Usage"] = SkillDesc(_rotation.Casts[DamageAbility.SealDot]);
            dictValues["Seal of Command Usage"] = SkillDesc(_rotation.Casts[DamageAbility.SoC]);
            dictValues["Crusader Strike Usage"] = SkillDesc(_rotation.Casts[DamageAbility.CrusaderStrike]);
            dictValues["Templar's Verdict Usage"] = SkillDesc(_rotation.Casts[DamageAbility.TemplarsVerdict]);
            dictValues["Exorcism Usage"] = SkillDesc(_rotation.Casts[DamageAbility.Exorcism]);
            dictValues["Hammer of Wrath Usage"] = SkillDesc(_rotation.Casts[DamageAbility.HammerOfWrath]);
            dictValues["Judgement Usage"] = SkillDesc(_rotation.Casts[DamageAbility.Judgement]);
            dictValues["Holy Wrath Usage"] = SkillDesc(_rotation.Casts[DamageAbility.HolyWrath]);
            dictValues["Consecration Usage"] = SkillDesc(_rotation.Casts[DamageAbility.Consecration]);
            dictValues["GoaK Usage"] = SkillDesc(_rotation.Casts[DamageAbility.GoaK]);

            return dictValues;
        }

        private string SkillDesc(sCasts casts)
        {
            string output = "";
            foreach (KeyValuePair<float, float> kvp in casts.CastDist)
            {
                output += string.Format("{0,3:F2} attacks with {1:F2} % damage\n", kvp.Value, kvp.Key);
            }
            if (output.Length != 0)
                output = output.Remove(output.Length - 1);
            return string.Format(_skillOut, casts.Casts, output);
        }

        /// <summary>
        /// Obtain optimizable values.
        /// </summary>
        /// <param name="calculation"></param>
        /// <returns></returns>
        /// The list of labels listed here needs to match with the list in OptimizableCalculationLabels override in CalculationsRetribution.cs
        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "% Chance to Miss (Melee)": return WhiteSkill.CT.ChanceToMiss * 100f;  // White and Melee hit for ret are identical since we can't dual wield.
                case "% Chance to Miss (Spells)": return ExorcismSkill.CT.ChanceToMiss * 100f;
                case "% Chance to be Dodged": return ((BasePhysicalWhiteCombatTable)WhiteSkill.CT).ChanceToDodge * 100f;
                case "% Chance to be Parried": return ((BasePhysicalWhiteCombatTable)WhiteSkill.CT).ChanceToParry * 100f;
                case "% Chance to be Avoided (Yellow/Dodge)": return (1f - CrusaderStrikeSkill.CT.ChanceToLand) * 100f;
            }
            return 0f;
        }
    }
}