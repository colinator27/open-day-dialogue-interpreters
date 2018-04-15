/// @description Adds/sets a function name paired with a handler script.
/// @param instance
/// @param function_name
/// @param handler

var instance = argument0;
var function_name = argument1;
var handler = argument2;

with (instance)
{
	functionHandlers[? function_name] = handler;
}