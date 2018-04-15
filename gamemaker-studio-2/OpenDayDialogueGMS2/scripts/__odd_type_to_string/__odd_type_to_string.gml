var type = argument0;

switch(type)
{
	case odd_type_array:
		return "Array";
		break;
	case odd_type_boolean:
		return "Boolean";
		break;
	case odd_type_double:
		return "Double";
		break;
	case odd_type_int32:
		return "Int32";
		break;
	case odd_type_rawidentifier:
		return "RawIdentifier";
		break;
	case odd_type_string:
		return "String";
		break;
	case odd_type_undefined:
		return "Undefined";
		break;
	case odd_type_variable:
		return "Variable";
		break;
}

return "Unknown";