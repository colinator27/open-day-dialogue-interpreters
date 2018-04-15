/// @description Runs through as much code as it can until it exits or hits a pause.
/// @param vm

var vm = argument0;

with (vm)
{
	if (currentScene != undefined)
	{
		while (!pause)
		{
			odd_instance_run_instruction(vm, binary.instructions[programCounter++]);
		}
	}
}