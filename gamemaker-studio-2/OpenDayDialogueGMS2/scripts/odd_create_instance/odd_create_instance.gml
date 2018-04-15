/// @description Create an VM instance from a binary file instance
/// @param binary
/// @param variableStore_setVariable
/// @param variableStore_getVariable
/// @param variableStore_cleanUp

var vm = __odd_instance_create(odd_vm);

// Assign variables
vm.binary = argument[0];
vm.variableStore_setVariable = argument[1];
vm.variableStore_getVariable = argument[2];
vm.variableStore_cleanUp = argument[3];

if (argument_count > 4)
{
	vm.handleText = argument[4];
	if (argument_count > 5)
	{
		vm.handleChoice = argument[5];
		if (argument_count > 6)
		{
			vm.textMainProcessor = argument[6];
			if (argument_count > 7)
			{
				vm.textChoiceProcessor = argument[7];
				if (argument_count > 8)
				{
					vm.textDefinitionProcessor = argument[8];
				}
			}
		}
	}
}

return vm;