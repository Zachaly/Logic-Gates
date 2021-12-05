using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Symulator_układów_logicznych
{
    class GateSchema
    {
        public List<LogicGate> Gates { get; set; }
        public List<InputField> Inputs { get; set; }
        public OutputField Output { get; set; }
        public List<Connection> Connections { get; set; }

        Canvas Container;
        public GateSchema(Canvas canv)
        {
            Container = canv;
            UpdateSchema();
        }

        public void UpdateSchema()
        {
            Gates = (from GateContainer el in Container.Children
                     where el.Gate is LogicGate && el.Gate is InputField == false && el.Gate is OutputField == false
                     select el.Gate).ToList();

            Inputs = (from GateContainer el in Container.Children
                      where el.Gate is InputField
                      select (InputField)el.Gate).ToList();

            Output = (from GateContainer el in Container.Children
                      where el.Gate is OutputField
                      select (OutputField)el.Gate).First();

            Connections = new List<Connection>();

            foreach(var gate in Gates)
                foreach (var input in gate.GetInputs)
                    Connections.Add(new Connection(input, gate));
        }
    }

    // struct made to organize connections between two logic gates
    struct Connection
    {
        public LogicGate InputGate; // gate receiving the signal
        public LogicGate OutputGate; // gate giving a signal

        public Connection(LogicGate output, LogicGate input)
        {
            OutputGate = output;
            InputGate = input;
        }
    }
}
