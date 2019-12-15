New-Item -Force -ItemType Directory releases
Compress-Archive -Force -Path ./seedtable-egui/bin/Desktop/seedtable-egui-*-win -DestinationPath releases/seedtable-egui-win.zip
