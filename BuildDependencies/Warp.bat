rename d3d9.dll.disabled d3d9.dll
for %%f in (*.exe) do (
	start %%f -hmd on
)
start OculusSupport\RiftServer\RiftVRPN.exe
cd OculusSupport\UIVAServer\
start UIVA_Server.exe