@echo off

echo Restoring packages for build tasks
dnu restore "%~dp0tasks" >nul

echo Running build tasks
dnx -p "%~dp0tasks" run