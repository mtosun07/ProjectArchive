using System.Collections.Generic;

namespace TSN.AWebProject.Common.UtilitiesLibrary
{
    public static class CollectionExtensions
    {
        public static T[] MakeArray<T>(params T[] values)
        {
            var array = new T[values?.Length ?? 0];
            for (int i = 0; i < array.Length; i++)
                array[i] = values[i];
            return array;
        }
        public static void SwapItem<T>(this IList<T> collection, int index1, int index2)
        {
            var temp = collection[index1];
            collection[index1] = collection[index2];
            collection[index2] = temp;
        }
        public static void Shuffle<T>(this IList<T> collection)
        {
            for (int i = collection.Count - 1; i > 0; i--)
                collection.SwapItem(i, Shared.Random.Next(i + 1));
        }
    }
}