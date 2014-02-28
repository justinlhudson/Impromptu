namespace Impromptu.DesignPattern
{
    /// <summary>
    ///   The 'Command' abstract class
    /// </summary>
    internal abstract class Command
    {
        /*
         * Eaxmple: When doing math problem and adding you must minus
         *          to "UnExecute" the "Execute" add command.
         */
        public abstract void Execute();
        //define how to Do
        public abstract void UnExecute();
        //define how to UnDo
    }
}
