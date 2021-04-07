using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Feral
{
    public class FeralRotationList
    {
        public bool Shred;
        public bool Rake;
        public bool SavageRoar;
        public float SavageRoar_CP;
        public bool Rip;
        public float Rip_CP;
        public bool FerociousBite;
        public float FerociousBite_CP;
        public float FerociousBite_Energy;

        public FeralRotationList(bool shred, bool rake, bool sr, float sr_cp, bool rip, float rip_cp, bool fb, float fb_cp, float fb_energy)
        {
            Shred = shred;
            Rake = rake;
            SavageRoar = sr;
            SavageRoar_CP = sr_cp;
            Rip = rip;
            Rip_CP = rip_cp;
            FerociousBite = fb;
            FerociousBite_CP = fb_cp;
            FerociousBite_Energy = fb_energy;
        }

        public FeralRotationList(bool shred, bool rake, bool sr, bool rip, float rip_cp, bool fb, float fb_cp, float fb_energy)
        {
            Shred = shred;
            Rake = rake;
            SavageRoar = sr;
            SavageRoar_CP = 0;
            Rip = rip;
            Rip_CP = rip_cp;
            FerociousBite = fb;
            FerociousBite_CP = fb_cp;
            FerociousBite_Energy = fb_energy;
        }

        public FeralRotationList(bool shred, bool rake, bool sr, bool rip, bool fb, float fb_cp, float fb_energy)
        {
            Shred = shred;
            Rake = rake;
            SavageRoar = sr;
            SavageRoar_CP = 0;
            Rip = rip;
            Rip_CP = 0;
            FerociousBite = fb;
            FerociousBite_CP = fb_cp;
            FerociousBite_Energy = fb_energy;
        }

        public FeralRotationList(bool shred, bool rake, bool sr, bool rip, bool fb )
        {
            Shred = shred;
            Rake = rake;
            SavageRoar = sr;
            SavageRoar_CP = 0;
            Rip = rip;
            Rip_CP = 0;
            FerociousBite = fb;
            FerociousBite_CP = 0;
            FerociousBite_Energy = 0;
        }
    }

    public class FeralRotation
    {
        public const float MIN_GCD_MS = 1.5f;

        private FeralCombatState CombatState = new FeralCombatState();
        private float _percentBehindBoss = 0;
        /// <summary>Length of the fight</summary>
        private float _fightLength = 0;
        /// <summary>Holds the number of attacks needed to reach said CP</summary>
        private float[] _averageNumberAttacksPerCP = new float[5];
        private FeralRotationType _feralRotationType = FeralRotationType.Feral;
        private float _soulOfTheForest = 0;
        private float _CP = 0;
        /// <summary>Total Energy a player is able to use in a fight</summary>
        private float _Energy = 0;

        public Tigers_Fury TigersFury;
        public Berserk Berserk;
        public Incarnation Incarnation;

        public Claw Claw;
        public Shred Shred;
        public Mangle_Cat Mangle;
        public Rake Rake;
        public Savage_Roar SavageRoar;
        public Rip Rip;
        public Ferocious_Bite FerociousBite;
        public Ravage Ravage;
        public Ravage_Proc RavageProc;
        public Pounce Pounce;
        public Swipe_Cat Swipe;
        public Thrash_Cat Thrash;
        public White_Attack Melee;

        public float TrinketDPS = 0;

        /// <summary>
        /// Generate a Feral Rotation if the base Specialization is Feral
        /// </summary>
        /// <param name="CState"></param>
        /// <param name="behindBoss"></param>
        /// <param name="fightLength"></param>
        /// <param name="frl"></param>

        public FeralRotation()
        {
            CombatState = new FeralCombatState();
            _percentBehindBoss = 1f;
            _fightLength = 600f;
            _feralRotationType = FeralRotationType.Feral;

            Shred = new Shred();
            SavageRoar = new Savage_Roar();
            SavageRoar.Initialize(CombatState);
            Rip = new Rip();
            Rip.Initialize(CombatState);

            // Now activate all the rest of the abilities that are used for both Feral and Guardian
            Claw = new Claw();
            Mangle = new Mangle_Cat();
            Rake = new Rake();
            Ravage = new Ravage();
            RavageProc = new Ravage_Proc();
            Pounce = new Pounce();
            FerociousBite = new Ferocious_Bite();
            FerociousBite.Initialize(CombatState);
            Swipe = new Swipe_Cat();
            Thrash = new Thrash_Cat();
            Melee = new White_Attack();

            // Activate the Cooldowns
            TigersFury = new Tigers_Fury();
            Berserk = new Berserk();
            Incarnation = new Incarnation();

            Initialize(CombatState);

            // Based on the current crit, evaluate the average number of attacks needed to reach the requested number of Combo Points
            _averageNumberAttacksPerCP = new float[] { getAverageNumberAttackstoReachCP(1), getAverageNumberAttackstoReachCP(2), getAverageNumberAttackstoReachCP(3), getAverageNumberAttackstoReachCP(4), getAverageNumberAttackstoReachCP(5) };

            Rotation();
        }

        public FeralRotation(FeralCombatState CState, float behindBoss, float fightLength, FeralRotationList frl)
        {
            CombatState = CState;
            _percentBehindBoss = behindBoss;
            _fightLength = fightLength;
            _feralRotationType = FeralRotationType.Feral;

            // First figure out the abilities that are Feral Specialization only and activate those
            Shred = new Shred();
            SavageRoar = new Savage_Roar();
            SavageRoar.Initialize(CombatState, frl.SavageRoar_CP);
            Rip = new Rip();
            Rip.Initialize(CombatState, frl.Rip_CP);

            // Now activate all the rest of the abilities that are used for both Feral and Guardian
            Claw = new Claw();
            Mangle = new Mangle_Cat();
            Rake = new Rake();
            Ravage = new Ravage();
            RavageProc = new Ravage_Proc();
            Pounce = new Pounce();
            FerociousBite = new Ferocious_Bite();
            FerociousBite.Initialize(CombatState, frl.FerociousBite_CP, frl.FerociousBite_Energy);
            Swipe = new Swipe_Cat();
            Thrash = new Thrash_Cat();
            Melee = new White_Attack();

            // Activate the Cooldowns
            TigersFury = new Tigers_Fury();
            Berserk = new Berserk();
            Incarnation = new Incarnation();

            Initialize(CombatState);

            // Based on the current crit, evaluate the average number of attacks needed to reach the requested number of Combo Points
            _averageNumberAttacksPerCP = new float[] { getAverageNumberAttackstoReachCP(1), getAverageNumberAttackstoReachCP(2), getAverageNumberAttackstoReachCP(3), getAverageNumberAttackstoReachCP(4), getAverageNumberAttackstoReachCP(5) };

            Rotation();
        }

        private void Initialize(FeralCombatState CombatState)
        {
            Shred.Initialize(CombatState);

            Claw.Initialize(CombatState);
            Mangle.Initialize(CombatState);
            Rake.Initialize(CombatState);
            Ravage.Initialize(CombatState);
            RavageProc.Initialize(CombatState);
            Pounce.Initialize(CombatState);
            Swipe.Initialize(CombatState);
            Thrash.Initialize(CombatState);
            Melee.Initialize(CombatState);

            TigersFury.Initialize(CombatState);
            Berserk.Initialize(CombatState);
            Incarnation.Initialize(CombatState);
        }

        private void AverageIncarnation(float uptime)
        {
            // First figure out the abilities that are Feral Specialization only and average those
            Shred.AverageIncarnationCount(uptime);
            SavageRoar.AverageIncarnationCount(uptime);
            Rip.AverageIncarnationCount(uptime);

            // Now average all the rest of the abilities that are used for both Feral and Guardian
            Claw.AverageIncarnationCount(uptime);
            Mangle.AverageIncarnationCount(uptime);
            Rake.AverageIncarnationCount(uptime);
            Ravage.AverageIncarnationCount(uptime);
            RavageProc.AverageIncarnationCount(uptime);
            Pounce.AverageIncarnationCount(uptime);
            FerociousBite.AverageIncarnationCount(uptime);
            Swipe.AverageIncarnationCount(uptime);
            Thrash.AverageIncarnationCount(uptime);
            //Melee.AverageIncarnationCount(uptime);
        }

        public void updateCombatState(FeralCombatState cState)
        {
            // First figure out the abilities that are Feral Specialization only and update those
            Shred.UpdateCombatState(cState);
            SavageRoar.UpdateCombatState(cState);
            Rip.UpdateCombatState(cState);

            // Now update all the rest of the abilities that are used for both Feral and Guardian
            Claw.UpdateCombatState(cState);
            Mangle.UpdateCombatState(cState);
            Rake.UpdateCombatState(CombatState);
            Ravage.UpdateCombatState(cState);
            RavageProc.UpdateCombatState(cState);
            Pounce.UpdateCombatState(cState);
            FerociousBite.UpdateCombatState(cState);
            Swipe.UpdateCombatState(cState);
            Thrash.UpdateCombatState(cState);
            Melee.UpdateCombatState(cState);

            // Update the Cooldowns
            TigersFury.UpdateCombatState(cState);
            Berserk.UpdateCombatState(cState);
            Incarnation.UpdateCombatState(cState);
        }

        /// <summary>
        /// Figure out the total amount of energy a Feral will have for the entire fight
        /// </summary>
        /// <returns></returns>
        private float getTotalEnergy(bool Incarnation)
        {
            // Start out with a full energy bar
            float energy = 100;
            // Figure out Energy Regen
            float EnergyRegen = 10 + (10 * (CombatState.MainHand.Haste));
            // Add the Energy Regen to the total amount
            energy += _fightLength * EnergyRegen;

            // Figure out Clearcasting amounts
            energy += ((Incarnation ? Ravage.Energy : Shred.Energy)) * Melee.feralAbility.HitChance;

            // Figure out amount Tigers Fury gives us.
            energy += (TigersFury.Count * TigersFury.Energy);

            // Berserk is built into each of the abilities

            return energy;
        }

        /// <summary>
        /// Get the Average Number of Shreds, Mangles, Rake, and/or Swipe to Reach the number of CP requested
        /// </summary>
        /// <param name="cp">Number of Combo Points that needs to be reached</param>
        /// <returns>Average number of attacks to reach the CP requested.</returns>
        private float getAverageNumberAttackstoReachCP(int cp)
        {
            float hit = 1 - CombatState.MainHand.CriticalStrike;
            float crit = CombatState.MainHand.CriticalStrike;
            float bonusCritCP = 1; // In cata; CombatState.Talents.PrimalFury gave 0/50%/100% chance to add a second CP on a Crit; for testing reasons defaulting to 100%.
            switch (cp)
            {
                case 1:
                    return 1;
                case 2:
                    {
                        // Get the different combinations to reach 2 CP
                        float c = crit * bonusCritCP; // 1 Crit
                        float hh = (float)Math.Pow(hit, 2); // 2 Hits
                        float sum = c + hh;
                        // get the percent of set in relation to sum and multiply by the number of attacks
                        c = (c / sum) * 1;
                        hh = (hh / sum) * 2;
                        return c + hh;
                    }
                case 3:
                    {
                        // Get the different combinations to reach 3 CP
                        float ch = ((crit * bonusCritCP) * hit) * 2; // 1 Crit 1 Hit; 1 Hit 1 Crit
                        float hhh = (float)Math.Pow(hit, 3); // 3 Hits
                        float sum = ch + hhh;
                        // get the percent of set in relation to sum and multiply by the number of attacks
                        ch = (ch / sum) * 2;
                        hhh = (hhh / sum) * 3;
                        return ch + hhh;
                    }
                case 4:
                    {
                        // Get the different combinations to reach 4 CP
                        float cc = (float)Math.Pow((crit * bonusCritCP), 2); // 2 Crit
                        float chh = ((crit * bonusCritCP) * (float)Math.Pow(hit, 2)) * 2; // 1 Crit 2 Hit; 2 Hit 1 Crit
                        float hch = (hit * (crit * bonusCritCP)) * hit; // 1 Hit 1 Crit 1 Hit
                        float hhhh = (float)Math.Pow(hit, 4); // 4 Hits
                        float sum = cc + chh + hch + hhhh;
                        // get the percent of set in relation to sum and multiply by the number of attacks
                        cc = (cc / sum) * 2;
                        chh = (chh / sum) * 3;
                        hch = (hch / sum) * 3;
                        hhhh = (hhhh / sum) * 4;
                        return cc + chh + hch + hhhh;
                    }
                case 5:
                    {
                        // Get the different combinations to reach 5 CP
                        float ccc = (float)Math.Pow((crit * bonusCritCP), 3); // 3 Crit
                        float cch = ((float)Math.Pow((crit * bonusCritCP), 2) * hit) * 3; // 2 Crit 1 Hit; 1 Hit 2 Crit; 1 Crit 1 Hit 1 Crit
                        float chhh = ((crit * bonusCritCP) * (float)Math.Pow(hit, 3)) * 4; // 1 Hit 1 Crit 1 Hit
                        float hhhhh = (float)Math.Pow(hit, 5); // 5 Hits
                        float sum = ccc + cch + chhh + hhhhh;
                        // get the percent of set in relation to sum and multiply by the number of attacks
                        ccc = (ccc / sum) * 3;
                        cch = (cch / sum) * 3;
                        chhh = (chhh / sum) * 4;
                        hhhhh = (hhhhh / sum) * 5;
                        return ccc + cch + chhh + hhhhh;
                    }
            }
            return 0;
        }

        private void Rotation()
        {
            float SavageRoarTime = 0f;
            float BloodInTheWater = (CombatState.MainHand.Stats.Tier_13_2_piece ? CombatState.Below60Percent : CombatState.Below25Percent);
            _soulOfTheForest = (CombatState.Talents.SoulOfTheForest ? 4 : 0);

            #region Base Uptime Setting
            // adjust the fight length by 4.5 seconds to adjust for the first three attacks of the fight before using Berserk and/or TF
            // Get information about Berserk
            Berserk.Count = (float)Math.Floor((_fightLength - 4.5f - Berserk.Duration) / Berserk.Cooldown);
            CombatState.BerserkUptime = (Berserk.Count * Berserk.Duration) / (_fightLength - 4.5f);

            // Get information about Tigers Fury
            TigersFury.Count = (float)Math.Floor(((_fightLength - 4.5f - TigersFury.Duration) - (Berserk.Count * Berserk.Duration)) / TigersFury.Cooldown);
            CombatState.TigersFuryUptime = (TigersFury.Count * TigersFury.Duration) / (_fightLength - 4.5f);

            // Get information about Incarnation
            if (CombatState.Talents.Incarnation)
                Incarnation.Count = (float)Math.Floor((_fightLength - 4.5f - Incarnation.Duration) / Incarnation.Cooldown);
            else
                Incarnation.Count = 0;
            CombatState.IncarnationUptime = (Incarnation.Count * Incarnation.Duration) / (_fightLength - 4.5f);

            #endregion

            // Figure out Melee Auto Attack Count since this will be passive
            if (CombatState.MainHand.hastedSpeed > 0)
                Melee.Count = _fightLength / CombatState.MainHand.hastedSpeed;
            else
                Melee.Count = _fightLength;

            if (CombatState.Talents.Incarnation)
            {
                withIncarnation(SavageRoarTime, BloodInTheWater);
            }

            withoutIncarnation(SavageRoarTime, BloodInTheWater);

            this.AverageIncarnation(CombatState.IncarnationUptime);

            this.updateCombatState(CombatState);
        }

        private void withoutIncarnation(float SavageRoarTime, float BloodInTheWater)
        {
            // Figure out the total amount of energy that will be used in the rotation.
            _Energy = getTotalEnergy(false);

            #region Rake
            float openingTimeLength = 0;
            Rake.nonIncarnationCount = (_fightLength / Rake.Duration);
            openingTimeLength += 1.5f;
            _Energy -= Rake.Count * Rake.Energy;
            CombatState.NumberOfBleeds += 1f;
            #endregion

            #region Savage Roar
            if (_averageNumberAttacksPerCP[1] < 1.5f)
            {
                SavageRoarTime = AbilityFeral_SavageRoar.getSRLength(2);
                _CP += 2;
                _Energy += _soulOfTheForest * 2f;
            }
            else
            {
                SavageRoarTime = AbilityFeral_SavageRoar.getSRLength(1);
                _CP += 1;
                _Energy += _soulOfTheForest;
            }
            openingTimeLength += 1.5f;

            ////////////////////////////////////////////////////////////
            // TODO: If less than 5 CP used for SR, evaluate CP and CP+1
            ////////////////////////////////////////////////////////////
            // Get the number of Savage Roars in a given fight
            SavageRoar.nonIncarnationCount += (float)Math.Floor(((_fightLength - openingTimeLength) - SavageRoarTime) / AbilityFeral_SavageRoar.getSRLength(SavageRoar.FormulaComboPoint));
            SavageRoarTime += SavageRoar.nonIncarnationCount * SavageRoar.Duration;
            // Add in the Savage Roar from the Opening Time Length
            SavageRoar.nonIncarnationCount += 1;
            CombatState.SavageRoarUptime = SavageRoarTime / _fightLength;
            _CP -= (SavageRoar.nonIncarnationCount - 1) * SavageRoar.FormulaComboPoint;
            // Based on the number of Savage Roars remove from the 
            _Energy -= SavageRoar.nonIncarnationCount * SavageRoar.Energy;
            _Energy += (SavageRoar.nonIncarnationCount - 1) * _soulOfTheForest;
            #endregion

            #region Rip
            float sCount = _averageNumberAttacksPerCP[4];
            openingTimeLength += (Shred.Energy / (10 + (10 * CombatState.MainHand.Haste))) * sCount;
            _CP += 5f;

            // Get the number of rips in a fight
            Rip.nonIncarnationCount += (_fightLength - openingTimeLength) / Rip.Duration;
            // Set the number of Rips used; separating Blood in the Water and the forced count.
            Rip.nonIncarnationBitWCount = (Rip.nonIncarnationCount * BloodInTheWater);
            Rip.nonIncarnationCount -= Rip.nonIncarnationBitWCount;

            _Energy -= Rip.nonIncarnationCount * Rip.Energy;
            _Energy += Rip.nonIncarnationCount * _soulOfTheForest;
            _CP -= (Rip.nonIncarnationCount * Rip.FormulaComboPoint);

            // Ferocious Bite to activate Blood In The Water
            FerociousBite.nonIncarnationCount += Rip.nonIncarnationBitWCount;
            _Energy -= (FerociousBite.nonIncarnationCount * FerociousBite.Energy);
            _Energy += (FerociousBite.nonIncarnationCount * _soulOfTheForest);
            _CP -= (FerociousBite.nonIncarnationCount * FerociousBite.FormulaComboPoint);

            // Add a second dot to the Combat State
            CombatState.NumberOfBleeds++;
            #endregion

            #region Catch up on CP generation
            // Get CP back to Zero
            float MaxCP = (float)Math.Floor(-_CP / 5f);
            float ModCP = (float)Math.Floor(-_CP % 5f);
            // Use Shred Count as a temporary placing holder for all further CP gaining abilities
            // CP gaining Abilities will take out the number of times they are performed from Shred.
            sCount += MaxCP * _averageNumberAttacksPerCP[4];
            _CP += MaxCP * 5f;
            if (ModCP > 0)
            {
                sCount += _averageNumberAttacksPerCP[((int)ModCP - 1)];
                _CP += ModCP;
            }
            #endregion

            #region Ravage Proc
            RavageProc.nonIncarnationCount = (CombatState.MainHand.Stats.Tier_13_4_piece ? TigersFury.Count : 0f);
            // Ravage Proc is free so no need to manipulate the energy
            // Remove the number of Ravage Procs from Shred Count
            if (sCount > RavageProc.nonIncarnationCount)
                sCount -= RavageProc.nonIncarnationCount;
            _Energy -= RavageProc.nonIncarnationCount * (CombatState.MainHand.Stats.Tier_13_4_piece ? 0 : RavageProc.Energy);
            #endregion

            #region Rake Part 2
            // Remove the number of Rakes used from Shred's count
            if (sCount > Rake.nonIncarnationCount)
                sCount -= Rake.nonIncarnationCount;
            #endregion

            // I'm back to zero Combo Points at this point.
            #region Thrash
            float cCCount = (Melee.Count * AbilityFeral_WhiteSwing.Clearcasting);
            if (cCCount > ((_fightLength - openingTimeLength) / Thrash.Duration))
            {
                Thrash.nonIncarnationCount = ((_fightLength - openingTimeLength) / Thrash.Duration) / 3;
                sCount += cCCount - ((_fightLength - openingTimeLength) / Thrash.Duration) / 3;
            }
            else
            {
                Thrash.nonIncarnationCount = cCCount / 3;
                sCount += cCCount * 2 / 3;
            }
            #endregion

            // Now I figure out Shred Count + Ferocious Bite.
            #region Shred + FB
            Shred.nonIncarnationCount = sCount;
            _Energy -= sCount * Shred.Energy;
            sCount = 0;
            // first get the total number of Shred + Ferocious Bite combos left in Energy
            float ShredFBCombo = (float)Math.Floor(_Energy / ((FerociousBite.Energy) +
                                (_averageNumberAttacksPerCP[(int)(FerociousBite.FormulaComboPoint - 1)] * Shred.Energy) - (FerociousBite.FormulaComboPoint * _soulOfTheForest)));
            // Add Combo to FB's Count
            FerociousBite.nonIncarnationCount += ShredFBCombo;
            _Energy -= (FerociousBite.Energy) * ShredFBCombo;

            // Since Shred may have residual amounts lets add a temp file just in case and use that in Shred's place
            sCount = _averageNumberAttacksPerCP[(int)(FerociousBite.FormulaComboPoint - 1)] * ShredFBCombo;
            Shred.nonIncarnationCount += sCount;
            _Energy -= sCount * Shred.Energy;

            // Now that we are near the end of the amount of Energy we can use lets finish with only shredding the rest
            Shred.nonIncarnationCount += _Energy / Shred.Energy;
            #endregion

            #region Mangle
            if (CombatState.MainHand.Stats.Tier_11_4pc)
            {
                float baseMangleBuildUp = (Mangle.Energy / (10 + (10 * CombatState.MainHand.Haste))) * 3;
                Mangle.nonIncarnationCount += (_fightLength - 3f - baseMangleBuildUp) / 27f + 3f;
                //CombatState.MainHand.Stats.AttackPower *= 1.03f;
                Shred.nonIncarnationCount -= Mangle.Count;
            }
            Mangle.nonIncarnationCount += Shred.nonIncarnationCount * (Shred.Energy / Mangle.Energy) * (1 - _percentBehindBoss);
            Shred.nonIncarnationCount *= _percentBehindBoss;
            #endregion
        }

        private void withIncarnation(float SavageRoarTime, float BloodInTheWater)
        {
            // Figure out the total amount of energy that will be used in the rotation.
            _Energy = getTotalEnergy(true);

            #region Rake
            float openingTimeLength = 0;
            Rake.IncarnationCount = (_fightLength / Rake.Duration);
            openingTimeLength += 1.5f;
            _Energy -= Rake.IncarnationCount * Rake.Energy;
            CombatState.NumberOfBleeds += 1f;
            #endregion

            #region Savage Roar
            if (_averageNumberAttacksPerCP[1] < 1.5f)
            {
                SavageRoarTime = AbilityFeral_SavageRoar.getSRLength(2);
                _CP += 2;
            }
            else
            {
                SavageRoarTime = AbilityFeral_SavageRoar.getSRLength(1);
                _CP += 1;
            }
            openingTimeLength += 1.5f;

            ////////////////////////////////////////////////////////////
            // TODO: If less than 5 CP used for SR, evaluate CP and CP+1
            ////////////////////////////////////////////////////////////
            // Get the number of Savage Roars in a given fight
            SavageRoar.IncarnationCount += (float)Math.Floor(((_fightLength - openingTimeLength) - SavageRoarTime) / AbilityFeral_SavageRoar.getSRLength(SavageRoar.FormulaComboPoint));
            SavageRoarTime += SavageRoar.Count * SavageRoar.Duration;
            // Add in the Savage Roar from the Opening Time Length
            SavageRoar.IncarnationCount += 1;
            CombatState.SavageRoarUptime = SavageRoarTime / _fightLength;
            _CP -= (SavageRoar.IncarnationCount - 1) * SavageRoar.FormulaComboPoint;
            // Based on the number of Savage Roars remove from the 
            _Energy -= SavageRoar.IncarnationCount * SavageRoar.Energy;
            #endregion

            #region Rip
            float sCount = _averageNumberAttacksPerCP[4];
            openingTimeLength += (Shred.Energy / (10 + (10 * CombatState.MainHand.Haste))) * sCount;
            _CP += 5f;

            // Get the number of rips in a fight
            Rip.IncarnationCount += (_fightLength - openingTimeLength) / Rip.Duration;
            // Set the number of Rips used; separating Blood in the Water and the forced count.
            Rip.IncarnationBitWCount = (Rip.IncarnationCount * BloodInTheWater);
            Rip.IncarnationCount -= Rip.IncarnationBitWCount;

            _Energy -= Rip.IncarnationCount * Rip.Energy;
            _CP -= (Rip.IncarnationCount * Rip.FormulaComboPoint);

            // Ferocious Bite to activate Blood In The Water
            FerociousBite.IncarnationCount += Rip.IncarnationBitWCount;
            _Energy -= (FerociousBite.IncarnationCount * (FerociousBite.Energy));
            _CP -= (FerociousBite.IncarnationCount * FerociousBite.FormulaComboPoint);

            // Add a second dot to the Combat State
            CombatState.NumberOfBleeds++;
            #endregion

            #region Catch up on CP generation
            // Get CP back to Zero
            float MaxCP = (float)Math.Floor(-_CP / 5f);
            float ModCP = (float)Math.Floor(-_CP % 5f);
            // Use Shred Count as a temporary placing holder for all further CP gaining abilities
            // CP gaining Abilities will take out the number of times they are performed from Shred.
            sCount += MaxCP * _averageNumberAttacksPerCP[4];
            _CP += MaxCP * 5f;
            if (ModCP > 0)
            {
                sCount += _averageNumberAttacksPerCP[((int)ModCP - 1)];
                _CP += ModCP;
            }
            #endregion

            #region Ravage Proc
            RavageProc.IncarnationCount = (CombatState.MainHand.Stats.Tier_13_4_piece ? TigersFury.Count : 0f);
            // Ravage Proc is free so no need to manipulate the energy
            // Remove the number of Ravage Procs from Shred Count
            if (sCount > RavageProc.IncarnationCount)
                sCount -= RavageProc.IncarnationCount;
            _Energy -= RavageProc.IncarnationCount * (CombatState.MainHand.Stats.Tier_13_4_piece ? 0 : RavageProc.Energy);
            #endregion

            #region Rake Part 2
            // Remove the number of Rakes used from Shred's count
            if (sCount > Rake.IncarnationCount)
                sCount -= Rake.IncarnationCount;
            #endregion

            #region Pounce
            Pounce.IncarnationCount = ((_fightLength - openingTimeLength) / Pounce.Duration);
            _Energy -= Pounce.IncarnationCount * Pounce.Energy;
            if (sCount > Pounce.IncarnationCount)
                sCount -= Pounce.IncarnationCount;
            CombatState.NumberOfBleeds += 1f;
            #endregion

            // I'm back to zero Combo Points at this point.
            #region Thrash
            float cCCount = (Melee.Count * AbilityFeral_WhiteSwing.Clearcasting);
            if (cCCount > ((_fightLength - openingTimeLength) / Thrash.Duration))
            {
                Thrash.IncarnationCount = ((_fightLength - openingTimeLength) / Thrash.Duration) / 3;
                sCount += cCCount - ((_fightLength - openingTimeLength) / Thrash.Duration) / 3;
            }
            else
            {
                Thrash.IncarnationCount = cCCount / 3;
                sCount += cCCount * 2 / 3;
            }
            #endregion

            // Now I figure out Ravage Count + Ferocious Bite.
            #region Ravage + FB
            Ravage.IncarnationCount = sCount;
            _Energy -= sCount * Ravage.Energy;
            sCount = 0;
            // first get the total number of Shred + Ferocious Bite combos left in Energy
            float RavageFBCombo = (float)Math.Floor(_Energy / ((FerociousBite.Energy) +
                                (_averageNumberAttacksPerCP[(int)(FerociousBite.FormulaComboPoint - 1)] * Ravage.Energy)));
            // Add Combo to FB's Count
            FerociousBite.IncarnationCount += RavageFBCombo;
            _Energy -= (FerociousBite.Energy) * RavageFBCombo;

            // Since Ravage may have residual amounts lets add a temp file just in case and use that in Ravage's place
            sCount = _averageNumberAttacksPerCP[(int)(FerociousBite.FormulaComboPoint - 1)] * RavageFBCombo;
            Ravage.IncarnationCount += sCount;
            _Energy -= sCount * Ravage.Energy;

            // Now that we are near the end of the amount of Energy we can use lets finish with only shredding the rest
            Ravage.IncarnationCount += _Energy / Ravage.Energy;
            #endregion

            #region Mangle
            if (CombatState.MainHand.Stats.Tier_11_4pc)
            {
                float baseMangleBuildUp = (Mangle.Energy / (10 + (10 * CombatState.MainHand.Haste))) * 3;
                Mangle.IncarnationCount += (_fightLength - 3f - baseMangleBuildUp) / 27f + 3f;
                Ravage.IncarnationCount -= Mangle.Count;
            }
            #endregion
        }

        #region Damage
        public float totalDamageDone
        {
            get
            {
                float damageDone = 0;

                if (Shred.Count > 0)
                    damageDone += Shred.DamageDone();
                if (Mangle.Count > 0)
                    damageDone += Mangle.DamageDone();
                if (Rake.Count > 0)
                    damageDone += Rake.DamageDone();
                if (Rip.Count > 0)
                    damageDone += Rip.DamageDone();
                if (FerociousBite.Count > 0)
                    damageDone += FerociousBite.DamageDone();
                if (Ravage.Count > 0)
                    damageDone += Ravage.DamageDone();
                if (RavageProc.Count > 0)
                    damageDone += RavageProc.DamageDone();
                if (Pounce.Count > 0)
                    damageDone += Pounce.DamageDone();
                if (Swipe.Count > 0)
                    damageDone += Swipe.DamageDone();
                if (Thrash.Count > 0)
                {
                    damageDone += Thrash.DamageDone();
                }
                damageDone += Melee.DamageDone();

                if (TrinketDPS > 0)
                    damageDone += (TrinketDPS * _fightLength);

                return damageDone;
            }
        }

        public float totalDPS()
        {
            return this.totalDamageDone / _fightLength;
        }

        public float totalDPS(float damage)
        {
            return (this.totalDamageDone + damage) / _fightLength;
        }

        public string optimalRotation()
        {
            string temp = "*";
            float cp = 0;

            if (SavageRoar.Count > 0)
            {
                cp = SavageRoar.FormulaComboPoint;
                temp += string.Format("{0}: {1} Combo Point{2} every {3} seconds\n", SavageRoar.Name, cp.ToString("n0"), ((cp > 1) ? "s" : ""), AbilityFeral_SavageRoar.getSRLength(cp).ToString("n0"));
            }
            if (CombatState.MainHand.Stats.Tier_11_4pc)
            {
                temp += string.Format("{0}: Combo Point generator used every 27 - 30 seconds\n   to keep up the Tier 11 4-piece Strength of the Panther buff active{1}", Mangle.Name, (Shred.Count > 0 ? "\n" : ""));
            }
            if (Rip.Count > 0)
            {
                cp = Rip.FormulaComboPoint;
                temp += string.Format("{0}: {1} Combo Point{2} every {3} seconds\n   Ferocious Bite to refresh below {4}\n", Rip.Name, cp.ToString("n0"), ((cp > 1) ? "s" : ""), 
                    Rip.Duration, (CombatState.MainHand.Stats.Tier_13_2_piece ? .60f : .25f).ToString("p0"));
            }
            if (Pounce.Count > 0)
            {
                temp += string.Format("{0}: Every {1} seconds while Incarnation is active\n", Pounce.Name, Pounce.Duration.ToString("n0"));
            }
            if (Rake.Count > 0)
            {
                temp += string.Format("{0}: Every {1} seconds\n", Rake.Name, Rake.Duration.ToString("n0"));
            }
            if (RavageProc.Count > 0)
            {
                temp += string.Format("{0}: Every {1} seconds\n", RavageProc.Name, TigersFury.Cooldown.ToString("n0"));
            }
            if (FerociousBite.Count > 0)
            {
                cp = FerociousBite.FormulaComboPoint;
                float fbEnergy = FerociousBite.FormulaEnergy;

                temp += string.Format("{0}: {1} Combo Point{2}{3}\n", FerociousBite.Name, cp.ToString("n0"), ((cp > 1) ? "s" : ""),
                    (fbEnergy > 0 ? (" plus " + fbEnergy + " Energy\n   that increases damage by " + (fbEnergy / FerociousBite.feralAbility.maxFormulaEnergy).ToString("p0")) : ""));
            }
            if (Ravage.Count > 0)
            {
                temp += string.Format("{0}: Combo Point generator while Incarnation is active\n", Ravage.Name);
            }
            if ((Mangle.Count > 0) && (_percentBehindBoss < 1))
            {
                temp += string.Format("{0}: Combo Point generator while in front of the mob{1}", Mangle.Name, (Shred.Count > 0 ? "\n" : ""));
            }
            if (Shred.Count > 0)
            {
                temp += string.Format("{0}: Combo Point generator while behind of the mob", Shred.Name);
            }
            return temp;
        }
        #endregion

        public string byAbility()
        {
            string temp = "*";
            float total = this.totalDamageDone;
            List<stringAbility> list = new List<stringAbility>();
            float ripPercent, rakePercent, ravagePercent, ravageProcPercent, pouncePercent, fbPercent, manglePercent, shredPercent, whitePercent, swipePercent, thrashPercent, trinketPercent = 0;
            if (Rip.Count > 0) {
                ripPercent = Rip.DamageDone() / total;
                list.Add(new stringAbility() {
                    percent = ripPercent,
                    description = Rip.byAbility(Rip.Count, Rip.BloodInTheWaterCount, ripPercent, total, Rip.DamageDone())
                });
            }
            if (Rake.Count > 0) {
                rakePercent = Rake.DamageDone() / total;
                list.Add(new stringAbility()
                {
                    percent = rakePercent,
                    description = Rake.byAbility(Rake.Count, rakePercent, total, Rake.DamageDone())
                });
            }
            if (Ravage.Count > 0 ) {
                ravagePercent = Ravage.DamageDone() / total;
                list.Add(new stringAbility()
                {
                    percent = ravagePercent,
                    description = Ravage.byAbility(Ravage.Count, ravagePercent, total, Ravage.DamageDone())
                });
            }
            if (RavageProc.Count > 0) {
                ravageProcPercent = RavageProc.DamageDone() / total;
                list.Add(new stringAbility()
                {
                    percent = ravageProcPercent,
                    description = RavageProc.byAbility(RavageProc.Count, ravageProcPercent, total, RavageProc.DamageDone())
                });
            }
            if (Pounce.Count > 0)
            {
                pouncePercent = Pounce.DamageDone() / total;
                list.Add(new stringAbility()
                {
                    percent = pouncePercent,
                    description = Pounce.byAbility(RavageProc.Count, pouncePercent, total, RavageProc.DamageDone())
                });
            }
            if (FerociousBite.Count > 0)
            {
                fbPercent = FerociousBite.DamageDone() / total;
                list.Add(new stringAbility()
                {
                    percent = fbPercent,
                    description = FerociousBite.byAbility(FerociousBite.Count, fbPercent, total, FerociousBite.DamageDone())
                });
            }
            if (Mangle.Count > 0) {
                manglePercent = Mangle.DamageDone() / total;
                list.Add(new stringAbility()
                {
                    percent = manglePercent,
                    description = Mangle.byAbility(Mangle.Count, manglePercent, total, Mangle.DamageDone())
                });
            }
            if (Shred.Count > 0) {
                shredPercent = Shred.DamageDone() / total;
                list.Add(new stringAbility()
                {
                    percent = shredPercent,
                    description = Shred.byAbility(Shred.Count, shredPercent, total, Shred.DamageDone())
                });
            }
            if (Swipe.Count > 0)
            {
                swipePercent = Swipe.DamageDone() / total;
                list.Add(new stringAbility()
                {
                    percent = swipePercent,
                    description = Swipe.byAbility(Swipe.Count, swipePercent, total, Swipe.DamageDone())
                });
            }
            if (Thrash.Count > 0)
            {
                thrashPercent = Thrash.DamageDone() / total;
                list.Add(new stringAbility()
                {
                    percent = thrashPercent,
                    description = Thrash.byAbility(Thrash.Count, thrashPercent, total, Thrash.DamageDone())
                });
            }
            whitePercent = Melee.DamageDone() / total;
            list.Add(new stringAbility()
            {
                percent = whitePercent,
                description = Melee.byAbility(Melee.Count, whitePercent, total, Melee.DamageDone())
            });
            if (TrinketDPS > 0)
            {
                trinketPercent = (TrinketDPS * _fightLength) / total;
                list.Add(new stringAbility()
                {
                    percent = trinketPercent,
                    description = string.Format("Proc Damage: {0}, {1}", trinketPercent.ToString("p"), (TrinketDPS * _fightLength).ToString("n0"))
                });
            }
            List<stringAbility> newList = list.OrderByDescending(x => x.percent).ToList();
            foreach (stringAbility sa in newList)
            {
                temp += sa.description + "\n";
            }
            return temp;
        }

        public float Duration
        {
            get
            {
                return _fightLength;
            }
        }

        #region Counts and Intervals
        private float AbilityCount
        {
            get
            {
                return Claw.Count + Shred.Count + Mangle.Count + Rake.Count + Rip.Count + FerociousBite.Count + Ravage.Count + RavageProc.Count 
                            + Pounce.Count + Swipe.Count + Thrash.Count/* + Melee.Count*/;
            }
        }

        private float AbilityCritCount
        {
            get
            {
                float clawCount = Claw.Count * Claw.CritChance;
                float shredCount = Shred.Count * Shred.CritChance;
                float mangleCount = Mangle.Count * Mangle.CritChance;
                float rakeCount = Rake.Count * Rake.CritChance;
                float ripCount = Rip.Count * Rip.CritChance;
                float fbCount = FerociousBite.Count * FerociousBite.CritChance;
                float ravageCount = Ravage.Count * Ravage.CritChance;
                float ravageProcCount = RavageProc.Count * RavageProc.CritChance;
                float pounceCount = Pounce.Count * Pounce.CritChance;
                float swipeCount = Swipe.Count * Swipe.CritChance;
                float thrashCount = Thrash.Count * Thrash.CritChance;

                return clawCount + shredCount + mangleCount + rakeCount + ripCount + fbCount + ravageCount + ravageProcCount + pounceCount + swipeCount + thrashCount;
            }
        }

        private float MangleAbilityCount
        {
            get
            {
                return Mangle.Count;
            }
        }

        public float MangleAbilityInterval
        {
            get
            {
                return Math.Max(1, Duration / MangleAbilityCount);
            }
        }

        private float MangleShredAbilityCount
        {
            get
            {
                return Mangle.Count + Shred.Count;
            }
        }

        public float MangleShredAbilityInterval
        {
            get
            {
                return Math.Max(1, Duration / MangleShredAbilityCount);
            }
        }

        public float HitInterval
        {
            get
            {
                return Math.Max(1, Duration / AbilityCount);
            }
        }

        public float HitChance
        {
            get
            {
                return Shred.feralAbility.HitChance;
            }
        }

        public float CritChance
        {
            get
            {
                return (Claw.CritChance + Shred.CritChance + Mangle.CritChance + Rake.CritChance + FerociousBite.CritChance + 
                    Ravage.CritChance + RavageProc.CritChance + Swipe.CritChance + Pounce.CritChance + Thrash.CritChance) / 11f;
            }
        }

        public float CritInterval
        {
            get
            {
                return Math.Max(1, Duration / AbilityCritCount);
            }
        }

        public float DotTickInterval
        {
            get
            {
                float ripTick = Rip.BaseTickCount * (Rip.Count + Rip.BloodInTheWaterCount);
                float rakeTick = Rake.BaseTickCount * Rake.Count;
                float thrashTick = Thrash.BaseTickCount * Thrash.Count;
                float pounceTick = Pounce.BaseTickCount * Pounce.Count;

                return _fightLength / (ripTick + rakeTick + thrashTick + pounceTick);
            }
        }
        #endregion
    }

    public class stringAbility
    {
        public float percent = 0;
        public string description = string.Empty;
    }
}
