using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
//using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TSN.AudioWave;

namespace TSN.FileToVideo
{
    internal partial class MainForm : Form
    {
        public MainForm()
        {
            _stopwatch = new Stopwatch();
            _i = _j = _k = -1;
            _imagesCompleted = _checkingCompleted = _audiosCompleted = false;
            InitializeComponent();
            cmbBitsPerSample.SelectedIndex = 2;
            btnConvert.Text = btnConvert_DefaultText;
            Utility.SavedImageFrame += Utility_ProgressChanged;
            Utility.SavedAllImageFrames += Utility_SavedAllImageFrames;
            Utility.CheckedFrame += Utility_CheckedFrame;
            Utility.CheckedAllFrames += Utility_CheckedAllFrames;
            Utility.SavedAudioFrame += Utility_SavedAudioFrame;
            Utility.SavedAllAudioFrames += Utility_SavedAllAudioFrames;

        }


        private const string btnConvert_DefaultText = "Convert";
        private const string btnConvert_AlternateText1 = "Cancel (Converting...)";
        private const string btnConvert_AlternateText2 = "Cancelling...";

        private readonly Stopwatch _stopwatch;
        private int _i, _j, _k;
        private bool _imagesCompleted,  _checkingCompleted,  _audiosCompleted;



        private void CancelConverting()
        {
            if (bgwConvert.IsBusy && MessageBox.Show("Are you sure to cancel converting the file?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                btnConvert.Enabled = false;
                btnConvert.Text = btnConvert_AlternateText2;
                bgwConvert.CancelAsync();
            }
        }
        private void CheckCancellation(CancellableEventArgs e)
        {
            if (bgwConvert.CancellationPending)
                e.CancelOperation();
        }
        private void ReportProggress(int progressType, ProgressChangedEventArgs e)
        {
            bgwConvert.ReportProgress(progressType, e);
            CheckCancellation(e);
        }

        private void btnBrowse_Click(object sender, EventArgs e) => txtSourcePath.Text = ofdFile.ShowDialog() == DialogResult.OK ? ofdFile.FileName : string.Empty;
        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (!bgwConvert.IsBusy)
            {
                if (txtSourcePath.Text.Equals(string.Empty))
                {
                    MessageBox.Show("Source file was not specified.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (fbdTargetFolder.ShowDialog() != DialogResult.OK)
                    return;
                _i = _j = _k = 0;
                _imagesCompleted = _checkingCompleted = _audiosCompleted = false;
                btnBrowse.Enabled = false;
                lblElapsed.Text = string.Empty;
                btnConvert.Text = btnConvert_AlternateText1;
                lblProgress.Text = string.Empty;
                prgConvertImages.Style = ProgressBarStyle.Marquee;
                prgConvertImages.Value = prgConvertChecking.Value = prgConvertAudios.Value = 0;
                var argument = (new Size((int)nudPixelWidth.Value, (int)nudPixelHeight.Value), (int)nudResolutionWidth.Value, (int)nudResolutionHeight.Value, (int)nudFramesPerSecond.Value, (byte)(cmbBitsPerSample.SelectedIndex == 0 ? 8 : (cmbBitsPerSample.SelectedIndex == 1 ? 16 : 32)), (ushort)nudChannels.Value, (SamplingFrequencies)(uint)nudSamplingFrequency.Value);
                bgwElapsed.RunWorkerAsync();
                bgwConvert.RunWorkerAsync(argument);
            }
            else if (!bgwConvert.CancellationPending)
                CancelConverting();
        }
        private void bgwConvert_DoWork(object sender, DoWorkEventArgs e)
        {
            var argument = ((Size, int, int, int, byte, ushort, SamplingFrequencies))e.Argument;
            var fi = new FileInfo(txtSourcePath.Text);
            var str = $"{fi.Name} ({Utility.EncryptFileName(fi.Name)})";
            var dir = Path.Combine(fbdTargetFolder.SelectedPath, str);
            for (int i = 1; Directory.Exists(dir); i++)
                dir = Path.Combine(fbdTargetFolder.SelectedPath, $"{str} ({i})");
            Directory.CreateDirectory(dir);
            Utility.FileToVideo(fi.FullName, dir, $"{fi.Name} ({fi.Length} bytes)", argument.Item1, argument.Item2, argument.Item3, argument.Item4, argument.Item5, argument.Item6, argument.Item7);
            //var di = new DirectoryInfo(dir);
            //using (var gifw = new GifWriter(Path.Combine(dir, $"{fi.Name}.gif"), 10))
            //    foreach (var frame in di.EnumerateFiles("*.png", SearchOption.AllDirectories).OrderBy(png => png.Name).Select(png => Image.FromFile(png.FullName)))
            //        gifw.WriteFrame(frame);
        }
        private void bgwConvert_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            prgConvertImages.Style = ProgressBarStyle.Blocks;
            var state = (ProgressChangedEventArgs)e.UserState;
            if (_audiosCompleted)
            {
                lblProgress.Text = $"100 % | 100 % | 100 %";
                prgConvertImages.Value = prgConvertImages.Maximum;
                prgConvertChecking.Value = prgConvertChecking.Maximum;
                prgConvertAudios.Value = prgConvertAudios.Maximum;
            }
            else if (_checkingCompleted && e.ProgressPercentage == -3)
            {
                var percentage = 100D * Interlocked.Increment(ref _k) / state.Count;
                lblProgress.Text = $"100 % | 100 % | {percentage:N2} %";
                prgConvertImages.Value = prgConvertImages.Maximum;
                prgConvertChecking.Value = prgConvertChecking.Maximum;
                prgConvertAudios.Value = (int)(percentage * 100D);
            }
            else if (_imagesCompleted && e.ProgressPercentage == -2)
            {
                var percentage = 100D * Interlocked.Increment(ref _j) / state.Count;
                lblProgress.Text = $"100 % | {percentage:N2} %";
                prgConvertImages.Value = prgConvertImages.Maximum;
                prgConvertChecking.Value = (int)(percentage * 100D);
            }
            else if (e.ProgressPercentage == -1)
            {
                var percentage = 100D * Interlocked.Increment(ref _i) / state.Count;
                lblProgress.Text = $"{percentage:N2} %";
                prgConvertImages.Value = (int)(percentage * 100D);
            }
        }
        private void bgwConvert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgwElapsed.CancelAsync();
            GC.Collect();
            btnConvert.Text = btnConvert_DefaultText;
            btnConvert.Enabled = true;
            btnBrowse.Enabled = true;
            if (e.Error != null)
                MessageBox.Show($"An error occured while converting the file: {Environment.NewLine}{e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (!e.Cancelled)
                MessageBox.Show($"Converting the file was completed successfuly.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Converting the file was cancelled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void bgwElapsed_DoWork(object sender, DoWorkEventArgs e)
        {
            _stopwatch.Restart();
            tmrElapsed.Enabled = true;
            while (!bgwElapsed.CancellationPending && bgwConvert.IsBusy)
            {
                bgwElapsed.ReportProgress(-1);
                Thread.Sleep(100);
            }
        }
        private void bgwElapsed_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) { }
        private void bgwElapsed_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _stopwatch.Stop();
            tmrElapsed.Enabled = false;
            lblElapsed.Text = _stopwatch.Elapsed.ToString();
            prgConvertImages.Style = ProgressBarStyle.Blocks;
        }
        private void tmrElapsed_Tick(object sender, EventArgs e) => lblElapsed.Text = _stopwatch.Elapsed.ToString();

        private void Utility_ProgressChanged(object sender, ProgressChangedEventArgs e) => ReportProggress(-1, e);
        private void Utility_SavedAllAudioFrames(object sender, CancellableEventArgs e) => _audiosCompleted = true;
        private void Utility_CheckedFrame(object sender, ProgressChangedEventArgs e) => ReportProggress(-2, e);
        private void Utility_CheckedAllFrames(object sender, CancellableEventArgs e) => _checkingCompleted = true;
        private void Utility_SavedAudioFrame(object sender, ProgressChangedEventArgs e) => ReportProggress(-3, e);
        private void Utility_SavedAllImageFrames(object sender, CancellableEventArgs e) => _imagesCompleted = true;
    }
}