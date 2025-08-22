using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    internal partial class ThingInfoUserControl : UserControl
    {
        static ThingInfoUserControl() => _fixedSize = new Size(_fixedWidth, _fixedHeight);
        public ThingInfoUserControl() => InitializeComponent();


        private const int _fixedWidth = 550;
        private const int _fixedHeight = 350;

        private static readonly Size _fixedSize;

        private Thing _thing;

        protected sealed override Size DefaultMaximumSize => _fixedSize;
        protected sealed override Size DefaultMinimumSize => _fixedSize;
        protected sealed override Size DefaultSize => _fixedSize;
        public sealed override bool AutoSize { get => false; set { } }
        public sealed override Size MaximumSize { get => _fixedSize; set { } }
        public sealed override Size MinimumSize { get => _fixedSize; set { } }



        public void RepresentInfo(Thing thing)
        {
            if (thing == null)
                throw new ArgumentNullException(nameof(thing));
            _thing = thing;
            lblReproduceRate.Text = _thing.ReproduceRate.ToString("P");
            lblDeathRate.Text = _thing.DeathRate.ToString("P");
            lblSense.Text = _thing.Sense.ToString();
            if (_thing.TryGetReturnedFood(out var returned))
                llblReturnsFood.Text = returned.Id.ToString();
            else
                switch (_thing.ReturnsFoodAfterDeath)
                {
                    case FoodReturningOptions.DontReturn:
                        llblReturnsFood.Text = "No";
                        break;
                    case FoodReturningOptions.ReturnAlways:
                        llblReturnsFood.Text = "Always";
                        break;
                    case FoodReturningOptions.ReturnIfNotHungry:
                        llblReturnsFood.Text = "Yes, If Not Hungry";
                        break;
                }
            lblIsHungry.Text = thing.IsHungry ? "Yes" : "No";
            int count;
            llblEatenFoodsCount.Text = (count = _thing.EatenFoodsCount) == 0 ? string.Empty : count.ToString();
            llblAscendantsCount.Text = (count = _thing.GetAscendants().Count()) == 0 ? string.Empty : count.ToString();
            var parents = _thing.GetParents();
            if (parents.Count == 0)
            {
                llblParent1.ResetText();
                llblParent2.ResetText();
            }
            else
            {
                llblParent1.Text = parents.ElementAt(0).Id.ToString();
                llblParent2.Text = parents.ElementAt(1).Id.ToString();
            }
            llblDescendantsCount.Text = (count = _thing.GetDescendants().Count()) == 0 ? string.Empty : count.ToString();
            llblChildrenCount.Text = (count = _thing.GetChildrenCount()) == 0 ? string.Empty : count.ToString();
            llblMultiChildrenCount.Text = (count = _thing.GetChildrenCount(x => x > 1)) == 0 ? string.Empty : count.ToString();
            lblSpousesCountDistinct.Text = (count = _thing.GetSpouseCountDistinct()) == 0 ? string.Empty : count.ToString();
            llblMatchCount.Text = (count = _thing.GetMatchCount()) == 0 ? string.Empty : count.ToString();
        }
        public void Clear()
        {
            _thing = null;
            lblReproduceRate.ResetText();
            lblDeathRate.ResetText();
            lblSense.ResetText();
            llblReturnsFood.ResetText();
            llblEatenFoodsCount.ResetText();
            llblAscendantsCount.ResetText();
            llblParent1.ResetText();
            llblParent2.ResetText();
            llblDescendantsCount.ResetText();
            llblChildrenCount.ResetText();
            llblMultiChildrenCount.ResetText();
            llblMatchCount.ResetText();
        }

        private void llblMatterId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var text = ((LinkLabel)sender).Text;
            if (!string.IsNullOrEmpty(text) && Guid.TryParse(text, out var id))
                MatterInfoForm.ShowInstantce(_thing.OwnerUniverse.AllMattersEver[id]);
        }
        private void llblEatenFoodsCount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!int.TryParse(((Control)sender).Text, out var count) || count <= 0)
                return;
            MatterTraitListForm<Thing, Food>.ShowInstantce("Eaten Foods by:", _thing, x => x.GetEatenFoods().OrderBy(y => y.VanishingGeneration.Value).ToArray(), null, x => MatterInfoForm.ShowInstantce(x));
        }
        private void llblSpousesCountDistinct_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!int.TryParse(((Control)sender).Text, out var count) || count <= 0)
                return;
            MatterTraitListForm<Thing, Thing>.ShowInstantce("Spouses of:", _thing, x => x.GetSpousesDistinct().ToArray(), null, x => MatterInfoForm.ShowInstantce(x));
        }
        private void llblMatchCount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!int.TryParse(((Control)sender).Text, out var count) || count <= 0)
                return;
            MatterTraitListForm<Thing, Thing.MatchInfo>.ShowInstantce("Successful Matches of:", _thing, x => x.GetMatches().OrderBy(y => y.Generation).ToArray(), null, match =>
            {
                MatterInfoForm.ShowInstantce(match.OtherParent);
                MatterTraitListForm<Thing, Thing>.ShowInstantce($"At Generation {match.Generation}, Children of [{match.OtherParent.Id}] and the Thing:", _thing, thing => thing.GetChildrenAt(match.Generation).Where(child => child.GetParents().Any(parent => parent.Id.Equals(match.OtherParent.Id))).ToArray(), null, x => MatterInfoForm.ShowInstantce(x));
            });
        }
        private void llblAscendantsCount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!int.TryParse(((Control)sender).Text, out var count) || count <= 0)
                return;
            MatterTraitListForm<Thing, Thing>.ShowInstantce("All ascendants of:", _thing, x => x.GetAscendants().ToArray(), null, x => MatterInfoForm.ShowInstantce(x));
        }
        private void llblDescendantsCount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!int.TryParse(((Control)sender).Text, out var count) || count <= 0)
                return;
            MatterTraitListForm<Thing, Thing>.ShowInstantce("All descendants of:", _thing, x => x.GetDescendants().ToArray(), null, x => MatterInfoForm.ShowInstantce(x));
        }
        private void llblChildrenCount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!int.TryParse(((Control)sender).Text, out var count) || count <= 0)
                return;
            MatterTraitListForm<Thing, Thing>.ShowInstantce("Children of:", _thing, x => x.GetChildren().ToArray(), null, x => MatterInfoForm.ShowInstantce(x));
        }
        private void llblMultiChildrenCount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!int.TryParse(((Control)sender).Text, out var count) || count <= 0)
                return;
            MatterTraitListForm<Thing, Thing>.ShowInstantce("Multi-Children of:", _thing, x => x.GetChildren(y => y > 1).ToArray(), null, x => MatterInfoForm.ShowInstantce(x));
        }
    }
}