using System.Collections;
using System.Collections.Generic;

namespace OpenDayDialogue
{
    public class FunctionHandler
    {
        public delegate Value Handler(Value[] args);

        private Dictionary<string, Handler> handlers;

        public FunctionHandler()
        {
            handlers = new Dictionary<string, Handler>();
        }

        public Value RunFunction(string name, Value[] args)
        {
            if(handlers.ContainsKey(name))
            {
                return handlers[name](args);
            } else
            {
                throw new OpenDayDialogueException(string.Format("Undefined function with name \"{0}\".", name));
            }
        }

        public void AddNewFunction(string name, Handler handler)
        {
            handlers[name] = handler;
        }

        public void RemoveFunction(string name)
        {
            if(handlers.ContainsKey(name))
                handlers.Remove(name);
        }

        public void ClearFunctions()
        {
            handlers.Clear();
        }
    }
}