dotnet publish seedtable -c Release /p:PublishProfile=.seedtable/Properties/PublishProfiles/win-x86.pubxml
dotnet publish seedtable -c Release /p:PublishProfile=.seedtable/Properties/PublishProfiles/linux-x64.pubxml
dotnet publish seedtable -c Release /p:PublishProfile=.seedtable/Properties/PublishProfiles/osx-x64.pubxml
dotnet publish seedtable -c Release /p:PublishProfile=.seedtable/Properties/PublishProfiles/portable.pubxml
dotnet publish seedtable-gui -c Release /p:PublishProfile=.seedtable-gui/Properties/PublishProfiles/win-x86.pubxml
dotnet publish seedtable-gui -c Release /p:PublishProfile=.seedtable-gui/Properties/PublishProfiles/portable.pubxml
