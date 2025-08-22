using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static TSN.Universe.Statistics;

namespace TSN.Universe.DesktopApp
{
    internal partial class StatisticsUserControl : UserControl
    {
        static StatisticsUserControl()
        {
            _maximumSize = _defaultMaximumSize = new Size(_fixedWidth, _maximumHeight);
            _minimumSize = new Size(_fixedWidth, _minimumHeight);
        }
        public StatisticsUserControl() => InitializeComponent();


        private const int _maximumHeight = 985;
        private const int _minimumHeight = 535;
        private const string _emptyCellText = "N/A";
        private const int _tableWidth = 1200;
        private const int _firstColumnWidth = 300;
        private const int _columnWidth = 150;
        private const int _rowHeight = 25;

        private static readonly int _fixedWidth = _tableWidth + SystemInformation.VerticalScrollBarWidth;
        private static readonly Size _defaultMaximumSize;
        private static readonly Size _minimumSize;
        private static Size _maximumSize;

        private IReadOnlyCollection<StatisticsCollectionItem> _statistics;

        protected sealed override Size DefaultMaximumSize => _defaultMaximumSize;
        protected sealed override Size DefaultMinimumSize => _minimumSize;
        protected sealed override Size DefaultSize => _minimumSize;
        public sealed override bool AutoSize { get => false; set { } }
        public sealed override Size MaximumSize { get => _maximumSize; set { } }
        public sealed override Size MinimumSize { get => _minimumSize; set { } }



        private Label CreateCellLabel(int height, Point location, int groupIndex, int rowIndex, int cellIndex, string text)
        {
            var isEmptyCell = string.IsNullOrEmpty(text);
            var isFirstColumn = cellIndex == 0;
            var lbl = new Label()
            {
                AutoSize = false,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font((isFirstColumn || isEmptyCell) ? "Segoe UI" : "Segoe UI Semibold", 9.75f, (!isFirstColumn && isEmptyCell) ? FontStyle.Regular : FontStyle.Bold, GraphicsUnit.Point, 162),
                ForeColor = Color.MidnightBlue,
                Location = location,
                Name = $"lblCell_{groupIndex}_{rowIndex}_{cellIndex}",
                Size = new Size(isFirstColumn ? _firstColumnWidth : _columnWidth, height),
                TabIndex = cellIndex,
                Text = isEmptyCell ? _emptyCellText : text,
                TextAlign = isFirstColumn ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter
            };
            if (isFirstColumn)
            {
                lbl.MouseEnter += lblCell_MouseEnter;
                lbl.MouseLeave += lblCell_MouseLeave;
                tooltip.SetToolTip(lbl, isEmptyCell ? _emptyCellText : text);
            }
            lbl.ResizeText();
            return lbl;
        }
        public void RepresentStatistics(Statistics statistics)
        {
            if (statistics == null)
                throw new ArgumentNullException(nameof(statistics));
            var dictionary = new SortedDictionary<NameOrderPair, SortedDictionary<NameOrderPair, SortedDictionary<NameOrderPair, IStatisticsMagnitude>>>();
            _statistics = statistics.OrderBy(x => x).ToList().AsReadOnly();
            foreach (var sci in _statistics)
            {
                var group = new NameOrderPair(sci.StatisticsTitle.GroupOrder, sci.StatisticsTitle.Group);
                var title = new NameOrderPair(sci.StatisticsTitle.TitleOrder, sci.StatisticsTitle.Title);
                var subTitle = new NameOrderPair(sci.StatisticsTitle.SubTitleOrder, sci.StatisticsTitle.SubTitle);
                if (!dictionary.ContainsKey(group))
                    dictionary.Add(group, new SortedDictionary<NameOrderPair, SortedDictionary<NameOrderPair, IStatisticsMagnitude>>());
                if (!dictionary[group].ContainsKey(title))
                    dictionary[group].Add(title, new SortedDictionary<NameOrderPair, IStatisticsMagnitude>());
                dictionary[group][title].Add(subTitle, sci.StatisticsMagnitude);
            }
            pnlLabelContainer.VerticalScroll.Value = 0;
            SuspendLayout();
            pnlLabelContainer.SuspendLayout();
            _maximumSize = new Size(_minimumSize.Width, (_rowHeight * _statistics.Count) + pnlLabelContainer.Location.Y);
            pnlLabelContainer.Controls.Clear();
            int i = 0, y_group = 0, groupIndex = 0;
            foreach (var group in dictionary)
            {
                int height = _rowHeight * group.Value.Sum(title => title.Value.Count);
                var groupContainer = new Panel();
                groupContainer.SuspendLayout();
                groupContainer.BackColor = Color.Transparent;
                groupContainer.Location = new Point(0, y_group);
                groupContainer.Name = $"pnlGroup_{groupIndex}";
                groupContainer.Size = new Size(_tableWidth, height);
                groupContainer.TabIndex = groupIndex;
                int y_row = 0, rowIndex = 0;
                foreach (var title in group.Value)
                {
                    int x_cell = 0, x_cellInner, y_cell = 0, cellIndex = 0;
                    var row = new Panel();
                    row.SuspendLayout();
                    row.BackColor = i++ % 2 == 0 ? Color.LightCyan : Color.Azure;
                    row.Location = new Point(0, _rowHeight * y_row);
                    row.Name = $"pnlRow_{groupIndex}_{rowIndex}";
                    row.Size = new Size(_tableWidth, _rowHeight * title.Value.Count);
                    row.TabIndex = rowIndex;
                    row.Tag = row.BackColor;
                    row.Controls.Add(CreateCellLabel(row.Height, new Point(0, 0), groupIndex, rowIndex, cellIndex++, title.Key.Name));
                    x_cell += _firstColumnWidth;
                    if (title.Value.Select(st => st.Value.MagnitudeType).Distinct().Count() == 1)
                        row.Controls.Add(CreateCellLabel(row.Height, new Point(x_cell, 0), groupIndex, rowIndex, cellIndex++, title.Value.First().Value.MagnitudeType.ToString()));
                    else
                        foreach (var subTitle in title.Value)
                        {
                            row.Controls.Add(CreateCellLabel(_rowHeight, new Point(x_cell, y_cell), groupIndex, rowIndex, cellIndex++, subTitle.Value.MagnitudeType.ToString()));
                            y_cell += _rowHeight;
                        }
                    x_cell += _columnWidth;
                    y_cell = 0;
                    row.Controls.Add(CreateCellLabel(row.Height, new Point(x_cell, 0), groupIndex, rowIndex, cellIndex++, group.Key.Name));
                    x_cell += _columnWidth;
                    foreach (var subTitle in title.Value)
                    {
                        x_cellInner = x_cell;
                        row.Controls.Add(CreateCellLabel(_rowHeight, new Point(x_cellInner, y_cell), groupIndex, rowIndex, cellIndex++, subTitle.Key.Name));
                        x_cellInner += _columnWidth;
                        row.Controls.Add(CreateCellLabel(_rowHeight, new Point(x_cellInner, y_cell), groupIndex, rowIndex, cellIndex++, subTitle.Value.LocalMagnitudeNew?.ToString()));
                        x_cellInner += _columnWidth;
                        row.Controls.Add(CreateCellLabel(_rowHeight, new Point(x_cellInner, y_cell), groupIndex, rowIndex, cellIndex++, subTitle.Value.LocalMagnitudeAll?.ToString()));
                        x_cellInner += _columnWidth;
                        row.Controls.Add(CreateCellLabel(_rowHeight, new Point(x_cellInner, y_cell), groupIndex, rowIndex, cellIndex++, subTitle.Value.GeneralMagnitude?.ToString()));
                        y_cell += _rowHeight;
                    }
                    x_cell += _columnWidth;
                    row.ResumeLayout(false);
                    groupContainer.Controls.Add(row);
                    rowIndex++;
                    y_row += title.Value.Count;
                }
                groupIndex++;
                y_group += height;
                groupContainer.ResumeLayout(false);
                pnlLabelContainer.Controls.Add(groupContainer);
            }
            pnlLabelContainer.Width = _fixedWidth;
            pnlLabelContainer.AutoScroll = true;
            pnlLabelContainer.ResumeLayout(false);
            ResumeLayout(false);
            pnlLabelContainer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        }
        public void Clear()
        {
            _statistics = null;
            SuspendLayout();
            pnlLabelContainer.SuspendLayout();
            _maximumSize = _defaultMaximumSize;
            pnlLabelContainer.Controls.Clear();
            pnlLabelContainer.ResumeLayout(false);
            ResumeLayout(false);
        }
        public Bitmap TableToBitmap(bool libraryInfo, bool header)
        {
            if (_statistics == null)
                throw new InvalidOperationException();
            var controls = new List<Control>();
            if (libraryInfo)
                controls.Add(lblLibrary);
            if (header)
                controls.Add(pnlHeader);
            controls.AddRange(pnlLabelContainer.Controls.OfType<Panel>());
            controls.TrimExcess();
            var bitmaps = new List<Bitmap>(controls.Count);
            foreach (var row in controls)
            {
                var bmp_ = new Bitmap(row.Width, row.Height);
                row.DrawToBitmap(bmp_, new Rectangle(new Point(0, 0), row.Size));
                bitmaps.Add(bmp_);
            }
            var bmp = new Bitmap(controls.Max(x => x.Width), controls.Sum(x => x.Height));
            int y = 0;
            using (var gr = Graphics.FromImage(bmp))
                foreach (var item in bitmaps)
                {
                    gr.DrawImage(item, new PointF(0, y));
                    y += item.Height;
                    item.Dispose();
                }
            bitmaps.Clear();
            bitmaps.TrimExcess();
            controls.Clear();
            controls.TrimExcess();
            return bmp;
        }
        public string TableToText(bool libraryInfo)
        {
            if (_statistics == null)
                throw new InvalidOperationException();
            var sb = new StringBuilder();
            if (libraryInfo)
                sb.AppendLine(lblLibrary.Text);
            foreach (var sci in _statistics)
                sb.AppendLine(sci.ToString());
            return sb.ToString();
        }
        public string TableToCsv(bool libraryInfo, bool header)
        {
            if (_statistics == null)
                throw new InvalidOperationException();
            var sb = new StringBuilder();
            if (libraryInfo)
                sb.AppendLine($"{lblLibrary.Text};;;;;;");
            if (header)
            {
                sb.AppendLine($"{lblHeaderTitle.Text};{lblHeaderMagnitudeType.Text};{lblHeaderMatterOf.Text};;{lblHeaderMagnitudes.Text};;");
                sb.AppendLine($";;{lblHeaderType.Text};{lblHeaderStatus.Text};{lblHeaderNewBorns.Text};{lblHeaderCurrentSimulation.Text};{lblHeaderAmongstAllSimulations.Text}");
            }
            foreach (var sci in _statistics)
                sb.AppendLine($"{sci.StatisticsTitle.Title};{sci.StatisticsMagnitude.MagnitudeType.ToString()};{sci.StatisticsTitle.Group};{sci.StatisticsTitle.SubTitle};{sci.StatisticsMagnitude.LocalMagnitudeNew ?? string.Empty};{sci.StatisticsMagnitude.LocalMagnitudeAll ?? string.Empty};{sci.StatisticsMagnitude.GeneralMagnitude ?? string.Empty}");
            return sb.ToString();
        }

        private void StatisticsUserControl_Load(object sender, EventArgs e)
        {
            var library = Program.ApplicationModel.GetLibraryInfo();
            lblLibrary.Text = $"{library.Name}, Version: {library.Version}";
        }
        private void lblCell_MouseEnter(object sender, EventArgs e)
        {
            foreach (Label lblCell in ((Panel)((Label)sender).Parent).Controls)
                lblCell.BackColor = Color.Gainsboro;
        }
        private void lblCell_MouseLeave(object sender, EventArgs e)
        {
            var pnlRow = (Panel)((Label)sender).Parent;
            foreach (Label lblCell in pnlRow.Controls)
                lblCell.BackColor = (Color)pnlRow.Tag;
        }



        internal struct NameOrderPair : IEquatable<NameOrderPair>, IComparable, IComparable<NameOrderPair>
        {
            public NameOrderPair(byte order, string name)
            {
                _order = order;
                _name = name;
            }


            private readonly byte _order;
            private readonly string _name;

            public byte Order => _order;
            public string Name => _name;



            public override string ToString() => _name;
            public override int GetHashCode() => _order.GetHashCode();
            public override bool Equals(object obj) => obj != null && obj is NameOrderPair nop && Equals(nop);

            public bool Equals(NameOrderPair other) => _order == other._order;
            public int CompareTo(NameOrderPair other)
            {
                var result = _order.CompareTo(other._order);
                return result != 0 ? result : string.Compare(_name, other._name);
            }
            int IComparable.CompareTo(object obj) => obj is NameOrderPair nop ? CompareTo(nop) : throw new InvalidOperationException();
        }
    }
}