/// @param buffer
/// @param length

var str = "";
for (var i = 0; i < argument1; i++)
{
	str += chr(buffer_read(argument0, buffer_u8));
}
return str;