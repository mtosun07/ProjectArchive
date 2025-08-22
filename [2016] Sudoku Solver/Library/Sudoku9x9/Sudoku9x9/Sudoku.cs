using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku9x9
{
    public class Sudoku : IEnumerable, IEquatable<Sudoku>, ICloneable
    {
        public Sudoku(IntSudoku?[,] matrix9x9)
        {
            if (matrix9x9 == null)
                throw new ArgumentNullException("matrix9x9");
            if (matrix9x9.GetLength(0) != Length_Row || matrix9x9.GetLength(1) != Length_Column)
                throw new LengthNotValidException("matrix9x9");
            _matrix = matrix9x9;
        }

        public const int GeneralFixedLength = 9;
        public readonly static int Length_Row = GeneralFixedLength;
        public readonly static int Length_Column = GeneralFixedLength;
        public readonly static int CountOfSquares = GeneralFixedLength;
        public readonly static int FixedLengthForSquares = (int)Math.Sqrt(GeneralFixedLength);
        public readonly static int CountOfCells = Length_Row * Length_Column;

        private IntSudoku?[,] _matrix;

        public IntSudoku? this[IndicesPair indices]
        {
            get { return _matrix[indices.RowIndex, indices.ColumnIndex]; }
            set { _matrix[indices.RowIndex, indices.ColumnIndex] = value; }
        }
        public IntSudoku? this[int squareIndex, IndicesPair subIndices]
        {
            get
            {
                var i = RealCellIndices(squareIndex, subIndices);
                return _matrix[i.RowIndex, i.ColumnIndex];
            }
            set
            {
                var i = RealCellIndices(squareIndex, subIndices);
                _matrix[i.RowIndex, i.ColumnIndex] = value;
            }
        }
        public IntSudoku?[] this[SubArrayType type, int index]
        {
            get
            {
                if (type != SubArrayType.Row && type != SubArrayType.Column && type != SubArrayType.Square)
                    throw new ArgumentOutOfRangeException(nameof(type));
                int length = type == SubArrayType.Row ? Length_Row : (type == SubArrayType.Column ? Length_Column : CountOfSquares);
                if (index < 0 || index > length - 1)
                    throw new IndexOutOfRangeException();
                var values = new IntSudoku?[length];
                if (type != SubArrayType.Square)
                    for (int i = 0; i < length; i++)
                        values[i] = type == SubArrayType.Row ? _matrix[index, i] : _matrix[i, index];
                else
                {
                    var square = this[index];
                    for (int k = 0, i = 0; i < square.GetLength(0); i++)
                        for (int j = 0; j < square.GetLength(1); j++)
                            values[k++] = square[i, j];
                }
                return values;
                //if (type != SubArrayType.Row && type != SubArrayType.Column)
                //    throw new ArgumentException("Type was not neither Row nor Column.", "type");
                //int length = type == SubArrayType.Row ? Length_Div0 : Length_Div1;
                //if (startIndex < 0 || startIndex > length - 1)
                //    throw new IndexOutOfRangeException();
                //var values = new IntSudoku?[length];
                //for (int i = 0; i < length; i++)
                //    values[i] = type == SubArrayType.Row ? _matrix[startIndex, i] : _matrix[i, startIndex];
                //return values;
            }
            set
            {
                if (!type.IsFlagDefined())
                    throw new ArgumentOutOfRangeException(nameof(type));
                int length = type == SubArrayType.Row ? Length_Row : (type == SubArrayType.Column ? Length_Column : CountOfSquares);
                if (index < 0 || index > length - 1)
                    throw new IndexOutOfRangeException();
                if (value == null)
                    throw new ArgumentNullException();
                if (value.Length != length)
                    throw new LengthNotValidException();
                switch (type)
                {
                    case SubArrayType.Row:
                        for (int i = 0; i < length; i++)
                            _matrix[index, i] = value[i];
                        break;
                    case SubArrayType.Column:
                        for (int i = 0; i < length; i++)
                            _matrix[i, index] = value[i];
                        break;
                    case SubArrayType.Square:
                        var square = new IntSudoku?[FixedLengthForSquares, FixedLengthForSquares];
                        for (int k = 0, i = 0; i < FixedLengthForSquares; i++)
                            for (int j = 0; j < FixedLengthForSquares; j++)
                                square[i, j] = value[k++];
                        this[index] = square;
                        break;
                }
                //if (type != SubArrayType.Row && type != SubArrayType.Column)
                //    throw new ArgumentException("Type was not neither Row nor Column.", "type");
                //int length = type == SubArrayType.Row ? Length_Div0 : Length_Div1;
                //if (index < 0 || index > length - 1)
                //    throw new IndexOutOfRangeException();
                //if (value == null)
                //    throw new ArgumentNullException();
                //if (value.Length != length)
                //    throw new ArgumentOutOfRangeException("Argument length was out of range.");
                //switch (type)
                //{
                //    case SubArrayType.Row:
                //        for (int i = 0; i < length; i++)
                //            _matrix[index, i] = value[i];
                //        break;
                //    case SubArrayType.Column:
                //        for (int i = 0; i < length; i++)
                //            _matrix[i, index] = value[i];
                //        break;
                //}
            }
        }
        public IntSudoku?[,] this[int squareIndex]
        {
            get
            {
                var indices = ZeroPointOfSquare(squareIndex);
                var square = new IntSudoku?[FixedLengthForSquares, FixedLengthForSquares];
                for (int i = 0; i < FixedLengthForSquares; i++)
                    for (int j = 0; j < FixedLengthForSquares; j++)
                        square[i, j] = _matrix[indices.RowIndex + i, indices.ColumnIndex + j];
                return square;
            }
            set
            {
                var indices = ZeroPointOfSquare(squareIndex);
                if (value == null)
                    throw new ArgumentNullException();
                if (value.Length != CountOfSquares || value.GetLength(0) != value.GetLength(1))
                    throw new LengthNotValidException();
                for (int i = 0; i < FixedLengthForSquares; i++)
                    for (int j = 0; j < FixedLengthForSquares; j++)
                        _matrix[indices.RowIndex + i, indices.ColumnIndex + j] = value[i, j];
            }
        }



        public IntSudoku? GetCell(IndicesPair indices)
        {
            return this[indices];
        }
        public IntSudoku? GetCell_fromSquare(int squareIndex, IndicesPair indices)
        {
            return this[squareIndex, indices];
        }
        public IntSudoku?[] GetRow(int rowIndex)
        {
            return this[SubArrayType.Row, rowIndex];
        }
        public IntSudoku?[] GetColumn(int columnIndex)
        {
            return this[SubArrayType.Row, columnIndex];
        }
        public IntSudoku?[] GetSquare_oneDimension(int squareIndex)
        {
            return this[SubArrayType.Square, squareIndex];
        }
        public IntSudoku?[,] GetSquare(int squareIndex)
        {
            return this[squareIndex];
        }
        public void SetCell(IndicesPair indices, IntSudoku? value)
        {
            this[indices] = value;
        }
        public void SetCell_fromSquare(int squareIndex, IndicesPair subIndices, IntSudoku? value)
        {
            this[squareIndex, subIndices] = value;
        }
        public void SetRow(int rowIndex, IntSudoku?[] values)
        {
            this[SubArrayType.Row, rowIndex] = values;
        }
        public void SetColumn(int columnIndex, IntSudoku?[] values)
        {
            this[SubArrayType.Row, columnIndex] = values;
        }
        public void SetSquare_OneDimension(int squareIndex, IntSudoku?[] values)
        {
            this[SubArrayType.Square, squareIndex] = values;
        }
        public void SetSquare(int squareIndex, IntSudoku?[,] values)
        {
            this[squareIndex] = values;
        }

        public static IndicesPair RealCellIndices(int squareIndex, int subIndex_oneDim)
        {
            if (squareIndex < 0 || subIndex_oneDim < 0 || squareIndex > CountOfSquares - 1 || subIndex_oneDim > GeneralFixedLength - 1)
                throw new IndexOutOfRangeException();
            int realRowIndex = ((squareIndex / FixedLengthForSquares) * FixedLengthForSquares) + (subIndex_oneDim / FixedLengthForSquares);
            int realColumnIndex = ((squareIndex % FixedLengthForSquares) * FixedLengthForSquares) + (subIndex_oneDim % FixedLengthForSquares);
            return new IndicesPair(realRowIndex, realColumnIndex);
        }
        public static IndicesPair RealCellIndices(int squareIndex, IndicesPair subIndices)
        {
            if (squareIndex < 0 || squareIndex > CountOfSquares - 1 || subIndices.RowIndex < 0 || subIndices.RowIndex > FixedLengthForSquares - 1 || subIndices.ColumnIndex < 0 || subIndices.ColumnIndex > FixedLengthForSquares - 1)
                throw new IndexOutOfRangeException();
            int realRowIndex = (squareIndex / FixedLengthForSquares) * FixedLengthForSquares + subIndices.RowIndex;
            int realColumnIndex = (squareIndex % FixedLengthForSquares) * FixedLengthForSquares + subIndices.ColumnIndex;
            return new IndicesPair(realRowIndex, realColumnIndex);
        }
        public static IndicesPair ZeroPointOfSquare(int squareIndex)
        {
            if (squareIndex < 0 || squareIndex > CountOfSquares - 1)
                throw new IndexOutOfRangeException();
            int realRowIndex = (squareIndex / FixedLengthForSquares) * FixedLengthForSquares;
            int realColumnIndex = (squareIndex % FixedLengthForSquares) * FixedLengthForSquares;
            return new IndicesPair(realRowIndex, realColumnIndex);
        }
        public static int LinkedSquareIndexOfCell(IndicesPair cell)
        {
            if (cell.RowIndex < 0 || cell.ColumnIndex < 0 || cell.RowIndex > GeneralFixedLength || cell.ColumnIndex > GeneralFixedLength)
                throw new IndexOutOfRangeException();
            return ((cell.RowIndex / FixedLengthForSquares) * 3) + (cell.ColumnIndex / FixedLengthForSquares);
        }

        public static IndicesPair[] LinkedCellsOf(IndicesPair cell)
        {
            var l = new List<IndicesPair>();
            var zeroSq = ZeroPointOfSquare(LinkedSquareIndexOfCell(cell));
            for (int i = 0; i < FixedLengthForSquares; i++)
                for (int j = 0; j < FixedLengthForSquares; j++)
                    l.Add(new IndicesPair(zeroSq.RowIndex + i, zeroSq.ColumnIndex + j));
            for (int i = 0; i < GeneralFixedLength; i++)
            {
                l.Add(new IndicesPair(cell.RowIndex, i));
                l.Add(new IndicesPair(i, cell.ColumnIndex));
            }
            return l.Distinct().Except(new[] { cell }).OrderBy(x => x).ToArray();
        }

        public IEnumerator GetEnumerator()
        {
            return _matrix.GetEnumerator();
        }
        public bool Equals(Sudoku other)
        {
            if (other == null)
                return false;
                //throw new ArgumentNullException("other");
            if (_matrix.GetLength(0) != other._matrix.GetLength(0) || _matrix.GetLength(1) != other._matrix.GetLength(1))
                return false;
            for (int i = 0; i < _matrix.GetLength(0); i++)
                for (int j = 0; j < _matrix.GetLength(1); j++)
                    if (_matrix[i, j] != other._matrix[i, j])
                        return false;
            return true;
        }
        public object Clone()
        {
            return new Sudoku(_matrix.Clone() as IntSudoku?[,]);
        }

        public override bool Equals(object obj)
        {
            return obj == null || GetType() != obj.GetType() ? false : Equals(obj as Sudoku);
        }
        public override int GetHashCode()
        {
            return _matrix.GetHashCode();
        }
        public override string ToString()
        {
            string str = string.Empty, newLine = Environment.NewLine;
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int j = 0; j < _matrix.GetLength(1); j++)
                    str += $"[{(_matrix[i, j].HasValue ? _matrix[i, j].ToString() : " ")}]";
                if (i != _matrix.GetLength(0) - 1)
                    str += newLine;
            }
            return str;
        }
    }
}