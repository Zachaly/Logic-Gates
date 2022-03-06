namespace LogicGates
{
    // Field that is generating the logical sygnal
    public class InputField : LogicGate
    {
        bool logicalState = false; // Current output of the field
        public override bool Output
        {
            get => logicalState;
        }

        public InputField() : base("Input Field") { }

        // Connect this to the target gate
        public override void ConnectWith(LogicGate gate)
        {
            gate.ConnectWith(this);
        }

        // Reverses current logical state
        public void ChangeLogicalState()
        {
            logicalState = !logicalState;
        }
    }
}
