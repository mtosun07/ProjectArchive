using System;
using System.Collections;
using System.Collections.Generic;

namespace TSN.Utility.Extensions
{
    public static class PrimeNumber
    {
        static PrimeNumber()
        {
            _theFirstPrimes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113 };
        }


        public static readonly int[] _theFirstPrimes;

        public static int[] TheFirstPrimes
        {
            get { return _theFirstPrimes; }
        }



        private static int ApproximateNthPrime(int n)
        {
            // Source: https://stackoverflow.com/a/1072205
            if (n < 1)
                throw new ArgumentOutOfRangeException(nameof(n));
            double d = n;
            double prime = 0;
            if (n <= TheFirstPrimes.Length)
                return TheFirstPrimes[n - 1];
            if (n >= 7022)
            {
                prime = d * Math.Log(d) + d * (Math.Log(Math.Log(d)) - 0.9385);
            }
            else if (n >= 6)
            {
                prime = d * Math.Log(d) + d * Math.Log(Math.Log(d));
            }
            return (int)prime;
        }
        private static BitArray SieveOfEratosthenes(int limit)
        {
            BitArray bits = new BitArray(limit + 1, true);
            bits[0] = false;
            bits[1] = false;
            for (int i = 0, sq; (sq = i * i) <= limit; i++)
                if (bits[i])
                    for (int j = sq; j <= limit; j += i)
                        bits[j] = false;
            return bits;
        }
        public static IEnumerable<int> GeneratePrimesSieveOfEratosthenes(int n)
        {
            int limit = ApproximateNthPrime(n);
            var bits = SieveOfEratosthenes(limit);
            for (int i = 0, found = 0; i < limit && found < n; i++)
                if (bits[i])
                {
                    found++;
                    yield return i;
                }
        }
    }
}