@echo off
powershell -NoProfile -NoLogo -File %~dp0\build.ps1 %*
exit %errorlevel%
