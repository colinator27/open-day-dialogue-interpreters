using System.Collections;
using System.Collections.Generic;

namespace OpenDayDialogue
{
    public abstract class VariableStore
    {
        public abstract void SetVariable(string name, Value value);
        public abstract Value GetVariable(string name);
        public abstract void CleanUp();
    }
}