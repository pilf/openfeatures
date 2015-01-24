@echo Building Consumer Web with MSBuild
"C:\Windows\Microsoft.NET\Framework\v4.0.30319"\msbuild "..\..\ConsumerWeb\FrontEnd\solution\ot.sln" /t:build
goto CONSUMER_ASPNET_COMPILER

:CONSUMER_ASPNET_COMPILER
@echo Test Compilation Of Consumer Web
%WINDIR%\microsoft.NET\framework\v2.0.50727\aspnet_compiler -f -u -p ..\..\ConsumerWeb\FrontEnd -v / c:\temp\compile\consumer
@echo Consumer Web Build Successful
goto BUILD_CHARM

:BUILD_CHARM
@echo Building CHARM with MSBuild
"C:\Windows\Microsoft.NET\Framework\v4.0.30319"\msbuild "..\..\CHARM\FrontEnd\CHARM.sln" /t:build
goto CHARM_ASPNET_COMPILER

:CHARM_ASPNET_COMPILER
@echo Test Compilation Of CHARM
%WINDIR%\microsoft.NET\framework\v2.0.50727\aspnet_compiler -f -u -p ..\..\CHARM\FrontEnd -v / c:\temp\compile\charm
@echo CHARM Build Successful
goto SUCCESSFUL

@ECHO *****************************************************************
@ECHO ********************** MERGE UNSUCCESSFUL ***********************
@ECHO *****************************************************************
GOTO CLEANUP

:SUCCESSFUL
@ECHO *****************************************************************
@ECHO *********************** MERGE SUCCESSFUL ************************
@ECHO *****************************************************************
GOTO CLEANUP

:CLEANUP
RMDIR c:\temp\compile /s /q
pause
