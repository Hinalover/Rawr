using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Brewmaster
{
    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalculationOptionsBrewmaster : ICalculationOptionBase, INotifyPropertyChanged
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

        #region ICalculationOptionBase Overrides
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsBrewmaster));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
