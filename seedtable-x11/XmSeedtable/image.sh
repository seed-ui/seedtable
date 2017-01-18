#!/bin/bash
for K in `find ../../seedtable-gui/Resources -type f`;do
	IN=$K
	OUT=`echo $K | sed 's/.*\///g' | sed 's/\.png/.xpm/g'`
	echo "$IN => $OUT"
  convert $IN "(" +clone -alpha opaque -fill gray77 -colorize 100% ")" +swap -geometry +0+0 -compose Over -composite -alpha off "`dirname $0`/Resources/$OUT"
done
#convert ../../../seedtable-gui/Resources/excelToYaml.png Resources/excelToYaml.xpm
