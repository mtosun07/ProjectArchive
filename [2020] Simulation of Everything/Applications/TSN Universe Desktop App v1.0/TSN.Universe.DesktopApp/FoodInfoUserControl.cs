using System;
using System.Drawing;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    internal partial class FoodInfoUserControl : UserControl
    {
        static FoodInfoUserControl() => _fixedSize = new Size(_fixedWidth, _fixedHeight);
        public FoodInfoUserControl() => InitializeComponent();


        private const int _fixedWidth = 550;
        private const int _fixedHeight = 50;

        private static readonly Size _fixedSize;

        private Food _food;

        protected sealed override Size DefaultMaximumSize => _fixedSize;
        protected sealed override Size DefaultMinimumSize => _fixedSize;
        protected sealed override Size DefaultSize => _fixedSize;
        public sealed override bool AutoSize { get => false; set { } }
        public sealed override Size MaximumSize { get => _fixedSize; set { } }
        public sealed override Size MinimumSize { get => _fixedSize; set { } }



        public void RepresentInfo(Food food)
        {
            if (food == null)
                throw new ArgumentNullException(nameof(food));
            _food = food;
            if (food.TryGetReturnedFrom(out var returnedFrom))
                llblReturnedFrom.Text = returnedFrom.Id.ToString();
            if (food.TryGetEatenBy(out var eatenBy))
                llblEatenBy.Text = eatenBy.Id.ToString();
        }
        public void Clear()
        {
            _food = null;
            llblReturnedFrom.ResetText();
            llblEatenBy.ResetText();
        }
        
        private void llblMatterId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var text = ((LinkLabel)sender).Text;
            if (!string.IsNullOrEmpty(text) && Guid.TryParse(text, out var id))
                MatterInfoForm.ShowInstantce(_food.OwnerUniverse.AllMattersEver[id]);
        }
    }
}