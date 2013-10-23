using System;

namespace Impromptu
{
    public static class EnvironmentVariable
    {

        #region Public Static Methods

        public static bool SetVariable(string variable, string value, EnvironmentVariableTarget target = EnvironmentVariableTarget.User)
        {
            Environment.SetEnvironmentVariable(variable, value, target);
            return true;
        }

        public static string GetVariable(string variable, EnvironmentVariableTarget target = EnvironmentVariableTarget.User)
        {
            string value;
            try
            {
                value = Environment.GetEnvironmentVariable(variable, target);
            }
            catch
            {
                value = null;
            }

            return value;
        }

        #endregion

    }
}
