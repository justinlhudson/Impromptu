using System;
using System.Collections.Generic;
using System.Threading;

namespace Impromptu.Comparer
{
    public class InvertedComparer <T> : Comparer<T>
    {

        #region Fields

        private static InvertedComparer<T> m_defaultComparer;
        private readonly IComparer<T> m_comparer;

        #endregion

        #region Properties

        public new static InvertedComparer<T> Default {
            get {
                if(m_defaultComparer == null)
                    Interlocked.CompareExchange(ref m_defaultComparer, new InvertedComparer<T>(Comparer<T>.Default), null); //make atomic operation
                return m_defaultComparer;
            }
        }

        #endregion

        #region Constructor

        public InvertedComparer(IComparer<T> comparer)
        {
            if(comparer == null)
                throw new ArgumentNullException("comparer");
            m_comparer = comparer;
        }

        #endregion

        #region Public Override Methods

        public override int Compare(T x, T y)
        {
            // Reverse the arguments to get the inverted result.
            return m_comparer.Compare(y, x);
        }

        #endregion

    }
}
