using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolution
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            this._sudokuLabels = new List<Label>();
            this._selectedSudokuLabel = null;
            foreach (var ctrl in this.Controls)
            {
                var lbl = ctrl as Label;
                if (lbl != null)
                {
                    lbl.Click += lbl_Click;
                    this._sudokuLabels.Add(lbl);
                }
            }
            this._sudokuLabels.TrimExcess();
        }


        private const int EMPTY_VALUE = -1;
        
        private Color _colorNormal = Color.Silver;
        private Color _colorSelected = Color.Goldenrod;
        private Color _colorValued = Color.MediumAquamarine;

        private List<Label> _sudokuLabels;
        private Label _selectedSudokuLabel;



        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Clear(false);
                    break;
                case Keys.Delete:
                case Keys.Back:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = String.Empty;
                        this._selectedSudokuLabel.BackColor = this._colorNormal;
                        this._selectedSudokuLabel = null;
                    }
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = "1";
                        this._selectedSudokuLabel.BackColor = this._colorValued;
                        this._selectedSudokuLabel = null;
                    }
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = "2";
                        this._selectedSudokuLabel.BackColor = this._colorValued;
                        this._selectedSudokuLabel = null;
                    }
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = "3";
                        this._selectedSudokuLabel.BackColor = this._colorValued;
                        this._selectedSudokuLabel = null;
                    }
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = "4";
                        this._selectedSudokuLabel.BackColor = this._colorValued;
                        this._selectedSudokuLabel = null;
                    }
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = "5";
                        this._selectedSudokuLabel.BackColor = this._colorValued;
                        this._selectedSudokuLabel = null;
                    }
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = "6";
                        this._selectedSudokuLabel.BackColor = this._colorValued;
                        this._selectedSudokuLabel = null;
                    }
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = "7";
                        this._selectedSudokuLabel.BackColor = this._colorValued;
                        this._selectedSudokuLabel = null;
                    }
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = "8";
                        this._selectedSudokuLabel.BackColor = this._colorValued;
                        this._selectedSudokuLabel = null;
                    }
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    if (this._selectedSudokuLabel != null)
                    {
                        this._selectedSudokuLabel.Text = "9";
                        this._selectedSudokuLabel.BackColor = this._colorValued;
                        this._selectedSudokuLabel = null;
                    }
                    break;
            }
        }
        void lbl_Click(object sender, EventArgs e)
        {
            var lbl = sender as Label;
            bool selecting = lbl.BackColor != this._colorSelected;
            foreach (var l in this._sudokuLabels)
            {
                if (l.BackColor != this._colorValued)
                    l.BackColor = this._colorNormal;
                if (!String.IsNullOrEmpty(l.Text))
                    l.BackColor = this._colorValued;
            }
            if (selecting)
                lbl.BackColor = this._colorSelected;
            this._selectedSudokuLabel = selecting ? lbl : null;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Clear(true);
        }
        private void btnSolution_Click(object sender, EventArgs e)
        {
            this.Clear(false);
            bool ok = false;
            foreach (var lbl in this._sudokuLabels)
                if (!String.IsNullOrEmpty(lbl.Text))
                {
                    ok = true;
                    break;
                }
            if (!ok)
            {
                MessageBox.Show("You should fill enough boxes to get a solution.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<Dictionary<string, int>> sudokus = null;
            var values = new Dictionary<string, int>();
            bool allFull = true;
            foreach (var lbl in this._sudokuLabels)
            {
                values.Add(lbl.Name.Substring(3), !String.IsNullOrEmpty(lbl.Text) ? int.Parse(lbl.Text) : -1);
                if (String.IsNullOrEmpty(lbl.Text))
                    allFull = false;
            }
            sudokus = allFull ? (IsSolution(values) ? (sudokus = new List<Dictionary<string, int>>() { values }) : null) : this.GetSolution(values);
            if (sudokus.Count == 0)
                MessageBox.Show("There is not any solution, you may have entered wrong numbers.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (sudokus.Count == 1 || MessageBox.Show("There are " + sudokus.Count.ToString() + " solutions. Do you want to see one of them?", 
                "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                foreach (var sudoku in sudokus[0])
                    (from l in this._sudokuLabels
                     where l.Name.Equals("lbl" + sudoku.Key)
                     select l).First().Text = sudoku.Value.ToString();
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("This application is developed by Mustafa Tosun.\nwww.mustafatosun.net",
                "About \"SUDOKU Solution\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Dictionary<string, List<int>> GetPossibleValues(Dictionary<string, int> sudoku)
        {
            var numbers = Enumerable.Range(1, 9);
            var possibilities = new Dictionary<string, List<int>>();
            foreach (var s in sudoku)
            {
                //if (s.Value != emptyValue)
                //{
                //    possibilities.Add(s.Key, new List<int> { s.Value });
                //    continue;
                //}
                if (s.Value == EMPTY_VALUE)
                {
                    var cannot = (from x in sudoku
                                  where
                                        (x.Key[1].Equals(s.Key[1]) && x.Key[3].Equals(s.Key[3])) ||
                                        (x.Key[0].Equals(s.Key[0]) && x.Key[2].Equals(s.Key[2])) ||
                                        (x.Key[0].Equals(s.Key[0]) && x.Key[1].Equals(s.Key[1]))
                                  select x.Value).Distinct();
                    var l = new List<int>(from n in numbers
                                          where !cannot.Contains(n)
                                          select n);
                    if (l == null || l.Count == 0)
                        return null;
                    possibilities.Add(s.Key, l);
                }
            }
            return possibilities.Count == 0 ? null : possibilities;
        }
        private bool IsSolution(Dictionary<string, int> sudoku)
        {
            if (sudoku.Values.Contains(EMPTY_VALUE))
                return false;
            foreach (var s in sudoku)
            {
                var existed = from x in sudoku
                              where
                                    x.Key != s.Key && (
                                    (x.Key[1].Equals(s.Key[1]) && x.Key[3].Equals(s.Key[3])) ||
                                    (x.Key[0].Equals(s.Key[0]) && x.Key[2].Equals(s.Key[2])) ||
                                    (x.Key[0].Equals(s.Key[0]) && x.Key[1].Equals(s.Key[1])))
                              select x.Value;
                if (existed != null && existed.Count() > 0)
                    return false;
            }
            return true;
        }
        private List<Dictionary<string, int>> GetSolution(Dictionary<string, int> values)
        {
            var sudokus = new List<Dictionary<string, int>>();
            var sudoku = (from v in values orderby v.Key ascending select v).ToDictionary(v => v.Key, v => v.Value);

            var possibleValues = this.GetPossibleValues(sudoku);
            if (possibleValues == null)
                return sudokus;

            foreach (var s in sudoku)
                if (s.Value != EMPTY_VALUE)
                    possibleValues.Add(s.Key, new List<int> { s.Value });
            possibleValues = (from pv in possibleValues orderby pv.Key ascending select pv).ToDictionary(pv => pv.Key, pv => pv.Value);

            var shift = (from s in sudoku orderby s.Key ascending select new { s.Key, Value = 0 }).ToDictionary(s => s.Key, s => s.Value);

            bool isFirstIteration = true;
            Dictionary<string, List<int>> previousSudoku = null;
            for (int currentCellIndex = 0, firstCellIndex = 0; currentCellIndex >= 0; )
            {
                string currentKey = sudoku.Keys.ElementAt(currentCellIndex), firstKey = sudoku.Keys.ElementAt(firstCellIndex);
                int nextCellIndex = -1, previousCellIndex = -1;

                if (isFirstIteration && sudoku.Values.ElementAt(currentCellIndex) != EMPTY_VALUE)
                {
                    isFirstIteration = false;
                    firstCellIndex = -1;
                    for (int ix = 0; ix < possibleValues.Count; ix++)
                        if (possibleValues.Values.ElementAt(ix).Count > 1)
                        {
                            firstCellIndex = ix;
                            break;
                        }
                    if ((currentCellIndex = firstCellIndex) == -1 && this.IsSolution(sudoku))
                        sudokus.Add(sudoku);
                    continue;
                }

                if (currentCellIndex == 0 && shift[currentKey] == possibleValues[currentKey].Count)
                    break;

                var possibilities = this.GetPossibleValues(sudoku);
                if (possibilities == null)
                {
                    for (int ix = currentCellIndex - 1; ix >= 0; ix--)
                        if (previousSudoku.Values.ElementAt(ix).Count > 1)
                        {
                            previousCellIndex = ix;
                            break;
                        }

                    if (previousCellIndex == -1)
                        break;
                    var previousKey = previousSudoku.Keys.ElementAt(previousCellIndex);

                    shift[currentKey] = 0;
                    shift[previousKey]++;
                    sudoku[previousKey] = EMPTY_VALUE;
                    currentCellIndex = previousCellIndex;

                    continue;
                }
                foreach (var s in sudoku)
                    if (s.Value != EMPTY_VALUE)
                        possibilities.Add(s.Key, new List<int> { s.Value });
                possibilities = (from p in possibilities orderby p.Key ascending select p).ToDictionary(p => p.Key, p => p.Value);
                previousSudoku = new Dictionary<string, List<int>>(possibilities);

                for (int ix = currentCellIndex + 1; ix < previousSudoku.Count; ix++)
                    if (previousSudoku.Values.ElementAt(ix).Count > 1)
                    {
                        nextCellIndex = ix;
                        break;
                    }
                for (int ix = currentCellIndex - 1; ix >= 0; ix--)
                    if (previousSudoku.Values.ElementAt(ix).Count > 1)
                    {
                        previousCellIndex = ix;
                        break;
                    }

                if (shift[currentKey] < previousSudoku[currentKey].Count)
                {
                    sudoku[currentKey] = previousSudoku[currentKey][shift[currentKey]];
                    if (this.IsSolution(sudoku))
                        sudokus.Add(sudoku);

                    if (nextCellIndex != -1)
                    {
                        var nextKey = previousSudoku.Keys.ElementAt(nextCellIndex);
                        shift[nextKey] = 0;
                        currentCellIndex = nextCellIndex;
                    }
                    else
                        shift[currentKey]++;
                }
                else
                {
                    if (previousCellIndex == -1)
                        break;
                    var previousKey = previousSudoku.Keys.ElementAt(previousCellIndex);

                    shift[currentKey] = 0;
                    shift[previousKey]++;
                    sudoku[previousKey] = EMPTY_VALUE;
                    currentCellIndex = previousCellIndex;
                }
            }

            #region ... Old Trial (not working) ...
            //var shift = new Dictionary<string, int>(possibilities.Count);
            //var shiftMax = new Dictionary<string, int>(possibilities.Count);
            //foreach (var p in possibilities)
            //{
            //    shift.Add(p.Key, 0);
            //    shiftMax.Add(p.Key, p.Value.Count - 1);
            //}
            //int iteration_dim0 = 0;
            //int iteration_dim1 = 0;
            //while (true)
            //{
            //    var sudoku = new Dictionary<string, int>();
            //    for (int k = 0; k < possibilities.Count; k++)
            //    {
            //        var p = possibilities.ElementAt(k);
            //        sudoku.Add(p.Key, p.Value[shift[p.Key]]);
            //        if ((k == iteration_dim1 ? ++shift[p.Key] : shift[p.Key]) == shiftMax[p.Key] + 1 && ++iteration_dim1 != possibilities.Count)
            //            shift[p.Key] = 0;
            //    }
            //    if (IsSolution(sudoku))
            //        sudokus.Add(sudoku);
            //    if (iteration_dim1 == possibilities.Count && shift.ElementAt(iteration_dim1 - 1).Value == shiftMax.ElementAt(iteration_dim1 - 1).Value + 1)
            //    {
            //        if (iteration_dim0 == possibilities.Count)
            //            break;
            //        shift[shift.ElementAt(iteration_dim1 - 1).Key] = shiftMax[shift.ElementAt(iteration_dim1 - 1).Key] + (iteration_dim0++);
            //        shift[shift.ElementAt(iteration_dim1 - 1).Key] = 
            //            shift.ElementAt(iteration_dim1 - 1).Value > shiftMax.ElementAt(iteration_dim1 - 1).Value ? 
            //                shiftMax.ElementAt(iteration_dim1 - 1).Value : shift.ElementAt(iteration_dim1 - 1).Value;
            //        iteration_dim1 = 0;
            //    }
            //}
            #endregion

            return sudokus;
        }

        private void Clear(bool clearValues)
        {
            this._selectedSudokuLabel = null;
            foreach (var l in this._sudokuLabels)
            {
                l.BackColor = clearValues ? this._colorNormal : (l.BackColor == this._colorValued ? l.BackColor : this._colorNormal);
                if (clearValues)
                    l.Text = String.Empty;
            }
        }
    }
}