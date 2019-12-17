cd seedtable-egui
electronize build /target linux
cd obj/desktop/linux
npm install electron@7.1.2
npx electron-builder . --config=./bin/electron-builder.json --linux --x64 -c.electronVersion=7.1.2
cd ../../..
cd ..
