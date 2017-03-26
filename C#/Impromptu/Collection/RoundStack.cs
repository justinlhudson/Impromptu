using System;

namespace Impromptu.Collection
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>Not thread safe.</remarks>
	public class RoundStack<T>
	{
		#region Fields

		private readonly T[] _items;
		// Note: top == bottom ==> full
		private int _bottom;
		private int _top = 1;

		#endregion

		#region Properties

		public bool IsFull
		{
			get { return _top == _bottom; }
		}

		public int Count
		{
			get
			{
				var count = _top - _bottom - 1;
				if (count < 0)
					count += _items.Length;
				return count;
			}
		}

		public int Capacity
		{
			get { return _items.Length - 1; }
		}

		#endregion

		#region Constructor

		public RoundStack(int capacity)
		{
			_items = new T[capacity + 1];
		}

		#endregion

		#region Public Methods

		public T Pop()
		{
			if (Count > 0)
			{
				var removed = _items[_top];
				_items[_top--] = default(T); //null or 0 depending on Type defined/used
				if (_top < 0)
					_top += _items.Length;
				return removed;
			}
			throw new InvalidOperationException("Stack empty");
		}

		public void Push(T item)
		{
			if (IsFull)
			{
				_bottom++;
				if (_bottom >= _items.Length)
					_bottom -= _items.Length;
			}
			if (++_top >= _items.Length)
				_top -= _items.Length;
			_items[_top] = item;
		}

		public T Peek()
		{
			return _items[_top];
		}

		public void Clear()
		{
			if (Count > 0)
			{
				for (var i = 0; i < _items.Length; i++)
					_items[i] = default(T); //null or 0 depending on Type defined/used
				_top = 1;
				_bottom = 0;
			}
		}

		#endregion
	}
}
