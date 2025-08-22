using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AOE3_HomeCity.HelperForms
{
    public partial class frmSelector<T> : Form
        where T : class
    {
        public frmSelector(IEnumerable<T> list, string title)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            InitializeComponent();
            _list = list;
            _selecteds = new List<T>();
            Text = string.IsNullOrEmpty(title) ? "Select" : title;
        }


        private IEnumerable<T> _list;

        private IEnumerable<T> _selecteds;
        public T[] Selecteds
        {
            get { return _selecteds.ToArray(); }
        }



        private void frmSelector_Load(object sender, EventArgs e)
        {
            lbx.Items.AddRange(_list.ToArray());
            llblSelectAll.Text += string.Format(" ({0})", lbx.Items.Count);
            llblSelectAll.Enabled = (btnOK.Enabled = lbx.Items.Count > 0);
        }

        private void llblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < lbx.Items.Count; i++)
                lbx.SetSelected(i, true);
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lbx.SelectedIndices.Count > 0)
                _selecteds = lbx.SelectedItems.Cast<T>();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}