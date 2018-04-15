/// @description Jumps to a scene's code and starts it. If the interpreter was paused, this will resume it.
/// @param instance
/// @param name

var instance = argument0;
var name = argument1;

with (instance)
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