call CopySourceToReleaseAndBuild.cmd %1
call CreateBinariesZip_NoMerge.cmd %1
call CreateNuGetPackages.cmd %1
rem call CopySourceToReleaseAndZip.cmd %1
