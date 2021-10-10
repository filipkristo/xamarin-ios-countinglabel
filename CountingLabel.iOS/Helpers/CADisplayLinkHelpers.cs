using System;
using System.Diagnostics;
using CoreAnimation;

namespace CountingLabel.iOS.Helpers
{
    internal static class CADisplayLinkHelpers
    {
        internal static CAFrameRateRange GetPreferredFrameRateRange(double start, double end, double duration)
        {
            var updatesPerSecond = GetUpdatesPerSecond(start, end, duration);

            if (IsNumerInRange(updatesPerSecond, 0, 10))
            {
                Debug.WriteLine("10, 10, 10");
                return new CAFrameRateRange(10, 10, 10);
            }
            if (IsNumerInRange(updatesPerSecond, 0, 30))
            {
                Debug.WriteLine("10, 30, 30");
                return new CAFrameRateRange(10, 30, 30);
            }
            if (IsNumerInRange(updatesPerSecond, 0, 60))
            {
                Debug.WriteLine("Default");
                return CAFrameRateRange.Default;
            }
            if (IsNumerInRange(updatesPerSecond, 0, 80))
            {
                Debug.WriteLine("80, 120, 80");
                return new CAFrameRateRange(80, 120, 80);
            }

            Debug.WriteLine("80, 120, 120");
            return new CAFrameRateRange(80, 120, 120);
        }

        internal static nint GetPreferredFramesPerSecond(double start, double end, double duration)
        {
            var updatesPerSecond = GetUpdatesPerSecond(start, end, duration);

            if (IsNumerInRange(updatesPerSecond, 0, 15))
            {
                return 15;
            }
            if (IsNumerInRange(updatesPerSecond, 15, 20))
            {
                return 20;
            }
            if (IsNumerInRange(updatesPerSecond, 20, 30))
            {
                return 30;
            }
            return 60;
        }

        private static int GetUpdatesPerSecond(double start, double end, double duration)
        {
            var interation = Math.Round(end - start, 0);
            var updatesPerSecond = Convert.ToInt32(Math.Round(interation / duration));

            return updatesPerSecond;
        }

        private static bool IsNumerInRange(int number, int min, int max) => number >= min && number <= max;
    }
}
