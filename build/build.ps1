$SOLUTION = 'MvvmDialogs.Contrib.sln'
$NETPROJECT = 'src\net\MvvmDialogs.Contrib.csproj'
$NUSPEC = 'MvvmDialogs.Contrib.nuspec'
$LOGGER = 'C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll'

Write-Host 'Restore NuGet packages...' -ForegroundColor Green
nuget restore $SOLUTION

Write-Host 'Build for default .NET version...' -ForegroundColor Green
msbuild $SOLUTION /t:Rebuild /p:'Configuration=Release' /m /logger:$LOGGER

Write-Host 'Build for additional .NET versions...' -ForegroundColor Green
msbuild $NETPROJECT /t:Rebuild /p:'Configuration=Release;TargetFrameworkVersion=v4.0' /m /logger:$LOGGER
msbuild $NETPROJECT /t:Rebuild /p:'Configuration=Release;TargetFrameworkVersion=v3.5' /m /logger:$LOGGER

Write-Host 'Create NuGet package...'
if ($ENV:APPVEYOR_REPO_BRANCH -ne  'master') {
    $VERSION_SUFFIX = '-' + $ENV:APPVEYOR_BUILD_ID
}

nuget pack $NUSPEC -version 'test$VERSION_SUFFIX'