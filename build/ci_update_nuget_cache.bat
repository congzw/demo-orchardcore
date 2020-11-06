cd ../src/NbSites.Web

@REM update share cim nuget cache dir, to speed up with cim_nuget_cache
call dotnet restore --source https://api.nuget.org/v3/index.json --packages \\192.168.1.236\share\cim\nuget_cache
call dotnet restore --source https://api.nuget.org/v3/index.json --packages \\192.168.1.236\share\cim\nuget_cache -r win-x64
call dotnet restore --source https://api.nuget.org/v3/index.json --packages \\192.168.1.236\share\cim\nuget_cache -r linux-x64

pause