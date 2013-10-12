@setlocal enableextensions enabledelayedexpansion
@echo off
for /f %%i in ('tasklist /FI "IMAGENAME eq RiftVRPN.exe" /FO CSV') do set tasks=%%i
set t1=x%tasks:RiftVRPN=%
set t2=x%tasks%
if not t1==t2 goto started

:notstarted
echo "Starting VRPN"

:started
echo "VRPN Already Started"