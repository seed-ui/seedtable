New-Item -Force -ItemType Directory releases
Compress-Archive -Force -Path ./seedtable/bin/Release/netcoreapp3.1/publish/win-x86/seedtable.exe -DestinationPath releases/seedtable.zip
Compress-Archive -Force -Path ./seedtable/bin/Release/netcoreapp3.1/publish/linux-x64/seedtable -DestinationPath releases/seedtable-linux.zip
Compress-Archive -Force -Path ./seedtable/bin/Release/netcoreapp3.1/publish/osx-x64/seedtable -DestinationPath releases/seedtable-osx.zip
Compress-Archive -Force -Path ./seedtable/bin/Release/netcoreapp3.1/publish/portable -DestinationPath releases/seedtable-need-runtime.zip
Compress-Archive -Force -Path ./seedtable-gui/bin/Release/netcoreapp3.1/publish/win-x86/seedtable-gui.exe -DestinationPath releases/seedtable-gui.zip
Compress-Archive -Force -Path ./seedtable-gui/bin/Release/netcoreapp3.1/publish/portable -DestinationPath releases/seedtable-gui-need-runtime.zip
