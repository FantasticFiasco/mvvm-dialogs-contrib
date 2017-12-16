$SOLUTION = 'MvvmDialogs.Contrib.sln'
$NETPROJECT = 'src\net\MvvmDialogs.Contrib.csproj'
$LOGGER = 'C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll'

Write-Host 'Restore NuGet packages...' -ForegroundColor Green
nuget restore $SOLUTION

Write-Host 'Build solution in default configuration...' -ForegroundColor Green
msbuild $SOLUTION /t:Rebuild /p:Configuration=Release /m /logger:$LOGGER