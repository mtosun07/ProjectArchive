using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sudoku9x9
{
    public class SudokuSolution
    {
        #region . Constructors .
        private SudokuSolution()
        {
            Matrix = null;
            HasSolution = false;
            Solution = null;
            _possibilitiesOfEmptyCells = null;
            possibilitiesOfEmptyCells = null;
        }
        #endregion


        #region . Fields and Properties .
        public Sudoku Matrix { get; private set; }
        public bool HasSolution { get; private set; }
        public Sudoku Solution { get; private set; }

        private List<Possibilities> _possibilitiesOfEmptyCells;
        private ReadOnlyCollection<Possibilities> possibilitiesOfEmptyCells;
        public ReadOnlyCollection<Possibilities> PossibilitiesOfEmptyCells
        {
            get
            {
                return possibilitiesOfEmptyCells == null ?
                       (possibilitiesOfEmptyCells = new ReadOnlyCollection<Possibilities>(_possibilitiesOfEmptyCells == null ? new List<Possibilities>(0) : _possibilitiesOfEmptyCells)) :
                       possibilitiesOfEmptyCells;
            }
        }
        #endregion



        #region . Methods and Functions .
        #region . Helper Functions .
        private static bool IsValid(IntSudoku?[] subset)
        {
            if (subset == null)
                throw new ArgumentNullException("subset");
            if (subset.Length != Sudoku.GeneralFixedLength)
                throw new LengthNotValidException("subset");
            var temp = subset.Where(x => x.HasValue).ToList();
            return temp.Count == temp.Distinct().ToList().Count;
        }
        public static bool IsValid(Sudoku matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");
            for (int i = 0; i < Sudoku.GeneralFixedLength; i++)
                if (!IsValid(matrix[SubArrayType.Row, i]) || !IsValid(matrix[SubArrayType.Column, i]) || !IsValid(matrix[SubArrayType.Square, i]))
                    return false;
            return true;
        }
        private static bool _IsSolved(Sudoku matrix, bool validationControl = false)
        {
            if (validationControl && !IsValid(matrix))
                throw new InvalidMatrixException("matrix");
            for (int i = 0; i < Sudoku.Length_Row; i++)
                for (int j = 0; j < Sudoku.Length_Column; j++)
                    if (!matrix[new IndicesPair(i, j)].HasValue)
                        return false;
            return true;
        }
        public static bool IsSolved(Sudoku matrix)
        {
            return _IsSolved(matrix, true);
        }
        private static Dictionary<SubArrayType, IntSudoku?[]> LinkedSubsetsOfCell(Sudoku matrix, IndicesPair initialCell, bool validationControl = false)
        {
            if (validationControl)
            {
                if (!IsValid(matrix))
                    throw new InvalidMatrixException("matrix");
                if (initialCell.RowIndex < 0 || initialCell.ColumnIndex < 0 || initialCell.RowIndex > Sudoku.GeneralFixedLength || initialCell.ColumnIndex > Sudoku.GeneralFixedLength)
                    throw new IndexOutOfRangeException();
            }
            int squareIndex = Sudoku.LinkedSquareIndexOfCell(initialCell);
            var linkedSubsets = new Dictionary<SubArrayType, IntSudoku?[]>(3);
            linkedSubsets.Add(SubArrayType.Row, matrix[SubArrayType.Row, initialCell.RowIndex]);
            linkedSubsets.Add(SubArrayType.Column, matrix[SubArrayType.Column, initialCell.ColumnIndex]);
            linkedSubsets.Add(SubArrayType.Square, matrix[SubArrayType.Square, squareIndex]);
            return linkedSubsets;
        }
        private static IEnumerable<IndicesPair> LinkedCellsFromTheSameSubArray(SubArrayType type, Sudoku matrix, IndicesPair initialCell, bool validationControl = false)
        {
            if (validationControl)
            {
                if (!IsValid(matrix))
                    throw new InvalidMatrixException("matrix");
                if (type != SubArrayType.Row && type != SubArrayType.Column && type != SubArrayType.Square)
                    throw new ArgumentOutOfRangeException("SubArrayType was not valid.", "type");
                if (initialCell.RowIndex < 0 || initialCell.ColumnIndex < 0 || initialCell.RowIndex > Sudoku.GeneralFixedLength || initialCell.ColumnIndex > Sudoku.GeneralFixedLength)
                    throw new IndexOutOfRangeException();
            }
            switch (type)
            {
                case SubArrayType.Row:
                    for (int i = 0; i < Sudoku.Length_Column; i++)
                        if (i != initialCell.ColumnIndex)
                            yield return new IndicesPair(initialCell.RowIndex, i);
                    break;
                case SubArrayType.Column:
                    for (int i = 0; i < Sudoku.Length_Row; i++)
                        if (i != initialCell.RowIndex)
                            yield return new IndicesPair(i, initialCell.ColumnIndex);
                    break;
                case SubArrayType.Square:
                    int squareIndex = Sudoku.LinkedSquareIndexOfCell(initialCell);
                    for (int i = 0; i < Sudoku.FixedLengthForSquares; i++)
                        for (int j = 0; j < Sudoku.FixedLengthForSquares; j++)
                        {
                            var x = Sudoku.RealCellIndices(squareIndex, new IndicesPair(i, j));
                            if (!x.Equals(initialCell))
                                yield return x;
                        }
                    break;
            }
        }
        private static IEnumerable<int> EmptyIndicesOf(IntSudoku?[] subset, bool validationControl = false)
        {
            if (validationControl)
            {
                if (subset == null)
                    throw new ArgumentNullException("subset");
                if (subset.Length != Sudoku.GeneralFixedLength)
                    throw new LengthNotValidException("subset");
                if (!IsValid(subset))
                    throw new InvalidSubsetException("subset");
            }
            for (int i = 0; i < subset.Length; i++)
                if (!subset[i].HasValue)
                    yield return i;
        }
        private static IEnumerable<IndicesPair> EmptyCells(Sudoku matrix, bool validationControl = false)
        {
            if (validationControl && !IsValid(matrix))
                throw new InvalidMatrixException("matrix");
            for (int i = 0; i < Sudoku.GeneralFixedLength; i++)
                for (int j = 0; j < Sudoku.GeneralFixedLength; j++)
                {
                    var indices = new IndicesPair(i, j);
                    if (!matrix[indices].HasValue)
                        yield return indices;
                }
        }
        private static IEnumerable<IntSudoku> NotPotentialValuesOf(Sudoku matrix, IndicesPair indices, bool validationControl = false)
        {
            var links = LinkedSubsetsOfCell(matrix, indices, validationControl);
            return links[SubArrayType.Row].Concat(links[SubArrayType.Column]).Concat(links[SubArrayType.Square]).Where(x => x.HasValue).Select(x => x.Value).Distinct().OrderBy(x => x);
        }
        private static IEnumerable<IntSudoku> PotentialValuesOf(Sudoku matrix, IndicesPair indices, bool validationControl = false)
        {
            return IntSudoku.ValidNumbers.Where(x => !NotPotentialValuesOf(matrix, indices, validationControl).Contains(x));
        }
        private static IEnumerable<IntSudoku> PotentialValuesOf(IntSudoku?[] subset, bool validationControl = false)
        {
            if (validationControl)
            {
                if (subset == null)
                    throw new ArgumentNullException("subset");
                if (subset.Length != Sudoku.GeneralFixedLength)
                    throw new LengthNotValidException("subset");
                if (!IsValid(subset))
                    throw new InvalidSubsetException("subset");
            }
            var temp = subset.Where(x => x.HasValue).Select(x => x.Value);
            return IntSudoku.ValidNumbers.Where(x => !temp.Contains(x)).OrderBy(x => x);
        }
        private static IEnumerable<Possibilities> AllPossibilities(Sudoku matrix, bool validationControl = false)
        {
            if (validationControl && !IsValid(matrix))
                throw new InvalidMatrixException("matrix");
            return EmptyCells(matrix).Select(x => new Possibilities(x, PotentialValuesOf(matrix, x)));
        }
        public static IEnumerable<Possibilities> GetAllPossibilities(Sudoku matrix)
        {
            return AllPossibilities(matrix, true);
        }
        #endregion

        #region . Specific Functions .
        private static bool TrySetValuesToSingles(Sudoku matrix, out Sudoku lastCertain, List<Possibilities> allPossibilities, bool validationControl = false)
        {
            if (validationControl && _IsSolved(matrix, true))
            {
                lastCertain = matrix.Clone() as Sudoku;
                return true;
            }
            bool newOne = false, retVal = false;
            var curr = matrix.Clone() as Sudoku;
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : null;
            List<Possibilities> singles = null;
            do
            {
                newOne = false;
                if (_allPossibilities == null)
                    _allPossibilities = AllPossibilities(curr).ToList();
                singles = _allPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                if (newOne = singles.Count > 0)
                {
                    retVal = true;
                    foreach (var cell in singles)
                        curr[cell.Index] = cell.PossibleNumbers.Single();
                    _allPossibilities = null;
                }
            } while (newOne);
            lastCertain = curr.Clone() as Sudoku;
            return retVal;
        }
        private static IEnumerable<IntSudoku[]> GetCombination(int tupleLength, bool sameNumbers = false)
        {
            if (tupleLength < 2 || tupleLength > 4)
                throw new ArgumentOutOfRangeException(nameof(tupleLength));
            var allNumbers = IntSudoku.ValidNumbers.ToList();
            int x = sameNumbers ? 0 : 1;
            switch (tupleLength)
            {
                case 2:
                    for (int i = 0; i < allNumbers.Count; i++)
                        for (int j = i + x; j < allNumbers.Count; j++)
                            yield return new[] { allNumbers[i], allNumbers[j] };
                    break;
                case 3:
                    for (int i = 0; i < allNumbers.Count; i++)
                        for (int j = i + x; j < allNumbers.Count; j++)
                            for (int k = j + x; k < allNumbers.Count; k++)
                                yield return new[] { allNumbers[i], allNumbers[j], allNumbers[k] };
                    break;
                case 4:
                    for (int i = 0; i < allNumbers.Count; i++)
                        for (int j = i + x; j < allNumbers.Count; j++)
                            for (int k = j + x; k < allNumbers.Count; k++)
                                for (int m = k + x; m < allNumbers.Count; m++)
                                    yield return new[] { allNumbers[i], allNumbers[j], allNumbers[k], allNumbers[m] };
                    break;
            }
        }
        private static List<PossibilitiesMulti> GetNakedTuples(List<Possibilities> allForms, SubArrayTypes searchIn = SubArrayTypes.ALL, int? indexOfSubArray = null, int? tupleLength = null)
        {
            #region . Validation Control .
            if (allForms == null)
                throw new ArgumentNullException(nameof(allForms));
            if (searchIn == 0 || !searchIn.IsFlagDefined())
                throw new ArgumentOutOfRangeException(nameof(searchIn));
            if (indexOfSubArray.HasValue && (indexOfSubArray.Value < 0 || indexOfSubArray.Value > Sudoku.GeneralFixedLength))
                throw new IndexOutOfRangeException();
            if (tupleLength.HasValue && tupleLength.Value != 1 && tupleLength.Value != 2 && tupleLength.Value != 3 && tupleLength.Value != 4)
                throw new ArgumentOutOfRangeException(nameof(tupleLength));
            #endregion
            var nakedTuples = new List<PossibilitiesMulti>();
            for (int
                 k = 0,                                                      // k is the unit specifier.
                 m = !tupleLength.HasValue ? 4 : tupleLength.Value,          // m is max. tuple length.
                 n = !tupleLength.HasValue ? 2 : tupleLength.Value,          // n is min. tuple length.
                 i = !indexOfSubArray.HasValue ? 0 : indexOfSubArray.Value,  // i is the variant of the units' loop.
                 t = -1;                                                     // t is the length of k-th unit.
                 k < 3; k++)
            {
                if ((k == 0 && (searchIn & SubArrayTypes.Row) == 0) ||
                    (k == 1 && (searchIn & SubArrayTypes.Column) == 0) ||
                    (k == 2 && (searchIn & SubArrayTypes.Square) == 0))
                    continue;
                for (t = indexOfSubArray.HasValue ? i :
                         ((k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : k == 2 ? Sudoku.CountOfSquares : -1)) - 1);
                     i <= t; i++)
                {
                    var tuples =
                    (k == 0 ? allForms.Where(x => x.Index.RowIndex == i) :
                    (k == 1 ? allForms.Where(x => x.Index.ColumnIndex == i) :
                    (k == 2 ? allForms.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                    null))).ToList();
                    for (; m >= n; m--)
                    {
                        if (tuples.Count <= m)
                            continue;
                        #region . Getting Naked Tuples .
                        switch (m)
                        {
                            case 2:
                                for (int a = 0; a < tuples.Count; a++)
                                    for (int b = a + 1; b < tuples.Count; b++)
                                    {
                                        Possibilities t1 = tuples[a], t2 = tuples[b];
                                        var pn = t1.PossibleNumbers.Concat(t2.PossibleNumbers).ToList();
                                        if (pn.Count == 2)
                                            nakedTuples.Add(new PossibilitiesMulti(new[] { t1.Index, t2.Index }, pn));
                                    }
                                break;
                            case 3:
                                for (int a = 0; a < tuples.Count; a++)
                                    for (int b = a + 1; b < tuples.Count; b++)
                                        for (int c = b + 1; c < tuples.Count; c++)
                                        {
                                            Possibilities t1 = tuples[a], t2 = tuples[b], t3 = tuples[c];
                                            var pn = t1.PossibleNumbers.Concat(t2.PossibleNumbers).Concat(t3.PossibleNumbers).ToList();
                                            if (pn.Count == 3)
                                                nakedTuples.Add(new PossibilitiesMulti(new[] { t1.Index, t2.Index, t3.Index }, pn));
                                        }
                                break;
                            case 4:
                                for (int a = 0; a < tuples.Count; a++)
                                    for (int b = a + 1; b < tuples.Count; b++)
                                        for (int c = b + 1; c < tuples.Count; c++)
                                            for (int d = c + 1; d < tuples.Count; d++)
                                            {
                                                Possibilities t1 = tuples[a], t2 = tuples[b], t3 = tuples[c], t4 = tuples[d];
                                                var pn = t1.PossibleNumbers.Concat(t2.PossibleNumbers).Concat(t3.PossibleNumbers).Concat(t4.PossibleNumbers).ToList();
                                                if (pn.Count == 4)
                                                    nakedTuples.Add(new PossibilitiesMulti(new[] { t1.Index, t2.Index, t3.Index, t4.Index }, pn));
                                            }
                                break;
                        }
                        #endregion
                    }
                }
            }
            nakedTuples.TrimExcess();
            return nakedTuples;
        }
        private static List<Possibilities> GetAllFormsForEachConjugatePairs(List<Possibilities> allPossibilities, IntSudoku? ofASpesificNumber = null, bool onlyFromPairs = false)
        {
            if (allPossibilities == null)
                throw new ArgumentNullException(nameof(allPossibilities));
            var _allPossibilities = onlyFromPairs ? allPossibilities.Where(x => x.PossibleNumbers.Count == 2).ToList() : allPossibilities;
            var allForms = new List<Possibilities>();
            Action<IntSudoku> act = (number) =>
            {
                for (int r = 0; r < Sudoku.Length_Row; r++)
                {
                    var _forms = _allPossibilities.Where(x => x.Index.RowIndex == r && x.PossibleNumbers.Contains(number)).ToList();
                    if (_forms.Count == 2)
                        allForms.AddRange(_forms.Where(x => !allForms.Any(y => y.Index.Equals(x))).ToList());
                }
                for (int c = 0; c < Sudoku.Length_Column; c++)
                {
                    var _forms = _allPossibilities.Where(x => x.Index.ColumnIndex == c && x.PossibleNumbers.Contains(number)).ToList();
                    if (_forms.Count == 2)
                        allForms.AddRange(_forms.Where(x => !allForms.Any(y => y.Index.Equals(x))).ToList());
                }
                for (int s = 0; s < Sudoku.CountOfSquares; s++)
                {
                    var _forms = _allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == s && x.PossibleNumbers.Contains(number)).ToList();
                    if (_forms.Count == 2)
                        allForms.AddRange(_forms.Where(x => !allForms.Any(y => y.Index.Equals(x))).ToList());
                }
            };
            if (!ofASpesificNumber.HasValue)
            {
                var allNumbers = IntSudoku.ValidNumbers.ToList();
                foreach (var number in allNumbers)
                    act(number);
            }
            else
                act(ofASpesificNumber.Value);
            return allForms.Distinct().ToList();
        }
        private static List<ConjugateTupleForm> GetSingleChain(List<Possibilities> allPossibilities, IntSudoku number, IndicesPair startIndex, List<IndicesPair> formsByNumber, bool startFlag = false, bool validationControl = false)
        {
            if (validationControl)
            {
                if (allPossibilities == null)
                    throw new ArgumentNullException(nameof(allPossibilities));
                if (allPossibilities.Count == 0)
                    throw new ArgumentException(nameof(allPossibilities), "List was empty.");
                if (formsByNumber == null)
                    throw new ArgumentNullException(nameof(formsByNumber));
                if (formsByNumber.Count < 2)
                    throw new ArgumentException("Count of list was less than 2.", nameof(formsByNumber));
                if (!formsByNumber.Contains(startIndex))
                    throw new ArgumentException(nameof(startIndex), "List was not containing the given cell index.");
                if (!allPossibilities.Single(x => x.Index.Equals(startIndex)).PossibleNumbers.Contains(number))
                    throw new ArgumentException(nameof(startIndex), "Cell's possible numbers was not containing the number.");
            }
            var singleChain = new List<ConjugateTupleForm>();
            byte flag = (byte)(startFlag ? 1 : 0);
            int squareIndex = Sudoku.LinkedSquareIndexOfCell(startIndex);
            var _newForms = formsByNumber.Except(new[] { startIndex }).ToList();
            var conjugates = _newForms
                             .Where(form => ((startIndex.RowIndex == form.RowIndex &&
                                              allPossibilities.Count(x => x.Index.RowIndex == startIndex.RowIndex && x.PossibleNumbers.Contains(number)) == 2) ||
                                             (startIndex.ColumnIndex == form.ColumnIndex &&
                                              allPossibilities.Count(x => x.Index.ColumnIndex == startIndex.ColumnIndex && x.PossibleNumbers.Contains(number)) == 2) ||
                                             (squareIndex == Sudoku.LinkedSquareIndexOfCell(form) &&
                                              allPossibilities.Count(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == squareIndex && x.PossibleNumbers.Contains(number)) == 2)))
                             .Distinct().ToList();
            singleChain.Add(new ConjugateTupleForm(startIndex, number, flag));
            foreach (var form in conjugates)
            {
                var chain = GetSingleChain(allPossibilities, number, form, _newForms, !startFlag, false).ToList();
                singleChain = singleChain.Concat(chain.Where(x => !singleChain.Any(y => y.Index.Equals(x)))).OrderBy(x => x.Index).ToList();
            }
            return singleChain.Distinct().ToList();
        }
        private static List<YWing> GetYWings(List<Possibilities> AllPossibilities)
        {
            if (AllPossibilities == null)
                throw new ArgumentNullException(nameof(AllPossibilities));
            var allForms = GetAllFormsForEachConjugatePairs(AllPossibilities, onlyFromPairs: true);
            var yWings = new List<YWing>();
            foreach (var pivot in allForms)
            {
                var r_forms = allForms.Where(x => x.Index.RowIndex == pivot.Index.RowIndex && x.Index.ColumnIndex != pivot.Index.ColumnIndex).ToList();
                var c_forms = allForms.Where(x => x.Index.ColumnIndex == pivot.Index.ColumnIndex && x.Index.RowIndex != pivot.Index.RowIndex).ToList();
                var s_forms = allForms.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == Sudoku.LinkedSquareIndexOfCell(pivot.Index) && !x.Index.Equals(pivot.Index)).ToList();
                for (int i = 0; i < 2; i++)
                {
                    var _forms = i == 0 ? r_forms : c_forms;
                    foreach (var pincer1 in _forms)
                    {
                        var sharedNumbers1 = pivot.PossibleNumbers
                                             .Where(x => pincer1.PossibleNumbers.Any(y => y == x) &&
                                                         AllPossibilities.Count(y => (i == 0 ? (y.Index.RowIndex == pivot.Index.RowIndex) :
                                                                                               (y.Index.ColumnIndex == pivot.Index.ColumnIndex)) &
                                                                                     y.PossibleNumbers.Contains(x)) == 2).ToList();
                        //if (sharedNumbers1.Count > 1)
                        //    continue;
                        foreach (var sharedNumber1 in sharedNumbers1)
                            for (int j = i; j < 2; j++)
                            {
                                if (j == 1 && Sudoku.LinkedSquareIndexOfCell(pivot.Index) == Sudoku.LinkedSquareIndexOfCell(pincer1.Index))
                                    break;
                                var __forms = j == 0 ? c_forms : s_forms;
                                foreach (var pincer2 in __forms)
                                {
                                    var sharedNumbers2 = pivot.PossibleNumbers
                                                         .Where(x => pincer2.PossibleNumbers.Any(y => y == x) &&
                                                                     AllPossibilities.Count(y => (i == 0 ? (y.Index.ColumnIndex == pivot.Index.ColumnIndex) :
                                                                                                           (Sudoku.LinkedSquareIndexOfCell(y.Index) ==
                                                                                                            Sudoku.LinkedSquareIndexOfCell(pivot.Index))) &&
                                                                                                 y.PossibleNumbers.Contains(x)) == 2).ToList();
                                    if (sharedNumbers2.Count > 1 && sharedNumbers1.Count > 2)
                                        throw new InvalidMatrixException();
                                    //if (sharedNumbers2.Count > 1)
                                    //    continue;
                                    foreach (var sharedNumber2 in sharedNumbers2)
                                    {
                                        var sharedNumbers3 = pincer1.PossibleNumbers.Where(x => pincer2.PossibleNumbers.Any(y => y == x)).ToList();
                                        if ((sharedNumbers1.Count > 1 || sharedNumbers2.Count > 1) && sharedNumbers3.Count > 1)
                                            throw new InvalidMatrixException();
                                        //if (sharedNumbers3.Count > 1)
                                        //    continue;
                                        foreach (var sharedNumber3 in sharedNumbers3)
                                        {
                                            if (sharedNumber1 == sharedNumber2 || sharedNumber2 == sharedNumber3 || sharedNumber1 == sharedNumber3)
                                                continue;
                                            var yWing = new YWing(pivot.Index, pincer1.Index, pincer2.Index, sharedNumber1, sharedNumber2, sharedNumber3);
                                            if (!yWings.Contains(yWing))
                                                yWings.Add(yWing);
                                        }
                                    }
                                }
                            }
                    }
                }
            }
            yWings.TrimExcess();
            return yWings;
        }
        #endregion

        #region . Solver Functions .
        private static bool TrySetAnyValues_0_ForcedValues(Sudoku matrix, out Sudoku lastCertain, List<Possibilities> allPossibilities, bool validationControl = false)
        {
            #region . Validation Control .
            if (validationControl)
            {
                if (matrix == null)
                    throw new ArgumentNullException(nameof(matrix));
                if (!IsValid(matrix))
                    throw new InvalidMatrixException(nameof(matrix));
                if (_IsSolved(matrix))
                {
                    lastCertain = matrix.Clone() as Sudoku;
                    return true;
                }
            }
            #endregion
            #region . Declarations .
            bool newOne = false;
            var curr = matrix.Clone() as Sudoku;
            var allValues = IntSudoku.ValidNumbers.ToList();
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : null;
            Action<IndicesPair, IntSudoku, bool> setValue = (index, value, inLoop) =>
            {
                curr[index] = value;
                if (Sudoku.LinkedCellsOf(index).Any(x => curr[x].HasValue && curr[x] == value))
                    throw new InvalidMatrixException();
                if (!inLoop)
                {
                    newOne = true;
                    _allPossibilities = null;
                }
            };
            #endregion
            #region . General Loop .
            do
            {
                #region . Validation Control .
                if (_allPossibilities == null)
                    _allPossibilities = AllPossibilities(curr).ToList();
                if (_allPossibilities.Any(x => x.PossibleNumbers.Count == 0))
                    throw new InvalidMatrixException(nameof(matrix));
                if (_IsSolved(matrix, false))
                {
                    lastCertain = curr.Clone() as Sudoku;
                    return true;
                }
                #endregion
                #region . Setting Values to Singles .
                var singles = _allPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                if (singles.Count > 0)
                {
                    foreach (var cell in singles)
                        setValue(cell.Index, cell.PossibleNumbers.Single(), true);
                    newOne = true;
                    _allPossibilities = null;
                }
                if (_allPossibilities == null)
                    continue;
                #endregion
                #region . Setting Values to Forced Cells .
                for (int k = 0; k < 3; k++)
                {
                    for (int i = 0, length = k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : (k == 2 ? Sudoku.CountOfSquares : -1)); i < length; i++)
                    {
                        var unit = (k == 0 ? _allPossibilities.Where(x => x.Index.RowIndex == i) :
                                   (k == 1 ? _allPossibilities.Where(x => x.Index.ColumnIndex == i) :
                                   (k == 2 ? _allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                               new List<Possibilities>()))).ToList();
                        if (unit.Count == 0)
                            continue;
                        if (unit.Count == 1)
                        {
                            var cell = unit.Single();
                            setValue(cell.Index, cell.PossibleNumbers.Single(), false);
                            break;
                        }
                        var promisingNumbers = unit.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                        foreach (var number in promisingNumbers)
                        {
                            var containers = unit.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                            if (containers.Count == 1)
                            {
                                var cell = containers.Single();
                                setValue(cell.Index, number, false);
                                break;
                            }
                        }
                        if (_allPossibilities == null)
                            break;
                    }
                    if (_allPossibilities == null)
                        break;
                }
                #endregion
            } while (_allPossibilities == null);
            #endregion
            #region . Return Value .
            if (!IsValid(curr))
                throw new InvalidMatrixException();
            lastCertain = curr.Clone() as Sudoku;
            return newOne;
            #endregion
        }
        private static bool TrySetAnyValues_1_IntersectionRemoval(Sudoku matrix, out Sudoku lastCertain, List<Possibilities> allPossibilities, out List<Possibilities> newPossibilities, bool validationControl = false)
        {
            #region . Validation Control .
            if (validationControl && _IsSolved(matrix, true))
            {
                lastCertain = matrix.Clone() as Sudoku;
                newPossibilities = null;
                return true;
            }
            #endregion
            #region . Declarations .
            bool possChanged = false;
            var curr = matrix.Clone() as Sudoku;
            var allValues = IntSudoku.ValidNumbers.ToList();
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : AllPossibilities(curr).ToList();
            #endregion
            #region . Pointing Tuples .
            for (int i = 0; i < Sudoku.CountOfSquares; i++)
            {
                #region . Preparing for i-th Square .
                var square = _allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i).ToList();
                if (square.Count == 0)
                    continue;
                if (square.Count == 1)
                {
                    var cell = square.Single();
                    curr[cell.Index] = cell.PossibleNumbers.Single();
                    lastCertain = curr.Clone() as Sudoku;
                    newPossibilities = null;
                    return true;
                }
                #endregion
                #region . Numbers In Which Cells In Where i-th Square, May Contain .
                foreach (var number in allValues)
                {
                    var _newPossibilities = new List<Possibilities>();
                    var cellsWhichMayContainTheNumber = square.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                    if (cellsWhichMayContainTheNumber.Count == 0 || cellsWhichMayContainTheNumber.Count > Sudoku.FixedLengthForSquares)
                        continue;
                    var first = cellsWhichMayContainTheNumber.First();
                    var cellsIndices = cellsWhichMayContainTheNumber.Select(x => x.Index).ToList();
                    if (cellsIndices.All(x => x.RowIndex == first.Index.RowIndex))
                    {
                        var theirPossibilities = (from x in _allPossibilities
                                                  where x.Index.RowIndex == first.Index.RowIndex &&
                                                        !cellsIndices.Contains(x.Index)
                                                  select new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList())).ToList();
                        if (theirPossibilities.Count > 0)
                        {
                            var singles = theirPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                            if (singles.Count > 0)
                            {
                                foreach (var item in singles)
                                    curr[item.Index] = item.PossibleNumbers.Single();
                                newPossibilities = null;
                                lastCertain = curr.Clone() as Sudoku;
                                return true;
                            }
                            var others = theirPossibilities.Where(x => x.PossibleNumbers.Count > 1 &&
                                                                 _allPossibilities.Single(y => y.Index.Equals(x.Index)).PossibleNumbers.Count > x.PossibleNumbers.Count).ToList();
                            if (others.Count > 0)
                                _newPossibilities.AddRange(others);
                        }
                    }
                    else if (cellsIndices.All(x => x.ColumnIndex == first.Index.ColumnIndex))
                    {
                        var theirPossibilities = (from x in _allPossibilities
                                                  where x.Index.ColumnIndex == first.Index.ColumnIndex &&
                                                        !cellsIndices.Contains(x.Index)
                                                  select new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList())).ToList();
                        if (theirPossibilities.Count > 0)
                        {
                            var singles = theirPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                            if (singles.Count > 0)
                            {
                                foreach (var item in singles)
                                    curr[item.Index] = item.PossibleNumbers.Single();
                                newPossibilities = null;
                                lastCertain = curr.Clone() as Sudoku;
                                return true;
                            }
                            var others = theirPossibilities.Where(x => x.PossibleNumbers.Count > 1 &&
                                                                 _allPossibilities.Single(y => y.Index.Equals(x.Index)).PossibleNumbers.Count < x.PossibleNumbers.Count).ToList();
                            if (others.Count > 0)
                                _newPossibilities.AddRange(others);
                        }
                    }
                    if (_newPossibilities.Count > 0)
                    {
                        _allPossibilities = _newPossibilities.Concat(_allPossibilities.Where(x => !_newPossibilities.Any(y => y.Index.Equals(x.Index)))).ToList();
                        possChanged = true;
                    }
                }
                #endregion
            }
            #endregion
            #region . Box/Line Reduction .
            #region . For Rows .
            for (int i = 0; i < Sudoku.Length_Row; i++)
            {
                #region . Preparing for i-th Row .
                var row = _allPossibilities.Where(x => x.Index.RowIndex == i).ToList();
                if (row.Count == 0)
                    continue;
                if (row.Count == 1)
                {
                    var cell = row.Single();
                    curr[cell.Index] = cell.PossibleNumbers.Single();
                    lastCertain = curr.Clone() as Sudoku;
                    newPossibilities = null;
                    return true;
                }
                #endregion
                #region . Numbers In Which Cells In Where i-th Square, May Contain .
                foreach (var number in allValues)
                {
                    var cellsWhichMayContainTheNumber = row.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                    if (cellsWhichMayContainTheNumber.Count == 0 || cellsWhichMayContainTheNumber.Count > Sudoku.FixedLengthForSquares)
                        continue;
                    var first = cellsWhichMayContainTheNumber.First();
                    var cellsIndices = cellsWhichMayContainTheNumber.Select(x => x.Index).ToList();
                    if (cellsIndices.All(x => Sudoku.LinkedSquareIndexOfCell(x) == Sudoku.LinkedSquareIndexOfCell(first.Index)))
                    {
                        var theirPossibilities = (from x in _allPossibilities
                                                  where Sudoku.LinkedSquareIndexOfCell(x.Index) == Sudoku.LinkedSquareIndexOfCell(first.Index) &&
                                                        !cellsIndices.Contains(x.Index)
                                                  select new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList())).ToList();
                        if (theirPossibilities.Count > 0)
                        {
                            var singles = theirPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                            if (singles.Count > 0)
                            {
                                foreach (var item in singles)
                                    curr[item.Index] = item.PossibleNumbers.Single();
                                newPossibilities = null;
                                lastCertain = curr.Clone() as Sudoku;
                                return true;
                            }
                            var others = theirPossibilities.Where(x => x.PossibleNumbers.Count > 1 &&
                                                                 _allPossibilities.Single(y => y.Index.Equals(x.Index)).PossibleNumbers.Count > x.PossibleNumbers.Count).ToList();
                            if (others.Count > 0)
                            {
                                _allPossibilities = others.Concat(_allPossibilities.Where(x => !others.Any(y => y.Index.Equals(x.Index)))).ToList();
                                possChanged = true;
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion
            #region . For Columns .
            for (int i = 0; i < Sudoku.Length_Column; i++)
            {
                #region . Preparing for i-th Column .
                var column = _allPossibilities.Where(x => x.Index.ColumnIndex == i).ToList();
                if (column.Count == 0)
                    continue;
                if (column.Count == 1)
                {
                    var cell = column.Single();
                    curr[cell.Index] = cell.PossibleNumbers.Single();
                    lastCertain = curr.Clone() as Sudoku;
                    newPossibilities = null;
                    return true;
                }
                #endregion
                #region . Numbers In Which Cells In Where i-th Square, May Contain .
                foreach (var number in allValues)
                {
                    var cellsWhichMayContainTheNumber = column.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                    if (cellsWhichMayContainTheNumber.Count == 0 || cellsWhichMayContainTheNumber.Count > Sudoku.FixedLengthForSquares)
                        continue;
                    var first = cellsWhichMayContainTheNumber.First();
                    var cellsIndices = cellsWhichMayContainTheNumber.Select(x => x.Index).ToList();
                    if (cellsIndices.All(x => Sudoku.LinkedSquareIndexOfCell(x) == Sudoku.LinkedSquareIndexOfCell(first.Index)))
                    {
                        var theirPossibilities = (from x in _allPossibilities
                                                  where Sudoku.LinkedSquareIndexOfCell(x.Index) == Sudoku.LinkedSquareIndexOfCell(first.Index) &&
                                                        !cellsIndices.Contains(x.Index)
                                                  select new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList())).ToList();
                        if (theirPossibilities.Count > 0)
                        {
                            var singles = theirPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                            if (singles.Count > 0)
                            {
                                foreach (var item in singles)
                                    curr[item.Index] = item.PossibleNumbers.Single();
                                newPossibilities = null;
                                lastCertain = curr.Clone() as Sudoku;
                                return true;
                            }
                            var others = theirPossibilities.Where(x => x.PossibleNumbers.Count > 1 &&
                                                                 _allPossibilities.Single(y => y.Index.Equals(x.Index)).PossibleNumbers.Count > x.PossibleNumbers.Count).ToList();
                            if (others.Count > 0)
                            {
                                _allPossibilities = others.Concat(_allPossibilities.Where(x => !others.Any(y => y.Index.Equals(x.Index)))).ToList();
                                possChanged = true;
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion
            #endregion
            #region . Return Value .
            lastCertain = curr.Clone() as Sudoku;
            if (!possChanged)
            {
                newPossibilities = null;
                return false;
            }
            _allPossibilities.TrimExcess();
            List<Possibilities> np = null;
            if (TrySetAnyValues_1_IntersectionRemoval(curr, out curr, _allPossibilities, out np, true))
            {
                newPossibilities = null;
                return true;
            }
            np?.TrimExcess();
            newPossibilities = np != null && np.Count > 0 ? np : _allPossibilities;
            return false;
            #endregion
        }
        private static bool TrySetAnyValues_2_NakedTuples(Sudoku matrix, out Sudoku lastCertain, List<Possibilities> allPossibilities, out List<Possibilities> newPossibilities, bool validationControl = false)
        {
            #region . Validation Control .
            if (validationControl && _IsSolved(matrix, true))
            {
                lastCertain = matrix.Clone() as Sudoku;
                newPossibilities = null;
                return true;
            }
            #endregion
            #region . Declarations .
            bool possChanged = false;
            var curr = matrix.Clone() as Sudoku;
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : AllPossibilities(curr).ToList();
            #endregion
            #region . General Loop .
            do
            {
                var allForms = _allPossibilities.Where(x => x.PossibleNumbers.Count <= 4).ToList();
                #region . SubArrays' Loop .
                for (int k = 0; k < 3; k++)
                {
                    #region . Declarations . 
                    int length = k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : k == 2 ? Sudoku.CountOfSquares : -1);
                    var singles = _allPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                    if (singles.Count > 0)
                    {
                        foreach (var cell in singles)
                            curr[cell.Index] = cell.PossibleNumbers.Single();
                        lastCertain = curr.Clone() as Sudoku;
                        newPossibilities = null;
                        return true;
                    }
                    var allTuples = _allPossibilities.Where(x => x.PossibleNumbers.Count <= 4).ToList();
                    #endregion
                    #region . Deciding for i-th of k-th SubArray .
                    for (int i = 0; i < length; i++)
                    {
                        #region . Tuples' Loop .
                        for (int m = 4; m >= 2; m--)
                        {
                            var nakedTuples = GetNakedTuples(allForms, k == 0 ? SubArrayTypes.Row : (k == 1 ? SubArrayTypes.Column : SubArrayTypes.Square), i, m);
                            if (nakedTuples.Count == 0)
                                continue;
                            var allEmpties =
                                (k == 0 ? _allPossibilities.Where(x => x.Index.RowIndex == i) :
                                (k == 1 ? _allPossibilities.Where(x => x.Index.ColumnIndex == i) :
                                (k == 2 ? _allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                                null))).ToList();
                            var certainValues = nakedTuples.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                            var certainCells = nakedTuples.SelectMany(x => x.Indices).Distinct().ToList();
                            var others = (from empty in allEmpties
                                          where !certainCells.Contains(empty.Index)
                                          let newPn = empty.PossibleNumbers.Where(number => !certainValues.Contains(number)).ToList()
                                          select new
                                          {
                                              CellIndex = empty.Index,
                                              PossibleNumbers = newPn,
                                              IsJustDiscovered = empty.PossibleNumbers.Count > newPn.Count
                                          }).ToList();
                            var _singles = others.Where(x => x.PossibleNumbers.Count == 1).ToList();
                            if (_singles.Count > 0)
                            {
                                foreach (var other in _singles)
                                    curr[other.CellIndex] = other.PossibleNumbers.First();
                                lastCertain = curr.Clone() as Sudoku;
                                newPossibilities = null;
                                return true;
                            }
                            #region . Discovered Tuples .
                            var discoveredTuples = others.Where(x => x.IsJustDiscovered)
                                                   .Select(x => new Possibilities(x.CellIndex, x.PossibleNumbers)).ToList();
                            if (possChanged = discoveredTuples.Count > 0)
                            {
                                _allPossibilities = discoveredTuples.Select(x => new Possibilities(x.Index, x.PossibleNumbers)).ToList()
                                                    .Concat(_allPossibilities.Where(empty => !discoveredTuples.Any(tuple => tuple.Index.Equals(empty.Index))))
                                                    .OrderBy(x => x.Index).ToList();
                                break;
                            }
                            #endregion
                        }
                        #endregion
                        if (possChanged)
                            break;
                    }
                    #endregion
                    if (possChanged)
                        break;
                }
                #endregion
                possChanged = false;
            } while (possChanged);
            #endregion
            #region . Return Value .
            lastCertain = curr.Clone() as Sudoku;
            newPossibilities = possChanged ? _allPossibilities : null;
            return false;
            #endregion
        }
        private static bool TrySetAnyValues_3_HiddenTuples(Sudoku matrix, out Sudoku lastCertain, List<Possibilities> allPossibilities, out List<Possibilities> newPossibilities, bool validationControl = false)
        {
            #region . Validation Control .
            if (validationControl && _IsSolved(matrix, true))
            {
                lastCertain = matrix.Clone() as Sudoku;
                newPossibilities = null;
                return true;
            }
            #endregion
            #region . Declarations .
            bool possChanged = false;
            var curr = matrix.Clone() as Sudoku;
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : AllPossibilities(curr).ToList();
            var perm2 = GetCombination(2).ToList();
            var perm3 = GetCombination(3).ToList();
            var perm4 = GetCombination(4).ToList();
            #endregion
            #region . General Loop .
            do
            {
                possChanged = false;
                for (int k = 0; k < 3; k++)
                    for (int i = 0, length = k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : Sudoku.CountOfSquares); i < length; i++)
                    {
                        var cells = _allPossibilities.Where(x => i == (k == 0 ? x.Index.RowIndex :
                                                                      (k == 1 ? x.Index.ColumnIndex :
                                                                      Sudoku.LinkedSquareIndexOfCell(x.Index)))).ToList();
                        if (cells.Count == 0)
                            continue;
                        for (int j = 2; j <= 4; j++)
                        {
                            if (cells.Count <= j)
                                continue;
                            var bannedNumbers = new List<IntSudoku>();
                            var tuples = j == 2 ? perm2 : (j == 3 ? perm3 : perm4);
                            foreach (var tuple in tuples)
                            {
                                if (tuple.Any(x => bannedNumbers.Contains(x)))
                                    continue;
                                var _containers = new List<IndicesPair>();
                                bool ok = true;
                                foreach (var t in tuple)
                                {
                                    var cont = cells.Where(x => x.PossibleNumbers.Contains(t)).ToList();
                                    if (cont.Count == 1)
                                    {
                                        curr[cont.Single().Index] = t;
                                        lastCertain = curr as Sudoku;
                                        newPossibilities = null;
                                        return true;
                                    }
                                    if (cont.Count < 2 || cont.Count > j)
                                    {
                                        bannedNumbers.Add(t);
                                        ok = false;
                                        break;
                                    }
                                    _containers.AddRange(cont.Select(x => x.Index).ToList());
                                }
                                if (!ok)
                                    continue;
                                _containers = _containers.Distinct().ToList();
                                if (_containers.Count == j)
                                {
                                    var containers = _allPossibilities
                                                     .Where(x => _containers.Contains(x.Index))
                                                     .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Where(y => tuple.Any(z => z == y)).ToList())).ToList();
                                    _allPossibilities = containers.Concat(_allPossibilities.Where(x => !containers.Any(y => y.Index.Equals(x.Index))).ToList()).ToList();
                                    possChanged = true;
                                }
                            }
                        }
                    }
            } while (possChanged);
            #endregion
            #region . Return Value .
            lastCertain = curr.Clone() as Sudoku;
            newPossibilities = possChanged ? _allPossibilities : null;
            return false;
            #endregion
        }
        private static bool TrySetAnyValues_4_XWing(Sudoku matrix, out Sudoku lastCertain, List<Possibilities> allPossibilities, out List<Possibilities> newPossibilities, bool validationControl = false)
        {
            #region . Validation Control .
            if (validationControl && _IsSolved(matrix, true))
            {
                lastCertain = matrix.Clone() as Sudoku;
                newPossibilities = null;
                return true;
            }
            #endregion
            #region . Declarations .
            bool possChanged = false;
            var curr = matrix.Clone() as Sudoku;
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : AllPossibilities(curr).ToList();
            var allNumbers = IntSudoku.ValidNumbers.ToList();
            List<Possibilities> singles = null;
            Action<SubArrayType, int[], int[], IntSudoku> clearNumbers = (type, indicesOfSubArrays, exceptSubindices, number) =>
            {
                foreach (var index in indicesOfSubArrays)
                {
                    var _changings = (from x in _allPossibilities
                                      where (type == SubArrayType.Row ? (x.Index.RowIndex == index && !exceptSubindices.Contains(x.Index.ColumnIndex)) :
                                            (type == SubArrayType.Column ? (x.Index.ColumnIndex == index && !exceptSubindices.Contains(x.Index.RowIndex)) :
                                            (false)))
                                      select new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList())).ToList();
                    if (_changings.Count == 0)
                        continue;
                    possChanged = true;
                    _allPossibilities = _changings.Concat(_allPossibilities.Where(x => !_changings.Any(y => y.Index.Equals(x.Index)))).OrderBy(x => x.Index).ToList();
                }
            };
            #endregion
            #region . General Loop .
            do
            {
                possChanged = false;
                #region . All Valid Numbers' Loop .
                foreach (var number in allNumbers)
                {
                    #region . Deciding for i-th Row .
                    for (int i = 0; i < Sudoku.Length_Row; i++)
                    {
                        var rowPos = _allPossibilities.Where(x => x.Index.RowIndex == i).ToList();
                        var containingCells = rowPos.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                        if (containingCells.Count != 2)
                            continue;
                        for (int j = i + 1; j < Sudoku.Length_Row; j++)
                        {
                            var _rowPos = _allPossibilities.Where(x => x.Index.RowIndex == j).ToList();
                            var _containingCells = rowPos.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                            if (_containingCells.Count != 2)
                                continue;
                            if (_containingCells[0].Index.ColumnIndex == containingCells[0].Index.ColumnIndex &&
                                _containingCells[1].Index.ColumnIndex == containingCells[1].Index.ColumnIndex)
                            {
                                clearNumbers(SubArrayType.Column,
                                             new[] { containingCells[0].Index.ColumnIndex, _containingCells[0].Index.ColumnIndex },
                                             new[] { i, j },
                                             number);
                                break;
                            }
                        }
                        if (possChanged)
                            break;
                    }
                    #endregion
                    if (possChanged)
                        break;
                    #region . Deciding for i-th Column .
                    for (int i = 0; i < Sudoku.Length_Column; i++)
                    {
                        var columnPos = _allPossibilities.Where(x => x.Index.ColumnIndex == i).ToList();
                        var containingCells = columnPos.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                        if (containingCells.Count != 2)
                            continue;
                        for (int j = i + 1; j < Sudoku.Length_Column; j++)
                        {
                            var _columnPos = _allPossibilities.Where(x => x.Index.ColumnIndex == j).ToList();
                            var _containingCells = columnPos.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                            if (_containingCells.Count != 2)
                                continue;
                            if (_containingCells[0].Index.RowIndex == containingCells[0].Index.RowIndex &&
                                _containingCells[1].Index.RowIndex == containingCells[1].Index.RowIndex)
                            {
                                clearNumbers(SubArrayType.Row,
                                             new[] { containingCells[0].Index.RowIndex, _containingCells[0].Index.RowIndex },
                                             new[] { i, j },
                                             number);
                                break;
                            }
                        }
                        if (possChanged)
                            break;
                    }
                    #endregion
                    if (possChanged)
                        break;
                }
                #endregion
                if (possChanged)
                    break;
            } while (possChanged);
            #endregion
            #region . Trying to Set Any Values .
            if (possChanged)
            {
                singles = _allPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                if (singles.Count > 0)
                {
                    foreach (var single in singles)
                        curr[single.Index] = single.PossibleNumbers.Single();
                    lastCertain = curr.Clone() as Sudoku;
                    newPossibilities = null;
                    return true;
                }
            }
            #endregion
            #region . Return Value .
            lastCertain = curr.Clone() as Sudoku;
            newPossibilities = possChanged ? _allPossibilities : null;
            return false;
            #endregion
        }
        private static bool TrySetAnyValues_5_SingleChains(Sudoku matrix, out Sudoku lastCertain, List<Possibilities> allPossibilities, out List<Possibilities> newPossibilities, bool validationControl = false)
        {
            #region . Validation Control .
            if (validationControl && _IsSolved(matrix, true))
            {
                lastCertain = matrix.Clone() as Sudoku;
                newPossibilities = null;
                return true;
            }
            #endregion
            #region . Declarations .
            bool possChanged = false;
            var curr = matrix.Clone() as Sudoku;
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : AllPossibilities(curr).ToList();
            var allNumbers = IntSudoku.ValidNumbers.ToList();
            #endregion
            #region . General Loop .
            do
            {
                possChanged = false;
                #region . Numbers' Loop .
                foreach (var number in allNumbers)
                {
                    var allForms = GetAllFormsForEachConjugatePairs(_allPossibilities, ofASpesificNumber: number).Select(x => x.Index).ToList().ToList();
                    #region . Getting Single Chains .
                    var allSingleChains = new List<List<ConjugateTupleForm>>();
                    foreach (var form in allForms)
                    {
                        var singleChain = GetSingleChain(_allPossibilities, number, form, allForms);
                        if (singleChain.Count <= 2)
                            continue;
                        var _reverse = singleChain.Select(x => new ConjugateTupleForm(x.Index, x.Number, (byte)(x.Flag == 0 ? 1 : 0))).ToList();
                        if (!allSingleChains.Any(x => x.SequenceEqual(singleChain)) && !allSingleChains.Any(x => x.SequenceEqual(_reverse)))
                            allSingleChains.Add(singleChain);
                    }
                    allSingleChains.TrimExcess();
                    #endregion
                    #region . Deciding for Single Chains .
                    foreach (var singleChain in allSingleChains)
                    {
                        for (int i = 0; i < singleChain.Count; i++)
                        {
                            var form1 = singleChain[i];
                            for (int j = i + 1; j < singleChain.Count; j++)
                            {
                                var form2 = singleChain[j];
                                if (form1.Index.Equals(form2.Index))
                                    continue;
                                #region . Deciding for Forms Which Ones Are In The Same Unit .
                                if (form1.Index.RowIndex == form2.Index.RowIndex ||
                                    form1.Index.ColumnIndex == form2.Index.ColumnIndex ||
                                    Sudoku.LinkedSquareIndexOfCell(form1.Index) == Sudoku.LinkedSquareIndexOfCell(form2.Index))
                                {
                                    #region . The Same Flag Appears Twice in a Unit .
                                    if (form1.Flag == form2.Flag)
                                    {
                                        var corrects = singleChain.Where(x => x.Flag != form1.Flag);
                                        foreach (var cell in corrects)
                                            curr[cell.Index] = number;
                                        lastCertain = curr.Clone() as Sudoku;
                                        newPossibilities = null;
                                        return true;
                                    }
                                    #endregion
                                    #region . Opposite Flags Appear in a Unit .
                                    else
                                    {
                                        #region The Same Square .
                                        int s = Sudoku.LinkedSquareIndexOfCell(form1.Index);
                                        if (s == Sudoku.LinkedSquareIndexOfCell(form2.Index))
                                        {
                                            var l = _allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == s &&
                                                                                 !x.Index.Equals(form1.Index) &&
                                                                                 !x.Index.Equals(form2.Index) &&
                                                                                 x.PossibleNumbers.Contains(number)).ToList();
                                            if (l.Count > 0)
                                            {
                                                possChanged = true;
                                                var _np = l.Select(x => new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList())).ToList();
                                                _allPossibilities = _np.Concat(_allPossibilities.Where(x => !_np.Any(y => y.Index.Equals(x.Index))).ToList())
                                                                       .OrderBy(x => x.Index).ToList();
                                            }
                                        }
                                        #endregion
                                        #region The Same Row .
                                        if (form1.Index.RowIndex == form2.Index.RowIndex)
                                        {
                                            var l = _allPossibilities.Where(x => x.Index.RowIndex == form1.Index.RowIndex &&
                                                                                 x.Index.ColumnIndex != form1.Index.ColumnIndex &&
                                                                                 x.Index.ColumnIndex != form2.Index.ColumnIndex &&
                                                                                 x.PossibleNumbers.Contains(number)).ToList();
                                            if (l.Count > 0)
                                            {
                                                possChanged = true;
                                                var _np = l.Select(x => new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList())).ToList();
                                                _allPossibilities = _np.Concat(_allPossibilities.Where(x => !_np.Any(y => y.Index.Equals(x.Index))).ToList())
                                                                       .OrderBy(x => x.Index).ToList();
                                            }
                                        }
                                        #endregion
                                        #region The Same Column .
                                        else if (form1.Index.ColumnIndex == form2.Index.ColumnIndex)
                                        {
                                            var l = _allPossibilities.Where(x => x.Index.ColumnIndex == form1.Index.ColumnIndex &&
                                                                                 x.Index.RowIndex != form1.Index.RowIndex &&
                                                                                 x.Index.RowIndex != form2.Index.RowIndex &&
                                                                                 x.PossibleNumbers.Contains(number)).ToList();
                                            if (l.Count > 0)
                                            {
                                                possChanged = true;
                                                var _np = l.Select(x => new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList())).ToList();
                                                _allPossibilities = _np.Concat(_allPossibilities.Where(x => !_np.Any(y => y.Index.Equals(x.Index))).ToList())
                                                                       .OrderBy(x => x.Index).ToList();
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion
                                #region . "Seeing"s .
                                if (form1.Flag != form2.Flag)
                                {
                                    var seeing1 = Sudoku.LinkedCellsOf(form1.Index);
                                    var seeing2 = Sudoku.LinkedCellsOf(form2.Index);
                                    var common = seeing1.Intersect(seeing2).ToList();
                                    var cells = _allPossibilities.Where(x => common.Any(y => y.Equals(x.Index)) && x.PossibleNumbers.Contains(number)).ToList();
                                    if (cells.Count > 0)
                                    {
                                        possChanged = true;
                                        var _cells = cells.Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { number }).ToList())).ToList();
                                        _allPossibilities = _cells.Concat(_allPossibilities.Where(x => !_cells.Any(y => y.Index.Equals(x.Index))).ToList())
                                                                  .OrderBy(x => x.Index).ToList();
                                    }
                                }
                                #endregion
                                #region . Setting Values of Singles to the Matrix .
                                if (possChanged)
                                {
                                    var singles = _allPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                                    if (singles.Count > 0)
                                    {
                                        foreach (var cell in singles)
                                            curr[cell.Index] = cell.PossibleNumbers.Single();
                                        lastCertain = curr.Clone() as Sudoku;
                                        newPossibilities = null;
                                        return true;
                                    }
                                    break;
                                }
                                #endregion
                                if (possChanged)
                                    break;
                            }
                            if (possChanged)
                                break;
                        }
                        if (possChanged)
                            break;
                    }
                    #endregion
                    if (possChanged)
                        break;
                }
                #endregion
            } while (possChanged);
            #endregion
            #region . Return Value .
            lastCertain = curr.Clone() as Sudoku;
            newPossibilities = possChanged ? _allPossibilities : null;
            return false;
            #endregion
        }
        private static bool TrySetAnyValues_6_YWing(Sudoku matrix, out Sudoku lastCertain, List<Possibilities> allPossibilities, out List<Possibilities> newPossibilities, bool validationControl = false)
        {
            #region . Validation Control .
            if (validationControl && _IsSolved(matrix, true))
            {
                lastCertain = matrix.Clone() as Sudoku;
                newPossibilities = null;
                return true;
            }
            #endregion
            #region . Declarations .
            bool possChanged = false;
            var curr = matrix.Clone() as Sudoku;
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : AllPossibilities(curr).ToList();
            #endregion
            #region . General Loop .
            do
            {
                var newPos = new List<Possibilities>();
                var yWings = GetYWings(_allPossibilities);
                foreach (var yWing in yWings)
                {
                    var seeings1 = Sudoku.LinkedCellsOf(yWing.Pincer1);
                    var seeings2 = Sudoku.LinkedCellsOf(yWing.Pincer2);
                    var shared = seeings1.Intersect(seeings2).Except(new[] { yWing.Pivot }).ToList();
                    newPos.AddRange(allPossibilities
                                    .Where(x => shared.Any(y => y.Equals(x.Index)))
                                    .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { yWing.Pincer1ToPincer2 })))
                                    .ToList());
                }
                if (possChanged = newPos.Count > 0)
                {
                    var singles = newPos.Where(x => x.PossibleNumbers.Count == 1).ToList();
                    if (singles.Count > 0)
                    {
                        foreach (var cell in singles)
                            curr[cell.Index] = cell.PossibleNumbers.Single();
                        lastCertain = curr.Clone() as Sudoku;
                        newPossibilities = null;
                        return true;
                    }
                    _allPossibilities = newPos.Concat(_allPossibilities.Where(x => !newPos.Any(y => y.Index.Equals(x.Index))).ToList()).ToList();
                }
            } while (possChanged);
            #endregion
            #region . Return Value .
            lastCertain = curr.Clone() as Sudoku;
            newPossibilities = possChanged ? _allPossibilities : null;
            return false;
            #endregion
        }
        private static bool TrySetAnyValues_7_Swordfish(Sudoku matrix, out Sudoku lastCertain, List<Possibilities> allPossibilities, out List<Possibilities> newPossibilities, bool validationControl = false)
        {
            #region . Validation Control .
            if (validationControl && _IsSolved(matrix, true))
            {
                lastCertain = matrix.Clone() as Sudoku;
                newPossibilities = null;
                return true;
            }
            #endregion
            #region . Declarations .
            bool possChanged = false;
            var curr = matrix.Clone() as Sudoku;
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : AllPossibilities(curr).ToList();
            var allNumbers = IntSudoku.ValidNumbers.ToList();
            Func<Possibilities, Possibilities, Possibilities, int, bool> isRowOK = (p1, p2, p3, i) =>
            {
                return ((p1.PossibleNumbers.Count == 1 || !p1.PossibleNumbers.Contains(i) ? 1 : 0) +
                        (p2.PossibleNumbers.Count == 1 || !p2.PossibleNumbers.Contains(i) ? 1 : 0) +
                        (p3.PossibleNumbers.Count == 1 || !p3.PossibleNumbers.Contains(i) ? 1 : 0)) <= 1;
            };
            Func<int, int, int, int, int, int, int, bool> areColumnsOK = (row1, row2, row3, column1, column2, column3, number) =>
            {
                Possibilities p = null;
                int i1 = ((p = _allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(row1, column1)))) == null ? 1 : (p.PossibleNumbers.Contains(number) ? 0 : 1))
                       + ((p = _allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(row2, column1)))) == null ? 1 : (p.PossibleNumbers.Contains(number) ? 0 : 1))
                       + ((p = _allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(row3, column1)))) == null ? 1 : (p.PossibleNumbers.Contains(number) ? 0 : 1));
                if (i1 >= 1)
                    return false;
                int i2 = ((p = _allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(row1, column2)))) == null ? 1 : (p.PossibleNumbers.Contains(number) ? 0 : 1))
                       + ((p = _allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(row2, column2)))) == null ? 1 : (p.PossibleNumbers.Contains(number) ? 0 : 1))
                       + ((p = _allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(row3, column2)))) == null ? 1 : (p.PossibleNumbers.Contains(number) ? 0 : 1));
                if (i2 >= 1)
                    return false;
                int i3 = ((p = _allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(row1, column3)))) == null ? 1 : (p.PossibleNumbers.Contains(number) ? 0 : 1))
                       + ((p = _allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(row2, column3)))) == null ? 1 : (p.PossibleNumbers.Contains(number) ? 0 : 1))
                       + ((p = _allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(row3, column3)))) == null ? 1 : (p.PossibleNumbers.Contains(number) ? 0 : 1));
                if (i3 >= 1)
                    return false;
                return true;
            };
            #endregion
            #region . General Loop .
            do
            {
                possChanged = false;
                foreach (var number in allNumbers)
                {
                    #region . Getting All Triples .
                    var tuple = new List<Tuple<int, int[]>>();
                    for (int r = 0; r < Sudoku.GeneralFixedLength; r++)
                    {
                        var cells = _allPossibilities.Where(x => x.Index.RowIndex == r).ToList().Concat(curr[SubArrayType.Row, r]
                                                                                                .OrderBy(x => x)
                                                                                                .Where(x => x.HasValue)
                                                                                                .Select((x, c) => new Possibilities(new IndicesPair(r, c), new[] { x.Value }))
                                                                                                .ToList()).ToList();
                        var combinations = GetCombination(3).Where(x => !x.Any(y => y > cells.Count - 1)).Select(x => x.Select(y => (int)y).ToArray()).ToList();
                        foreach (var columns in combinations)
                        {
                            var cell1 = cells[columns[0]];
                            var cell2 = cells[columns[1]];
                            var cell3 = cells[columns[2]];
                            if (isRowOK(cell1, cell2, cell3, number))
                                tuple.Add(new Tuple<int, int[]>(r, (new[] { cell1.Index.ColumnIndex, cell2.Index.ColumnIndex, cell3.Index.ColumnIndex }).OrderBy(x => x).ToArray()));
                        }
                    }
                    var triples = tuple.Select(x => new { RowIndex = x.Item1, ColumnIndices = x.Item2 }).ToList();
                    #endregion
                    #region . Deciding which ones are Swordfish .
                    for (int i = 0; i < triples.Count; i++)
                    {
                        var iCells = triples[i];
                        for (int j = i + 1; j < triples.Count; j++)
                        {
                            var jCells = triples[j];
                            #region . Deciding whether cells are valid or not .
                            if (iCells.RowIndex == jCells.RowIndex || !iCells.ColumnIndices.SequenceEqual(jCells.ColumnIndices))
                                continue;
                            #endregion
                            for (int k = j + 1; k < triples.Count; k++)
                            {
                                var kCells = triples[k];
                                #region . Deciding whether cells are valid or not .
                                if (iCells.RowIndex == kCells.RowIndex || jCells.RowIndex == kCells.RowIndex || !jCells.ColumnIndices.SequenceEqual(kCells.ColumnIndices) ||
                                    !areColumnsOK(iCells.RowIndex, jCells.RowIndex, kCells.RowIndex, iCells.ColumnIndices[0], iCells.ColumnIndices[1], iCells.ColumnIndices[2], number))
                                    continue;
                                #endregion
                                var rows = new[] { iCells.RowIndex, jCells.RowIndex, kCells.RowIndex };
                                var newPos = _allPossibilities.Where(x => x.PossibleNumbers.Contains(number) && (rows.Contains(x.Index.RowIndex) ?
                                                                                                                 !iCells.ColumnIndices.Contains(x.Index.ColumnIndex) :
                                                                                                                 iCells.ColumnIndices.Contains(x.Index.ColumnIndex)))
                                                              .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { number }).ToList()))
                                                              .ToList();
                                if (newPos.Count > 0)
                                {
                                    possChanged = true;
                                    _allPossibilities = newPos.Concat(_allPossibilities.Where(x => !newPos.Any(y => y.Index.Equals(x.Index)))).ToList();
                                }
                                if (possChanged)
                                {
                                    #region . Setting Values to Singles .
                                    var singles = _allPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                                    if (singles.Count > 0)
                                    {
                                        foreach (var cell in singles)
                                            curr[cell.Index] = cell.PossibleNumbers.Single();
                                        lastCertain = curr.Clone() as Sudoku;
                                        newPossibilities = null;
                                        return true;
                                    }
                                    #endregion
                                    break;
                                }
                            }
                            if (possChanged)
                                break;
                        }
                        if (possChanged)
                            break;
                    }
                    #endregion
                    if (possChanged)
                        break;
                }
            } while (possChanged);
            #endregion
            #region . Return Value .
            lastCertain = curr.Clone() as Sudoku;
            newPossibilities = possChanged ? _allPossibilities : null;
            return false;
            #endregion
        }

        private static bool _TrySolve(Sudoku matrix, out Sudoku solution, bool onlySimples = false, bool validationControl = true)
        {
            #region . Validation Control .
            if (_IsSolved(matrix, true))
            {
                solution = matrix.Clone() as Sudoku;
                return true;
            }
            #endregion
            #region . Declarations .
            Sudoku prev = null, curr = matrix.Clone() as Sudoku;
            #endregion
            #region . Other Strategies .
            TrySetAnyValues_0_ForcedValues(curr, out curr, null);
            if (!onlySimples && !_IsSolved(curr))
            {
                List<Possibilities> newPos = null, _np = null;
                Func<List<Possibilities>> newPossibilities = () => { return _np != null && _np.Count > 0 ? (newPos = new List<Possibilities>(_np)) : newPos; };
                do
                {
                    prev = curr.Clone() as Sudoku;
                    if ((TrySetAnyValues_1_IntersectionRemoval(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
                    || (TrySetAnyValues_2_NakedTuples(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
                    || (TrySetAnyValues_3_HiddenTuples(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
                    || (TrySetAnyValues_4_XWing(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
                    || (TrySetAnyValues_5_SingleChains(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
                    || (TrySetAnyValues_6_YWing(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
                    || (TrySetAnyValues_7_Swordfish(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true)))
                    {
                        solution = curr.Clone() as Sudoku;
                        return true;
                    }
                } while (!prev.Equals(curr));
            }
            #endregion
            #region . Return Value .
            return _IsSolved((solution = curr.Clone() as Sudoku));
            #endregion
        }
        private static IEnumerable<Sudoku> _AllSolutions(Sudoku matrix, uint i, bool trySolve = true)
        {
            if (i == 0)
                throw new StackOverflowException();
            if (!IsValid(matrix))
                throw new InvalidMatrixException("matrix");
            var curr = matrix.Clone() as Sudoku;
            bool ok = false;
            if (trySolve)
            {
                var sol = GetSolution(curr);
                if (ok = sol.HasSolution)
                    yield return sol.Solution;
            }
            if (!trySolve || !ok)
            {
                var firstEmpty = EmptyCells(curr).First();
                var potVals = PotentialValuesOf(curr, firstEmpty).ToList();
                foreach (var val in potVals)
                {
                    curr[firstEmpty] = val;
                    var sols = new List<Sudoku>();
                    try
                    {
                        sols = _AllSolutions(curr, i - 1).ToList();
                    }
                    catch (InvalidMatrixException)
                    {
                        continue;
                    }
                    foreach (var sol in sols)
                        yield return sol;
                }
            }
        }
        private static IEnumerable<Sudoku> _AllSolutions(Sudoku matrix, bool trySolve = true)
        {
            if (!IsValid(matrix))
                throw new InvalidMatrixException("matrix");
            var curr = matrix.Clone() as Sudoku;
            bool ok = false;
            if (trySolve)
            {
                var sol = GetSolution(curr);
                if (ok = sol.HasSolution)
                    yield return sol.Solution;
            }
            if (!trySolve || !ok)
            {
                var firstEmpty = EmptyCells(curr).First();
                var potVals = PotentialValuesOf(curr, firstEmpty).ToList();
                foreach (var val in potVals)
                {
                    curr[firstEmpty] = val;
                    var sols = new List<Sudoku>();
                    try
                    {
                        sols = _AllSolutions(curr).ToList();
                    }
                    catch (InvalidMatrixException)
                    {
                        continue;
                    }
                    foreach (var sol in sols)
                        yield return sol;
                }
            }
        }
        #endregion

        #region . Finalizer Functions .
        public static SudokuSolution GetSolution(Sudoku matrix)
        {
            #region . Validation Control .
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            if (!IsValid(matrix))
                throw new InvalidMatrixException(nameof(matrix));
            if (_IsSolved(matrix, false))
                return new SudokuSolution()
                {
                    HasSolution = true,
                    Matrix = matrix.Clone() as Sudoku,
                    _possibilitiesOfEmptyCells = null,
                    Solution = matrix.Clone() as Sudoku
                };
            #endregion
            #region . Declarations .
            bool possChanged = false;
            int n = -1, k = -1, i = -1, length = -1;
            Sudoku curr = matrix.Clone() as Sudoku;
            SudokuSolution sol = null;
            List<IntSudoku> allNumbers = IntSudoku.ValidNumbers.ToList(), promisingNumbers = null;
            IntSudoku[] __numbers = null;
            IntSudoku number = 1, val = 1, __value = 1;
            List<Possibilities> allPossibilities = AllPossibilities(curr).ToList(), __newPossibilities = null, singles = null, cells = null, unit = null,
                containers = null, specificContainers = null;
            Possibilities cell = null, first = null;
            IndicesPair __index = new IndicesPair();
            IndicesPair[] __indices = null;
            Action concatPossibilities = () =>
            {
                if (possChanged = __newPossibilities.Count > 0)
                    allPossibilities = __newPossibilities.Concat(allPossibilities.Where(x => !__newPossibilities.Any(y => y.Index.Equals(x.Index))).ToList()).ToList();
            };
            Action removePossibleNumbers = () =>
            {
                __newPossibilities = allPossibilities.Where(x => __indices.Contains(x.Index)).Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(__numbers).ToList())).ToList();
                concatPossibilities();
            };
            Action setValue = () =>
            {
                curr[__index] = __value;
                allPossibilities.RemoveAll(x => x.Index.Equals(__index));
                __indices = Sudoku.LinkedCellsOf(__index);
                __numbers = new[] { __value };
                if (__indices.Any(x => curr[x].HasValue && curr[x] == __value))
                    throw new InvalidMatrixException();
                if (allPossibilities.Count == 0)
                {
                    sol = new SudokuSolution()
                    {
                        HasSolution = true,
                        Matrix = matrix.Clone() as Sudoku,
                        Solution = curr.Clone() as Sudoku,
                        _possibilitiesOfEmptyCells = null
                    };
                    possChanged = true;
                    return;
                }
                removePossibleNumbers();
            };
            #endregion
            #region . General Loop .
            do
            {
                #region . 0 | Basic Strategies .
                #region . Setting Values to Singles .
                while (true)
                {
                    singles = allPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList();
                    if (singles.Count == 0)
                        break;
                    for (i = 0; i < singles.Count; i++)
                    {
                        __index = singles[i].Index;
                        __value = singles[i].PossibleNumbers.Single();
                        setValue();
                    }
                }
                if (sol != null)
                    break;
                possChanged = false;
                #endregion
                #region . Setting Values to Forced Cells .
                for (k = 0; k < 3; k++)
                {
                    for (i = 0, length = k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : (k == 2 ? Sudoku.CountOfSquares : -1)); i < length; i++)
                    {
                        unit = (k == 0 ? allPossibilities.Where(x => x.Index.RowIndex == i) :
                               (k == 1 ? allPossibilities.Where(x => x.Index.ColumnIndex == i) :
                               (k == 2 ? allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                               new List<Possibilities>()))).ToList();
                        if (unit.Count == 0)
                            continue;
                        if (unit.Count == 1)
                        {
                            cell = unit.Single();
                            __index = cell.Index;
                            __value = cell.PossibleNumbers.Single();
                            setValue();
                            break;
                        }
                        promisingNumbers = unit.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                        for (n = 0; n < promisingNumbers.Count; n++)
                        {
                            number = promisingNumbers[n];
                            containers = unit.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                            if (containers.Count == 1)
                            {
                                cell = containers.Single();
                                __index = cell.Index;
                                __value = number;
                                setValue();
                                break;
                            }
                        }
                        if (possChanged)
                            break;
                    }
                    if (possChanged)
                        break;
                }
                #endregion
                if (possChanged)
                    continue;
                #endregion
                #region . 1 | Intersection Removal .
                for (k = 0; k < 3; k++)
                {
                    for (i = 0, length = k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : (k == 2 ? Sudoku.CountOfSquares : -1)); i < length; i++)
                    {
                        #region . Preparing for i-th of k-th Unit .
                        cells = (k == 0 ? (allPossibilities.Where(x => x.Index.RowIndex == i)) :
                                (k == 1 ? (allPossibilities.Where(x => x.Index.ColumnIndex == i)) :
                                (k == 2 ? allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                                new List<Possibilities>()))).ToList();
                        if (cells.Count == 0)
                            continue;
                        if (cells.Count == 1)
                        {
                            cell = cells.Single();
                            __index = cell.Index;
                            __value = cell.PossibleNumbers.Single();
                            setValue();
                            break;
                        }
                        #endregion
                        #region . Pointing Tuples (k<2) & Box/Line Reduction (k=2) .
                        promisingNumbers = cells.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                        for (n = 0; n < promisingNumbers.Count; n++)
                        {
                            number = promisingNumbers[n];
                            containers = cells.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                            if (containers.Count > Sudoku.FixedLengthForSquares)
                                continue;
                            first = containers.First();
                            specificContainers = (k == 0 || k == 1 ?
                                                 (containers.All(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == Sudoku.LinkedSquareIndexOfCell(first.Index)) ?
                                                                      (allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == Sudoku.LinkedSquareIndexOfCell(first.Index) &&
                                                                                                   !containers.Any(y => y.Index.Equals(x.Index)))) :
                                                                      new List<Possibilities>()) :
                                                 (k == 2 ?
                                                 (containers.All(x => x.Index.RowIndex == first.Index.RowIndex) ?
                                                                      (allPossibilities.Where(x => x.Index.RowIndex == first.Index.RowIndex &&
                                                                                                   !containers.Any(y => y.Index.Equals(x.Index)))) :
                                                                      (containers.All(x => x.Index.ColumnIndex == first.Index.ColumnIndex) ?
                                                                      allPossibilities.Where(x => x.Index.ColumnIndex == first.Index.ColumnIndex &&
                                                                                                  !containers.Any(y => y.Index.Equals(x.Index))) :
                                                                      new List<Possibilities>())) :
                                                 new List<Possibilities>()))
                                                 .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { number }).ToList())).ToList();
                            if (specificContainers.Count > 0)
                            {
                                __newPossibilities = specificContainers
                                                     .Where(x => allPossibilities.Single(y => y.Index.Equals(x.Index)).PossibleNumbers.Count > x.PossibleNumbers.Count).ToList();
                                concatPossibilities();
                                break;
                            }
                        }
                        if (possChanged)
                            break;
                        #endregion
                    }
                    if (possChanged)
                        break;
                }
                if (possChanged)
                    continue;
                #endregion
            } while (possChanged && sol == null);
            #endregion
            #region . Return Value .
            if (sol != null)
                return sol;
            return new SudokuSolution()
            {
                HasSolution = false,
                Matrix = matrix.Clone() as Sudoku,
                _possibilitiesOfEmptyCells = new List<Possibilities>(allPossibilities),
                Solution = curr.Clone() as Sudoku
            };
            #endregion
        }
        public static bool TrySolve(Sudoku matrix, out Sudoku solution)
        {
            return _TrySolve(matrix, out solution, false, true);
        }
        public static IEnumerable<Sudoku> AllSolutions(Sudoku matrix, bool trySolveAtFirst = true)
        {
            return _AllSolutions(matrix, 1000, trySolveAtFirst);
        }
        public static IEnumerable<Sudoku> AllSolutions(Sudoku matrix, bool forceToSolve, bool trySolveAtFirst = true)
        {
            return forceToSolve ? _AllSolutions(matrix, trySolveAtFirst) : AllSolutions(matrix, trySolveAtFirst);
        }
        #endregion
        #endregion
    }
}