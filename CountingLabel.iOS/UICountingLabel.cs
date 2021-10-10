using System;
using System.Globalization;
using CoreAnimation;
using CountingLabel.iOS.Enums;
using CountingLabel.iOS.Helpers;
using CountingLabel.iOS.Interfaces;
using Foundation;
using UIKit;

namespace CountingLabel.iOS
{
    public class UICountingLabel : UILabel, ICountingLabel
    {
        private const double _counterRate = 3.0;

        private double _startingValue;
        private double _destinationValue;

        private double _progress;
        private double _lastUpdate;
        private double _totalTime;

        private CADisplayLink _timer;

        private TimingFunction _timingFunction;

        public event EventHandler<ExecutionCompletedEventArgs> ExecutionCompleted;

        public Func<double, string> SetTextDelegate { get; set; }

        public string StringFormat { get; set; }

        public void CountFrom(double startValue, double endValue, double duration, TimingFunction timingFunction)
        {
            _startingValue = startValue;
            _destinationValue = endValue;
            _timingFunction = timingFunction;

            if (_timer != null)
            {
                _timer.Invalidate();
                _timer = null;
                RaiseExecutionCompletedEvent(true, _progress);
            }

            if (duration <= 0)
            {
                return;
            }

            _progress = 0;
            _totalTime = duration;
            _lastUpdate = NSDate.Now.SecondsSinceReferenceDate;

            _timer = CADisplayLink.Create(UpdateValue);
            if (UIDevice.CurrentDevice.CheckSystemVersion(15, 0))
            {
                _timer.PreferredFrameRateRange = CADisplayLinkHelpers.GetPreferredFrameRateRange(_startingValue, _destinationValue, _totalTime);
            }
            else
            {
                _timer.PreferredFramesPerSecond = CADisplayLinkHelpers.GetPreferredFramesPerSecond(_startingValue, _destinationValue, _totalTime);
            }
            _timer.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Default);
            _timer.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.UITracking);
        }

        private void UpdateValue()
        {
            var now = (nfloat)NSDate.Now.SecondsSinceReferenceDate;
            _progress += now - _lastUpdate;
            _lastUpdate = now;

            if (_progress >= _totalTime)
            {
                _timer.Invalidate();
                _timer = null;
                _progress = _totalTime;
                RaiseExecutionCompletedEvent(false, _progress);
            }

            SetTextValue(CurrentValue());
        }

        private double CurrentValue()
        {
            if (_progress >= _totalTime)
            {
                return _destinationValue;
            }

            var percent = _progress / _totalTime;

            double updateVal = 0;

            switch (_timingFunction)
            {
                case TimingFunction.EasyIn:
                    updateVal = UpdateEasyIn(percent);
                    break;
                case TimingFunction.EasyInOut:
                    updateVal = UpdateEasyInOut(percent);
                    break;
                case TimingFunction.EasyOut:
                    updateVal = UpdateEasyOut(percent);
                    break;
                case TimingFunction.Linear:
                    updateVal = UpdateLinear(percent);
                    break;
            }

            return _startingValue + (updateVal * (_destinationValue - _startingValue));
        }

        private double UpdateEasyInOut(double t)
        {
            int sign = 1;
            int r = (int)_counterRate;

            if (r % 2 == 0)
                sign = -1;

            t *= 2;

            if (t < 1)
                return 0.5f * Math.Pow(t, _counterRate);

            return sign * 0.5f * (Math.Pow(t - 2, _counterRate) + sign * 2);
        }

        private double UpdateEasyOut(double t) => 1.0 - Math.Pow(1.0 - t, _counterRate);

        private double UpdateEasyIn(double t) => Math.Pow(t, _counterRate);

        private double UpdateLinear(double t) => t;

        private void SetTextValue(double value)
        {
            if (SetTextDelegate is { })
            {
                Text = SetTextDelegate.Invoke(value);
            }
            else if (StringFormat is { })
            {
                Text = string.Format(CultureInfo.CurrentCulture, StringFormat, Math.Round(value, 0));
            }
            else
            {
                Text = Math.Round(value, 0).ToString();
            }
        }

        private void RaiseExecutionCompletedEvent(bool isCancelled, double duration)
        {
            ExecutionCompleted?.Invoke(this, new ExecutionCompletedEventArgs(isCancelled, duration));
        }
    }
}