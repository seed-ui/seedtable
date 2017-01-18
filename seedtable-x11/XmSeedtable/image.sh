#!/bin/bash
for K in `find ../../seedtable-gui/Resources -type f`;do
	IN=$K
	OUT=`echo $K | sed 's/.*\///g' | sed 's/\.png/.xpm/g'`
	echo "$IN => $OUT"
	convert $IN "Resources/$OUT"
done
#convert ../../../seedtable-gui/Resources/excelToYaml.png Resources/excelToYaml.xpm
