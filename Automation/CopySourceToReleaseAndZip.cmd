pushd ..\
msbuild.exe Automation\msbuild\ReleaseSource.proj /p:pSolutionFile=Solutions\Wolfpack.sln;pProjectFolder=%cd%;pReleaseNumber=%1 /t:ExecuteCopyAndZip
ren Releases\v%1\Source_v%1.zip Wolfpack_Source_v%1.zip

