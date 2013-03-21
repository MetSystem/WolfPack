SET NuGetVersion=%1
IF NOT %2 == "" (
	SET NuGetVersion=%1-%2
)

ECHO NuGetVersion is %NuGetVersion%

pushd ..\releases\v%1\source

rem WebServices
del NuGet\Templates\Wolfpack.WebServices\lib\net40\delete.me
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.WebServices.Interfaces.dll NuGet\Templates\Wolfpack.WebServices\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.dll NuGet\Templates\Wolfpack.WebServices\lib\net40

rem WebServices Client
del NuGet\Templates\Wolfpack.WebServices.Client\lib\net40\delete.me
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.WebServices.Client.dll NuGet\Templates\Wolfpack.WebServices.Client\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.WebServices.Interfaces.dll NuGet\Templates\Wolfpack.WebServices.Client\lib\net40
copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.dll NuGet\Templates\Wolfpack.WebServices.Client\lib\net40


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
rem del NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40\delete.me
rem copy Wolfpack.Contrib.BuildAnalytics\bin\debug\Wolfpack.Contrib.BuildAnalytics.dll NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40
rem copy Wolfpack.Contrib.BuildAnalytics\bin\debug\Wolfpack.Contrib.BuildAnalytics.pdb NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40
rem copy Wolfpack.Contrib.BuildAnalytics\bin\debug\Sharp2City.dll NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40
rem copy Wolfpack.Contrib.BuildAnalytics\bin\debug\Sharp2City.pdb NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\lib\net40

rem Core.Testing
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\delete.me
copy Wolfpack.Core.Testing\bin\debug\*.* NuGet\Templates\Wolfpack.Core.Testing\lib\net40
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\wolfpack.agent.exe.config
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\FluentIO.*
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\FluentAssertions.*
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\Magnum.*
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\StoryQ*.*
del NuGet\Templates\Wolfpack.Core.Testing\lib\net40\ServiceStack*.*

rem Wolfpack (+Sidewinder)
del NuGet\Templates\Wolfpack\tools\delete.me
del NuGet\Templates\Wolfpack\lib\net40\delete.me
del NuGet\Templates\Wolfpack\content\config\delete.me
del NuGet\Templates\Wolfpack\content\growl\delete.me
copy Wolfpack.Agent\bin\debug\*.* NuGet\Templates\Wolfpack\lib\net40
del NuGet\Templates\Wolfpack\lib\net40\*.xml
del NuGet\Templates\Wolfpack\lib\net40\*.pdb
move NuGet\Templates\Wolfpack\lib\net40\wolfpack.agent.exe.config NuGet\Templates\Wolfpack\content
xcopy Wolfpack.Agent\bin\debug\config\*.* NuGet\Templates\Wolfpack\content\config /Y /R /I /E
xcopy Wolfpack.Agent\bin\debug\content\*.* NuGet\Templates\Wolfpack\content\content /Y /R /I /E
xcopy Wolfpack.Agent\bin\debug\views\*.* NuGet\Templates\Wolfpack\content\views /Y /R /I /E
xcopy Wolfpack.Agent\bin\debug\growl\*.* NuGet\Templates\Wolfpack\content\growl /Y /R /I /E


popd
pushd ..\
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.WebServices\Wolfpack.WebServices.nuspec -version %NuGetVersion% -OutputDirectory Releases\v%1
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.WebServices.Client\Wolfpack.WebServices.Client.nuspec -version %NuGetVersion% -OutputDirectory Releases\v%1
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Publisher\Wolfpack.Publisher.nuspec -version %NuGetVersion% -OutputDirectory Releases\v%1
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.HealthCheck\Wolfpack.HealthCheck.nuspec -version %NuGetVersion% -OutputDirectory Releases\v%1 
rem Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\Wolfpack.Contrib.BuildAnalytics.nuspec -version %1 -OutputDirectory Releases\v%1 
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack\Wolfpack.nuspec -version %NuGetVersion% -OutputDirectory Releases\v%1 
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Core.Testing\Wolfpack.Core.Testing.nuspec -version %NuGetVersion% -OutputDirectory Releases\v%1 
popd

