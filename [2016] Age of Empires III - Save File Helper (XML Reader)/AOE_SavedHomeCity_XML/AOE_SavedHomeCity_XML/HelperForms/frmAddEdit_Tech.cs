using AOE3_HomeCity.Entities;
using System;
using System.Windows.Forms;

namespace AOE3_HomeCity.HelperForms
{
    public partial class frmAddEdit_Tech : Form
    {
        public frmAddEdit_Tech()
        {
            InitializeComponent();
            _entity = null;
            _formType = FormType.Add;
            _cancelClosing = false;
        }
        public frmAddEdit_Tech(Tech tech)
        {
            InitializeComponent();
            _entity = tech;
            _formType = FormType.Edit;
            _cancelClosing = false;
        }


        private Tech _entity;
        public Tech RetvalEntity
        {
            get { return _entity; }
        }

        private FormType _formType;
        public FormType Type
        {
            get { return _formType; }
        }

        private bool _cancelClosing;



        private void frmAddEdit_Tech_Load(object sender, EventArgs e)
        {
            RestoreDefaults();
            if (_formType == FormType.Edit)
            {
                if (!(chcDatabaseID.Checked = !_entity.DBID.HasValue))
                    nudDatabaseID.Value = _entity.DBID.Value;
                txtName.Text = _entity.Name == null ? string.Empty : _entity.Name;
            }
        }
        private void frmAddEdit_Tech_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _entity = null;
            Close();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            var tech = GetEntityByControls();
            if (tech.ExactEquals(Tech.Empty) &&
                MessageBox.Show("Are you sure to save as empty?", "Empty Entity", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                _cancelClosing = true;
                return;
            }
            _entity = tech;
        }

        private Tech GetEntityByControls()
        {
            return new Tech()
            {
                DBID = chcDatabaseID.Checked ? null : (uint?)nudDatabaseID.Value,
                Name = txtName.Text.Trim()
            };
        }
        private void RestoreDefaults()
        {
            Text = string.Format("{0} {1}", (btnOK.Text = _formType.ToString()), Text);
            chcDatabaseID.Tag = nudDatabaseID;
            chcDatabaseID.Checked = false;
            txtName.Text = string.Empty;
        }
    }
}