using System;
using System.IO;
using System.Reflection;

namespace Impromptu
{
    #region Delegate Types
    /// <summary>
    ///   Super generic delegate for simple notification.
    /// </summary>
    /// <remarks>
    ///   Used as a void no arguments passed function pointer.
    /// </remarks>
    public delegate void Delegate();
    /// <summary>
    ///   Super generic delegate for simple notification with event agruments.
    /// </summary>
    /// <param name = "arg"></param>
    public delegate void DelegateArg(object arg);
    /// <summary>
    ///   Thread safe delegate type
    /// </summary>
    /// <param name = "args"></param>
	public delegate void DelegateArguments(params object[] args);
    #endregion
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
    ///   Used for most common shared functions
    /// </summary>
    public static class Common
    {
        public static string PathExecuting { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }
    }
}
