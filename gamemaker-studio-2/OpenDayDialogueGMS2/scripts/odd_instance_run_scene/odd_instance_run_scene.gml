/// @description Jumps to a scene's code and starts it. If the interpreter was paused, this will resume it.
/// @param vm
/// @param name The name of the scene to run

var vm = argument0;
var name = argument1;

with (vm)
{
	if (!ds_map_exists(binary.scenes, name))
	{
		__odd_error("Scene with name \"" + name + "\" does not exist.");
	}
	programCounter = binary.labels[? binary.scenes[? name]];
	currentScene = name;
	currentText = undefined;
	ds_list_clear(currentChoicesLabels);
	ds_list_clear(currentChoicesTexts);
	inChoice = false;
	ds_stack_clear(stack);
	debugCurrentLine = -1;
	pause = false;
}