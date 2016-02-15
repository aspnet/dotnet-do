@echo off

setlocal

cd %~dp0

echo Restoring packages for build tasks
dotnet restore
if errorlevel 1 goto fail

echo Building build tasks
dotnet build --framework dnxcore50 "%~dp0tasks"
if errorlevel 1 goto fail

echo Running build tasks
%~dp0\tasks\bin\Debug\dnxcore50\tasks.exe %*
if errorlevel 1 goto fail

echo Build completed
goto end

:fail
echo Build failed

:end