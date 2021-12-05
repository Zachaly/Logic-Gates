
namespace Symulator_układów_logicznych
{
    class ANDGate : LogicGate
    {
        // returns true if all inputs have the same logic value(all are true || all are false)
        public override bool Output
        {
            get
            {
                // if the next element has different logic value, return false
                for (int i = 0; i < Inputs.Count - 2; i++)
                    if (Inputs[i].Output != Inputs[i + 1].Output)
                        return false;

                return true;
            } 
        }
        public ANDGate() : base("AND") { }

        public override void ConnectWith(LogicGate gate)
        {
            Inputs.Add(gate);
        }
    }
}
