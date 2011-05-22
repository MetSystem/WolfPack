pushd ..\
msbuild.exe Automation\msbuild\ReleaseSource.proj /p:pSolutionFile=Solutions\Wolfpack.sln;pProjectFolder=%cd%;pReleaseNumber=%1

