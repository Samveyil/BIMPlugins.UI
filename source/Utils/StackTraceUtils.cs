using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BIMPlugins.UI.Utils
{
    /// <summary>
    /// Helpers to find the assembly that called this library.
    /// </summary>
    internal static class StackTraceUtils
    {
        /// <summary>
        /// Gets the first assembly in the call stack that is not this library.
        /// Used to resolve images stored as resources of the calling add-in.
        /// </summary>
        /// <returns>The calling assembly, or <c>null</c> if the stack trace is not available.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Assembly GetCallingAssembly()
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            var executingAssembly = Assembly.GetExecutingAssembly();
            if (callingAssembly != executingAssembly) { return callingAssembly; }

            const int skipFrames = 1; // Skip 'StackTraceUtils' method.
            var stackFrames = new StackTrace(skipFrames).GetFrames();
            if (stackFrames == null) return null;

            foreach (var frame in stackFrames)
            {
                var assembly = frame.GetMethod().DeclaringType.Assembly;
                if (assembly != executingAssembly)
                {
                    callingAssembly = assembly;
                    break;
                }
            }
            return callingAssembly;
        }
    }
}