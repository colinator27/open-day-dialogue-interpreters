using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OpenDayDialogue
{
	public struct Command
	{
		public string name;
		public Value[] arguments;
	}

	public class Binary
	{
		public Dictionary<uint, string> stringTable; // id number, value string
		public Dictionary<uint, Value> valueTable; // id number, value Value
		public Dictionary<string, string> definitions; // key string, value string
		public Dictionary<uint, Command> commands; // id number, command
		public Dictionary<string, uint> scenes; // name string, bytecode label number
		public Dictionary<uint, int> labels; // id number, bytecode label location number
		public List<Instruction> instructions;

		public Binary(Stream s)
		{
			stringTable = new Dictionary<uint, string>();
			valueTable = new Dictionary<uint, Value>();
			definitions = new Dictionary<string, string>();
			commands = new Dictionary<uint, Command>();
			scenes = new Dictionary<string, uint>();
			labels = new Dictionary<uint, int>();
			instructions = new List<Instruction>();

			using(OReader br = new OReader(s, Encoding.UTF8))
			{
				if(Encoding.UTF8.GetString(br.ReadBytes(4)) != "OPDA")
					throw new OpenDayDialogueException("Invalid header");

				// Read string table
				int stringTableCount = br.ReadInt32();
				for(int i = 0; i < stringTableCount; i++)
				{
					uint key = br.ReadUInt32();
					string value = br.ReadZeroTermString();
					stringTable[key] = value;
				}

				// Read value table
				int valueTableCount = br.ReadInt32();
				for(int i = 0; i < valueTableCount; i++)
				{
					uint key = br.ReadUInt32();
					Value value = new Value(br, this);
					valueTable[key] = value;
				}

				// Read definitions
				int definitionCount = br.ReadInt32();
				for(int i = 0; i < definitionCount; i++)
				{
					string key = stringTable[br.ReadUInt32()];
					string value = stringTable[br.ReadUInt32()];
					definitions[key] = value;
				}

				// Read commands
				int commandCount = br.ReadInt32();
				for(int i = 0; i < commandCount; i++)
				{
					uint key = br.ReadUInt32();

					// Read name
					string name = stringTable[br.ReadUInt32()];

					// Read arguments
					int argCount = br.ReadInt32();
					List<Value> args = new List<Value>();
					for(int j = 0; j < argCount; j++)
					{
						args.Add(valueTable[br.ReadUInt32()]);	
					}

					// Add to the dictionary
					commands[key] = new Command() {
						name = name,
						arguments = args.ToArray()
					};
				}

				// Read scenes
				int sceneCount = br.ReadInt32();
				for(int i = 0; i < sceneCount; i++)
				{
					string name = stringTable[br.ReadUInt32()];
					uint label = br.ReadUInt32();
					scenes[name] = label;
				}

				// Read bytecode
				int instructionCount = br.ReadInt32();
				for(int i = 0; i < instructionCount; i++)
				{
					Instruction inst = new Instruction(br, this);
					instructions.Add(inst);

					// Get the label locations
					if(inst.opcode == Instruction.Opcode.Label)
						labels[(uint)inst.operand1] = i;
				}
			}
		}
	}
}