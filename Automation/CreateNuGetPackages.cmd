pushd ..\releases\v%1\source
del NuGet\Templates\Wolfpack.Publisher\lib\net40\delete.me
copy Wolfpack.Agent\bin\debug\magnum.dll NuGet\Templates\Wolfpack.Publisher\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.dll NuGet\Templates\Wolfpack.Publisher\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.dll NuGet\Templates\Wolfpack.Publisher\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.Magnum.dll NuGet\Templates\Wolfpack.Publisher\lib\net40

del NuGet\Templates\Wolfpack.HealthCheck\lib\net40\delete.me
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net40

del NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40\delete.me
copy Wolfpack.Agent\bin\debug\Wolfpack.Contrib.BuildAnalytics.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Contrib.BuildAnalytics.pdb NuGet\Templates\Wolfpack.HealthCheck\lib\net40
copy Wolfpack.Agent\bin\debug\Sharp2City.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net40
copy Wolfpack.Agent\bin\debug\Sharp2City.xml NuGet\Templates\Wolfpack.HealthCheck\lib\net40

popd
pushd ..\
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Publisher\Wolfpack.Publisher.nuspec -version %1 -OutputDirectory Releases\v%1
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.HealthCheck\Wolfpack.HealthCheck.nuspec -version %1 -OutputDirectory Releases\v%1 
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\Wolfpack.Contrib.BuildAnalytics.nuspec -version %1 -OutputDirectory Releases\v%1 
popd

