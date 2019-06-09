using System;
using System.Collections.Generic;

namespace CsharpHelpers.Helpers
{

    public static class CollectionHelper
    {

        public static T GetFirstItem<T>(IEnumerable<T> collection)
        {
            var enumerator = collection.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new ArgumentException("Empty collection.", nameof(collection));

            return enumerator.Current;
        }


        /// <summary>
        /// Similar behavior of BinarySearch from dotnet. The collection must be sorted.
        /// </summary>
        public static int BinarySearch<T>(IList<T> collection, T item) where T : IComparable<T>
        {
            var startIndex = 0;
            var endIndex = collection.Count - 1;

            while (startIndex <= endIndex)
            {
                var midIndex = (startIndex + endIndex) / 2;
                var position = collection[midIndex].CompareTo(item);

                if (position < 0)
                    startIndex = midIndex + 1;
                else if (position > 0)
                    endIndex = midIndex - 1;
                else
                    return midIndex;
            }

            return ~startIndex;
        }


        public static void Sort<T>(IList<T> collection) where T : IComparable<T>
        {
            QuickSort(collection, 0, collection.Count - 1);
        }


        private static void QuickSort<T>(IList<T> collection, int startIndex, int endIndex) where T : IComparable<T>
        {
            if (startIndex < endIndex)
            {
                var partIndex = Partition(collection, startIndex, endIndex);

                QuickSort(collection, startIndex, partIndex - 1);
                QuickSort(collection, partIndex + 1, endIndex);
            }
        }


        private static int Partition<T>(IList<T> collection, int startIndex, int endIndex) where T : IComparable<T>
        {
            var index = startIndex;
            var pivot = collection[endIndex];

            for (var i = startIndex; i < endIndex; ++i)
            {
                if (collection[i].CompareTo(pivot) <= 0)
                {
                    var temp = collection[index];
                    collection[index] = collection[i];
                    collection[i] = temp;

                    ++index;
                }
            }

            collection[endIndex] = collection[index];
            collection[index] = pivot;

            return index;
        }

    }

}
