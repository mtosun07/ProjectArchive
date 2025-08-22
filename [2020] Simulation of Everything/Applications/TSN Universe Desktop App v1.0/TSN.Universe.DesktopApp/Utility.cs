using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    internal static class Utility
    {
        private static int GetRandomNumber(RNGCryptoServiceProvider seed, int min, int max)
        {
            if (seed == null)
                throw new ArgumentNullException(nameof(seed));
            if (min > max)
                throw new ArgumentOutOfRangeException();
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                var data = new byte[4];
                seed.GetBytes(data);
                scale = BitConverter.ToUInt32(data, 0);
            }
            return (int)(min + (max - min) * (scale / (double)uint.MaxValue));
        }
        public static void Serialize<T>(this T obj, Stream target)
            where T : ISerializable
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            using (var zs = new GZipStream(target, CompressionMode.Compress, true))
            {
                new BinaryFormatter().Serialize(zs, obj);
                zs.Flush();
                zs.Close();
            }
        }
        public static T Deserialize<T>(this Stream source)
            where T : ISerializable
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            T obj = default;
            if (source.Length > 0)
                using (var zs = new GZipStream(source, CompressionMode.Decompress, true))
                {
                    source.Position = 0;
                    obj = (T)(new BinaryFormatter().Deserialize(zs));
                    zs.Close();
                }
            return obj;
        }
        public static string GetDoubleExtenstion(string filePath)
        {
            var name = filePath?.Trim();
            if (string.IsNullOrEmpty(name) || File.GetAttributes(name).HasFlag(FileAttributes.Directory) || !File.Exists(name))
                return string.Empty;
            if (name != null && Path.HasExtension(name = Path.GetFileName(name)))
                while (Path.HasExtension($"a{name = name.Substring(name.IndexOf('.'))}") && name.Count(c => c == '.') > 2) ;
            return name;
        }
        public static Bitmap DrawUniverseCanvas(this Universe universe, uint? generation = null) => Program.ApplicationModel.UniverseIllustrator.DrawCanvas(generation);
        public static void ResizeText(this Control control, float fontSizeReduce = .25f, float minimumFontSize = 8f, string addition = "...")
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            if (fontSizeReduce < 0 || fontSizeReduce >= control.Font.Size)
                throw new ArgumentOutOfRangeException(nameof(fontSizeReduce));
            if (minimumFontSize < 1)
                throw new ArgumentOutOfRangeException(nameof(minimumFontSize));
            if (control.Font == null)
                throw new ArgumentException("Font was null.", nameof(control));
            if (control.Text == null)
                return;
            var fixedWidth = control.Width - 2;
            var add = addition ?? string.Empty;
            var text = control.Text;
            var fontFamilyName = control.Font.FontFamily;
            var fontStyle = control.Font.Style;
            var emSize = control.Font.Size;
            var fontUnit = control.Font.Unit;
            byte fontGdiCharSet = control.Font.GdiCharSet;
            Font font;
            for (float size = emSize; ; size -= fontSizeReduce)
            {
                font = new Font(fontFamilyName, size, fontStyle, fontUnit, fontGdiCharSet);
                if (fixedWidth >= TextRenderer.MeasureText(text, font).Width)
                    break;
                font.Dispose();
            }
            if (font.Size < minimumFontSize)
            {
                font.Dispose();
                font = new Font(fontFamilyName, emSize, fontStyle, fontUnit, fontGdiCharSet);
                while (fixedWidth < TextRenderer.MeasureText($"{text = text.Substring(0, text.Length - 1)}{add}", font).Width) ;
                text += add;
            }
            control.Font.Dispose();
            control.Font = font;
            control.Text = text;
        }
        public static bool ValidateFileToSave(uint fileCount, string filePath, ref OverwriteDialogForm.OverwritingOptions overwrite)
        {
            if (!File.Exists(filePath))
                return true;
            if (overwrite == OverwriteDialogForm.OverwritingOptions.NoToAll)
                return false;
            if (overwrite != OverwriteDialogForm.OverwritingOptions.YesToAll)
                using (var dialog = new OverwriteDialogForm
                {
                    TargetFolderName = Path.GetFileName(Path.GetDirectoryName(filePath)),
                    FileName = Path.GetFileName(filePath),
                    FileCount = fileCount
                })
                {
                    var dr = dialog.ShowDialog();
                    overwrite = dialog.SelectedOption;
                    if (dr == DialogResult.No)
                        return false;
                }
            return true;
        }
        public static ImageFormat GetImageFormat(string filePath)
        {
            switch (Path.GetExtension(filePath).ToLowerInvariant())
            {
                case ".bmp":
                case ".dib":
                    return ImageFormat.Bmp;
                case ".emf":
                    return ImageFormat.Emf;
                case ".exif":
                    return ImageFormat.Exif;
                case ".gif":
                    return ImageFormat.Gif;
                case ".ico":
                case ".icon":
                    return ImageFormat.Icon;
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                case ".jif":
                case ".jfif":
                case ".jfi":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".tiff":
                case ".tif":
                    return ImageFormat.Tiff;
                case ".wmf":
                    return ImageFormat.Wmf;
                default:
                    return null;
            }
        }
        public static void SaveImage<T>(ICollection<(string FilePath, T Image, ImageFormat ImageFormat)> images)
            where T : Image
        {
            var count = (uint)(images?.Count ?? 0);
            if (count == 0)
                return;
            int i = 0;
            var overwrite = OverwriteDialogForm.OverwritingOptions.NONE;
            try
            {
                foreach (var (filePath, image, imageFormat) in images)
                    if (count == 1 || ValidateFileToSave(count, filePath, ref overwrite))
                    {
                        var format = imageFormat ?? GetImageFormat(filePath);
                        if (format == null)
                            image.Save(filePath);
                        else
                            image.Save(filePath, format);
                        i++;
                    }
                MessageBox.Show($"{(i == 1 ? "The image has" : $"{i} images have")} been saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while saving {(count == 1 ? "the image" : "images")}:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void SaveImage<T>(this T image, string filePath, ImageFormat imageFormat = null) where T : Image => SaveImage(new[] { (filePath, image, imageFormat) });
        public static void SaveToFile<T>(ICollection<(string FilePath, T Model)> models)
            where T : ISerializable
        {
            var count = (uint)(models?.Count ?? 0);
            if (count == 0)
                return;
            int i = 0;
            var overwrite = OverwriteDialogForm.OverwritingOptions.NONE;
            try
            {
                foreach (var (filePath, model) in models)
                    if (count == 1 || ValidateFileToSave(count, filePath, ref overwrite))
                        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            model.Serialize(fs);
                            fs.Flush();
                            i++;
                        }
                MessageBox.Show($"{(i == 1 ? "The file has" : $"{i} files have")} been saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while saving {(count == 1 ? "the file" : "files")}:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void SaveToFile<T>(this T obj, string filePath) where T : ISerializable => SaveToFile(new[] { (filePath, obj) });
        public static void SaveTextToFile(string text, string filePath)
        {
            try
            {
                using (var sw = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write)))
                {
                    sw.Write(text);
                    sw.Flush();
                }
                MessageBox.Show($"The file has been saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while saving the file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static bool TryLoadFromFile<T>(string filePath, out T model, Action<T> done = null)
            where T : ISerializable
        {
            T m = default;
            var result = TryLoadFromFile<T>(filePath, x =>
            {
                m = x;
                done?.Invoke(x);
            });
            model = m;
            return result;
        }
        public static bool TryLoadFromFile<T>(string filePath, Action<T> done)
            where T : ISerializable
        {
            if (done == null)
                throw new ArgumentNullException(nameof(done));
            Exception exc;
            T model;
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    model = Deserialize<T>(fs);
                exc = null;
            }
            catch (Exception ex)
            {
                model = default;
                exc = ex;
            }
            if (exc == null)
            {
                done(model);
                MessageBox.Show("The file has been loaded successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            MessageBox.Show($"An error while retrieving data from the specified file.\nMessage: {exc.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);



        public sealed class UniverseIllustrator
        {
            static UniverseIllustrator()
            {
                _locker = new object();
            }
            public UniverseIllustrator(Universe universe)
            {
                _universe = universe;
                CanvasSize = new Size(1920, 1080);
                CanvasBackgroundColor = Color.White;
                CanvasMatterFrameColor = Color.Gainsboro;
                CanvasMatterFrameWidth = 1F;
            }


            private static readonly object _locker;

            private readonly Universe _universe;

            private Size CanvasSize { get; set; }
            private Color CanvasBackgroundColor { get; set; }
            private Color CanvasMatterFrameColor { get; set; }
            private float CanvasMatterFrameWidth { get; set; }



            public Bitmap DrawCanvas(uint? generation = null)
            {
                lock (_locker)
                {
                    if (_universe == null)
                        throw new ArgumentNullException(nameof(_universe));
                    var randomiser = new RNGCryptoServiceProvider();
                    var gen = generation ?? _universe.CurrentGeneration.Value;
                    var matters = _universe.GetUniverse(gen).ToArray();
                    var parameters = _universe.GetSimulationParameters().ToArray();
                    var matterSize = new SizeF((float)CanvasSize.Width / _universe.M, (float)CanvasSize.Height / _universe.N);
                    var senseMin = parameters.Min(x => x.Parameters.Sense_Min);
                    var senseCoefficient = (double)byte.MaxValue / (parameters.Max(x => x.Parameters.Sense_Max) - senseMin + 1);
                    float mWidth = CanvasSize.Width / (float)_universe.M, nHeight = CanvasSize.Height / (float)_universe.N;
                    List<Bitmap> imagesFood = new List<Bitmap>(), imagesThing = new List<Bitmap>();
                    var rssSet = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
                    foreach (DictionaryEntry rss in rssSet)
                        if (rss.Value is Bitmap bmp)
                        {
                            var name = (string)rss.Key;
                            if (name.ToLowerInvariant().StartsWith("food"))
                                imagesFood.Add(bmp);
                            else if (name.ToLowerInvariant().StartsWith("thing"))
                                imagesThing.Add(bmp);
                        }
                    var canvas = new Bitmap(CanvasSize.Width, CanvasSize.Height);
                    using (var gr = Graphics.FromImage(canvas))
                    {
                        gr.SmoothingMode = SmoothingMode.AntiAlias;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        if (!CanvasBackgroundColor.Equals(Color.Transparent))
                            using (var br = new SolidBrush(CanvasBackgroundColor))
                                gr.FillRectangle(br, new Rectangle(Point.Empty, CanvasSize));
                        if (CanvasMatterFrameWidth > 0)
                            using (var p = new Pen(CanvasMatterFrameColor, CanvasMatterFrameWidth))
                            {
                                for (float x = mWidth; x < CanvasSize.Width; x += mWidth)
                                    gr.DrawLine(p, new PointF(x, 0F), new PointF(x, CanvasSize.Height));
                                for (float y = nHeight; y < CanvasSize.Height; y += nHeight)
                                    gr.DrawLine(p, new PointF(0F, y), new PointF(CanvasSize.Width, y));
                            }
                        foreach (var m in matters)
                        {
                            if (m == null)
                                continue;
                            var loc = m.GetLocationAt(gen);
                            var rect = new RectangleF(new PointF((loc.X * matterSize.Width) + CanvasMatterFrameWidth, (loc.Y * matterSize.Height) + CanvasMatterFrameWidth), new SizeF(matterSize.Width - (CanvasMatterFrameWidth * 2), matterSize.Height - (CanvasMatterFrameWidth * 2)));
                            if (m is Food f)
                            {
                                if (imagesFood.Count == 0)
                                    gr.DrawEllipse(Pens.Black, rect);
                                else
                                {
                                    var bmp = imagesFood[GetRandomNumber(randomiser, 0, imagesFood.Count)];
                                    gr.DrawImage(bmp, rect, new Rectangle(Point.Empty, bmp.Size), GraphicsUnit.Pixel);
                                }
                            }
                            else if (m is Thing t)
                                using (var br = new SolidBrush(Color.FromArgb((int)((t.Sense - senseMin) * senseCoefficient), (int)(t.ReproduceRate * 100 * 2.55D), (int)(t.DeathRate * 100D * 2.55D))))
                                {
                                    gr.FillRectangle(br, rect);
                                    if (imagesThing.Count > 0)
                                    {
                                        var bmp = imagesThing[GetRandomNumber(randomiser, 0, imagesThing.Count)];
                                        gr.DrawImage(bmp, rect, new Rectangle(Point.Empty, bmp.Size), GraphicsUnit.Pixel);
                                    }
                                }
                        }
                        gr.Flush();
                    }
                    imagesFood.Clear();
                    imagesFood.TrimExcess();
                    imagesThing.Clear();
                    imagesThing.TrimExcess();
                    return canvas;
                }
            }
            public Matter GetMatter(Size canvasSize, Point point, uint? generation = null)
            {
                lock (_locker)
                {
                    decimal sizeCoefficientX = (decimal)CanvasSize.Width / canvasSize.Width, sizeCoefficientY = (decimal)CanvasSize.Height / canvasSize.Height;
                    var matterSize = new SizeF((float)CanvasSize.Width / _universe.M, (float)CanvasSize.Height / _universe.N);
                    var gen = generation ?? _universe.CurrentGeneration.Value;
                    return _universe.GetUniverse(gen).SingleOrDefault(x => x.GetLocationAt(gen).Equals(new Location((ushort)(sizeCoefficientX * point.X / (decimal)matterSize.Width), (ushort)(sizeCoefficientY * point.Y / (decimal)matterSize.Height))));
                }
            }
        }
    }
}