@setlocal enableextensions enabledelayedexpansion
@echo off

rename d3d9.dll.disabled d3d9.dll
for %%f in (*.exe) do (
	start %%f -hmd on
)

for /f %%i in ('tasklist /FI "IMAGENAME eq RiftVRPN.exe" /FO CSV') do set tasks=%%i
set t1=x%tasks:"=%
set t2=%t1:RiftVRPN=%
if not ["%t1%"]==["%t2%"] goto started

:notstarted
echo Starting VRPN
cd OculusSupport\
start StartOculus.bat
cd ..
exit

:started
echo VRPN Already Started
exit