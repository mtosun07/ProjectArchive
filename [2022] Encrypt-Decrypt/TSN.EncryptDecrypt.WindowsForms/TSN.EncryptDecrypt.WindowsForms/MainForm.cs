using System;
using System.Windows.Forms;

namespace TSN.EncryptDecrypt.WindowsForms
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();



        private bool CheckTexts(bool isEncryption)
        {

            if ((isEncryption ? _textBoxData.Text : _textBoxData.Text.Trim()).Equals(string.Empty))
            {
                MessageBox.Show("Data was empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (_textboxPassword.Text.Length < AesHmac.MinPasswordLength)
            {
                MessageBox.Show($"Password must have {AesHmac.MinPasswordLength} characters at least.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        
        private void ButtonEncrypt_Click(object sender, EventArgs e)
        {
            _textBoxResult.Clear();
            if (!CheckTexts(true))
                return;
            try
            {
                _textBoxResult.Text = AesHmac.SimpleEncryptWithPassword(_textBoxData.Text, _textboxPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ButtonDecrypt_Click(object sender, EventArgs e)
        {
            _textBoxResult.Clear();
            if (!CheckTexts(false))
                return;
            try
            {
                _textBoxResult.Text = AesHmac.SimpleDecryptWithPassword(_textBoxData.Text, _textboxPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}