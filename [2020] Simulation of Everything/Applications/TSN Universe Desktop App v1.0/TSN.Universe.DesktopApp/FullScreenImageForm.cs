using System;
using System.Drawing;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    internal partial class FullScreenImageForm : Form
    {
        static FullScreenImageForm()
        {
            _locker = new object();
            _instance = null;
            Program.ApplicationModelChanged += Program_ApplicationModelChanged;
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
        }
        private FullScreenImageForm(Form senderForm, Bitmap picture, Action<PictureBox, MouseEventArgs> onClick, Action<PictureBox, MouseEventArgs> onMouseMove)
        {
            if (senderForm == null)
                throw new ArgumentNullException(nameof(senderForm));
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));
            _senderForm = senderForm;
            _onClick = onClick;
            _onMouseMove = onMouseMove;
            _senderForm.Hide();
            InitializeComponent();
            picturebox.Image = picture;
        }


        private static readonly object _locker;
        private static FullScreenImageForm _instance;

        private readonly Form _senderForm;
        private readonly Action<PictureBox, MouseEventArgs> _onClick;
        private readonly Action<PictureBox, MouseEventArgs> _onMouseMove;



        public static void ClearInstance(bool close = true)
        {
            lock (_locker)
            {
                if (!(_instance?.IsDisposed ?? true))
                {
                    _instance.picturebox.Image?.Dispose();
                    _instance.picturebox.Image = null;
                    if (close)
                        _instance.Close();
                }
                _instance = null;
            }
        }
        public static void ShowInstance(Form senderForm, Bitmap picture, Action<PictureBox, MouseEventArgs> onClick = null, Action<PictureBox, MouseEventArgs> onMouseMove = null)
        {
            lock (_locker)
            {
                if (_instance?.IsDisposed ?? true)
                    _instance = new FullScreenImageForm(senderForm, picture, onClick, onMouseMove);
                _instance.Show();
                _instance.Focus();
            }
        }
        
        private void FullScreenImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClearInstance(false);
            _senderForm.Show();
        }
        private void FullScreenImageForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
        private void picturebox_MouseClick(object sender, MouseEventArgs e) => _onClick?.Invoke(sender as PictureBox, e);
        //private void picturebox_MouseMove(object sender, MouseEventArgs e) => _onMouseMove?.Invoke(sender as PictureBox, e);

        private static void Program_ApplicationModelChanged(object sender, EventArgs e)
        {
            ClearInstance();
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
        }
        private static void ApplicationModel_UniverseChanged(object sender, EventArgs e) => ClearInstance();
    }
}
