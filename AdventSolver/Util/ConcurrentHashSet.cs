using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace AdventSolver.Util
{
    public class ConcurrentHashSet<T> : IReadOnlyCollection<T>, ICollection<T>
    {
        private const int DefaultCapacity = 31;
        private const int MaxLockNumber = 1024;

        private readonly IEqualityComparer<T> comparer;
        private readonly bool growLockArray;

        private int budget;
        private volatile Tables dataTables;

        public ConcurrentHashSet()
            : this(DefaultConcurrencyLevel, DefaultCapacity, true, null)
        {
        }

        public ConcurrentHashSet(int concurrencyLevel, int capacity)
            : this(concurrencyLevel, capacity, false, null)
        {
        }

        public ConcurrentHashSet(IEnumerable<T> collection)
            : this(collection, null)
        {
        }

        public ConcurrentHashSet(IEqualityComparer<T> comparer)
            : this(DefaultConcurrencyLevel, DefaultCapacity, true, comparer)
        {
        }

        public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(comparer)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            InitializeFromCollection(collection);
        }

        public ConcurrentHashSet(int concurrencyLevel, IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(concurrencyLevel, DefaultCapacity, false, comparer)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            InitializeFromCollection(collection);
        }

        public ConcurrentHashSet(int concurrencyLevel, int capacity, IEqualityComparer<T> comparer)
            : this(concurrencyLevel, capacity, false, comparer)
        {
        }

        private ConcurrentHashSet(int concurrencyLevel, int capacity, bool growLockArray, IEqualityComparer<T> comparer)
        {
            if (concurrencyLevel < 1) throw new ArgumentOutOfRangeException(nameof(concurrencyLevel));
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            if (capacity < concurrencyLevel) capacity = concurrencyLevel;

            var locks = new object[concurrencyLevel];
            for (int i = 0; i < locks.Length; i++) locks[i] = new object();

            int[] countPerLock = new int[locks.Length];
            var buckets = new Node[capacity];
            dataTables = new Tables(buckets, locks, countPerLock);

            this.growLockArray = growLockArray;
            budget = buckets.Length / locks.Length;
            this.comparer = comparer ?? EqualityComparer<T>.Default;
        }

        private static int DefaultConcurrencyLevel => PlatformHelper.ProcessorCount;

        public bool IsEmpty
        {
            get
            {
                int acquiredLocks = 0;
                try
                {
                    AcquireAllLocks(ref acquiredLocks);

                    for (int i = 0; i < dataTables.CountPerLock.Length; i++)
                        if (dataTables.CountPerLock[i] != 0)
                            return false;
                }
                finally
                {
                    ReleaseLocks(0, acquiredLocks);
                }

                return true;
            }
        }

        public void Clear()
        {
            int locksAcquired = 0;
            try
            {
                AcquireAllLocks(ref locksAcquired);

                var newTables = new Tables(new Node[DefaultCapacity], dataTables.Locks,
                    new int[dataTables.CountPerLock.Length]);
                dataTables = newTables;
                budget = Math.Max(1, newTables.Buckets.Length / newTables.Locks.Length);
            }
            finally
            {
                ReleaseLocks(0, locksAcquired);
            }
        }

        public bool Contains(T item)
        {
            int hashcode = comparer.GetHashCode(item);
            var tables = dataTables;
            int bucketNo = GetBucket(hashcode, tables.Buckets.Length);

            var current = Volatile.Read(ref tables.Buckets[bucketNo]);

            while (current != null)
            {
                if (hashcode == current.Hashcode && comparer.Equals(current.Item, item)) return true;
                current = current.Next;
            }

            return false;
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        bool ICollection<T>.IsReadOnly => false;

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            int locksAcquired = 0;
            try
            {
                AcquireAllLocks(ref locksAcquired);

                int count = 0;

                for (int i = 0; i < dataTables.Locks.Length && count >= 0; i++) count += dataTables.CountPerLock[i];

                if (array.Length - count < arrayIndex || count < 0
                ) //"count" itself or "count + arrayIndex" can overflow
                    throw new ArgumentException(
                        "The index is equal to or greater than the length of the array, or the number of elements in the set is greater than the available space from index to the end of the destination array.");

                CopyToItems(array, arrayIndex);
            }
            finally
            {
                ReleaseLocks(0, locksAcquired);
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            return TryRemove(item);
        }

        public int Count
        {
            get
            {
                int count = 0;
                int acquiredLocks = 0;
                try
                {
                    AcquireAllLocks(ref acquiredLocks);

                    for (int i = 0; i < dataTables.CountPerLock.Length; i++) count += dataTables.CountPerLock[i];
                }
                finally
                {
                    ReleaseLocks(0, acquiredLocks);
                }

                return count;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var buckets = dataTables.Buckets;

            for (int i = 0; i < buckets.Length; i++)
            {
                var current = Volatile.Read(ref buckets[i]);

                while (current != null)
                {
                    yield return current.Item;
                    current = current.Next;
                }
            }
        }

        public bool Add(T item)
        {
            return AddInternal(item, comparer.GetHashCode(item), true);
        }

        public bool TryRemove(T item)
        {
            int hashcode = comparer.GetHashCode(item);
            while (true)
            {
                var tables = dataTables;

                GetBucketAndLockNo(hashcode, out int bucketNo, out int lockNo, tables.Buckets.Length,
                    tables.Locks.Length);

                lock (tables.Locks[lockNo])
                {
                    if (tables != dataTables) continue;

                    Node previous = null;
                    for (var current = tables.Buckets[bucketNo]; current != null; current = current.Next)
                    {
                        Debug.Assert(
                            previous == null && current == tables.Buckets[bucketNo] || previous.Next == current);

                        if (hashcode == current.Hashcode && comparer.Equals(current.Item, item))
                        {
                            if (previous == null)
                                Volatile.Write(ref tables.Buckets[bucketNo], current.Next);
                            else
                                previous.Next = current.Next;

                            tables.CountPerLock[lockNo]--;
                            return true;
                        }

                        previous = current;
                    }
                }

                return false;
            }
        }

        private void InitializeFromCollection(IEnumerable<T> collection)
        {
            foreach (var item in collection) AddInternal(item, comparer.GetHashCode(item), false);

            if (budget == 0) budget = dataTables.Buckets.Length / dataTables.Locks.Length;
        }

        private bool AddInternal(T item, int hashcode, bool acquireLock)
        {
            while (true)
            {
                var tables = dataTables;

                GetBucketAndLockNo(hashcode, out int bucketNo, out int lockNo, tables.Buckets.Length,
                    tables.Locks.Length);

                bool resizeDesired = false;
                bool lockTaken = false;
                try
                {
                    if (acquireLock)
                        Monitor.Enter(tables.Locks[lockNo], ref lockTaken);

                    if (tables != dataTables) continue;

                    Node previous = null;
                    for (var current = tables.Buckets[bucketNo]; current != null; current = current.Next)
                    {
                        Debug.Assert(
                            previous == null && current == tables.Buckets[bucketNo] || previous.Next == current);
                        if (hashcode == current.Hashcode && comparer.Equals(current.Item, item)) return false;
                        previous = current;
                    }

                    Volatile.Write(ref tables.Buckets[bucketNo], new Node(item, hashcode, tables.Buckets[bucketNo]));
                    checked
                    {
                        tables.CountPerLock[lockNo]++;
                    }

                    if (tables.CountPerLock[lockNo] > budget) resizeDesired = true;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(tables.Locks[lockNo]);
                }

                if (resizeDesired) GrowTable(tables);

                return true;
            }
        }

        private static int GetBucket(int hashcode, int bucketCount)
        {
            int bucketNo = (hashcode & 0x7fffffff) % bucketCount;
            Debug.Assert(bucketNo >= 0 && bucketNo < bucketCount);
            return bucketNo;
        }

        private static void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount,
            int lockCount)
        {
            bucketNo = (hashcode & 0x7fffffff) % bucketCount;
            lockNo = bucketNo % lockCount;

            Debug.Assert(bucketNo >= 0 && bucketNo < bucketCount);
            Debug.Assert(lockNo >= 0 && lockNo < lockCount);
        }

        private void GrowTable(Tables tables)
        {
            const int maxArrayLength = 0X7FEFFFFF;
            int locksAcquired = 0;
            try
            {
                AcquireLocks(0, 1, ref locksAcquired);

                if (tables != dataTables)
                    return;

                long approxCount = tables.CountPerLock
                    .Aggregate<int, long>(0, (current, val) => current + val);

                if (approxCount < tables.Buckets.Length / 4)
                {
                    budget = 2 * budget;
                    if (budget < 0) budget = int.MaxValue;
                    return;
                }

                int newLength = 0;
                bool maximizeTableSize = false;
                try
                {
                    checked
                    {
                        newLength = tables.Buckets.Length * 2 + 1;

                        while (newLength % 3 == 0 || newLength % 5 == 0 || newLength % 7 == 0) newLength += 2;

                        Debug.Assert(newLength % 2 != 0);

                        if (newLength > maxArrayLength) maximizeTableSize = true;
                    }
                }
                catch (OverflowException)
                {
                    maximizeTableSize = true;
                }

                if (maximizeTableSize)
                {
                    newLength = maxArrayLength;
                    budget = int.MaxValue;
                }

                AcquireLocks(1, tables.Locks.Length, ref locksAcquired);
                var newLocks = tables.Locks;

                if (growLockArray && tables.Locks.Length < MaxLockNumber)
                {
                    newLocks = new object[tables.Locks.Length * 2];
                    Array.Copy(tables.Locks, 0, newLocks, 0, tables.Locks.Length);
                    for (int i = tables.Locks.Length; i < newLocks.Length; i++) newLocks[i] = new object();
                }

                var newBuckets = new Node[newLength];
                int[] newCountPerLock = new int[newLocks.Length];

                for (int i = 0; i < tables.Buckets.Length; i++)
                {
                    var current = tables.Buckets[i];
                    while (current != null)
                    {
                        var next = current.Next;
                        GetBucketAndLockNo(current.Hashcode, out int newBucketNo, out int newLockNo, newBuckets.Length,
                            newLocks.Length);

                        newBuckets[newBucketNo] = new Node(current.Item, current.Hashcode, newBuckets[newBucketNo]);

                        checked
                        {
                            newCountPerLock[newLockNo]++;
                        }

                        current = next;
                    }
                }

                budget = Math.Max(1, newBuckets.Length / newLocks.Length);
                dataTables = new Tables(newBuckets, newLocks, newCountPerLock);
            }
            finally
            {
                ReleaseLocks(0, locksAcquired);
            }
        }

        private void AcquireAllLocks(ref int locksAcquired)
        {
            AcquireLocks(0, 1, ref locksAcquired);
            AcquireLocks(1, dataTables.Locks.Length, ref locksAcquired);
            Debug.Assert(locksAcquired == dataTables.Locks.Length);
        }

        private void AcquireLocks(int fromInclusive, int toExclusive, ref int locksAcquired)
        {
            Debug.Assert(fromInclusive <= toExclusive);
            var locks = dataTables.Locks;

            for (int i = fromInclusive; i < toExclusive; i++)
            {
                bool lockTaken = false;
                try
                {
                    Monitor.Enter(locks[i], ref lockTaken);
                }
                finally
                {
                    if (lockTaken) locksAcquired++;
                }
            }
        }

        private void ReleaseLocks(int fromInclusive, int toExclusive)
        {
            Debug.Assert(fromInclusive <= toExclusive);

            for (int i = fromInclusive; i < toExclusive; i++) Monitor.Exit(dataTables.Locks[i]);
        }

        private void CopyToItems(T[] array, int index)
        {
            var buckets = dataTables.Buckets;
            for (int i = 0; i < buckets.Length; i++)
            for (var current = buckets[i]; current != null; current = current.Next)
            {
                array[index] = current.Item;
                index++; //this should never flow, CopyToItems is only called when there's no overflow risk
            }
        }

        private class Tables
        {
            public readonly Node[] Buckets;
            public readonly object[] Locks;

            public volatile int[] CountPerLock;

            public Tables(Node[] buckets, object[] locks, int[] countPerLock)
            {
                Buckets = buckets;
                Locks = locks;
                CountPerLock = countPerLock;
            }
        }

        private class Node
        {
            public readonly int Hashcode;
            public readonly T Item;

            public volatile Node Next;

            public Node(T item, int hashcode, Node next)
            {
                Item = item;
                Hashcode = hashcode;
                Next = next;
            }
        }

        private static class PlatformHelper
        {
            private const int ProcessorCountRefreshIntervalMs = 30000;

            private static volatile int processorCount;
            private static volatile int lastProcessorCountRefreshTicks;

            internal static int ProcessorCount
            {
                get
                {
                    int now = Environment.TickCount;
                    if (processorCount != 0 && now - lastProcessorCountRefreshTicks < ProcessorCountRefreshIntervalMs)
                        return processorCount;

                    processorCount = Environment.ProcessorCount;
                    lastProcessorCountRefreshTicks = now;
                    return processorCount;
                }
            }
        }
    }
}