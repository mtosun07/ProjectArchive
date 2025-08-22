using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    internal abstract class ListFormBase : Form
    {
        static ListFormBase()
        {
            _locker = new object();
            _instances = new List<ListFormBase>();
            Program.ApplicationModelChanged += Program_ApplicationModelChanged;
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
        }
        private protected ListFormBase(string title, string matterInfo, string matterInfoShort = null)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));
            var _title = title.Trim();
            if (_title.Equals(string.Empty))
                throw new ArgumentException("Argument was empty.", nameof(title));
            if (matterInfo == null)
                throw new ArgumentNullException(nameof(matterInfo));
            var _matterInfo = matterInfo.Trim();
            if (_title.Equals(string.Empty))
                throw new ArgumentException("Argument was empty.", nameof(matterInfo));
            var _matterInfoShort = matterInfoShort?.Trim() ?? string.Empty;
            if (_matterInfoShort.Equals(string.Empty))
                _matterInfoShort = matterInfo;
            InitializeComponent($"{_title} {_matterInfoShort}", _title, _matterInfo);
        }


        private static readonly object _locker;
        private static readonly List<ListFormBase> _instances;

        private Button btnUpdate;
        private Label lblMatterInfo;
        private Label lblSubInfo;
        private Label lblTitle;
        private ListBox lstItems;
        private ToolTip tooltip;



        protected static void AddInstance(ListFormBase instance)
        {
            lock (_locker)
            {
                if (instance == null)
                    throw new ArgumentNullException(nameof(instance));
                _instances.Add(instance);
            }
        }
        public static void ClearInstances()
        {
            lock (_locker)
            {
                var instances = _instances.ToArray();
                foreach (var frm in instances)
                    frm.Close();
            }
        }

        private void InitializeComponent(string formText, string lblTitleText, string lblMatterInfoText)
        {
            lblTitle = new Label();
            lblMatterInfo = new Label();
            lstItems = new ListBox();
            lblSubInfo = new Label();
            btnUpdate = new Button();
            tooltip = new ToolTip();
            SuspendLayout();
            btnUpdate.BackColor = Color.Black;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnUpdate.ForeColor = Color.Gainsboro;
            btnUpdate.Location = new Point(12, 364);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(100, 25);
            btnUpdate.TabIndex = 4;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            lblMatterInfo.BackColor = Color.DimGray;
            lblMatterInfo.BorderStyle = BorderStyle.FixedSingle;
            lblMatterInfo.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblMatterInfo.ForeColor = Color.White;
            lblMatterInfo.Location = new Point(262, 9);
            lblMatterInfo.Name = "lblMatterInfo";
            lblMatterInfo.Size = new Size(510, 30);
            lblMatterInfo.TabIndex = 1;
            lblMatterInfo.Text = lblMatterInfoText;
            lblMatterInfo.TextAlign = ContentAlignment.MiddleCenter;
            lblMatterInfo.ResizeText();
            lblSubInfo.BackColor = Color.Gainsboro;
            lblSubInfo.BorderStyle = BorderStyle.FixedSingle;
            lblSubInfo.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblSubInfo.Location = new Point(112, 364);
            lblSubInfo.Name = "lblSubInfo";
            lblSubInfo.Size = new Size(660, 25);
            lblSubInfo.TabIndex = 3;
            lblSubInfo.TextAlign = ContentAlignment.MiddleRight;
            lblTitle.BackColor = Color.DarkGray;
            lblTitle.BorderStyle = BorderStyle.FixedSingle;
            lblTitle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(12, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(250, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = lblTitleText;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lstItems.BackColor = Color.Gainsboro;
            lstItems.BorderStyle = BorderStyle.FixedSingle;
            lstItems.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lstItems.FormattingEnabled = true;
            lstItems.HorizontalScrollbar = true;
            lstItems.ItemHeight = 17;
            lstItems.Location = new Point(12, 39);
            lstItems.Name = "lstItems";
            lstItems.Size = new Size(760, 325);
            lstItems.TabIndex = 2;
            lstItems.SelectedIndexChanged += new EventHandler(lstItems_SelectedIndexChanged);
            tooltip.ToolTipIcon = ToolTipIcon.None;
            tooltip.SetToolTip(lblMatterInfo, lblMatterInfoText);
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 398);
            Controls.Add(btnUpdate);
            Controls.Add(lblSubInfo);
            Controls.Add(lstItems);
            Controls.Add(lblMatterInfo);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MaximumSize = new Size(800, 437);
            MinimizeBox = false;
            MinimumSize = new Size(800, 437);
            Name = "MatterTraitListFormBase";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = formText;
            Load += new EventHandler(MatterTraitList_Load);
            ResumeLayout(false);

        }
        private void RepresentItems()
        {
            lstItems.Items.Clear();
            lstItems.Items.AddRange(GetItems(out var subInfo));
            lblSubInfo.Text = subInfo;
        }
        protected abstract string[] GetItems(out string subInfo);
        protected abstract void OnItemClicked(int index);

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            lock (_locker)
                _instances.Remove(this);
            base.OnFormClosed(e);
        }

        private void MatterTraitList_Load(object sender, EventArgs e) => RepresentItems();
        private void btnUpdate_Click(object sender, EventArgs e) => RepresentItems();
        private void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstItems.SelectedIndex < 0)
                return;
            OnItemClicked(lstItems.SelectedIndex);
        }

        private static void Program_ApplicationModelChanged(object sender, EventArgs e)
        {
            ClearInstances();
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
        }
        private static void ApplicationModel_UniverseChanged(object sender, EventArgs e) => ClearInstances();
    }
}