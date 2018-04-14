/// @param buff
/// @param binary

var buff = argument0;
var binary = argument1;

var type = __odd_buffer_read(buff, buffer_u16);
var value;
switch (type)
{
	case odd_type_double:
		value = __odd_buffer_read(buff, buffer_f64);
		break;
	case odd_type_int32:
		value = __odd_buffer_read(buff, buffer_s32);
		break;
	case odd_type_boolean:
		value = __odd_buffer_read(buff, buffer_bool);
		break;
	case odd_type_undefined:
		value = undefined;
		break;
	case odd_type_string:
	case odd_type_variable:
	case odd_type_rawidentifier:
		value = binary.stringTable[? __odd_buffer_read(buff, buffer_u32)];
		break;
	default:
		__odd_error("Invalid or corrupt file: value type invalid");
		break;
}

return odd_create_value(type, value);