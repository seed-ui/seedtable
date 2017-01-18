#!/bin/sh
MONO=mono
if [ `uname -s` = "Darwin" ];then
	MONO="mono --arch=64" 
fi
$MONO bin/Debug/XmSeedtable.exe
