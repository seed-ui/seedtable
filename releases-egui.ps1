New-Item -Force -ItemType Directory releases
Move-Item ./seedtable-egui/bin/Desktop/win-unpacked ./seedtable-egui/bin/Desktop/seedtable-egui-win
Compress-Archive -Force -Path ./seedtable-egui/bin/Desktop/seedtable-egui-win -DestinationPath releases/seedtable-egui-win.zip
