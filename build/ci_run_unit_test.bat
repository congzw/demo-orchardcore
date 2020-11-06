cd ../src

@REM call dotnet nuget enable source nuget.org
@REM call dotnet nuget locals all --list
@REM call dotnet nuget list source
call dotnet restore
call dotnet test --no-restore --logger trx

pause