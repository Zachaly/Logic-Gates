
namespace Symulator_układów_logicznych
{
    public class ANDGate : LogicGate
    {
        // Returns true if all inputs have the same logic value(all are true || all are false)
        public override bool Output
        {
            get
            {
                if (Inputs.Count == 0 || Inputs.Count == 1)
                    return false;

                foreach (var inp in Inputs)
                    if (!inp.Output)
                        return false;

                return true;
            }
        }
        public ANDGate() : base("AND") { }

        public override void ConnectWith(LogicGate gate)
        {
            base.ConnectWith(gate);
            Inputs.Add(gate);
        }
    }
}
