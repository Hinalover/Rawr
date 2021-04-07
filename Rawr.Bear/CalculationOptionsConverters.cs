using System;
using System.Windows.Data;

namespace Rawr.Bear
{
    /// <summary>
    /// Converts text to the multiplier value and back
    /// </summary>
    public class SymbiosisValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            float symbiosisValue = (float)value;
            if (symbiosisValue == 0f) return "None";
            if (symbiosisValue == 1f) return "Death Knight: Bone Shield";
            if (symbiosisValue == 2f) return "Hunter: Ice Trap";
            if (symbiosisValue == 3f) return "Mage: Mage Ward";
            if (symbiosisValue == 4f) return "Monk: Elusive Brew";
            if (symbiosisValue == 5f) return "Paladin: Consecration";
            if (symbiosisValue == 6f) return "Priest: Fear Ward";
            if (symbiosisValue == 7f) return "Rogue: Feint";
            if (symbiosisValue == 8f) return "Shaman: Lightning Shield";
            if (symbiosisValue == 9f) return "Warlock: Life Tap";
            if (symbiosisValue == 10f) return "Warrior: Spell Reflection";
            else return "None";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string symbiosisValue = (string)value;
            switch (symbiosisValue)
            {
                case "None": return 0f;
                case "Death Knight: Bone Shield": return 1f;
                case "Hunter: Ice Trap": return 2f;
                case "Mage: Mage Ward": return 3f;
                case "Monk: Elusive Brew": return 4f;
                case "Paladin: Consecration": return 5f;
                case "Priest: Fear Ward": return 6f;
                case "Rogue: Feint": return 7f;
                case "Shaman: Lightning Shield": return 8f;
                case "Warlock: Life Tap": return 9f;
                case "Warrior: Spell Reflection": return 10f;
            }
            return 0;
        }

        #endregion
    }
}
