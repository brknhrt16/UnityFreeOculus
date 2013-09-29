rename d3d9.dll.disabled d3d9.dll
for %%f in (*.exe) do (
	start %%f -hmd on
	exit
)