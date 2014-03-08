using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Impromptu.Compare;

namespace Impromptu.Collection
{
    /// <summary>
    /// Priority queue.
    /// </summary>
    /// <remarks>Not thread safe.</remarks>
    public sealed class PriorityQueue <T> : IEnumerable<T>, ICollection
    {
        #region Fields

        private readonly IComparer<T> _comparer = InvertedComparer<T>.Default;
        private readonly List<T> _queue;
        private object m_syncRoot;

        #endregion

        #region Public IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            var copy = new List<T>(_queue);
            copy.Sort(_comparer);
            return copy.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Public ICollection

        public int Count {
            get { return _queue.Count; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_queue).CopyTo(array, index);
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
            get { return _queue.Capacity; }
            set { _queue.Capacity = value; }
        }

        #endregion

        #region Constructor

        public PriorityQueue() : this((IComparer<T>)null)
        {
        }

        public PriorityQueue(IEnumerable<T> collection, IComparer<T> comparer) : this((List<T>)null, comparer)
        {
            _comparer = comparer;

            if(collection == null)
                throw new ArgumentNullException("collection");
            _queue = new List<T>(collection);

            if(_queue.Count > 1)
            {
                for(var index = (_queue.Count - 1) >> 1; index >= 0; --index)
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
            _comparer = comparer ?? Comparer<T>.Default;
            _queue = queue;
        }

        private PriorityQueue(T[] queue, IComparer<T> comparer)
        {
            _comparer = comparer ?? Comparer<T>.Default;
            _queue = new List<T>();

            foreach(var item in queue)
                _queue.Add(item);
        }

        #endregion

        #region Public Methods

        public void Enqueue(T item)
        {
            _queue.Add(item);
            UpList();
        }

        public T Dequeue()
        {
            if(_queue.Count == 0)
                throw new InvalidOperationException("Empty");
            var result = _queue[0];
            var lastIndex = _queue.Count - 1;
            _queue[0] = _queue[lastIndex];
            _queue.RemoveAt(lastIndex);
            if(_queue.Count > 0)
                DownList(0);
            return result;
        }

        public T Peek()
        {
            if(_queue.Count == 0)
                throw new InvalidOperationException("Empty");
            return _queue[0];
        }

        public void AdjustFirstItem()
        {
            if(_queue.Count == 0)
                throw new InvalidOperationException("Empty");

            DownList(0);
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public void TrimExcess()
        {
            _queue.TrimExcess();
        }

        public bool Contains(T item)
        {
            return _queue.Contains(item);
        }

        public T[] ToArray()
        {
            var result = _queue.ToArray();
            //return the elements in the same order in which they are enumerated
            Array.Sort(result, _comparer);
            return result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _queue.CopyTo(array, arrayIndex);
            Array.Sort(array, arrayIndex, _queue.Count, _comparer);
        }

        #endregion

        #region Private Methods

        private void UpList()
        {
            var index = _queue.Count - 1;
            var item = _queue[index];
            var parentIndex = (index - 1) >> 1;
            //if already at zero then at top
            while(index > 0 && _comparer.Compare(item, _queue[parentIndex]) < 0)
            {
                _queue[index] = _queue[parentIndex];
                index = parentIndex;
                parentIndex = (index - 1) >> 1;
            }
            _queue[index] = item;
        }

        private void DownList(int index)
        {
            var item = _queue[index];
            var count = _queue.Count;
            var firstChild = (index << 1) + 1;
            var secondChild = firstChild + 1;
            var smallestChild = (secondChild < count && _comparer.Compare(_queue[secondChild], _queue[firstChild]) < 0) ? secondChild : firstChild;
            while(smallestChild < count && _comparer.Compare(_queue[smallestChild], item) < 0)
            {
                _queue[index] = _queue[smallestChild];
                index = smallestChild;
                firstChild = (index << 1) + 1;
                secondChild = firstChild + 1;
                smallestChild = (secondChild < count && _comparer.Compare(_queue[secondChild], _queue[firstChild]) < 0) ? secondChild : firstChild;
            }
            _queue[index] = item;
        }

        #endregion
    }
}
