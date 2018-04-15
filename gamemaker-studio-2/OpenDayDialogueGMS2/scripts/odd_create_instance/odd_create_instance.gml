/// @description Create an VM instance from a binary file instance
/// @param binary
/// @param variableStore_setVariable
/// @param variableStore_getVariable
/// @param variableStore_cleanUp
/// @param optional_handleText
/// @param optional_handleChoice
/// @param optional_textMainProcessor
/// @param optional_textChoiceProcessor
/// @param optional_textDefinitionProcessor

var instance = __odd_instance_create(odd_instance);

// Assign variables
instance.binary = argument[0];
instance.variableStore_setVariable = argument[1];
instance.variableStore_getVariable = argument[2];
instance.variableStore_cleanUp = argument[3];

if (argument_count > 4)
{
	instance.handleText = argument[4];
	if (argument_count > 5)
	{
		instance.handleChoice = argument[5];
		if (argument_count > 6)
		{
			instance.textMainProcessor = argument[6];
			if (argument_count > 7)
			{
				instance.textChoiceProcessor = argument[7];
				if (argument_count > 8)
				{
					instance.textDefinitionProcessor = argument[8];
				}
			}
		}
	}
}

return instance;