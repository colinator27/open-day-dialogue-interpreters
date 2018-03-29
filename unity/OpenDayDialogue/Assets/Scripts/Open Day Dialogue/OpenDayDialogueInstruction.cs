using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenDayDialogue
{
    public class Instruction
    {
        public enum Opcode
        {
            // Global opcodes
            Nop = 0x00,
            Label = 0x01, // operand1: label id

            // Stack operations
            Push = 0xA0, // operand1: value id
            Pop = 0xA1,
            Convert = 0xA2, // operand1: short number of new type, of Value.Type

            // Builtin operators
            BOAdd = 0xC0,
            BOSub = 0xC1,
            BOMul = 0xC2,
            BODiv = 0xC3,
            BOMod = 0xC4,
            BOEqual = 0xC5,
            BONotEqual = 0xC6,
            BOGreater = 0xC7,
            BOGreaterEqual = 0xC8,
            BOLessThan = 0xC9,
            BOLessThanEqual = 0xCA,
            BONegate = 0xCB,
            BOOr = 0xCC,
            BOXor = 0xCD,
            BOAnd = 0xCE,
            BOInvert = 0xCF,

            // Scene opcodes
            Jump = 0xB0, // operand1: label id
            Exit = 0xB1, // exits current scene
            TextRun = 0xB2, // operand1: string id
            CommandRun = 0xB3, // operand1: command id from command table
            SetVariable = 0xB4, // operand1: variable name id. Uses top value on stack
            CallFunction = 0xB5, // operand1: function name id. Used for non-builitin functions

            // jump if top val of stack is true, pop it off
            JumpTrue = 0xB6, // operand1: label id

            // jump if top val of stack is false, pop it off
            JumpFalse = 0xB7, // operand1: label id

            // be ready to read choices
            BeginChoice = 0xB8,

            // add choice
            Choice = 0xB9, // operand1: string id, operand2: label id for the choice

            // if the top val of stack is true, pop it off, add choice
            ChoiceTrue = 0xBA, // operand1: string id, operand2: label id for the choice

            // wait for user input, goes to one of the choices, and if no conditions match/there are none, go to
            // the end specified in operand1
            ChoiceSelection = 0xBB,

            // set the debug line for interpreter, not always emitted (based on flag)
            DebugLine = 0xD0 // operand1: line number
        }

        public Opcode opcode;
        public uint? operand1;
        public uint? operand2;
        public Binary b;

        public Instruction(OReader br, Binary b)
        {
            this.b = b;
            opcode = (Opcode)br.ReadByte();
            switch(opcode)
            {
                case Opcode.Label:
                case Opcode.Push:
                case Opcode.Convert:
                case Opcode.Jump:
                case Opcode.TextRun:
                case Opcode.CommandRun:
                case Opcode.SetVariable:
                case Opcode.CallFunction:
                case Opcode.JumpTrue:
                case Opcode.JumpFalse:
                case Opcode.ChoiceSelection:
                case Opcode.DebugLine:
                    operand1 = br.ReadUInt32();
                    break;
                case Opcode.Choice:
                case Opcode.ChoiceTrue:
                    operand1 = br.ReadUInt32();
                    operand2 = br.ReadUInt32();
                    break;
            }
        }
    }
}