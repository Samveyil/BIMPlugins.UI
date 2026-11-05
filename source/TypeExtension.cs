using System;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods for <see cref="Type"/>.
    /// </summary>
    internal static class TypeExtension
    {
        /// <summary>
        /// Gets the name of <paramref name="type"/>, including its generic arguments in square brackets.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type name, for example <c>Command`1[System.String]</c>.</returns>
        internal static string GetName(this Type type)
        {
            var name = type.Name;
            if (type.IsGenericType)
            {
                foreach (var generic in type.GetGenericArguments())
                    name += $"[{generic}]";
            }
            return name;
        }
    }
}