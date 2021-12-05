using System.Collections.Generic;

namespace Symulator_układów_logicznych
{
    public abstract class LogicGate
    {
        string _name = "";
        abstract public bool Output { get; } // an output signal of this logic gates
        protected List<LogicGate> Inputs; // gates connected with this one
        public List<LogicGate> GetInputs { get { return Inputs; } } // returns inputs
        public string Name { get { return _name; } } // returns friendly name of this logic gate

        public LogicGate(string name)
        {
            Inputs = new List<LogicGate>();
            _name = name;
        }

        // adds given gate to inputs of this one
        abstract public void ConnectWith(LogicGate gate);

        // removes gate from inputs
        public void Disconnect(LogicGate gate)
        {
            Inputs.Remove(gate);
        }

    }

}
