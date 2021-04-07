using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using Rawr.Feral;

namespace Rawr.Bear
{
    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsBear : ICalculationOptionBase, INotifyPropertyChanged
	{
        [DefaultValue(true)]
        public bool UseBossHandler
        {
            get
            {
                return _useBossHandler;
            }
            set
            {
                _useBossHandler = value;
                OnPropertyChanged("ckbUseBossHandler");
            }
        }
        private bool _useBossHandler = true;

        [DefaultValue(90)]
        public int CharacterLevel
        {
            get
            {
                return _characterLevel;
            }
            set
            {
                _characterLevel = value;
                OnPropertyChanged("nudCharacterLevel");
            }
        }
        private int _characterLevel = 90;

        [DefaultValue(93)]
        public int TargetLevel
        {
            get
            {
                return _targetLevel;
            }
            set
            {
                _targetLevel = value;
                OnPropertyChanged("nudTargetLevel");
            }
        }
        private int _targetLevel = 93;

        public int TargetDifferance
        {
            get
            {
                return _targetLevel - _characterLevel;
            }
        }

        [DefaultValue(600f)]
        public float FightLength
        {
            get
            {
                return _fightLength;
            }
            set
            {
                _fightLength = value;
                OnPropertyChanged("nudLengthofFight");
            }
        }
        private float _fightLength = 600;

        public int LagVariance
        {
            get { return _lagVariance; }
            set { if (_lagVariance != value) { _lagVariance = value; OnPropertyChanged("LagVariance"); } }
        }
        private int _lagVariance = 200;

        private int _symbiosis = 0;
        [DefaultValue(0)]
        public int Symbiosis
        {
            get { return (int)_symbiosis; }
            set { if ((int)_symbiosis != value) { _symbiosis = value; OnPropertyChanged("cmbSymbiosis"); } }
        }
        public GuardianSymbiosis GuardianSymbiosis
        {
            get
            {
                switch (_symbiosis)
                {
                    case 0:
                        return GuardianSymbiosis.None;
                    case 1:
                        return GuardianSymbiosis.BoneShield;
                    case 2:
                        return GuardianSymbiosis.IceTrap;
                    case 3:
                        return GuardianSymbiosis.MageWard;
                    case 4:
                        return GuardianSymbiosis.ElusiveBrew;
                    case 5:
                        return GuardianSymbiosis.Consecration;
                    case 6:
                        return GuardianSymbiosis.FearWard;
                    case 7:
                        return GuardianSymbiosis.Feint;
                    case 8:
                        return GuardianSymbiosis.LightningShield;
                    case 9:
                        return GuardianSymbiosis.LifeTap;
                    case 10:
                        return GuardianSymbiosis.SpellReflection;
                    default:
                        return GuardianSymbiosis.None;
                }
            }
        }

        private float _bossUnmitigatedDamage = 350000f;
        [DefaultValue(350000f)]
        public float BossUnmitigatedDamage
        {
            get { return _bossUnmitigatedDamage; }
            set { if (_bossUnmitigatedDamage != value) { _bossUnmitigatedDamage = value; OnPropertyChanged("NUD_BossUnmitigatedDamage"); } }
        }

        private float _bossSwingSpeed = 1.5f;
        [DefaultValue(1.5f)]
        public float BossSwingSpeed
        {
            get { return _bossSwingSpeed; }
            set { if (_bossSwingSpeed != value) { _bossSwingSpeed = value; OnPropertyChanged("NUD_SwingSpeed"); } }
        }

        private float _hitsToLive = 3.5f;
        [DefaultValue(3.5f)]
        public float HitsToLive
        {
            get { return _hitsToLive; }
            set { if (_hitsToLive != value) { _hitsToLive = value; OnPropertyChanged("NUD_HitsToSurvive"); } }
        }

        private float _mitigationOrDPS = 0f;
        [Percentage]
        [DefaultValue(0f)]
        public float MitigationOrDPS
        {
            get { return _mitigationOrDPS; }
            set { if (_mitigationOrDPS != value) { _mitigationOrDPS = value; OnPropertyChanged("nudMitigationOrDPS"); } }
        }

        [DefaultValue(false)]
        public bool PTRMode { get { return _ptrMode; } set { _ptrMode = value; OnPropertyChanged("ckbPTRMode"); } }
        private bool _ptrMode = false;

        public string GetXml()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsBear));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		#region Rating Customization
		private float _threatScale = 5f;
		public float ThreatScale
		{
			get { return _threatScale; }
			set { if (_threatScale != value) { _threatScale = value; OnPropertyChanged("ThreatScale"); } }
		}
		private double _survivalSoftCap = 437500f;
        public double SurvivalSoftCap
		{
            get { return _survivalSoftCap; }
            set { if (_survivalSoftCap != value) { _survivalSoftCap = value; OnPropertyChanged("SurvivalSoftCap"); } }
		}
		private float _temporarySurvivalScale = 1f;
		public float TemporarySurvivalScale
		{
			get { return _temporarySurvivalScale; }
			set { if (_temporarySurvivalScale != value) { _temporarySurvivalScale = value; OnPropertyChanged("TemporarySurvivalScale"); } }
		}
		#endregion

		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
		}
		#endregion
	}
}
