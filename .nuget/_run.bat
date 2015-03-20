"%~dp0NuGet.exe" pack "..\DNTProfiler.Core\DNTProfiler.EntityFramework.Core.csproj" -Prop Configuration=Release
"%~dp0NuGet.exe" pack "..\NHibernate\DNTProfiler.NHibernate.Core\DNTProfiler.NHibernate.Core.csproj" -Prop Configuration=Release
copy "%~dp0*.nupkg" "%localappdata%\NuGet\Cache"

pause