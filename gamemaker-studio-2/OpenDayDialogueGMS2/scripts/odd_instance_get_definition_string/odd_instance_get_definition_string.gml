/// @description Gets a definition from the binary's definition table.
/// @param vm
/// @param key The key for the definition

var vm = argument0;
var key = argument1;

var val = odd_instance_get_definition(vm, key);
if (is_undefined(val))
	return undefined;
if (odd_value_type(val) != odd_type_string)
	__odd_error("Definition is not a string");

return odd_value_val(val);