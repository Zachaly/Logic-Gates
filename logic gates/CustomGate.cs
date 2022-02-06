using System.Collections.Generic;

namespace Symulator_układów_logicznych
{
    // logic gate created by user based on given schema
    class CustomGate : LogicGate
    {
        GateSchema _schema;

        LogicGate OutputGate; // gate used as an output
        public GateSchema Schema { get { return _schema; } } // schema describing this gate
        public override bool Output // output of the custom gate is an output of output gate
        {
            get
            {
                return OutputGate.Output;
            }
        }

        public CustomGate(string name, GateSchema schema) : base(name)
        {
            UpdateSchema(schema);
        }


        // connects given gate with this one's output
        public override void ConnectWith(LogicGate gate)
        {

            foreach(var buff in Inputs)
                if(buff.GetInputs.Count == 0)
                {
                    buff.ConnectWith(gate);
                    break;
                }
            
        }

        // updates info basing on given schema
        public void UpdateSchema(GateSchema schema)
        {
            _schema = schema;
            OutputGate = schema.Output;
            Inputs = schema.Inputs;
            
        }
    }

}
