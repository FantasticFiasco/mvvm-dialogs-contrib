#addin "Cake.FileHelpers"

public string GetAssemblyVersion(string filePath)
{
    var versionNumbers = FindRegexMatchesGroupsInFile(
        new FilePath(filePath),
        "AssemblyVersion\\(\"(\\d+).(\\d+).(\\d+)\"\\)",
        RegexOptions.Multiline);

    var major = versionNumbers[0][1];
    var minor = versionNumbers[0][2];
    var patch = versionNumbers[0][3];

    return $"{major}.{minor}.{patch}";
}

public string GetGitTagName()
{
    var tag =  EnvironmentVariable("APPVEYOR_REPO_TAG");
    var tagName = EnvironmentVariable("APPVEYOR_REPO_TAG_NAME");

    return tag == "true" ? tagName : null;
}
