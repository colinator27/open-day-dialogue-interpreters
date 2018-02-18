using System.Collections;
using System.Collections.Generic;

namespace OpenDayDialogue
{
	public class Interpreter
	{
		private struct DialogueChoice
		{
			public string text;
			public uint label;
		}

		public delegate string TextProcessor(string input);
		public delegate void HandleText(string text);
		public delegate void HandleChoice(IList<string> choices);

		public Binary binary;
		public VariableStore variableStore;
		public FunctionHandler functionHandler;
		public CommandHandler commandHandler;
		public HandleText handleText;
		public HandleChoice handleChoice;
		public TextProcessor textMainProcessor;
		public TextProcessor textChoiceProcessor;
		public TextProcessor textDefinitionProcessor;

		private int programCounter;
		private Stack<Value> stack;
		private string currentScene;
		private string currentText;
		private List<DialogueChoice> currentChoices;
		private bool inChoice;
		private bool pause;

		public Interpreter(Binary binary, VariableStore variableStore, FunctionHandler functionHandler, CommandHandler commandHandler, 
							HandleText handleText = null, HandleChoice handleChoice = null, TextProcessor textMainProcessor = null,
							TextProcessor textChoiceProcessor = null, TextProcessor textDefinitionProcessor = null)
		{
			this.binary = binary;
			this.variableStore = variableStore;
			this.functionHandler = functionHandler;
			this.commandHandler = commandHandler;
			this.handleText = handleText;
			this.handleChoice = handleChoice;
			this.textMainProcessor = textMainProcessor;
			this.textChoiceProcessor = textChoiceProcessor;
			this.textDefinitionProcessor = textDefinitionProcessor;

			programCounter = 0;
			stack = new Stack<Value>();
			currentScene = null;
			currentText = null;
			currentChoices = new List<DialogueChoice>();
			inChoice = false;
			pause = false;
		}

		public void SelectChoice(int index)
		{
			if(!inChoice)
				return;
			inChoice = false;
			pause = false;
			currentText = null;
			programCounter = binary.labels[currentChoices[index].label];
			currentChoices.Clear();
		}

		public void Pause()
		{
			pause = true;
		}

		public void Resume()
		{
			pause = false;
		}

		// Runs through as much code as it can until it exits or hits a pause
		public void Update()
		{
			if(currentScene != null)
			{
				while(!pause)
				{
					RunInstruction(binary.instructions[programCounter++]);
				}
			}
		}

		public void RunScene(string name)
		{
			if(!binary.scenes.ContainsKey(name))
				return;
			programCounter = binary.labels[binary.scenes[name]];
			currentScene = name;
			currentText = null;
			currentChoices.Clear();
			stack.Clear();
			pause = false;
		}

		public void StopScene()
		{
			currentText = null;
			currentScene = null;
			currentChoices.Clear();
			stack.Clear();
			pause = true;
		}

		public string GetDefinition(string key)
		{
			string value = binary.definitions[key];
			if(textDefinitionProcessor != null)
				value = textDefinitionProcessor(value);
			return value;
		}

		public Value ResolveVariable(string name)
		{
			return variableStore.GetVariable(name);
		}

		// Pops the value from the top of the stack, and resolves variables
		private Value PopValue()
		{
			Value v = stack.Pop();
			if(v.type == Value.Type.Variable)
				return ResolveVariable(v.valueVariable);
			else
				return v;
		}

		public void RunInstruction(Instruction inst)
		{
			switch(inst.opcode)
			{
				case Instruction.Opcode.Push:
					stack.Push(binary.valueTable[(uint)inst.operand1]);
					break;
				case Instruction.Opcode.Pop:
					stack.Pop();
					break;
				case Instruction.Opcode.Convert:
					{
						Value oldValue = PopValue();
						Value.Type newType = (Value.Type)((uint)inst.operand1);
						stack.Push(oldValue.ConvertTo(newType, this));
					}
					break;
				case Instruction.Opcode.BOAdd:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.String) ||
						   (val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(val1 + val2.ConvertTo(val1.type, this));
						} else
						{
							// Convert first number to second
							stack.Push(val1.ConvertTo(val2.type, this) + val2);
						}
					}
					break;
				case Instruction.Opcode.BOSub:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(val1 - val2.ConvertTo(val1.type, this));
						} else
						{
							// Convert first number to second
							stack.Push(val1.ConvertTo(val2.type, this) - val2);
						}
					}
					break;
				case Instruction.Opcode.BOMul:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(val1 * val2.ConvertTo(val1.type, this));
						} else
						{
							// Convert first number to second
							stack.Push(val1.ConvertTo(val2.type, this) * val2);
						}
					}
					break;
				case Instruction.Opcode.BODiv:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(val1 / val2.ConvertTo(val1.type, this));
						} else
						{
							// Convert first number to second
							stack.Push(val1.ConvertTo(val2.type, this) / val2);
						}
					}
					break;
				case Instruction.Opcode.BOMod:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(val1 % val2.ConvertTo(val1.type, this));
						} else
						{
							// Convert first number to second
							stack.Push(val1.ConvertTo(val2.type, this) % val2);
						}
					}
					break;
				case Instruction.Opcode.BOEqual:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = Value.IsEqual(val1, val2.ConvertTo(val1.type, this)) });
						} else
						{
							// Convert first number to second
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = Value.IsEqual(val1.ConvertTo(val2.type, this), val2) });
						}
					}
					break;
				case Instruction.Opcode.BONotEqual:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = Value.IsNotEqual(val1, val2.ConvertTo(val1.type, this)) });
						} else
						{
							// Convert first number to second
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = Value.IsNotEqual(val1.ConvertTo(val2.type, this), val2) });
						}
					}
					break;
				case Instruction.Opcode.BOGreater:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = (val1 > val2.ConvertTo(val1.type, this)) });
						} else
						{
							// Convert first number to second
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = (val1.ConvertTo(val2.type, this) > val2) });
						}
					}
					break;
				case Instruction.Opcode.BOGreaterEqual:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = (val1 >= val2.ConvertTo(val1.type, this)) });
						} else
						{
							// Convert first number to second
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = (val1.ConvertTo(val2.type, this) >= val2) });
						}
					}
					break;
				case Instruction.Opcode.BOLessThan:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = (val1 < val2.ConvertTo(val1.type, this)) });
						} else
						{
							// Convert first number to second
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = (val1.ConvertTo(val2.type, this) < val2) });
						}
					}
					break;
				case Instruction.Opcode.BOLessThanEqual:
					{
						Value val2 = PopValue();
						Value val1 = PopValue();

						if((val1.type == Value.Type.Double && val2.type == Value.Type.Int32))
						{
							// Convert second number to first
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = (val1 <= val2.ConvertTo(val1.type, this)) });
						} else
						{
							// Convert first number to second
							stack.Push(new Value(){ type = Value.Type.Boolean, valueBoolean = (val1.ConvertTo(val2.type, this) <= val2) });
						}
					}
					break;
				case Instruction.Opcode.BONegate:
					{
						Value v = PopValue();
						switch(v.type)
						{
							case Value.Type.Double:
								stack.Push(new Value() {
									type = Value.Type.Double,
									valueDouble = -v.valueDouble
								});
								break;
							case Value.Type.Int32:
								stack.Push(new Value() {
									type = Value.Type.Int32,
									valueDouble = -v.valueInt32
								});
								break;
							default:
								throw new OpenDayDialogueException(string.Format("Cannot negate type {0}.", v.type));
						}
					}
					break;
				case Instruction.Opcode.BOInvert:
					{
						Value v = PopValue();
						switch(v.type)
						{
							case Value.Type.Boolean:
								stack.Push(new Value() {
									type = Value.Type.Boolean,
									valueBoolean = !v.valueBoolean
								});
								break;
							default:
								throw new OpenDayDialogueException(string.Format("Cannot invert type {0}.", v.type));
						}
					}
					break;
				case Instruction.Opcode.BOOr:
					{
						Value val2 = PopValue().ConvertTo(Value.Type.Boolean, this);
						Value val1 = PopValue().ConvertTo(Value.Type.Boolean, this);

						stack.Push(new Value() { type = Value.Type.Boolean, valueBoolean = (val1.valueBoolean || val2.valueBoolean) });
					}
					break;
				case Instruction.Opcode.BOAnd:
					{
						Value val2 = PopValue().ConvertTo(Value.Type.Boolean, this);
						Value val1 = PopValue().ConvertTo(Value.Type.Boolean, this);

						stack.Push(new Value() { type = Value.Type.Boolean, valueBoolean = (val1.valueBoolean && val2.valueBoolean) });
					}
					break;
				case Instruction.Opcode.BOXor:
					{
						Value val2 = PopValue().ConvertTo(Value.Type.Boolean, this);
						Value val1 = PopValue().ConvertTo(Value.Type.Boolean, this);

						stack.Push(new Value() { type = Value.Type.Boolean, valueBoolean = (val1.valueBoolean != val2.valueBoolean) });
					}
					break;
				case Instruction.Opcode.Jump:
					programCounter = binary.labels[(uint)inst.operand1];
					break;
				case Instruction.Opcode.Exit:
					currentScene = null;
					currentText = null;
					currentChoices.Clear();
					stack.Clear();
					pause = true;
					break;
				case Instruction.Opcode.TextRun:
					currentText = binary.stringTable[(uint)inst.operand1];
					pause = true;
					if(textMainProcessor != null)
						currentText = textMainProcessor(currentText);
					if(handleText != null)
						handleText(currentText);
					break;
				case Instruction.Opcode.CommandRun:
					{
						Command cmd = binary.commands[(uint)inst.operand1];
						commandHandler.RunCommand(cmd.name, cmd.arguments);
					}
					break;
				case Instruction.Opcode.SetVariable:
					variableStore.SetVariable(binary.stringTable[(uint)inst.operand1], stack.Pop());
					break;
				case Instruction.Opcode.CallFunction:
					{
						string name = binary.stringTable[(uint)inst.operand1];
						int argCount = stack.Pop().ConvertTo(Value.Type.Int32, this).valueInt32;
						List<Value> args = new List<Value>();
						for(int i = 0; i < argCount; i++)
						{
							args.Add(stack.Pop());
						}
						functionHandler.RunFunction(name, args.ToArray());
					}
					break;
				case Instruction.Opcode.JumpTrue:
					if(stack.Pop().ConvertTo(Value.Type.Boolean, this).valueBoolean == true)
					{
						programCounter = binary.labels[(uint)inst.operand1];
					}
					break;
				case Instruction.Opcode.JumpFalse:
					if(stack.Pop().ConvertTo(Value.Type.Boolean, this).valueBoolean == false)
					{
						programCounter = binary.labels[(uint)inst.operand1];
					}
					break;
				case Instruction.Opcode.BeginChoice:
					currentChoices.Clear();
					inChoice = false;
					break;
				case Instruction.Opcode.Choice:
					{
						string text = binary.stringTable[(uint)inst.operand1];
						if(textChoiceProcessor != null)
							text = textChoiceProcessor(text);
						currentChoices.Add(new DialogueChoice() {
							text = text,
							label = (uint)inst.operand2
						});
					}
					break;
				case Instruction.Opcode.ChoiceTrue:
					{
						if(stack.Pop().ConvertTo(Value.Type.Boolean, this).valueBoolean == true)
						{
							string text = binary.stringTable[(uint)inst.operand1];
							if(textChoiceProcessor != null)
								text = textChoiceProcessor(text);
							currentChoices.Add(new DialogueChoice() {
								text = text,
								label = (uint)inst.operand2
							});
						}
					}
					break;
				case Instruction.Opcode.ChoiceSelection:
					{
						// Skip to after the choice if none exist/are available
						if(currentChoices.Count == 0)
						{
							programCounter = binary.labels[(uint)inst.operand1];
						}
						inChoice = true;
						pause = true;

						// Call the handler
						if(handleChoice != null)
						{
							List<string> choices = new List<string>();
							for(int i = 0; i < currentChoices.Count; i++)
							{
								choices.Add(currentChoices[i].text);
							}
							handleChoice(choices);
						}
					}
					break;
			}
		}
	}
}