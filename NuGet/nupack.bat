rmdir package\lib\net45 /s /q
"%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe" ..\Fitbit\Fitbit.sln /p:Configuration=Release /m
mkdir .\package\lib\net45
copy ..\Fitbit\bin\Release\* .\package\lib\net45\
nuget pack .\package\Fitbit.nuspec