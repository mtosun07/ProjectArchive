using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TSN.Universe.DesktopApp
{
    public partial class ChartForm : Form
    {
        static ChartForm()
        {
            _chartPalettes = Enum.GetValues(typeof(ChartColorPalette));
            _defaultChartPaletteIndex = Array.IndexOf(_chartPalettes, ChartColorPalette.BrightPastel);
            _chartTypes = Enum.GetValues(typeof(SeriesChartType));
            _defaultChartTypeIndex_single = Array.IndexOf(_chartTypes, _defaultChartType_single);
            _defaultChartTypeIndex_multi = Array.IndexOf(_chartTypes, _defaultChartType_multi);
            _instances = new List<ChartForm>();
            Program.ApplicationModelChanged += Program_ApplicationModelChanged;
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
            lock (_locker1)
            {
                var universe = Program.ApplicationModel.Universe;
                if (universe != null)
                    universe.Simulated += Universe_SimulationProcessEnded;
            }
        }
        private ChartForm(ChartOptions options)
        {
            _chartOptions = new List<ChartOptions> { options };
            InitializeComponent();
            var universe = Program.ApplicationModel.Universe;
            if ((!universe?.IsInitialized) ?? true)
                throw new InvalidOperationException();
            chkPrevious.Checked = options.ChartType == ChartOptions.ChartTypes.MultiGeneration;
            SetMaximumGeneration();
            for (int i = 0; i < _chartPalettes.Length; i++)
                cmbChartPalette.Items.Add(_chartPalettes.GetValue(i));
            cmbChartPalette.SelectedIndex = _defaultChartPaletteIndex;
            for (int i = 0; i < _chartTypes.Length; i++)
                cmbChartType.Items.Add(_chartTypes.GetValue(i));
            cmbChartType.SelectedIndex = (chkPrevious.Checked = options.ChartType != ChartOptions.ChartTypes.SingleGeneration) ? _defaultChartTypeIndex_multi : _defaultChartTypeIndex_single;
        }


        private const SeriesChartType _defaultChartType_single = SeriesChartType.StackedColumn;
        private const SeriesChartType _defaultChartType_multi = SeriesChartType.Line;

        private static readonly object _locker1 = new object();
        private static readonly Array _chartPalettes;
        private static readonly Array _chartTypes;
        private static readonly int _defaultChartPaletteIndex;
        private static readonly int _defaultChartTypeIndex_single;
        private static readonly int _defaultChartTypeIndex_multi;
        private static readonly List<ChartForm> _instances;

        private readonly object _locker2 = new object();
        private readonly List<ChartOptions> _chartOptions;



        public static void ShowInstantce(ChartOptions options, uint? generation = null)
        {
            lock (_locker1)
            {
                if (options.IsEmpty)
                    throw new ArgumentException("Argument was empty.", nameof(options));
                var frm = new ChartForm(options);
                _instances.Add(frm);
                frm.RepresentCharts(generation);
                frm.Show();
                frm.Focus();
            }
        }
        public static void ClearInstances()
        {
            lock (_locker1)
            {
                var instances = _instances.ToArray();
                foreach (var frm in instances)
                {
                    frm.Close();
                    frm.Dispose();
                }
                _instances.TrimExcess();
            }
        }

        private void SetMaximumGeneration()
        {
            lock (_locker2)
                nudGeneration.Maximum = Program.ApplicationModel.Universe?.CurrentGeneration ?? throw new InvalidOperationException();
        }
        private void RepresentCharts(uint? generation = null)
        {
            lock (_locker2)
            {
                nudGeneration.ValueChanged -= RefreshRequested;
                chkPrevious.CheckStateChanged -= RefreshRequested;
                chart.SizeChanged -= RefreshRequested;
                cmbChartPalette.SelectedIndexChanged -= RefreshRequested;
                cmbChartType.SelectedIndexChanged -= RefreshRequested;
                uint genMax;
                if (generation.HasValue)
                    nudGeneration.Value = genMax = generation.Value;
                else
                    genMax = (uint)nudGeneration.Value;
                var sct = (SeriesChartType)cmbChartType.SelectedItem;
                var pal = (ChartColorPalette)cmbChartPalette.SelectedItem;
                chart.Palette = pal != ChartColorPalette.None ? pal : (ChartColorPalette)_chartPalettes.GetValue(_defaultChartPaletteIndex);
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                chart.Legends.Clear();
                chart.Titles.Clear();
                chart.Titles.Add(new Title("", Docking.Top, new Font("Segoe UI", 8.75F, FontStyle.Regular, GraphicsUnit.Point, 162), Color.Crimson));
                chart.Legends.Add("Titles");
                for (int i = 0; i < _chartOptions.Count; i++)
                {
                    var genMin = (chkPrevious.CheckState == CheckState.Indeterminate ? (_chartOptions[i].ChartType == ChartOptions.ChartTypes.MultiGeneration) : chkPrevious.Checked) ? 0 : genMax;
                    var selectedTitles = _chartOptions[i].GetSelectedTitles();
                    var titles = new Dictionary<Statistics.StatisticsTitle, Dictionary<uint, Statistics.StatisticsCollectionItem>>();
                    for (uint gen = genMin; gen <= genMax; gen++)
                    {
                        var statistics = Statistics.GetOrderedStatistics(Program.ApplicationModel.Universe.GetStatistics(gen)).Where(x => selectedTitles.Contains(x.StatisticsTitle));
                        foreach (var sci in statistics)
                            if (titles.ContainsKey(sci.StatisticsTitle))
                                titles[sci.StatisticsTitle].Add(gen, sci);
                            else
                                titles[sci.StatisticsTitle] = new Dictionary<uint, Statistics.StatisticsCollectionItem> { { gen, sci } };
                    }
                    Func<Statistics.StatisticsCollectionItem, IConvertible> magnitudeSelector;
                    string magnitude;
                    switch (_chartOptions[i].Magnitude)
                    {
                        case ChartOptions.Magnitudes.NewBorns:
                            magnitude = "New Borns";
                            magnitudeSelector = sci => sci.StatisticsMagnitude.LocalMagnitudeNew;
                            break;
                        case ChartOptions.Magnitudes.CurrentSimulation:
                            magnitude = "Current Simulation";
                            magnitudeSelector = sci => sci.StatisticsMagnitude.LocalMagnitudeAll;
                            break;
                        case ChartOptions.Magnitudes.AmongstAllSimulations:
                            magnitude = "Amongst All Simulations";
                            magnitudeSelector = sci => sci.StatisticsMagnitude.GeneralMagnitude;
                            break;
                        default:
                            magnitude = string.Empty;
                            magnitudeSelector = sci => null;
                            break;
                    }
                    double yMin = 0D, yMax = 0D;
                    var titleAddition = (_chartOptions.Count == 1 || string.IsNullOrEmpty(magnitude)) ? string.Empty : $" - {magnitude}";
                    var area = new ChartArea($"area_{i}");
                    area.AxisX.Title = "Generation";
                    area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
                    area.AxisX.Minimum = 0D;
                    area.AxisY.Title = $"Magnitudes{(magnitude.Equals(string.Empty) ? string.Empty : $" for {magnitude}")}";
                    area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
                    chart.ChartAreas.Add(area);
                    foreach (var kvp in titles)
                    {
                        if (kvp.Value.Any(x => magnitudeSelector(x.Value) == null))
                            continue;
                        bool add = true;
                        var title = $"{kvp.Key.ToString()} ({kvp.Value.Values.First().StatisticsMagnitude.MagnitudeType}{titleAddition})";
                        var s = new Series($"{title}_{i}")
                        {
                            ChartArea = area.Name,
                            ChartType = sct,
                            IsXValueIndexed = false,
                            XValueType = ChartValueType.Int64,
                            YValueType = ChartValueType.Double,
                            ToolTip = title,
                            LegendText = title
                        };
                        foreach (var kvp_ in kvp.Value)
                        {
                            var yPoint = magnitudeSelector(kvp_.Value);
                            if (yPoint == null)
                            {
                                add = false;
                                break;
                            }
                            var y = yPoint.ToDouble(NumberFormatInfo.InvariantInfo);
                            yMin = y < yMin ? y : yMin;
                            yMax = y > yMax ? y : yMax;
                            s.Points.AddXY(kvp_.Key, y);
                        }
                        if (add)
                        {
                            if (genMax == 0)
                                s.Points.AddXY(-1D, 0D);
                            chart.Series.Add(s);
                        }
                    }
                }
                nudGeneration.ValueChanged += RefreshRequested;
                chkPrevious.CheckStateChanged += RefreshRequested;
                chart.SizeChanged += RefreshRequested;
                cmbChartPalette.SelectedIndexChanged += RefreshRequested;
                cmbChartType.SelectedIndexChanged += RefreshRequested;
            }
        }
        private void ChartToImage(int width, int height)
        {
            lock (this)
            {
                saveFileDialog.FileName = string.Format("SOE Statistics Charts {0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    using (var bmp = new Bitmap(width, height))
                    {
                        chart.Visible = false;
                        var size = chart.Size;
                        chart.Size = bmp.Size;
                        chart.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
                        chart.Size = size;
                        chart.Visible = true;
                        switch (saveFileDialog.FilterIndex)
                        {
                            case 1:
                                bmp.SaveImage(saveFileDialog.FileName, ImageFormat.Png);
                                break;
                            case 2:
                                bmp.SaveImage(saveFileDialog.FileName, ImageFormat.Jpeg);
                                break;
                            case 3:
                                bmp.SaveImage(saveFileDialog.FileName, ImageFormat.Bmp);
                                break;
                            default:
                                bmp.SaveImage(saveFileDialog.FileName);
                                break;
                        }
                    }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            lock (_locker1)
                _instances.Remove(this);
            base.OnFormClosed(e);
        }

        private void RefreshRequested(object sender, EventArgs e)
        {
            lock (_locker1)
                RepresentCharts();
        }
        private void chart_DoubleClick(object sender, EventArgs e) => WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            chart.Titles[0].Text = string.Empty;
            foreach (var chartArea in chart.ChartAreas)
            {
                chartArea.CursorX.LineColor = Color.Transparent;
                chartArea.CursorY.LineColor = Color.Transparent;
            }
            var result = chart.HitTest(e.X, e.Y, ChartElementType.PlottingArea);
            if (result?.ChartArea == null)
                return;
            double x = double.NaN, y = double.NaN;
            result.ChartArea.CursorX.SetCursorPixelPosition(new PointF(e.X, e.Y), true);
            result.ChartArea.CursorY.SetCursorPixelPosition(new PointF(e.X, e.Y), true);
            result.ChartArea.CursorY.Interval = chart.Series.Where(s => s.ChartArea.Equals(result.ChartArea.Name)).Any(s => s.Points.Any(p => p.YValues.Any(o => Math.Truncate(o) != o))) ? .01 : 1D;
            double pX = result.ChartArea.CursorX.Position, pY = result.ChartArea.CursorY.Position;
            if (!double.IsNaN(pX) && !double.IsNaN(pY))
            {
                x = pX;
                y = pY;
            }
            result.ChartArea.CursorX.LineColor = Color.Crimson;
            result.ChartArea.CursorY.LineColor = Color.Crimson;
            chart.Titles[0].Text = (double.IsNaN(x) || x < 0 || double.IsNaN(y) || y < 0) ? string.Empty : $"Generation: {x} | Magnitude: {y}";
        }
        private void btnAddChart_Click(object sender, EventArgs e)
        {
            lock (_locker2)
            {
                using (var dialog = new ChartOptionsDialogForm())
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _chartOptions.Add(dialog.Selection);
                        chkPrevious.CheckStateChanged -= RefreshRequested;
                        if (chkPrevious.CheckState != CheckState.Indeterminate && dialog.Selection.ChartType == ChartOptions.ChartTypes.MultiGeneration != chkPrevious.Checked)
                            chkPrevious.CheckState = CheckState.Indeterminate;
                        chkPrevious.CheckStateChanged += RefreshRequested;
                        RepresentCharts();
                    }
            }
        }
        private void tsmiExport2_Click(object sender, EventArgs e) => ChartToImage(1920, 1080);
        private void tsmiExport4_Click(object sender, EventArgs e) => ChartToImage(3840, 2160);
        private void tsmiExport8_Click(object sender, EventArgs e) => ChartToImage(7680, 4320);
        private static void Program_ApplicationModelChanged(object sender, EventArgs e)
        {
            ClearInstances();
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
        }
        private static void ApplicationModel_UniverseChanged(object sender, EventArgs e) => ClearInstances();
        private static void Universe_SimulationProcessEnded(object sender, SimulationEventArgs e)
        {
            lock (_locker1)
            {
                if (!((Universe)sender).CurrentGeneration.HasValue)
                {
                    ClearInstances();
                    return;
                }
                else
                    foreach (var frm in _instances)
                        frm.SetMaximumGeneration();
            }
        }
    }
}