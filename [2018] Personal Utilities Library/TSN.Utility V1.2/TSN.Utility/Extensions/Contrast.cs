using System;
using System.Drawing;

namespace TSN.Utility.Extensions
{
    public static class Contrast
    {
        static Contrast()
        {
            MidpointRoundingDigits = _midpointRoundingDigits;
            _whiteConstant = Math.Sqrt(1.05D * .05D) - .05D;
        }


        private const byte _midpointRoundingDigits = 7;     // MAX

        private static readonly double _whiteConstant;

        public static byte MidpointRoundingDigits { get; set; }
        public static double Midpoint => Math.Round(_whiteConstant, MidpointRoundingDigits, MidpointRounding.AwayFromZero);


        private static bool IsMidpoint(double luminance)
        {
            var digits = MidpointRoundingDigits;
            return Math.Round(luminance, digits, MidpointRounding.AwayFromZero) == Math.Round(_whiteConstant, digits, MidpointRounding.AwayFromZero);
        }
        public static bool IsMostlyWhite(this Color c, out bool isMidpoint)
        {
            double l = GetLuminance(c);
            isMidpoint = IsMidpoint(l);
            return l > _whiteConstant;
        }
        public static bool IsMostlyBlack(this Color c, out bool isMidpoint)
        {
            double l = GetLuminance(c);
            isMidpoint = IsMidpoint(l);
            return l <= _whiteConstant;
        }
        public static double GetLuminance(this Color c)
        {
            // Source: https://stackoverflow.com/a/3943023
            double GetL(int rgb)
            {
                double l = rgb / 255D;
                return l <= .03928D ? (l / 12.92D) : Math.Pow(((l + .055D) / 1.055D), 2.4D);
            }
            return .2126D * GetL(c.R) + .7152D * GetL(c.G) + .0722 * GetL(c.B);
        }
    }
}