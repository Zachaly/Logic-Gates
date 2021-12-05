
namespace Symulator_układów_logicznych
{
    // field that is generating the logical sygnal
    class InputField : LogicGate
    {
        bool logical_state = false; // current output of the field
        public override bool Output
        {
            get { return logical_state; }
        }

        public InputField() : base("") { }

        // Connect this to the target gate
        public override void ConnectWith(LogicGate gate)
        {
            gate.ConnectWith(this);
        }

        // reverses current logical state
        public void ChangeLogicalState()
        {
            logical_state = !logical_state;
        }
    }
}
