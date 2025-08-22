using System;
using System.Collections.Generic;

namespace TSN.Hashing.FNV
{
    /// <summary>
    /// Provides a class which is of an implementation of the <see cref="IEqualityComparer{T}"/> for the purpose of calculation by the FNV algorithm.
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare and/or on which, hash code calculations to be made.</typeparam>
    public class FNV32EqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FNV32EqualityComparer{T}"/> class.
        /// </summary>
        private FNV32EqualityComparer()
        {
            _comparer = (x, y) => EqualityComparer<T>.Default.Equals(x, y);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FNV32EqualityComparer{T}"/> class with the specified parameter of type <see cref="ComparisonDelegate{T}"/> that serves a function to compare objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="comparer">A method to use to determine whether two objects of type <typeparamref name="T"/> are equal.</param>
        public FNV32EqualityComparer(ComparisonDelegate<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
        }

        
        private readonly ComparisonDelegate<T> _comparer;

        /// <summary>
        /// Returns a default equality comparer of <see cref="FNV32EqualityComparer{T}"/> for the type specified by the generic argument.
        /// </summary>
        public virtual FNV32EqualityComparer<T> Default => new FNV32EqualityComparer<T>();



        /// <summary>
        /// Determines whether two objects of type <typeparamref name="T"/> are equal by the argument of type <see cref="ComparisonDelegate{T}"/> if it specified while the type was initializing, otherwise, provided comparison method of the <see cref="EqualityComparer{T}.Default"/>.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }
        /// <summary>
        /// Serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table. Calculates the hash code by using the <see cref="HashingFNV32.GetHashCodeFNV32(object)"/>.
        /// </summary>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <returns>0 if the specified object is null, otherwise; a hash code which is a result of the <see cref="HashingFNV32.GetHashCodeFNV32(object)"/> for the object.</returns>
        public int GetHashCode(T obj)
        {
            return HashingFNV32.GetHashCodeFNV32(obj);
        }
    }
}