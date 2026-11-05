using BIMPlugins.Nuke;
using Nuke.Common;
using Nuke.Common.ProjectModel;

public class Build : BIMPluginsBuild
{
    protected override int MajorVersion => 1;
    protected override int MinorVersion => 0;
    protected override int MaintenanceVersion => 4;


    [Solution("BIMPlugins.UI.sln")]
    protected override Solution Solution { get; }

    [Parameter("Project name to build")]
    protected override string ProjectName { get; } = "BIMPlugins.UI";

    [Parameter("Build configurations to run sequentially")]
    protected override string[] Configurations { get; } = new[] { "Release" };

    public static int Main() => Execute<Build>(x => x.Compile);
}