using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System;
using System.Windows;
using System.Threading.Tasks;

namespace LogicGates
{
    public class GateSchema
    {
        public List<LogicGate> Gates { get; set; }
        public List<LogicGate> Inputs { get; set; }
        public LogicGate Output { get; set; }

        // Field containing output, used to refresh appearance when something changes
        OutputFieldContainer OutputContainer;
        public List<Connection> Connections { get; set; }
        public int NumberOfInputs { get => Inputs.Count; }

        Canvas Container; // Workspace from which gates are loaded
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
            // Making sure that all gates can be cloned
            Gates.ForEach(gate => gate.Cloned = false);
            Inputs.ForEach(gate => gate.Cloned = false);
            Output.Cloned = false;
            List<LogicGate> newGates = new List<LogicGate>();
            List<LogicGate> newInputs = new List<LogicGate>();
            List<Connection> newConnections = new List<Connection>();
            LogicGate newOutput = null;

            await CopyWithInputs(Output, newGates, newConnections, newInputs);

            // Getting output of the schema
            newOutput = (from el in newConnections where el.InputGate is Buffer select el.InputGate).FirstOrDefault();

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

        // Version of clone used in tests, you cannot use container in tests
        public GateSchema TestClone()
        {
            Gates.ForEach(gate => gate.Cloned = false);
            Inputs.ForEach(gate => gate.Cloned = false);
            Output.Cloned = false;
            List<LogicGate> newGates = new List<LogicGate>();
            List<LogicGate> newInputs = new List<LogicGate>();
            List<Connection> newConnections = new List<Connection>();
            LogicGate newOutput = null;

            CopyWithInputs(Output, newGates, newConnections, newInputs);

            newOutput = (from el in newConnections where el.InputGate is Buffer select el.InputGate).FirstOrDefault();

            GateSchema gateSchema = new GateSchema()
            {
                Gates = newGates,
                Connections = newConnections,
                Output = newOutput,
                Inputs = newInputs
            };

            return gateSchema;
        }

        // Copies given gate with its inputs and so on, adds them to the list
        async Task CopyWithInputs(LogicGate gate, List<LogicGate> gates, List<Connection> connections, List<LogicGate> inputs, bool present = false, LogicGate inputCopy = null)
        {
            var newGate = inputCopy;
            if (!present)
            {
                newGate = gate.Clone();
            }

            if(!gates.Contains(newGate))
                gates.Add(newGate);

            foreach(var input in gate.GetInputs)
            {
                try
                {
                    LogicGate newInput = input.Clone();

                    newGate.ConnectWith(newInput);
                    connections.Add(new Connection(newInput, newGate));

                    await CopyWithInputs(input, gates, connections, inputs, true, newInput);
                }
                catch(NullReferenceException _)
                {
                    continue;
                }
            }

            if (newGate.GetInputs.Count == 0 && !inputs.Contains(newGate))
                inputs.Add(newGate);
        }

        // Fills workspace with given schema
        public async void FillWorkspaceWithSchema()
        {
            var schema = await Clone();
            Container.Children.Clear();

            (App.Current.MainWindow as MainWindow).SetStandartWorkspace(schema.NumberOfInputs);
            var inputContainers = (from UIElement el in Container.Children where el is InputFieldContainer select el as InputFieldContainer).ToList();

            // Replaces buffer inputs with input fields
            for(int i = 0; i < schema.Inputs.Count; i++)
            {
                LogicGate[] outputs = new LogicGate[schema.Inputs[i].Outputs.Count];
                var conns = (from el in schema.Connections where el.OutputGate == schema.Inputs[i] select el).ToList();
                try
                {
                    schema.Inputs[i].Outputs.CopyTo(outputs);
                    for (int j = 0; j < outputs.Length; j++)
                    {
                        outputs[j].Disconnect(schema.Inputs[i]);
                    }
                }
                catch(NullReferenceException _) 
                { 

                }
                schema.Inputs[i] = new InputField();
                inputContainers[i].Field = schema.Inputs[i];

                foreach (var conn in outputs) 
                {
                    var connectionGate = conn;

                    if (conn is Buffer)
                        connectionGate = (conn as Buffer).Holder;

                    connectionGate.ConnectWith(inputContainers[i].Field);
                    schema.Connections.Add(new Connection(inputContainers[i].Field, connectionGate));
                }

                foreach (var conn in conns)
                {
                    schema.Connections.Remove(conn);
                }
            }

            // Replaces buffer output with output field
            var outputConnection = (from el in schema.Connections where el.InputGate == schema.Output select el).First();
            schema.Connections.Remove(outputConnection);
            schema.Output = new OutputField();
            schema.Output.ConnectWith(outputConnection.OutputGate);
            schema.Connections.Add(new Connection(outputConnection.OutputGate, schema.Output));

            OutputFieldContainer output = (from UIElement el in Container.Children where el is OutputFieldContainer select el as OutputFieldContainer).First();
            output.Field = schema.Output;

            // Adds visual gate containers and connections between them
            await AddGateContainers(schema.Output, true);
            AddGateConnections(schema.Output);

            MainWindow.CurrentSchema.UpdateSchema();
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
                if (input is Buffer && input.GetInputs.Count > 0)
                   newInput = input.GetInputs[0];

                await AddGateContainers(newInput, false, x + Constants.HorizontalDistanceBetweenGateContainers,
                    y + currentInput * Constants.VerticalDistanceBetweenGateContainers);
                currentInput += 1;
            }
        }

        // Adds visual connections to workspace
        void AddGateConnections(LogicGate startGate)
        {
            foreach (var selectedInput in startGate.GetInputs)
            {
                var currentInput = selectedInput;
                if (currentInput is Buffer)
                    currentInput = (currentInput as Buffer).Holder;

                var items = from UIElement el in Container.Children where el is IWorkspaceItem select el as IWorkspaceItem;

                try
                {
                    var input = (from IWorkspaceItem el in items where el.Gate == startGate select el).First();
                    var output = (from IWorkspaceItem el in items where el.Gate == currentInput select el).First();

                    VisualConnection VisualConnection = new VisualConnection(output as UserControl, input as UserControl);
                    input.Connections.Add(VisualConnection);
                    output.Connections.Add(VisualConnection);
                    Container.Children.Add(VisualConnection);
                    VisualConnection.Update();
                    AddGateConnections(currentInput);
                }
                catch (InvalidOperationException _)
                {
                    continue;
                }
            }
        }
    }

    // Struct made to organize connections between two logic gates
    public struct Connection
    {
        public LogicGate InputGate; // Gate receiving the signal
        public LogicGate OutputGate; // Gate giving a signal

        public Connection(LogicGate output, LogicGate input)
        {
            OutputGate = output;
            InputGate = input;
        }
    }
}