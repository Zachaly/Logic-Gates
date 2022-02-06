using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System;
using System.Windows;

namespace Symulator_układów_logicznych
{
    class GateSchema : ICloneable
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
            catch (InvalidOperationException e)
            {
                Output = new OutputField();
            }

            Gates.Add(Output);

            Connections = new List<Connection>();

            foreach (var gate in Gates)
                foreach (var input in gate.GetInputs)
                    Connections.Add(new Connection(input, gate));

            
        }

        public object Clone()
        {
            List<LogicGate> newGates = new List<LogicGate>();
            List<LogicGate> newInputs = new List<LogicGate>();
            List<Connection> newConnections = new List<Connection>();
            LogicGate newOutput = new Buffer();

            foreach (var el in Connections)
            {
                newConnections.Add((Connection)el.Clone());
            }


            foreach (var el in newConnections)
            {
                el.InputGate.ConnectWith(el.OutputGate);

                if (newGates.Contains(el.OutputGate))
                {
                    continue;
                }
                
                newGates.Add(el.OutputGate);

                if(el.InputGate is Buffer) 
                {
                    newOutput = el.InputGate;
                }

                if (el.OutputGate is Buffer)
                    newInputs.Add(el.OutputGate);
            }

            GateSchema gateSchema = new GateSchema()
            {
                Gates = newGates,
                Connections = newConnections,
                Output = newOutput,
                Container = this.Container,
                Inputs = newInputs
            };
            gateSchema.Test();

            return gateSchema;


        }

        void Test()
        {
            string str = "";
            foreach (var gate in Gates)
                str += $"{gate.Name}({gate.Output})\n";

            MessageBox.Show(str, "Outputs");

            if (Connections is null)
                return;

            str = "";
            foreach (var gate in Connections)
                str += $"{gate.OutputGate.Name}-{gate.InputGate.Name}\n ";

            MessageBox.Show(str, "Connections");

            str = "";
            foreach (var gate in Gates)
            {
                str += gate.Name + " has: ";
                foreach (var el in gate.GetInputs)
                    str += $"{el.Name}, ";
                str += "\n";
            }


            MessageBox.Show(str, "inputs");

            str = "";
            foreach (var gate in Inputs)
            {
                str += gate.Name + " ";
            }


            MessageBox.Show(str, "inputs");
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
           
            Connection con = new Connection((LogicGate)OutputGate.Clone(), (LogicGate)InputGate.Clone());

            con.InputGate.ConnectWith(con.OutputGate);

            return con;
        }
    }
}