mkdir -p releases
cd ./seedtable-egui/bin/Desktop
mv linux-unpacked/electron.net.host linux-unpacked/seedtable-egui
mv linux-unpacked seedtable-egui-linux
zip -r -9 ../../../releases/seedtable-egui-linux.zip seedtable-egui-linux
