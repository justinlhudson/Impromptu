using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Impromptu.Utilities
{
    public static class Helper
    {
        public static string ProcessCommand(string command, string args = "", bool useShell = false, bool autoRestart = false)
        {
            var result = string.Empty;
            while(true)
            {
                try
                {
                    var process = new Process();

                    process.StartInfo.FileName = command;
                    process.StartInfo.Arguments = args;   

                    process.StartInfo.RedirectStandardOutput = !useShell;      
                    process.StartInfo.RedirectStandardError = false;
                    process.StartInfo.UseShellExecute = useShell;
                    process.StartInfo.CreateNoWindow = !useShell;

                    process.Start();
                                            
                    process.WaitForExit();

                    result = process.StandardOutput.ReadToEnd();     
                    return result;
                }
                catch(Exception ex)
                {
                    if(!autoRestart)
                        throw;
                    Trace.WriteLine(ex);
                }
            }
            return result;
        }
    }
}

