using Autodesk.Revit.UI;

namespace BIMPlugins.UI
{
    /// <summary>
    /// Extension methods to work with <see cref="ContextualHelp"/>.
    /// </summary>
    public static class RibbonHelpExtension
    {
        /// <summary>
        /// Creates a <see cref="ContextualHelp"/> for <paramref name="helpPath"/>.
        /// </summary>
        /// <param name="helpPath">An url (starting with <c>http</c>) or a path to a CHM file.</param>
        /// <returns>The created <see cref="ContextualHelp"/>, or <c>null</c> if it could not be created.</returns>
        internal static ContextualHelp GetContextualHelp(string helpPath)
        {
            ContextualHelp contextHelp = null;
            try
            {
                if (helpPath.StartsWith("http"))
                {
                    contextHelp = new ContextualHelp(ContextualHelpType.Url, helpPath);
                }
                else
                {
                    contextHelp = new ContextualHelp(ContextualHelpType.ChmFile, helpPath);
                }
            }
            catch { }
            return contextHelp;
        }
    }
}