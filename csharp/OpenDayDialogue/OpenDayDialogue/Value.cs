using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OpenDayDialogue
{
    public class Value
    {
        public enum Type
        {
            Double = 0,
            Int32 = 1,
            String = 2,
            Boolean = 3,
            Undefined = 4,
            Variable = 5,
            RawIdentifier = 6
        }

        public Type type;
        public double valueDouble;
        public int valueInt32;
        public string valueString;
        public bool valueBoolean;
        public string valueVariable;
        public string valueRawIdentifier;

        public Value()
        {
        }

        public Value(OReader br, Binary b)
        {
            type = (Value.Type)br.ReadUInt16();
            switch(type)
            {
                case Value.Type.Double:
                    valueDouble = br.ReadDouble();
                    break;
                case Value.Type.Int32:
                    valueInt32 = br.ReadInt32();
                    break;
                case Value.Type.String:
                    valueString = b.stringTable[br.ReadUInt32()];
                    break;
                case Value.Type.Boolean:
                    valueBoolean = (br.ReadByte() != 0) ? true : false;
                    break;
                case Value.Type.Variable:
                    valueVariable = b.stringTable[br.ReadUInt32()];
                    break;
                case Value.Type.RawIdentifier:
                    valueRawIdentifier = b.stringTable[br.ReadUInt32()];
                    break;
            }
        }

        public Value ConvertTo(Value.Type type, Interpreter interpreter = null)
        {
            if(this.type == type)
                return this;

            switch(this.type)
            {
                case Value.Type.Double:
                    switch(type)
                    {
                        case Value.Type.Int32:
                            return new Value() {
                                type = type,
                                valueInt32 = (int)(this.valueDouble)
                            };
                        case Value.Type.String:
                            return new Value() {
                                type = type,
                                valueString = this.valueDouble.ToString()
                            };
                    }
                    break;
                case Value.Type.Int32:
                    switch(type)
                    {
                        case Value.Type.Double:
                            return new Value() {
                                type = type,
                                valueDouble = (double)(this.valueInt32)
                            };
                        case Value.Type.String:
                            return new Value() {
                                type = type,
                                valueString = this.valueInt32.ToString()
                            };
                    }
                    break;
                case Value.Type.String:
                    switch(type)
                    {
                        case Value.Type.Double:
                            return new Value() {
                                type = type,
                                valueDouble = double.Parse(this.valueString)
                            };
                        case Value.Type.Int32:
                            return new Value() {
                                type = type,
                                valueInt32 = int.Parse(this.valueString)
                            };
                        case Value.Type.Boolean:
                            return new Value() {
                                type = type,
                                valueBoolean = (this.valueString != "")
                            };
                    }
                    break;
                case Value.Type.Boolean:
                    switch(type)
                    {
                        case Value.Type.Double:
                            return new Value() {
                                type = type,
                                valueDouble = (valueBoolean ? 1d : 0d)
                            };
                        case Value.Type.Int32:
                            return new Value() {
                                type = type,
                                valueInt32 = (valueBoolean ? 1 : 0)
                            };
                        case Value.Type.String:
                            return new Value() {
                                type = type,
                                valueString = (valueBoolean ? "true" : "false")
                            };
                    }
                    break;
                case Value.Type.Variable:
                    if(interpreter != null)
                    {
                        return interpreter.ResolveVariable(valueVariable).ConvertTo(type);
                    }
                    break;
                case Value.Type.Undefined:
                    switch(type)
                    {
                        case Value.Type.String:
                            return new Value() {
                                type = type,
                                valueString = "undefined"
                            };
                    }
                    break;
            }

            throw new OpenDayDialogueException(string.Format("Cannot convert type {0} to {1}.", this.type, type));
        }

        public static Value operator+(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) + b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return new Value() {
                        type = a.type,
                        valueDouble = a.valueDouble + b.valueDouble
                    };
                case Value.Type.Int32:
                    return new Value() {
                        type = a.type,
                        valueInt32 = a.valueInt32 + b.valueInt32
                    };
                case Value.Type.String:
                    return new Value() {
                        type = a.type,
                        valueString = a.valueString + b.valueString
                    };
            }
            throw new OpenDayDialogueException(string.Format("Cannot add with type {0}.", a.type));
        }

        public static Value operator-(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) - b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return new Value() {
                        type = a.type,
                        valueDouble = a.valueDouble - b.valueDouble
                    };
                case Value.Type.Int32:
                    return new Value() {
                        type = a.type,
                        valueInt32 = a.valueInt32 - b.valueInt32
                    };
            }
            throw new OpenDayDialogueException(string.Format("Cannot subtract with type {0}.", a.type));
        }

        public static Value operator*(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) * b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return new Value() {
                        type = a.type,
                        valueDouble = a.valueDouble * b.valueDouble
                    };
                case Value.Type.Int32:
                    return new Value() {
                        type = a.type,
                        valueInt32 = a.valueInt32 * b.valueInt32
                    };
            }
            throw new OpenDayDialogueException(string.Format("Cannot multiply with type {0}.", a.type));
        }

        public static Value operator/(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) / b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return new Value() {
                        type = a.type,
                        valueDouble = a.valueDouble / b.valueDouble
                    };
                case Value.Type.Int32:
                    return new Value() {
                        type = a.type,
                        valueDouble = (double)a.valueInt32 / (double)b.valueInt32
                    };
            }
            throw new OpenDayDialogueException(string.Format("Cannot divide with type {0}.", a.type));
        }

        private static double mod(double a, double b)
        {
            return a - (b * Math.Floor(a / b));
        }

        public static Value operator%(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) % b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return new Value() {
                        type = a.type,
                        valueDouble = mod(a.valueDouble, b.valueDouble)
                    };
                case Value.Type.Int32:
                    return new Value() {
                        type = a.type,
                        valueDouble = mod((double)a.valueInt32, (double)b.valueInt32)
                    };
            }
            throw new OpenDayDialogueException(string.Format("Cannot divide with type {0}.", a.type));
        }

        public static bool IsEqual(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) == b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return a.valueDouble == b.valueDouble;
                case Value.Type.Int32:
                    return a.valueInt32 == b.valueInt32;
                case Value.Type.String:
                    return a.valueString == b.valueString;
                case Value.Type.Boolean:
                    return a.valueBoolean == b.valueBoolean;
            }
            throw new OpenDayDialogueException(string.Format("Cannot check equality with type {0}.", a.type));
        }

        public static bool IsNotEqual(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) != b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return a.valueDouble != b.valueDouble;
                case Value.Type.Int32:
                    return a.valueInt32 != b.valueInt32;
                case Value.Type.String:
                    return a.valueString != b.valueString;
                case Value.Type.Boolean:
                    return a.valueBoolean != b.valueBoolean;
            }
            throw new OpenDayDialogueException(string.Format("Cannot check inequality with type {0}.", a.type));
        }

        public static bool operator>(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) > b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return a.valueDouble > b.valueDouble;
                case Value.Type.Int32:
                    return a.valueInt32 > b.valueInt32;
            }
            throw new OpenDayDialogueException(string.Format("Cannot use operator > with type {0}.", a.type));
        }

        public static bool operator<(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) < b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return a.valueDouble < b.valueDouble;
                case Value.Type.Int32:
                    return a.valueInt32 < b.valueInt32;
            }
            throw new OpenDayDialogueException(string.Format("Cannot use operator < with type {0}.", a.type));
        }

        public static bool operator>=(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) >= b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return a.valueDouble >= b.valueDouble;
                case Value.Type.Int32:
                    return a.valueInt32 >= b.valueInt32;
            }
            throw new OpenDayDialogueException(string.Format("Cannot use operator >= with type {0}.", a.type));
        }

        public static bool operator<=(Value a, Value b)
        {
            if(a.type != b.type)
                return a.ConvertTo(b.type, null) <= b;
            switch(a.type)
            {
                case Value.Type.Double:
                    return a.valueDouble <= b.valueDouble;
                case Value.Type.Int32:
                    return a.valueInt32 <= b.valueInt32;
            }
            throw new OpenDayDialogueException(string.Format("Cannot use operator <= with type {0}.", a.type));
        }

    }
}