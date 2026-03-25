using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BIMPlugins.UI.Utils
{
    internal static class StackTraceUtils
    {
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