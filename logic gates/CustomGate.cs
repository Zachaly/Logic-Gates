using System.Collections.Generic;

namespace LogicGates
{
    // Logic gate created by user, based on given schema
    public class CustomGate : LogicGate
    {
        GateSchema _schema; // Schema of this custom gate
        LogicGate OutputGate; // Gate used as an output
        List<LogicGate> BufferInputs; // buffers that gates connect with
        public GateSchema Schema // Schema describing this gate
        { 
            get => _schema; 
        } 
        public override bool Output // Output of the custom gate is an output of output gate
        {
            get => OutputGate.Output;
        }

        public CustomGate(string name, GateSchema schema) : base(name)
        {
            UpdateSchema(schema);
        }

        // Connects given gate with this one's output
        public override void ConnectWith(LogicGate gate)
        {
            foreach(var buffer in BufferInputs)
                if(buffer.GetInputs.Count == 0)
                {
                    base.ConnectWith(gate);
                    buffer.ConnectWith(gate);
                    if (gate is Buffer)
                        Inputs.Add((gate as Buffer).Holder);
                    else
                        Inputs.Add(gate);
                    break;
                }
        }

        public override void Disconnect(LogicGate gate)
        {
            foreach (var buffer in BufferInputs)
                if (buffer.GetInputs.Contains(gate))
                    buffer.Disconnect(gate);

            base.Disconnect(gate);
        }

        // Updates info basing on given schema
        public void UpdateSchema(GateSchema schema)
        {
            _schema = schema.Clone().Result;
            OutputGate = schema.Output;
            BufferInputs = schema.Inputs;

            foreach (var buffer in schema.Gates)
                if(buffer is Buffer)
                    (buffer as Buffer).Holder = this;
        }
    }
}
