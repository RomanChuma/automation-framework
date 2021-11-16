using System;

using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Extensions;

namespace AutomationFramework.Core.Utils
{
    public static class HtmlStyleHelper
    {
        public static FontWeight GetFontWeightNameFromItsValue(int fontValue)
        {
            if (fontValue > 1 && fontValue <= Convert.ToInt32(FontWeight.Thin.GetDescription()))
            {
                return FontWeight.Thin;
            }

            if (fontValue <= Convert.ToInt32(FontWeight.ExtraLight.GetDescription()))
            {
                return FontWeight.ExtraLight;
            }

            if (fontValue <= Convert.ToInt32(FontWeight.Light.GetDescription()))
            {
                return FontWeight.Light;
            }

            if (fontValue <= Convert.ToInt32(FontWeight.Normal.GetDescription()))
            {
                return FontWeight.Normal;
            }

            if (fontValue <= Convert.ToInt32(FontWeight.Normal.GetDescription()))
            {
                return FontWeight.Normal;
            }

            if (fontValue <= Convert.ToInt32(FontWeight.Medium.GetDescription()))
            {
                return FontWeight.Medium;
            }

            if (fontValue <= Convert.ToInt32(FontWeight.SemiBold.GetDescription()))
            {
                return FontWeight.SemiBold;
            }

            if (fontValue <= Convert.ToInt32(FontWeight.ExtraBold.GetDescription()))
            {
                return FontWeight.ExtraBold;
            }

            if (fontValue <= Convert.ToInt32(FontWeight.Black.GetDescription()) && fontValue <= 1000)
            {
                return FontWeight.Black;
            }

            return FontWeight.None;
        }

        /// <summary>
        /// Converts RGB Value of color to Hexadecimal Value
        /// </summary>
        public static string ConvertRgbToHexaDecimal(string code)
        {
            string[] numbers = code.Replace("rgba(", string.Empty).Replace(")", string.Empty).Split(',');

            int redValue = Convert.ToInt32(numbers[0]);
            int greenValue = Convert.ToInt32(numbers[1]);
            int blueValue = Convert.ToInt32(numbers[2]);

            string hexadecimalValue = "#" + redValue.ToString("X") + greenValue.ToString("X") + blueValue.ToString("X");
            return hexadecimalValue;
        }

        /// <summary>
        /// Returns color name to Hexadecimal Value
        /// </summary>
        public static string GetColorNameFromHexaDecimalValue(string hexaValue)
        {
            ColorCode[] hexColorCodeList = (ColorCode[])Enum.GetValues(typeof(ColorCode));
            foreach (ColorCode ColorCodeInHex in hexColorCodeList)
            {
                string hexcolorcode = ColorCodeInHex.GetDescription().ToLower();
                if (hexcolorcode.Equals(hexaValue.ToLower()))
                {
                    return ColorCodeInHex.ToString();
                }
            }

            return null;
        }
    }
}
