using System;
using System.Collections.Generic;

namespace Rawr.Retribution
{
    public class sCasts
    {
        public double CastsPerSec;

        private float _casts = 0f;
        public float Casts 
        { 
            get { return _casts; } 
            private set 
            { 
                _casts = value; 
                CastsPerSec = _casts / _fightlength; 
            } 
        }
        public float CastsNormal = 0f;

        private float _fightlength = 1f;
        public float Fightlength
        {
            get { return _fightlength; }
            set
            {
                _fightlength = value;
                CastsPerSec = _casts / _fightlength;
            }
        }

        private Dictionary<float, float> _castDist = new Dictionary<float, float>();
        public Dictionary<float, float> CastDist { get { return _castDist; } }

        public void AddCasts(float casts, float percentDamage)
        {
            if (casts > 0f)
            {
                if (_castDist.ContainsKey(percentDamage))
                {
                    _castDist[percentDamage] += casts;
                }
                else
                    _castDist.Add(percentDamage, casts);

                Casts += casts;
                CastsNormal += casts * percentDamage;
            }
        }
    }

    public class RotationInfo
    {
        public bool Below20;
        public bool Zeal;
        public Dictionary<DamageAbility, float> Casts;

        public RotationInfo(bool below20, bool zeal, Ability[] allAb)
        {
            Below20 = below20;
            Zeal = zeal;

            Casts = new Dictionary<DamageAbility, float>();
            foreach (DamageAbility abil in allAb)
            {
                Casts.Add(abil, 0f);
            }
        }
    }

    public class RotationCalculation
    {
        public StatsRetri Stats { get; private set; }
        public Character Character { get; private set; }
        public CalculationOptionsRetribution CalcOpts { get; private set; }

        public Skill CS { get { return skills[DamageAbility.CrusaderStrike]; } }
        public Skill TV { get { return skills[DamageAbility.TemplarsVerdict]; } }
        public Skill Judge { get { return skills[DamageAbility.Judgement]; } }
        public Skill Exo { get { return skills[DamageAbility.Exorcism]; } }
        public Skill HW { get { return skills[DamageAbility.HolyWrath]; } }
        public Skill HoW { get { return skills[DamageAbility.HammerOfWrath]; } }
        public Skill Cons { get { return skills[DamageAbility.Consecration]; } }
        public Skill Seal { get { return skills[DamageAbility.Seal]; } }
        public Skill SealDot { get { return skills[DamageAbility.SealDot]; } }
        public Skill SoC { get { return skills[DamageAbility.SoC]; } }
        public Skill GoaK { get { return skills[DamageAbility.GoaK]; } }
        public White White { get { return (White)skills[DamageAbility.White]; } }

        public Dictionary<DamageAbility, sCasts> Casts = new Dictionary<DamageAbility, sCasts>();
        private Dictionary<DamageAbility, Skill> skills = new Dictionary<DamageAbility, Skill>();
        private Dictionary<DamageAbility, double> abilityHitPerSec = new Dictionary<DamageAbility, double>();
        private Dictionary<DamageAbility, double> abilityCritPerSec = new Dictionary<DamageAbility, double>();

        
        public RotationCalculation(Character character, StatsRetri stats)
        {
            Character = character;
            Stats = stats;
            CalcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            dpChance = character.PaladinTalents.DivinePurpose * PaladinConstants.DP_CHANCE;

            #region Initialization
            foreach (DamageAbility abb in DamageAbilities)
            {
                Casts[abb] = new sCasts();
            }
            
            skills[DamageAbility.CrusaderStrike] = new CrusaderStrike(Character, Stats);
            skills[DamageAbility.TemplarsVerdict] = new TemplarsVerdict(Character, Stats);
            skills[DamageAbility.White] = new White(Character, Stats);
            skills[DamageAbility.Exorcism] = new Exorcism(Character, Stats, White.CT.ChanceToLand);
            skills[DamageAbility.Inquisition] = new Inquisition(Character, Stats, CalcOpts.HPperInq);
            skills[DamageAbility.HolyWrath] = new HolyWrath(Character, Stats);
            skills[DamageAbility.HammerOfWrath] = new HammerOfWrath(Character, Stats);
            skills[DamageAbility.Consecration] = new Consecration(Character, Stats);
            skills[DamageAbility.GoaK] = new GuardianOfTheAncientKings(Character, Stats);
            skills[DamageAbility.SoC] = new SealOfCommand(Character, Stats);

            switch (CalcOpts.Seal)
            {
                case SealOf.Righteousness:
                    skills[DamageAbility.Seal] = new SealOfRighteousness(Character, Stats);
                    skills[DamageAbility.SealDot] = new NullSealDoT(Character, Stats);
                    skills[DamageAbility.Judgement] = new JudgementOfRighteousness(Character, Stats);
                    break;
                case SealOf.Truth:
                    skills[DamageAbility.Seal] = new SealOfTruth(Character, Stats);
                    skills[DamageAbility.SealDot] = new SealOfTruthDoT(Character, Stats, 5f);
                    skills[DamageAbility.Judgement] = new JudgementOfTruth(Character, Stats, 5f);
                    break;
                default:
                    skills[DamageAbility.Seal] = new NullSeal(Character, Stats);
                    skills[DamageAbility.SealDot] = new NullSealDoT(Character, Stats);
                    skills[DamageAbility.Judgement] = new NullJudgement(Character, Stats);
                    break;
            }
            #endregion

            CalcRotation();
        }

        #region Rotation

        private readonly Ability[] CastedAbilities = { Ability.Consecration, Ability.CrusaderStrike, Ability.Exorcism, Ability.HammerOfWrath, Ability.HolyWrath, Ability.Inquisition, Ability.Judgement, Ability.TemplarsVerdict };
        private readonly DamageAbility[] DamageAbilities = { DamageAbility.Consecration, DamageAbility.CrusaderStrike, DamageAbility.Exorcism, DamageAbility.GoaK, DamageAbility.HammerOfWrath, DamageAbility.HolyWrath, DamageAbility.Inquisition, DamageAbility.Judgement, DamageAbility.Seal, DamageAbility.SealDot, DamageAbility.SoC, DamageAbility.TemplarsVerdict, DamageAbility.White };
        private float dpChance;

        private void DoRotation(double fightlength, float normGCD, bool below20, bool zeal, Dictionary<DamageAbility, float> tmpCast)
        {
            float old_holyPower; 
            float holyPower = 0f;
            float holyPowerDP;
            double numOfGCD = fightlength / normGCD;
            
            int iterator = 0;
            while (iterator < 10)
            {
                double remainingNumOfGCD = numOfGCD;
                old_holyPower = holyPower;
                holyPower = 0f;

                holyPowerDP = (tmpCast[DamageAbility.Inquisition] +
                               tmpCast[DamageAbility.TemplarsVerdict] +
                               tmpCast[DamageAbility.Exorcism] +
                               tmpCast[DamageAbility.Judgement] +
                               tmpCast[DamageAbility.HolyWrath])
                              * dpChance;
                float addHPWrapUpTime = Math.Max(CalcOpts.HPperInq / (zeal ? 3f : 1f), 1f)* CS.CooldownWithLatency;
                float dpProcWrapUpTime = (float) fightlength / holyPowerDP;
                addHPWrapUpTime = dpProcWrapUpTime / (1f + dpProcWrapUpTime / addHPWrapUpTime);

                //Inq has the highest priority
                if (old_holyPower > 0f) {
                    tmpCast[DamageAbility.Inquisition] = (float)fightlength / (skills[DamageAbility.Inquisition].CooldownWithLatency - CalcOpts.InqRefresh + addHPWrapUpTime);
                    remainingNumOfGCD -= skills[DamageAbility.Inquisition].GCDPercentage * tmpCast[DamageAbility.Inquisition];
                    holyPower -= tmpCast[DamageAbility.Inquisition] * CalcOpts.HPperInq;
                }

                //Do Holy Power TV
                tmpCast[DamageAbility.TemplarsVerdict] = ExecuteSkill(DamageAbility.TemplarsVerdict, ref remainingNumOfGCD, old_holyPower / 3f);
                //Do CS
                tmpCast[DamageAbility.CrusaderStrike] = ExecuteSkill(DamageAbility.CrusaderStrike, fightlength, ref remainingNumOfGCD);
                holyPower += tmpCast[DamageAbility.CrusaderStrike] * (zeal ? 3f : 1f) * skills[DamageAbility.CrusaderStrike].CT.ChanceToLand;

                //Do HoW
                if (below20) {
                    tmpCast[DamageAbility.HammerOfWrath] = ExecuteSkill(DamageAbility.HammerOfWrath, fightlength, ref remainingNumOfGCD);                    
                }
                //Do DP TV
                tmpCast[DamageAbility.TemplarsVerdict] += ExecuteSkill(DamageAbility.TemplarsVerdict, ref remainingNumOfGCD, holyPowerDP);
                //Do Exo
                tmpCast[DamageAbility.Exorcism] = ExecuteSkill(DamageAbility.Exorcism, fightlength, ref remainingNumOfGCD);
                //Do Judge
                tmpCast[DamageAbility.Judgement] = ExecuteSkill(DamageAbility.Judgement, fightlength, ref remainingNumOfGCD);
                //Do HW 
                tmpCast[DamageAbility.HolyWrath] = ExecuteSkill(DamageAbility.HolyWrath, fightlength, ref remainingNumOfGCD);
                //Do Cons
                tmpCast[DamageAbility.Consecration] = ExecuteSkill(DamageAbility.Consecration, fightlength, ref remainingNumOfGCD);
                
                iterator++;
            }
        }

        private float ExecuteSkill(DamageAbility abil, double fightlength, ref double remainingNumOfGCD)
        {
            return ExecuteSkill(abil, ref remainingNumOfGCD, fightlength / skills[abil].CooldownWithLatency);
        }

        private float ExecuteSkill(DamageAbility abil, ref double remainingNumOfGCD, double count)
        {
            if (remainingNumOfGCD > 0d && count > 0d)
            {
                double usedGCD = Math.Min(remainingNumOfGCD, skills[abil].GCDPercentage * count);
                remainingNumOfGCD -= usedGCD;
                return (float)(usedGCD / skills[abil].GCDPercentage);
            }
            return 0f;
        }

        public void CalcRotation()
        {
            float normGCD = (1.5f + .1f);
            float lostTime = Impedance.GetTotalImpedancePercs(Character.BossOptions, PLAYER_ROLES.MeleeDPS, Stats.MovementSpeed, Stats.FearDurReduc, Stats.StunDurReduc, 
                                                                                                            Stats.SnareRootDurReduc, Stats.SilenceDurReduc);
            float fightlength = Character.BossOptions.BerserkTimer;
            float fightLengthAttacking = fightlength * (1f - lostTime);
            
            float timeZeal = (fightlength / PaladinConstants.ZEAL_COOLDOWN) * (PaladinConstants.ZEAL_DURATION + (Stats.T12_4P ? 15f : 0f));
            float timeAW = (fightlength / (PaladinConstants.AW_COOLDOWN - Character.PaladinTalents.SanctifiedWrath * PaladinConstants.SANCTIFIED_WRATH_CD)) * PaladinConstants.AW_DURATION;
            float timeBoth = 0;
            if (timeAW > timeZeal)
            {
                timeBoth = timeZeal;
                timeAW -= timeZeal;
                timeZeal = 0f;
            }
            else
            {
                timeBoth = timeAW;
                timeZeal -= timeAW;
                timeAW = 0f;
            }
            float timeNormal = fightlength - timeAW - timeBoth - timeZeal;

            float uTNormal = (float)Character.BossOptions.Under20Perc * timeNormal;
            float uTZeal = (float)Character.BossOptions.Under20Perc * timeZeal;
            float uTAW = (float)Character.BossOptions.Under20Perc * timeAW;
            float uTBoth = (float)Character.BossOptions.Under20Perc * timeBoth;
            timeNormal -= uTNormal;
            timeZeal -= uTZeal;
            timeAW -= uTAW;
            timeBoth -= uTBoth;

            RotationInfo[] infos = new RotationInfo[] { new RotationInfo(false, false, CastedAbilities), //Normal rotation, no zeal, above 20%
                                                        new RotationInfo(false, true, CastedAbilities),  //Zeal only, above 20%
                                                        new RotationInfo(true, false, CastedAbilities),  //AW or below 20%, no Zeal
                                                        new RotationInfo(true, true, CastedAbilities) }; //AW or below 20% AND Zeal
            foreach (RotationInfo info in infos)
            {
                DoRotation(100f, normGCD, info.Below20, info.Zeal, info.Casts);
            }
            AddCasts(infos[0] ,timeNormal, false, false);
            AddCasts(infos[2], uTNormal, false, false);

            AddCasts(infos[1], timeZeal, false, true);
            AddCasts(infos[3], uTZeal, false, true);

            AddCasts(infos[2], timeAW + uTAW, true, false);
            
            AddCasts(infos[3], timeBoth + uTBoth, true, true);

            float[] percent = new float[4] { (timeNormal + uTNormal) / fightlength,
                                             (timeAW + uTAW) / fightlength,
                                             (timeZeal + uTZeal) / fightlength,
                                             (timeBoth + uTBoth) / fightlength };

            SetCasts(DamageAbility.White, percent, fightLengthAttacking / AbilityHelper.WeaponSpeed(Character, Stats.PhysicalHaste));
            SetCasts(DamageAbility.GoaK, percent, fightlength / PaladinConstants.GOAK_COOLDOWN);
            SetCasts(DamageAbility.SealDot, percent, (float)(fightlength * SealDotProcPerSec(Seal)));

            SetSealProcs(Seal.GetType(), DamageAbility.Seal);
            SetSealProcs(Seal.GetType(), DamageAbility.SoC);

            float inquptime = Math.Min((Casts[DamageAbility.Inquisition].Casts * skills[DamageAbility.Inquisition].Cooldown) / fightLengthAttacking, 1f);

            //UsagePerSecCalc
            foreach (DamageAbility abb in DamageAbilities)
            {
                Casts[abb].Fightlength = fightlength;
                skills[abb].InqUptime = inquptime;

            }
            CalculatePerSeconds();
        }

        private void AddCasts(RotationInfo info, float length, bool IsAW, bool IsZeal)
        {
            float weight = length/100f;
            float percentDamage = (IsAW ? 1 + PaladinConstants.AW_DMG_BONUS : 1f) * 
                                   (IsZeal && Stats.T13_4P ? 1f + PaladinConstants.T13_4P_DMG_BONUS : 1f);
            foreach (DamageAbility abb in CastedAbilities)
            {
                Casts[abb].AddCasts(weight * info.Casts[abb], percentDamage);
            }
        }
        private void SetCasts(DamageAbility abb, float[] dist, float count)
        {
            float percentDamage;
            for (int i = 0; i < dist.Length; i++)
            {
                switch (i)
                {
                    case 1: percentDamage = 1f + PaladinConstants.AW_DMG_BONUS; break;
                    case 2: percentDamage = 1f + (Stats.T13_4P ? PaladinConstants.T13_4P_DMG_BONUS : 0f); break;
                    case 3: percentDamage = (1 + PaladinConstants.AW_DMG_BONUS) * 
                                            (Stats.T13_4P ? 1f + PaladinConstants.T13_4P_DMG_BONUS : 1f); break;
                    default: percentDamage = 1f; break;
                }
                Casts[abb].AddCasts(count * dist[i], percentDamage);
            }
        }
        private void SetSealProcs(Type seal, DamageAbility skill)
        {
            if (seal == typeof(SealOfTruth))
            {
                AddCastsFromTo(DamageAbility.CrusaderStrike, skill);
                AddCastsFromTo(DamageAbility.TemplarsVerdict, skill);
                AddCastsFromTo(DamageAbility.White, skill);
                AddCastsFromTo(DamageAbility.Judgement, skill);
                AddCastsFromTo(DamageAbility.HammerOfWrath, skill);
                AddCastsFromTo(DamageAbility.Exorcism, skill);
            }
            else if (seal == typeof(SealOfRighteousness))
            {
                AddCastsFromTo(DamageAbility.CrusaderStrike, skill);
                AddCastsFromTo(DamageAbility.TemplarsVerdict, skill);
                AddCastsFromTo(DamageAbility.White, skill);
                AddCastsFromTo(DamageAbility.HammerOfWrath, skill);
            }
        }
        private void AddCastsFromTo(DamageAbility From, DamageAbility To)
        {
            foreach (KeyValuePair<float, float> kvp in Casts[From].CastDist)
            {
                Casts[To].AddCasts(kvp.Value, kvp.Key);
            }
        }
        #endregion
        public float GetDPS(DamageAbility skill)
        {
            return (Casts[skill].CastsNormal * skills[skill].AverageDamageWithTriggers) / Character.BossOptions.BerserkTimer;
        }

        public void SetDPS(CharacterCalculationsRetribution calc)
        {
            calc.WhiteSkill = White;
            calc.SealSkill = Seal;
            calc.SealDotSkill = SealDot;
            calc.CommandSkill = SoC;
            calc.JudgementSkill = Judge;
            calc.TemplarsVerdictSkill = TV;
            calc.CrusaderStrikeSkill = CS;
            calc.ConsecrationSkill = Cons;
            calc.ExorcismSkill = Exo;
            calc.HolyWrathSkill = HW;
            calc.HammerOfWrathSkill = HoW;
            calc.GoakSkill = GoaK;

            calc.DPSPoints = GetDPS(DamageAbility.White) +
                GetDPS(DamageAbility.Seal) +
                GetDPS(DamageAbility.SealDot) +
                GetDPS(DamageAbility.SoC) +
                GetDPS(DamageAbility.Judgement) +
                GetDPS(DamageAbility.CrusaderStrike) +
                GetDPS(DamageAbility.TemplarsVerdict) +
                GetDPS(DamageAbility.Exorcism) +
                GetDPS(DamageAbility.HolyWrath) +
                GetDPS(DamageAbility.Consecration) +
                GetDPS(DamageAbility.HammerOfWrath) +
                GetDPS(DamageAbility.GoaK) +
                calc.OtherDPS;
        }

        #region Ability per second
        private void CalculatePerSeconds()
        {
            foreach (DamageAbility abb in DamageAbilities)
            {
                if (abb != DamageAbility.Inquisition)
                {
                    abilityHitPerSec.Add(abb,
                        Casts[abb].CastsPerSec *
                        skills[abb].CT.ChanceToLand *
                        skills[abb].AvgTargets *
                        skills[abb].TickCount);

                    abilityCritPerSec.Add(abb,
                        Casts[abb].CastsPerSec *
                        skills[abb].CT.ChanceToCrit *
                        skills[abb].AvgTargets *
                        skills[abb].TickCount);
                }
            }
 
        }

        public double SealDotProcPerSec(Skill seal)
        {
            if (seal.GetType() == typeof(SealOfTruth))
                return 1 / (3d / (1 + Stats.PhysicalHaste));
            return 0d;
        }

        public double GetAbilityHitsPerSecond(DamageAbility skill)
        {
            return abilityHitPerSec[skill];
        }

        public double GetAbilityCritsPerSecond(DamageAbility skill)
        {
            return abilityCritPerSec[skill];
        }

        private double _meleeAttacksPerSec = 0f;
        public double MeleeAttacksPerSec
        {
            get
            {
                if (_meleeAttacksPerSec == 0f)
                    _meleeAttacksPerSec = GetAbilityHitsPerSecond(DamageAbility.CrusaderStrike) +
                                          GetAbilityHitsPerSecond(DamageAbility.White) +
                                          GetAbilityHitsPerSecond(DamageAbility.TemplarsVerdict);
                return _meleeAttacksPerSec; 
            }
        }

        private double _rangedAttacksPerSec = 0f;
        public double RangedAttacksPerSec
        {
            get
            {
                if (_rangedAttacksPerSec == 0f)
                    _rangedAttacksPerSec = GetAbilityHitsPerSecond(DamageAbility.Judgement) +
                                           GetAbilityHitsPerSecond(DamageAbility.HammerOfWrath);
                return _rangedAttacksPerSec;
            }
        }

        private double _spellAttacksPerSec = 0f;
        public double SpellAttacksPerSec
        {
            get
            {
                if (_spellAttacksPerSec == 0f)
                    _spellAttacksPerSec = GetAbilityHitsPerSecond(DamageAbility.Exorcism) +
                                          GetAbilityHitsPerSecond(DamageAbility.HolyWrath) +
                                          GetAbilityHitsPerSecond(DamageAbility.SealDot);
                return _spellAttacksPerSec;
            }
        }

        public double GetPhysicalAttacksPerSec()
        {
            return
                MeleeAttacksPerSec +
                RangedAttacksPerSec;
        }

        public double GetMeleeCritsPerSec()
        {
            return
                GetAbilityCritsPerSecond(DamageAbility.CrusaderStrike) +
                GetAbilityCritsPerSecond(DamageAbility.White) +
                GetAbilityCritsPerSecond(DamageAbility.TemplarsVerdict);
        }

        public double GetRangeCritsPerSec()
        {
            return
                GetAbilityCritsPerSecond(DamageAbility.Judgement) +
                GetAbilityCritsPerSecond(DamageAbility.HammerOfWrath);
        }

        public double GetSpellCritsPerSec()
        {
            return
                GetAbilityCritsPerSecond(DamageAbility.Exorcism) +
                GetAbilityCritsPerSecond(DamageAbility.HolyWrath) +
                GetAbilityCritsPerSecond(DamageAbility.SealDot);
        }

        public double GetPhysicalCritsPerSec()
        {
            return
                GetMeleeCritsPerSec() +
                GetRangeCritsPerSec();
        }

        public double GetAttacksPerSec()
        {
            return
                MeleeAttacksPerSec +
                RangedAttacksPerSec +
                SpellAttacksPerSec;
        }
        #endregion
    }
}
