using System;
using CountingLabel.iOS.Enums;

namespace CountingLabel.iOS.Interfaces
{
    internal interface ICountingLabel
    {
        void CountFrom(double startValue, double endValue, double duration, TimingFunction timingFunction);

        Func<double, string> SetTextDelegate { get; set; }

        string StringFormat { get; set; }
    }
}
