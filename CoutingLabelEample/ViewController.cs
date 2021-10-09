using CountingLabel.iOS;
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
                Text = "0",
                TextColor = UIColor.Black,
                StringFormat = "{0} %"
            };

            _button = new UIButton();
            _button.SetTitle("Count", UIControlState.Normal);
            _button.SetTitleColor(UIColor.Blue, UIControlState.Normal);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            View.AddSubview(_label);
            View.AddSubview(_button);

            _label.Frame = new CoreGraphics.CGRect(30, 30, 100, 40);

            _button.Frame = new CoreGraphics.CGRect(30, 80, 100, 40);

            _button.TouchUpInside += (sender, e) => {
                _label.CountFrom(0, 16, 8, CountingLabel.iOS.Enums.TimingFunction.EasyIn);
            };
        }
    }
}
