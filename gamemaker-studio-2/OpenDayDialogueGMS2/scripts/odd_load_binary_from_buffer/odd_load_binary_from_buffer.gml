/// @description Load a binary "file" from a pre-existing buffer and create a reference for it in memory.
/// @param buffer

var buff = argument0;

// Make sure header is readable
__odd_assert(buffer_get_size(buff) >= 8, "Invalid or corrupt file: file size too small.");

// Read beginning of header
__odd_assert(__odd_buffer_read_string(buff, 4) == "OPDA", "Invalid or corrupt file: header mismatch.");

// Confirm this is the right format
__odd_assert(buffer_read(buff, buffer_u32) == odd_format_version(), "Invalid file: version of format is not compatible with this interpreter.");

// Create the instance
var bin = __odd_instance_create(odd_binary);

/// Begin reading contents! ///

// Read string table
var stringTableCount = __odd_buffer_read(buff, buffer_s32);
for (var i = 0; i < stringTableCount; i++)
{
	var key = __odd_buffer_read(buff, buffer_u32);
	var value = __odd_buffer_read(buff, buffer_string);
	bin.stringTable[? key] = value;
}

// Read value table
var valueTableCount = __odd_buffer_read(buff, buffer_s32);
for (var i = 0; i < valueTableCount; i++)
{
	var key = __odd_buffer_read(buff, buffer_u32);
	var value = __odd_buffer_read_value(buff, bin);
	bin.valueTable[? key] = value;
}

// Read definitions
var definitionCount = __odd_buffer_read(buff, buffer_s32);
for (var i = 0; i < definitionCount; i++)
{
	var key = bin.stringTable[? __odd_buffer_read(buff, buffer_u32)];
	var value = bin.valueTable[? __odd_buffer_read(buff, buffer_u32)];
	bin.definitions[? key] = value;
}

// Read commands
var commandCount = __odd_buffer_read(buff, buffer_s32);
for (var i = 0; i < commandCount; i++)
{
	var key = __odd_buffer_read(buff, buffer_u32);
	var name = bin.stringTable[? __odd_buffer_read(buff, buffer_u32)];
	var argCount = __odd_buffer_read(buff, buffer_s32);
	var args = array_create(argCount);
	for (var j = 0; j < argCount; j++)
	{
		args[j] = bin.valueTable[? __odd_buffer_read(buff, buffer_u32)];
	}
	bin.commands[? key] = odd_create_command(name, args);
}

// Read scenes
var sceneCount = __odd_buffer_read(buff, buffer_s32);
for (var i = 0; i < sceneCount; i++)
{
	var name = bin.stringTable[? __odd_buffer_read(buff, buffer_u32)];
	var label = __odd_buffer_read(buff, buffer_u32);
	bin.scenes[? name] = label;
}

// Read bytecode
var instructionCount = __odd_buffer_read(buff, buffer_s32);
bin.instructions = array_create(instructionCount);
for (var i = 0; i < instructionCount; i++)
{
	bin.instructions[i] = __odd_buffer_read_instruction(buff);
	if (odd_instruction_opcode(bin.instructions[i]) == odd_opcode.Label)
	{
		bin.labels[? odd_instruction_op1(bin.instructions[i])] = i;
	}
}

buffer_delete(buff);

return bin;