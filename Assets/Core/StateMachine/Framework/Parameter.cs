using System;

namespace Core.StateMachine
{
    [Serializable]
    public struct Parameter {

        public string Name;
        public string TypeName;

        public Parameter(string name, string typeName) {
            Name = name;
            TypeName = typeName;
        }

    }
}