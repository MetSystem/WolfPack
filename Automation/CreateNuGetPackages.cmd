pushd ..\releases\v%1\source

rem Publishers
del NuGet\Templates\Wolfpack.Publisher\lib\net40\delete.me
copy Wolfpack.Agent\bin\debug\magnum.dll NuGet\Templates\Wolfpack.Publisher\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.dll NuGet\Templates\Wolfpack.Publisher\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.dll NuGet\Templates\Wolfpack.Publisher\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.Magnum.dll NuGet\Templates\Wolfpack.Publisher\lib\net40

rem HealthCheck
del NuGet\Templates\Wolfpack.HealthCheck\lib\net40\delete.me
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net40

rem Build Analytics
del NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40\delete.me
copy Wolfpack.Contrib.BuildAnalytics\bin\debug\Wolfpack.Contrib.BuildAnalytics.dll NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40
copy Wolfpack.Contrib.BuildAnalytics\bin\debug\Wolfpack.Contrib.BuildAnalytics.pdb NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40
copy Wolfpack.Contrib.BuildAnalytics\bin\debug\Sharp2City.dll NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40
copy Wolfpack.Contrib.BuildAnalytics\bin\debug\Sharp2City.pdb NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40

rem Wolfpack (+Sidewinder)
del NuGet\Templates\Wolfpack\lib\net40\delete.me
del NuGet\Templates\Wolfpack\content\config\delete.me
del NuGet\Templates\Wolfpack\content\growl\delete.me
copy Wolfpack.Agent\bin\debug\*.* NuGet\Templates\Wolfpack\lib\net40
copy Wolfpack.Agent\bin\debug\config\*.* NuGet\Templates\Wolfpack\content\config
copy Wolfpack.Agent\bin\debug\growl\*.* NuGet\Templates\Wolfpack\content\growl

popd
pushd ..\
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Publisher\Wolfpack.Publisher.nuspec -version %1 -OutputDirectory Releases\v%1
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.HealthCheck\Wolfpack.HealthCheck.nuspec -version %1 -OutputDirectory Releases\v%1 
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\Wolfpack.Contrib.BuildAnalytics.nuspec -version %1 -OutputDirectory Releases\v%1 
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack\Wolfpack.nuspec -version %1 -OutputDirectory Releases\v%1 
popd

