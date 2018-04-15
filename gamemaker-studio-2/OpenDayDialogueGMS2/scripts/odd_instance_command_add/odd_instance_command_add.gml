/// @description Adds/sets a command name paired with a handler script.
/// @param vm
/// @param command_name
/// @param handler

var vm = argument0;
var command_name = argument1;
var handler = argument2;

with (vm)
{
	commandHandlers[? command_name] = handler;
}