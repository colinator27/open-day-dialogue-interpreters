/// @description Selects a choice and branches to its code. If the interpreter is paused, this will resume it.
/// @param vm
/// @param index The index of the selection

var vm = argument0;
var index = argument1;

with(vm)
{
	if (!inChoice)
		return;
	inChoice = false;
	pause = false;
	currentText = undefined;
	programCounter = binary.labels[? currentChoicesLabels[| index]];
	ds_list_clear(currentChoicesLabels);
	ds_list_clear(currentChoicesTexts);
}