../../seedtable/bin/Debug/seedtable.exe from seedtable_example.xlsx -o seeds --yaml-columns data_yaml -e EPPlus
../../seedtable/bin/Debug/seedtable.exe to seedtable_example.xlsx -s seeds_to -o seeds_to_union --yaml-columns data_yaml -e EPPlus --calc-formulas
../../seedtable/bin/Debug/seedtable.exe from seedtable_example.xlsx -i seeds_to_union -o seeds_to_union --yaml-columns data_yaml -e EPPlus
../../seedtable/bin/Debug/seedtable.exe to seedtable_example.xlsx -s seeds_to -o seeds_to_delete --yaml-columns data_yaml -e EPPlus --delete --calc-formulas
../../seedtable/bin/Debug/seedtable.exe from seedtable_example.xlsx -i seeds_to_delete -o seeds_to_delete --yaml-columns data_yaml -e EPPlus
rm ./seeds_to_union/seedtable_example.xlsx
rm ./seeds_to_delete/seedtable_example.xlsx
