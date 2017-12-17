#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var solution = new FilePath("./MvvmDialogs.Contrib.sln");
var netProject = new FilePath("./src/net/MvvmDialogs.Contrib.csproj");
var configuration = Argument("Configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Restore NuGet packages")
    .Does(() =>
    {
        NuGetRestore(solution);
    });

Task("Build")
    .IsDependentOn("Restore NuGet packages")
    .Does(() =>
    {
        // Build for default .NET version
        MSBuild(solution, settings => settings
            .SetConfiguration(configuration)
            .SetMaxCpuCount(0));    // Enable parallel build

        // Build for .NET version 4.0
        MSBuild(netProject, settings => settings
            .SetConfiguration(configuration)
            .WithProperty("TargetFrameworkVersion", "v4.0")
            .SetMaxCpuCount(0));    // Enable parallel build

        // Build for .NET version 3.5
        MSBuild(netProject, settings => settings
            .SetConfiguration(configuration)
            .WithProperty("TargetFrameworkVersion", "v3.5")
            .SetMaxCpuCount(0));    // Enable parallel build
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new NUnit3Settings { NoResults = true };

        NUnit3("./**/bin/" + configuration + "/*Test.dll", settings);
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", "Default"));
