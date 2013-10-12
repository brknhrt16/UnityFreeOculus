rename d3d9.dll d3d9.dll.disabled
for %%f in (*.exe) do (
	start %%f -hmd off
	exit
)