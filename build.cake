#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#load "build/utils.cake"

using System.Text.RegularExpressions;

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
    .Description("Run all tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        NUnit3("./**/bin/" + configuration + "/*Test.dll");
    });

Task("Pack")
    .Description("Create NuGet package")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var version = GetAssemblyVersion("./SolutionInfo.cs");
        var branch = EnvironmentVariable("APPVEYOR_REPO_BRANCH");

        // Unless on master, this is a pre-release
        if (branch != "master")
        {
            var sha = EnvironmentVariable("APPVEYOR_REPO_COMMIT");
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
