using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogicGates
{
    // Output of the whole sheme
    public partial class OutputFieldContainer : UserControl, IWorkspaceItem
    {
        public LogicGate Gate 
        { 
            get => Field; 
            set => Field = value; 
        }
        public LogicGate Field { get; set; }
        public List<VisualConnection> Connections { get; set; }

        public OutputFieldContainer()
        {
            InitializeComponent();
            Field = new OutputField();
            Connections = new List<VisualConnection>();
        }

        public Point GetStartPoint()
        {
            return new Point();
        }

        public Point GetEndPoint(int conNum)
        {
            return TransformToAncestor(App.Current.MainWindow).Transform(new Point(0, 0));
        }

        // Changes color basing on logical state
        public void ChangeState()
        {
            if (Field.Output)
                Circle.Fill = new SolidColorBrush(Colors.Red);
            else
                Circle.Fill = new SolidColorBrush(Colors.Gray);
        }

        // Functions below impelemented only for sake of compatibilyty
        public void CreateConnection(IWorkspaceItem target)
        {

        }

        public void DeleteConnection(IWorkspaceItem target)
        {

        }

        public void CreateVisualConnection()
        {

        }

        public void Delete()
        {

        }
    }
}
