using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sudoku9x9
{
    public static class Helper
    {
        public static bool IsFlagDefined(this Enum e)
        {
            decimal d;
            return !decimal.TryParse(e.ToString(), out d);
        }
        public static int GetPrime(int index)
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
        }
    }
    public struct IndicesPair : IEquatable<IndicesPair>
    {
        public IndicesPair(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }



        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }



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
    public class Possibilities : IEquatable<Possibilities>
    {
        private Possibilities()
        {
            Index = new IndicesPair();
            _possibleNumbers = null;
        }
        public Possibilities(IndicesPair index, IEnumerable<IntSudoku> possibleNumbers)
        {
            Index = index;
            _possibleNumbers = possibleNumbers.ToList();
        }
        public Possibilities(IndicesPair index, List<IntSudoku> possibleNumbers)
        {
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
            int hash1 = Index.GetHashCode();
            int hash2 = 0;
            for (int i = 0; i < _possibleNumbers.Count; i++)
                hash2 += Helper.GetPrime(i + 1) * _possibleNumbers[i].GetHashCode();
            return hash1 + Helper.GetPrime(0) * hash2;
        }
        public override bool Equals(object obj)
        {
            return obj == null || !(obj is Possibilities) ? this == obj : Equals(obj as Possibilities);
        }

        public bool Equals(Possibilities other)
        {
            return
                other != null &&
                Index.Equals(other.Index) &&
                _possibleNumbers.OrderBy(x => x).ToList().SequenceEqual(other._possibleNumbers.OrderBy(x => x).ToList());
        }
    }
    internal class PossibilitiesMulti : IEquatable<PossibilitiesMulti>
    {
        private PossibilitiesMulti()
        {
            _indices = null;
            _possibleNumbers = null;
        }
        public PossibilitiesMulti(IEnumerable<IndicesPair> indices, IEnumerable<IntSudoku> possibleNumbers)
        {
            _indices = indices.ToList();
            _possibleNumbers = possibleNumbers.ToList();
        }
        public PossibilitiesMulti(List<IndicesPair> indices, IEnumerable<IntSudoku> possibleNumbers)
        {
            _indices = indices;
            _possibleNumbers = possibleNumbers.ToList();
        }
        public PossibilitiesMulti(IEnumerable<IndicesPair> indices, List<IntSudoku> possibleNumbers)
        {
            _indices = indices.ToList();
            _possibleNumbers = possibleNumbers;
        }
        public PossibilitiesMulti(List<IndicesPair> indices, List<IntSudoku> possibleNumbers)
        {
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
            int hash1 = 0;
            int hash2 = 0;
            for (int i = 0; i < _indices.Count; i++)
                hash1 += Helper.GetPrime(i + 1) * _indices[i].GetHashCode();
            for (int i = 0; i < _possibleNumbers.Count; i++)
                hash2 += Helper.GetPrime(i + 1 + _indices.Count) * _possibleNumbers[i].GetHashCode();
            return hash1 + Helper.GetPrime(0) * hash2;
        }
        public override bool Equals(object obj)
        {
            return obj == null || !(obj is PossibilitiesMulti) ? this == obj : Equals(obj as PossibilitiesMulti);
        }

        public bool Equals(PossibilitiesMulti other)
        {
            return
                other != null &&
                _indices.OrderBy(x => x).ToList().SequenceEqual(other._indices.OrderBy(x => x).ToList()) &&
                _possibleNumbers.OrderBy(x => x).ToList().SequenceEqual(other._possibleNumbers.OrderBy(x => x).ToList());
        }
    }
    internal class ConjugateTupleForm : IEquatable<ConjugateTupleForm>, IComparable<ConjugateTupleForm>
    {
        private ConjugateTupleForm()
        {
            Index = new IndicesPair();
            Number = new IntSudoku();
            Flag = new byte();
        }
        public ConjugateTupleForm(IndicesPair index, IntSudoku number, byte flag)
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
            return obj == null || !(obj is ConjugateTupleForm) ? this == obj : Equals(obj as ConjugateTupleForm);
        }

        public bool Equals(ConjugateTupleForm other)
        {
            return other != null && Index.Equals(other.Index) && Number == other.Number && Flag == other.Flag;
        }

        public int CompareTo(ConjugateTupleForm other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            int a = 0, b = 0;
            return (a = Index.CompareTo(other.Index)) != 0 ? a : ((b = Number.CompareTo(other.Number)) != 0 ? b : Flag.CompareTo(other.Flag));
        }
    }
    internal class YWing : IEquatable<YWing>
    {
        private YWing()
        {
            Pivot = new IndicesPair();
            Pincer1 = new IndicesPair();
            Pincer2 = new IndicesPair();
            PivotToPincer1 = new IntSudoku();
            PivotToPincer2 = new IntSudoku();
            Pincer1ToPincer2 = new IntSudoku();
        }
        public YWing(IndicesPair pivot, IndicesPair pincer1, IndicesPair pincer2, IntSudoku pivotToPincer1, IntSudoku pivotToPincer2, IntSudoku pincer1ToPincer2)
        {
            if (pivot.Equals(pincer1) && pivot.Equals(pincer2) && pincer1.Equals(pincer2))
                throw new ArgumentException("Some/All of indices were the same.");
            Pivot = pivot;
            Pincer1 = pincer1;
            Pincer2 = pincer2;
            PivotToPincer1 = pivotToPincer1;
            PivotToPincer2 = pivotToPincer2;
            Pincer1ToPincer2 = pincer1ToPincer2;
        }


        public IndicesPair Pivot { get; private set; }
        public IndicesPair Pincer1 { get; private set; }
        public IndicesPair Pincer2 { get; private set; }
        public IntSudoku PivotToPincer1 { get; private set; }
        public IntSudoku PivotToPincer2 { get; private set; }
        public IntSudoku Pincer1ToPincer2 { get; private set; }



        public override string ToString()
        {
            return string.Format(
                "[{0}: {1}; {2}: {3}; {4}: {5}; {6}: {7}; {8}: {9}; {10}: {11}]",
                nameof(Pivot), Pivot,
                nameof(Pincer1), Pincer1,
                nameof(Pincer2), Pincer2,
                nameof(PivotToPincer1), PivotToPincer1,
                nameof(PivotToPincer2), PivotToPincer2,
                nameof(Pincer1ToPincer2), Pincer1ToPincer2);
        }
        public override int GetHashCode()
        {
            return
                     Pivot.GetHashCode() +
                 3 * Pincer1.GetHashCode() +
                 5 * Pincer2.GetHashCode() +
                 7 * PivotToPincer1.GetHashCode() +
                11 * PivotToPincer2.GetHashCode() +
                13 * PivotToPincer2.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return obj == null || !(obj is YWing) ? this == obj : Equals(obj as YWing);
        }

        public bool Equals(YWing other)
        {
            Func<YWing, bool> equals = (obj) =>
            {
                return
                obj != null &&
                Pivot.Equals(obj.Pivot) &&
                Pincer1.Equals(obj.Pincer1) &&
                Pincer2.Equals(obj.Pincer2) &&
                PivotToPincer1.Equals(obj.PivotToPincer1) &&
                PivotToPincer2.Equals(obj.PivotToPincer2) &&
                Pincer1ToPincer2.Equals(obj.Pincer1ToPincer2);
            };
            return
                equals(other) ||
                equals(new YWing(other.Pivot, other.Pincer2, other.Pincer1, other.PivotToPincer2, other.PivotToPincer1, other.Pincer1ToPincer2));
        }
    }
}