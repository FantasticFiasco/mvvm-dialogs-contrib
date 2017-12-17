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
var nugetSpecification = new FilePath("./MvvmDialogs.Contrib.nuspec");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does (() =>
    {
	    CleanDirectories("./**/bin");
	    CleanDirectories("./**/obj");
    });

Task("Restore NuGet packages")
    .IsDependentOn("Clean")
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

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var version = GetAssemblyVersion("./SolutionInfo.cs");
        var tagName = GetGitTagName();

        // Unless build was trigged by a git tag, this is a pre-release
        if (tagName == null)
        {
            var id = EnvironmentVariable("APPVEYOR_REPO_COMMIT");
            version += $"-{id}";
        }

        NuGetPack(nugetSpecification, new NuGetPackSettings { Version = version });
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
