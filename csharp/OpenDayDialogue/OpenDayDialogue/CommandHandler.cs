using System.Collections;
using System.Collections.Generic;

namespace OpenDayDialogue
{
    public class CommandHandler
    {
        public delegate void Handler(Value[] args);

        private Dictionary<string, Handler> handlers;

        public CommandHandler()
        {
            handlers = new Dictionary<string, Handler>();
        }

        public void RunCommand(string name, Value[] args)
        {
            if(handlers.ContainsKey(name))
            {
                handlers[name](args);
            } else
            {
                throw new OpenDayDialogueException(string.Format("Undefined command with name \"{0}\".", name));
            }
        }

        public void AddNewCommand(string name, Handler handler)
        {
            handlers[name] = handler;
        }

        public void RemoveCommand(string name)
        {
            if(handlers.ContainsKey(name))
                handlers.Remove(name);
        }

        public void ClearCommands()
        {
            handlers.Clear();
        }
    }
}