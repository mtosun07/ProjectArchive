using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TSN.ColorSwitcher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            _pixels = new Dictionary<Color, List<Point>>();
            InitializeComponent();
        }


        private Dictionary<Color, List<Point>> _pixels;



        private string ColorToHex(Color color) => $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}".ToUpperInvariant(); //(new ColorConverter()).ConvertToString(color);
        private Color HexToColor(string hex)
        {
            //return (Color)((new ColorConverter()).ConvertFromString(hex));
            var s = hex?.Trim() ?? string.Empty;
            s = s.StartsWith("#") ? s.Substring(1) : s;
            if (string.IsNullOrEmpty(s))
                return Color.Transparent;
            switch (s.Length)
            {
                case 3:
                    s = string.Concat(hex[0], hex[0], hex[1], hex[1], hex[2], hex[2]);
                    break;
                case 4:
                    s = string.Concat(hex[0], hex[0], hex[1], hex[1], hex[2], hex[2], hex[3], hex[3]);
                    break;
                default:
                    break;
            }
            int a = -1, r, g, b;
            try
            {
                if (s.Length == 6)
                {
                    r = Convert.ToInt32(s.Substring(0, 2), 16);
                    g = Convert.ToInt32(s.Substring(2, 2), 16);
                    b = Convert.ToInt32(s.Substring(4, 2), 16);
                }
                else
                {
                    a = Convert.ToInt32(s.Substring(0, 2), 16);
                    r = Convert.ToInt32(s.Substring(2, 2), 16);
                    g = Convert.ToInt32(s.Substring(4, 2), 16);
                    b = Convert.ToInt32(s.Substring(6, 2), 16);
                }
            }
            catch
            {
                throw new ArgumentOutOfRangeException(nameof(hex));
            }
            return a < 0 ? Color.FromArgb(r, g, b) : Color.FromArgb(a, r, g, b);
        }
        private void ClearSelectedColor()
        {
            btnOpenChangeColor.Enabled = false;
            pnlSelectedColor.Visible = false;
            lblSelectedColor.BackColor = Color.White;
            txtSelectedColorR.Text = string.Empty;
            txtSelectedColorG.Text = string.Empty;
            txtSelectedColorB.Text = string.Empty;
            txtSelectedColorA.Text = string.Empty;
            txtSelectedColorHex.Text = string.Empty;
            grbChangeColor.Visible = false;
        }
        private void ClearImage()
        {
            btnSaveImage.Visible = false;
            lblFilePath.Text = string.Empty;
            picImage.Image?.Dispose();
            picImage.Image = null;
            picImage.BackColor = SystemColors.Control;
            picImage.BorderStyle = BorderStyle.FixedSingle;
            grbChangeColor.Visible = false;
            grbColorsInImage.Visible = false;
            lstColorsInImage.Items.Clear();
            ClearSelectedColor();
        }
        private void PickColor()
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                nudNewColorA.Value = colorDialog1.Color.A;
                nudNewColorR.Value = colorDialog1.Color.R;
                nudNewColorG.Value = colorDialog1.Color.G;
                nudNewColorB.Value = colorDialog1.Color.B;
            }
        }
        private void RefreshColorsList()
        {
            lstColorsInImage.Items.Clear();
            if (_pixels.Count == 0)
                return;
            var array = _pixels.OrderByDescending(x => x.Value.Count).ToArray();
            var padding = array[0].Value.Count.ToString().Length;
            lstColorsInImage.Items.AddRange(array.Select(x => (object)$"{x.Value.Count.ToString().PadLeft(padding)} x {ColorToHex(x.Key)}").ToArray());
        }
        private void ChangeColor()
        {
            if (lstColorsInImage.SelectedIndices.Count == 0)
                return;
            var bmp = (Bitmap)picImage.Image;
            var newColor = Color.FromArgb((int)nudNewColorA.Value, (int)nudNewColorR.Value, (int)nudNewColorG.Value, (int)nudNewColorB.Value);
            var newPoints = new List<Point>();
            foreach (var color in lstColorsInImage.SelectedItems.Cast<object>().Select(x => HexToColor(((string)x).Split(new[] { " x " }, StringSplitOptions.RemoveEmptyEntries)[1])).Distinct())
            {
                if (!_pixels.TryGetValue(color, out var points))
                    continue;
                foreach (var p in points)
                    bmp.SetPixel(p.X, p.Y, newColor);
                newPoints.AddRange(points);
                _pixels.Remove(color);
            }
            if (newPoints.Count > 0)
            {
                if (_pixels.TryGetValue(newColor, out var points_))
                    points_.AddRange(newPoints);
                else
                    _pixels.Add(newColor, newPoints);
            }
            picImage.Image = bmp;
            RefreshColorsList();
        }

        private void lblSelectedColor_BackColorChanged(object sender, EventArgs e)
        {
            if (lblSelectedColor.BackColor.IsEmpty || lblSelectedColor.BackColor.Equals(Color.Transparent))
                btnOpenChangeColor.Enabled = false;
        }
        private void btnClearSelectedImage_Click(object sender, EventArgs e) => lstColorsInImage.SelectedIndices.Clear();
        private void lstColorsInImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearSelectedColor();
            if (lstColorsInImage.SelectedItems.Count > 0)
            {
                btnOpenChangeColor.Enabled = true;
                if (lstColorsInImage.SelectedItems.Count == 1)
                {
                    string s = (string)lstColorsInImage.SelectedItem;
                    string hex = s.Substring(s.LastIndexOf('#'));
                    var color = HexToColor(hex);
                    pnlSelectedColor.Visible = true;
                    lblSelectedColor.BackColor = Color.FromArgb(255, color.R, color.G, color.B);
                    txtSelectedColorR.Text = color.R.ToString();
                    txtSelectedColorG.Text = color.G.ToString();
                    txtSelectedColorB.Text = color.B.ToString();
                    txtSelectedColorA.Text = color.A.ToString();
                    txtSelectedColorHex.Text = ColorToHex(color);
                }
            }
        }
        private void nudNewColor_ValueChanged(object sender, EventArgs e) => txtNewColorHex.Text = ColorToHex(lblNewColor.BackColor = Color.FromArgb(255, (int)nudNewColorR.Value, (int)nudNewColorG.Value, (int)nudNewColorB.Value));
        private void grbChangeColor_VisibleChanged(object sender, EventArgs e)
        {
            if (!grbChangeColor.Visible)
            {
                nudNewColorA.Value = 255;
                nudNewColorR.Value = 255;
                nudNewColorG.Value = 255;
                nudNewColorB.Value = 255;
            }
        }
        private void btnOpenChangeColor_Click(object sender, EventArgs e) => grbChangeColor.Visible = true;
        private void btnCancelChangeColor_Click(object sender, EventArgs e) => grbChangeColor.Visible = false;
        private void btnColorPicker_Click(object sender, EventArgs e) => PickColor();
        private void lblNewColor_Click(object sender, EventArgs e) => PickColor();
        private void btnChangeColor_Click(object sender, EventArgs e) => ChangeColor();
        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            ClearImage();
            Bitmap bmp = null;
            try
            {
                picImage.Image = bmp = (Bitmap)Image.FromFile(openFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                ClearImage();
                MessageBox.Show($"An error occured: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bmp?.Dispose();
                return;
            }
            lblFilePath.Text = openFileDialog1.FileName;
            picImage.BackColor = Color.White;
            picImage.BorderStyle = BorderStyle.None;
            grbColorsInImage.Visible = true;
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    var c = bmp.GetPixel(x, y);
                    var p = new Point(x, y);
                    if (_pixels.Keys.Contains(c))
                        _pixels[c].Add(p);
                    else
                        _pixels.Add(c, new List<Point> { p });
                }
            RefreshColorsList();
            btnSaveImage.Visible = true;
        }
        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                try
                {
                    picImage.Image.Save(saveFileDialog1.FileName);
                    MessageBox.Show($"Edited image was saved successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occured: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
    }
}