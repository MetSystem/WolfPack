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

rem Core.Testing
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\delete.me
copy Wolfpack.Core.Testing\bin\debug\*.* NuGet\Templates\Wolfpack.Core.Testing\lib\net40
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\wolfpack.agent.exe.config
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\FluentAssertions.*
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\Magnum.*
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\StoryQ*.*

rem Wolfpack (+Sidewinder)
del NuGet\Templates\Wolfpack\tools\delete.me
del NuGet\Templates\Wolfpack\lib\net40\delete.me
del NuGet\Templates\Wolfpack\content\config\delete.me
del NuGet\Templates\Wolfpack\content\growl\delete.me
copy Wolfpack.Agent\bin\debug\*.* NuGet\Templates\Wolfpack\lib\net40
move NuGet\Templates\Wolfpack\lib\net40\wolfpack.agent.exe.config NuGet\Templates\Wolfpack\content
xcopy Wolfpack.Agent\bin\debug\config\*.* NuGet\Templates\Wolfpack\content\config /Y /R /I /E
xcopy Wolfpack.Agent\bin\debug\growl\*.* NuGet\Templates\Wolfpack\content\growl /Y /R /I /E


popd
pushd ..\
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Publisher\Wolfpack.Publisher.nuspec -version %1 -OutputDirectory Releases\v%1
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.HealthCheck\Wolfpack.HealthCheck.nuspec -version %1 -OutputDirectory Releases\v%1 
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\Wolfpack.Contrib.BuildAnalytics.nuspec -version %1 -OutputDirectory Releases\v%1 
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack\Wolfpack.nuspec -version %1 -OutputDirectory Releases\v%1 
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Core.Testing\Wolfpack.Core.Testing.nuspec -version %1 -OutputDirectory Releases\v%1 
popd

