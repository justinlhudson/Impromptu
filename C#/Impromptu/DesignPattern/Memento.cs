using System.Collections.Generic;

namespace Impromptu.DesignPattern
{
    /// <summary>
    ///   Restores value to the state memorized by this memento.
    /// </summary>
    /// <returns>
    ///   A memento of the state before restoring
    /// </returns>
    public interface IMemento <in T>
    {
        IMemento<T> Restore(T value);
    }

    /// <summary>
    ///   A class used to group multiple mementos together.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class Mementos <T> : IMemento<T>
    {
        #region Fields

        private readonly List<IMemento<T>> _mementos = new List<IMemento<T>>();

        #endregion

        #region Properties

        public int Size {
            get { return _mementos.Count; }
        }

        #endregion

        #region IMemento<T> Members

        IMemento<T> IMemento<T>.Restore(T value)
        {
            return Restore(value);
        }

        #endregion

        #region Public Methods

        public Mementos<T> Restore(T value)
        {
            var inverse = new Mementos<T>();
            // starts from the last action
            for(var i = _mementos.Count - 1; i >= 0; i--)
                inverse.Add(_mementos[i].Restore(value));
            return inverse;
        }

        public void Add(IMemento<T> memento)
        {
            _mementos.Add(memento);
        }

        #endregion
    }
}
