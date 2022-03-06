using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LogicGates
{
    // Class for a graphical field containg the logic gate 
    partial class GateContainer : UserControl, IWorkspaceItem
    {
        public LogicGate Gate { get; set; }
        public List<VisualConnection> Connections { get; set; }

        public static Dictionary<string, Color> GateColours { get; } = new Dictionary<string, Color>(); // Stores info about colors of different gate types
        public GateContainer() : base()
        {
            InitializeComponent();
        }

        public GateContainer(LogicGate gate, Color color) : base()
        {
            InitializeComponent();
            Gate = gate;
            ContentLabel.Content = gate.Name;
            ContentLabel.Background = new SolidColorBrush(color);
            Connections = new List<VisualConnection>();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void CreateVisualConnection_Click(object sender, RoutedEventArgs e)
        {
            CreateVisualConnection();
        }
        
        // Used in drag and drop
        private void Content_MouseMove(object sender, MouseEventArgs e)
        {
            MainWindow.DragElement = this;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this), DragDropEffects.All);
                foreach (var connection in Connections)
                    connection.Update();
            }
        }

        //Implementation of IWorkspaceItem
        public void CreateConnection(IWorkspaceItem target)
        {
            target.Gate.ConnectWith(Gate);
        }

        public void DeleteConnection(IWorkspaceItem target)
        {
            target.Gate.Disconnect(Gate);
        }

        public void CreateVisualConnection()
        {
            (App.Current.MainWindow as MainWindow).ConnectionEnable(this);
        }

        public void Delete()
        {
            (App.Current.MainWindow as MainWindow).DeleteContainer(this);
        }

        public Point GetStartPoint()
        {
            return new Point(Canvas.GetLeft(this) + Width,
                Canvas.GetTop(this) + Height / 2);
        }

        public Point GetEndPoint(int connectionNumber)
        {
            double left = Canvas.GetLeft(this);
            double top = Canvas.GetTop(this) + Height / (Connections.Count + 2) + connectionNumber * 10;

            Point pt = new Point(left, top);

            return pt;
        }
    }
}
