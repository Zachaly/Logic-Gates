
namespace Symulator_układów_logicznych
{
    // end of the logic gate layout, used as an output in custom logic gates
    public class OutputField : LogicGate
    {
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
                catch (System.ArgumentOutOfRangeException _)
                {
                    return false;
                }
            }
        }


        public OutputField() : base("Output Field") { }

        // this field can have only one input, so it is not adding new gate, it replaces it
        public override void ConnectWith(LogicGate gate)
        {
            if (Inputs.Count > 0)
                return;

            base.ConnectWith(gate);
            Inputs.Add(gate);
        }
    }
}
