using Autodesk.Revit.UI;

namespace BIMPlugins.UI
{
    /// <summary>
    /// RibbonHelpExtension
    /// </summary>
    public static class RibbonHelpExtension
    {
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