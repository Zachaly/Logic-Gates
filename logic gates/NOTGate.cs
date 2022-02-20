
namespace Symulator_układów_logicznych
{
    class NOTGate : LogicGate
    {
        // reverses the input value, returns false if there are no input
        public override bool Output
        {
            get
            {

                try
                {
                    if (Inputs[0] is null == false)
                        return !Inputs[0].Output;
                    else
                        return false;
                }
                catch (System.ArgumentOutOfRangeException _)
                {
                    return false;
                }
            }
        }

        public NOTGate() : base("NOT") { }

        // NOT can have only one input, the function does not add to list, it replaces the element
        public override void ConnectWith(LogicGate gate)
        {
            if (Inputs.Count < 1)
                Inputs.Add(gate);
        }
    }
}
