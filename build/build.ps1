$SOLUTION = 'MvvmDialogs.Contrib.sln'
$NETPROJECT = 'src\net\MvvmDialogs.Contrib.csproj'
$LOGGER = 'C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll'

Write-Host 'Restore NuGet packages...'
nuget restore $SOLUTION

Write-Host 'Build solution in default configuration...'
msbuild $SOLUTION /t:Rebuild /p:Configuration=Release /m /logger:$LOGGER