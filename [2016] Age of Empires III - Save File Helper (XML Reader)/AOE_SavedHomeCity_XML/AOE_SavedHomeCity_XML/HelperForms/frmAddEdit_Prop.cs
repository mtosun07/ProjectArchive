using AOE3_HomeCity.Entities;
using System;
using System.Windows.Forms;

namespace AOE3_HomeCity.HelperForms
{
    public partial class frmAddEdit_Prop : Form
    {
        public frmAddEdit_Prop()
        {
            InitializeComponent();
            _entity = null;
            _formType = FormType.Add;
            _cancelClosing = false;
        }
        public frmAddEdit_Prop(Prop prop)
        {
            InitializeComponent();
            _entity = prop;
            _formType = FormType.Edit;
            _cancelClosing = false;
        }


        private Prop _entity;
        public Prop RetvalEntity
        {
            get { return _entity; }
        }

        private FormType _formType;
        public FormType Type
        {
            get { return _formType; }
        }

        private bool _cancelClosing;



        private void frmAddEdit_Prop_Load(object sender, EventArgs e)
        {
            RestoreDefaults();
            if (_formType == FormType.Edit)
            {
                if (!(radEnabled_empty.Checked = !_entity.Enabled.HasValue))
                {
                    radEnabled_true.Checked = _entity.Enabled.Value.BooleanValue;
                    radEnabled_false.Checked = radEnabled_true.Checked;
                }
                txtName.Text = _entity.Name == null ? string.Empty : _entity.Name;
            }
        }
        private void frmAddEdit_Prop_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cancelClosing)
            {
                _cancelClosing = false;
                e.Cancel = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _entity = null;
            Close();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            var prop = GetEntityByControls();
            if (prop.ExactEquals(Prop.Empty) &&
                MessageBox.Show("Are you sure to save as empty?", "Empty Entity", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                _cancelClosing = true;
                return;
            }
            _entity = prop;
        }

        private Prop GetEntityByControls()
        {
            return new Prop()
            {
                Enabled = radEnabled_empty.Checked ? null : (bool?)(radEnabled_true.Checked),
                Name = txtName.Text.Trim()
            };
        }
        private void RestoreDefaults()
        {
            Text = string.Format("{0} {1}", (btnOK.Text = _formType.ToString()), Text);
            radEnabled_true.Checked = true;
            txtName.Text = string.Empty;
        }
    }
}