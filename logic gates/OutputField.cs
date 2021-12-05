
namespace Symulator_układów_logicznych
{
    // end of the logic gate layout, used as an output in custom logic gates
    class OutputField : LogicGate
    {
        public override bool Output
        {
            get
            {
                if (Inputs[0] is null == false)
                    return Inputs[0].Output;
                else
                    return false;
            }
        }


        public OutputField() : base("")
        {
            Inputs.Add(null);
        }

        // this field can have only one input, so it is not adding new gate, it replaces it
        public override void ConnectWith(LogicGate gate)
        {
            Inputs[0] = gate;
        }
    }
}
