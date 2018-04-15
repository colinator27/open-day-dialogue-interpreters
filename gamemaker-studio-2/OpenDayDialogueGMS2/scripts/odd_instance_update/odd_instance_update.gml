/// @description Runs through as much code as it can until it exits or hits a pause.
/// @param instance

var instance = argument0;

with (instance)
{
	if (currentScene != undefined)
	{
		while (!pause)
		{
			odd_instance_run_instruction(instance, binary.instructions[programCounter++]);
		}
	}
}