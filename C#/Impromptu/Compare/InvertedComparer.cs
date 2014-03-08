using System;
using System.Collections.Generic;
using System.Threading;

namespace Impromptu.Compare
{
    public class InvertedComparer <T> : Comparer<T>
    {
        #region Fields

        private static InvertedComparer<T> _defaultComparer;
        private readonly IComparer<T> _comparer;

        #endregion

        #region Properties

        public new static InvertedComparer<T> Default {
            get {
                if(_defaultComparer == null)
                    Interlocked.CompareExchange(ref _defaultComparer, new InvertedComparer<T>(Comparer<T>.Default), null); //make atomic operation
                return _defaultComparer;
            }
        }

        #endregion

        #region Constructor

        public InvertedComparer(IComparer<T> comparer)
        {
            if(comparer == null)
                throw new ArgumentNullException("comparer");
            _comparer = comparer;
        }

        #endregion

        #region Public Override Methods

        public override int Compare(T x, T y)
        {
            // Reverse the arguments to get the inverted result.
            return _comparer.Compare(y, x);
        }

        #endregion
    }
}
