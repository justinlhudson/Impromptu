using System;
using System.IO;
using System.Reflection;

namespace Impromptu
{
    #region Event Custom Arguments
    /// <summary>
    ///   As of now used as an example.
    /// </summary>
    public class EventArgsCustom : EventArgs
    {
        #region Fields

        #endregion

        #region Properties

        public object Value { get; set; }

        #endregion

        #region Public Methods

        public EventArgsCustom(object value)
        {
            Value = value;
        }

        #endregion
    }
    #endregion
    /// <summary>
    ///   Used for most common shared methods
    /// </summary>
    public static class Common
    {
        public static string PathExecuting { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }
    }
}
