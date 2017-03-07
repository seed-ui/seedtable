#!/bin/sh
MONO=mono
if [ `uname -s` = "Darwin" ];then
	MONO="mono --arch=64" 
fi
$MONO --debug bin/Debug/XmSeedtable.exe
