/// @description Adds/sets a function name paired with a handler script.
/// @param vm
/// @param function_name
/// @param handler

var vm = argument0;
var function_name = argument1;
var handler = argument2;

with (vm)
{
	functionHandlers[? function_name] = handler;
}