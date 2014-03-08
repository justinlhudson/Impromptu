using System;
using Impromptu.Collection;

namespace Impromptu.DesignPattern
{
    /// <summary>
    ///   This class represents an undo and redo history.
    /// </summary>
    public class UndoRedo <T>
    {
        #region Fields

        private const int DefaultCapacity = 25;
        private readonly Object m_lock = new Object();
        private readonly T m_subject;
        private readonly RoundStack<IMemento<T>> m_redoStack;
        private readonly RoundStack<IMemento<T>> m_undoStack;

        #endregion

        #region Properties

        public bool CanUndo {
            get { return (m_undoStack.Count != 0); }
        }

        public bool CanRedo {
            get { return (m_redoStack.Count != 0); }
        }

        public int UndoCount {
            get { return m_undoStack.Count; }
        }

        public int RedoCount {
            get { return m_redoStack.Count; }
        }

        #endregion

        #region Constructor

        public UndoRedo(T subject, int capacity = DefaultCapacity)
        {
            m_subject = subject;
            m_undoStack = new RoundStack<IMemento<T>>(capacity);
            m_redoStack = new RoundStack<IMemento<T>>(capacity);
        }

        #endregion

        #region Public Methods

        public void Do(IMemento<T> m)
        {
            lock(m_lock)
            {
                m_undoStack.Push(m);
            }
        }

        public void Undo()
        {
            lock(m_lock)
            {
                if(CanUndo)
                {
                    var top = m_undoStack.Pop();
                    m_redoStack.Push(top.Restore(m_subject));
                }
            }
        }

        public void Redo()
        {
            lock(m_lock)
            {
                if(CanRedo)
                {
                    var top = m_redoStack.Pop();
                    m_undoStack.Push(top.Restore(m_subject));
                }
            }
        }

        public void Clear()
        {
            m_undoStack.Clear();
            m_redoStack.Clear();
        }

        public IMemento<T> PeekUndo()
        {
            if(m_undoStack.Count > 0)
                return m_undoStack.Peek();
            return null;
        }

        public IMemento<T> PeekRedo()
        {
            if(m_redoStack.Count > 0)
                return m_redoStack.Peek();
            return null;
        }

        #endregion
    }
}
