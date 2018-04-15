/// @description Converts a Value to another type.
/// @param value
/// @param type

var oldValue = argument0;
var newType = argument1;

var oldValueType = odd_value_type(oldValue);
var oldValueVal = odd_value_val(oldValue);

if (oldValueType == newType)
	return oldValue;
	
switch (oldValueType)
{
	case odd_type_double:
		switch (newType)
		{
			case odd_type_int32:
				return odd_create_value(newType, round(oldValueVal));
				break;
			case odd_type_string:
				return odd_create_value(newType, string(oldValueVal));
				break;
			case odd_type_boolean:
				return odd_create_value(newType, (oldValueVal != 0));
				break;
		}
		break;
	case odd_type_int32:
		switch(newType)
		{
			case odd_type_double:
				return odd_create_value(newType, oldValueVal);
				break;
			case odd_type_string:
				return odd_create_value(newType, string(oldValueVal));
				break;
			case odd_type_boolean:
				return odd_create_value(newType, (oldValueVal != 0));
				break;
		}
		break;
	case odd_type_string:
		switch(newType)
		{
			case odd_type_double:
				return odd_create_value(newType, real(oldValueVal));
				break;
			case odd_type_int32:
				return odd_create_value(newType, round(real(oldValueVal)));
				break;
			case odd_type_boolean:
				return odd_create_value(newType, (oldValueVal != ""));
				break;
		}
		break;
	case odd_type_boolean:
		switch(newType)
		{
			case odd_type_double:
				var b = 0;
				if (oldValueVal)
					b = 1;
				return odd_create_value(newType, b);
				break;
			case odd_type_int32:
				var b = 0;
				if (oldValueVal)
					b = 1;
				return odd_create_value(newType, b);
				break;
			case odd_type_string:
				var b = "false";
				if (oldValueVal)
					b = "true";
				return odd_create_value(newType, b);
				break;
		}
		break;
	case odd_type_undefined:
		switch(newType)
		{
			case odd_type_string:
				return odd_create_value(newType, "undefined");
				break;
		}
		break;
}

__odd_error("Cannot convert type " + 
				__odd_type_to_string(oldValueType) + 
				" to " + 
				__odd_type_to_string(newType) + 
				".");