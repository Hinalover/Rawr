using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rawr.Feral
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalculationOptionsFeral : ICalculationOptionBase, INotifyPropertyChanged
    {
        [DefaultValue(false)]
        public bool UseBossHandler
        {
            get
            {
                return _useBossHandler;
            }
            set
            {
                _useBossHandler = (bool)value;
                OnPropertyChanged("ckbUseBossHandler");
            }
        }
        private bool _useBossHandler = false;

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

        /// <summary>
        /// Percent of time spent behind the boss
        /// </summary>
        [DefaultValue(1f)]
        public float PercentBehindBoss
        {
            get
            {
                return _percentBehindBoss;
            }
            set
            {
                _percentBehindBoss = value;
                OnPropertyChanged("nudPercentBehindBoss");
            }
        }
        private float _percentBehindBoss = 1f;

        public int LagVariance
        {
            get { return _lagVariance; }
            set { if (_lagVariance != value) { _lagVariance = value; OnPropertyChanged("LagVariance"); } }
        }
        private int _lagVariance = 200;

        [DefaultValue(false)]
        public bool PTRMode { get { return _ptrMode; } set { _ptrMode = value; OnPropertyChanged("ckbPTRMode"); } }
        private bool _ptrMode = false;

        public CalculationOptionsFeral Clone()
        {
            CalculationOptionsFeral clone = new CalculationOptionsFeral
            {
                UseBossHandler = UseBossHandler,
                CharacterLevel = CharacterLevel,
                TargetLevel = TargetLevel,
                FightLength = FightLength,
                PercentBehindBoss = PercentBehindBoss,
                LagVariance = LagVariance
            };
            // Tab - Fight Parameters
            return clone;
        }

        #region ICalculationOptionBase Members
        /// <summary>
        /// Gets the XML serialization of the calculation options for use in the character file.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsFeral));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
