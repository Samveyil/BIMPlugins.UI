using System;

namespace BIMPlugins.UI
{
    internal static class TypeExtension
    {
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