using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Bosses
{
    #region World Bosses
    public class ShaofAnger : MultiDiffBoss
    {
        public ShaofAnger()
        {
            #region Info
            Name = "Sha of Anger";
            Instance = "World Boss";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_LFR, BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H };
            #endregion
            #region Basics
            Health = new float[] { 264000000f, 0, 0, 0, 0 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 0, 0, 0, 0 };
            SpeedKillTimer = new int[] { 7 * 60, 0, 0, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0, 0, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 25, 0, 0, 0, 0 };
            Min_Tanks = new int[] { 2, 0, 0, 0, 0 };
            Min_Healers = new int[] { 6, 0, 0, 0, 0 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class SalyissWarband : MultiDiffBoss
    {
        public SalyissWarband()
        {
            #region Info
            Name = "Salyis's Warband";
            Instance = "World Boss";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_LFR, BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H };
            #endregion
            #region Basics
            Health = new float[] { 264000000f, 0, 0, 0, 0 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 0, 0, 0, 0 };
            SpeedKillTimer = new int[] { 7 * 60, 0, 0, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0, 0, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 25, 0, 0, 0, 0 };
            Min_Tanks = new int[] { 2, 0, 0, 0, 0 };
            Min_Healers = new int[] { 6, 0, 0, 0, 0 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }
    #endregion

    #region Mogu'shan Vaults
    // ===== Mogu'shan Vaults ======================
    public class TheStoneGuard : MultiDiffBoss
    {
        public TheStoneGuard()
        {
            #region Info
            Name = "The Stone Guard";
            Instance = "Mogu'shan Vaults";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class FengtheAccursed : MultiDiffBoss
    {
        public FengtheAccursed()
        {
            #region Info
            Name = "Feng the Accursed";
            Instance = "Mogu'shan Vaults";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class GarajaltheSpiritbinder : MultiDiffBoss
    {
        public GarajaltheSpiritbinder()
        {
            #region Info
            Name = "Gara'jal the Spiritbinder";
            Instance = "Mogu'shan Vaults";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class TheSpiritKings : MultiDiffBoss
    {
        public TheSpiritKings()
        {
            #region Info
            Name = "The Spirit Kings";
            Instance = "Mogu'shan Vaults";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class Elegon : MultiDiffBoss
    {
        public Elegon()
        {
            #region Info
            Name = "Elegon";
            Instance = "Mogu'shan Vaults";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class WillOfTheEmperor : MultiDiffBoss
    {
        public WillOfTheEmperor()
        {
            #region Info
            Name = "Will Of The Emperor";
            Instance = "Mogu'shan Vaults";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }
    #endregion

    #region Heart of Fear
    public class ImperialVizierZorlok : MultiDiffBoss
    {
        public ImperialVizierZorlok()
        {
            #region Info
            Name = "Imperial Vizier Zor'lok";
            Instance = "Heart of Fear";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class BladeLordTayak : MultiDiffBoss
    {
        public BladeLordTayak()
        {
            #region Info
            Name = "Blade Lord Ta'yak";
            Instance = "Heart of Fear";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class Garalon : MultiDiffBoss
    {
        public Garalon()
        {
            #region Info
            Name = "Garalon";
            Instance = "Heart of Fear";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class WindLordMeljarak : MultiDiffBoss
    {
        public WindLordMeljarak()
        {
            #region Info
            Name = "Wind Lord Mel'jarak";
            Instance = "Heart of Fear";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class AmberShaperUnsok : MultiDiffBoss
    {
        public AmberShaperUnsok()
        {
            #region Info
            Name = "Amber-Shaper Un'sok";
            Instance = "Heart of Fear";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class GrandEmpressShekzeer : MultiDiffBoss
    {
        public GrandEmpressShekzeer()
        {
            #region Info
            Name = "Grand Empress Shek'zeer";
            Instance = "Heart of Fear";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }
    #endregion

    #region Terrace of Endless Spring
    public class ProtectorsOfTheEndless : MultiDiffBoss
    {
        public ProtectorsOfTheEndless()
        {
            #region Info
            Name = "Protectors of the Endless";
            Instance = "Terrace of Endless Spring";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class Tsulong : MultiDiffBoss
    {
        public Tsulong()
        {
            #region Info
            Name = "Tsulong";
            Instance = "Terrace of Endless Spring";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class LeiShi : MultiDiffBoss
    {
        public LeiShi()
        {
            #region Info
            Name = "Lei Shi";
            Instance = "Terrace of Endless Spring";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }

    public class ShaOfFear : MultiDiffBoss
    {
        public ShaOfFear()
        {
            #region Info
            Name = "Sha of Fear";
            Instance = "Terrace of Endless Spring";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T14_10N, BossHandler.TierLevels.T14_25N, BossHandler.TierLevels.T14_10H, BossHandler.TierLevels.T14_25H, BossHandler.TierLevels.T14_LFR };
            #endregion
            #region Basics
            Health = new float[] { 80000000f, 264000000f, 120000000f, 480000000f, 250000000 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5, 6 };
            TimeBossIsInvuln = new float[] { 0, 0, 0, 0, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 5; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 1.15f,
                    AttackSpeed = 1.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0, 0.00f, 0.00f, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0, 0.00f, 0.00f, 0, 0 };
        }
    }
    #endregion
}
