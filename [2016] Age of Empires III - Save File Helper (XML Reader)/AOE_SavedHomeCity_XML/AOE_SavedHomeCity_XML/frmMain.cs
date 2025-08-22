using AOE3_HomeCity.Entities;
using AOE3_HomeCity.HelperForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AOE3_HomeCity
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            _defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Age of Empires 3", "campaign");
            _loadedEntity = null;
            _gameIdExists = true;
        }


        private string _defaultPath;
        private SavedHomeCity _loadedEntity;
        private bool _gameIdExists;

        private bool RespecExists
        {
            get { return pnlRespec.Visible; }
        }

        private string CurrentFileName
        {
            get { return string.IsNullOrEmpty(lblFileName.Text) ? string.Empty : Path.Combine(txtDirectory.Text, lblFileName.Text); }
        }



        private void frmMain_Load(object sender, EventArgs e)
        {
            txtDirectory.Text = _defaultPath;
            txtDirectory.SelectionStart = txtDirectory.Text.Length;
            RestoreDefaultsForControls();
        }

        private void chcLast_CheckedChanged(object sender, EventArgs e)
        {
            RestoreDefaults();
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
        private void lbxTechs_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnTechs_delete.Enabled = lbxTechs.SelectedIndices.Count > 0;
            btnTechs_edit.Enabled = lbxTechs.SelectedIndices.Count == 1;
            btnCards_add.Enabled = lbxTechs.SelectedIndices.Count > 0 && lbxDecks.SelectedIndices.Count == 1;
        }
        private void lbxProps_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnProps_delete.Enabled = lbxProps.SelectedIndices.Count > 0;
            btnProps_edit.Enabled = lbxProps.SelectedIndices.Count == 1;
        }
        private void lbxDecks_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblHeader_cards.Text = "Cards of Selected Deck";
            btnDecks_delete.Enabled = lbxDecks.SelectedIndices.Count > 0;
            btnDecks_edit.Enabled = lbxDecks.SelectedIndices.Count == 1;
            btnDecks_copy.Enabled = lbxDecks.SelectedIndices.Count == 1;
            lbxCards.Items.Clear();
            if ((lbxCards.Enabled = lbxDecks.SelectedIndices.Count == 1))
            {
                lbxCards.Items.AddRange(((Deck)lbxDecks.SelectedItem).Cards.ToArray());
                lblHeader_cards.Text += string.Format(" ({0})", lbxCards.Items.Count.ToString());
            }
            btnCards_add.Enabled = lbxCards.Enabled && lbxTechs.SelectedIndices.Count > 0 && lbxDecks.SelectedIndices.Count == 1;
            btnCards_delete.Enabled = false;
        }
        private void lbxCards_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCards_add.Enabled = lbxTechs.SelectedIndices.Count > 0 && lbxDecks.SelectedIndices.Count == 1;
            btnCards_delete.Enabled = lbxCards.SelectedIndices.Count > 0;
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            (new AboutBox1()).ShowDialog();
        }
        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Directory.Exists(txtDirectory.Text) ? txtDirectory.Text : (Directory.Exists(_defaultPath) ? _defaultPath : string.Empty);
            while (true)
            {
                openFileDialog1.FileName = "*.xml";
                var dr = DialogResult.None;
                if (openFileDialog1.ShowDialog() != DialogResult.OK || (!Helper.IsXmlFile(openFileDialog1.FileName) &&
                    (dr = MessageBox.Show("Selected file was not an XML file!", "Warning",
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning)) == DialogResult.Cancel))
                {
                    chcLast.Checked = true;
                    break;
                }
                else if (dr != DialogResult.Retry)
                {
                    if (!chcLast.Checked)
                        RestoreDefaults();
                    else
                        chcLast.Checked = false;
                    txtDirectory.Text = Path.GetDirectoryName(openFileDialog1.FileName);
                    lblFileName.Text = Path.GetFileName(openFileDialog1.FileName);
                    break;
                }
            }
        }
        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            while (true)
            {
                var dr = DialogResult.None;
                if (folderBrowserDialog1.ShowDialog() != DialogResult.OK || (!Helper.DoesContainXmlFile(folderBrowserDialog1.SelectedPath) &&
                    (dr = MessageBox.Show("There was not any XML file in selected directory!", "Warning",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning)) == DialogResult.Cancel))
                    break;
                else if (dr != DialogResult.Retry)
                {
                    txtDirectory.Text = folderBrowserDialog1.SelectedPath;
                    if (chcLast.Checked)
                        RestoreDefaults();
                    else
                        chcLast.Checked = true;
                    break;
                }
            }
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            #region . File Validation .
            bool ok = true;
            string msg = "", file = "";
            if (!chcLast.Checked)
            {
                if (string.IsNullOrEmpty(lblFileName.Text))
                {
                    msg = "Please define a save file.";
                    ok = false;
                }
                else if (!File.Exists((file = CurrentFileName)))
                {
                    msg = "The file you definied does not exist!";
                    ok = false;
                }
                else if (!Helper.IsXmlFile(file))
                {
                    msg = "The file you definied is invalid!";
                    ok = false;
                }
            }
            else
            {
                if (!Directory.Exists(txtDirectory.Text))
                {
                    msg = "The folder does not exist!";
                    ok = false;
                }
                else if (!Helper.DoesContainXmlFile(txtDirectory.Text))
                {
                    msg = "There was not any XML file in selected directory!";
                    ok = false;
                }
                lblFileName.Text = Path.GetFileName(Helper.GetLastModifiedXmlFile(txtDirectory.Text));
                file = CurrentFileName;
            }
            if (!ok)
            {
                Helper.ShowMessageBox(PrimaryMessage: msg, Icon: MessageBoxIcon.Warning);
                return;
            }
            #endregion
            try
            {
                _gameIdExists = ((_loadedEntity = SavedHomeCity.FromXml(Helper.ReadText(file))).Decks == null || _loadedEntity.Decks.Length == 0) ?
                    (Helper.ShowMessageBox(Title: "Important Information Required", Buttons: MessageBoxButtons.YesNo, Icon: MessageBoxIcon.Exclamation, PrimaryMessage: "If you are playing the game without any of its expantion packs, or you are playing Extended Edition, please click YES.\nIf not, please click NO.") == DialogResult.Yes) : 
                    _loadedEntity.Decks.First().GameID_Exists;
                RestoreDefaultsForControls();
                SetControlsByEntity(_loadedEntity);
                pnlDependencies.Enabled = true;

                grbActiveTechs.Text = string.Format("Active Techs ({0})", lbxTechs.Items.Count.ToString());
                grbActiveProps.Text = string.Format("Active Props ({0})", lbxProps.Items.Count.ToString());
                lblHeader_decks.Text = string.Format("Decks ({0})", lbxDecks.Items.Count.ToString());
            }
            catch (Exception ex)
            {
                Helper.ShowMessageBox(ex);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chcBackup.Checked &&
                    MessageBox.Show("Do you want to backup the current file while it will be overwritten?", "Backup",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    chcBackup.Checked = true;
                string file = CurrentFileName;
                string msg = "New dependecies are successfuly saved";
                var curr = GetEntityFromControls();
                if (chcBackup.Checked)
                {
                    string backup = Path.Combine(Path.GetDirectoryName(file), 
                        Path.GetFileNameWithoutExtension(file) + "_backup" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file));
                    Helper.WriteText(_loadedEntity.ToXml(), backup);
                    msg += string.Format(" and backup file is created at:\n{0}", backup);
                }
                Helper.WriteText(curr.ToXml(), file);
                _loadedEntity = curr;
                MessageBox.Show(msg + ".", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Helper.ShowMessageBox(ex);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            _loadedEntity = null;
            lblFileName.Text = string.Empty;
            RestoreDefaultsForControls();
        }

        private void btnTechs_add_Click(object sender, EventArgs e)
        {
            var frm = new frmAddEdit_Tech();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var allItems = lbxTechs.Items.Cast<Tech>().ToList();
                int index = -1, currIndex = lbxTechs.SelectedIndex;
                if (allItems.Count > 0)
                    for (int i = 0; i < allItems.Count; i++)
                        if (allItems[i].UniqueIdEquals(frm.RetvalEntity))
                        {
                            index = i;
                            break;
                        }
                if (index != -1)
                {
                    Helper.ShowMessageBox(PrimaryMessage: "An input with the same identifier already exsits; it will be highligted...", 
                        Icon: MessageBoxIcon.Warning);
                    if (currIndex != -1)
                        lbxTechs.SetSelected(currIndex, false);
                    lbxTechs.SelectedIndex = index;
                    return;
                }
                lbxTechs.Items.Add(frm.RetvalEntity);
                grbActiveTechs.Text = string.Format("Active Techs ({0})", lbxTechs.Items.Count.ToString());
            }
        }
        private void btnTechs_edit_Click(object sender, EventArgs e)
        {
            var frm = new frmAddEdit_Tech((Tech)lbxTechs.SelectedItem);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var allItems = lbxTechs.Items.Cast<Tech>().ToList();
                int index = -1, currIndex = lbxTechs.SelectedIndex;
                if (allItems.Count > 1)
                    for (int i = 0; i < allItems.Count; i++)
                        if (i != currIndex && allItems[i].UniqueIdEquals(frm.RetvalEntity))
                        {
                            index = i;// < currIndex ? i : i - 1;
                            break;
                        }
                if (index != -1)
                {
                    Helper.ShowMessageBox(PrimaryMessage:
                        "The input could not be edited because another input with the same identifier already exsits; it will be highligted...",
                        Icon: MessageBoxIcon.Warning);
                    //lbxTechs.Items.RemoveAt(currIndex);
                    lbxTechs.SelectedIndex = index;
                    return;
                }
                lbxTechs.Items[currIndex] = frm.RetvalEntity;
            }
        }
        private void btnTechs_delete_Click(object sender, EventArgs e)
        {
            var indices = lbxTechs.SelectedIndices.Cast<int>().OrderByDescending(x => x);
            foreach (var index in indices)
            {
                foreach (Deck deck in lbxDecks.Items)
                    if (deck.Cards != null)
                        deck.Cards = (from card in deck.Cards
                                      let tech = lbxTechs.Items[index] as Tech
                                      where !card.ExactEquals(tech)
                                      select card).ToArray();
                lbxTechs.Items.RemoveAt(index);
            }

            //var allItems0 = lbxTechs.Items.Cast<Tech>().ToList();
            //var indices0 = new List<int>();
            //foreach (var tech in selection)
            //{
            //    for (int i = 0; i < allItems0.Count; i++)
            //        if (allItems0[i].ExactEquals(tech))
            //            indices0.Add(i);
            //    foreach (Deck deck in lbxDecks.Items)
            //        if (deck.Cards != null)
            //        {
            //            var allItems1 = deck.Cards.ToList();
            //            var indices1 = new List<int>();
            //            for (int i = 0; i < allItems1.Count; i++)
            //                if (allItems1[i].ExactEquals(tech))
            //                    indices1.Add(i);
            //            indices1.TrimExcess();
            //            indices1 = indices1.OrderByDescending(x => x).ToList();
            //            foreach (var index1 in indices1)
            //                allItems1.RemoveAt(index1);
            //            deck.Cards = allItems1.ToArray();
            //        }
            //}
            //indices0.TrimExcess();
            //indices0 = indices0.OrderByDescending(x => x).ToList();
            //foreach (var index0 in indices0)
            //    allItems0.RemoveAt(index0);
            //lbxTechs.SelectedIndex = -1;
            //lbxTechs.Items.Clear();
            //lbxTechs.Items.AddRange(allItems0.ToArray());
            
            int indexDecks = lbxDecks.SelectedIndex;
            lbxDecks.SelectedIndex = -1;
            lbxDecks.SelectedIndex = indexDecks;
            grbActiveTechs.Text = string.Format("Active Techs ({0})", lbxTechs.Items.Count.ToString());
        }
        private void btnProps_add_Click(object sender, EventArgs e)
        {
            var frm = new frmAddEdit_Prop();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var allItems = lbxProps.Items.Cast<Prop>().ToList();
                int index = -1, currIndex = lbxProps.SelectedIndex;
                if (allItems.Count > 0)
                    for (int i = 0; i < allItems.Count; i++)
                        if (allItems[i].UniqueIdEquals(frm.RetvalEntity))
                        {
                            index = i;
                            break;
                        }
                if (index != -1)
                {
                    Helper.ShowMessageBox(PrimaryMessage: "An input with the same identifier already exsits; it will be highligted...",
                        Icon: MessageBoxIcon.Warning);
                    if (currIndex != -1)
                        lbxProps.SetSelected(currIndex, false);
                    lbxProps.SelectedIndex = index;
                    return;
                }
                lbxProps.Items.Add(frm.RetvalEntity);
                grbActiveProps.Text = string.Format("Active Props ({0})", lbxProps.Items.Count.ToString());
            }
        }
        private void btnProps_edit_Click(object sender, EventArgs e)
        {
            var frm = new frmAddEdit_Prop((Prop)lbxProps.SelectedItem);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var allItems = lbxProps.Items.Cast<Prop>().ToList();
                int index = -1, currIndex = lbxProps.SelectedIndex;
                if (allItems.Count > 1)
                    for (int i = 0; i < allItems.Count; i++)
                        if (i != currIndex && allItems[i].UniqueIdEquals(frm.RetvalEntity))
                        {
                            index = i;// < currIndex ? i : i - 1;
                            break;
                        }
                if (index != -1)
                {
                    Helper.ShowMessageBox(PrimaryMessage:
                        "The input could not be edited because another input with the same identifier already exsits; it will be highligted...",
                        Icon: MessageBoxIcon.Warning);
                    //lbxProps.Items.RemoveAt(currIndex);
                    lbxProps.SelectedIndex = index;
                    return;
                }
                lbxProps.Items[currIndex] = frm.RetvalEntity;
            }
        }
        private void btnProps_delete_Click(object sender, EventArgs e)
        {
            var indices = lbxProps.SelectedIndices.Cast<int>().OrderByDescending(x => x);
            foreach (var index in indices)
                lbxProps.Items.RemoveAt(index);
            //var selection = lbxProps.SelectedItems.Cast<Prop>();
            //var allItems = lbxProps.Items.Cast<Prop>().ToList();
            //var indices = new List<int>();
            //foreach (var prop in selection)
            //    for (int i = 0; i < allItems.Count; i++)
            //        if (allItems[i].ExactEquals(prop))
            //            indices.Add(i);
            //indices.TrimExcess();
            //indices = indices.OrderByDescending(x => x).ToList();
            //foreach (var index in indices)
            //    allItems.RemoveAt(index);
            //lbxProps.SelectedIndex = -1;
            //lbxProps.Items.Clear();
            //lbxProps.Items.AddRange(allItems.ToArray());
            grbActiveProps.Text = string.Format("Active Props ({0})", lbxProps.Items.Count.ToString());
        }
        private void btnDecks_add_Click(object sender, EventArgs e)
        {
            var frm = new frmAddEdit_Deck(lbxTechs.Items.Cast<Tech>().ToArray(), _gameIdExists);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var allItems = lbxDecks.Items.Cast<Deck>().ToList();
                int index = -1, currIndex = lbxDecks.SelectedIndex;
                if (allItems.Count > 0)
                    for (int i = 0; i < allItems.Count; i++)
                        if (allItems[i].UniqueIdEquals(frm.RetvalEntity))
                        {
                            index = i;
                            break;
                        }
                if (index != -1)
                {
                    Helper.ShowMessageBox(PrimaryMessage: "An input with the same identifier already exsits; it will be highligted...",
                        Icon: MessageBoxIcon.Warning);
                    if (currIndex != -1)
                        lbxDecks.SetSelected(currIndex, false);
                    lbxDecks.SelectedIndex = index;
                    return;
                }
                lbxDecks.Items.Add(frm.RetvalEntity);
                lblHeader_decks.Text = string.Format("Decks ({0})", lbxDecks.Items.Count.ToString());
            }
        }
        private void btnDecks_edit_Click(object sender, EventArgs e)
        {
            var frm = new frmAddEdit_Deck((Deck)lbxDecks.SelectedItem, lbxTechs.Items.Cast<Tech>().ToArray());
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var allItems = lbxDecks.Items.Cast<Deck>().ToList();
                int index = -1, currIndex = lbxDecks.SelectedIndex;
                if (allItems.Count > 1)
                    for (int i = 0; i < allItems.Count; i++)
                        if (i != currIndex && allItems[i].UniqueIdEquals(frm.RetvalEntity))
                        {
                            index = i;
                            break;
                        }
                if (index != -1)
                {
                    Helper.ShowMessageBox(PrimaryMessage:
                        "The input could not be edited because another input with the same identifier already exsits; it will be highligted...",
                        Icon: MessageBoxIcon.Warning);
                    lbxDecks.SelectedIndex = index;
                    return;
                }
                if (currIndex != -1)
                    lbxDecks.SetSelected(currIndex, false);
                lbxDecks.Items[currIndex] = frm.RetvalEntity;
                lbxDecks.SelectedIndex = currIndex;
            }
        }
        private void btnDecks_copy_Click(object sender, EventArgs e)
        {
            var frm = new frmAddEdit_Deck((Deck)lbxDecks.SelectedItem, lbxTechs.Items.Cast<Tech>().ToArray()) { SpecialTexts = true };            
            frm.Text = "Add Deck";
            frm.btnOK.Text = "Add";
            frm.txtName.Text = "New Deck";
            frm.txtName.SelectAll();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var allItems = lbxDecks.Items.Cast<Deck>().ToList();
                int index = -1, currIndex = lbxDecks.SelectedIndex;
                if (allItems.Count > 0)
                    for (int i = 0; i < allItems.Count; i++)
                        if (allItems[i].UniqueIdEquals(frm.RetvalEntity))
                        {
                            index = i;
                            break;
                        }
                if (index != -1)
                {
                    Helper.ShowMessageBox(PrimaryMessage: "An input with the same identifier already exsits; it will be highligted...",
                        Icon: MessageBoxIcon.Warning);
                    if (currIndex != -1)
                        lbxDecks.SetSelected(currIndex, false);
                    lbxDecks.SelectedIndex = index;
                    return;
                }
                lbxDecks.Items.Add(frm.RetvalEntity);
                lblHeader_decks.Text = string.Format("Decks ({0})", lbxDecks.Items.Count.ToString());
            }
        }
        private void btnDecks_delete_Click(object sender, EventArgs e)
        {
            var indices = lbxDecks.SelectedIndices.Cast<int>().OrderByDescending(x => x);
            foreach (var index in indices)
                lbxDecks.Items.RemoveAt(index);
            //var selection = lbxDecks.SelectedItems.Cast<Deck>();
            //var allItems = lbxDecks.Items.Cast<Deck>().ToList();
            //var indices = new List<int>();
            //foreach (var deck in selection)
            //    for (int i = 0; i < allItems.Count; i++)
            //        if (allItems[i].ExactEquals(deck))
            //            indices.Add(i);
            //indices.TrimExcess();
            //indices = indices.OrderByDescending(x => x).ToList();
            //foreach (var index in indices)
            //    allItems.RemoveAt(index);
            //lbxDecks.SelectedIndex = -1;
            //lbxDecks.Items.Clear();
            //lbxDecks.Items.AddRange(allItems.ToArray());
            lblHeader_decks.Text = string.Format("Decks ({0})", lbxDecks.Items.Count.ToString());
        }
        private void btnCards_add_Click(object sender, EventArgs e)
        {
            int total = lbxCards.Items.Count + lbxTechs.SelectedItems.Count;

            int decksIndex = lbxDecks.SelectedIndex;
            var deck = lbxDecks.Items[decksIndex] as Deck;
            lbxDecks.SelectedIndex = -1;
            
            var cards = new List<Tech>(deck.Cards);
            cards.AddRange(lbxTechs.SelectedItems.Cast<Tech>().Excluding(deck.Cards, Helper.EqualityType.UniqueIdentifierEquals));
            cards.TrimExcess();
            deck.Cards = cards.ToArray();
            
            lbxDecks.SelectedIndex = decksIndex;

            int currCount = lbxCards.Items.Count;
            if (currCount < total)
                Helper.ShowMessageBox(
                    PrimaryMessage: string.Format("{0} input(s) could not be added because the same values with their identifiers already exist in the list.", total - currCount),
                    Icon: MessageBoxIcon.Warning);

            lblHeader_cards.Text = string.Format("Cards of Selected Deck ({0})", lbxCards.Items.Count.ToString());

            //int decksIndex = lbxDecks.SelectedIndex;
            //var deck = (Deck)lbxDecks.SelectedItem;
            //lbxDecks.SelectedIndex = -1;
            //bool missing = false;

            //var techs = lbxTechs.SelectedItems.Cast<Tech>();
            //var cards = deck.Cards == null ? new List<Tech>() : deck.Cards.ToList();
            //foreach (var tech in techs)
            //{
            //    bool exists = false;
            //    foreach (var card in cards)
            //        if (card.UniqueIdEquals(tech))
            //        {
            //            exists = true;
            //            missing = true;
            //            break;
            //        }
            //    if (!exists)
            //        cards.Add(tech);
            //}
            //cards.TrimExcess();
            //deck.Cards = cards.ToArray();

            //lbxDecks.SelectedIndex = decksIndex;

            //if (missing)
            //    Helper.ShowMessageBox(
            //        PrimaryMessage: "Some inputs could not be added because the same values with their identifiers already exist.", 
            //        Icon: MessageBoxIcon.Warning);
            //lblHeader_cards.Text = string.Format("Cards of Selected Deck ({0})", lbxCards.Items.Count.ToString());
        }
        private void btnCards_delete_Click(object sender, EventArgs e)
        {
            int decksIndex = lbxDecks.SelectedIndex;
            var deck = (Deck)lbxDecks.SelectedItem;

            var cards = deck.Cards.ToList();
            var indices = lbxCards.SelectedIndices.Cast<int>().OrderByDescending(x => x);
            foreach (var index in indices)
                cards.RemoveAt(index);
            cards.TrimExcess();
            deck.Cards = cards.ToArray();

            //var selection = lbxCards.SelectedItems.Cast<Tech>();
            //var allItems = deck.Cards.ToList();

            //var indices = new List<int>();
            //foreach (var card in selection)
            //    for (int i = 0; i < allItems.Count; i++)
            //        if (allItems[i].ExactEquals(card))
            //            indices.Add(i);
            //indices.TrimExcess();
            //indices = indices.OrderByDescending(x => x).ToList();

            //foreach (var index in indices)
            //    allItems.RemoveAt(index);
            //deck.Cards = allItems.ToArray();

            lbxDecks.SelectedIndex = -1;
            lbxDecks.SelectedIndex = decksIndex;
            lblHeader_cards.Text = string.Format("Cards of Selected Deck ({0})", lbxCards.Items.Count.ToString());
        }

        private void SetControlsByEntity(SavedHomeCity hc)
        {
            if (hc == null)
                throw new ArgumentNullException("hc");

            pnlRespec.Visible = (lblRespec.Visible = hc.Respec_Exists);

            if (!(chcVersion.Checked = !hc.Version.HasValue))
                nudVersion.Value = hc.Version.Value;
            if (!(chcDefaultDirectoryID.Checked = !hc.DefaultDirectoryID.HasValue))
                nudDefaultDirectoryID.Value = hc.DefaultDirectoryID.Value;
            txtDefaultFileName.Text = hc.DefaultFileName == null ? string.Empty : hc.DefaultFileName;
            txtCivilization.Text = hc.Civilization == null ? string.Empty : hc.Civilization;
            txtHomeCityType.Text = hc.HomeCityType == null ? string.Empty : hc.HomeCityType;
            txtName.Text = hc.Name == null ? string.Empty : hc.Name;
            txtHeroName.Text = hc.HeroName == null ? string.Empty : hc.HeroName;
            txtHeroDogName.Text = hc.HeroDogName == null ? string.Empty : hc.HeroDogName;
            txtShipName.Text = hc.ShipName == null ? string.Empty : hc.ShipName;
            if (!(chcHomeCityID.Checked = !hc.HomeCityID.HasValue))
                nudHomeCityID.Value = hc.HomeCityID.Value;
            if (!(radRespec_empty.Checked = !hc.Respec.HasValue))
            {
                radRespec_true.Checked = hc.Respec.Value.BooleanValue;
                radRespec_false.Checked = !radRespec_true.Checked;
            }
            if (!(chcLevel.Checked = !hc.Level.HasValue))
                nudLevel.Value = hc.Level.Value;
            if (!(chcXp.Checked = !hc.XP.HasValue))
                nudXp.Value = hc.XP.Value;
            if (!(chcSkillPoints.Checked = !hc.SkillPoints.HasValue))
                nudSkillPoints.Value = hc.SkillPoints.Value;
            if (!(chcXpPercentage.Checked = !hc.XPPercentage.HasValue))
                nudXpPercentage.Value = hc.XPPercentage.Value.Value;
            if (!(chcNumPropUnlocksEarned.Checked = !hc.NumPropUnlocksEarned.HasValue))
                nudNumPropUnlocksEarned.Value = hc.NumPropUnlocksEarned.Value;

            lbxTechs.Items.Clear();
            if (hc.ActiveTechs != null)
                lbxTechs.Items.AddRange(hc.ActiveTechs);

            lbxProps.Items.Clear();
            if (hc.ActiveProps != null)
                lbxProps.Items.AddRange(hc.ActiveProps);

            lbxDecks.Items.Clear();
            if (hc.Decks != null)
                lbxDecks.Items.AddRange(hc.Decks);
        }
        private SavedHomeCity GetEntityFromControls()
        {
            return new SavedHomeCity()
            {
                ActiveProps = lbxProps.Items.Cast<Prop>().ToArray(),
                ActiveTechs = lbxTechs.Items.Cast<Tech>().ToArray(),
                Civilization = txtCivilization.Text.Trim(),
                Decks = lbxDecks.Items.Cast<Deck>().ToArray(),
                DefaultDirectoryID = chcDefaultDirectoryID.Checked ? null : (uint?)nudDefaultDirectoryID.Value,
                DefaultFileName = txtDefaultFileName.Text.Trim(),
                HeroDogName = txtHeroDogName.Text.Trim(),
                HeroName = txtHeroName.Text.Trim(),
                HomeCityID = chcHomeCityID.Checked ? null : (uint?)nudHomeCityID.Value,
                HomeCityType = txtHomeCityType.Text.Trim(),
                Level = chcLevel.Checked ? null : (uint?)nudLevel.Value,
                Name = txtName.Text.Trim(),
                NumPropUnlocksEarned = chcNumPropUnlocksEarned.Checked ? null : (uint?)nudNumPropUnlocksEarned.Value,
                Respec_Exists = RespecExists,
                Respec = radRespec_empty.Checked ? null : (bool?)radRespec_true.Checked,
                ShipName = txtShipName.Text.Trim(),
                SkillPoints = chcSkillPoints.Checked ? null : (uint?)nudSkillPoints.Value,
                Version = chcVersion.Checked ? null : (uint?)nudVersion.Value,
                XP = chcXp.Checked ? null : (uint?)nudXp.Value,
                XPPercentage = chcXpPercentage.Checked ? null : (decimal?)nudXpPercentage.Value
            };
        }

        private void RestoreDefaults()
        {
            btnBrowseFile.Enabled = !chcLast.Checked;
            lblFileName.Text = string.Empty;

            RestoreDefaultsForControls();
        }
        private void RestoreDefaultsForControls()
        {
            pnlDependencies.Enabled = false;

            chcVersion.Tag = nudVersion;
            chcDefaultDirectoryID.Tag = nudDefaultDirectoryID;
            chcHomeCityID.Tag = nudHomeCityID;
            chcLevel.Tag = nudLevel;
            chcXp.Tag = nudXp;
            chcSkillPoints.Tag = nudSkillPoints;
            chcXpPercentage.Tag = nudXpPercentage;
            chcNumPropUnlocksEarned.Tag = nudNumPropUnlocksEarned;

            chcVersion.Checked = true;
            chcDefaultDirectoryID.Checked = true;
            chcHomeCityID.Checked = true;
            chcLevel.Checked = true;
            chcXp.Checked = true;
            chcSkillPoints.Checked = true;
            chcXpPercentage.Checked = true;
            chcNumPropUnlocksEarned.Checked = true;
            radRespec_empty.Checked = true;

            txtCivilization.Text = string.Empty;
            txtDefaultFileName.Text = string.Empty;
            txtHeroDogName.Text = string.Empty;
            txtHeroName.Text = string.Empty;
            txtHomeCityType.Text = string.Empty;
            txtName.Text = string.Empty;
            txtShipName.Text = string.Empty;

            lbxTechs.Items.Clear();
            lbxProps.Items.Clear();
            lbxDecks.Items.Clear();
            lbxCards.Items.Clear();

            btnTechs_edit.Enabled = false;
            btnTechs_delete.Enabled = false;
            btnProps_edit.Enabled = false;
            btnProps_delete.Enabled = false;
            btnDecks_edit.Enabled = false;
            btnDecks_copy.Enabled = false;
            btnDecks_delete.Enabled = false;
            btnCards_add.Enabled = false;
            btnCards_delete.Enabled = false;

            grbActiveTechs.Text = "Active Techs";
            grbActiveProps.Text = "Active Props";
            lblHeader_decks.Text = "Decks";
            lblHeader_cards.Text = "Cards of Selected Deck";
        }
    }
}