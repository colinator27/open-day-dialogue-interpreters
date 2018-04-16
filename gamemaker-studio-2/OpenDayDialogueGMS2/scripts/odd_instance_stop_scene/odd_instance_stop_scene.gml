/// @description Stops/exits the current scene, pausing the interpreter.
/// @param instance

var instance = argument0;

with (instance)
{
	currentText = undefined;
	currentScene = undefined;
	ds_list_clear(currentChoicesLabels);
	ds_list_clear(currentChoicesTexts);
	inChoice = false;
	ds_stack_clear(stack);
	debugCurrentLine = -1;
	pause = true;
}