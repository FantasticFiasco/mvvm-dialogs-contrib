#load "build/utils.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////

var solution = new FilePath("./MvvmDialogs.Contrib.sln");
var netProject = new FilePath("./src/net/MvvmDialogs.Contrib.csproj");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Description("Clean all files")
    .Does (() =>
    {
	    CleanDirectories("./**/bin");
	    CleanDirectories("./**/obj");
    });

Task("Restore")
    .Description("Restore NuGet packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore(solution);
    });

Task("Build")
    .Description("Build the solution")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        MSBuild(solution, settings => settings
            .SetConfiguration(configuration)
            .SetMaxCpuCount(0));    // Enable parallel build
    });

Task("Pack")
    .Description("Create NuGet package")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var version = GetAssemblyVersion("./SolutionInfo.cs");
        var branch = EnvironmentVariable("APPVEYOR_REPO_BRANCH");

        // Unless on master, this is a pre-release
        if (branch != "master")
        {
            var sha = EnvironmentVariable("APPVEYOR_REPO_COMMIT") ?? "local";
            version += $"-sha-{sha}";
        }

        NuGetPack(
            "./MvvmDialogs.Contrib.nuspec",
            new NuGetPackSettings
            {
                Version = version,
                Symbols = true
            });
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
