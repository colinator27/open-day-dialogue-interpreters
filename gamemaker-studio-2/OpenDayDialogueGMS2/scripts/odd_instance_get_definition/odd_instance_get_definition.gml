/// @description Gets a definition from the binary's definition table.
/// @param instance
/// @param key

var instance = argument0;
var key = argument1;

var val;
with (instance)
{
	val = binary.definitions[? key];
	if (textDefinitionProcessor != undefined && !is_undefined(val) && odd_value_type(val) == odd_type_string)
		val = odd_create_value(odd_type_string, script_execute(textDefinitionProcessor, odd_value_val(val)));
}

return val;