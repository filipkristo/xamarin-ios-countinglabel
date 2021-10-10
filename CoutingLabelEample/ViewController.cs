using CountingLabel.iOS;
using Security;
using System;
using UIKit;

namespace CoutingLabelEample
{
    public partial class ViewController : UIViewController
    {
        private readonly UICountingLabel _label;
        private readonly UIButton _button;

        public ViewController(IntPtr handle) : base(handle)
        {
            _label = new UICountingLabel
            {
                Text = "0 %",
                TextColor = UIColor.Black,
                StringFormat = "{0} %"
            };
            _label.ExecutionCompleted += _label_ExecutionCompleted;

            _button = new UIButton();
            _button.SetTitle("Count", UIControlState.Normal);
            _button.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            _button.TouchUpInside += _button_TouchUpInside;
        }

        private void _button_TouchUpInside(object sender, EventArgs e)
        {
            var controller = UIAlertController.Create(null, "Select speed", UIAlertControllerStyle.ActionSheet);
            controller.AddAction(UIAlertAction.Create("Slow 10 FPS", UIAlertActionStyle.Default, (_) =>
            {
                _label.CountFrom(0, 16, 8, CountingLabel.iOS.Enums.TimingFunction.EasyIn);
            }));
            controller.AddAction(UIAlertAction.Create("Slow 30 FPS", UIAlertActionStyle.Default, (_) =>
            {
                _label.CountFrom(0, 90, 3, CountingLabel.iOS.Enums.TimingFunction.EasyIn);
            }));
            controller.AddAction(UIAlertAction.Create("Normal 60 FPS", UIAlertActionStyle.Default, (_) =>
            {
                _label.CountFrom(0, 180, 3, CountingLabel.iOS.Enums.TimingFunction.EasyIn);
            }));
            controller.AddAction(UIAlertAction.Create("ProMotion 120 FPS", UIAlertActionStyle.Default, (_) =>
            {
                _label.CountFrom(0, 360, 3, CountingLabel.iOS.Enums.TimingFunction.EasyIn);
            }));

            PresentViewController(controller, true, null);
        }

        private void _label_ExecutionCompleted(object sender, ExecutionCompletedEventArgs e)
        {
            var okAlertController = UIAlertController.Create("Execution Completed", $"IsCancelled:{e.IsCancelled}{Environment.NewLine}Duration:{e.Duration}", UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            PresentViewController(okAlertController, true, null);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            View.AddSubview(_label);
            View.AddSubview(_button);

            var viewWidth = View.Bounds.Width;
            var viewHeight = 31f;

            _label.Frame = new CoreGraphics.CGRect(30, 30, 100, 40);
            _button.Frame = new CoreGraphics.CGRect(30, 80, 100, 40);
        }
    }
}