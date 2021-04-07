using System;
using System.Collections.Generic;
using System.Text;
using Rawr.ModelFramework;

namespace Rawr.Mistweaver {
    public class CharacterCalculationsMistweaver : CharacterCalculationsBase
    {
        private float[] subPoints = new float[(int)1];
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }
        public override float OverallPoints { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            return retVal;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
            }
            return 0;
        }
    }
}
