using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    public partial class WaitForm : Form
    {
        static WaitForm()
        {
            _locker = new object();
            _instance = null;
        }
        private WaitForm(Func<bool> worker, string actionInfo)
        {
            _openForms = new List<(Form, bool, bool)>();
            foreach (Form frm in Application.OpenForms)
                if (frm != this)
                {
                    _openForms.Add((frm, frm.Visible, frm.Enabled));
                    frm.Visible = frm.Enabled = false;
                }
            _worker = worker;
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(Utility.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            var text = actionInfo?.Trim();
            lblActionInfo.Text = $"Please wait{(string.IsNullOrEmpty(text) ? string.Empty : $" while {actionInfo}")}...";
        }


        private static readonly object _locker;
        private static WaitForm _instance;

        private readonly List<(Form Form, bool Visible, bool Enabled)> _openForms;
        private readonly Func<bool> _worker;



        public static WaitForm GetInstance(Func<bool> worker = null, string actionInfo = null)
        {
            lock (_locker)
            {
                _instance?.Close();
                while (_instance != null) ;
                return _instance = new WaitForm(worker, actionInfo);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (var frm in _openForms)
            {
                if (frm.Form.IsDisposed)
                    continue;
                frm.Form.Visible = frm.Visible;
                frm.Form.Enabled = frm.Enabled;
            }
            _instance = null;
            base.OnClosed(e);
            if (_worker != null)
                MessageBox.Show("Action was completed successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void WaitForm_Load(object sender, EventArgs e)
        {
            if (_worker != null)
                bgwAction.RunWorkerAsync();
        }
        private void bgwAction_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (_locker)
                e.Result = _worker.Invoke();
        }
        private void bgwAction_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (_locker)
            {
                if (e.Error != null)
                {
                    DialogResult = DialogResult.Cancel;
                    Hide();
                    MessageBox.Show($"An error occured: {e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                DialogResult = (bool)e.Result ? DialogResult.Yes : DialogResult.No;
                Close();
            }
        }
    }
}