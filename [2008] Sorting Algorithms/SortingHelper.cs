using System;
using System.Collections.Generic;

namespace SortingAlgorithms
{
    public static class SortingHelper
    {
        private static void SwapElement<TElement>(IList<TElement> collection, int i, int j)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (i < 0 || j < 0 || i >= collection.Count || j >= collection.Count)
                throw new IndexOutOfRangeException();
            if (i == j)
                return;
            TElement temp = collection[i];
            collection[i] = collection[j];
            collection[j] = temp;
        }

        public static IEnumerable<TElement> Reverse<TElement>(IList<TElement> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            for (int i = collection.Count - 1; i >= 0; i--)
                yield return collection[i];
        }

        public static bool TryFindMinMax<TElement>(IList<TElement> collection, Comparison<TElement> comparer, out TElement min, out TElement max)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count > 0)
            {
                min = collection[0];
                max = collection[0];
                for (int i = 1; i < collection.Count; i++)
                {
                    if (comparer(collection[i], min) < 0)
                        min = collection[i];
                    if (comparer(collection[i], max) > 0)
                        max = collection[i];
                }
                return true;
            }
            min = default(TElement);
            max = default(TElement);
            return false;
        }
        public static bool TryFindMinMax<TElement>(IList<TElement> collection, IComparer<TElement> comparer, out TElement min, out TElement max)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            return TryFindMinMax(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(a, b);
            }, out min, out max);
        }
        public static bool TryFindMinMax<TElement>(IList<TElement> collection, out TElement min, out TElement max)
            where TElement : IComparable
        {
            return TryFindMinMax(collection, Comparer<TElement>.Default, out min, out max);
        }
        public static bool TryFindMinMax<TElement, TMapping>(IList<TElement> collection, Converter<TElement, TMapping> mapper, Comparison<TMapping> comparer, out TElement min, out TElement max)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (mapper == null)
                throw new ArgumentNullException("mapper");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count > 0)
            {
                min = collection[0];
                max = collection[0];
                if (collection.Count > 1)
                {
                    TMapping o = mapper(collection[0]);
                    TMapping min_ = o;
                    TMapping max_ = o;
                    for (int i = 1; i < collection.Count; i++)
                    {
                        o = mapper(collection[i]);
                        if (comparer(o, min_) < 0)
                            min = collection[i];
                        if (comparer(o, max_) > 0)
                            max = collection[i];
                    }
                }
                return true;
            }
            min = default(TElement);
            max = default(TElement);
            return false;
        }
        public static bool TryFindMinMax<TElement, TMapping>(IList<TElement> collection, Converter<TElement, TMapping> mapper, IComparer<TMapping> comparer, out TElement min, out TElement max)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            return TryFindMinMax(collection, mapper, delegate (TMapping a, TMapping b)
            {
                return comparer.Compare(a, b);
            }, out min, out max);
        }
        public static bool TryFindMinMax<TElement, TMapping>(IList<TElement> collection, Converter<TElement, TMapping> mapper, out TElement min, out TElement max)
            where TMapping : IComparable
        {
            return TryFindMinMax(collection, mapper, Comparer<TMapping>.Default, out min, out max);
        }

        public static void BubbleSort<TElement>(IList<TElement> collection, Comparison<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            int upperBound = collection.Count - 1;
            for (int i = 0; i < upperBound; i++)
                for (int j = 0; j < upperBound - i; j++)
                    if (comparer(collection[j], collection[j + 1]) > 0)
                        SwapElement(collection, j, j + 1);
        }
        public static void BubbleSortAscending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            BubbleSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(a, b);
            });
        }
        public static void BubbleSortDescending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            BubbleSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(b, a);
            });
        }
        public static void BubbleSortAscending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            BubbleSortAscending(collection, Comparer<TElement>.Default);
        }
        public static void BubbleSortDescending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            BubbleSortDescending(collection, Comparer<TElement>.Default);
        }

        public static void SelectionSort<TElement>(IList<TElement> collection, Comparison<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            int upperBound = collection.Count - 1;
            for (int i = 0; i < upperBound; i++)
            {
                int wantedIndex = i;
                for (int j = i + 1; j < collection.Count; j++)
                    if (comparer(collection[j], collection[wantedIndex]) < 0)
                        wantedIndex = j;
                SwapElement(collection, i, wantedIndex);
            }
        }
        public static void SelectionSortAscending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            SelectionSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(a, b);
            });
        }
        public static void SelectionSortDescending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            SelectionSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(b, a);
            });
        }
        public static void SelectionSortAscending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            SelectionSortAscending(collection, Comparer<TElement>.Default);
        }
        public static void SelectionSortDescending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            SelectionSortDescending(collection, Comparer<TElement>.Default);
        }

        public static void InsertionSort<TElement>(IList<TElement> collection, Comparison<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            for (int i = 1; i < collection.Count; i++)
            {
                TElement key = collection[i];
                int j;
                for (j = i - 1; j >= 0 && comparer(collection[j], key) > 0; j--)
                    collection[j + 1] = collection[j];
                collection[j + 1] = key;
            }
        }
        public static void InsertionSortAscending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            InsertionSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(a, b);
            });
        }
        public static void InsertionSortDescending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            InsertionSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(b, a);
            });
        }
        public static void InsertionSortAscending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            InsertionSortAscending(collection, Comparer<TElement>.Default);
        }
        public static void InsertionSortDescending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            InsertionSortDescending(collection, Comparer<TElement>.Default);
        }

        public static void ShellSort<TElement>(IList<TElement> collection, Comparison<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            for (int gap = collection.Count / 2; gap > 0; gap /= 2)
                for (int i = gap; i < collection.Count; i += 1)
                {
                    TElement temp = collection[i];
                    int j;
                    for (j = i; j >= gap && comparer(collection[j - gap], temp) > 0; j -= gap)
                        collection[j] = collection[j - gap];
                    collection[j] = temp;
                }
        }
        public static void ShellSortAscending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            ShellSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(a, b);
            });
        }
        public static void ShellSortDescending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            ShellSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(b, a);
            });
        }
        public static void ShellSortAscending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            ShellSortAscending(collection, Comparer<TElement>.Default);
        }
        public static void ShellSortDescending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            ShellSortDescending(collection, Comparer<TElement>.Default);
        }

        private static int Partition<TElement>(IList<TElement> collection, int low, int high, Comparison<TElement> comparer)
        {
            if (low < 0 || high < 0 || low >= collection.Count || high >= collection.Count)
                throw new IndexOutOfRangeException();
            TElement pivot = collection[high];
            int i = low - 1;
            for (int j = low; j <= high - 1; j++)
                if (comparer(collection[j], pivot) < 0)
                {
                    i++;
                    SwapElement(collection, i, j);
                }
            SwapElement(collection, i + 1, high);
            return i + 1;
        }
        public static void QuickSort<TElement>(IList<TElement> collection, int low, int high, Comparison<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (low >= high)
                return;
            int pivotIndex = Partition(collection, low, high, comparer);
            QuickSort(collection, low, pivotIndex - 1, comparer);
            QuickSort(collection, pivotIndex + 1, high, comparer);
        }
        public static void QuickSort<TElement>(IList<TElement> collection, Comparison<TElement> comparer)
        {
            if (collection.Count < 2)
                return;
            QuickSort(collection, 0, collection.Count - 1, comparer);
        }
        public static void QuickSortAscending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            QuickSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(a, b);
            });
        }
        public static void QuickSortDescending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            QuickSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(b, a);
            });
        }
        public static void QuickSortAscending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            QuickSortAscending(collection, Comparer<TElement>.Default);
        }
        public static void QuickSortDescending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            QuickSortDescending(collection, Comparer<TElement>.Default);
        }

        private static void Merge<TElement>(IList<TElement> collection, IList<TElement> temporaryCollection, int left, int middle, int right, Comparison<TElement> comparer)
        {
            int i = left;
            int j = middle + 1;
            int k = left;
            while (i <= middle && j <= right)
                if (comparer(collection[i], collection[j]) > 0)
                    temporaryCollection[k++] = collection[j++];
                else
                    temporaryCollection[k++] = collection[i++];
            while (i <= middle)
                temporaryCollection[k++] = collection[i++];
            while (j <= right)
                temporaryCollection[k++] = collection[j++];
            for (i = left; i <= right; i++)
                collection[i] = temporaryCollection[i];
        }
        private static void MergeSort<TElement>(IList<TElement> collection, IList<TElement> temporaryCollection, int left, int right, Comparison<TElement> comparer)
        {
            if (left >= right)
                return;
            int middle = (left + right) / 2;
            MergeSort(collection, temporaryCollection, left, middle, comparer);
            MergeSort(collection, temporaryCollection, middle + 1, right, comparer);
            Merge(collection, temporaryCollection, left, middle, right, comparer);
        }
        public static void MergeSort<TElement>(IList<TElement> collection, Comparison<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            TElement[] temporaryCollection = new TElement[collection.Count];
            MergeSort(collection, temporaryCollection, 0, collection.Count - 1, comparer);
        }
        public static void MergeSortAscending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            MergeSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(a, b);
            });
        }
        public static void MergeSortDescending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            MergeSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(b, a);
            });
        }
        public static void MergeSortAscending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            MergeSortAscending(collection, Comparer<TElement>.Default);
        }
        public static void MergeSortDescending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            MergeSortDescending(collection, Comparer<TElement>.Default);
        }

        private static void Heapify<TElement>(IList<TElement> collection, int count, int index, Comparison<TElement> comparer)
        {
            int maxIndex = index;
            int low = 2 * index + 1;
            int high = 2 * index + 2;
            if (low < count && comparer(collection[low], collection[maxIndex]) > 0)
                maxIndex = low;
            if (high < count && comparer(collection[high], collection[maxIndex]) > 0)
                maxIndex = high;
            if (maxIndex != index)
            {
                SwapElement(collection, index, maxIndex);
                Heapify(collection, count, maxIndex, comparer);
            }
        }
        public static void HeapSort<TElement>(IList<TElement> collection, Comparison<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            if (collection == null)
                throw new ArgumentNullException("collection");
            for (int i = collection.Count / 2 - 1; i >= 0; i--)
                Heapify(collection, collection.Count, i, comparer);
            for (int i = collection.Count - 1; i > 0; i--)
            {
                SwapElement(collection, 0, i);
                Heapify(collection, i, 0, comparer);
            }
        }
        public static void HeapSortAscending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            HeapSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(a, b);
            });
        }
        public static void HeapSortDescending<TElement>(IList<TElement> collection, IComparer<TElement> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            HeapSort(collection, delegate (TElement a, TElement b)
            {
                return comparer.Compare(b, a);
            });
        }
        public static void HeapSortAscending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            HeapSortAscending(collection, Comparer<TElement>.Default);
        }
        public static void HeapSortDescending<TElement>(IList<TElement> collection)
            where TElement : IComparable<TElement>
        {
            HeapSortDescending(collection, Comparer<TElement>.Default);
        }

        private static int GetBucketIndex(double item, double min, double max, int count)
        {
            double range = max - min;
            if (range == 0)
                return 0;
            return (int)((item - min) / range * (count - 1));
        }
        private static int GetBucketIndex(decimal item, decimal min, decimal max, int count)
        {
            decimal range = max - min;
            if (range == 0)
                return 0;
            return (int)((item - min) / range * (count - 1));
        }
        private static void InsertToBuckets<TElement>(IDictionary<int, List<TElement>> buckets, int bucketIndex, int itemIndex, IList<TElement> collection)
        {
            if (bucketIndex >= collection.Count)
                bucketIndex = collection.Count - 1;
            List<TElement> bucket;
            if (buckets.TryGetValue(bucketIndex, out bucket))
                bucket.Add(collection[itemIndex]);
            else
            {
                bucket = new List<TElement>();
                bucket.Add(collection[itemIndex]);
                buckets.Add(bucketIndex, bucket);
            }
        }
        private static IEnumerable<TElement> SortBuckets<TElement, U>(int collectionCount, IDictionary<int, List<TElement>> buckets, Converter<TElement, U> mapper, bool isAscending)
            where U : IComparable
        {
            List<TElement> bucket;
            if (isAscending)
            {
                Comparison<TElement> comparer = delegate (TElement a, TElement b)
                {
                    return mapper(a).CompareTo(mapper(b));
                };
                for (int i = 0; i < collectionCount; i++)
                {
                    if (!buckets.TryGetValue(i, out bucket))
                        continue;
                    InsertionSort(bucket, comparer);
                    for (int j = 0; j < bucket.Count; j++)
                        yield return bucket[j];
                }
            }
            else
            {
                Comparison<TElement> comparer = delegate (TElement a, TElement b)
                {
                    return mapper(b).CompareTo(mapper(a));
                };
                for (int i = collectionCount - 1; i >= 0; i--)
                {
                    if (!buckets.TryGetValue(i, out bucket))
                        continue;
                    InsertionSort(bucket, comparer);
                    for (int j = 0; j < bucket.Count; j++)
                        yield return bucket[j];
                }
            }
        }
        private static IEnumerable<TElement> BucketSort<TElement>(IList<TElement> collection, Converter<TElement, double> mapper, bool isAscending)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return collection;
            double min = mapper(collection[0]), max = min;
            for (int i = 1; i < collection.Count; i++)
            {
                double d = mapper(collection[i]);
                if (d < min)
                    min = d;
                if (d > max)
                    max = d;
            }
            Dictionary<int, List<TElement>> buckets = new Dictionary<int, List<TElement>>();
            for (int i = 0; i < collection.Count; i++)
                InsertToBuckets(buckets, GetBucketIndex(mapper(collection[i]), min, max, collection.Count), i, collection);
            return SortBuckets(collection.Count, buckets, mapper, isAscending);
        }
        private static IEnumerable<TElement> BucketSort<TElement>(IList<TElement> collection, Converter<TElement, decimal> mapper, bool isAscending)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count == 0)
                return collection;
            decimal min = mapper(collection[0]), max = min;
            for (int i = 1; i < collection.Count; i++)
            {
                decimal d = mapper(collection[i]);
                if (d < min)
                    min = d;
                if (d > max)
                    max = d;
            }
            Dictionary<int, List<TElement>> buckets = new Dictionary<int, List<TElement>>();
            for (int i = 0; i < collection.Count; i++)
                InsertToBuckets(buckets, GetBucketIndex(mapper(collection[i]), min, max, collection.Count), i, collection);
            return SortBuckets(collection.Count, buckets, mapper, isAscending);
        }
        public static IEnumerable<TElement> BucketSortAscending<TElement>(IList<TElement> collection, Converter<TElement, double> mapper)
            where TElement : IComparable<TElement>
        {
            return BucketSort(collection, mapper, true);
        }
        public static IEnumerable<TElement> BucketSortAscending<TElement>(IList<TElement> collection, Converter<TElement, decimal> mapper)
            where TElement : IComparable<TElement>
        {
            return BucketSort(collection, mapper, true);
        }
        public static IEnumerable<float> BucketSortAscending(IList<float> collection)
        {
            return BucketSort(
                collection,
                delegate (float obj)
                {
                    return (double)obj;
                },
                true);
        }
        public static IEnumerable<double> BucketSortAscending(IList<double> collection)
        {
            return BucketSort(
                collection,
                delegate (double obj)
                {
                    return obj;
                },
                true);
        }
        public static IEnumerable<decimal> BucketSortAscending(IList<decimal> collection)
        {
            return BucketSort(
                collection,
                delegate (decimal obj)
                {
                    return obj;
                },
                true);
        }
        public static IEnumerable<sbyte> BucketSortAscending(IList<sbyte> collection)
        {
            return BucketSort(
                collection,
                delegate (sbyte obj)
                {
                    return (double)obj;
                },
                true);
        }
        public static IEnumerable<byte> BucketSortAscending(IList<byte> collection)
        {
            return BucketSort(
                collection,
                delegate (byte obj)
                {
                    return (double)obj;
                },
                true);
        }
        public static IEnumerable<short> BucketSortAscending(IList<short> collection)
        {
            return BucketSort(
                collection,
                delegate (short obj)
                {
                    return (double)obj;
                },
                true);
        }
        public static IEnumerable<ushort> BucketSortAscending(IList<ushort> collection)
        {
            return BucketSort(
                collection,
                delegate (ushort obj)
                {
                    return (double)obj;
                },
                true);
        }
        public static IEnumerable<int> BucketSortAscending(IList<int> collection)
        {
            return BucketSort(
                collection,
                delegate (int obj)
                {
                    return (double)obj;
                },
                true);
        }
        public static IEnumerable<uint> BucketSortAscending(IList<uint> collection)
        {
            return BucketSort(
                collection,
                delegate (uint obj)
                {
                    return (double)obj;
                },
                true);
        }
        public static IEnumerable<long> BucketSortAscending(IList<long> collection)
        {
            return BucketSort(
                collection,
                delegate (long obj)
                {
                    return (double)obj;
                },
                true);
        }
        public static IEnumerable<ulong> BucketSortAscending(IList<ulong> collection)
        {
            return BucketSort(
                collection,
                delegate (ulong obj)
                {
                    return (double)obj;
                },
                true);
        }
        public static IEnumerable<TElement> BucketSortDescending<TElement>(IList<TElement> collection, Converter<TElement, double> mapper)
            where TElement : IComparable<TElement>
        {
            return BucketSort(collection, mapper, false);
        }
        public static IEnumerable<TElement> BucketSortDescending<TElement>(IList<TElement> collection, Converter<TElement, decimal> mapper)
            where TElement : IComparable<TElement>
        {
            return BucketSort(collection, mapper, false);
        }
        public static IEnumerable<float> BucketSortDescending(IList<float> collection)
        {
            return BucketSort(
                collection,
                delegate (float obj)
                {
                    return (double)obj;
                },
                false);
        }
        public static IEnumerable<double> BucketSortDescending(IList<double> collection)
        {
            return BucketSort(
                collection,
                delegate (double obj)
                {
                    return obj;
                },
                false);
        }
        public static IEnumerable<decimal> BucketSortDescending(IList<decimal> collection)
        {
            return BucketSort(
                collection,
                delegate (decimal obj)
                {
                    return obj;
                },
                false);
        }
        public static IEnumerable<sbyte> BucketSortDescending(IList<sbyte> collection)
        {
            return BucketSort(
                collection,
                delegate (sbyte obj)
                {
                    return (double)obj;
                },
                false);
        }
        public static IEnumerable<byte> BucketSortDescending(IList<byte> collection)
        {
            return BucketSort(
                collection,
                delegate (byte obj)
                {
                    return (double)obj;
                },
                false);
        }
        public static IEnumerable<short> BucketSortDescending(IList<short> collection)
        {
            return BucketSort(
                collection,
                delegate (short obj)
                {
                    return (double)obj;
                },
                false);
        }
        public static IEnumerable<ushort> BucketSortDescending(IList<ushort> collection)
        {
            return BucketSort(
                collection,
                delegate (ushort obj)
                {
                    return (double)obj;
                },
                false);
        }
        public static IEnumerable<int> BucketSortDescending(IList<int> collection)
        {
            return BucketSort(
                collection,
                delegate (int obj)
                {
                    return (double)obj;
                },
                false);
        }
        public static IEnumerable<uint> BucketSortDescending(IList<uint> collection)
        {
            return BucketSort(
                collection,
                delegate (uint obj)
                {
                    return (double)obj;
                },
                false);
        }
        public static IEnumerable<long> BucketSortDescending(IList<long> collection)
        {
            return BucketSort(
                collection,
                delegate (long obj)
                {
                    return (double)obj;
                },
                false);
        }
        public static IEnumerable<ulong> BucketSortDescending(IList<ulong> collection)
        {
            return BucketSort(
                collection,
                delegate (ulong obj)
                {
                    return (double)obj;
                },
                false);
        }

        private static void CountingSort(IList<sbyte> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            sbyte min, max;
            TryFindMinMax(collection, out min, out max);
            int range = max - min + 1;
            int[] count = new int[range];
            for (int i = 0; i < collection.Count; i++)
                count[collection[i] - min]++;
            sbyte[] output = new sbyte[collection.Count];
            if (isAscending)
            {
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = collection.Count - 1; i >= 0; i--)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            else
            {
                for (int i = count.Length - 2; i >= 0; i--)
                    count[i] += count[i + 1];
                for (int i = 0; i < collection.Count; i++)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            for (int i = 0; i < collection.Count; i++)
                collection[i] = output[i];
        }
        private static void CountingSort(IList<byte> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            byte min, max;
            TryFindMinMax(collection, out min, out max);
            int range = max - min + 1;
            int[] count = new int[range];
            for (int i = 0; i < collection.Count; i++)
                count[collection[i] - min]++;
            byte[] output = new byte[collection.Count];
            if (isAscending)
            {
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = collection.Count - 1; i >= 0; i--)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            else
            {
                for (int i = count.Length - 2; i >= 0; i--)
                    count[i] += count[i + 1];
                for (int i = 0; i < collection.Count; i++)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            for (int i = 0; i < collection.Count; i++)
                collection[i] = output[i];
        }
        private static void CountingSort(IList<short> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            short min, max;
            TryFindMinMax(collection, out min, out max);
            int range = max - min + 1;
            int[] count = new int[range];
            for (int i = 0; i < collection.Count; i++)
                count[collection[i] - min]++;
            short[] output = new short[collection.Count];
            if (isAscending)
            {
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = collection.Count - 1; i >= 0; i--)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            else
            {
                for (int i = count.Length - 2; i >= 0; i--)
                    count[i] += count[i + 1];
                for (int i = 0; i < collection.Count; i++)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            for (int i = 0; i < collection.Count; i++)
                collection[i] = output[i];
        }
        private static void CountingSort(IList<ushort> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            ushort min, max;
            TryFindMinMax(collection, out min, out max);
            int range = max - min + 1;
            int[] count = new int[range];
            for (int i = 0; i < collection.Count; i++)
                count[collection[i] - min]++;
            ushort[] output = new ushort[collection.Count];
            if (isAscending)
            {
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = collection.Count - 1; i >= 0; i--)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            else
            {
                for (int i = count.Length - 2; i >= 0; i--)
                    count[i] += count[i + 1];
                for (int i = 0; i < collection.Count; i++)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            for (int i = 0; i < collection.Count; i++)
                collection[i] = output[i];
        }
        private static void CountingSort(IList<int> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            int min, max;
            TryFindMinMax(collection, out min, out max);
            int range = max - min + 1;
            int[] count = new int[range];
            for (int i = 0; i < collection.Count; i++)
                count[collection[i] - min]++;
            int[] output = new int[collection.Count];
            if (isAscending)
            {
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = collection.Count - 1; i >= 0; i--)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            else
            {
                for (int i = count.Length - 2; i >= 0; i--)
                    count[i] += count[i + 1];
                for (int i = 0; i < collection.Count; i++)
                {
                    int x = collection[i] - min;
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            for (int i = 0; i < collection.Count; i++)
                collection[i] = output[i];
        }
        private static void CountingSort(IList<uint> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            uint min, max;
            TryFindMinMax(collection, out min, out max);
            uint range = max - min + 1;
            if (range > int.MaxValue)
                throw new InvalidOperationException();
            int[] count = new int[(int)range];
            for (int i = 0; i < collection.Count; i++)
                count[(int)(collection[i] - min)]++;
            uint[] output = new uint[collection.Count];
            if (isAscending)
            {
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = collection.Count - 1; i >= 0; i--)
                {
                    int x = (int)(collection[i] - min);
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            else
            {
                for (int i = count.Length - 2; i >= 0; i--)
                    count[i] += count[i + 1];
                for (int i = 0; i < collection.Count; i++)
                {
                    int x = (int)(collection[i] - min);
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            for (int i = 0; i < collection.Count; i++)
                collection[i] = output[i];
        }
        private static void CountingSort(IList<long> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            long min, max;
            TryFindMinMax(collection, out min, out max);
            long range = max - min + 1;
            if (range > int.MaxValue)
                throw new InvalidOperationException();
            int[] count = new int[(int)range];
            for (int i = 0; i < collection.Count; i++)
                count[(int)(collection[i] - min)]++;
            long[] output = new long[collection.Count];
            if (isAscending)
            {
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = collection.Count - 1; i >= 0; i--)
                {
                    int x = (int)(collection[i] - min);
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            else
            {
                for (int i = count.Length - 2; i >= 0; i--)
                    count[i] += count[i + 1];
                for (int i = 0; i < collection.Count; i++)
                {
                    int x = (int)(collection[i] - min);
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            for (int i = 0; i < collection.Count; i++)
                collection[i] = output[i];
        }
        private static void CountingSort(IList<ulong> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
                return;
            ulong min, max;
            TryFindMinMax(collection, out min, out max);
            ulong range = max - min + 1;
            if (range > int.MaxValue)
                throw new InvalidOperationException();
            int[] count = new int[(int)range];
            for (int i = 0; i < collection.Count; i++)
                count[(int)(collection[i] - min)]++;
            ulong[] output = new ulong[collection.Count];
            if (isAscending)
            {
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = collection.Count - 1; i >= 0; i--)
                {
                    int x = (int)(collection[i] - min);
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            else
            {
                for (int i = count.Length - 2; i >= 0; i--)
                    count[i] += count[i + 1];
                for (int i = 0; i < collection.Count; i++)
                {
                    int x = (int)(collection[i] - min);
                    output[count[x] - 1] = collection[i];
                    count[x]--;
                }
            }
            for (int i = 0; i < collection.Count; i++)
                collection[i] = output[i];
        }
        public static void CountingSortAscending(IList<sbyte> collection)
        {
            CountingSort(collection, true);
        }
        public static void CountingSortAscending(IList<byte> collection)
        {
            CountingSort(collection, true);
        }
        public static void CountingSortAscending(IList<short> collection)
        {
            CountingSort(collection, true);
        }
        public static void CountingSortAscending(IList<ushort> collection)
        {
            CountingSort(collection, true);
        }
        public static void CountingSortAscending(IList<int> collection)
        {
            CountingSort(collection, true);
        }
        public static void CountingSortAscending(IList<uint> collection)
        {
            CountingSort(collection, true);
        }
        public static void CountingSortAscending(IList<long> collection)
        {
            CountingSort(collection, true);
        }
        public static void CountingSortAscending(IList<ulong> collection)
        {
            CountingSort(collection, true);
        }
        public static void CountingSortDescending(IList<sbyte> collection)
        {
            CountingSort(collection, false);
        }
        public static void CountingSortDescending(IList<byte> collection)
        {
            CountingSort(collection, false);
        }
        public static void CountingSortDescending(IList<short> collection)
        {
            CountingSort(collection, false);
        }
        public static void CountingSortDescending(IList<ushort> collection)
        {
            CountingSort(collection, false);
        }
        public static void CountingSortDescending(IList<int> collection)
        {
            CountingSort(collection, false);
        }
        public static void CountingSortDescending(IList<uint> collection)
        {
            CountingSort(collection, false);
        }
        public static void CountingSortDescending(IList<long> collection)
        {
            CountingSort(collection, false);
        }
        public static void CountingSortDescending(IList<ulong> collection)
        {
            CountingSort(collection, false);
        }

        private static ulong ToUnsignedAbs(long value)
        {
            return value < 0 ? (ulong)(~value + 1L) : (ulong)value;
        }
        private static IList<byte> CountingSortByDigit(IList<byte> collection, byte exp)
        {
            byte[] output = new byte[collection.Count];
            int[] count = new int[10];
            for (int i = 0; i < collection.Count; i++)
                count[collection[i] / exp % 10]++;
            for (int i = 1; i < 10; i++)
                count[i] += count[i - 1];
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                int digit = collection[i] / exp % 10;
                output[count[digit] - 1] = collection[i];
                count[digit]--;
            }
            return output;
        }
        private static IEnumerable<byte> RadixSort(IList<byte> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
            {
                foreach (byte item in collection)
                    yield return item;
                yield break;
            }
            byte max = collection[0];
            for (int i = 1; i < collection.Count; i++)
                if (collection[i] > max)
                    max = collection[i];
            IList<byte> sorted = new List<byte>(collection);
            for (byte exp = 1; max / exp > 0; exp *= 10)
                sorted = CountingSortByDigit(sorted, exp);
            if (isAscending)
                foreach (byte item in sorted)
                    yield return item;
            else
                foreach (byte item in Reverse(sorted))
                    yield return item;
        }
        private static IEnumerable<sbyte> RadixSort(IList<sbyte> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
            {
                foreach (sbyte item in collection)
                    yield return item;
                yield break;
            }
            List<byte> negatives = new List<byte>();
            List<byte> positives = new List<byte>();
            foreach (sbyte item in collection)
            {
                if (item < 0L)
                    negatives.Add((byte)-item);
                else
                    positives.Add((byte)item);
            }
            if (isAscending)
            {
                if (negatives.Count > 0)
                    foreach (byte item in RadixSort(negatives, false))
                        yield return (sbyte)-item;
                if (positives.Count > 0)
                    foreach (byte item in RadixSort(positives, true))
                        yield return (sbyte)item;
            }
            else
            {
                if (positives.Count > 0)
                    foreach (byte item in RadixSort(positives, false))
                        yield return (sbyte)item;
                if (negatives.Count > 0)
                    foreach (byte item in RadixSort(negatives, true))
                        yield return (sbyte)-item;
            }
        }
        private static IList<ushort> CountingSortByDigit(IList<ushort> collection, ushort exp)
        {
            ushort[] output = new ushort[collection.Count];
            int[] count = new int[10];
            for (int i = 0; i < collection.Count; i++)
                count[collection[i] / exp % 10]++;
            for (int i = 1; i < 10; i++)
                count[i] += count[i - 1];
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                int digit = collection[i] / exp % 10;
                output[count[digit] - 1] = collection[i];
                count[digit]--;
            }
            return output;
        }
        private static IEnumerable<ushort> RadixSort(IList<ushort> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
            {
                foreach (ushort item in collection)
                    yield return item;
                yield break;
            }
            ushort max = collection[0];
            for (int i = 1; i < collection.Count; i++)
                if (collection[i] > max)
                    max = collection[i];
            IList<ushort> sorted = new List<ushort>(collection);
            for (ushort exp = 1; max / exp > 0; exp *= 10)
                sorted = CountingSortByDigit(sorted, exp);
            if (isAscending)
                foreach (ushort item in sorted)
                    yield return item;
            else
                foreach (ushort item in Reverse(sorted))
                    yield return item;
        }
        private static IEnumerable<short> RadixSort(IList<short> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
            {
                foreach (short item in collection)
                    yield return item;
                yield break;
            }
            List<ushort> negatives = new List<ushort>();
            List<ushort> positives = new List<ushort>();
            foreach (short item in collection)
            {
                if (item < 0L)
                    negatives.Add((ushort)-item);
                else
                    positives.Add((ushort)item);
            }
            if (isAscending)
            {
                if (negatives.Count > 0)
                    foreach (ushort item in RadixSort(negatives, false))
                        yield return (short)-item;
                if (positives.Count > 0)
                    foreach (ushort item in RadixSort(positives, true))
                        yield return (short)item;
            }
            else
            {
                if (positives.Count > 0)
                    foreach (ushort item in RadixSort(positives, false))
                        yield return (short)item;
                if (negatives.Count > 0)
                    foreach (ushort item in RadixSort(negatives, true))
                        yield return (short)-item;
            }
        }
        private static IList<uint> CountingSortByDigit(IList<uint> collection, uint exp)
        {
            uint[] output = new uint[collection.Count];
            int[] count = new int[10];
            for (int i = 0; i < collection.Count; i++)
                count[(int)(collection[i] / exp % 10U)]++;
            for (int i = 1; i < 10; i++)
                count[i] += count[i - 1];
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                int digit = (int)(collection[i] / exp % 10U);
                output[count[digit] - 1] = collection[i];
                count[digit]--;
            }
            return output;
        }
        private static IEnumerable<uint> RadixSort(IList<uint> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
            {
                foreach (uint item in collection)
                    yield return item;
                yield break;
            }
            uint max = collection[0];
            for (int i = 1; i < collection.Count; i++)
                if (collection[i] > max)
                    max = collection[i];
            IList<uint> sorted = new List<uint>(collection);
            for (uint exp = 1U; max / exp > 0U; exp *= 10U)
                sorted = CountingSortByDigit(sorted, exp);
            if (isAscending)
                foreach (uint item in sorted)
                    yield return item;
            else
                foreach (uint item in Reverse(sorted))
                    yield return item;
        }
        private static IEnumerable<int> RadixSort(IList<int> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
            {
                foreach (int item in collection)
                    yield return item;
                yield break;
            }
            List<uint> negatives = new List<uint>();
            List<uint> positives = new List<uint>();
            foreach (int item in collection)
            {
                if (item < 0L)
                    negatives.Add((uint)ToUnsignedAbs(item));
                else
                    positives.Add((uint)item);
            }
            if (isAscending)
            {
                if (negatives.Count > 0)
                    foreach (uint item in RadixSort(negatives, false))
                        yield return -(int)item;
                if (positives.Count > 0)
                    foreach (uint item in RadixSort(positives, true))
                        yield return (int)item;
            }
            else
            {
                if (positives.Count > 0)
                    foreach (uint item in RadixSort(positives, false))
                        yield return (int)item;
                if (negatives.Count > 0)
                    foreach (uint item in RadixSort(negatives, true))
                        yield return -(int)item;
            }
        }
        private static IList<ulong> CountingSortByDigit(IList<ulong> collection, ulong exp)
        {
            ulong[] output = new ulong[collection.Count];
            int[] count = new int[10];
            for (int i = 0; i < collection.Count; i++)
                count[(int)(collection[i] / exp % 10UL)]++;
            for (int i = 1; i < 10; i++)
                count[i] += count[i - 1];
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                int digit = (int)(collection[i] / exp % 10UL);
                output[count[digit] - 1] = collection[i];
                count[digit]--;
            }
            return output;
        }
        private static IEnumerable<ulong> RadixSort(IList<ulong> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
            {
                foreach (ulong item in collection)
                    yield return item;
                yield break;
            }
            ulong max = collection[0];
            for (int i = 1; i < collection.Count; i++)
                if (collection[i] > max)
                    max = collection[i];
            IList<ulong> sorted = new List<ulong>(collection);
            for (ulong exp = 1UL; max / exp > 0UL; exp *= 10UL)
                sorted = CountingSortByDigit(sorted, exp);
            if (isAscending)
                foreach (ulong item in sorted)
                    yield return item;
            else
                foreach (ulong item in Reverse(sorted))
                    yield return item;
        }
        private static IEnumerable<long> RadixSort(IList<long> collection, bool isAscending)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (collection.Count < 2)
            {
                foreach (long item in collection)
                    yield return item;
                yield break;
            }
            List<ulong> negatives = new List<ulong>();
            List<ulong> positives = new List<ulong>();
            foreach (long item in collection)
            {
                if (item < 0L)
                    negatives.Add(ToUnsignedAbs(item));
                else
                    positives.Add((ulong)item);
            }
            if (isAscending)
            {
                if (negatives.Count > 0)
                    foreach (ulong item in RadixSort(negatives, false))
                        yield return -(long)item;
                if (positives.Count > 0)
                    foreach (ulong item in RadixSort(positives, true))
                        yield return (long)item;
            }
            else
            {
                if (positives.Count > 0)
                    foreach (ulong item in RadixSort(positives, false))
                        yield return (long)item;
                if (negatives.Count > 0)
                    foreach (ulong item in RadixSort(negatives, true))
                        yield return -(long)item;
            }
        }
        public static IEnumerable<byte> RadixSortAscending(IList<byte> collection)
        {
            return RadixSort(collection, true);
        }
        public static IEnumerable<sbyte> RadixSortAscending(IList<sbyte> collection)
        {
            return RadixSort(collection, true);
        }
        public static IEnumerable<ushort> RadixSortAscending(IList<ushort> collection)
        {
            return RadixSort(collection, true);
        }
        public static IEnumerable<short> RadixSortAscending(IList<short> collection)
        {
            return RadixSort(collection, true);
        }
        public static IEnumerable<uint> RadixSortAscending(IList<uint> collection)
        {
            return RadixSort(collection, true);
        }
        public static IEnumerable<int> RadixSortAscending(IList<int> collection)
        {
            return RadixSort(collection, true);
        }
        public static IEnumerable<ulong> RadixSortAscending(IList<ulong> collection)
        {
            return RadixSort(collection, true);
        }
        public static IEnumerable<long> RadixSortAscending(IList<long> collection)
        {
            return RadixSort(collection, true);
        }
        public static IEnumerable<byte> RadixSortDescending(IList<byte> collection)
        {
            return RadixSort(collection, false);
        }
        public static IEnumerable<sbyte> RadixSortDescending(IList<sbyte> collection)
        {
            return RadixSort(collection, false);
        }
        public static IEnumerable<ushort> RadixSortDescending(IList<ushort> collection)
        {
            return RadixSort(collection, false);
        }
        public static IEnumerable<short> RadixSortDescending(IList<short> collection)
        {
            return RadixSort(collection, false);
        }
        public static IEnumerable<uint> RadixSortDescending(IList<uint> collection)
        {
            return RadixSort(collection, false);
        }
        public static IEnumerable<int> RadixSortDescending(IList<int> collection)
        {
            return RadixSort(collection, false);
        }
        public static IEnumerable<ulong> RadixSortDescending(IList<ulong> collection)
        {
            return RadixSort(collection, false);
        }
        public static IEnumerable<long> RadixSortDescending(IList<long> collection)
        {
            return RadixSort(collection, false);
        }
    }
}