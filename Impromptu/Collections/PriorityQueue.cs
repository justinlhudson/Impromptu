using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Impromptu.Comparer;

namespace Impromptu.Collections
{
    public sealed class PriorityQueue <T> : IEnumerable<T>, ICollection
    {

        #region Fields

        private readonly IComparer<T> m_comparer = InvertedComparer<T>.Default;
        private readonly List<T> m_queue;
        private object m_syncRoot;

        #endregion

        #region Public IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            var copy = new List<T>(m_queue);
            copy.Sort(m_comparer);
            return copy.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Public ICollection

        public int Count {
            get { return m_queue.Count; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)m_queue).CopyTo(array, index);
        }

        bool ICollection.IsSynchronized {
            get { return false; }
        }

        object ICollection.SyncRoot {
            get {
                if(m_syncRoot == null)
                    Interlocked.CompareExchange(ref m_syncRoot, new object(), null);
                return m_syncRoot;
            }
        }

        #endregion

        #region Properties

        //public IComparer<T> Comparer { get; private set; }
        public int Capacity {
            get { return m_queue.Capacity; }
            set { m_queue.Capacity = value; }
        }

        #endregion

        #region Constructor

        public PriorityQueue() : this((IComparer<T>)null)
        {
        }

        public PriorityQueue(IEnumerable<T> collection, IComparer<T> comparer) : this((List<T>)null, comparer)
        {
            m_comparer = comparer;

            if(collection == null)
                throw new ArgumentNullException("collection");
            m_queue = new List<T>(collection);

            if(m_queue.Count > 1)
            {
                for(var index = (m_queue.Count - 1) >> 1; index >= 0; --index)
                    DownList(index);
            }
        }

        public PriorityQueue(IEnumerable<T> collection) : this(collection, null)
        {
        }

        public PriorityQueue(int capacity, IComparer<T> comparer) : this(new List<T>(capacity), comparer)
        {
        }

        public PriorityQueue(IComparer<T> comparer) : this(new List<T>(), comparer)
        {
        }

        private PriorityQueue(List<T> queue, IComparer<T> comparer)
        {
            m_comparer = comparer ?? Comparer<T>.Default;
            m_queue = queue;
        }

        private PriorityQueue(T[] queue, IComparer<T> comparer)
        {
            m_comparer = comparer ?? Comparer<T>.Default;
            m_queue = new List<T>();

            foreach(var item in queue)
                m_queue.Add(item);
        }

        #endregion

        #region Public Methods

        public void Enqueue(T item)
        {
            m_queue.Add(item);
            UpList();
        }

        public T Dequeue()
        {
            if(m_queue.Count == 0)
                throw new InvalidOperationException("Empty");
            var result = m_queue[0];
            var lastIndex = m_queue.Count - 1;
            m_queue[0] = m_queue[lastIndex];
            m_queue.RemoveAt(lastIndex);
            if(m_queue.Count > 0)
                DownList(0);
            return result;
        }

        public T Peek()
        {
            if(m_queue.Count == 0)
                throw new InvalidOperationException("Empty");
            return m_queue[0];
        }

        public void AdjustFirstItem()
        {
            if(m_queue.Count == 0)
                throw new InvalidOperationException("Empty");

            DownList(0);
        }

        public void Clear()
        {
            m_queue.Clear();
        }

        public void TrimExcess()
        {
            m_queue.TrimExcess();
        }

        public bool Contains(T item)
        {
            return m_queue.Contains(item);
        }

        public T[] ToArray()
        {
            var result = m_queue.ToArray();
            //return the elements in the same order in which they are enumerated
            Array.Sort(result, m_comparer);
            return result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_queue.CopyTo(array, arrayIndex);
            Array.Sort(array, arrayIndex, m_queue.Count, m_comparer);
        }

        #endregion

        #region Private Methods

        private void UpList()
        {
            var index = m_queue.Count - 1;
            var item = m_queue[index];
            var parentIndex = (index - 1) >> 1;
            //if already at zero then at top
            while(index > 0 && m_comparer.Compare(item, m_queue[parentIndex]) < 0)
            {
                m_queue[index] = m_queue[parentIndex];
                index = parentIndex;
                parentIndex = (index - 1) >> 1;
            }
            m_queue[index] = item;
        }

        private void DownList(int index)
        {
            var item = m_queue[index];
            var count = m_queue.Count;
            var firstChild = (index << 1) + 1;
            var secondChild = firstChild + 1;
            var smallestChild = (secondChild < count && m_comparer.Compare(m_queue[secondChild], m_queue[firstChild]) < 0) ? secondChild : firstChild;
            while(smallestChild < count && m_comparer.Compare(m_queue[smallestChild], item) < 0)
            {
                m_queue[index] = m_queue[smallestChild];
                index = smallestChild;
                firstChild = (index << 1) + 1;
                secondChild = firstChild + 1;
                smallestChild = (secondChild < count && m_comparer.Compare(m_queue[secondChild], m_queue[firstChild]) < 0) ? secondChild : firstChild;
            }
            m_queue[index] = item;
        }

        #endregion

    }
}
