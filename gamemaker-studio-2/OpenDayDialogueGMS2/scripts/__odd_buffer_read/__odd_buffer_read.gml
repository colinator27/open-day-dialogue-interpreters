/// @param buffer
/// @param type

gml_pragma("forceinline");

var length = 1;
switch (argument1)
{
	case buffer_u16:
	case buffer_s16:
		length = 2;
		break;
	case buffer_u32:
	case buffer_s32:
	case buffer_f32:
		length = 4;
		break;
	case buffer_u64:
	case buffer_f64:
		length = 8;
		break;
}
__odd_assert((buffer_tell(argument0) + length) <= buffer_get_size(argument0), "Invalid or corrupt file: unexpected end of file.");

return buffer_read(argument0, argument1);