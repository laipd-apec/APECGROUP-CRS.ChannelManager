@ECHO OFF

set COMMAND=%1

for /D %%I in ("CRS.ChannelManager") do set ApiDirName=%%~nxI
echo %ApiDirName%

for /D %%I in ("*.Domain") do set InfrastructureDirName=%%~nxI
echo %InfrastructureDirName%

if "%COMMAND%"=="" (
SET /P COMMAND="Choose dotnet ef command (add, remove, update, list, revert): "
)

2>NUL CALL :CASE_%COMMAND%
IF ERRORLEVEL 1 CALL :DEFAULT_CASE # If label doesn't exist

echo Done.
exit /B

:CASE_add
  cd ./%InfrastructureDirName%
  SET /P MIGRATION="Name of migration: "
  dotnet ef migrations add %MIGRATION% --startup-project "../CRS.ChannelManager/CRS.ChannelManager.csproj"
  cd ..
  GOTO END_CASE
:CASE_remove
  cd ./%InfrastructureDirName%
  dotnet ef migrations remove --startup-project "../CRS.ChannelManager/CRS.ChannelManager.csproj"
  cd ..
  GOTO END_CASE
:CASE_update
  cd ./%InfrastructureDirName%
  dotnet ef database update --startup-project "../CRS.ChannelManager/CRS.ChannelManager.csproj"
  cd ..
  GOTO END_CASE
:CASE_revert
  cd ./%InfrastructureDirName%
  SET /P MIGRATION_REVERT="Name of previous migration: "
  dotnet ef database update "%MIGRATION_REVERT%" --startup-project "../CRS.ChannelManager/CRS.ChannelManager.csproj"
  cd ..
  GOTO END_CASE
:CASE_list
  cd ./%InfrastructureDirName%
  dotnet ef migrations list --startup-project "../CRS.ChannelManager/CRS.ChannelManager.csproj"
  cd ..
  GOTO END_CASE
:DEFAULT_CASE
  ECHO Unknown command "%COMMAND%"
  GOTO END_CASE
:END_CASE
  echo "end run migration"
  pause
  VER > NUL
  GOTO :EOF