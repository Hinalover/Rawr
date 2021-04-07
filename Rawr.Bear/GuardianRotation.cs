using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Feral;

namespace Rawr.Bear
{
    public class GuardianRotation
    {
        public const float MIN_GCD_MS = 1.5f;

        private GuardianCombatState CombatState = new GuardianCombatState();
        /// <summary>Length of the fight</summary>
        private float _fightLength = 0;
        /// <summary>Total Rage a player is able to use in a fight</summary>
        private float _Rage = 0;
        private GuardianSymbiosis _Symbiosis = GuardianSymbiosis.None;

        // Base damage values
        public float TrinketDPS = 0;
        public White_Attack Melee;
        public Mangle_Bear Mangle;
        public Swipe_Bear Swipe;
        public Thrash_Bear Thrash;
        /// <summary>
        /// Lacerate at one stack
        /// </summary>
        public Lacerate Lacerate_One;
        /// <summary>
        /// Lacerate at two stacks
        /// </summary>
        public Lacerate Lacerate_Two;
        /// <summary>
        /// Lacerate at three stacks
        /// </summary>
        public Lacerate Lacerate_Three;
        public Faerie_Fire Faerie_Fire;
        public Maul Maul;
        public Enrage Enrage;
        public Berserk_Bear Berserk;
        public Incarnation_Bear Incarnation;

        // Base Defensive Cooldowns
        public Barkskin Barkskin;
        public Savage_Defense SavageDefense;
        public Frenzied_Regeneration FrenziedRegen;
        public Survival_Instincts SurvivalInstincts;
        public Might_of_Ursoc MightofUrsoc;
        public Leader_Of_The_Pack LeaderOfThePack;

        // Symbiosis Damage values
        public Consecration Consecration;
        public Lightning_Shield LightningShield;
        public Life_Tap LifeTap;

        // Symbiosis Defensive Cooldown
        public Bone_Shield BoneShield;
        public Elusive_Brew ElusiveBrew;
        public Feint Feint;

        public Renewal Renewal;
        public Cenarion_Ward CenarionWard;
        public Healing_Touch_wo_NS HealingTouchWithoutNaturesSwifteness;
        public Healing_Touch_w_NS HealingTouchWithNaturesSwifteness;

        public GuardianRotation()
        {
            CombatState = new GuardianCombatState();
            _fightLength = 600f;
            _Symbiosis = GuardianSymbiosis.None;

            // Base Damage Abilities
            Melee = new White_Attack();
            Mangle = new Mangle_Bear();
            Swipe = new Swipe_Bear();
            Thrash = new Thrash_Bear();
            Lacerate_One = new Lacerate();
            Lacerate_Two = new Lacerate();
            Lacerate_Three = new Lacerate();
            Faerie_Fire = new Faerie_Fire();
            Maul = new Maul();
            Enrage = new Enrage();
            Berserk = new Berserk_Bear();
            Incarnation = new Incarnation_Bear();

           // Base Defensive Cooldowns
            Barkskin = new Barkskin();
            SavageDefense = new Savage_Defense();
            FrenziedRegen = new Frenzied_Regeneration();
            SurvivalInstincts = new Survival_Instincts();
            MightofUrsoc = new Might_of_Ursoc();
            LeaderOfThePack = new Leader_Of_The_Pack();

            // Symbiosis Damage values
            Consecration = new Consecration();
            LightningShield = new Lightning_Shield();
            LifeTap = new Life_Tap();

            // Symbiosis Defensive Cooldown
            BoneShield = new Bone_Shield();
            ElusiveBrew = new Elusive_Brew();
            Feint = new Feint();

            // Healing Talents
            Renewal = new Renewal();
            CenarionWard = new Cenarion_Ward();
            HealingTouchWithoutNaturesSwifteness = new Healing_Touch_wo_NS();
            HealingTouchWithNaturesSwifteness = new Healing_Touch_w_NS();

            Initialize(CombatState);

            Rotation();
        }

        public GuardianRotation(GuardianCombatState CState, float fightLength, GuardianSymbiosis symbiosis)
        {
            CombatState = CState;
            _fightLength = fightLength;
            _Symbiosis = symbiosis;

            // Base Damage Abilities
            Melee = new White_Attack();
            Mangle = new Mangle_Bear();
            Swipe = new Swipe_Bear();
            Thrash = new Thrash_Bear();
            Lacerate_One = new Lacerate();
            Lacerate_Two = new Lacerate();
            Lacerate_Three = new Lacerate();
            Faerie_Fire = new Faerie_Fire();
            Maul = new Maul();
            Enrage = new Enrage();
            Berserk = new Berserk_Bear();
            Incarnation = new Incarnation_Bear();

            // Base Defensive Cooldowns
            Barkskin = new Barkskin();
            SavageDefense = new Savage_Defense();
            FrenziedRegen = new Frenzied_Regeneration();
            SurvivalInstincts = new Survival_Instincts();
            MightofUrsoc = new Might_of_Ursoc();
            LeaderOfThePack = new Leader_Of_The_Pack();

            // Symbiosis Damage values
            Consecration = new Consecration();
            LightningShield = new Lightning_Shield();
            LifeTap = new Life_Tap();

            // Symbiosis Defensive Cooldown
            BoneShield = new Bone_Shield();
            ElusiveBrew = new Elusive_Brew();
            Feint = new Feint();

            // Healing Talents
            Renewal = new Renewal();
            CenarionWard = new Cenarion_Ward();
            HealingTouchWithoutNaturesSwifteness = new Healing_Touch_wo_NS();
            HealingTouchWithNaturesSwifteness = new Healing_Touch_w_NS();

            Initialize(CombatState);

            Rotation();
        }

        private void Initialize(GuardianCombatState CState)
        {
            // Base Damage Abilities
            Melee.Initialize(CState);
            Mangle.Initialize(CState);
            Swipe.Initialize(CState);
            Thrash.Initialize(CState);
            Lacerate_One.Initialize(CState, 1f);
            Lacerate_Two.Initialize(CState, 2f);
            Lacerate_Three.Initialize(CState, 3f);
            Faerie_Fire.Initialize(CState);
            Maul.Initialize(CState);
            Enrage.Initialize(CState);
            Berserk.Initialize(CState);
            Incarnation.Initialize(CState);

            // Base Defensive Cooldowns
            Barkskin.Initialize(CState);
            SavageDefense.Initialize(CState);
            FrenziedRegen.Initialize(CState, 60f);
            SurvivalInstincts.Initialize(CState);
            MightofUrsoc.Initialize(CState);
            LeaderOfThePack.Initialize(CState);

            // Symbiosis Damage values
            Consecration.Initialize(CState);
            LightningShield.Initialize(CState);
            LifeTap.Initialize(CState);

            // Symbiosis Defensive Cooldown
            BoneShield.Initialize(CState);
            ElusiveBrew.Initialize(CState);
            Feint.Initialize(CState);

            // Healing Talents
            Renewal.Initialize(CState);
            CenarionWard.Initialize(CState);
            HealingTouchWithoutNaturesSwifteness.Initialize(CState);
            HealingTouchWithNaturesSwifteness.Initialize(CState);
        }

        public void updateCombatState(GuardianCombatState CState)
        {
            // Base Damage Abilities
            Melee.UpdateCombatState(CState);
            Mangle.UpdateCombatState(CState);
            Swipe.UpdateCombatState(CState);
            Thrash.UpdateCombatState(CState);
            Lacerate_One.UpdateCombatState(CState);
            Lacerate_Two.UpdateCombatState(CState);
            Lacerate_Three.UpdateCombatState(CState);
            Faerie_Fire.UpdateCombatState(CState);
            Maul.UpdateCombatState(CState);
            Berserk.UpdateCombatState(CState);
            Incarnation.UpdateCombatState(CState);

            // Base Defensive Cooldowns
            Barkskin.UpdateCombatState(CState);
            SavageDefense.UpdateCombatState(CState);
            FrenziedRegen.UpdateCombatState(CState);
            SurvivalInstincts.UpdateCombatState(CState);
            MightofUrsoc.UpdateCombatState(CState);
            LeaderOfThePack.UpdateCombatState(CState);

            // Symbiosis Damage values
            Consecration.UpdateCombatState(CState);
            LightningShield.UpdateCombatState(CState);
            LifeTap.UpdateCombatState(CState);

            // Symbiosis Defensive Cooldown
            BoneShield.UpdateCombatState(CState);
            ElusiveBrew.UpdateCombatState(CState);
            Feint.UpdateCombatState(CState);

            // Healing Talents
            Renewal.UpdateCombatState(CState);
            CenarionWard.UpdateCombatState(CState);
            HealingTouchWithoutNaturesSwifteness.UpdateCombatState(CState);
            HealingTouchWithNaturesSwifteness.UpdateCombatState(CState);
        }

        private void Rotation()
        {
            // Figure out Melee Auto Attack Count since this will be passive
            Melee.Count = (_fightLength / CombatState.MainHand.hastedSpeed);
            Enrage.Count = (_fightLength / Enrage.Cooldown);

            float GCD_count = _fightLength / MIN_GCD_MS;

            // TODO: Get a better rotation set up. This is only temporary to get a base idea on damage
            Faerie_Fire.Count = (float)Math.Floor(_fightLength / Faerie_Fire.Duration);
            Thrash.Count = (float)Math.Floor((_fightLength / Thrash.Duration));
            CombatState.NumberOfBleeds++;
            Lacerate_Three.Count = (float)Math.Floor((_fightLength / Lacerate_Three.Duration));
            CombatState.NumberOfBleeds++;

            float ManglesPerSecond = 0.23701331018518f;
            Mangle.Count = (float)Math.Floor((_fightLength * ManglesPerSecond));

            // Symbiosis
            if (_Symbiosis == GuardianSymbiosis.Consecration)
                Consecration.Count = (Duration / Consecration.Cooldown);
            else
                Consecration.Count = 0;

            if (_Symbiosis == GuardianSymbiosis.LightningShield)
                LightningShield.Count = (Duration / LightningShield.Cooldown);
            else
                LightningShield.Count = 0;

            if (_Symbiosis == GuardianSymbiosis.LifeTap)
                LifeTap.Count = (Duration / LifeTap.Cooldown);
            else
                LifeTap.Count = 0;

            Faerie_Fire.Count += GCD_count - (Faerie_Fire.Count + Thrash.Count + Lacerate_Three.Count + Mangle.Count + Enrage.Count + Consecration.Count + LightningShield.Count + LifeTap.Count);

            // Apply Hit Chance
            Faerie_Fire.Count *= Faerie_Fire.HitChance;
            Thrash.Count *= Thrash.HitChance;
            Lacerate_Three.Count *= Lacerate_Three.HitChance;
            Mangle.Count *= Mangle.HitChance;
            Swipe.Count *= Mangle.HitChance;
            updateCombatState(CombatState);
        }

        #region Damage
        public float totalDamageDone
        {
            get
            {
                float damageDone = 0;

                if (Mangle.Count > 0)
                    damageDone += Mangle.DamageDone();
                if (Faerie_Fire.Count > 0)
                    damageDone += Faerie_Fire.DamageDone();
                if (Thrash.Count > 0)
                    damageDone += Thrash.DamageDone();
                if (Lacerate_One.Count > 0)
                    damageDone += Lacerate_One.DamageDone();
                if (Lacerate_Two.Count > 0)
                    damageDone += Lacerate_Two.DamageDone();
                if (Lacerate_Three.Count > 0)
                    damageDone += Lacerate_Three.DamageDone();
                if (Swipe.Count > 0)
                    damageDone += Swipe.DamageDone();
                if (Maul.Count > 0)
                    damageDone += Maul.DamageDone();
                if (Consecration.Count > 0)
                    damageDone += Consecration.DamageDone();
                if (LightningShield.Count > 0)
                    damageDone += LightningShield.DamageDone();
                damageDone += Melee.DamageDone();
                damageDone += TrinketDPS;

                return damageDone;
            }
        }

        public float totalDPS()
        {
            return this.totalDamageDone / _fightLength;
        }

        public float totalThreatDone
        {
            get
            {
                return totalDamageDone * (5 * (1 + CombatState.Stats.ThreatIncreaseMultiplier) * (1 - CombatState.Stats.ThreatReductionMultiplier));
            }
        }

        public float totalTPS()
        {
            return this.totalThreatDone / _fightLength;
        }

        #endregion

        public float getTotalRageGenerated()
        {
            return Mangle.TotalRageGenerated + Melee.TotalRageGenerated + Enrage.TotalRageGenerated + LifeTap.TotalRageGenerated;
        }

        public float getTotalRPS()
        {
            return Mangle.RagePerSecond(_fightLength) + Melee.RagePerSecond(_fightLength) + Enrage.RagePerSecond(_fightLength);
        }

        public float Duration
        {
            get
            {
                return _fightLength;
            }
        }

        public void generateDefensiveCooldownInfo()
        {
            float totalrage = getTotalRageGenerated();
            SavageDefense.Uptime = SavageDefense.getUptime(totalrage, Duration);
            SavageDefense.AverageDodge = SavageDefense.getAverageDodge(totalrage, Duration);
            SavageDefense.Count = (Duration * SavageDefense.Uptime) / SavageDefense.Duration;
            totalrage -= SavageDefense.getTotalRageUsed(totalrage, Duration);

            // For the time being just worry about Savage Defense and Frenzied Regen as the only two abilities using Rage.
            // Not going to worry about Maul or Feint for the time being.
            FrenziedRegen.Count = totalrage / FrenziedRegen.Rage;

            Barkskin.Count = (Duration / Barkskin.Cooldown);
            Barkskin.Uptime = (Barkskin.Count * Barkskin.Duration) / Duration;

            SurvivalInstincts.Count = (Duration / SurvivalInstincts.Cooldown);
            SurvivalInstincts.Uptime = (SurvivalInstincts.Count * SurvivalInstincts.Duration) / Duration;

            MightofUrsoc.Count = (Duration / MightofUrsoc.Cooldown);
            MightofUrsoc.Uptime = (MightofUrsoc.Count * MightofUrsoc.Duration) / Duration;

            LeaderOfThePack.BaseCritInterval = CritInterval;
            LeaderOfThePack.Count = (Duration / LeaderOfThePack.CritInterval);

            HealingTouchWithNaturesSwifteness.Count = (CombatState.Talents.NaturesSwiftness > 0 ? (Duration / HealingTouchWithNaturesSwifteness.Cooldown) : 0);

            Renewal.Count = (CombatState.Talents.Renewal > 0 ? (Duration / Renewal.Cooldown) : 0);

            CenarionWard.Count = (CombatState.Talents.CenarionWard > 0 ? (Duration / CenarionWard.Cooldown) : 0);

            // Symbiosis
            if (_Symbiosis == GuardianSymbiosis.BoneShield)
                BoneShield.Count = (Duration / BoneShield.Cooldown);
            else
                BoneShield.Count = 0;
            BoneShield.Uptime = (BoneShield.Count * BoneShield.Duration) / Duration;

            if (_Symbiosis == GuardianSymbiosis.ElusiveBrew)
                ElusiveBrew.Count = (Duration / ElusiveBrew.Cooldown);
            else
                ElusiveBrew.Count = 0;
            ElusiveBrew.Uptime = (ElusiveBrew.Count * ElusiveBrew.Duration) / Duration;
        }

        #region Counts and Intervals
        private float _abilityCount = 0;
        private float AbilityCount
        {
            get
            {
                if (_abilityCount == 0)
                {
                    _abilityCount = Mangle.Count + Swipe.Count + Thrash.Count + Lacerate_One.Count + Lacerate_Two.Count + Lacerate_Three.Count + Faerie_Fire.Count + Maul.Count;
                }
                return _abilityCount;
            }
        }

        private float _abilityCritCount = 0;
        private float AbilityCritCount
        {
            get
            {
                if (_abilityCritCount == 0)
                {
                    float mangleCount = Mangle.Count * Mangle.CritChance;
                    float swipeCount = Swipe.Count * Swipe.CritChance;
                    float thrashCount = Thrash.Count * Thrash.CritChance;
                    float lacerateCount = (Lacerate_One.Count + Lacerate_Two.Count + Lacerate_Three.Count) * Lacerate_One.CritChance;
                    float ffCount = Faerie_Fire.Count * Faerie_Fire.CritChance;
                    float maulCount = Maul.Count * Maul.CritChance;
                    _abilityCritCount = mangleCount + swipeCount + thrashCount + lacerateCount + ffCount + maulCount;
                }

                return _abilityCritCount;
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
                return MangleAbilityCount;
            }
        }

        public float MangleShredAbilityInterval
        {
            get
            {
                return MangleAbilityInterval;
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
                return Mangle.HitChance;
            }
        }

        private float _critChance = 0;
        public float CritChance
        {
            get
            {
                if (_critChance == 0)
                {
                    _critChance = (Mangle.CritChance + Swipe.CritChance + Thrash.CritChance + Lacerate_One.CritChance + Lacerate_Two.CritChance +
                    Lacerate_Three.CritChance + Faerie_Fire.CritChance + Maul.CritChance) / 8f;
                }
                return _critChance;
            }
        }

        public float CritInterval
        {
            get
            {
                return Math.Max(1, Duration / AbilityCritCount);
            }
        }

        private float _doTTickCount = 0;
        public float DoTTickCount
        {
            get
            {
                if (_doTTickCount == 0) 
                {
                    float thrashTick = Thrash.BaseTickCount * Thrash.Count;
                    float lacerateTick = Lacerate_One.BaseTickCount * (Lacerate_One.Count + Lacerate_Two.Count + Lacerate_Three.Count);
                    _doTTickCount = (thrashTick + lacerateTick);
                }

                return _doTTickCount;
            }
        }

        public float DotTickInterval
        {
            get
            {
                return Math.Max(1, Duration / DoTTickCount);
            }
        }

        public float HitorDoTTickInterval
        {
            get
            {
                return Math.Max(1, Duration / (AbilityCount + DoTTickCount));
            }
        }
        #endregion
    }
}
