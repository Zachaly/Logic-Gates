using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System;

namespace Symulator_układów_logicznych
{
    class GateSchema : ICloneable
    {
        public List<LogicGate> Gates { get; set; }
        public List<InputField> Inputs { get; set; }
        public OutputField Output { get; set; }

        // Field containing output, used to refresh appearance when something changes
        OutputFieldContainer OutputContainer;

        public List<Connection> Connections { get; set; }

        Canvas Container;
        public GateSchema(Canvas canv)
        {
            Container = canv;
            UpdateSchema();
        }
        public GateSchema() { }

        // Gets all info about schema on canvas
        public void UpdateSchema()
        {
            Gates = (from UserControl el in Container.Children
                     where el is GateContainer
                     select (el as GateContainer).Gate).ToList();

            Inputs = (from UserControl el in Container.Children
                      where el is InputFieldContainer
                      select (el as InputFieldContainer).Field).ToList();


            try
            {
                OutputContainer = (from UserControl el in Container.Children
                                   where el is OutputFieldContainer
                                   select el as OutputFieldContainer).First();

                OutputContainer.ChangeState();

                Output = OutputContainer.Field;
            }
            catch (InvalidOperationException e)
            {
                Output = new OutputField();
            }

            Connections = new List<Connection>();

            foreach (var gate in Gates)
                foreach (var input in gate.GetInputs)
                    Connections.Add(new Connection(input, gate));
        }

        public object Clone()
        {
            List<LogicGate> newGates = new List<LogicGate>();

            foreach (var el in Gates)
                newGates.Add((LogicGate)el.Clone());

            List<InputField> newInputs = new List<InputField>();
            foreach (var el in Inputs)
                newInputs.Add((InputField)el.Clone());

            List<Connection> newConnections = new List<Connection>();
            foreach (var el in Connections)
                newConnections.Add((Connection)el.Clone());

            return new GateSchema()
            {
                Gates = newGates,
                Output = (OutputField)Output.Clone(),
                Inputs = newInputs
            };
        }
    }

    // struct made to organize connections between two logic gates
    struct Connection : ICloneable
    {
        public LogicGate InputGate; // gate receiving the signal
        public LogicGate OutputGate; // gate giving a signal

        public Connection(LogicGate output, LogicGate input)
        {
            OutputGate = output;
            InputGate = input;
        }

        public object Clone()
        {
            return new Connection((LogicGate)OutputGate.Clone(), (LogicGate)InputGate.Clone());
        }
    }
}