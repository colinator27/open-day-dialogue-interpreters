/// @param buff

var buff = argument0;

var opcode = __odd_buffer_read(buff, buffer_u8);
var operand1 = undefined, operand2 = undefined;
switch(opcode)
{
	case odd_opcode.Label:
	case odd_opcode.Push:
	case odd_opcode.Convert:
	case odd_opcode.Jump:
	case odd_opcode.TextRun:
	case odd_opcode.CommandRun:
	case odd_opcode.SetVariable:
	case odd_opcode.CallFunction:
	case odd_opcode.JumpTrue:
	case odd_opcode.JumpFalse:
	case odd_opcode.ChoiceSelection:
	case odd_opcode.DebugLine:
	case odd_opcode.MakeArray:
	case odd_opcode.PushVariable:
		operand1 = __odd_buffer_read(buff, buffer_u32);
		break;
	case odd_opcode.Choice:
	case odd_opcode.ChoiceTrue:
		operand1 = __odd_buffer_read(buff, buffer_u32);
		operand2 = __odd_buffer_read(buff, buffer_u32);
		break;
	default:
		break;
}

return odd_create_instruction(opcode, operand1, operand2);