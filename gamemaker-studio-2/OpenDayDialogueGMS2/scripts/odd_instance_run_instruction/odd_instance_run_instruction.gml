/// @description Runs an instruction on the interpreter.
/// @param instance
/// @param inst The instruction to run

var instance = argument0;
var inst = argument1;

with (instance)
{
	switch (odd_instruction_opcode(inst))
	{
		case odd_opcode.Push:
			ds_stack_push(stack, binary.valueTable[? odd_instruction_op1(inst)]);
			break;
		case odd_opcode.Pop:
			ds_stack_pop(stack);
			break;
		case odd_opcode.Convert:
			var oldValue = ds_stack_pop(stack);
			var newType = odd_instruction_op1(inst);
			ds_stack_push(stack, odd_value_convert(oldValue, newType));
			break;
		case odd_opcode.Duplicate:
			ds_stack_push(stack, ds_stack_top(stack));
			break;
		case odd_opcode.BOAdd:
			var val2 = ds_stack_pop(stack);
			var val1 = ds_stack_pop(stack);
			
			var val1type = odd_value_type(val1);
			var val2type = odd_value_type(val2);
			
			if ((val1type == odd_type_string) ||
				(val1type == odd_type_double && val2type == odd_type_int32))
			{
				// Convert second number to first
				ds_stack_push(__odd_value_add(val1, odd_value_convert(val2, val1type)));
			} else
			{
				// Convert first number to second
				ds_stack_push(__odd_value_add(odd_value_convert(val1, val2type), val2));
			}
			break;
		case odd_opcode.BOSub:
			var val2 = ds_stack_pop(stack);
			var val1 = ds_stack_pop(stack);
			
			var val1type = odd_value_type(val1);
			var val2type = odd_value_type(val2);
			
			if (val1type == odd_type_double && val2type == odd_type_int32)
			{
				// Convert second number to first
				ds_stack_push(__odd_value_sub(val1, odd_value_convert(val2, val1type)));
			} else
			{
				// Convert first number to second
				ds_stack_push(__odd_value_sub(odd_value_convert(val1, val2type), val2));
			}
			break;
		case odd_opcode.BOMod:
			var val2 = ds_stack_pop(stack);
			var val1 = ds_stack_pop(stack);
			
			var val1type = odd_value_type(val1);
			var val2type = odd_value_type(val2);
			
			if (val1type == odd_type_double && val2type == odd_type_int32)
			{
				// Convert second number to first
				ds_stack_push(__odd_value_mod(val1, odd_value_convert(val2, val1type)));
			} else
			{
				// Convert first number to second
				ds_stack_push(__odd_value_mod(odd_value_convert(val1, val2type), val2));
			}
			break;
		case odd_opcode.BOEqual:
			var val2 = ds_stack_pop(stack);
			var val1 = ds_stack_pop(stack);
			
			var val1type = odd_value_type(val1);
			var val2type = odd_value_type(val2);
			
			if (val1type == odd_type_double && val2type == odd_type_int32)
			{
				// Convert second number to first
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_equal(val1, odd_value_convert(val2, val1type))));
			} else
			{
				// Convert first number to second
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_equal(odd_value_convert(val1, val2type), val2)));
			}
			break;
		case odd_opcode.BONotEqual:
			var val2 = ds_stack_pop(stack);
			var val1 = ds_stack_pop(stack);
			
			var val1type = odd_value_type(val1);
			var val2type = odd_value_type(val2);
			
			if (val1type == odd_type_double && val2type == odd_type_int32)
			{
				// Convert second number to first
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_not_equal(val1, odd_value_convert(val2, val1type))));
			} else
			{
				// Convert first number to second
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_not_equal(odd_value_convert(val1, val2type), val2)));
			}
			break;
		case odd_opcode.BOGreater:
			var val2 = ds_stack_pop(stack);
			var val1 = ds_stack_pop(stack);
			
			var val1type = odd_value_type(val1);
			var val2type = odd_value_type(val2);
			
			if (val1type == odd_type_double && val2type == odd_type_int32)
			{
				// Convert second number to first
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_greater(val1, odd_value_convert(val2, val1type))));
			} else
			{
				// Convert first number to second
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_greater(odd_value_convert(val1, val2type), val2)));
			}
			break;
		case odd_opcode.BOGreaterEqual:
			var val2 = ds_stack_pop(stack);
			var val1 = ds_stack_pop(stack);
			
			var val1type = odd_value_type(val1);
			var val2type = odd_value_type(val2);
			
			if (val1type == odd_type_double && val2type == odd_type_int32)
			{
				// Convert second number to first
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_greater_equal(val1, odd_value_convert(val2, val1type))));
			} else
			{
				// Convert first number to second
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_greater_equal(odd_value_convert(val1, val2type), val2)));
			}
			break;
		case odd_opcode.BOLessThan:
			var val2 = ds_stack_pop(stack);
			var val1 = ds_stack_pop(stack);
			
			var val1type = odd_value_type(val1);
			var val2type = odd_value_type(val2);
			
			if (val1type == odd_type_double && val2type == odd_type_int32)
			{
				// Convert second number to first
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_less(val1, odd_value_convert(val2, val1type))));
			} else
			{
				// Convert first number to second
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_less(odd_value_convert(val1, val2type), val2)));
			}
			break;
		case odd_opcode.BOLessThanEqual:
			var val2 = ds_stack_pop(stack);
			var val1 = ds_stack_pop(stack);
			
			var val1type = odd_value_type(val1);
			var val2type = odd_value_type(val2);
			
			if (val1type == odd_type_double && val2type == odd_type_int32)
			{
				// Convert second number to first
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_less_equal(val1, odd_value_convert(val2, val1type))));
			} else
			{
				// Convert first number to second
				ds_stack_push(odd_create_value(odd_type_boolean, __odd_value_is_less_equal(odd_value_convert(val1, val2type), val2)));
			}
			break;
		case odd_opcode.BONegate:
			var val = ds_stack_pop(stack);
			switch(odd_value_type(val))
			{
				case odd_type_double:
					ds_stack_push(odd_create_value(odd_type_double, -odd_value_val(val)));
					break;
				case odd_type_int32:
					ds_stack_push(odd_create_value(odd_type_int32, -odd_value_val(val)));
					break;
				default:
					__odd_error("Cannot negate type " + __odd_type_to_string(odd_value_type(val)) + ".");
					break;
			}
			break;
		case odd_opcode.BOInvert:
			var val = ds_stack_pop(stack);
			switch(odd_value_type(val))
			{
				case odd_type_boolean:
					ds_stack_push(odd_create_value(odd_type_boolean, !odd_value_val(val)));
					break;
				default:
					__odd_error("Cannot invert type " + __odd_type_to_string(odd_value_type(val)) + ".");
					break;
			}
			break;
		case odd_opcode.BOOr:
			var val2 = odd_value_convert(ds_stack_pop(stack), odd_type_boolean);
			var val1 = odd_value_convert(ds_stack_pop(stack), odd_type_boolean);
			ds_stack_push(stack, odd_create_value(odd_type_boolean, (odd_value_val(val1) || odd_value_val(val2))));
			break;
		case odd_opcode.BOAnd:
			var val2 = odd_value_convert(ds_stack_pop(stack), odd_type_boolean);
			var val1 = odd_value_convert(ds_stack_pop(stack), odd_type_boolean);
			ds_stack_push(stack, odd_create_value(odd_type_boolean, (odd_value_val(val1) && odd_value_val(val2))));
			break;
		case odd_opcode.BOXor:
			var val2 = odd_value_convert(ds_stack_pop(stack), odd_type_boolean);
			var val1 = odd_value_convert(ds_stack_pop(stack), odd_type_boolean);
			ds_stack_push(stack, odd_create_value(odd_type_boolean, (odd_value_val(val1) != odd_value_val(val2))));
			break;
		case odd_opcode.Jump:
			programCounter = binary.labels[? odd_instruction_op1(inst)];
			break;
		case odd_opcode.Exit:
			currentScene = undefined;
			currentText = undefined;
			ds_list_clear(currentChoicesLabels);
			ds_list_clear(currentChoicesTexts);
			inChoice = false;
			ds_stack_clear(stack);
			pause = true;
			break;
		case odd_opcode.TextRun:
			currentText = binary.stringTable[? odd_instruction_op1(inst)];
			pause = true;
			if (textMainProcessor != undefined)
				currentText = script_execute(textMainProcessor, currentText);
			if (handleText != undefined)
				script_execute(handleText, currentText);
			break;
		case odd_opcode.CommandRun:
			var cmd = binary.commands[? odd_instruction_op1(inst)];
			var name = odd_command_name(cmd);
			var args = odd_command_args(cmd);
			if (!ds_map_exists(commandHandlers, name))
				__odd_error("Undefined command with name \"" + name + "\".");
			script_execute(commandHandlers[? name], args);
			break;
		case odd_opcode.SetVariable:
			script_execute(variableStore_setVariable, binary.stringTable[? odd_instruction_op1(inst)], ds_stack_pop(stack));
			break;
		case odd_opcode.CallFunction:
			var name = binary.stringTable[? odd_instruction_op1(inst)];
			if (!ds_map_exists(functionHandlers, name))
				__odd_error("Undefined function with name \"" + name + "\".");
			var argCount = odd_value_val(odd_value_convert(ds_stack_pop(stack), odd_type_int32));
			var args = array_create(argCount);
			for (var i = 0; i < argCount; i++)
			{
				args[i] = ds_stack_pop(stack);
			}
			script_execute(functionHandlers[? name], args);
			break;
		case odd_opcode.JumpTrue:
			if (odd_value_val(odd_value_convert(ds_stack_pop(stack), odd_type_boolean)) == true)
			{
				programCounter = binary.labels[? odd_instruction_op1(inst)];
			}
			break;
		case odd_opcode.JumpFalse:
			if (odd_value_val(odd_value_convert(ds_stack_pop(stack), odd_type_boolean)) == false)
			{
				programCounter = binary.labels[? odd_instruction_op1(inst)];
			}
			break;
		case odd_opcode.BeginChoice:
			ds_list_clear(currentChoicesLabels);
			ds_list_clear(currentChoicesTexts);
			inChoice = true;
			break;
		case odd_opcode.Choice:
			var text = binary.stringTable[? odd_instruction_op1(inst)];
			if (textChoiceProcessor != undefined)
				text = script_execute(textChoiceProcessor, text);
			ds_list_add(currentChoicesLabels, odd_instruction_op2(inst));
			ds_list_add(currentChoicesTexts, text);
			break;
		case odd_opcode.ChoiceTrue:
			if (odd_value_val(odd_value_convert(ds_stack_pop(stack), odd_type_boolean)) == true)
			{
				var text = binary.stringTable[? odd_instruction_op1(inst)];
				if (textChoiceProcessor != undefined)
					text = script_execute(textChoiceProcessor, text);
				ds_list_add(currentChoicesLabels, odd_instruction_op2(inst));
				ds_list_add(currentChoicesTexts, text);
			}
			break;
		case odd_opcode.ChoiceSelection:
			// Skip to after the choice if none exist/are available
			var size = ds_list_size(currentChoicesLabels);
			if (size == 0)
			{
				programCounter = binary.labels[? odd_instruction_op1(inst)];
				inChoice = false;
				break;
			}
			inChoice = true;
			pause = true;
			
			// Call the handler
			if (handleChoice != undefined)
			{
				var choices = array_create(size);
				for (var i = 0; i < size; i++)
				{
					choices[i] = currentChoicesTexts[| i];
				}
				script_execute(handleChoice, choices);
			}
			break;
		case odd_opcode.DebugLine:
			debugCurrentLine = odd_instruction_op1(inst);
			break;
		case odd_opcode.PushVariable:
			ds_stack_push(stack, script_execute(variableStore_getVariable, binary.stringTable[? odd_instruction_op1(inst)]));
			break;
		case odd_opcode.MakeArray:
			ds_stack_push(odd_create_value(odd_type_array, array_create(odd_instruction_op1(inst))));
			break;
		case odd_opcode.SetArrayIndex:
			var index = odd_value_val(odd_value_convert(ds_stack_pop(stack), odd_type_int32));
			var val = ds_stack_pop(stack);
			var array = ds_stack_pop(stack);
			if (odd_value_type(array) != odd_type_array)
				__odd_error("Cannot perform array operations on a non-array Value");
			var realArray = odd_value_val(array);
			realArray[index] = val;
			ds_stack_push(stack, odd_create_value(odd_type_array, realArray));
			break;
		case odd_opcode.PushArrayIndex:
			var index = odd_value_val(odd_value_convert(ds_stack_pop(stack), odd_type_int32));
			var array = ds_stack_pop(stack);
			if (odd_value_type(array) != odd_type_array)
				__odd_error("Cannot perform array operations on a non-array Value");
			var realArray = odd_value_val(array);
			ds_stack_push(stack, realArray[index]);
			break;
	}
}