var val1 = argument0;
var val2 = argument1;

var val1type = odd_value_type(val1);
var val2type = odd_value_type(val2);

if (val1type != val2type)
	return __odd_value_is_equal(odd_value_convert(val1, val2type), val2);
	
switch (val1type)
{
	case odd_type_double:
	case odd_type_int32:
	case odd_type_string:
	case odd_type_boolean:
		return (odd_value_val(val1) == odd_value_val(val2));
		break;
	case odd_type_undefined:
		return true;
		break;
}

__odd_error("Cannot check equality with type " + __odd_type_to_string(val1type) + ".");