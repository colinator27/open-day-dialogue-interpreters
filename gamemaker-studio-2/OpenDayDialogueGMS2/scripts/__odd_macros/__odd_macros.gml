#macro odd_type_double 0
#macro odd_type_int32 1
#macro odd_type_string 2
#macro odd_type_boolean 3
#macro odd_type_undefined 4
#macro odd_type_variable 5
#macro odd_type_rawidentifier 6
#macro odd_type_array 7

enum odd_opcode
{
    // Global opcodes
    Nop = 0,
    Label = 1, // operand1: label id

    // Stack operations
    Push = 160, // operand1: value id
    Pop = 161,
    Convert = 162, // operand1: short number of new type, of Value.Type
    Duplicate = 163, // Pushes a copy of the top value of the stack to the stack (duplicates).

    // Builtin operators
    BOAdd = 192,
    BOSub = 193,
    BOMul = 194,
    BODiv = 195,
    BOMod = 196,
    BOEqual = 197,
    BONotEqual = 198,
    BOGreater = 199,
    BOGreaterEqual = 200,
    BOLessThan = 201,
    BOLessThanEqual = 202,
    BONegate = 203,
    BOOr = 204,
    BOXor = 205,
    BOAnd = 206,
    BOInvert = 207,

    // Scene opcodes
    Jump = 176, // operand1: label id
    Exit = 177, // exits current scene
    TextRun = 178, // operand1: string id
    CommandRun = 179, // operand1: command id from command table
    SetVariable = 180, // operand1: variable name id. Uses top value on stack
    CallFunction = 181, // operand1: function name id. Used for non-builitin functions
    MakeArray = 188, // operand1: count of initial indices. Initializes array to a Value and pushes
    SetArrayIndex = 189, // First the array is pushed, then the value, then the index (3 items prepared on stack for this function). Index will be popped off.
    PushArrayIndex = 190, // Pushes the value of an index in array, using the values on the stack for array and index. On the top of stack should be index. The instruction pops off the original Value/array as well.
    PushVariable = 191, // operand1: variable name id. Pushes a variable's Value to the stack.

    // jump if top val of stack is true, pop it off
    JumpTrue = 182, // operand1: label id

    // jump if top val of stack is false, pop it off
    JumpFalse = 183, // operand1: label id

    // be ready to read choices
    BeginChoice = 184,

    // add choice
    Choice = 185, // operand1: string id, operand2: label id for the choice

    // if the top val of stack is true, pop it off, add choice
    ChoiceTrue = 186, // operand1: string id, operand2: label id for the choice

    // wait for user input, goes to one of the choices, and if no conditions match/there are none, go to
    // the end specified in operand1
    ChoiceSelection = 187,

    // set the debug line for interpreter, not always emitted (based on flag)
    DebugLine = 208 // operand1: line number
}