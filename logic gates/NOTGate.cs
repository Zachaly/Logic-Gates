
namespace Symulator_układów_logicznych
{
    class NOTGate : LogicGate
    {
        // reverses the input value, returns false if there are no input
        public override bool Output
        {
            get
            {
                if (Inputs[0] is null)
                    return false;

                return !Inputs[0].Output;
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
