using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    internal partial class MatterInfoForm : Form
    {
        static MatterInfoForm()
        {
            _locker = new object();
            _defaultSize = new Size(590, 327);
            _instances = new List<MatterInfoForm>();
            Program.ApplicationModelChanged += Program_ApplicationModelChanged;
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
        }
        private MatterInfoForm(Matter matter)
        {
            _matter = matter ?? throw new ArgumentNullException(nameof(matter));
            _userControl = null;
            InitializeComponent();
            Hide();
            RepresentMatterInfo(true);
        }


        private static readonly object _locker;
        private static readonly Size _defaultSize;
        private static readonly List<MatterInfoForm> _instances;

        private readonly Matter _matter;
        private UserControl _userControl;



        public static void ShowInstantce(Matter matter)
        {
            lock (_locker)
            {
                if (matter == null)
                    throw new ArgumentNullException(nameof(matter));
                var frm = _instances.FirstOrDefault(x => x._matter.OwnerUniverse == matter.OwnerUniverse && x._matter.Id.Equals(matter.Id));
                if (frm != null)
                {
                    frm.RepresentMatterInfo();
                    return;
                }
                _instances.Add(new MatterInfoForm(matter));
            }
        }
        public static void ClearInstances()
        {
            lock (_locker)
            {
                var instances = _instances.ToArray();
                foreach (var frm in instances)
                {
                    frm.Close();
                    frm.Dispose();
                }
                _instances.TrimExcess();
            }
        }

        private void RepresentMatterInfo(bool centerToScreen = false)
        {
            pnlUserControlContainer.Controls.Clear();
            _userControl?.Dispose();
            _userControl = null;
            MinimumSize = MaximumSize = _defaultSize;
            var type = _matter.GetType().Name;
            Text = $"{type} {_matter.Location}";
            lblCaption.Text = $"{type} Info";
            lblId.Text = _matter.Id.ToString();
            lblLocation.Text = _matter.Location.ToString();
            lblIsSpawned.Text = _matter.IsSpawned ? "Yes" : "No";
            lblGeneration.Text = _matter.Generation.ToString();
            lblVanishingGeneration.Text = _matter.VanishingGeneration?.ToString();
            switch (_matter.DeathReason)
            {
                case DeathReasons.ByChance:
                    lblDeathReason.Text = "By Chance";
                    break;
                case DeathReasons.Starved:
                    lblDeathReason.Text = "Starved";
                    break;
                case DeathReasons.FoodReplacedToThing:
                case DeathReasons.ThingReplacedToThing:
                    lblDeathReason.Text = "Replaced with a new-born Thing";
                    break;
                case DeathReasons.ThingReplacedToFood:
                    lblDeathReason.Text = "Replaced with a new-born Food";
                    break;
                case DeathReasons.DueToSimulation:
                    lblDeathReason.Text = "Due to Simulation";
                    break;
                case DeathReasons.Eaten:
                    lblDeathReason.Text = "Eaten";
                    break;
                default:
                    lblDeathReason.Text = string.Empty;
                    break;
            }
            llblReplacedTo.Text = _matter.TryGetReplacedAndVanished(out var replacedTo) ? replacedTo.Id.ToString() : string.Empty;
            int count;
            llblPreviousLocations.Text = (count = _matter.LocationsCount) == 0 ? string.Empty : count.ToString();
            if (_matter is Food f)
            {
                var uc = new FoodInfoUserControl();
                uc.RepresentInfo(f);
                _userControl = uc;
            }
            else if (_matter is Thing t)
            {
                var uc = new ThingInfoUserControl();
                uc.RepresentInfo(t);
                _userControl = uc;
            }
            if (_userControl == null)
                return;
            SuspendLayout();
            pnlUserControlContainer.SuspendLayout();
            pnlUserControlContainer.Controls.Add(_userControl);
            _userControl.Location = new Point(0, 0);
            pnlUserControlContainer.ResumeLayout(false);
            ResumeLayout(false);
            MinimumSize = MaximumSize = new Size(_userControl.Width <= _defaultSize.Width ? _defaultSize.Width : _userControl.Width, _defaultSize.Height + _userControl.Height);
            if (centerToScreen)
                CenterToScreen();
            Show();
            Focus();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            lock (_locker)
                _instances.RemoveAt(_instances.FindIndex(x => x == this || (x._matter.OwnerUniverse == _matter.OwnerUniverse && x._matter.Id.Equals(_matter.Id))));
            base.OnFormClosed(e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lock (_locker)
            {
                Hide();
                RepresentMatterInfo();
            }
        }
        private void llblMatterId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var text = ((LinkLabel)sender).Text;
            if (!string.IsNullOrEmpty(text) && Guid.TryParse(text, out var id))
                ShowInstantce(_matter.OwnerUniverse.AllMattersEver[id]);
        }
        private void llblLocations_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!int.TryParse(((Control)sender).Text, out var count) || count <= 0)
                return;
            MatterTraitListForm<Matter, (uint, Location)>.ShowInstantce("Previous Locations of:", _matter, x => x.GetLocations().OrderBy(y => y.Generation).ToArray(), x => $"Generation: {x.Item1} | Location: {x.Item2}", loc =>
            {
                var id = _matter.OwnerUniverse.GetMatterId(loc.Item2);
                if (id.HasValue)
                    ShowInstantce(_matter.OwnerUniverse.AllMattersEver[id.Value]);
            });
        }

        private static void Program_ApplicationModelChanged(object sender, EventArgs e)
        {
            ClearInstances();
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
        }
        private static void ApplicationModel_UniverseChanged(object sender, EventArgs e) => ClearInstances();
    }
}