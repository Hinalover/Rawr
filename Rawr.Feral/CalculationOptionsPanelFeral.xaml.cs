﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.Feral
{
    public partial class CalculationOptionsPanelFeral : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelFeral()
        {
            _loadingCalculationOptions = true;
            InitializeComponent();
            _loadingCalculationOptions = false;
        }

        #region ICalculationOptionsPanel Members
        public UserControl PanelControl { get { return this; } }

        public bool _loadingCalculationOptions = false;
        CalculationOptionsFeral calcOpts = null;

        private Character character;
        public Character Character
        {
            get { return character; }
            set {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsFeral)
                    ((CalculationOptionsFeral)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelFeral_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                this.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelFeral_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelFeral_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            //
            if (Character != null && Character.CalculationOptions == null) {
                Character.CalculationOptions = new CalculationOptionsFeral();
                _loadingCalculationOptions = true;
            } else if (Character == null) { return; }
            calcOpts = Character.CalculationOptions as CalculationOptionsFeral;
            //
            _loadingCalculationOptions = false;
        }

        public void CalculationOptionsPanelFeral_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }
            if (e == null) { e = new PropertyChangedEventArgs(""); }
            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            if (e.PropertyName == "SomeProperty")
            {
                // Model Specific Code
            }
            //
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        #endregion
    }
}
