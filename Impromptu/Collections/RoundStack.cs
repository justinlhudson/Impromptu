using System;

namespace Impromptu.Collections
{
    public class RoundStack <T>
    {

        #region Fields

        private readonly T[] m_items;
        // Note: top == bottom ==> full
        private int m_bottom;
        private int m_top = 1;

        #endregion

        #region Properties

        public bool IsFull {
            get { return m_top == m_bottom; }
        }

        public int Count {
            get {
                var count = m_top - m_bottom - 1;
                if(count < 0)
                    count += m_items.Length;
                return count;
            }
        }

        public int Capacity {
            get { return m_items.Length - 1; }
        }

        #endregion

        #region Constructor

        public RoundStack(int capacity)
        {
            m_items = new T[capacity + 1];
        }

        #endregion

        #region Public Methods

        public T Pop()
        {
            if(Count > 0)
            {
                var removed = m_items[m_top];
                m_items[m_top--] = default(T); //null or 0 depending on Type defined/used
                if(m_top < 0)
                    m_top += m_items.Length;
                return removed;
            }
            throw new InvalidOperationException("Stack empty");
        }

        public void Push(T item)
        {
            if(IsFull)
            {
                m_bottom++;
                if(m_bottom >= m_items.Length)
                    m_bottom -= m_items.Length;
            }
            if(++m_top >= m_items.Length)
                m_top -= m_items.Length;
            m_items[m_top] = item;
        }

        public T Peek()
        {
            return m_items[m_top];
        }

        public void Clear()
        {
            if(Count > 0)
            {
                for(var i = 0; i < m_items.Length; i++)
                    m_items[i] = default(T); //null or 0 depending on Type defined/used
                m_top = 1;
                m_bottom = 0;
            }
        }

        #endregion

    }
}
