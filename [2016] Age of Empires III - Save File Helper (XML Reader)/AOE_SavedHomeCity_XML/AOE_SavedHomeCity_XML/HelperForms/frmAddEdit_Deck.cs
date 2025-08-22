using AOE3_HomeCity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AOE3_HomeCity.HelperForms
{
    public partial class frmAddEdit_Deck : Form
    {
        public frmAddEdit_Deck(Tech[] activeTechs, bool gameIdExists)
        {
            if (activeTechs == null)
                throw new ArgumentException("activeTechs");
            InitializeComponent();
            _entity = null;
            _activeTechs = activeTechs;
            _formType = FormType.Add;
            _specialTexts = false;
            _gameIdExists = gameIdExists;
            _cancelClosing = false;
        }
        public frmAddEdit_Deck(Deck deck, Tech[] activeTechs)
        {
            if (activeTechs == null)
                throw new ArgumentException("activeTechs");
            InitializeComponent();
            _entity = deck;
            _formType = FormType.Edit;
            _specialTexts = false;
            _activeTechs = activeTechs;
            _gameIdExists = deck.GameID_Exists;
            _cancelClosing = false;
        }


        private Deck _entity;
        public Deck RetvalEntity
        {
            get { return _entity; }
        }

        private FormType _formType;
        public FormType Type
        {
            get { return _formType; }
        }

        private bool _specialTexts;
        public bool SpecialTexts
        {
            set { _specialTexts = value; }
        }

        private Tech[] _activeTechs;
        private bool _gameIdExists;
        private bool _cancelClosing;



        private void frmAddEdit_Deck_Load(object sender, EventArgs e)
        {
            RestoreDefaults();
            if (_formType == FormType.Edit)
            {
                if (!(chcGameID.Checked = !_entity.GameID.HasValue))
                    nudGameID.Value = _entity.GameID.Value;
                if (!_specialTexts)
                    txtName.Text = _entity.Name == null ? string.Empty : _entity.Name;
                if ((llblCards_deleteAll.Enabled = _entity.Cards != null && _entity.Cards.Length > 0))
                    lbxCards.Items.AddRange(_entity.Cards);
                chcGameIDExists.Checked = (_gameIdExists = _entity.GameID_Exists);
            }
            lblCards.Text = string.Format("Cards ({0})", lbxCards.Items.Count);
            llblCards_add.Enabled = _activeTechs.Where(x => !lbxCards.Items.Cast<Tech>().Includes(x, Helper.EqualityType.UniqueIdentifierEquals)).Count() > 0;
        }
        private void frmAddEdit_Deck_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cancelClosing)
            {
                _cancelClosing = false;
                e.Cancel = true;
            }
        }

        private void chcEmpty_CheckedChanged(object sender, EventArgs e)
        {
            var chc = sender as CheckBox;
            var ctrl = chc.Tag as Control;
            var nud = ctrl as NumericUpDown;
            if ((ctrl.Enabled = !chc.Checked))
                ctrl.Text = (ctrl as NumericUpDown).Minimum.ToString();
            else
                ctrl.ResetText();
        }
        private void lbxCards_SelectedIndexChanged(object sender, EventArgs e)
        {
            llblCards_delete.Enabled = lbxCards.SelectedIndices.Count > 0;
        }

        private void llblCards_add_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var techs = _activeTechs.Where(x => !lbxCards.Items.Cast<Tech>().Includes(x, Helper.EqualityType.UniqueIdentifierEquals));
            Tech[] selecteds = null;
            var frm = new frmSelector<Tech>(techs, "Select Techs to Add as Cards");
            if (frm.ShowDialog() != DialogResult.OK || ((selecteds = frm.Selecteds) == null || selecteds.Length == 0))
                return;
            int total = lbxCards.Items.Count + selecteds.Length;
            var cards = lbxCards.Items.Cast<Tech>().ToArray();
            lbxCards.Items.AddRange(selecteds.Excluding(cards, Helper.EqualityType.UniqueIdentifierEquals).ToArray());
            int currCount = lbxCards.Items.Count;
            if (currCount < total)
                Helper.ShowMessageBox(
                    PrimaryMessage: string.Format("{0} input(s) could not be added because the same values with their identifiers already exist in the list.", total - currCount),
                    Icon: MessageBoxIcon.Warning);
            llblCards_add.Enabled = _activeTechs.Where(x => !lbxCards.Items.Cast<Tech>().Includes(x, Helper.EqualityType.UniqueIdentifierEquals)).Count() > 0;
            llblCards_deleteAll.Enabled = lbxCards.Items.Count > 0;
            lblCards.Text = string.Format("Cards ({0})", currCount);

            //var techs = _activeTechs.Where(x => !lbxCards.Items.Cast<Tech>().Contains(x));
            //var frm = new frmSelector<Tech>(techs, "Select Techs to Add as Cards");
            //Tech[] selecteds = null;
            //if (frm.ShowDialog() != DialogResult.OK || ((selecteds = frm.Selecteds) == null || selecteds.Length == 0))
            //    return;
            //var cards = lbxCards.Items.Cast<Tech>();
            //bool missing = false;
            //foreach (var tech in selecteds)
            //{
            //    var exists = false;
            //    foreach (var card in cards)
            //        if (card.UniqueIdEquals(tech))
            //        {
            //            exists = true;
            //            missing = true;
            //            break;
            //        }
            //    if (!exists)
            //        lbxCards.Items.Add(tech);
            //}
            //if (missing)
            //    Helper.ShowMessageBox(PrimaryMessage: "Some inputs could not be added because the same values with their identifiers already exist.",
            //        Icon: MessageBoxIcon.Warning);
            //llblCards_deleteAll.Enabled = lbxCards.Items.Count > 0;
            //lblCards.Text = string.Format("Cards ({0})", lbxCards.Items.Count);
        }
        private void llblCards_delete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var indices = lbxCards.SelectedIndices.Cast<int>().OrderByDescending(x => x);
            foreach (var index in indices)
                lbxCards.Items.RemoveAt(index);
            llblCards_deleteAll.Enabled = lbxCards.Items.Count > 0;
            lblCards.Text = string.Format("Cards ({0})", lbxCards.Items.Count);
            llblCards_add.Enabled = _activeTechs.Where(x => !lbxCards.Items.Cast<Tech>().Includes(x, Helper.EqualityType.UniqueIdentifierEquals)).Count() > 0;
        }
        private void llblCards_deleteAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lbxCards.Items.Clear();
            llblCards_deleteAll.Enabled = false;
            lblCards.Text = string.Format("Cards ({0})", lbxCards.Items.Count);
            llblCards_add.Enabled = _activeTechs.Where(x => !lbxCards.Items.Cast<Tech>().Includes(x, Helper.EqualityType.UniqueIdentifierEquals)).Count() > 0;
        }

        private void chcGameIDExists_CheckedChanged(object sender, EventArgs e)
        {
            if (!(_gameIdExists = chcGameIDExists.Checked))
                chcGameID.Checked = false;
            lblGameID.Visible = _gameIdExists;
            nudGameID.Visible = _gameIdExists;
            chcGameID.Visible = _gameIdExists;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var deck = GetEntityByControls();
            if (deck.IsEmpty &&
                MessageBox.Show("Are you sure to save as empty?", "Empty Entity", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                _cancelClosing = true;
                return;
            }
            _entity = deck;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _entity = null;
            Close();
        }

        private Deck GetEntityByControls()
        {
            return new Deck()
            {
                GameID = chcGameID.Checked ? null : (uint?)nudGameID.Value,
                Name = txtName.Text.Trim(),
                Cards = lbxCards.Items.Cast<Tech>().ToArray(),
                GameID_Exists = chcGameIDExists.Checked
            };
        }
        private void RestoreDefaults()
        {
            if (!_specialTexts)
            {
                Text = string.Format("{0} {1}", (btnOK.Text = _formType.ToString()), Text);
                txtName.Text = string.Empty;
            }
            chcGameID.Tag = nudGameID;
            chcGameID.Checked = false;
            lbxCards.Items.Clear();
            llblCards_add.Enabled = _activeTechs.Length > 0;
            llblCards_delete.Enabled = false;
            llblCards_deleteAll.Enabled = false;

            lblGameID.Visible = _gameIdExists;
            nudGameID.Visible = _gameIdExists;
            chcGameID.Visible = _gameIdExists;
            chcGameIDExists.Checked = _gameIdExists;
        }
    }
}