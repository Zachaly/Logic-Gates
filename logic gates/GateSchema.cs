using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System;
using System.Windows;
using System.Threading.Tasks;

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

        public async Task<GateSchema> Clone()
        {
            List<LogicGate> newGates = new List<LogicGate>();
            List<LogicGate> newInputs = new List<LogicGate>();
            List<Connection> newConnections = new List<Connection>();
            LogicGate newOutput = null;

            await CopyWithInputs(Output, newGates, newConnections);

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

        // Copies given gate with its inputs and so on, adds them to the list
        async Task CopyWithInputs(LogicGate gate, List<LogicGate> gates, List<Connection> connections, bool present = false, LogicGate inputCopy = null)
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
                if (nInput is Buffer)
                    gates.Add((nInput as Buffer).Holder);
                else
                    gates.Add(nInput);

                nGate.ConnectWith(nInput);
                connections.Add(new Connection(nInput, nGate));

                CopyWithInputs(input, gates, connections, true, nInput);
            }
        }

        // fills workspace with given schema
        public async void FillWorkspaceWithSchema()
        {
            var schema = await Clone();
            Container.Children.Clear();

            var gates = from el in schema.Gates where !(el is Buffer) select el;
            int positionY = 50;
            int positionX = 50;
            InputFieldContainer newInput = null;

            for(int i = 0; i < schema.Inputs.Count; i++)
            {
                LogicGate[] outputs = new LogicGate[schema.Inputs[i].Outputs.Count];
                try
                {
                    schema.Inputs[i].Outputs.CopyTo(outputs);
                    for (int j = 0; j < outputs.Length; j++)
                    {
                        outputs[j].Disconnect(schema.Inputs[i]);
                    }
                }
                catch(NullReferenceException _) { continue; }
                schema.Inputs[i] = new InputField();
                newInput = new InputFieldContainer();
                newInput.Field = schema.Inputs[i];

                foreach (var conn in outputs)
                    conn.ConnectWith(newInput.Field);

                Canvas.SetTop(newInput, positionY);
                Canvas.SetLeft(newInput, positionX);
                Container.Children.Add(newInput);
                positionY += 100;
            }

            OutputFieldContainer output = new OutputFieldContainer();
            output.Field = schema.Output;
            Canvas.SetTop(output, 150);
            Canvas.SetRight(output, 50);
            Container.Children.Add(output);

            await AddGateContainers(schema.Output, true);
            AddGateConnections(schema);

            var inputField = (from UIElement el in Container.Children where el is InputFieldContainer select el as InputFieldContainer).ToList();
            var items = (from UIElement el in Container.Children where el is IWorkspaceItem select el as IWorkspaceItem).ToList();

            foreach(var input in inputField)
            {
                var outputs = (from el in items where input.Gate.Outputs.Contains(el.Gate) select el).ToList();
                foreach(var outputGate in outputs)
                {
                    VisualConnection VisualConnection = new VisualConnection(input, outputGate as UserControl);
                    input.Connections.Add(VisualConnection);
                    outputGate.Connections.Add(VisualConnection);
                    Container.Children.Add(VisualConnection);
                    VisualConnection.Update();
                }
            }
        }

        // Adds gate containers to Workspace
        async Task AddGateContainers(LogicGate startGate, bool isStart, int x = 100, int y = 150 )
        {
            if (startGate is null)
                return;

            if (!isStart)
            {
                try
                {
                    GateContainer container = new GateContainer(startGate, GateContainer.GateColours[startGate.Name]);
                    Canvas.SetTop(container, y);
                    Canvas.SetRight(container, x);
                    Container.Children.Add(container);
                }
                catch(KeyNotFoundException _)
                {
                    return;
                }  
            }

            int currentInput = 0;
            foreach(var input in startGate.GetInputs)
            {
                LogicGate newInput = input;
                //if (input is Buffer)
                //    newInput = (input as Buffer).Holder;

                await AddGateContainers(newInput, false, x + 150, y + currentInput * 150);
                currentInput += 1;
            }
        }

        // Adds visual connections to workspace
        void AddGateConnections(GateSchema schema)
        {
            foreach(var connection in schema.Connections)
            {
                var items = from UIElement el in Container.Children where el is IWorkspaceItem select el as IWorkspaceItem;
                try
                {
                    var input = (from IWorkspaceItem el in items where el.Gate == connection.OutputGate select el).First();
                    var output = (from IWorkspaceItem el in items where el.Gate == connection.InputGate select el).First();

                    VisualConnection VisualConnection = new VisualConnection(input as UserControl, output as UserControl);
                    input.Connections.Add(VisualConnection);
                    output.Connections.Add(VisualConnection);
                    Container.Children.Add(VisualConnection);
                }
                catch(InvalidOperationException _)
                {
                    continue;
                }
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