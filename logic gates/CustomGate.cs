using System.Collections.Generic;
using System.Linq;

namespace Symulator_układów_logicznych
{
    // logic gate created by user based on given schema
    public class CustomGate : LogicGate
    {
        GateSchema _schema;

        LogicGate OutputGate; // gate used as an output
        List<LogicGate> BufferInputs;
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
            foreach(var buff in BufferInputs)
                if(buff.GetInputs.Count == 0)
                {
                    base.ConnectWith(gate);
                    buff.ConnectWith(gate);
                    Inputs.Add(gate);
                    break;
                }
        }

        // updates info basing on given schema
        public void UpdateSchema(GateSchema schema)
        {
            _schema = schema;
            OutputGate = schema.Output;
            BufferInputs = schema.Inputs;

            foreach (var buff in schema.Gates)
                if(buff is Buffer)
                    (buff as Buffer).Holder = this;
        }
    }
}
