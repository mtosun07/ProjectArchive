namespace TSN.Hashing
{
    /// <summary>
    /// Encapsulates a method that serves a function to compare equalities of any two objects of type <typeparamref name="T"/>, which are to be specified as parameters.
    /// </summary>
    /// <typeparam name="T">Type of the objects that are to be compared.</typeparam>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>true if the specified objects are equal; otherwise, false.</returns>
    public delegate bool ComparisonDelegate<in T>(T x, T y);
}