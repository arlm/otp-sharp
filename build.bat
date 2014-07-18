@echo Off
set config=%1

if "%config%" == "" (
  set config=release
)


"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" Build.proj /p:Configuration="%config%" /p:Platform="Any CPU" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Detailed /nr:false
pause