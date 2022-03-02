using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System;
using System.Windows;

namespace Symulator_układów_logicznych
{
    public class GateSchema
    {
        public List<LogicGate> Gates { get; set; }
        public List<LogicGate> Inputs { get; set; }
        public LogicGate Output { get; set; }

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
                      select (el as InputFieldContainer).Field as LogicGate).ToList();

            try
            {
                OutputContainer = (from UserControl el in Container.Children
                                   where el is OutputFieldContainer
                                   select el as OutputFieldContainer).First();

                OutputContainer.ChangeState();

                Output = OutputContainer.Field;
            }
            catch (InvalidOperationException _)
            {
                Output = new OutputField();
            }

            Gates.Add(Output);

            Connections = new List<Connection>();

            foreach (var gate in Gates)
                foreach (var input in gate.GetInputs)
                    Connections.Add(new Connection(input, gate));
        }

        public GateSchema Clone()
        {
            List<LogicGate> newGates = new List<LogicGate>();
            List<LogicGate> newInputs = new List<LogicGate>();
            List<Connection> newConnections = new List<Connection>();
            LogicGate newOutput = null;

            CopyWithInputs(Output, newGates, newConnections);

            newOutput = (from el in newConnections where el.InputGate is Buffer select el.InputGate).FirstOrDefault();
            newInputs.AddRange(from el in newConnections where el.OutputGate is Buffer select el.OutputGate);

            GateSchema gateSchema = new GateSchema()
            {
                Gates = newGates,
                Connections = newConnections,
                Output = newOutput,
                Container = this.Container,
                Inputs = newInputs
            };

            return gateSchema;
        }
        public GateSchema TestClone()
        {
            List<LogicGate> newGates = new List<LogicGate>();
            List<LogicGate> newInputs = new List<LogicGate>();
            List<Connection> newConnections = new List<Connection>();
            LogicGate newOutput = null;

            CopyWithInputs(Output, newGates, newConnections);

            newOutput = (from el in newConnections where el.InputGate is Buffer select el.InputGate).FirstOrDefault();
            newInputs.AddRange(from el in newConnections where el.OutputGate is Buffer select el.OutputGate);

            GateSchema gateSchema = new GateSchema()
            {
                Gates = newGates,
                Connections = newConnections,
                Output = newOutput,
                Inputs = newInputs
            };

            return gateSchema;
        }

        void CopyWithInputs(LogicGate gate, List<LogicGate> gates, List<Connection> connections, bool present = false, LogicGate inputCopy = null)
        {
            var nGate = inputCopy;
            if (!present)
            {
                nGate = gate.Clone();
                gates.Add(nGate);
            }

            foreach(var input in gate.GetInputs)
            {
                var nInput = input.Clone();

                gates.Add(nInput);
                nGate.ConnectWith(nInput);
                connections.Add(new Connection(nInput, nGate));

                CopyWithInputs(input, gates, connections, true, nInput);
            }
        }
    }

    // struct made to organize connections between two logic gates
    public struct Connection
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