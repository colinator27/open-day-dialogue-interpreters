/// @description Adds/sets a command name paired with a handler script.
/// @param instance
/// @param command_name
/// @param handler

var instance = argument0;
var command_name = argument1;
var handler = argument2;

with (instance)
{
	commandHandlers[? command_name] = handler;
}