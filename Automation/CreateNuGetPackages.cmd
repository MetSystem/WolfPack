pushd ..\releases\v%1\source
del NuGet\Templates\Wolfpack.Publisher\lib\net20\delete.me
copy Wolfpack.Agent\bin\debug\magnum.dll NuGet\Templates\Wolfpack.Publisher\lib\net20
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.dll NuGet\Templates\Wolfpack.Publisher\lib\net20
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.dll NuGet\Templates\Wolfpack.Publisher\lib\net20
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.Magnum.dll NuGet\Templates\Wolfpack.Publisher\lib\net20

del NuGet\Templates\Wolfpack.HealthCheck\lib\net20\delete.me
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net20
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net20

popd
pushd ..\
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Publisher\Wolfpack.Publisher.nuspec -version %1 -OutputDirectory Releases\v%1
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.HealthCheck\Wolfpack.HealthCheck.nuspec -version %1 -OutputDirectory Releases\v%1 
popd

