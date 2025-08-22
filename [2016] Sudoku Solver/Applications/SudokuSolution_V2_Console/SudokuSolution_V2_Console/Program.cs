using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
//using Chains = System.Collections.Generic.List<System.Collections.Generic.List<SudokuSolution_V2_Console.SudokuSolution.ChainForm>>;
using static System.Console;

namespace SudokuSolution_V2_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Title = "SUDOKU Solver - alfa";
            ForegroundColor = ConsoleColor.DarkBlue;
            BackgroundColor = ConsoleColor.White;
            Clear();

            //WriteLine(string.Join("\n", IntSudoku.ValidNumbers.Combinations(2).Select(xx => $"({string.Join(", ", xx)})")));
            //ReadLine();

            Func<string, Sudoku> GetSudoku = (sudokuString) =>
            {
                var _matrix = new IntSudoku?[9, 9];
                for (int k = 0, i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                    {
                        var chr = sudokuString[k++];
                        IntSudoku n = 1;
                        _matrix[i, j] = IntSudoku.TryParse(chr.ToString(), out n) ? n : (IntSudoku?)null;
                    }
                return new Sudoku(_matrix);
            };
            //Sudoku solution = null;

            #region . old sudokus .
            //var matrix = new List<List<IntSudoku?>>()
            //{
            //    new List<IntSudoku?>() {6   ,9   , null, 7,4   ,null,null,null,null},
            //    new List<IntSudoku?>() {null, null, null, 8,null,null,9   , null, null   },
            //    new List<IntSudoku?>() { null, null,4, null, 1   ,5   ,null,null,null},
            //    new List<IntSudoku?>() {5   , null, 2, null, null, null,null,3   ,4   },
            //    new List<IntSudoku?>() { null, null, 8, null, null,null,null,1   ,7   },
            //    new List<IntSudoku?>() {1   ,6   , null, 4,null,null,null, null, null},
            //    new List<IntSudoku?>() {null,8   ,9,2,null, null, null,null,null},
            //    new List<IntSudoku?>() {7   ,null, null, null, null,null, null, null,9   },
            //    new List<IntSudoku?>() {null,1   , null, null, 5   , null, null,null,null}

            //    //new List<IntSudoku?>() {5,9   ,7   ,null,4   ,null,null,3,null},
            //    //new List<IntSudoku?>() {3,4   ,8   ,null,null,null,null,6,null},
            //    //new List<IntSudoku?>() {6,1   ,2   ,null,9   ,null,null,8,4   },
            //    //new List<IntSudoku?>() {7,5   ,null,null,null,null,4   ,9,null},
            //    //new List<IntSudoku?>() {8,null,9   ,null,null,null,null,7,null},
            //    //new List<IntSudoku?>() {4,null,null,6   ,null,null,null,5,null},
            //    //new List<IntSudoku?>() {1,7   ,null,null,2   ,null,6   ,4,null},
            //    //new List<IntSudoku?>() {9,6   ,null,null,8   ,3   ,null,2,null},
            //    //new List<IntSudoku?>() {2,8   ,null,null,null,null,null,1,null}
            //};
            //Sudoku sudoku = new Sudoku(new IntSudoku?[9, 9]);
            //for (int i = 0; i < 9; i++)
            //    for (int j = 0; j < 9; j++)
            //        sudoku[new IndicesPair(i, j)] = matrix[i][j];
            #endregion

            //var sudoku = GetSudoku
            //    (
            //        //"000042780070600024000000000000000675040080000001000200064000001008030000103500000"
            //        //"690740000000800900004015000502000034008000017160400000089200000700000009010050000"
            //        //"708000009105073682000800705050084067007206003016357094972008001584731926631000478"
            //        //"002003009007500020060200700000007000000008503630000004000030497500080000700400008"
            //        //"000000000904607000076804100309701080708000301051308702007502610005403208000000000"
            //        //"100000569492056108056109240009640801064010000218035604040500016905061402621000005"
            //        //"020043069003896200960025030890560013600030000030081026300010070009674302270358090"
            //        //"091723850700851000008469007073248000000396000009175283917684532000932001326517948"
            //        //"900850000050201000600030008005070012080000070730010500100020003000109020000043006"
            //        //"158700063390056187670318509081632095006501000035000010507163908019805000863000051"
            //        //"800000000003600000070090200050007000000045700000100030001000068008500010090000400"
            //        //"704300000380060900060000005800700600170050000000800410000004180007000004000100009"
            //        //"000003060001040000400000801204900006005206107700001302807000003000070400020600000"
            //        //"090000000000007600000542100900006002050020030600100009006481000001700000030000080"
            //        "070100300028000400000007009060000070000010000050032080600500000002000750001006040"
            //    );

            while (true)
            {
                Sudoku sudoku = null;
                while (sudoku == null)
                {
                    Write("Specify the SUDOKU to Resolve (as an 'One Dimensional Array' - Type '0' or 'space' for empty cells.) >:\r\n");
                    string _sudoku = ReadLine();
                    try
                    {
                        sudoku = GetSudoku(_sudoku);
                    }
                    catch (Exception ex)
                    {
                        Write("\r\n\r\nINVALID ENTRY!\r\nError Message: " + ex.Message + "\r\nPress any key to retry...");
                        ReadKey();
                    }
                    Clear();
                }

                bool error = false;
                try
                {
                    //try
                    //{
                    WriteLine("Original:");
                    WriteLine(sudoku.ToString(true));
                    WriteLine();


                    var watch = new System.Diagnostics.Stopwatch();

                    //watch.Restart();
                    //bool solved = SudokuSolution.TrySolve(sudoku, out solution);
                    //watch.Stop();
                    //WriteLine($"Time elapsed: {watch.Elapsed}");
                    //WriteLine($"Is solved: {solved}");
                    //WriteLine("Certain solution:");
                    //WriteLine(solution);
                    //WriteLine();


                    watch.Restart();
                    var x = SudokuSolution.GetSolution(sudoku);
                    watch.Stop();
                    WriteLine("Certain solution:");
                    WriteLine(x.Solution.ToString(true));
                    WriteLine();
                    WriteLine($"Time elapsed:\t\t{watch.Elapsed}");
                    WriteLine($"Is solved:\t\t{x.HasSolution.ToString().ToUpper()}");
                    WriteLine(string.Format("Used Strategies:\t{0}",
                        x.UsedStrategies == null || x.UsedStrategies.Count == 0 ? "NONE" : string.Join("", x.UsedStrategies.Select((s, j) =>
                        "\n\t\t\t" + (j + 1).ToString().PadLeft(x.UsedStrategies.Count.ToString().Length, '0') + ": " + s.ToString()))));


                    if (!x.HasSolution)
                    {
                        watch.Restart();
                        var solutions = SudokuSolution.AllSolutions(sudoku, true, true).ToList();
                        WriteLine();
                        WriteLine(new string('-', 50));
                        WriteLine($"Count of possible solutions: {solutions.Count}");
                        WriteLine($"Time elapsed: {watch.Elapsed}");
                        watch.Stop();
                        int ii = 1;
                        foreach (var sol in solutions)
                        {
                            WriteLine();
                            WriteLine($"Solution #{ii++}:");
                            WriteLine(sol.ToString(true));
                            if (!SudokuSolution.IsValid(sol))
                                WriteLine("INVALID SUDOKU !");
                        }
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Write("ERROR >: " + ex.Message);
                    //}

                    ReadLine();
                }
                catch (Exception ex)
                {
                    error = true;
                    WriteLine("\r\n\r\nAn error occured:\r\n" + ex.ToString() + "\r\n");
                }

                Write("Press any key to resolve another SUDOKU or press ESC to quit...");
                var chr = ReadKey();
                if (chr.Key == ConsoleKey.Escape)
                    break;
                Clear();
            }

        }
    }

    #region DLL

    [Serializable]
    public class LengthNotValidException : ArgumentOutOfRangeException
    {
        private const string _message = "Length did not equal to general fixed lenght of Sudoku.";

        public LengthNotValidException() : base(string.Empty, _message) { }
        public LengthNotValidException(string paramName) : base(paramName, _message) { }
        public LengthNotValidException(string paramName, string message) : base(paramName, message) { }
        public LengthNotValidException(string paramName, object actualValue, string message) : base(paramName, actualValue, message) { }
        public LengthNotValidException(string message, Exception inner) : base(message, inner) { }
        protected LengthNotValidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidSubsetException : ArgumentException
    {
        private const string _message = "Subset of Sudoku was not valid.";

        public InvalidSubsetException() : base(_message) { }
        public InvalidSubsetException(string paramName) : base(_message, paramName) { }
        public InvalidSubsetException(string message, string paramName) : base(message, paramName) { }
        public InvalidSubsetException(string message, Exception inner) : base(message, inner) { }
        public InvalidSubsetException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
        protected InvalidSubsetException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidMatrixException : ArgumentException
    {
        private const string _message = "Matrix was not a valid sudoku.";

        public InvalidMatrixException() : base(_message) { }
        public InvalidMatrixException(string paramName) : base(_message, paramName) { }
        public InvalidMatrixException(string message, string paramName) : base(message, paramName) { }
        public InvalidMatrixException(string message, Exception inner) : base(message, inner) { }
        public InvalidMatrixException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
        protected InvalidMatrixException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidPossibilitiesException : ArgumentException
    {
        private const string _message = "Entity was unexpected.";

        public InvalidPossibilitiesException() : base(_message) { }
        public InvalidPossibilitiesException(string paramName) : base(_message, paramName) { }
        public InvalidPossibilitiesException(string message, string paramName) : base(message, paramName) { }
        public InvalidPossibilitiesException(string message, Exception inner) : base(message, inner) { }
        public InvalidPossibilitiesException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
        protected InvalidPossibilitiesException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidSwordfishException : ArgumentException
    {
        private const string _message = "Swordfish was invalid, more details should be in the inner exception.";

        public InvalidSwordfishException() : base(_message) { }
        public InvalidSwordfishException(string paramName) : base(_message, paramName) { }
        public InvalidSwordfishException(string message, string paramName) : base(message, paramName) { }
        public InvalidSwordfishException(string message, Exception inner) : base(message, inner) { }
        public InvalidSwordfishException(Exception inner) : base(_message, string.Empty, inner) { }
        public InvalidSwordfishException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
        protected InvalidSwordfishException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SolutionInfiniteException : Exception
    {
        private const string _message = "Count of solutions was almost infinite.";

        public SolutionInfiniteException() : base(_message) { }
        public SolutionInfiniteException(string message) : base(message) { }
        public SolutionInfiniteException(string message, Exception inner) : base(message, inner) { }
        protected SolutionInfiniteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public struct IntSudoku : IEquatable<IntSudoku>, IComparable<IntSudoku>
    {
        public IntSudoku(int value)
        {
            if (value > MaxValue || value < MinValue)
                throw new ArgumentOutOfRangeException("value");
            _value = value;
        }


        public const int MaxValue = 9;
        public const int MinValue = 1;

        private UInt4 _value;
        private UInt4 Value
        {
            get
            {
                _value = _value < MinValue ? (UInt4)MinValue : (_value > MaxValue ? (UInt4)MaxValue : _value);
                return _value;
            }
            set
            {
                _value = value < MinValue ? (UInt4)MinValue : (value > MaxValue ? (UInt4)MaxValue : value);
            }
        }

        public static IEnumerable<IntSudoku> ValidNumbers
        {
            get { return Enumerable.Range(MinValue, MaxValue - MinValue + 1).Select(x => (IntSudoku)x); }
        }



        public int CompareTo(IntSudoku other)
        {
            return Value.CompareTo(other.Value);
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            if (obj is IntSudoku)
                return Equals((IntSudoku)obj);
            else if (obj is UInt4)
                return Value.Equals((UInt4)obj);
            else if (obj is int)
                return ((int)Value).Equals((int)obj);
            else
                return base.Equals(obj);
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public bool Equals(IntSudoku other)
        {
            return ((int)this).Equals((int)other);
        }

        public static implicit operator int(IntSudoku value)
        {
            return value.Value;
        }
        public static implicit operator IntSudoku(int value)
        {
            return new IntSudoku(value);
        }

        public static IntSudoku Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            IntSudoku result = 1;
            if (!TryParse(s, out result))
                throw new FormatException();
            return result;
        }
        public static bool TryParse(string s, out IntSudoku result)
        {
            result = 1;
            UInt4 i = 0;
            if (string.IsNullOrEmpty(s) || !UInt4.TryParse(s, out i) || i < MinValue || i > MaxValue)
                return false;
            result = (int)i;
            return true;
        }
    }

    public enum SubArrayType : byte
    {
        Row = 1,
        Column = 2,
        Square = 4
    }

    [Flags]
    public enum SubArrayTypes : int
    {
        NONE = 0,
        Row = SubArrayType.Row,
        Column = SubArrayType.Column,
        Square = SubArrayType.Square,
        ALL = Row | Column | Square
    }

    public enum SwordfishDirection : byte
    {
        ByRows = 1,
        ByColumns = 2
    }

    public struct IndicesPair : IEquatable<IndicesPair>, IComparable<IndicesPair>
    {
        public IndicesPair(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }



        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public static IEnumerable<IndicesPair> ValidIndices
        {
            get
            {
                for (int i = 0; i < Sudoku.Length_Row; i++)
                    for (int j = 0; j < Sudoku.Length_Column; j++)
                        yield return new IndicesPair(i, j);
            }
        }



        public override string ToString()
        {
            return $"({RowIndex}, {ColumnIndex})";
        }
        public override int GetHashCode()
        {
            return 13 * RowIndex + 31 * ColumnIndex;
        }
        public override bool Equals(object obj)
        {
            return obj == null || GetType() != obj.GetType() ? false : Equals((IndicesPair)obj);
        }

        public bool Equals(IndicesPair other)
        {
            return RowIndex.Equals(other.RowIndex) && ColumnIndex.Equals(other.ColumnIndex);
        }
        public int CompareTo(IndicesPair other)
        {
            return RowIndex > other.RowIndex ? 1 : (RowIndex < other.RowIndex ? -1 : (ColumnIndex > other.ColumnIndex ? 1 : (ColumnIndex < other.ColumnIndex ? -1 : 0)));
        }
    }

    public class Sudoku : IEquatable<Sudoku>, ICloneable
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
        public string ToString(bool beautified)
        {
            if (!beautified)
                return ToString();
            var x = "ABCDEFGHJ";
            string str = string.Empty, newLine = Environment.NewLine;
            for (int i = -1; i < _matrix.GetLength(0); i++)
            {
                if (i == -1)
                {
                    str += "   1  2  3   4  5  6   7  8  9" + newLine;
                    continue;
                }
                str += (i != 0 && i % 3 == 0 ? newLine : "");
                str += x[i].ToString() + " ";
                for (int j = 0; j < _matrix.GetLength(1); j++)
                    str += (j != 0 && j % 3 == 0 ? " " : "") + $"[{(_matrix[i, j].HasValue ? _matrix[i, j].ToString() : " ")}]";
                if (i != _matrix.GetLength(0) - 1)
                    str += newLine;
            }
            return str;
        }
    }

    public enum SolvingStrategies : int
    {
        //NONE = 0,
        ForcedValues = 1,
        HiddenSingles = 2,
        NakedPairs = 4,
        HiddenPairs = 8,
        NakedTriples = 16,
        HiddenTriples = 32,
        NakedQuads = 64,
        HiddenQuads = 128,
        PointingPairs = 256,
        BoxLineReduction = 512,
        XWing = 1024,
        SingleChains = 2048,
        XYWing = 4096,
        Swordfish = 8192,
        XYZWing = 16384,
        RemotePairs = 32768,
        //XCycles = 32768,
        //Bug = 65536,
        //XYChain = 131072,
        //ThreeDMedusa = 262144,
        //Jellyfish = 524288,
        //UniqueRectangels = 1048576,
        //ExtendedUniqueRectangles = 2097152,
        //HiddenUniqueRectangles = 4194304,
        //WXYZWing = 8388608,
        //AlignedPairExclusion = 16777216,
        //x01 = 33554432,
        //x02 = 67108864,
        //x03 = 134217728,
        //x04 = 268435456,
        //x05 = 536870912,
        //x06 = 1073741824,
        //x07 = 2147483648,
        //x08 = 4294967296,
        //x09 = 8589934592,
        //x10 = 17179869184,
        //x11 = 34359738368,
        //x12 = 68719476736,
        //x13 = 137438953472,
        //x14 = 274877906944,

        //Basic_ = ForcedValues & HiddenSingles,
        //Naked_And_Hidden_Tuples = NakedPairs & HiddenPairs & NakedTriples & HiddenTriples & NakedQuads & HiddenQuads,
        //Intersection_Removal = PointingPairs & BoxLineReduction,
        //Rectangles_ = UniqueRectangels & ExtendedUniqueRectangles & HiddenUniqueRectangles,

        //SIMPLE_STRATEGIES = Basic_ & Naked_And_Hidden_Tuples & Intersection_Removal,
        //THOUGH_STRATEGIES = XWing & SingleChains & YWing & Swordfish & XYZWing,
        //DIABLOGICAL_STRATEGIES = XCycles & Bug & XYChain & ThreeDMedusa & Jellyfish & Rectangles_ & WXYZWing & AlignedPairExclusion,
        //EXTREME_STRATEGIES = x01 & x02 & x03 & x04 & x05 & x06 & x07 & x08 & x09 & x10 & x11 & x12 & x13 & x14,
        //TRIAL_AND_ERROR = 549755813888,

        //ALL = SIMPLE_STRATEGIES & THOUGH_STRATEGIES & DIABLOGICAL_STRATEGIES & EXTREME_STRATEGIES & TRIAL_AND_ERROR
    }

    public static class Helper
    {
        public static bool IsFlagDefined(this Enum e)
        {
            decimal d;
            return !decimal.TryParse(e.ToString(), out d);
        }
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
        public static IEnumerable<T> Distinct<T, TObject>(this IEnumerable<T> elements, Func<T, TObject> uniqueSelector)
        {
            return elements.GroupBy(uniqueSelector).Select(g => g.First());
        }
        public static IEnumerable<TKey> Keys<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> elements)
        {
            return elements.Select(x => x.Key).Distinct();
        }

        public static IEnumerable<SudokuSolution.Rows> GroupByRows(this IEnumerable<SudokuSolution.Possibilities> elements)
        {
            Func<SudokuSolution.Possibilities, int> keySelector = (e) => { return e.Index.RowIndex; };
            return elements
                .Select(e => keySelector(e))
                .Select(key => new SudokuSolution.Rows(key, elements.Where(e => keySelector(e) == key)));
        }
        public static IEnumerable<SudokuSolution.Columns> GroupByColumns(this IEnumerable<SudokuSolution.Possibilities> elements)
        {
            Func<SudokuSolution.Possibilities, int> keySelector = (e) => { return e.Index.ColumnIndex; };
            return elements
                .Select(e => keySelector(e))
                .Select(key => new SudokuSolution.Columns(key, elements.Where(e => keySelector(e) == key)));
        }
        public static IEnumerable<SudokuSolution.Squares> GroupBySquares(this IEnumerable<SudokuSolution.Possibilities> elements)
        {
            Func<SudokuSolution.Possibilities, int> keySelector = (e) => { return Sudoku.LinkedSquareIndexOfCell(e.Index); };
            return elements
                .Select(e => keySelector(e))
                .Select(key => new SudokuSolution.Squares(key, elements.Where(e => keySelector(e) == key)));
        }
    }

    public class SudokuSolution
    {
        private SudokuSolution()
        {
            Matrix = null;
            HasSolution = false;
            Solution = null;
            PossibilitiesOfEmptyCells = null;
        }


        public Sudoku Matrix { get; private set; }
        public bool HasSolution { get; private set; }
        public Sudoku Solution { get; private set; }
        public ReadOnlyCollection<Possibilities> PossibilitiesOfEmptyCells { get; private set; }
        public ReadOnlyCollection<SolvingStrategies> UsedStrategies { get; private set; }


        public class DistinctOrderedCollection<T, TKey> : ICollection<T>
        {
            public DistinctOrderedCollection(Func<T, TKey> orderingKeySelector)
            {
                if (orderingKeySelector == null)
                    throw new ArgumentNullException(nameof(orderingKeySelector));
                _collection = new Collection<T>();
                _orderingKeySelector = orderingKeySelector;
            }
            public DistinctOrderedCollection(IList<T> list, Func<T, TKey> orderingKeySelector)
            {
                if (list == null)
                    throw new ArgumentNullException(nameof(list));
                if (orderingKeySelector == null)
                    throw new ArgumentNullException(nameof(orderingKeySelector));
                _collection = new Collection<T>(list.Distinct().OrderBy(orderingKeySelector).ToList());
                _orderingKeySelector = orderingKeySelector;
            }


            private const string ErrorMessage = "Collection was containing the item.";
            private Collection<T> _collection;
            Func<T, TKey> _orderingKeySelector;

            public int Count
            {
                get
                {
                    return _collection.Count;
                }
            }
            bool ICollection<T>.IsReadOnly
            {
                get
                {
                    return ((ICollection<T>)_collection).IsReadOnly;
                }
            }



            public bool TryAdd(T item)
            {
                bool ok = false;
                if (ok = !Contains(item))
                    _collection.Add(item);
                _collection.OrderBy(_orderingKeySelector);
                return ok;
            }
            void ICollection<T>.Add(T item)
            {
                if (Contains(item))
                    throw new ArgumentException(ErrorMessage, nameof(item));
                _collection.Add(item);
                _collection.OrderBy(_orderingKeySelector);
            }
            public void Clear()
            {
                _collection.Clear();
            }
            public bool Contains(T item)
            {
                return _collection.Contains(item);
            }
            public void CopyTo(T[] array, int arrayIndex)
            {
                _collection.CopyTo(array, arrayIndex);
            }
            public bool Remove(T item)
            {
                return _collection.Remove(item);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _collection.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _collection.GetEnumerator();
            }
        }
        public abstract class UnitGroupingBase : IGrouping<int, Possibilities>
        {
            protected UnitGroupingBase(SubArrayType unitType, int key, List<Possibilities> elements)
            {
                if (unitType != SubArrayType.Row && unitType != SubArrayType.Column && unitType != SubArrayType.Square)
                    throw new ArgumentOutOfRangeException(nameof(unitType));
                if (key < 0 || key >= Sudoku.GeneralFixedLength)
                    throw new IndexOutOfRangeException();
                if (elements == null)
                    throw new ArgumentNullException(nameof(elements));
                Index = key;
                UnitType = unitType;
                Elements = elements;
            }

            protected readonly int Index;
            protected readonly List<Possibilities> Elements;

            public SubArrayType UnitType { get; private set; }
            int IGrouping<int, Possibilities>.Key
            {
                get { return Index; }
            }

            public IEnumerator<Possibilities> GetEnumerator()
            {
                return Elements.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)Elements).GetEnumerator();
            }
        }
        public class Rows : UnitGroupingBase
        {
            public Rows(IGrouping<int, Possibilities> grouping) : base(SubArrayType.Row, grouping.Key, grouping.ToList()) { }
            public Rows(int key, IEnumerable<Possibilities> elements) : base(SubArrayType.Row, key, elements.ToList()) { }
            public Rows(int key, List<Possibilities> elements) : base(SubArrayType.Row, key, elements) { }


            public int RowIndex
            {
                get { return base.Index; }
            }
        }
        public class Columns : UnitGroupingBase
        {
            public Columns(IGrouping<int, Possibilities> grouping) : base(SubArrayType.Column, grouping.Key, grouping.ToList()) { }
            public Columns(int key, IEnumerable<Possibilities> elements) : base(SubArrayType.Column, key, elements.ToList()) { }
            public Columns(int key, List<Possibilities> elements) : base(SubArrayType.Column, key, elements) { }


            public int ColumnIndex
            {
                get { return base.Index; }
            }
        }
        public class Squares : UnitGroupingBase
        {
            public Squares(IGrouping<int, Possibilities> grouping) : base(SubArrayType.Square, grouping.Key, grouping.ToList()) { }
            public Squares(int key, IEnumerable<Possibilities> elements) : base(SubArrayType.Square, key, elements.ToList()) { }
            public Squares(int key, List<Possibilities> elements) : base(SubArrayType.Square, key, elements) { }


            public int SquareIndex
            {
                get { return base.Index; }
            }
        }
        public class Possibilities : IEquatable<Possibilities>
        {
            private Possibilities()
            {
                Index = new IndicesPair();
                _possibleNumbers = null;
            }
            public Possibilities(IndicesPair index, IEnumerable<IntSudoku> possibleNumbers)
            {
                if (possibleNumbers == null)
                    throw new ArgumentNullException(nameof(possibleNumbers));
                Index = index;
                _possibleNumbers = possibleNumbers.ToList();
            }
            public Possibilities(IndicesPair index, List<IntSudoku> possibleNumbers)
            {
                if (possibleNumbers == null)
                    throw new ArgumentNullException(nameof(possibleNumbers));
                Index = index;
                _possibleNumbers = possibleNumbers;
            }


            public IndicesPair Index { get; private set; }

            private List<IntSudoku> _possibleNumbers;
            public ReadOnlyCollection<IntSudoku> PossibleNumbers
            {
                get { return _possibleNumbers.AsReadOnly(); }
            }



            public override string ToString()
            {
                return $"[{nameof(Index)}: {Index.ToString()}, {nameof(PossibleNumbers)}: {{{string.Join("; ", _possibleNumbers)}}}]";
            }
            public override int GetHashCode()
            {
                Func<int, int> getPrime = (index) =>
                {
                    var primes = new[] { 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };
                    if (index < primes.Length)
                        return primes[index];
                    for (int prime = 0, i = 37, j = primes.Length - index; ; i += 2)
                    {
                        bool isPrime = true;
                        int sqrt = (int)Math.Sqrt(i);
                        for (int k = 3; k <= sqrt; k++)
                            if (i % k == 0)
                            {
                                isPrime = false;
                                break;
                            }
                        if (isPrime && j-- == 0)
                            return prime;
                    }
                };
                int hash1 = Index.GetHashCode();
                int hash2 = 0;
                for (int i = 0; i < _possibleNumbers.Count; i++)
                    hash2 += getPrime(i + 1) * _possibleNumbers[i].GetHashCode();
                return hash1 + getPrime(0) * hash2;
            }
            public override bool Equals(object obj)
            {
                return (obj == null || !(obj is Possibilities)) ? this == obj : Equals(obj as Possibilities);
            }

            public bool Equals(Possibilities other)
            {
                return
                    other != null &&
                    Index.Equals(other.Index) &&
                    _possibleNumbers.OrderBy(x => x).ToList().SequenceEqual(other._possibleNumbers.OrderBy(x => x).ToList());
            }
        }
        public class PossibilitiesMulti : IEquatable<PossibilitiesMulti>
        {
            private PossibilitiesMulti()
            {
                _indices = null;
                _possibleNumbers = null;
            }
            public PossibilitiesMulti(IEnumerable<IndicesPair> indices, IEnumerable<IntSudoku> possibleNumbers)
            {
                if (possibleNumbers == null)
                    throw new ArgumentNullException(nameof(possibleNumbers));
                if (indices == null)
                    throw new ArgumentNullException(nameof(indices));
                _indices = indices.ToList();
                _possibleNumbers = possibleNumbers.ToList();
            }
            public PossibilitiesMulti(List<IndicesPair> indices, IEnumerable<IntSudoku> possibleNumbers)
            {
                if (possibleNumbers == null)
                    throw new ArgumentNullException(nameof(possibleNumbers));
                if (indices == null)
                    throw new ArgumentNullException(nameof(indices));
                _indices = indices;
                _possibleNumbers = possibleNumbers.ToList();
            }
            public PossibilitiesMulti(IEnumerable<IndicesPair> indices, List<IntSudoku> possibleNumbers)
            {
                if (possibleNumbers == null)
                    throw new ArgumentNullException(nameof(possibleNumbers));
                if (indices == null)
                    throw new ArgumentNullException(nameof(indices));
                _indices = indices.ToList();
                _possibleNumbers = possibleNumbers;
            }
            public PossibilitiesMulti(List<IndicesPair> indices, List<IntSudoku> possibleNumbers)
            {
                if (possibleNumbers == null)
                    throw new ArgumentNullException(nameof(possibleNumbers));
                if (indices == null)
                    throw new ArgumentNullException(nameof(indices));
                _indices = indices;
                _possibleNumbers = possibleNumbers;
            }


            private List<IndicesPair> _indices;
            public ReadOnlyCollection<IndicesPair> Indices
            {
                get { return _indices.AsReadOnly(); }
            }

            private List<IntSudoku> _possibleNumbers;
            public ReadOnlyCollection<IntSudoku> PossibleNumbers
            {
                get { return _possibleNumbers.AsReadOnly(); }
            }



            public override string ToString()
            {
                return $"[{nameof(Indices)}: {{{string.Join("; ", _indices)}}}, {nameof(PossibleNumbers)}: {{{string.Join("; ", _possibleNumbers)}}}]";
            }
            public override int GetHashCode()
            {
                Func<int, int> getPrime = (index) =>
                {
                    var primes = new[] { 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };
                    if (index < primes.Length)
                        return primes[index];
                    for (int prime = 0, i = 37, j = primes.Length - index; ; i += 2)
                    {
                        bool isPrime = true;
                        int sqrt = (int)Math.Sqrt(i);
                        for (int k = 3; k <= sqrt; k++)
                            if (i % k == 0)
                            {
                                isPrime = false;
                                break;
                            }
                        if (isPrime && j-- == 0)
                            return prime;
                    }
                };
                int hash1 = 0;
                int hash2 = 0;
                for (int i = 0; i < _indices.Count; i++)
                    hash1 += getPrime(i + 1) * _indices[i].GetHashCode();
                for (int i = 0; i < _possibleNumbers.Count; i++)
                    hash2 += getPrime(i + 1 + _indices.Count) * _possibleNumbers[i].GetHashCode();
                return hash1 + getPrime(0) * hash2;
            }
            public override bool Equals(object obj)
            {
                return (obj == null || !(obj is PossibilitiesMulti)) ? this == obj : Equals(obj as PossibilitiesMulti);
            }

            public bool Equals(PossibilitiesMulti other)
            {
                return
                    other != null &&
                    _indices.OrderBy(x => x).ToList().SequenceEqual(other._indices.OrderBy(x => x).ToList()) &&
                    _possibleNumbers.OrderBy(x => x).ToList().SequenceEqual(other._possibleNumbers.OrderBy(x => x).ToList());
            }
        }
        public class ChainForm : IEquatable<ChainForm>, IComparable<ChainForm>
        {
            private ChainForm()
            {
                Index = new IndicesPair();
                Number = new IntSudoku();
                Flag = new byte();
            }
            public ChainForm(IndicesPair index, IntSudoku number, byte flag)
            {
                Index = index;
                Number = number;
                Flag = flag;
            }


            public IndicesPair Index { get; private set; }
            public IntSudoku Number { get; private set; }
            public byte Flag { get; private set; }



            public override string ToString()
            {
                return $"[{nameof(Index)}: {Index}; {nameof(Number)}: {Number}; {nameof(Flag)}: {Flag}]";
            }
            public override int GetHashCode()
            {
                return Index.GetHashCode() + 13 * Number.GetHashCode() + 31 * Flag.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                return (obj == null || !(obj is ChainForm)) ? this == obj : Equals(obj as ChainForm);
            }

            public bool Equals(ChainForm other)
            {
                return other != null && Index.Equals(other.Index) && Number == other.Number && Flag == other.Flag;
            }

            public int CompareTo(ChainForm other)
            {
                if (other == null)
                    throw new ArgumentNullException("other");
                int a = 0, b = 0;
                return (a = Index.CompareTo(other.Index)) != 0 ? a : ((b = Number.CompareTo(other.Number)) != 0 ? b : Flag.CompareTo(other.Flag));
            }
        }
        public class Chains : List<List<ChainForm>> { }
        public abstract class XYWingBase<T> : IEquatable<T>
            where T: XYWingBase<T>
        {
            protected XYWingBase(IndicesPair pivot, IndicesPair pincer1, IndicesPair pincer2, IntSudoku pivotToPincer1, IntSudoku pivotToPincer2, IntSudoku commonNumber)
            {
                if (pivot.Equals(pincer1) && pivot.Equals(pincer2) && pincer1.Equals(pincer2))
                    throw new ArgumentException("Some/All of indices were the same.");
                Pivot = pivot;
                Pincer1 = pincer1;
                Pincer2 = pincer2;
                PivotToPincer1 = pivotToPincer1;
                PivotToPincer2 = pivotToPincer2;
                _commonNumber = commonNumber;
            }


            public IndicesPair Pivot { get; private set; }
            public IndicesPair Pincer1 { get; private set; }
            public IndicesPair Pincer2 { get; private set; }
            public IntSudoku PivotToPincer1 { get; private set; }
            public IntSudoku PivotToPincer2 { get; private set; }
            protected IntSudoku _commonNumber;



            public abstract T Reverse();

            public override string ToString()
            {
                return string.Format(
                    "[{0}: {1}; {2}: {3}; {4}: {5}; {6}: {7}; {8}: {9}; {10}: {11}]",
                    nameof(Pivot), Pivot,
                    nameof(Pincer1), Pincer1,
                    nameof(Pincer2), Pincer2,
                    nameof(PivotToPincer1), PivotToPincer1,
                    nameof(PivotToPincer2), PivotToPincer2,
                    nameof(_commonNumber), _commonNumber);
            }
            public override int GetHashCode()
            {
                return
                         Pivot.GetHashCode() +
                     3 * Pincer1.GetHashCode() +
                     5 * Pincer2.GetHashCode() +
                     7 * PivotToPincer1.GetHashCode() +
                    11 * PivotToPincer2.GetHashCode() +
                    13 * _commonNumber.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                return (obj == null || !(obj is XYWingBase<T>)) ? this == obj : Equals(obj as T);
            }

            public bool Equals(T other)
            {
                Func<T, bool> equals = (obj) =>
                {
                    return
                    obj != null &&
                    Pivot.Equals(obj.Pivot) &&
                    Pincer1.Equals(obj.Pincer1) &&
                    Pincer2.Equals(obj.Pincer2) &&
                    PivotToPincer1.Equals(obj.PivotToPincer1) &&
                    PivotToPincer2.Equals(obj.PivotToPincer2) &&
                    _commonNumber.Equals(obj._commonNumber);
                };
                return equals(other) || equals(other.Reverse());
            }
        }
        public class XYWing : XYWingBase<XYWing>
        {
            public XYWing(IndicesPair pivot, IndicesPair pincer1, IndicesPair pincer2, IntSudoku pivotToPincer1, IntSudoku pivotToPincer2, IntSudoku pincer1ToPincer2)
                : base(pivot, pincer1, pincer2, pivotToPincer1, pivotToPincer2, pincer1ToPincer2) { }


            public IntSudoku Pincer1ToPincer2
            {
                get { return _commonNumber; }
            }



            public override XYWing Reverse()
            {
                return new XYWing(Pivot, Pincer2, Pincer1, PivotToPincer2, PivotToPincer1, Pincer1ToPincer2);
            }
        }
        public class XYZWing : XYWingBase<XYZWing>
        {
            public XYZWing(IndicesPair pivot, IndicesPair pincer1, IndicesPair pincer2, IntSudoku pivotToPincer1, IntSudoku pivotToPincer2, IntSudoku commonNumber)
                : base(pivot, pincer1, pincer2, pivotToPincer1, pivotToPincer2, commonNumber) { }


            public IntSudoku CommonNumber
            {
                get { return _commonNumber; }
            }



            public override XYZWing Reverse()
            {
                return new XYZWing(Pivot, Pincer2, Pincer1, PivotToPincer2, PivotToPincer1, CommonNumber);
            }
        }
        public class SwordfishForm : IReadOnlyCollection<Possibilities>, IEquatable<SwordfishForm>
        {
            public SwordfishForm(Possibilities p1, Possibilities p2, Possibilities p3)
            {
                if (p1 == null)
                    throw new ArgumentNullException(nameof(p1));
                if (p1.PossibleNumbers == null || p1.PossibleNumbers.Count == 0)
                    throw new ArgumentException("Possible Numbers were null or empty.", nameof(p1));
                if (p2 == null)
                    throw new ArgumentNullException(nameof(p2));
                if (p2.PossibleNumbers == null || p2.PossibleNumbers.Count == 0)
                    throw new ArgumentException("Possible Numbers were null or empty.", nameof(p2));
                if (p3 == null)
                    throw new ArgumentNullException(nameof(p3));
                if (p3.PossibleNumbers == null || p3.PossibleNumbers.Count == 0)
                    throw new ArgumentException("Possible Numbers were null or empty.", nameof(p3));
                _triple = (new[] { p1, p2, p3 }).OrderBy(x => x.Index).ToArray();
                ValidateTriple();
            }


            private readonly Possibilities[] _triple;

            public SwordfishDirection CommonDirection { get; private set; }
            public ReadOnlyCollection<IntSudoku> CommonPossibleNumbers { get; private set; }
            public ReadOnlyCollection<Possibilities> EmptyCells { get; private set; }
            public ReadOnlyCollection<Possibilities> FullCells { get; private set; }

            public Possibilities this[int index]
            {
                get { return _triple[index]; }
            }
            public int Count
            {
                get { return _triple.Length; }
            }
            public Possibilities Cell1
            {
                get { return _triple[0]; }
            }
            public Possibilities Cell2
            {
                get { return _triple[1]; }
            }
            public Possibilities Cell3
            {
                get { return _triple[2]; }
            }



            public static bool TryCreateInstance(out SwordfishForm swff, params Possibilities[] triple)
            {
                if (triple == null)
                    throw new ArgumentNullException(nameof(triple));
                if (triple.Length != 3)
                    throw new ArgumentOutOfRangeException("Parameters' length was not 3.", nameof(triple));
                try
                {
                    swff = new SwordfishForm(triple[0], triple[1], triple[2]);
                    return true;
                }
                catch (Exception)
                {
                    swff = null;
                    return false;
                }
            }
            public static bool IsValid(params Possibilities[] triple)
            {
                if (triple == null)
                    throw new ArgumentNullException(nameof(triple));
                if (triple.Length != 3)
                    throw new ArgumentOutOfRangeException("Parameters' length was not 3.", nameof(triple));
                try
                {
                    var swff = new SwordfishForm(triple[0], triple[1], triple[2]);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            private void ValidateTriple()
            {
                if (Cell1.Index.Equals(Cell2.Index)
                 || Cell1.Index.Equals(Cell3.Index)
                 || Cell2.Index.Equals(Cell3.Index))
                    throw new ArgumentException("Some cells' indices were the same.");
                if (Cell1.Index.RowIndex == Cell2.Index.RowIndex && Cell2.Index.RowIndex == Cell3.Index.RowIndex)
                    CommonDirection = SwordfishDirection.ByRows;
                else if (Cell1.Index.ColumnIndex == Cell2.Index.ColumnIndex && Cell2.Index.ColumnIndex == Cell3.Index.ColumnIndex)
                    CommonDirection = SwordfishDirection.ByColumns;
                else
                    throw new ArgumentException("All cells were not in the same row or column.");
                if ((FullCells = new ReadOnlyCollection<Possibilities>(this.Where(x => x.PossibleNumbers.Count == 1).ToList())).Count > 1)
                    throw new ArgumentException("There were too many cells which don't contain the specified number.");
                var pns = (EmptyCells = new ReadOnlyCollection<Possibilities>(this.Where(x => x.PossibleNumbers.Count > 1).ToList()))
                    .Select(x => x.PossibleNumbers).ToList();
                List<IntSudoku> cpn = null;
                foreach (var pn in pns)
                    cpn = cpn == null ? pn.ToList() : cpn.Intersect(pn).ToList();
                if ((CommonPossibleNumbers = new ReadOnlyCollection<IntSudoku>(cpn)).Count == 0)
                    throw new ArgumentException("There was not any common possible numbers.");
            }

            public override string ToString()
            {
                return $"{nameof(Cell1)}: [{Cell1}]; {nameof(Cell2)}: [{Cell2}]; {nameof(Cell3)}: [{Cell3}]";
            }
            public override int GetHashCode()
            {
                return Cell1.GetHashCode() + 3 * Cell2.GetHashCode() + 5 * Cell3.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                return (obj == null || !(obj is SwordfishForm)) ? this == obj : Equals(obj as SwordfishForm);
            }

            public bool Equals(SwordfishForm other)
            {
                return other != null && this.SequenceEqual(other);
            }
            public IEnumerator<Possibilities> GetEnumerator()
            {
                return ((IReadOnlyCollection<Possibilities>)_triple).GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _triple.GetEnumerator();
            }
        }
        public class Swordfish : IEquatable<Swordfish>
        {
            public Swordfish(IntSudoku number, SwordfishForm triple1, SwordfishForm triple2, SwordfishForm triple3)
            {
                if (triple1 == null)
                    throw new ArgumentNullException(nameof(triple1));
                if (triple2 == null)
                    throw new ArgumentNullException(nameof(triple2));
                if (triple3 == null)
                    throw new ArgumentNullException(nameof(triple3));
                if (triple1.CommonDirection != triple2.CommonDirection || triple2.CommonDirection != triple3.CommonDirection)
                    throw new ArgumentException("Directions of forms of swordfish were not the same.");

                var ccpn = triple1.CommonPossibleNumbers.Intersect(triple2.CommonPossibleNumbers).Intersect(triple3.CommonPossibleNumbers).ToList();
                if (ccpn.Count == 0 || !ccpn.Contains(number))
                    throw new InvalidSwordfishException(
                        new ArgumentOutOfRangeException(nameof(number), "Number was not existed in intersection of all common possible numbers of triples."));
                if (triple1.Any(x => x.PossibleNumbers.Count == 1 && x.PossibleNumbers.Single() == number))
                    throw new InvalidSwordfishException(
                        new InvalidSubsetException(nameof(triple1)));
                if (triple1.Count(x => x.PossibleNumbers.Count == 1 || !x.PossibleNumbers.Contains(number)) > 1)
                    throw new InvalidSwordfishException(
                        new ArgumentException(nameof(triple1), "There were too many cells which don't contain the specified number."));
                if (triple2.Any(x => x.PossibleNumbers.Count == 1 && x.PossibleNumbers.Single() == number))
                    throw new InvalidSwordfishException(
                        new InvalidSubsetException(nameof(triple2)));
                if (triple2.Count(x => x.PossibleNumbers.Count == 1 || !x.PossibleNumbers.Contains(number)) > 1)
                    throw new InvalidSwordfishException(
                        new ArgumentException(nameof(triple2), "There were too many cells which don't contain the specified number."));
                if (triple3.Any(x => x.PossibleNumbers.Count == 1 && x.PossibleNumbers.Single() == number))
                    throw new InvalidSwordfishException(
                        new InvalidSubsetException(nameof(triple3)));
                if (triple3.Count(x => x.PossibleNumbers.Count == 1 || !x.PossibleNumbers.Contains(number)) > 1)
                    throw new InvalidSwordfishException(
                        new ArgumentException(nameof(triple3), "There were too many cells which don't contain the specified number."));
                Direction = triple1.CommonDirection;
                Number = number;
                _triple1 = triple1;
                _triple2 = triple2;
                _triple3 = triple3;
                AllCells = new ReadOnlyCollection<Possibilities>((new[]
                { triple1.Cell1, triple1.Cell2, triple1.Cell3,
                  triple2.Cell1, triple2.Cell2, triple2.Cell3,
                  triple3.Cell1, triple3.Cell2, triple3.Cell3}).OrderBy(x => x.Index).ToList());
                AllIndices = new ReadOnlyCollection<IndicesPair>((new[]
                { triple1.Cell1.Index, triple1.Cell2.Index, triple1.Cell3.Index,
                  triple2.Cell1.Index, triple2.Cell2.Index, triple2.Cell3.Index,
                  triple3.Cell1.Index, triple3.Cell2.Index, triple3.Cell3.Index}).Distinct().OrderBy(x => x).ToList());
                if (AllIndices.Count != 9)
                    throw new InvalidSwordfishException(
                        new ArgumentException("Some triples was containing some same cells."));
                if ((RowsIndices = new ReadOnlyCollection<int>(AllIndices.Select(x => x.RowIndex).Distinct().OrderBy(x => x).ToList())).Count != 3)
                    throw new InvalidSwordfishException(
                        new ArgumentException("All triples didn't form 3 different rows."));
                if ((ColumnsIndices = new ReadOnlyCollection<int>(AllIndices.Select(x => x.ColumnIndex).Distinct().OrderBy(x => x).ToList())).Count != 3)
                    throw new InvalidSwordfishException(
                        new ArgumentException("All triples didn't form 3 different columns."));
                AllPossibleNumbers = new ReadOnlyCollection<IntSudoku>(AllCells.SelectMany(x => x.PossibleNumbers).Distinct().OrderBy(x => x).ToList());
                GroupedRows = new ReadOnlyCollection<Rows>(AllCells.GroupByRows().ToList());
                GroupedColumns = new ReadOnlyCollection<Columns>(AllCells.GroupByColumns().ToList());
            }


            private SwordfishForm _triple1;
            private SwordfishForm _triple2;
            private SwordfishForm _triple3;

            public SwordfishDirection Direction { get; private set; }
            public IntSudoku Number { get; private set; }
            public ReadOnlyCollection<Possibilities> AllCells { get; private set; }
            public ReadOnlyCollection<IndicesPair> AllIndices { get; private set; }
            public ReadOnlyCollection<int> RowsIndices { get; private set; }
            public ReadOnlyCollection<int> ColumnsIndices { get; private set; }
            public ReadOnlyCollection<IntSudoku> AllPossibleNumbers { get; private set; }
            public ReadOnlyCollection<Rows> GroupedRows { get; private set; }
            public ReadOnlyCollection<Columns> GroupedColumns { get; private set; }


            public static bool TryCreateAValidInstance(out Swordfish swf, IntSudoku number, List<Possibilities> allPossibilities, params SwordfishForm[] triples)
            {
                if (allPossibilities == null)
                    throw new ArgumentNullException(nameof(allPossibilities));
                if (allPossibilities.Count == 0)
                    throw new ArgumentException("List was empty.", nameof(allPossibilities));
                if (triples == null)
                    throw new ArgumentNullException(nameof(triples));
                if (triples.Length != 3)
                    throw new ArgumentOutOfRangeException("Parameters' length was not 3.", nameof(triples));
                try
                {
                    return (swf = new Swordfish(number, triples[0], triples[1], triples[2])).IsValid(allPossibilities);
                }
                catch (Exception)
                {
                    swf = null;
                    return false;
                }
            }
            public bool IsValid(IEnumerable<Possibilities> allPossibilities)
            {
                if (allPossibilities == null)
                    throw new ArgumentNullException(nameof(allPossibilities));
                return IsValid(allPossibilities.ToList());
            }
            public bool IsValid(List<Possibilities> allPossibilities)
            {
                if (allPossibilities == null)
                    throw new ArgumentNullException(nameof(allPossibilities));
                if (allPossibilities.Count == 0)
                    throw new ArgumentException("List was empty.", nameof(allPossibilities));
                List<int> indices = null;
                List<Possibilities> p = null;
                if (Direction == SwordfishDirection.ByRows)
                {
                    foreach (var row in GroupedRows)
                    {
                        p = allPossibilities.Where(x => x.Index.RowIndex == row.RowIndex && x.PossibleNumbers.Contains(Number)).ToList();
                        indices = row.Select(x => x.Index.ColumnIndex).ToList();
                        foreach (var cell in p)
                            if (!indices.Contains(cell.Index.ColumnIndex))
                                return false;
                    }
                }
                else if (Direction == SwordfishDirection.ByColumns)
                {
                    foreach (var col in GroupedColumns)
                    {
                        p = allPossibilities.Where(x => x.Index.ColumnIndex == col.ColumnIndex && x.PossibleNumbers.Contains(Number)).ToList();
                        indices = col.Select(x => x.Index.RowIndex).ToList();
                        foreach (var cell in p)
                            if (!indices.Contains(cell.Index.RowIndex))
                                return false;
                    }
                }
                else
                    return false;
                return true;
                #region . olds .
                ////return !(allCells = AllCells.ToList()).Any(x => x.PossibleNumbers.Count == 1 && x.PossibleNumbers.Single() == Number)
                ////    && allCells.Select(x => x.Index).Distinct().Count() == 9
                ////    && !(rows = allCells.GroupByRows().ToList()).Any(x => x.Count(y => y.PossibleNumbers.Count == 1 || !y.PossibleNumbers.Contains(Number)) > 1)
                ////    && !(cols = allCells.GroupByColumns().ToList()).Any(x => x.Count(y => y.PossibleNumbers.Count == 1 || !y.PossibleNumbers.Contains(Number)) > 1)
                ////    && (Direction == SwordfishDirection.ByRows ? (!rows.Any(row => allPossibilities.Any(x => x.Index.RowIndex == row.RowIndex
                ////                                                                                          && !row.Any(y => y.Index.ColumnIndex == x.Index.ColumnIndex
                ////                                                                                          && x.PossibleNumbers.Contains(Number))))) :
                ////       (Direction == SwordfishDirection.ByColumns ? (!cols.Any(col => allPossibilities.Any(x => x.Index.ColumnIndex == col.ColumnIndex
                ////                                                                                             && !col.Any(y => y.Index.RowIndex == x.Index.RowIndex
                ////                                                                                             && x.PossibleNumbers.Contains(Number))))) :
                ////       false));
                //return (allCells = AllCells.ToList()).Select(x => x.Index).Distinct().Count() == 9
                //    && !(rows = allCells.GroupByRows().ToList()).Any(x => !IsTripleValid(x.ToList(), Direction, Number))
                //    && !(cols = allCells.GroupByColumns().ToList()).Any(x => !IsTripleValid(x.ToList(), Direction, Number))
                //    && (Direction == SwordfishDirection.ByRows ? (!(rows = allCells.GroupByRows().ToList())
                //                                                    .Any(row => allPossibilities.Any(x => x.Index.RowIndex == row.RowIndex
                //                                                                                       && !row.Any(y => y.Index.ColumnIndex == x.Index.ColumnIndex
                //                                                                                       && x.PossibleNumbers.Contains(Number))))) :
                //       (Direction == SwordfishDirection.ByColumns ? (!(cols = allCells.GroupByColumns().ToList())
                //                                                       .Any(col => allPossibilities.Any(x => x.Index.ColumnIndex == col.ColumnIndex
                //                                                                                          && !col.Any(y => y.Index.RowIndex == x.Index.RowIndex
                //                                                                                          && x.PossibleNumbers.Contains(Number))))) :
                //       false));
                #endregion
            }

            public override string ToString()
            {
                return string.Format("{0}: {1}{6}{2} #1: {3}{6}{2} #2: {4}{6}{2} #3: {5}",
                    nameof(Number), Number, 
                    Direction == SwordfishDirection.ByRows ? "Row" : (Direction == SwordfishDirection.ByColumns ? "Column" : string.Empty),
                    _triple1, _triple2, _triple3, Environment.NewLine);
            }
            public override int GetHashCode()
            {
                var primes = new[] { 5, 7, 11, 13, 17, 19, 23, 27, 29 };
                int hash = Direction.GetHashCode() + 3 * Number.GetHashCode();
                int i = 0;
                foreach (var cell in AllCells)
                    hash += primes[i++] * cell.GetHashCode();
                return hash;
            }
            public override bool Equals(object obj)
            {
                return (obj == null || !(obj is Swordfish)) ? this == obj : Equals(obj as Swordfish);
            }

            public bool Equals(Swordfish other)
            {
                return other != null && Direction == other.Direction && Number == other.Number && AllCells.SequenceEqual(other.AllCells);
            }
        }



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
                if (type != SubArrayType.Row && type != SubArrayType.Column && type != SubArrayType.Square)
                    throw new ArgumentOutOfRangeException("SubArrayType was not valid.", "type");
                if (!IsValid(matrix))
                    throw new InvalidMatrixException("matrix");
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
            if (validationControl && !IsValid(subset))
                throw new InvalidMatrixException(nameof(subset));
            for (int i = 0; i < subset.Length; i++)
                if (!subset[i].HasValue)
                    yield return i;
        }
        private static IEnumerable<IndicesPair> EmptyCells(Sudoku matrix, bool validationControl = false)
        {
            if (validationControl && !IsValid(matrix))
                throw new InvalidMatrixException(nameof(matrix));
            for (int i = 0; i < Sudoku.Length_Row; i++)
                for (int j = 0; j < Sudoku.Length_Column; j++)
                {
                    var indices = new IndicesPair(i, j);
                    if (!matrix[indices].HasValue)
                        yield return indices;
                }
        }
        private static IEnumerable<Possibilities> FullCells(Sudoku matrix, bool validationControl = false)
        {
            if (validationControl && !IsValid(matrix))
                throw new InvalidMatrixException(nameof(matrix));
            for (int i = 0; i < Sudoku.Length_Row; i++)
                for (int j = 0; j < Sudoku.Length_Column; j++)
                {
                    var indices = new IndicesPair(i, j);
                    if (matrix[indices].HasValue)
                        yield return new Possibilities(indices, new[] { matrix[indices].Value });
                }
        }
        private static IEnumerable<IntSudoku> NotPotentialValuesOf(Sudoku matrix, IndicesPair indices, bool validationControl = false)
        {
            //var links = LinkedSubsetsOfCell(matrix, indices, validationControl);
            //return links[SubArrayType.Row].Concat(links[SubArrayType.Column]).Concat(links[SubArrayType.Square]).Where(x => x.HasValue).Select(x => x.Value).Distinct().OrderBy(x => x);
            if (validationControl && !IsValid(matrix))
                throw new InvalidMatrixException(nameof(matrix));
            return Sudoku.LinkedCellsOf(indices)
                .Where(x => matrix[x].HasValue)
                .Select(x => matrix[x].Value)
                .Distinct();
        }
        private static IEnumerable<IntSudoku> PotentialValuesOf(Sudoku matrix, IndicesPair indices, bool validationControl = false)
        {
            if (validationControl && !IsValid(matrix))
                throw new InvalidMatrixException(nameof(matrix));
            return IntSudoku.ValidNumbers.Except(NotPotentialValuesOf(matrix, indices, false));
        }
        private static IEnumerable<IntSudoku> PotentialValuesOf(IntSudoku?[] subset, bool validationControl = false)
        {
            if (validationControl && !IsValid(subset))
                throw new InvalidSubsetException(nameof(subset));
            //var temp = subset.Where(x => x.HasValue).Select(x => x.Value);
            //return IntSudoku.ValidNumbers.Where(x => !temp.Contains(x)).OrderBy(x => x);
            return IntSudoku.ValidNumbers.Except(subset.Where(x => x.HasValue).Select(x => x.Value).Distinct());
        }
        private static IEnumerable<Possibilities> AllPossibilities(Sudoku matrix, bool validationControl = false)
        {
            if (validationControl && !IsValid(matrix))
                throw new InvalidMatrixException(nameof(matrix));
            return EmptyCells(matrix).Select(x => new Possibilities(x, PotentialValuesOf(matrix, x)));
        }
        public static IEnumerable<Possibilities> GetAllPossibilities(Sudoku matrix)
        {
            return AllPossibilities(matrix, true);
        }

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
        //private static IEnumerable<IntSudoku[]> GetCombination(int tupleLength, bool sameNumbers = false)
        //{
        //    if (tupleLength < 2 || tupleLength > 4)
        //        throw new ArgumentOutOfRangeException(nameof(tupleLength));
        //    var allNumbers = IntSudoku.ValidNumbers.ToList();
        //    int x = sameNumbers ? 0 : 1;
        //    switch (tupleLength)
        //    {
        //        case 2:
        //            for (int i = 0; i < allNumbers.Count; i++)
        //                for (int j = i + x; j < allNumbers.Count; j++)
        //                    yield return new[] { allNumbers[i], allNumbers[j] };
        //            break;
        //        case 3:
        //            for (int i = 0; i < allNumbers.Count; i++)
        //                for (int j = i + x; j < allNumbers.Count; j++)
        //                    for (int k = j + x; k < allNumbers.Count; k++)
        //                        yield return new[] { allNumbers[i], allNumbers[j], allNumbers[k] };
        //            break;
        //        case 4:
        //            for (int i = 0; i < allNumbers.Count; i++)
        //                for (int j = i + x; j < allNumbers.Count; j++)
        //                    for (int k = j + x; k < allNumbers.Count; k++)
        //                        for (int m = k + x; m < allNumbers.Count; m++)
        //                            yield return new[] { allNumbers[i], allNumbers[j], allNumbers[k], allNumbers[m] };
        //            break;
        //    }
        //}
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
                                        var pn = t1.PossibleNumbers.Union(t2.PossibleNumbers).ToList();
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
                                            var pn = t1.PossibleNumbers.Union(t2.PossibleNumbers).Union(t3.PossibleNumbers).ToList();
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
                                            var pn = t1.PossibleNumbers.Union(t2.PossibleNumbers).Union(t3.PossibleNumbers).Union(t4.PossibleNumbers).ToList();
                                            if (pn.Count == 4)
                                                nakedTuples.Add(new PossibilitiesMulti(new[] { t1.Index, t2.Index, t3.Index, t4.Index }, pn));
                                        }
                                break;
                        }
                        #region . OLD | By LINQ .
                        //var nakedTuples =
                        //    (m == 2 ? (from t1 in tuples
                        //               from t2 in tuples
                        //               let pn = t1.PossibleNumbers.Union(t2.PossibleNumbers).ToList()
                        //               where t1.Index.CompareTo(t2.Index) < 0 &&
                        //                     pn.Count == 2
                        //               select new PossibilitiesMulti
                        //               (
                        //                   new[] { t1.Index, t2.Index },
                        //                   pn
                        //               )) :
                        //    (m == 3 ? (from t1 in tuples
                        //               from t2 in tuples
                        //               from t3 in tuples
                        //               let pn = t1.PossibleNumbers.Union(t2.PossibleNumbers).Union(t3.PossibleNumbers).ToList()
                        //               where t1.Index.CompareTo(t2.Index) < 0 &&
                        //                     t2.Index.CompareTo(t3.Index) < 0 &&
                        //                     pn.Count == 3
                        //               select new PossibilitiesMulti
                        //               (
                        //                   new[] { t1.Index, t2.Index, t3.Index },
                        //                   pn
                        //               )) :
                        //    (m == 4 ? (from t1 in tuples
                        //               from t2 in tuples
                        //               from t3 in tuples
                        //               from t4 in tuples
                        //               let pn = t1.PossibleNumbers.Union(t2.PossibleNumbers).Union(t3.PossibleNumbers).Union(t4.PossibleNumbers).ToList()
                        //               where t1.Index.CompareTo(t2.Index) < 0 &&
                        //                     t2.Index.CompareTo(t3.Index) < 0 &&
                        //                     t3.Index.CompareTo(t4.Index) < 0 &&
                        //                     pn.Count == 4
                        //               select new PossibilitiesMulti
                        //               (
                        //                   new[] { t1.Index, t2.Index, t3.Index, t4.Index },
                        //                   pn
                        //               )) :
                        //    null))).ToList();
                        #endregion
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
        private static List<ChainForm> _GetSingleChain(List<Possibilities> allPossibilities, IntSudoku number, IndicesPair startIndex, List<IndicesPair> formsByNumber, bool startFlag = false, bool validationControl = false)
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
                    throw new ArgumentException(nameof(startIndex), "Cell's possible numbers were not containing the number.");
            }
            var singleChain = new List<ChainForm>();
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
            singleChain.Add(new ChainForm(startIndex, number, flag));
            foreach (var form in conjugates)
                singleChain = singleChain.Concat(_GetSingleChain(allPossibilities, number, form, _newForms, !startFlag, false)
                                                 .Where(x => !singleChain.Any(y => y.Index.Equals(x)))).OrderBy(x => x.Index).ToList();
            return singleChain.Distinct().ToList();
        }
        private static Chains GetSingleChains(List<Possibilities> allPossibilities, IntSudoku number)
        {
            var allForms = GetAllFormsForEachConjugatePairs(allPossibilities, ofASpesificNumber: number).Select(x => x.Index).ToList();
            var allSingleChains = new Chains();
            List<ChainForm> singleChain = null, reverse = null;
            foreach (var form in allForms)
            {
                singleChain = _GetSingleChain(allPossibilities, number, form, allForms);
                if (singleChain.Count <= 2)
                    continue;
                reverse = singleChain.Select(x => new ChainForm(x.Index, x.Number, (byte)(x.Flag == 0 ? 1 : 0))).ToList();
                if (!allSingleChains.Any(x => x.SequenceEqual(singleChain)) && !allSingleChains.Any(x => x.SequenceEqual(reverse)))
                    allSingleChains.Add(singleChain);
            }
            allSingleChains.TrimExcess();
            return allSingleChains;
        }
        private static Chains GetSingleChains(List<Possibilities> allPossibilities, params IntSudoku[] numbers)
        {
            if (numbers == null)
                throw new ArgumentNullException(nameof(numbers));
            var singleChains = new Chains();
            foreach (var number in numbers)
                singleChains.AddRange(GetSingleChains(allPossibilities, number));
            singleChains.TrimExcess();
            return singleChains;
        }
        private static Chains GetSingleChains(List<Possibilities> allPossibilities)
        {
            //var numbers = IntSudoku.ValidNumbers.ToList();
            //var singleChains = new Chains();
            //foreach (var number in numbers)
            //    singleChains.AddRange(GetSingleChains(allPossibilities, number));
            //singleChains.TrimExcess();
            //return singleChains;
            return GetSingleChains(allPossibilities, IntSudoku.ValidNumbers.ToArray());
        }
        private static IEnumerable<XYWing> GetXYWings(List<Possibilities> allPossibilities)
        {
            if (allPossibilities == null)
                throw new ArgumentNullException(nameof(allPossibilities));
            if (allPossibilities.Count == 0)
                throw new ArgumentException("List was empty.", nameof(allPossibilities));
            #region . Declarations .
            Possibilities pincer1 = null, pincer2 = null;
            List<Possibilities> allPairs = allPossibilities.Where(x => x.PossibleNumbers.Count == 2).ToList(), pairs = null;
            List<IntSudoku> pToP1 = null, pToP2 = null, p1ToP2 = null;
            IntSudoku sn1 = 1, sn2 = 1, sn3 = 1;
            int i = -1, j = -1;
            #endregion
            foreach (var pivot in allPairs)
            {
                pairs = Sudoku.LinkedCellsOf(pivot.Index).Select(x => allPairs.SingleOrDefault(y => y.Index.Equals(x))).Where(x => x != null).ToList();
                for (i = 0; i < pairs.Count; i++)
                    if ((pToP1 = pivot.PossibleNumbers.Intersect((pincer1 = pairs[i]).PossibleNumbers).ToList()).Count == 1)
                    {
                        sn1 = pToP1.Single();
                        for (j = i + 1; j < pairs.Count; j++)
                            if (pincer1.Index.RowIndex != (pincer2 = pairs[j]).Index.RowIndex
                             && pincer1.Index.ColumnIndex != pincer2.Index.ColumnIndex
                             && Sudoku.LinkedSquareIndexOfCell(pincer1.Index) != Sudoku.LinkedSquareIndexOfCell(pincer2.Index)
                             && (pToP2 = pivot.PossibleNumbers.Intersect(pincer2.PossibleNumbers).ToList()).Count == 1
                             && sn1 != (sn2 = pToP2.Single())
                             && (p1ToP2 = pincer1.PossibleNumbers.Intersect(pincer2.PossibleNumbers).ToList()).Count == 1
                             && sn1 != (sn3 = p1ToP2.Single())
                             && sn2 != sn3)
                                yield return new XYWing(pivot.Index, pincer1.Index, pincer2.Index, sn1, sn2, sn3);
                    }
            }
            #region . old .
            //var xyWings = new List<XYWing>();
            //List<Possibilities> r_forms = null, c_forms = null, s_forms = null, _forms = null, __forms = null;
            //List<IntSudoku> sharedNumbers1 = null, sharedNumbers2 = null, sharedNumbers3 = null;
            //XYWing yw = null;
            //int sq = -1;
            //foreach (var pivot in allPairs)
            //{
            //    r_forms = allPairs.Where(x => x.Index.RowIndex == pivot.Index.RowIndex && x.Index.ColumnIndex != pivot.Index.ColumnIndex).ToList();
            //    c_forms = allPairs.Where(x => x.Index.ColumnIndex == pivot.Index.ColumnIndex && x.Index.RowIndex != pivot.Index.RowIndex).ToList();
            //    s_forms = allPairs.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == Sudoku.LinkedSquareIndexOfCell(pivot.Index) && !x.Index.Equals(pivot.Index)).ToList();
            //    for (i = 0; i < 2; i++)
            //    {
            //        _forms = i == 0 ? r_forms : c_forms;
            //        foreach (var pincer1 in _forms)
            //        {
            //            if ((sharedNumbers1 = pivot.PossibleNumbers.Intersect(pincer1.PossibleNumbers).ToList()).Count != 1)
            //                continue;
            //            sn1 = sharedNumbers1.Single();
            //            for (j = i, sq = Sudoku.LinkedSquareIndexOfCell(pivot.Index); j < 2; j++)
            //            {
            //                if (j == 1 && sq == Sudoku.LinkedSquareIndexOfCell(pincer1.Index))
            //                    break;
            //                __forms = j == 0 ? c_forms : s_forms;
            //                foreach (var pincer2 in __forms)
            //                    if ((sharedNumbers2 = pivot.PossibleNumbers.Intersect(pincer2.PossibleNumbers).ToList()).Count == 1
            //                     && sn1 != (sn2 = sharedNumbers2.Single())
            //                     && (sharedNumbers3 = pincer1.PossibleNumbers.Intersect(pincer2.PossibleNumbers).ToList()).Count == 1
            //                     && sn1 != (sn3 = sharedNumbers3.Single()) && sn2 != sn3
            //                     && !xyWings.Contains(yw = new XYWing(pivot.Index, pincer1.Index, pincer2.Index, sn1, sn2, sn3)))
            //                        xyWings.Add(yw);
            //            }
            //        }
            //    }
            //}
            //xyWings.TrimExcess();
            //return xyWings;
            #endregion
        }
        //private static List<YWing> GetYWings(List<Possibilities> allPossibilities)
        //{
        //    if (allPossibilities == null)
        //        throw new ArgumentNullException(nameof(allPossibilities));
        //    #region . Declarations .
        //    var allForms = GetAllFormsForEachConjugatePairs(allPossibilities, onlyFromPairs: true);
        //    var yWings = new List<YWing>();
        //    YWing yw = null;
        //    List<Possibilities> r_forms = null, c_forms = null, s_forms = null, _forms = null, row = null, col = null;
        //    List<IntSudoku> sharedNumbers1 = null, sharedNumbers2 = null, sharedNumbers3 = null;
        //    int i = -1, j = -1;
        //    #endregion
        //    foreach (var pivot in allForms)
        //    {
        //        r_forms = allForms.Where(x => x.Index.RowIndex == pivot.Index.RowIndex && x.Index.ColumnIndex != pivot.Index.ColumnIndex).ToList();
        //        c_forms = allForms.Where(x => x.Index.ColumnIndex == pivot.Index.ColumnIndex && x.Index.RowIndex != pivot.Index.RowIndex).ToList();
        //        s_forms = allForms.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == Sudoku.LinkedSquareIndexOfCell(pivot.Index) && !x.Index.Equals(pivot.Index)).ToList();
        //        for (i = 0; i < 2; i++)
        //        {
        //            _forms = i == 0 ? r_forms : c_forms;
        //            foreach (var pincer1 in _forms)
        //            {
        //                sharedNumbers1 = pivot.PossibleNumbers
        //                                 .Where(x => pincer1.PossibleNumbers.Any(y => y == x)
        //                                          && allPossibilities.Count(y => (i == 0 ? (y.Index.RowIndex == pivot.Index.RowIndex) : 
        //                                                                                   (y.Index.ColumnIndex == pivot.Index.ColumnIndex))
        //                                                                      && y.PossibleNumbers.Contains(x)) == 2).ToList();
        //                //if (sharedNumbers1.Count > 1)
        //                //    continue;
        //                foreach (var sharedNumber1 in sharedNumbers1)
        //                    for (j = i; j < 2; j++)
        //                    {
        //                        if (j == 1 && Sudoku.LinkedSquareIndexOfCell(pivot.Index) == Sudoku.LinkedSquareIndexOfCell(pincer1.Index))
        //                            break;
        //                        var __forms = j == 0 ? c_forms : s_forms;
        //                        foreach (var pincer2 in __forms)
        //                        {
        //                            sharedNumbers2 = pivot.PossibleNumbers
        //                                             .Where(x => pincer2.PossibleNumbers.Any(y => y == x) &&
        //                                                         allPossibilities.Count(y => (i == 0 ? (y.Index.ColumnIndex == pivot.Index.ColumnIndex) :
        //                                                                                               (Sudoku.LinkedSquareIndexOfCell(y.Index) ==
        //                                                                                                Sudoku.LinkedSquareIndexOfCell(pivot.Index))) &&
        //                                                                                     y.PossibleNumbers.Contains(x)) == 2).ToList();
        //                            if (sharedNumbers2.Count > 1 && sharedNumbers1.Count > 2)
        //                                throw new InvalidMatrixException();
        //                            //if (sharedNumbers2.Count > 1)
        //                            //    continue;
        //                            foreach (var sharedNumber2 in sharedNumbers2)
        //                            {
        //                                sharedNumbers3 = pincer1.PossibleNumbers.Where(x => pincer2.PossibleNumbers.Any(y => y == x)).ToList();
        //                                if ((sharedNumbers1.Count > 1 || sharedNumbers2.Count > 1) && sharedNumbers3.Count > 1)
        //                                    throw new InvalidMatrixException();
        //                                //if (sharedNumbers3.Count > 1)
        //                                //    continue;
        //                                foreach (var sharedNumber3 in sharedNumbers3)
        //                                {
        //                                    if (sharedNumber1 == sharedNumber2 || sharedNumber2 == sharedNumber3 || sharedNumber1 == sharedNumber3)
        //                                        continue;
        //                                    yw = new YWing(pivot.Index, pincer1.Index, pincer2.Index, sharedNumber1, sharedNumber2, sharedNumber3);
        //                                    if (!yWings.Contains(yw))
        //                                        yWings.Add(yw);
        //                                }
        //                            }
        //                        }
        //                    }
        //            }
        //        }
        //    }
        //    yWings.TrimExcess();
        //    return yWings;
        //}
        public static IEnumerable<SwordfishForm> GetValidSwordfishForms(Sudoku matrix, List<Possibilities> allPossibilities, IntSudoku number)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            var _allPossibilities = FullCells(matrix).Concat(
                allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) :
                                                                         AllPossibilities(matrix)).OrderBy(x => x.Index).ToList();
            List<Possibilities> cells = null;
            ILookup<int, Possibilities> grouped = null;
            Possibilities t1 = null, t2 = null, t3 = null;
            int n = -1, m = -1, i = -1, j = -1, k = -1, length1 = -1, length2 = -1, a = -1, b = -1, c = -1, count = -1;
            SwordfishForm swff = null;
            for (n = 0; n < 2; n++)
            {
                length1 = n == 0 ? Sudoku.Length_Row : (n == 1 ? Sudoku.Length_Column : -1);
                length2 = n == 0 ? Sudoku.Length_Column : (n == 1 ? Sudoku.Length_Row : -1);
                grouped = (n == 0 ? (_allPossibilities.ToLookup(o => o.Index.RowIndex)) :
                          (n == 1 ? (_allPossibilities.ToLookup(o => o.Index.ColumnIndex)) :
                          null));
                for (m = 0; m < length1; m++)
                {
                    cells = grouped[m].ToList();
                    if ((count = cells.Count(x => x.PossibleNumbers.Contains(number))) == 2 || count == 3)
                        for (i = 0; i < length2; i++)
                        {
                            a = (t1 = cells[i]).PossibleNumbers.Count == 1 || !t1.PossibleNumbers.Contains(number) ? 1 : 0;
                            for (j = i + 1; j < length2; j++)
                                if ((b = a + ((t2 = cells[j]).PossibleNumbers.Count == 1 || !t2.PossibleNumbers.Contains(number) ? 1 : 0)) <= 1)
                                    for (k = j + 1; k < length2; k++)
                                        if ((c = b + ((t3 = cells[k]).PossibleNumbers.Count == 1 || !t3.PossibleNumbers.Contains(number) ? 1 : 0)) <= 1
                                         && c + count == 3
                                         && SwordfishForm.TryCreateInstance(out swff, t1, t2, t3))
                                            yield return swff;
                        }
                }
            }
        }
        public static List<Swordfish> GetSwordfishes(Sudoku matrix, List<Possibilities> allPossibilities, IntSudoku number)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            var _allPossibilities = FullCells(matrix).Concat(
                allPossibilities != null && allPossibilities.Count > 0 ? allPossibilities :
                                                                         AllPossibilities(matrix)).OrderBy(x => x.Index).ToList();
            List<Possibilities> cells = null;
            ILookup<int, Possibilities> grouped = null;
            Possibilities t1 = null, t2 = null, t3 = null;
            int n = -1, m = -1, i = -1, j = -1, k = -1, length1 = -1, length2 = -1, a = -1, b = -1, c = -1, count = -1;
            Swordfish swf = null;
            List<Swordfish> list = new List<Swordfish>();
            SwordfishForm swff = null;
            List<SwordfishForm> triples = null;
            for (n = 0; n < 2; n++)
            {
                triples = new List<SwordfishForm>();
                grouped = (n == 0 ? (_allPossibilities.ToLookup(o => o.Index.RowIndex)) :
                          (n == 1 ? (_allPossibilities.ToLookup(o => o.Index.ColumnIndex)) :
                          null));
                for (length1 = n == 0 ? Sudoku.Length_Row : (n == 1 ? Sudoku.Length_Column : -1), 
                     length2 = n == 0 ? Sudoku.Length_Column : (n == 1 ? Sudoku.Length_Row : -1),
                     m = 0; m < length1; m++)
                {
                    #region . old .
                    //cells = matrix[unitType, m].Select((x, l) =>
                    //    new Possibilities
                    //    (
                    //        n == 0 ? new IndicesPair(m, l) : (n == 1 ? new IndicesPair(l, m) : new IndicesPair()),
                    //        x.HasValue ? new List<IntSudoku>() { x.Value } :
                    //                     new List<IntSudoku>(_allPossibilities.Single(y => y.Index.Equals((n == 0 ? new IndicesPair(m, l) :
                    //                                                                                     (n == 1 ? new IndicesPair(l, m) :
                    //                                                                                     new IndicesPair())))).PossibleNumbers)
                    //    )).ToList();
                    #endregion
                    cells = grouped[m].ToList();
                    if ((count = cells.Count(x => x.PossibleNumbers.Contains(number))) == 2 || count == 3)
                        for (i = 0; i < length2; i++)
                        {
                            a = (t1 = cells[i]).PossibleNumbers.Count == 1 || !t1.PossibleNumbers.Contains(number) ? 1 : 0;
                            for (j = i + 1; j < length2; j++)
                                if ((b = a + ((t2 = cells[j]).PossibleNumbers.Count == 1 || !t2.PossibleNumbers.Contains(number) ? 1 : 0)) <= 1)
                                    for (k = j + 1; k < length2; k++)
                                        if ((c = b + ((t3 = cells[k]).PossibleNumbers.Count == 1 || !t3.PossibleNumbers.Contains(number) ? 1 : 0)) <= 1
                                         && c + count == 3
                                         && SwordfishForm.TryCreateInstance(out swff, t1, t2, t3))
                                            triples.Add(swff);
                        }
                }
                switch (n)
                {
                    case 0:
                        for (i = 0; i < triples.Count; i++)
                            for (j = i + 1; j < triples.Count; j++)
                                if (triples[i][0].Index.RowIndex != triples[j][0].Index.RowIndex)
                                    for (k = j + 1; k < triples.Count; k++)
                                        if (Swordfish.TryCreateAValidInstance(out swf, number, _allPossibilities, triples[i], triples[j], triples[k]))
                                            list.Add(swf);
                        break;
                    case 1:
                        for (i = 0; i < triples.Count; i++)
                            for (j = i + 1; j < triples.Count; j++)
                                if (triples[i][0].Index.ColumnIndex != triples[j][0].Index.ColumnIndex)
                                    for (k = j + 1; k < triples.Count; k++)
                                        if (Swordfish.TryCreateAValidInstance(out swf, number, _allPossibilities, triples[i], triples[j], triples[k]))
                                            list.Add(swf);
                        break;
                }
            }
            list.TrimExcess();
            return list;
            #region . OLD | IEnuarable<Swordfish> .
            //if (matrix == null)
            //    throw new ArgumentNullException(nameof(matrix));
            //var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) :
            //                                                                                 AllPossibilities(matrix).ToList();
            //Swordfish swf = null;
            //var triples = GetValidSwordfishForms(matrix, _allPossibilities, number).ToArray();
            //if (triples.Length > 0)
            //    for (int x = 0, i = -1, j = -1, k = -1; x < 2; x++)
            //        switch (x)
            //        {
            //            case 0:
            //                for (i = 0; i < triples.Length; i++)
            //                    for (j = i + 1; j < triples.Length; j++)
            //                        if (triples[i][0].Index.RowIndex != triples[j][0].Index.RowIndex)
            //                            for (k = j + 1; k < triples.Length; k++)
            //                                if (Swordfish.TryCreateAValidInstance(out swf, number, _allPossibilities, triples[i], triples[j], triples[k]))
            //                                    yield return swf;
            //                break;
            //            case 1:
            //                for (i = 0; i < triples.Length; i++)
            //                    for (j = i + 1; j < triples.Length; j++)
            //                        if (triples[i][0].Index.ColumnIndex != triples[j][0].Index.ColumnIndex)
            //                            for (k = j + 1; k < triples.Length; k++)
            //                                if (Swordfish.TryCreateAValidInstance(out swf, number, _allPossibilities, triples[i], triples[j], triples[k]))
            //                                    yield return swf;
            //                break;
            //        }
            #endregion
        }
        public static List<Swordfish> GetAllSwordfishes(Sudoku matrix, List<Possibilities> allPossibilities, List<IntSudoku> promisingNumbers = null)
        {
            var allNumbers = promisingNumbers == null || promisingNumbers.Count == 0 ? IntSudoku.ValidNumbers.ToList() : promisingNumbers;
            var allSwordfishes = new List<Swordfish>();
            foreach (var number in allNumbers)
                allSwordfishes.AddRange(GetSwordfishes(matrix, allPossibilities, number));
            allSwordfishes.TrimExcess();
            return allSwordfishes;
        }
        public static IEnumerable<XYZWing> GetXYZWings(List<Possibilities> allPossibilities)
        {
            if (allPossibilities == null)
                throw new ArgumentNullException(nameof(allPossibilities));
            if (allPossibilities.Count == 0)
                throw new ArgumentException("List was empty.", nameof(allPossibilities));
            #region . Declarations .
            var allPairs = allPossibilities.Where(x => x.PossibleNumbers.Count == 2).ToList();
            var allTriples = allPossibilities.Where(x => x.PossibleNumbers.Count == 3).ToList();
            Possibilities pincer1 = null, pincer2 = null;
            List<Possibilities> pairs = null;
            List<IntSudoku> pToP1 = null, pToP2 = null, p1ToP2 = null;
            IntSudoku sn1 = 1, sn2 = 1, cn = 1;
            int i = -1, j = -1;
            #endregion
            foreach (var pivot in allTriples)
            {
                pairs = Sudoku.LinkedCellsOf(pivot.Index).Select(x => allPairs.SingleOrDefault(y => y.Index.Equals(x))).Where(x => x != null).ToList();
                for (i = 0; i < pairs.Count; i++)
                    if ((pToP1 = pivot.PossibleNumbers.Intersect((pincer1 = pairs[i]).PossibleNumbers).ToList()).Count == 2)
                        for (j = i + 1; j < pairs.Count; j++)
                            if (pincer1.Index.RowIndex != (pincer2 = pairs[j]).Index.RowIndex
                             && pincer1.Index.ColumnIndex != pincer2.Index.ColumnIndex
                             && Sudoku.LinkedSquareIndexOfCell(pincer1.Index) != Sudoku.LinkedSquareIndexOfCell(pincer2.Index)
                             && (pToP2 = pivot.PossibleNumbers.Intersect(pincer2.PossibleNumbers).ToList()).Count == 2
                             && (p1ToP2 = pincer1.PossibleNumbers.Intersect(pincer2.PossibleNumbers).ToList()).Count == 1
                             && pToP1.Intersect(pToP2).SequenceEqual(p1ToP2))
                            {
                                cn = p1ToP2.Single();
                                yield return new XYZWing(pivot.Index, pincer1.Index, pincer2.Index, pToP1.Single(x => x != cn), pToP2.Single(x => x != cn), cn);
                            }
            }
            #region . old .
            //var xyzWings = new List<XYZWing>();
            //XYZWing zw = null;
            //List<Possibilities> r_pairs = null, c_pairs = null, s_pairs = null, _pairs = null, __pairs = null;
            //int sq = -1, a = -1, b = -1;
            //foreach (var pivot in allTriples)
            //{
            //    r_pairs = allPairs.Where(x => x.Index.RowIndex == pivot.Index.RowIndex && x.Index.ColumnIndex != pivot.Index.ColumnIndex).ToList();
            //    c_pairs = allPairs.Where(x => x.Index.ColumnIndex == pivot.Index.ColumnIndex && x.Index.RowIndex != pivot.Index.RowIndex).ToList();
            //    s_pairs = allPairs.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == Sudoku.LinkedSquareIndexOfCell(pivot.Index) && !x.Index.Equals(pivot.Index)).ToList();
            //    for (i = 0; i < 2; i++)
            //    {
            //        _pairs = i == 0 ? r_pairs : c_pairs;
            //        foreach (var pincer1 in _pairs)
            //        {
            //            if ((pToP1 = pincer1.PossibleNumbers.Intersect(pivot.PossibleNumbers).ToList()).Count != 2)
            //                continue;
            //            for (j = i, sq = Sudoku.LinkedSquareIndexOfCell(pivot.Index); j < 2; j++)
            //            {
            //                if (j == 1 && sq == Sudoku.LinkedSquareIndexOfCell(pincer1.Index))
            //                    continue;
            //                __pairs = j == 0 ? c_pairs : s_pairs;
            //                foreach (var pincer2 in __pairs)
            //                {
            //                    if ((pToP2 = pincer2.PossibleNumbers.Intersect(pivot.PossibleNumbers).ToList()).Count != 2
            //                     || (p1ToP2 = pincer1.PossibleNumbers.Intersect(pincer2.PossibleNumbers).ToList()).Count != 1)
            //                        continue;
            //                    cn = p1ToP2.Single();
            //                    for (a = 0; a < 2; a++)
            //                    {
            //                        if (cn != pToP1[2 - a - 1])
            //                            continue;
            //                        sn1 = pToP1[a];
            //                        for (b = 0; b < 2; b++)
            //                            if (cn == pToP2[2 - b - 1]
            //                             && sn1 != (sn2 = pToP2[b]) && sn1 != cn && sn2 != cn
            //                             && !xyzWings.Contains(zw = new XYZWing(pivot.Index, pincer1.Index, pincer2.Index, sn1, sn2, cn)))
            //                                xyzWings.Add(zw);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //xyzWings.TrimExcess();
            //return xyzWings;
            #endregion
        }

        #region . OLD .
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
                            //if (cell.PossibleNumbers.Count != 1)
                            //    throw new InvalidMatrixException(nameof(matrix));
                            setValue(cell.Index, cell.PossibleNumbers.Single(), false);
                            break;
                        }
                        var promisingNumbers = unit.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                        foreach (var number in promisingNumbers)
                        {
                            var containers = unit.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                            //if (containers.Count == 0)
                            //    throw new InvalidMatrixException(nameof(matrix));
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
            var allNumbers = IntSudoku.ValidNumbers.ToList();
            var _allPossibilities = allPossibilities != null && allPossibilities.Count > 0 ? new List<Possibilities>(allPossibilities) : AllPossibilities(curr).ToList();
            #endregion
            #region . General Loop .
            do
            {
                possChanged = false;
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
                    foreach (var number in allNumbers)
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
                            break;
                        }
                    }
                    if (possChanged)
                        break;
                    #endregion
                }
                if (possChanged)
                    continue;
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
                    foreach (var number in allNumbers)
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
                                    break;
                                }
                            }
                        }
                        if (possChanged)
                            break;
                    }
                    if (possChanged)
                        break;
                    #endregion
                }
                if (possChanged)
                    continue;
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
                    foreach (var number in allNumbers)
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
                                    break;
                                }
                            }
                        }
                        if (possChanged)
                            break;
                    }
                    if (possChanged)
                        break;
                    #endregion
                }
                #endregion
                #endregion
            } while (possChanged);
            #endregion
            #region . Return Value .
            lastCertain = curr.Clone() as Sudoku;
            newPossibilities = possChanged ? _allPossibilities : null;
            return false;
            //lastCertain = curr.Clone() as Sudoku;
            //if (!possChanged)
            //{
            //    newPossibilities = null;
            //    return false;
            //}
            //_allPossibilities.TrimExcess();
            //List<Possibilities> np = null;
            //if (TrySetAnyValues_1_IntersectionRemoval(curr, out curr, _allPossibilities, out np, true))
            //{
            //    newPossibilities = null;
            //    return true;
            //}
            //np?.TrimExcess();
            //newPossibilities = np != null && np.Count > 0 ? np : _allPossibilities;
            //return false;
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
                    //var allTuples = _allPossibilities.Where(x => x.PossibleNumbers.Count <= 4).ToList();
                    #endregion
                    #region . Deciding for i-th of k-th SubArray .
                    for (int i = 0; i < length; i++)
                    {
                        var allEmpties =
                            (k == 0 ? _allPossibilities.Where(x => x.Index.RowIndex == i) :
                            (k == 1 ? _allPossibilities.Where(x => x.Index.ColumnIndex == i) :
                            (k == 2 ? _allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                            null))).ToList();
                        #region . old .
                        //var tuples =
                        //    (k == 0 ? allTuples.Where(x => x.Index.RowIndex == i) :
                        //    (k == 1 ? allTuples.Where(x => x.Index.ColumnIndex == i) :
                        //    (k == 2 ? allTuples.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                        //    null))).ToList();
                        #endregion
                        #region . Tuples' Loop .
                        for (int m = 4; m >= 2; m--)
                        {
                            #region . old .
                            //if (tuples.Count <= m)
                            //    continue;
                            //#region . Getting Naked Tuples .
                            //var nakedTuples =
                            //    (m == 2 ? (from t1 in tuples
                            //               from t2 in tuples
                            //               let pn = t1.PossibleNumbers.Union(t2.PossibleNumbers).ToList()
                            //               where t1.Index.CompareTo(t2.Index) < 0 &&
                            //                     pn.Count == 2
                            //               select new PossibilitiesMulti
                            //               (
                            //                   new[] { t1.Index, t2.Index },
                            //                   pn
                            //               )) :
                            //    (m == 3 ? (from t1 in tuples
                            //               from t2 in tuples
                            //               from t3 in tuples
                            //               let pn = t1.PossibleNumbers.Union(t2.PossibleNumbers).Union(t3.PossibleNumbers).ToList()
                            //               where t1.Index.CompareTo(t2.Index) < 0 &&
                            //                     t2.Index.CompareTo(t3.Index) < 0 &&
                            //                     pn.Count == 3
                            //               select new PossibilitiesMulti
                            //               (
                            //                   new[] { t1.Index, t2.Index, t3.Index },
                            //                   pn
                            //               )) :
                            //    (m == 4 ? (from t1 in tuples
                            //               from t2 in tuples
                            //               from t3 in tuples
                            //               from t4 in tuples
                            //               let pn = t1.PossibleNumbers.Union(t2.PossibleNumbers).Union(t3.PossibleNumbers).Union(t4.PossibleNumbers).ToList()
                            //               where t1.Index.CompareTo(t2.Index) < 0 &&
                            //                     t2.Index.CompareTo(t3.Index) < 0 &&
                            //                     t3.Index.CompareTo(t4.Index) < 0 &&
                            //                     pn.Count == 4
                            //               select new PossibilitiesMulti
                            //               (
                            //                   new[] { t1.Index, t2.Index, t3.Index, t4.Index },
                            //                   pn
                            //               )) :
                            //    null))).ToList();
                            //#endregion
                            #endregion
                            var nakedTuples = GetNakedTuples(allForms, k == 0 ? SubArrayTypes.Row : (k == 1 ? SubArrayTypes.Column : SubArrayTypes.Square), i, m);
                            if (nakedTuples.Count == 0)
                                continue;
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
                            var discoveredTuples = others.Where(x => x.IsJustDiscovered)// && x.PossibleNumbers.Count >= 2)
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
            #region . old .
            //lastCertain = curr.Clone() as Sudoku;
            //if (!possChanged)
            //{
            //    newPossibilities = null;
            //    return false;
            //}
            //_allPossibilities.TrimExcess();
            //List<Possibilities> np = null;
            //if (TrySetAnyValues_2_NakedTuples(curr, out curr, _allPossibilities, out np, true))
            //{
            //    newPossibilities = null;
            //    return true;
            //}
            //np?.TrimExcess();
            //newPossibilities = np != null && np.Count > 0 ? np : _allPossibilities;
            //return false;
            #endregion
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
            //var comb2 = GetCombination(2).ToList();
            //var comb3 = GetCombination(3).ToList();
            //var comb4 = GetCombination(4).ToList();
            List<Possibilities> __newPossibilities = null;
            int k = -1, i = -1, j = -1, a = -1, b = -1, c = -1, d = -1, length = -1;
            List<IntSudoku> promisingNumbers = null, nCont1 = null, nCont2 = null, nCont3 = null, nCont4 = null, nConts = null;
            IntSudoku n1 = 1, n2 = 1, n3 = 1, n4 = 1;
            IntSudoku[] numbers = null;
            List<Possibilities> cells = null, cont1 = null, cont2 = null, cont3 = null, cont4 = null;
            List<IndicesPair> iCont1 = null, iCont2 = null, iCont3 = null, iCont4 = null, iConts = null;
            Action concatPossibilities = () =>
            {
                if (possChanged = __newPossibilities != null && __newPossibilities.Count > 0)
                    _allPossibilities = __newPossibilities.Concat(_allPossibilities.Where(x => !__newPossibilities.Any(y => y.Index.Equals(x.Index))).ToList()).OrderBy(x => x.Index).ToList();
                __newPossibilities = null;
            };
            #endregion
            #region . General Loop .
            do
            {
                possChanged = false;
                for (k = 0; k < 3; k++)
                    for (i = 0, length = k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : Sudoku.CountOfSquares); i < length; i++)
                    {
                        cells = (k == 0 ? _allPossibilities.Where(x => x.Index.RowIndex == i) :
                                (k == 1 ? _allPossibilities.Where(x => x.Index.ColumnIndex == i) :
                                (k == 2 ? _allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                                new List<Possibilities>()))).ToList();
                        if (cells.Count == 0)
                            continue;
                        promisingNumbers = cells.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                        __newPossibilities = new List<Possibilities>();
                        for (j = 2; j <= 4; j++)
                        {
                            if (cells.Count <= j)
                                continue;
                            #region . old .
                            //var bannedNumbers = new List<IntSudoku>();
                            //var tuples = j == 2 ? comb2 : (j == 3 ? comb3 : comb4);
                            //foreach (var tuple in tuples)
                            //{
                            //    if (tuple.Any(x => bannedNumbers.Contains(x)))
                            //        continue;
                            //    var _containers = new List<IndicesPair>();
                            //    bool ok = true;
                            //    foreach (var t in tuple)
                            //    {
                            //        var cont = cells.Where(x => x.PossibleNumbers.Contains(t)).ToList();
                            //        if (cont.Count == 1)
                            //        {
                            //            curr[cont.Single().Index] = t;
                            //            lastCertain = curr as Sudoku;
                            //            newPossibilities = null;
                            //            return true;
                            //        }
                            //        if (cont.Count < 2 || cont.Count > j)
                            //        {
                            //            bannedNumbers.Add(t);
                            //            ok = false;
                            //            break;
                            //        }
                            //        _containers.AddRange(cont.Select(x => x.Index).ToList());
                            //    }
                            //    if (!ok)
                            //        continue;
                            //    _containers = _containers.Distinct().ToList();
                            //    if (_containers.Count == j)
                            //    {
                            //        var containers = _allPossibilities
                            //                         .Where(x => _containers.Contains(x.Index))
                            //                         .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Where(y => tuple.Any(z => z == y)).ToList())).ToList();
                            //        _allPossibilities = containers.Concat(_allPossibilities.Where(x => !containers.Any(y => y.Index.Equals(x.Index))).ToList()).ToList();
                            //        possChanged = true;
                            //    }
                            //}
                            #endregion
                            #region . Getting Hidden Naked Tuples .
                            switch (j)
                            {
                                case 2:
                                    for (a = 0; a < promisingNumbers.Count; a++)
                                    {
                                        n1 = promisingNumbers[a];
                                        cont1 = cells.Where(x => x.PossibleNumbers.Contains(n1)).ToList();
                                        if (cont1.Count > j)
                                            continue;
                                        iCont1 = cont1.Select(x => x.Index).ToList();
                                        nCont1 = cont1.SelectMany(x => x.PossibleNumbers).ToList();
                                        for (b = a + 1; b < promisingNumbers.Count; b++)
                                        {
                                            n2 = promisingNumbers[b];
                                            cont2 = cells.Where(x => x.PossibleNumbers.Contains(n2)).ToList();
                                            if (cont2.Count > j)
                                                continue;
                                            iCont2 = cont2.Select(x => x.Index).ToList();
                                            nCont2 = cont2.SelectMany(x => x.PossibleNumbers).ToList();
                                            if ((iConts = iCont1.Concat(iCont2).Distinct().ToList()).Count == j && 
                                                (nConts = nCont1.Concat(nCont2).Distinct().ToList()).Count > j)
                                            {
                                                numbers = new[] { n1, n2 };
                                                __newPossibilities.AddRange(iConts.Select(x => new Possibilities(x, cells.Single(y => y.Index.Equals(x)).PossibleNumbers
                                                                                                                    .Where(y => numbers.Contains(y)).ToList())));
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    for (a = 0; a < promisingNumbers.Count; a++)
                                    {
                                        n1 = promisingNumbers[a];
                                        cont1 = cells.Where(x => x.PossibleNumbers.Contains(n1)).ToList();
                                        if (cont1.Count > j)
                                            continue;
                                        iCont1 = cont1.Select(x => x.Index).ToList();
                                        nCont1 = cont1.SelectMany(x => x.PossibleNumbers).ToList();
                                        for (b = a + 1; b < promisingNumbers.Count; b++)
                                        {
                                            n2 = promisingNumbers[b];
                                            cont2 = cells.Where(x => x.PossibleNumbers.Contains(n2)).ToList();
                                            if (cont2.Count > j)
                                                continue;
                                            iCont2 = cont2.Select(x => x.Index).ToList();
                                            nCont2 = cont2.SelectMany(x => x.PossibleNumbers).ToList();
                                            for (c = b + 1; c < promisingNumbers.Count; c++)
                                            {
                                                n3 = promisingNumbers[c];
                                                cont3 = cells.Where(x => x.PossibleNumbers.Contains(n3)).ToList();
                                                if (cont3.Count > j)
                                                    continue;
                                                iCont3 = cont3.Select(x => x.Index).ToList();
                                                nCont3 = cont3.SelectMany(x => x.PossibleNumbers).ToList();
                                                if ((iConts = iCont1.Concat(iCont2).Concat(iCont3).Distinct().ToList()).Count == j &&
                                                    (nConts = nCont1.Concat(nCont2).Concat(nCont3).Distinct().ToList()).Count > j)
                                                {
                                                    numbers = new[] { n1, n2, n3 };
                                                    __newPossibilities.AddRange(iConts.Select(x => new Possibilities(x, cells.Single(y => y.Index.Equals(x)).PossibleNumbers
                                                                                                                        .Where(y => numbers.Contains(y)).ToList())));
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 4:
                                    for (a = 0; a < promisingNumbers.Count; a++)
                                    {
                                        n1 = promisingNumbers[a];
                                        cont1 = cells.Where(x => x.PossibleNumbers.Contains(n1)).ToList();
                                        if (cont1.Count > j)
                                            continue;
                                        iCont1 = cont1.Select(x => x.Index).ToList();
                                        nCont1 = cont1.SelectMany(x => x.PossibleNumbers).ToList();
                                        for (b = a + 1; b < promisingNumbers.Count; b++)
                                        {
                                            n2 = promisingNumbers[b];
                                            cont2 = cells.Where(x => x.PossibleNumbers.Contains(n2)).ToList();
                                            iCont2 = cont2.Select(x => x.Index).ToList();
                                            if (cont2.Count > j)
                                                continue;
                                            nCont2 = cont2.SelectMany(x => x.PossibleNumbers).ToList();
                                            for (c = b + 1; c < promisingNumbers.Count; c++)
                                            {
                                                n3 = promisingNumbers[c];
                                                cont3 = cells.Where(x => x.PossibleNumbers.Contains(n3)).ToList();
                                                if (cont3.Count > j)
                                                    continue;
                                                iCont3 = cont3.Select(x => x.Index).ToList();
                                                nCont3 = cont3.SelectMany(x => x.PossibleNumbers).ToList();
                                                for (d = c + 1; d < promisingNumbers.Count; d++)
                                                {
                                                    n4 = promisingNumbers[d];
                                                    cont4 = cells.Where(x => x.PossibleNumbers.Contains(n4)).ToList();
                                                    if (cont4.Count > j)
                                                        continue;
                                                    iCont4 = cont4.Select(x => x.Index).ToList();
                                                    nCont4 = cont4.SelectMany(x => x.PossibleNumbers).ToList();
                                                    if ((iConts = iCont1.Concat(iCont2).Concat(iCont3).Concat(iCont4).Distinct().ToList()).Count == j &&
                                                        (nConts = nCont1.Concat(nCont2).Concat(nCont3).Concat(nCont4).Distinct().ToList()).Count > j)
                                                    {
                                                        numbers = new[] { n1, n2, n3, n4 };
                                                        __newPossibilities.AddRange(iConts.Select(x => new Possibilities(x, cells.Single(y => y.Index.Equals(x)).PossibleNumbers
                                                                                                                            .Where(y => numbers.Contains(y)).ToList())));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                            #endregion
                            if (__newPossibilities.Count > 0)
                                break;
                        }
                        concatPossibilities();
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
            //Action<SubArrayType, int[], int[], IntSudoku> clearNumbers = (type, indicesOfSubArrays, exceptSubindices, number) =>
            //{
            //    foreach (var index in indicesOfSubArrays)
            //    {
            //        var _changings = (from x in _allPossibilities
            //                          where (type == SubArrayType.Row ? (x.Index.RowIndex == index && !exceptSubindices.Contains(x.Index.ColumnIndex)) :
            //                                (type == SubArrayType.Column ? (x.Index.ColumnIndex == index && !exceptSubindices.Contains(x.Index.RowIndex)) :
            //                                (false)))
            //                          select new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList())).ToList();
            //        if (_changings.Count == 0)
            //            continue;
            //        possChanged = true;
            //        _allPossibilities = _changings.Concat(_allPossibilities.Where(x => !_changings.Any(y => y.Index.Equals(x.Index)))).OrderBy(x => x.Index).ToList();
            //    }
            //};
            int k = -1, i = -1, length = -1, j = -1;
            List<IntSudoku> promisingNumbers = null;
            IntSudoku[] numbers;
            List<Possibilities> __newPossibilities = null, cells = null, containers = null, containers2 = null;
            Action concatPossibilities = () =>
            {
                if (possChanged = __newPossibilities != null && __newPossibilities.Count > 0)
                    _allPossibilities = __newPossibilities.Concat(_allPossibilities.Where(x => !__newPossibilities.Any(y => y.Index.Equals(x.Index))).ToList()).OrderBy(x => x.Index).ToList();
                __newPossibilities = null;
            };
            #endregion
            #region . General Loop .
            do
            {
                possChanged = false;
                for (k = 0; k < 2; k++)
                {
                    for (i = 0, length = k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : -1); i < length; i++)
                    {
                        #region . Preparing i-th of k-th Unit's Cells .
                        cells = (k == 0 ? _allPossibilities.Where(x => x.Index.RowIndex == i) :
                                (k == 1 ? _allPossibilities.Where(x => x.Index.ColumnIndex == i) :
                                new List<Possibilities>())).ToList();
                        if (cells.Count == 0)
                            continue;
                        #endregion
                        promisingNumbers = cells.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                        if (cells.Count != promisingNumbers.Count)
                            throw new InvalidMatrixException();
                        foreach (var number in promisingNumbers)
                        {
                            containers = cells.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                            if (containers.Count != 2)
                                continue;
                            for (j = i + 1; j < length; j++)
                            {
                                containers2 = (k == 0 ? (_allPossibilities.Where(x => x.Index.RowIndex == j && x.PossibleNumbers.Contains(number))) :
                                              (k == 1 ? (_allPossibilities.Where(x => x.Index.ColumnIndex == j && x.PossibleNumbers.Contains(number))) :
                                              new List<Possibilities>())).OrderBy(x => x.Index).ToList();
                                if (containers2.Count != 2)
                                    continue;
                                numbers = new[] { number };
                                if (k == 0 && (containers2[0].Index.ColumnIndex == containers[0].Index.ColumnIndex &&
                                               containers2[1].Index.ColumnIndex == containers[1].Index.ColumnIndex))
                                    __newPossibilities = _allPossibilities.Where(x => x.PossibleNumbers.Contains(number) &&
                                                                                     x.Index.RowIndex != containers[0].Index.RowIndex &&
                                                                                     x.Index.RowIndex != containers2[0].Index.RowIndex &&
                                                                                     (x.Index.ColumnIndex == containers[0].Index.ColumnIndex || x.Index.ColumnIndex == containers[1].Index.ColumnIndex))
                                                                         .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(numbers)))
                                                                         .ToList();
                                else if (k == 1 && (containers2[0].Index.RowIndex == containers[0].Index.RowIndex &&
                                                    containers2[1].Index.RowIndex == containers[1].Index.RowIndex))
                                    __newPossibilities = _allPossibilities.Where(x => x.PossibleNumbers.Contains(number) &&
                                                                                     x.Index.ColumnIndex != containers[0].Index.ColumnIndex &&
                                                                                     x.Index.ColumnIndex != containers2[0].Index.ColumnIndex &&
                                                                                     (x.Index.RowIndex == containers[0].Index.RowIndex || x.Index.RowIndex == containers[1].Index.RowIndex))
                                                                         .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(numbers)))
                                                                         .ToList();
                                else
                                    __newPossibilities = null;
                                concatPossibilities();
                                if (possChanged)
                                    break;
                            }
                            if (possChanged)
                                break;
                        }
                        if (possChanged)
                            break;
                    }
                    if (possChanged)
                        break;
                }
                #region . old .
                //#region . All Valid Numbers' Loop .
                //foreach (var number in allNumbers)
                //{
                //    #region . Deciding for i-th Row .
                //    for (int i = 0; i < Sudoku.Length_Row; i++)
                //    {
                //        var rowPos = _allPossibilities.Where(x => x.Index.RowIndex == i).ToList();
                //        var containingCells = rowPos.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                //        if (containingCells.Count != 2)
                //            continue;
                //        for (int j = i + 1; j < Sudoku.Length_Row; j++)
                //        {
                //            var _rowPos = _allPossibilities.Where(x => x.Index.RowIndex == j).ToList();
                //            var _containingCells = _rowPos.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                //            if (_containingCells.Count != 2)
                //                continue;
                //            if (_containingCells[0].Index.ColumnIndex == containingCells[0].Index.ColumnIndex &&
                //                _containingCells[1].Index.ColumnIndex == containingCells[1].Index.ColumnIndex)
                //            {
                //                clearNumbers(SubArrayType.Column,
                //                             new[] { containingCells[0].Index.ColumnIndex, containingCells[1].Index.ColumnIndex },
                //                             new[] { i, j },
                //                             number);
                //                break; // If there are any other ones, the sudoku is invalid.
                //            }
                //        }
                //        if (possChanged)
                //            break;
                //    }
                //    #endregion
                //    if (possChanged)
                //        break;
                //    #region . Deciding for i-th Column .
                //    for (int i = 0; i < Sudoku.Length_Column; i++)
                //    {
                //        var columnPos = _allPossibilities.Where(x => x.Index.ColumnIndex == i).ToList();
                //        var containingCells = columnPos.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                //        if (containingCells.Count != 2)
                //            continue;
                //        for (int j = i + 1; j < Sudoku.Length_Column; j++)
                //        {
                //            var _columnPos = _allPossibilities.Where(x => x.Index.ColumnIndex == j).ToList();
                //            var _containingCells = _columnPos.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                //            if (_containingCells.Count != 2)
                //                continue;
                //            if (_containingCells[0].Index.RowIndex == containingCells[0].Index.RowIndex &&
                //                _containingCells[1].Index.RowIndex == containingCells[1].Index.RowIndex)
                //            {
                //                clearNumbers(SubArrayType.Row,
                //                             new[] { containingCells[0].Index.RowIndex, containingCells[1].Index.RowIndex },
                //                             new[] { i, j },
                //                             number);
                //                break; // If there are any other ones, the sudoku is invalid.
                //            }
                //        }
                //        if (possChanged)
                //            break;
                //    }
                //    #endregion
                //    if (possChanged)
                //        break;
                //}
                //#endregion
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
                    var allSingleChains = new Chains();
                    foreach (var form in allForms)
                    {
                        var singleChain = _GetSingleChain(_allPossibilities, number, form, allForms);
                        if (singleChain.Count <= 2)
                            continue;
                        var _reverse = singleChain.Select(x => new ChainForm(x.Index, x.Number, (byte)(x.Flag == 0 ? 1 : 0))).ToList();
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
                var yWings = GetXYWings(_allPossibilities);
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
            var comb = Enumerable.Range(0, Sudoku.GeneralFixedLength).Combinations(3).Select(x => x.ToList()).ToList();
            Func<Possibilities, Possibilities, Possibilities, int, bool> isRowOK = (p1, p2, p3, i) =>
            {
                return ((p1.PossibleNumbers.Count == 1 || !p1.PossibleNumbers.Contains(i) ? 1 : 0) + 
                        (p2.PossibleNumbers.Count == 1 || !p2.PossibleNumbers.Contains(i) ? 1 : 0) + 
                        (p3.PossibleNumbers.Count == 1 || !p3.PossibleNumbers.Contains(i) ? 1 : 0)) <= 1;
            };
            Func<int, int, int, int, int, int, IntSudoku, bool> areColumnsOK = (row1, row2, row3, column1, column2, column3, number) =>
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
                    #region . old .
                    //var allTriples = new Dictionary<IntSudoku, Dictionary<int, List<Tuple<Possibilities, Possibilities, Possibilities>>>>();
                    //foreach (var number in allNumbers)
                    //{
                    //    var empties = _allPossibilities.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                    //    allTriples.Add(number, new Dictionary<int, List<Tuple<Possibilities, Possibilities, Possibilities>>>());
                    //    for (int r = 0; r < Sudoku.GeneralFixedLength; r++)
                    //    {
                    //        allTriples[number].Add(r, new List<Tuple<Possibilities, Possibilities, Possibilities>>());
                    //        var cells = empties.Where(x => x.Index.RowIndex == r).ToList().Concat(matrix[SubArrayType.Row, r]
                    //                                                                              .OrderBy(x => x)
                    //                                                                              .Where(x => x.HasValue)
                    //                                                                              .Select((x, c) => new Possibilities(new IndicesPair(r, c), new[] { x.Value }))
                    //                                                                              .ToList()).ToList();
                    //        var combinations = GetCombination(3).Where(x => !x.Any(y => y > cells.Count - 1)).Select(x => x.Cast<int>().ToArray()).ToList();
                    //        foreach (var columns in combinations)
                    //        {
                    //            var t1 = cells[columns[0]];
                    //            var t2 = cells[columns[1]];
                    //            var t3 = cells[columns[2]];
                    //            int hasValue = 0;
                    //            if (t1.PossibleNumbers.Count == 1)
                    //                hasValue++;
                    //            if (t2.PossibleNumbers.Count == 1)
                    //                hasValue++;
                    //            if (t2.PossibleNumbers.Count == 1)
                    //                hasValue++;
                    //            if (hasValue > 1)
                    //                continue;
                    //            allTriples[number][r].Add(new Tuple<Possibilities, Possibilities, Possibilities>(t1, t2, t3));
                    //        }
                    //        if (possChanged)
                    //            break;
                    //    }
                    //    if (possChanged)
                    //        break;
                    //}
                    //
                    // //OTHER
                    //
                    //var allTriples = (from number in allNumbers
                    //                  let empties = _allPossibilities.Where(x => x.PossibleNumbers.Contains(number)).ToList()
                    //                  let range = Enumerable.Range(0, Sudoku.GeneralFixedLength).ToList()
                    //                  from r in range
                    //                  let cells = empties.Where(x => x.Index.RowIndex == r).ToList()
                    //                                                                       .Concat(curr[SubArrayType.Row, r]
                    //                                                                               .OrderBy(x => x)
                    //                                                                               .Where(x => x.HasValue)
                    //                                                                               .Select((x, c) => new Possibilities(new IndicesPair(r, c), new[] { x.Value }))
                    //                                                                               .ToList()).ToList()
                    //                  let combinations = GetCombination(3).Where(x => !x.Any(y => y > cells.Count - 1)).Select(x => x.Select(y => (int)y).ToArray()).ToList()
                    //                  from columns in combinations
                    //                  let t1 = cells[columns[0]]
                    //                  let t2 = cells[columns[1]]
                    //                  let t3 = cells[columns[2]]
                    //                  where (t1.PossibleNumbers.Count == 1 ? 1 : 0) + (t2.PossibleNumbers.Count == 1 ? 1 : 0) + (t3.PossibleNumbers.Count == 1 ? 1 : 0) <= 1
                    //                  group new
                    //                  {
                    //                      //Number = number,
                    //                      RowIndex = r,
                    //                      ColumnIndices = (new[] { t1.Index.ColumnIndex, t2.Index.ColumnIndex, t3.Index.ColumnIndex }).OrderBy(x => x).ToArray()
                    //                  } by number).ToDictionary(x => x.Key, x => x.ToList());
                    #endregion
                    var tuple = new List<Tuple<int, int[]>>();
                    for (int r = 0; r < Sudoku.GeneralFixedLength; r++)
                    {
                        var cells = _allPossibilities.Where(x => x.Index.RowIndex == r).ToList().Concat(curr[SubArrayType.Row, r]
                                                                                                        .Select((x, col) =>
                                                                                                                new Possibilities(new IndicesPair(r, col),
                                                                                                                                  x.HasValue ? 
                                                                                                                                  new List<IntSudoku>() { x.Value } : 
                                                                                                                                  new List<IntSudoku>(0)))
                                                                                                        .Where(x => x.PossibleNumbers.Count == 1)
                                                                                                        .ToList()).OrderBy(x => x.Index).ToList();
                                                                                                        //.OrderBy(x => x)
                                                                                                        //.Where(x => x.HasValue)
                                                                                                        //.Select((x, c) => new Possibilities(new IndicesPair(r, c), new[] { x.Value }))
                                                                                                        //.ToList()).ToList();
                        //var combinations = GetCombination(3).Where(x => !x.Any(y => y > cells.Count - 1)).Select(x => x.Select(y => (int)y).ToArray()).ToList();
                        //var combinations = comb.Where(x => !x.Any(y => y > cells.Count - 1)).ToList();
                        foreach (var columns in comb)
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
        #endregion

        private static bool _TrySolve(Sudoku matrix, out Sudoku solution, bool onlyBasicStrategies = false, bool validationControl = true)
        {
            solution = null;
            try
            {
                var sol = onlyBasicStrategies ? GetSolution_Basic(matrix) : GetSolution(matrix);
                solution = sol.Solution;
                return sol.HasSolution;
            }
            catch { return false; }

            #region . OLD .
            //#region . Validation Control .
            //////if (matrix == null)
            //////    throw new ArgumentNullException("matrix");
            //////if (!IsValid(matrix))
            //////    throw new MatrixNotValidException("matrix");
            //if (_IsSolved(matrix, true))
            //{
            //    solution = matrix.Clone() as Sudoku;
            //    return true;
            //}
            //#endregion
            //#region . Declarations .
            //Sudoku prev = null, curr = matrix.Clone() as Sudoku;
            //#endregion
            //#region . old .
            ////IntSudoku?[] row = null, column = null, square = null;
            ////List<IntSudoku> potentialValues = null;
            ////List<int> empties = null;
            ////bool newOne = false;
            ////#region . General Loop .
            ////do
            ////{
            //////prev = curr.Clone() as Sudoku;
            //////if (!TrySetAnyValues_0_ForcedValues(curr, out curr, null))
            //////    break;
            ////#region . Deciding for i-index Row .
            ////for (int i = 0; i < Sudoku.Length_Row; i++)
            ////    do
            ////    {
            ////        newOne = false;
            ////        row = curr[SubArrayType.Row, i];
            ////        potentialValues = PotentialValuesOf(row).ToList();
            ////        empties = EmptyIndicesOf(row).ToList();
            ////        var promisings = (from number in potentialValues
            ////                          let indices = from indices in empties
            ////                                        let ix = new IndicesPair(i, indices)
            ////                                        where !NotPotentialValuesOf(curr, ix).Contains(number)
            ////                                        select ix
            ////                          select new { number, indices = indices.ToList() }).ToList();
            ////        foreach (var item in promisings)
            ////        {
            ////            switch (item.indices.Count)
            ////            {
            ////                case 0:
            ////                    throw new InvalidMatrixException();
            ////                case 1:
            ////                    curr[item.indices.First()] = item.number;
            ////                    newOne = true;
            ////                    break;
            ////                default:
            ////                    break;
            ////            }
            ////            if (newOne)
            ////                break;
            ////        }
            ////    } while (newOne);
            ////#endregion
            ////#region . Deciding for i-index Column .
            ////for (int i = 0; i < Sudoku.Length_Column; i++)
            ////    do
            ////    {
            ////        newOne = false;
            ////        column = curr[SubArrayType.Column, i];
            ////        potentialValues = PotentialValuesOf(column).ToList();
            ////        empties = EmptyIndicesOf(column).ToList();
            ////        var promisings = (from number in potentialValues
            ////                          let indices = from indices in empties
            ////                                        let ix = new IndicesPair(indices, i)
            ////                                        where !NotPotentialValuesOf(curr, ix).Contains(number)
            ////                                        select ix
            ////                          select new { number, indices = indices.ToList() }).ToList();
            ////        foreach (var item in promisings)
            ////        {
            ////            switch (item.indices.Count)
            ////            {
            ////                case 0:
            ////                    throw new InvalidMatrixException();
            ////                case 1:
            ////                    curr[item.indices.First()] = item.number;
            ////                    newOne = true;
            ////                    break;
            ////                default:
            ////                    break;
            ////            }
            ////            if (newOne)
            ////                break;
            ////        }
            ////    } while (newOne);
            ////#endregion
            ////#region . Deciding for i-index Square .
            ////for (int i = 0; i < Sudoku.CountOfSquares; i++)
            ////    do
            ////    {
            ////        newOne = false;
            ////        square = curr[SubArrayType.Square, i];
            ////        potentialValues = PotentialValuesOf(square).ToList();
            ////        empties = EmptyIndicesOf(square).ToList();
            ////        var promisings = (from number in potentialValues
            ////                          let indices = (from indices in empties
            ////                                         let ix = Sudoku.RealCellIndices(i, indices)
            ////                                         where !NotPotentialValuesOf(curr, ix).Contains(number)
            ////                                         select ix).ToList()
            ////                          select new { number, indices = indices.ToList() }).ToList();
            ////        foreach (var item in promisings)
            ////        {
            ////            switch (item.indices.Count)
            ////            {
            ////                case 0:
            ////                    throw new InvalidMatrixException();
            ////                case 1:
            ////                    curr[item.indices.First()] = item.number;
            ////                    newOne = true;
            ////                    break;
            ////                default:
            ////                    break;
            ////            }
            ////            if (newOne)
            ////                break;
            ////        }
            ////    } while (newOne);
            ////#endregion
            ////#region . Deciding for All Empty Cells .
            ////do
            ////{
            ////    newOne = false;
            ////    var promisings = EmptyCells(curr).Select(indices => new { indices, numbers = PotentialValuesOf(curr, indices).ToList() }).ToList();
            ////    foreach (var item in promisings)
            ////    {
            ////        switch (item.numbers.Count)
            ////        {
            ////            case 0:
            ////                throw new InvalidMatrixException();
            ////            case 1:
            ////                curr[item.indices] = item.numbers.First();
            ////                newOne = true;
            ////                break;
            ////            default:
            ////                break;
            ////        }
            ////        if (newOne)
            ////            break;
            ////    }
            ////} while (newOne);
            ////#endregion
            ////} while (!prev.Equals(curr));
            ////#endregion
            //#endregion
            //#region . Other Strategies .
            //TrySetAnyValues_0_ForcedValues(curr, out curr, null);
            //if (!onlySimples && !_IsSolved(curr))
            //{
            //    List<Possibilities> newPos = null, _np = null;
            //    Func<List<Possibilities>> newPossibilities = () => { return _np != null && _np.Count > 0 ? (newPos = new List<Possibilities>(_np)) : newPos; };
            //    do
            //    {
            //        prev = curr.Clone() as Sudoku;
            //        if ((TrySetAnyValues_1_IntersectionRemoval(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
            //        ||  (TrySetAnyValues_2_NakedTuples(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
            //        ||  (TrySetAnyValues_3_HiddenTuples(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
            //        ||  (TrySetAnyValues_4_XWing(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
            //        ||  (TrySetAnyValues_5_SingleChains(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
            //        ||  (TrySetAnyValues_6_YWing(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))
            //        ||  (TrySetAnyValues_7_Swordfish(curr, out curr, newPossibilities(), out _np, true) && _TrySolve(curr, out curr, true, true))))
            //        {
            //            solution = curr.Clone() as Sudoku;
            //            return true;
            //        }
            //    } while (!prev.Equals(curr));
            //}
            //#endregion
            //#region . Return Value .
            //return _IsSolved((solution = curr.Clone() as Sudoku));
            //#endregion
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
                var sol = GetSolution_Basic(curr);
                if (ok = sol.HasSolution)
                    yield return sol.Solution;
            }
            //if (trySolve && TrySolve(curr, out curr))
            //    yield return curr.Clone() as Sudoku;
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
                var sol = GetSolution_Basic(curr);
                if (ok = sol.HasSolution)
                    yield return sol.Solution;
            }
            //if (trySolve && TrySolve(curr, out curr))
            //    yield return curr.Clone() as Sudoku;
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

        private static SudokuSolution GetSolution_Basic(Sudoku matrix)
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
                    PossibilitiesOfEmptyCells = null,
                    Solution = matrix.Clone() as Sudoku
                };
            #endregion
            #region . Declarations .
            bool possChanged = false;
            int k = -1, i = -1, length = -1;
            Sudoku curr = matrix.Clone() as Sudoku;
            SudokuSolution sol = null;
            List<IntSudoku> promisingNumbers = null;
            IntSudoku[] __numbers = null;
            IntSudoku value = 1, __value = 1, n1 = 1, n2 = 1, n3 = 1, n4 = 1, __number = 1;
            List<Possibilities> allPossibilities = AllPossibilities(curr).ToList(), __newPossibilities = null, cells = null, containers = null;
            Possibilities cell = null;
            IndicesPair __index = new IndicesPair();
            IndicesPair[] __indices = null;
            Action concatPossibilities = () =>
            {
                if (possChanged = __newPossibilities != null && __newPossibilities.Count > 0)
                    allPossibilities = __newPossibilities.Concat(allPossibilities.Where(x => !__newPossibilities.Any(y => y.Index.Equals(x.Index))).ToList()).OrderBy(x => x.Index).ToList();
                __newPossibilities = null;
            };
            Action removePossibleNumbers = () =>
            {
                __newPossibilities = allPossibilities.Where(x => __indices.Contains(x.Index)).Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(__numbers).ToList())).ToList();
                __indices = null;
                __numbers = null;
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
                __index = new IndicesPair();
                __value = new IntSudoku();
                if (allPossibilities.Count == 0)
                {
                    sol = new SudokuSolution()
                    {
                        HasSolution = true,
                        Matrix = matrix.Clone() as Sudoku,
                        Solution = curr.Clone() as Sudoku,
                        PossibilitiesOfEmptyCells = null
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
                while ((cells = allPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList()).Count != 0)
                    foreach (var _cell in cells)
                    {
                        __index = _cell.Index;
                        __value = _cell.PossibleNumbers.Single();
                        setValue();
                    }
                if (sol != null)
                    break;
                possChanged = false;
                for (k = 0; k < 3; k++)
                {
                    for (i = 0, length = k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : (k == 2 ? Sudoku.CountOfSquares : -1)); i < length; i++)
                    {
                        cells = (k == 0 ? allPossibilities.Where(x => x.Index.RowIndex == i) :
                                (k == 1 ? allPossibilities.Where(x => x.Index.ColumnIndex == i) :
                                (k == 2 ? allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                                new List<Possibilities>()))).ToList();
                        if (cells.Count == 0)
                            continue;
                        if (cells.Count == 1)
                        {
                            cell = cells.Single();
                            __index = cell.Index;
                            if (cell.PossibleNumbers.Count != 1)
                                throw new InvalidMatrixException(nameof(matrix));
                            __value = cell.PossibleNumbers.Single();
                            setValue();
                            break;
                        }
                        promisingNumbers = cells.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                        if (cells.Count != promisingNumbers.Count)
                            throw new InvalidMatrixException();
                        foreach (var number in promisingNumbers)
                        {
                            containers = cells.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                            if (containers.Count == 0)
                                throw new InvalidMatrixException(nameof(matrix));
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
                if (possChanged)
                    continue;
            } while (possChanged && sol == null);
            #endregion
            #region . Return Value .
            if (sol != null)
                return sol;
            //if (!IsValid(curr))
            //    throw new InvalidMatrixException();
            return new SudokuSolution()
            {
                HasSolution = false,
                Matrix = matrix.Clone() as Sudoku,
                PossibilitiesOfEmptyCells = new ReadOnlyCollection<Possibilities>(allPossibilities),
                Solution = curr.Clone() as Sudoku
            };
            #endregion
        }
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
                    PossibilitiesOfEmptyCells = null,
                    Solution = matrix.Clone() as Sudoku
                };
            #endregion
            #region . Declarations .
            var strategies = new List<SolvingStrategies>();
            bool possChanged = false, nakedOrHidden = false, remotePairOrSingleChain = false;
            int k = -1, i = -1, length = -1, m = -1, a = -1, b = -1, c = -1, d = -1, j = -1, s = -1, 
                __row1 = -1, __row2 = -1, __row3 = -1, __column1 = -1, __column2 = -1, __column3 = -1;
            Sudoku curr = matrix.Clone() as Sudoku;
            SudokuSolution sol = null;
            List<IntSudoku> promisingNumbers = null, certainValues = null, pn = null, nCont1 = null, nCont2 = null, nCont3 = null, nCont4 = null, nConts = null;
            IntSudoku[] __numbers = null, numbers;
            IntSudoku value = 1, __value = 1, n1 = 1, n2 = 1, n3 = 1, n4 = 1, __number = 1;
            List<Possibilities> allPossibilities = AllPossibilities(curr).ToList(), __newPossibilities = null, cells = null, containers = null, 
                containers2 = null, allForms = null, cont1 = null, cont2 = null, cont3 = null, cont4 = null;
            List<PossibilitiesMulti> nakedTuples = null;
            Possibilities cell = null, first = null, t1 = null, t2 = null, t3 = null, t4 = null, p = null;
            IndicesPair __index = new IndicesPair();
            List<IndicesPair> certainCells = null, iCont1 = null, iCont2 = null, iCont3 = null, iCont4 = null, iConts = null;
            IndicesPair[] __indices = null;
            Chains singleChains = null;
            ChainForm form1 = null, form2 = null;
            List<Swordfish> swordfishes = null;
            Action concatPossibilities = () =>
            {
                if (possChanged = __newPossibilities != null && __newPossibilities.Count > 0)
                    allPossibilities = __newPossibilities.Concat(allPossibilities.Where(x => !__newPossibilities.Any(y => y.Index.Equals(x.Index))).ToList()).OrderBy(x => x.Index).ToList();
                __newPossibilities = null;
            };
            Action removePossibleNumbers = () =>
            {
                __newPossibilities = allPossibilities.Where(x => __indices.Contains(x.Index)).Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(__numbers).ToList())).ToList();
                __indices = null;
                __numbers = null;
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
                __index = new IndicesPair();
                __value = new IntSudoku();
                if (allPossibilities.Count == 0)
                {
                    sol = new SudokuSolution()
                    {
                        HasSolution = true,
                        Matrix = matrix.Clone() as Sudoku,
                        Solution = curr.Clone() as Sudoku,
                        PossibilitiesOfEmptyCells = null,
                        UsedStrategies = new ReadOnlyCollection<SolvingStrategies>(strategies)//.Distinct().ToList())
                    };
                    possChanged = true;
                    return;
                }
                removePossibleNumbers();
            };
            Func<bool> areColumnsOK = () =>
            {
                int i1 = ((p = allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(__row1, __column1)))) == null ? 1 : (p.PossibleNumbers.Contains(__number) ? 0 : 1))
                       + ((p = allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(__row2, __column1)))) == null ? 1 : (p.PossibleNumbers.Contains(__number) ? 0 : 1))
                       + ((p = allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(__row3, __column1)))) == null ? 1 : (p.PossibleNumbers.Contains(__number) ? 0 : 1));
                if (i1 >= 1)
                    return false;
                int i2 = ((p = allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(__row1, __column2)))) == null ? 1 : (p.PossibleNumbers.Contains(__number) ? 0 : 1))
                       + ((p = allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(__row2, __column2)))) == null ? 1 : (p.PossibleNumbers.Contains(__number) ? 0 : 1))
                       + ((p = allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(__row3, __column2)))) == null ? 1 : (p.PossibleNumbers.Contains(__number) ? 0 : 1));
                if (i2 >= 1)
                    return false;
                int i3 = ((p = allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(__row1, __column3)))) == null ? 1 : (p.PossibleNumbers.Contains(__number) ? 0 : 1))
                       + ((p = allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(__row2, __column3)))) == null ? 1 : (p.PossibleNumbers.Contains(__number) ? 0 : 1))
                       + ((p = allPossibilities.SingleOrDefault(x => x.Index.Equals(new IndicesPair(__row3, __column3)))) == null ? 1 : (p.PossibleNumbers.Contains(__number) ? 0 : 1));
                if (i3 >= 1)
                    return false;
                return true;
            };
            #endregion
            #region . General Loop .
            do
            {
                #region . Strategies: [0], [1], [2], [3], [4] .
                #region . 0 | Setting Values to Singles .
                #region . Validation Control .
                //if (allPossibilities.Count == 0)
                //{
                //    if (!IsValid(curr))
                //        throw new InvalidMatrixException();
                //    break;
                //}
                //if (allPossibilities.Any(x => x.PossibleNumbers.Count == 0))
                //    throw new InvalidMatrixException(nameof(matrix));
                //if (IsSolved(matrix, false))
                //    return new SudokuSolution()
                //    {
                //        HasSolution = true,
                //        Matrix = matrix.Clone() as Sudoku,
                //        _possibilitiesOfEmptyCells = new List<Possibilities>(),
                //        Solution = curr.Clone() as Sudoku
                //    };
                #endregion
                while ((cells = allPossibilities.Where(x => x.PossibleNumbers.Count == 1).ToList()).Count != 0)
                {
                    foreach (var _cell in cells)
                    {
                        __index = _cell.Index;
                        __value = _cell.PossibleNumbers.Single();
                        setValue();
                    }
                    strategies.Add(SolvingStrategies.ForcedValues);
                }
                if (sol != null)
                    break;
                possChanged = false;
                #endregion
                for (k = 0; k < 3; k++)
                {
                    #region . k-th Unit's Loop .
                    for (i = 0, length = k == 0 ? Sudoku.Length_Row : (k == 1 ? Sudoku.Length_Column : (k == 2 ? Sudoku.CountOfSquares : -1)); i < length; i++)
                    {
                        #region . Preparing i-th of k-th Unit's Cells .
                        cells = (k == 0 ? allPossibilities.Where(x => x.Index.RowIndex == i) :
                                (k == 1 ? allPossibilities.Where(x => x.Index.ColumnIndex == i) :
                                (k == 2 ? allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == i) :
                                new List<Possibilities>()))).ToList();
                        if (cells.Count == 0)
                            continue;
                        #endregion
                        #region . 0 | Setting Forced Values .
                        if (cells.Count == 1)
                        {
                            cell = cells.Single();
                            __index = cell.Index;
                            if (cell.PossibleNumbers.Count != 1)
                                throw new InvalidMatrixException(nameof(matrix));
                            __value = cell.PossibleNumbers.Single();
                            setValue();
                            strategies.Add(SolvingStrategies.ForcedValues);
                            break;
                        }
                        #endregion
                        promisingNumbers = cells.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                        if (cells.Count != promisingNumbers.Count)
                            throw new InvalidMatrixException();
                        foreach (var number in promisingNumbers)
                        {
                            containers = cells.Where(x => x.PossibleNumbers.Contains(number)).ToList();
                            #region . 0 | Hidden Singles .
                            if (containers.Count == 0)
                                throw new InvalidMatrixException(nameof(matrix));
                            if (containers.Count == 1)
                            {
                                //__newPossibilities = new List<Possibilities>() { new Possibilities(containers.Single().Index, new[] { number }) };
                                //concatPossibilities();
                                //if (possChanged)
                                //    break;
                                cell = containers.Single();
                                __index = cell.Index;
                                __value = number;
                                setValue();
                                strategies.Add(SolvingStrategies.HiddenSingles);
                                break;
                            }
                            #endregion
                            #region . 1 | Intersection Removal (Pointing Tuples (k<2) & Box/Line Reduction (k=2)) .
                            if (containers.Count <= Sudoku.FixedLengthForSquares)
                            {
                                first = containers.First();
                                containers2 = ((k == 0 || k == 1 ?
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
                                               .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { number }).ToList())))
                                               .Where(x => allPossibilities.Single(y => y.Index.Equals(x.Index)).PossibleNumbers.Count > x.PossibleNumbers.Count).ToList();
                                if (containers2.Count > 0)
                                {
                                    __newPossibilities = containers2
                                                         .Where(x => allPossibilities.Single(y => y.Index.Equals(x.Index)).PossibleNumbers.Count > x.PossibleNumbers.Count).ToList();
                                    concatPossibilities();
                                    if (possChanged)
                                    {
                                        strategies.Add(k == 2 ? SolvingStrategies.BoxLineReduction : SolvingStrategies.PointingPairs);
                                        break;
                                    }
                                }
                            }
                            #endregion
                        }
                        if (possChanged)
                            break;
                        #region . 2 | Naked Tuples and 3 | Hidden Naked Tuples .
                        if (allForms == null)
                            allForms = allPossibilities.Where(x => x.PossibleNumbers.Count <= 4).ToList();
                        __newPossibilities = new List<Possibilities>();
                        for (m = 2; m < 4; m++)
                        {
                            if (cells.Count <= m)
                                continue;
                            #region . Getting Tuples .
                            nakedTuples = new List<PossibilitiesMulti>();
                            switch (m)
                            {
                                case 2:
                                    for (a = 0; a < cells.Count; a++)
                                    {
                                        t1 = cells[a];
                                        for (b = a + 1; b < cells.Count; b++)
                                        {
                                            t2 = cells[b];
                                            pn = t1.PossibleNumbers.Concat(t2.PossibleNumbers).Distinct().ToList();
                                            if (pn.Count == 2)
                                                nakedTuples.Add(new PossibilitiesMulti(new[] { t1.Index, t2.Index }, pn));
                                        }
                                    }
                                    #region . Hidden Naked Tuples .
                                    if (nakedTuples.Count == 0)
                                        for (a = 0; a < promisingNumbers.Count; a++)
                                        {
                                            n1 = promisingNumbers[a];
                                            cont1 = cells.Where(x => x.PossibleNumbers.Contains(n1)).ToList();
                                            if (cont1.Count > m)
                                                continue;
                                            iCont1 = cont1.Select(x => x.Index).ToList();
                                            nCont1 = cont1.SelectMany(x => x.PossibleNumbers).ToList();
                                            for (b = a + 1; b < promisingNumbers.Count; b++)
                                            {
                                                n2 = promisingNumbers[b];
                                                cont2 = cells.Where(x => x.PossibleNumbers.Contains(n2)).ToList();
                                                if (cont2.Count > m)
                                                    continue;
                                                iCont2 = cont2.Select(x => x.Index).ToList();
                                                nCont2 = cont2.SelectMany(x => x.PossibleNumbers).ToList();
                                                if ((iConts = iCont1.Concat(iCont2).Distinct().ToList()).Count == m &&
                                                    (nConts = nCont1.Concat(nCont2).Distinct().ToList()).Count > m)
                                                {
                                                    numbers = new[] { n1, n2 };
                                                    __newPossibilities.AddRange(iConts.Select(x => new Possibilities(x, cells.Single(y => y.Index.Equals(x)).PossibleNumbers
                                                                                                                        .Where(y => numbers.Contains(y)).ToList())));
                                                }
                                            }
                                        }
                                    #endregion
                                    break;
                                case 3:
                                    for (a = 0; a < cells.Count; a++)
                                    {
                                        t1 = cells[a];
                                        for (b = a + 1; b < cells.Count; b++)
                                        {
                                            t2 = cells[b];
                                            for (c = b + 1; c < cells.Count; c++)
                                            {
                                                t3 = cells[c];
                                                pn = t1.PossibleNumbers.Concat(t2.PossibleNumbers).Concat(t3.PossibleNumbers).Distinct().ToList();
                                                if (pn.Count == 3)
                                                    nakedTuples.Add(new PossibilitiesMulti(new[] { t1.Index, t2.Index, t3.Index }, pn));
                                            }
                                        }
                                    }
                                    #region . Hidden Naked Tuples .
                                    if (nakedTuples.Count == 0)
                                        for (a = 0; a < promisingNumbers.Count; a++)
                                        {
                                            n1 = promisingNumbers[a];
                                            cont1 = cells.Where(x => x.PossibleNumbers.Contains(n1)).ToList();
                                            if (cont1.Count > m)
                                                continue;
                                            iCont1 = cont1.Select(x => x.Index).ToList();
                                            nCont1 = cont1.SelectMany(x => x.PossibleNumbers).ToList();
                                            for (b = a + 1; b < promisingNumbers.Count; b++)
                                            {
                                                n2 = promisingNumbers[b];
                                                cont2 = cells.Where(x => x.PossibleNumbers.Contains(n2)).ToList();
                                                if (cont2.Count > m)
                                                    continue;
                                                iCont2 = cont2.Select(x => x.Index).ToList();
                                                nCont2 = cont2.SelectMany(x => x.PossibleNumbers).ToList();
                                                for (c = b + 1; c < promisingNumbers.Count; c++)
                                                {
                                                    n3 = promisingNumbers[c];
                                                    cont3 = cells.Where(x => x.PossibleNumbers.Contains(n3)).ToList();
                                                    if (cont3.Count > m)
                                                        continue;
                                                    iCont3 = cont3.Select(x => x.Index).ToList();
                                                    nCont3 = cont3.SelectMany(x => x.PossibleNumbers).ToList();
                                                    if ((iConts = iCont1.Concat(iCont2).Concat(iCont3).Distinct().ToList()).Count == m &&
                                                        (nConts = nCont1.Concat(nCont2).Concat(nCont3).Distinct().ToList()).Count > m)
                                                    {
                                                        numbers = new[] { n1, n2, n3 };
                                                        __newPossibilities.AddRange(iConts.Select(x => new Possibilities(x, cells.Single(y => y.Index.Equals(x)).PossibleNumbers
                                                                                                                            .Where(y => numbers.Contains(y)).ToList())));
                                                    }
                                                }
                                            }
                                        }
                                    #endregion
                                    break;
                                case 4:
                                    for (a = 0; a < cells.Count; a++)
                                    {
                                        t1 = cells[a];
                                        for (b = a + 1; b < cells.Count; b++)
                                        {
                                            t2 = cells[b];
                                            for (c = b + 1; c < cells.Count; c++)
                                            {
                                                t3 = cells[c];
                                                for (d = c + 1; d < cells.Count; d++)
                                                {
                                                    t4 = cells[d];
                                                    pn = t1.PossibleNumbers.Concat(t2.PossibleNumbers).Concat(t3.PossibleNumbers).Concat(t4.PossibleNumbers).Distinct().ToList();
                                                    if (pn.Count == 4)
                                                        nakedTuples.Add(new PossibilitiesMulti(new[] { t1.Index, t2.Index, t3.Index, t4.Index }, pn));
                                                }
                                            }
                                        }
                                    }
                                    #region . Hidden Naked Tuples .
                                    if (nakedTuples.Count == 0)
                                        for (a = 0; a < promisingNumbers.Count; a++)
                                        {
                                            n1 = promisingNumbers[a];
                                            cont1 = cells.Where(x => x.PossibleNumbers.Contains(n1)).ToList();
                                            if (cont1.Count > m)
                                                continue;
                                            iCont1 = cont1.Select(x => x.Index).ToList();
                                            nCont1 = cont1.SelectMany(x => x.PossibleNumbers).ToList();
                                            for (b = a + 1; b < promisingNumbers.Count; b++)
                                            {
                                                n2 = promisingNumbers[b];
                                                cont2 = cells.Where(x => x.PossibleNumbers.Contains(n2)).ToList();
                                                iCont2 = cont2.Select(x => x.Index).ToList();
                                                if (cont2.Count > m)
                                                    continue;
                                                nCont2 = cont2.SelectMany(x => x.PossibleNumbers).ToList();
                                                for (c = b + 1; c < promisingNumbers.Count; c++)
                                                {
                                                    n3 = promisingNumbers[c];
                                                    cont3 = cells.Where(x => x.PossibleNumbers.Contains(n3)).ToList();
                                                    if (cont3.Count > m)
                                                        continue;
                                                    iCont3 = cont3.Select(x => x.Index).ToList();
                                                    nCont3 = cont3.SelectMany(x => x.PossibleNumbers).ToList();
                                                    for (d = c + 1; d < promisingNumbers.Count; d++)
                                                    {
                                                        n4 = promisingNumbers[d];
                                                        cont4 = cells.Where(x => x.PossibleNumbers.Contains(n4)).ToList();
                                                        if (cont4.Count > m)
                                                            continue;
                                                        iCont4 = cont4.Select(x => x.Index).ToList();
                                                        nCont4 = cont4.SelectMany(x => x.PossibleNumbers).ToList();
                                                        if ((iConts = iCont1.Concat(iCont2).Concat(iCont3).Concat(iCont4).Distinct().ToList()).Count == m &&
                                                            (nConts = nCont1.Concat(nCont2).Concat(nCont3).Concat(nCont4).Distinct().ToList()).Count > m)
                                                        {
                                                            numbers = new[] { n1, n2, n3, n4 };
                                                            __newPossibilities.AddRange(iConts.Select(x => new Possibilities(x, cells.Single(y => y.Index.Equals(x)).PossibleNumbers
                                                                                                                                .Where(y => numbers.Contains(y)).ToList())));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    #endregion
                                    break;
                            }
                            #endregion
                            if (nakedOrHidden = nakedTuples.Count > 0)
                            {
                                certainValues = nakedTuples.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                                certainCells = nakedTuples.SelectMany(x => x.Indices).Distinct().ToList();
                                __newPossibilities = (from empty in cells
                                                      where !certainCells.Contains(empty.Index)
                                                      let newPn = empty.PossibleNumbers.Except(certainValues).ToList()
                                                      select new
                                                      {
                                                          Entity = new Possibilities(empty.Index, newPn),
                                                          IsJustDiscovered = empty.PossibleNumbers.Count > newPn.Count
                                                      }).ToList().Where(x => x.IsJustDiscovered).Select(x => x.Entity).ToList();
                            }
                            concatPossibilities();
                            if (possChanged)
                            {
                                strategies.Add(m == 2 ? (nakedOrHidden ? SolvingStrategies.NakedPairs : SolvingStrategies.HiddenPairs) :
                                              (m == 3 ? (nakedOrHidden ? SolvingStrategies.NakedTriples : SolvingStrategies.HiddenTriples) :
                                                        (nakedOrHidden ? SolvingStrategies.NakedQuads : SolvingStrategies.HiddenQuads)));
                                break;
                            }
                        }
                        if (possChanged)
                            break;
                        #region . old | hiddens .
                        //#region . Hidden Naked Tuples .
                        //__newPossibilities = new List<Possibilities>();
                        //for (m = 2; m <= 4; m++)
                        //{
                        //    if (cells.Count <= m)
                        //        continue;
                        //    #region . Getting Hidden Naked Tuples .
                        //    switch (m)
                        //    {
                        //        case 2:
                        //            for (a = 0; a < promisingNumbers.Count; a++)
                        //            {
                        //                n1 = promisingNumbers[a];
                        //                cont1 = cells.Where(x => x.PossibleNumbers.Contains(n1)).ToList();
                        //                if (cont1.Count > m)
                        //                    continue;
                        //                iCont1 = cont1.Select(x => x.Index).ToList();
                        //                nCont1 = cont1.SelectMany(x => x.PossibleNumbers).ToList();
                        //                for (b = a + 1; b < promisingNumbers.Count; b++)
                        //                {
                        //                    n2 = promisingNumbers[b];
                        //                    cont2 = cells.Where(x => x.PossibleNumbers.Contains(n2)).ToList();
                        //                    if (cont2.Count > m)
                        //                        continue;
                        //                    iCont2 = cont2.Select(x => x.Index).ToList();
                        //                    nCont2 = cont2.SelectMany(x => x.PossibleNumbers).ToList();
                        //                    if ((iConts = iCont1.Concat(iCont2).Distinct().ToList()).Count == m && 
                        //                        (nConts = nCont1.Concat(nCont2).Distinct().ToList()).Count > m)
                        //                    {
                        //                        numbers = new[] { n1, n2 };
                        //                        __newPossibilities.AddRange(iConts.Select(x => new Possibilities(x, cells.Single(y => y.Index.Equals(x)).PossibleNumbers
                        //                                                                                            .Where(y => numbers.Contains(y)).ToList())));
                        //                    }
                        //                }
                        //            }
                        //            break;
                        //        case 3:
                        //            for (a = 0; a < promisingNumbers.Count; a++)
                        //            {
                        //                n1 = promisingNumbers[a];
                        //                cont1 = cells.Where(x => x.PossibleNumbers.Contains(n1)).ToList();
                        //                if (cont1.Count > m)
                        //                    continue;
                        //                iCont1 = cont1.Select(x => x.Index).ToList();
                        //                nCont1 = cont1.SelectMany(x => x.PossibleNumbers).ToList();
                        //                for (b = a + 1; b < promisingNumbers.Count; b++)
                        //                {
                        //                    n2 = promisingNumbers[b];
                        //                    cont2 = cells.Where(x => x.PossibleNumbers.Contains(n2)).ToList();
                        //                    if (cont2.Count > m)
                        //                        continue;
                        //                    iCont2 = cont2.Select(x => x.Index).ToList();
                        //                    nCont2 = cont2.SelectMany(x => x.PossibleNumbers).ToList();
                        //                    for (c = b + 1; c < promisingNumbers.Count; c++)
                        //                    {
                        //                        n3 = promisingNumbers[c];
                        //                        cont3 = cells.Where(x => x.PossibleNumbers.Contains(n3)).ToList();
                        //                        if (cont3.Count > m)
                        //                            continue;
                        //                        iCont3 = cont3.Select(x => x.Index).ToList();
                        //                        nCont3 = cont3.SelectMany(x => x.PossibleNumbers).ToList();
                        //                        if ((iConts = iCont1.Concat(iCont2).Concat(iCont3).Distinct().ToList()).Count == m &&
                        //                            (nConts = nCont1.Concat(nCont2).Concat(nCont3).Distinct().ToList()).Count > m)
                        //                        {
                        //                            numbers = new[] { n1, n2, n3 };
                        //                            __newPossibilities.AddRange(iConts.Select(x => new Possibilities(x, cells.Single(y => y.Index.Equals(x)).PossibleNumbers
                        //                                                                                                .Where(y => numbers.Contains(y)).ToList())));
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //            break;
                        //        case 4:
                        //            for (a = 0; a < promisingNumbers.Count; a++)
                        //            {
                        //                n1 = promisingNumbers[a];
                        //                cont1 = cells.Where(x => x.PossibleNumbers.Contains(n1)).ToList();
                        //                if (cont1.Count > m)
                        //                    continue;
                        //                iCont1 = cont1.Select(x => x.Index).ToList();
                        //                nCont1 = cont1.SelectMany(x => x.PossibleNumbers).ToList();
                        //                for (b = a + 1; b < promisingNumbers.Count; b++)
                        //                {
                        //                    n2 = promisingNumbers[b];
                        //                    cont2 = cells.Where(x => x.PossibleNumbers.Contains(n2)).ToList();
                        //                    iCont2 = cont2.Select(x => x.Index).ToList();
                        //                    if (cont2.Count > m)
                        //                        continue;
                        //                    nCont2 = cont2.SelectMany(x => x.PossibleNumbers).ToList();
                        //                    for (c = b + 1; c < promisingNumbers.Count; c++)
                        //                    {
                        //                        n3 = promisingNumbers[c];
                        //                        cont3 = cells.Where(x => x.PossibleNumbers.Contains(n3)).ToList();
                        //                        if (cont3.Count > m)
                        //                            continue;
                        //                        iCont3 = cont3.Select(x => x.Index).ToList();
                        //                        nCont3 = cont3.SelectMany(x => x.PossibleNumbers).ToList();
                        //                        for (d = c + 1; d < promisingNumbers.Count; d++)
                        //                        {
                        //                            n4 = promisingNumbers[d];
                        //                            cont4 = cells.Where(x => x.PossibleNumbers.Contains(n4)).ToList();
                        //                            if (cont4.Count > m)
                        //                                continue;
                        //                            iCont4 = cont4.Select(x => x.Index).ToList();
                        //                            nCont4 = cont4.SelectMany(x => x.PossibleNumbers).ToList();
                        //                            if ((iConts = iCont1.Concat(iCont2).Concat(iCont3).Concat(iCont4).Distinct().ToList()).Count == m &&
                        //                                (nConts = nCont1.Concat(nCont2).Concat(nCont3).Concat(nCont4).Distinct().ToList()).Count > m)
                        //                            {
                        //                                numbers = new[] { n1, n2, n3, n4 };
                        //                                __newPossibilities.AddRange(iConts.Select(x => new Possibilities(x, cells.Single(y => y.Index.Equals(x)).PossibleNumbers
                        //                                                                                                    .Where(y => numbers.Contains(y)).ToList())));
                        //                            }
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //            break;
                        //    }
                        //    #endregion
                        //    if (__newPossibilities.Count > 0)
                        //        break;
                        //}
                        //concatPossibilities();
                        //if (possChanged)
                        //    break;
                        //#endregion
                        #endregion
                        #endregion
                        #region . 4 | X Wing .
                        if (k != 2)
                            foreach (var number in promisingNumbers)
                            {
                                containers = cells.Where(x => x.PossibleNumbers.Contains(number)).OrderBy(x => x.Index).ToList();
                                if (containers.Count != 2)
                                    continue;
                                for (j = i + 1; j < length; j++)
                                {
                                    containers2 = (k == 0 ? (allPossibilities.Where(x => x.Index.RowIndex == j && x.PossibleNumbers.Contains(number))) :
                                                  (k == 1 ? (allPossibilities.Where(x => x.Index.ColumnIndex == j && x.PossibleNumbers.Contains(number))) :
                                                  new List<Possibilities>())).OrderBy(x => x.Index).ToList();
                                    if (containers2.Count != 2)
                                        continue;
                                    if (k == 0 && (containers2[0].Index.ColumnIndex == containers[0].Index.ColumnIndex &&
                                                   containers2[1].Index.ColumnIndex == containers[1].Index.ColumnIndex))
                                        __newPossibilities = allPossibilities.Where(x => x.PossibleNumbers.Contains(number) &&
                                                                                         x.Index.RowIndex != containers[0].Index.RowIndex &&
                                                                                         x.Index.RowIndex != containers2[0].Index.RowIndex &&
                                                                                         (x.Index.ColumnIndex == containers[0].Index.ColumnIndex || x.Index.ColumnIndex == containers[1].Index.ColumnIndex))
                                                                             .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { number })))
                                                                             .ToList();
                                    else if (k == 1 && (containers2[0].Index.RowIndex == containers[0].Index.RowIndex &&
                                                        containers2[1].Index.RowIndex == containers[1].Index.RowIndex))
                                        __newPossibilities = allPossibilities.Where(x => x.PossibleNumbers.Contains(number) &&
                                                                                         x.Index.ColumnIndex != containers[0].Index.ColumnIndex &&
                                                                                         x.Index.ColumnIndex != containers2[0].Index.ColumnIndex &&
                                                                                         (x.Index.RowIndex == containers[0].Index.RowIndex || x.Index.RowIndex == containers[1].Index.RowIndex))
                                                                             .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { number })))
                                                                             .ToList();
                                    else
                                        __newPossibilities = null;
                                    concatPossibilities();
                                    if (possChanged)
                                    {
                                        strategies.Add(SolvingStrategies.XWing);
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
                    if (possChanged)
                        break;
                    #endregion
                }
                if (possChanged)
                    continue;
                #endregion
                promisingNumbers = allPossibilities.SelectMany(x => x.PossibleNumbers).Distinct().ToList();
                #region . 5 | Single Chains and 9 | Remote Pairs .
                foreach (var number in promisingNumbers)
                {
                    singleChains = GetSingleChains(allPossibilities, number);
                    #region . Deciding for Single Chains .
                    foreach (var singleChain in singleChains)
                    {
                        for (i = 0; i < singleChain.Count; i++)
                        {
                            form1 = singleChain[i];
                            for (j = i + 1; j < singleChain.Count; j++)
                            {
                                remotePairOrSingleChain = false;
                                form2 = singleChain[j];
                                if (form1.Index.Equals(form2.Index))
                                    continue;
                                #region . Deciding for Forms Which Ones Are In The Same Unit .
                                if (form1.Index.RowIndex == form2.Index.RowIndex ||
                                    form1.Index.ColumnIndex == form2.Index.ColumnIndex ||
                                    Sudoku.LinkedSquareIndexOfCell(form1.Index) == Sudoku.LinkedSquareIndexOfCell(form2.Index))
                                {
                                    #region . The Same Flag Which Appears Twice in a Unit .
                                    if (form1.Flag == form2.Flag)
                                        __newPossibilities = singleChain.Where(x => x.Flag != form1.Flag).Select(x => new Possibilities(x.Index, new[] { x.Number })).ToList();
                                    #endregion
                                    #region . Opposite Flags Which Appear in a Unit .
                                    else
                                    {
                                        #region . The Same Square .
                                        s = Sudoku.LinkedSquareIndexOfCell(form1.Index);
                                        if (s == Sudoku.LinkedSquareIndexOfCell(form2.Index))
                                        {
                                            __newPossibilities = allPossibilities.Where(x => Sudoku.LinkedSquareIndexOfCell(x.Index) == s &&
                                                                                             !x.Index.Equals(form1.Index) &&
                                                                                             !x.Index.Equals(form2.Index) &&
                                                                                             x.PossibleNumbers.Contains(number))
                                                                                 .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList()))
                                                                                 .ToList();
                                            concatPossibilities();
                                            if (possChanged)
                                                break;
                                        }
                                        #endregion
                                        #region . The Same Row .
                                        if (form1.Index.RowIndex == form2.Index.RowIndex)
                                            __newPossibilities = allPossibilities.Where(x => x.Index.RowIndex == form1.Index.RowIndex &&
                                                                                             x.Index.ColumnIndex != form2.Index.ColumnIndex &&
                                                                                             x.Index.ColumnIndex != form1.Index.ColumnIndex &&
                                                                                             x.PossibleNumbers.Contains(number))
                                                                                 .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList()))
                                                                                 .ToList();
                                        #endregion
                                        #region . The Same Column .
                                        else if (form1.Index.ColumnIndex == form2.Index.ColumnIndex)
                                            __newPossibilities = allPossibilities.Where(x => x.Index.ColumnIndex == form1.Index.ColumnIndex &&
                                                                                             x.Index.RowIndex != form2.Index.RowIndex &&
                                                                                             x.Index.RowIndex != form1.Index.RowIndex &&
                                                                                             x.PossibleNumbers.Contains(number))
                                                                                 .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Where(y => y != number).ToList()))
                                                                                 .ToList();
                                        #endregion
                                        else
                                            __newPossibilities = null;
                                    }
                                    #endregion
                                    concatPossibilities();
                                    if (possChanged)
                                        break;
                                }
                                #endregion
                                #region . 9 | Remote Pairs ("Seeing"s) .
                                if (remotePairOrSingleChain = form1.Flag != form2.Flag)
                                {
                                    //iConts = Sudoku.LinkedCellsOf(form1.Index).Intersect(Sudoku.LinkedCellsOf(form2.Index)).ToList();
                                    //__newPossibilities = allPossibilities.Where(x => iConts.Any(y => y.Equals(x.Index)) && x.PossibleNumbers.Contains(number))
                                    //                                     .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { number }).ToList())).ToList();
                                    __newPossibilities = Sudoku.LinkedCellsOf(form1.Index).Intersect(Sudoku.LinkedCellsOf(form2.Index))
                                        .Select(x => allPossibilities.SingleOrDefault(y => y.Index.Equals(x)))
                                        .Where(x => x != null && x.PossibleNumbers.Contains(number))
                                        .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { number }).ToList()))
                                        .ToList();
                                    concatPossibilities();
                                    if (possChanged)
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
                if (possChanged)
                {
                    strategies.Add(!remotePairOrSingleChain ? SolvingStrategies.SingleChains : SolvingStrategies.RemotePairs);
                    continue;
                }
                #endregion
                #region . 6 | XY Wing .
                #region . old .
                ////__newPossibilities = new List<Possibilities>();
                ////xyWings = GetXYWings(allPossibilities);
                ////foreach (var yWing in xyWings)
                ////    __newPossibilities.AddRange(
                ////        Sudoku.LinkedCellsOf(yWing.Pincer1).Intersect(Sudoku.LinkedCellsOf(yWing.Pincer2)).Except(new[] { yWing.Pivot })
                ////        .Select(x => allPossibilities.SingleOrDefault(y => y.Index.Equals(x)))
                ////        .Where(x => x != null && x.PossibleNumbers.Contains(yWing.Pincer1ToPincer2))
                ////        .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { yWing.Pincer1ToPincer2 }))));
                //////{iConts = Sudoku.LinkedCellsOf(yWing.Pincer1).Intersect(Sudoku.LinkedCellsOf(yWing.Pincer2)).Except(new[] { yWing.Pivot }).ToList();
                //////__newPossibilities.AddRange(allPossibilities
                //////                             .Where(x => iConts.Any(y => y.Equals(x.Index)) && x.PossibleNumbers.Contains(yWing.Pincer1ToPincer2))
                //////                             .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { yWing.Pincer1ToPincer2 })))
                //////                             .ToList());}
                //__newPossibilities = GetXYWings(allPossibilities)
                //        .SelectMany(yWing => Sudoku.LinkedCellsOf(yWing.Pincer1).Intersect(Sudoku.LinkedCellsOf(yWing.Pincer2)).Except(new[] { yWing.Pivot })
                //        .Select(x => allPossibilities.SingleOrDefault(y => y.Index.Equals(x)))
                //        .Where(x => x != null && x.PossibleNumbers.Contains(yWing.Pincer1ToPincer2))
                //        .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { yWing.Pincer1ToPincer2 }))))
                //        .ToList();
                //concatPossibilities();
                #endregion
                foreach (var yWing in GetXYWings(allPossibilities))
                {
                    __newPossibilities = Sudoku.LinkedCellsOf(yWing.Pincer1).Intersect(Sudoku.LinkedCellsOf(yWing.Pincer2)).Except(new[] { yWing.Pivot })
                                        .Select(x => allPossibilities.SingleOrDefault(y => y.Index.Equals(x)))
                                        .Where(x => x != null && x.PossibleNumbers.Contains(yWing.Pincer1ToPincer2))
                                        .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { yWing.Pincer1ToPincer2 }))).ToList();
                    concatPossibilities();
                    if (possChanged)
                        break;
                }
                if (possChanged)
                {
                    strategies.Add(SolvingStrategies.XYWing);
                    continue;
                }
                #endregion
                #region . 7 | Swordfish .
                #region . old .
                //foreach (var number in promisingNumbers)
                //{
                //    #region . Getting All Triples .
                //    triples = new List<Tuple<int, int[]>>();    // Item1: RowIndex, Item2: ColumnIndices
                //    for (i = 0; i < Sudoku.Length_Row; i++)
                //    {
                //        cells = curr[SubArrayType.Row, i]
                //                .Select((x, col) => new Possibilities
                //                                    (
                //                                        new IndicesPair(i, col),
                //                                        x.HasValue ? new List<IntSudoku>() { x.Value } :
                //                                                     new List<IntSudoku>(allPossibilities.Single(y => y.Index.Equals(new IndicesPair(i, col))).PossibleNumbers)
                //                                    )).OrderBy(x => x.Index.ColumnIndex).ToList();
                //        if ((count = cells.Count(x => x.PossibleNumbers.Contains(number))) == 2 || count == 3)
                //            continue;
                //        for (m = 0; m < Sudoku.Length_Column; m++)
                //        {
                //            a = ((t1 = cells[m]).PossibleNumbers.Count == 1 || !t1.PossibleNumbers.Contains(number) ? 1 : 0);
                //            for (j = m + 1; j < Sudoku.Length_Column; j++)
                //                if ((b = a + ((t2 = cells[j]).PossibleNumbers.Count == 1 || !t2.PossibleNumbers.Contains(number) ? 1 : 0)) <= 1)
                //                    for (k = j + 1; k < Sudoku.Length_Column; k++)
                //                        if ((b + ((t3 = cells[k]).PossibleNumbers.Count == 1 || !t3.PossibleNumbers.Contains(number) ? 1 : 0)) <= 1)
                //                            triples.Add(new Tuple<int, int[]>(i, (new[] { m, j, k }).ToArray()));
                //        }
                //        //foreach (var columns in combinations)
                //        //    if ((((t1 = cells[columns[0]]).PossibleNumbers.Count == 1 || !t1.PossibleNumbers.Contains(number) ? 1 : 0) +
                //        //         ((t2 = cells[columns[1]]).PossibleNumbers.Count == 1 || !t2.PossibleNumbers.Contains(number) ? 1 : 0) +
                //        //         ((t3 = cells[columns[2]]).PossibleNumbers.Count == 1 || !t3.PossibleNumbers.Contains(number) ? 1 : 0)) <= 1)
                //        //        triples.Add(new Tuple<int, int[]>(i, (new[] { t1.Index.ColumnIndex, t2.Index.ColumnIndex, t3.Index.ColumnIndex }).OrderBy(x => x).ToArray()));
                //    }
                //    if (triples.Count == 0)
                //        continue;
                //    #endregion
                //    #region . Deciding which ones are Swordfish .
                //    for (i = 0; i < triples.Count; i++)
                //    {
                //        iCells = triples[i];
                //        for (j = i + 1; j < triples.Count; j++)
                //        {
                //            jCells = triples[j];
                //            #region . Deciding whether cells are valid or not .
                //            if (iCells.Item1 == jCells.Item1 || !iCells.Item2.SequenceEqual(jCells.Item2))
                //                continue;
                //            #endregion
                //            for (k = j + 1; k < triples.Count; k++)
                //            {
                //                kCells = triples[k];
                //                #region . Deciding whether cells are valid or not .
                //                if (iCells.Item1 == kCells.Item1 || jCells.Item1 == kCells.Item1 || !jCells.Item2.SequenceEqual(kCells.Item2))
                //                    continue;
                //                __row1 = iCells.Item1;
                //                __row2 = jCells.Item1;
                //                __row3 = kCells.Item1;
                //                __column1 = iCells.Item2[0];
                //                __column2 = jCells.Item2[1];
                //                __column3 = kCells.Item2[2];
                //                __number = number;
                //                if (!areColumnsOK())
                //                    continue;
                //                #endregion
                //                indices = new[] { iCells.Item1, jCells.Item1, kCells.Item1 };
                //                __newPossibilities = allPossibilities.Where(x => x.PossibleNumbers.Contains(number) && (indices.Contains(x.Index.RowIndex) ?
                //                                                                                                        !iCells.Item2.Contains(x.Index.ColumnIndex) :
                //                                                                                                        iCells.Item2.Contains(x.Index.ColumnIndex)))
                //                                                     .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { number }).ToList()))
                //                                                     .ToList();
                //                concatPossibilities();
                //                if (possChanged)
                //                    break;
                //            }
                //            if (possChanged)
                //                break;
                //        }
                //        if (possChanged)
                //            break;
                //    }
                //    #endregion
                //    if (possChanged)
                //        break;
                //}
                #endregion
                foreach (var number in promisingNumbers)
                {
                    swordfishes = GetSwordfishes(curr, allPossibilities, number);
                    foreach (var swf in swordfishes)
                    {
                        __newPossibilities = new List<Possibilities>();
                        switch (swf.Direction)
                        {
                            case SwordfishDirection.ByRows:
                                foreach (var col in swf.ColumnsIndices)
                                    __newPossibilities.AddRange(allPossibilities
                                        .Where(x => x.Index.ColumnIndex == col
                                                 && !swf.RowsIndices.Contains(x.Index.RowIndex)
                                                 && x.PossibleNumbers.Contains(swf.Number))
                                        .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { swf.Number })))
                                        .ToList());
                                break;
                            case SwordfishDirection.ByColumns:
                                foreach (var row in swf.RowsIndices)
                                    __newPossibilities.AddRange(allPossibilities
                                        .Where(x => x.Index.RowIndex == row
                                                 && !swf.ColumnsIndices.Contains(x.Index.ColumnIndex)
                                                 && x.PossibleNumbers.Contains(swf.Number))
                                        .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { swf.Number })))
                                        .ToList());
                                break;
                        }
                        concatPossibilities();
                        if (possChanged)
                            break;
                    }
                    #region . old .
                    //foreach (var number in promisingNumbers)
                    //{
                    //    triples = GetValidSwordfishForms(curr, allPossibilities, number).ToArray();
                    //    if (triples.Length == 0)
                    //        continue;
                    //    for (m = 0, i = -1, j = -1, k = -1; m < 2; m++)
                    //    {
                    //        switch (m)
                    //        {
                    //            case 0:
                    //                for (i = 0; i < triples.Length; i++)
                    //                {
                    //                    for (j = i + 1; j < triples.Length; j++)
                    //                    {
                    //                        if (triples[i][0].Index.RowIndex != triples[j][0].Index.RowIndex)
                    //                            for (k = j + 1; k < triples.Length; k++)
                    //                            {
                    //                                if (Swordfish.TryCreateAValidInstance(out swf, number, allPossibilities, triples[i], triples[j], triples[k]))
                    //                                {
                    //                                    __newPossibilities = new List<Possibilities>();
                    //                                    foreach (var col in swf.ColumnsIndices)
                    //                                        __newPossibilities.AddRange(allPossibilities
                    //                                            .Where(x => x.Index.ColumnIndex == col
                    //                                                     && !swf.RowsIndices.Contains(x.Index.RowIndex)
                    //                                                     && x.PossibleNumbers.Contains(swf.Number))
                    //                                            .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { swf.Number })))
                    //                                            .ToList());
                    //                                }
                    //                                concatPossibilities();
                    //                                if (possChanged)
                    //                                    break;
                    //                            }
                    //                        if (possChanged)
                    //                            break;
                    //                    }
                    //                    if (possChanged)
                    //                        break;
                    //                }
                    //                break;
                    //            case 1:
                    //                for (i = 0; i < triples.Length; i++)
                    //                {
                    //                    for (j = i + 1; j < triples.Length; j++)
                    //                    {
                    //                        if (triples[i][0].Index.ColumnIndex != triples[j][0].Index.ColumnIndex)
                    //                            for (k = j + 1; k < triples.Length; k++)
                    //                            {
                    //                                if (Swordfish.TryCreateAValidInstance(out swf, number, allPossibilities, triples[i], triples[j], triples[k]))
                    //                                {
                    //                                    __newPossibilities = new List<Possibilities>();
                    //                                    foreach (var row in swf.RowsIndices)
                    //                                        __newPossibilities.AddRange(allPossibilities
                    //                                            .Where(x => x.Index.RowIndex == row
                    //                                                     && !swf.ColumnsIndices.Contains(x.Index.ColumnIndex)
                    //                                                     && x.PossibleNumbers.Contains(swf.Number))
                    //                                            .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { swf.Number })))
                    //                                            .ToList());
                    //                                }
                    //                                concatPossibilities();
                    //                                if (possChanged)
                    //                                    break;
                    //                            }
                    //                        if (possChanged)
                    //                            break;
                    //                    }
                    //                    if (possChanged)
                    //                        break;
                    //                }
                    //                break;
                    //        }
                    //        if (possChanged)
                    //            break;
                    //    }
                    //    if (possChanged)
                    //        break;
                    //}
                    #endregion
                    if (possChanged)
                        break;
                }
                if (possChanged)
                {
                    strategies.Add(SolvingStrategies.Swordfish);
                    continue;
                }
                #endregion
                #region . 8 | XYZ Wing .
                //__newPossibilities = GetXYZWings(allPossibilities)
                //    .SelectMany(zWing => Sudoku.LinkedCellsOf(zWing.Pincer1).Intersect(Sudoku.LinkedCellsOf(zWing.Pincer2)).Except(new[] { zWing.Pivot })
                //    .Select(x => allPossibilities.SingleOrDefault(y => y.Index.Equals(x)))
                //    .Where(x => x != null && x.PossibleNumbers.Contains(zWing.CommonNumber))
                //    .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { zWing.CommonNumber })))).ToList();
                //concatPossibilities();
                foreach (var zWing in GetXYZWings(allPossibilities))
                {
                    __newPossibilities = Sudoku.LinkedCellsOf(zWing.Pincer1).Intersect(Sudoku.LinkedCellsOf(zWing.Pincer2)).Except(new[] { zWing.Pivot })
                                        .Select(x => allPossibilities.SingleOrDefault(y => y.Index.Equals(x)))
                                        .Where(x => x != null && x.PossibleNumbers.Contains(zWing.CommonNumber))
                                        .Select(x => new Possibilities(x.Index, x.PossibleNumbers.Except(new[] { zWing.CommonNumber }))).ToList();
                    concatPossibilities();
                    if (possChanged)
                        break;
                }
                if (possChanged)
                {
                    strategies.Add(SolvingStrategies.XYZWing);
                    continue;
                }
                #endregion
                allForms = null;
            } while (possChanged && sol == null);
            #endregion
            #region . Return Value .
            if (sol != null)
                return sol;
            //if (!IsValid(curr))
            //    throw new InvalidMatrixException();
            return new SudokuSolution()
            {
                HasSolution = false,
                Matrix = matrix.Clone() as Sudoku,
                PossibilitiesOfEmptyCells = new ReadOnlyCollection<Possibilities>(allPossibilities),
                Solution = curr.Clone() as Sudoku,
                UsedStrategies = new ReadOnlyCollection<SolvingStrategies>(strategies)//.Distinct().ToList())
            };
            #endregion
        }
        public static bool TrySolve(Sudoku matrix, out Sudoku solution)
        {
            return _TrySolve(matrix, out solution, false, true);
        }
        public static IEnumerable<Sudoku> AllSolutions(Sudoku matrix, bool forceToSolve = true, bool trySolveAtFirst = true)
        {
            return forceToSolve ? _AllSolutions(matrix, trySolveAtFirst) : _AllSolutions(matrix, 1000, trySolveAtFirst);
        }
    }

    internal struct UInt4 : IEquatable<UInt4>, IComparable<UInt4>
    {
        public UInt4(int value)
        {
            if (value > MaxValue || value < MinValue)
                throw new ArgumentOutOfRangeException("value");
            _bits = ConvertToBitArray(value);
        }


        public const int MaxValue = 15;
        public const int MinValue = 0;

        private BitArray _bits;
        private BitArray Bits
        {
            get
            {
                if (_bits == null)
                    _bits = new BitArray(BitConverter.GetBytes(0));
                return _bits;
            }
            set
            {
                _bits = value == null ? new BitArray(BitConverter.GetBytes(0)) : value;
            }
        }



        private static BitArray ConvertToBitArray(int value)
        {
            //var temp = BitConverter.GetBytes(value);
            //return new BitArray((BitConverter.IsLittleEndian ? temp.Reverse() : temp).Take(1).ToArray());

            uint v = (uint)value;
            var allBytes = BitConverter.GetBytes(v);
            var oneByte = allBytes.Take(1).ToArray();
            var eightBits = new BitArray(oneByte).Cast<bool>();
            var fourBits = eightBits.Take(4).ToArray();
            var bits = new BitArray(fourBits);
            return bits;
        }
        public static int ConvertToInt32(UInt4 value)
        {
            int x = 0;
            for (int i = 0; i < value.Bits.Length; i++)
            {
                int first = value.Bits[i] ? 1 : 0, second = (int)Math.Pow(2, i);
                x += first * second;
            }
            return x;
        }

        public override int GetHashCode()
        {
            return ((int)this).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            if (obj is IntSudoku)
                return ((IntSudoku)((int)this)).Equals((IntSudoku)obj);
            else if (obj is UInt4)
                return Equals((UInt4)obj);
            else if (obj is int)
                return ((int)this).Equals((int)obj);
            else
                return base.Equals(obj);
        }
        public override string ToString()
        {
            return ((int)this).ToString();
        }
        public bool Equals(UInt4 other)
        {
            return ((int)this).Equals((int)other);
        }
        public int CompareTo(UInt4 other)
        {
            return ((int)this).CompareTo(this);
        }

        public static implicit operator UInt4(int value)
        {
            return new UInt4(value);
        }
        public static implicit operator int(UInt4 value)
        {
            return ConvertToInt32(value);
        }
        public static bool operator ==(UInt4 value1, UInt4 value2)
        {
            int length = 0;
            if ((length = value1.Bits.Length) != value2.Bits.Length)
                return false;
            bool equals = true;
            for (int i = 0; i < length; i++)
                if (!(equals = value1.Bits[i] == value2.Bits[i]))
                    break;
            return equals;
        }
        public static bool operator !=(UInt4 value1, UInt4 value2)
        {
            return !(value1 == value2);
        }
        public static bool operator <(UInt4 value1, UInt4 value2)
        {
            return (int)value1 < (int)value2;
        }
        public static bool operator >(UInt4 value1, UInt4 value2)
        {
            return (int)value1 > (int)value2;
        }
        public static int operator +(UInt4 value1, UInt4 value2)
        {
            return (int)value1 + (int)value2;
        }
        public static int operator -(UInt4 value1, UInt4 value2)
        {
            return (int)value1 - (int)value2;
        }
        public static int operator *(UInt4 value1, UInt4 value2)
        {
            return (int)value1 * (int)value2;
        }
        public static int operator /(UInt4 value1, UInt4 value2)
        {
            return (int)value1 / (int)value2;
        }
        public static int operator %(UInt4 value1, UInt4 value2)
        {
            return (int)value1 % (int)value2;
        }
        public static UInt4 operator >>(UInt4 value, int x)
        {
            if (x > 0 && x <= 4)
            {
                var ui = new UInt4();
                for (int i = 0; i < x; i++)
                    ui = value.ShiftRightOnce();
                return ui;
            }
            var bits = new BitArray(4);
            if (x <= 0)
                bits = new BitArray(value.Bits);
            else
                bits.SetAll(false);
            return new UInt4() { Bits = bits };
        }
        public static UInt4 operator <<(UInt4 value, int x)
        {
            if (x > 0 && x <= 4)
            {
                var ui = new UInt4();
                for (int i = 0; i < x; i++)
                    ui = value.ShiftLeftOnce();
                return ui;
            }
            var bits = new BitArray(4);
            if (x <= 0)
                bits = new BitArray(value.Bits);
            else
                bits.SetAll(false);
            return new UInt4() { Bits = bits };
        }

        private UInt4 ShiftRightOnce()
        {
            var bits = new BitArray(4);
            bits[0] = false;
            for (int i = 1; i < 4; i++)
                bits[i] = Bits[i - 1];
            return new UInt4() { Bits = bits };
        }
        private UInt4 ShiftLeftOnce()
        {
            var bits = new BitArray(4);
            bits[3] = false;
            for (int i = 3; i > 0; i--)
                bits[i - 1] = Bits[i];
            return new UInt4() { Bits = bits };
        }

        public static UInt4 Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            UInt4 result = 1;
            if (!TryParse(s, out result))
                throw new FormatException();
            return result;
        }
        public static bool TryParse(string s, out UInt4 result)
        {
            result = 0;
            byte i = 0;
            if (string.IsNullOrEmpty(s) || !byte.TryParse(s, out i) || i < MinValue || i > MaxValue)
                return false;
            result = i;
            return true;
        }
    }

    #endregion
}