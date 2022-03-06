using System;

namespace LogicGates
{
    // Used only in custom gates to replace input fields
    class Buffer : LogicGate
    {
        public LogicGate Holder { get; set; }

        // Buffer sends the same signal that it receives
        public override bool Output 
        {
            get
            {
                try
                {
                    if (Inputs[0] is null == false)
                        return Inputs[0].Output;
                    else
                        return false;
                }
                catch (ArgumentOutOfRangeException _)
                {
                    return false;
                }
            }
        }

        public Buffer() : base("Buffer")
        {
            Holder = this;
        }

        // Buffer can have only one input
        public override void ConnectWith(LogicGate gate)
        {
            if (Inputs.Count > 0)
                return;

            base.ConnectWith(this);
            Inputs.Add(gate);
        }
    }
}
