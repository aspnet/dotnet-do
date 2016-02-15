@echo off

setlocal

cd %~dp0

echo Restoring packages for build tasks
dotnet restore
if errorlevel 1 goto fail

echo Building build tasks
del /s /q "%~dp0tasks\bin\app"
dotnet build --framework dnxcore50 "%~dp0tasks" -o "%~dp0tasks\bin\app"
if errorlevel 1 goto fail

echo Running build tasks
%~dp0\tasks\bin\app\tasks.exe %*
if errorlevel 1 goto fail

echo Build completed
goto end

:fail
echo Build failed

:end