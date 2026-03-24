using Autodesk.Revit.UI;

namespace BIMPlugins.UI
{
    public static class RibbonHelp
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