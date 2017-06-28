rmdir package\lib\net45 /s /q
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" ..\Fitbit\Fitbit.sln /p:Configuration=Release /m
mkdir .\package\lib\net45
copy ..\Fitbit.Portable\bin\Release\* .\package\lib\net45\
nuget pack .\package\Fitbit.Public.nuspec -properties buildOutputPath=\lib\net45