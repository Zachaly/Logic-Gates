using System.Collections.Generic;
using System;

namespace Symulator_układów_logicznych
{
    public abstract class LogicGate
    {
        string _name = "";
        abstract public bool Output { get; } // an output signal of this logic gates
        protected List<LogicGate> Inputs; // gates connected with this one
        public List<LogicGate> GetInputs { get { return Inputs; } } // returns inputs
        public string Name { get { return _name; } } // returns friendly name of this logic gate
        public List<LogicGate> Outputs { get; }
        public bool Cloned { get; set; } = false;

        private LogicGate lastClone;

        public LogicGate(string name)
        {
            Inputs = new List<LogicGate>();
            Outputs = new List<LogicGate>();
            _name = name;
            Cloned = false;
        }

        // adds given gate to inputs of this one
        virtual public void ConnectWith(LogicGate gate)
        {
            gate.Outputs.Add(this);
        }

        // removes gate from inputs
        public virtual void Disconnect(LogicGate gate)
        {
            Inputs.Remove(gate);
            gate.Outputs.Remove(this);
        }

        // Clone differs on logic gate type
        public LogicGate Clone()
        {
            if (Cloned)
                return lastClone;
            
            if (this is ANDGate)
                lastClone =  new ANDGate();
            else if (this is NOTGate)
                lastClone = new NOTGate();
            else if (this is InputField || this is OutputField)
                lastClone = new Buffer();
            else if (this is Buffer)
                lastClone = new Buffer();
            else if (this is CustomGate)
            {
                CustomGate gate = this as CustomGate;

                lastClone =  new CustomGate(Name, gate.Schema.Clone().Result);
            }

            Cloned = true;
            return lastClone;
        }
    }
}
