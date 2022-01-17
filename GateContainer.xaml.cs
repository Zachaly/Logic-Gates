using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Symulator_układów_logicznych
{
    // class for a graphical field containg the logic gate 
    partial class GateContainer : UserControl, IWorkspaceItem
    {
        public LogicGate Gate;
        public List<VisualConnection> Connections { get; set; }

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
                foreach (var con in Connections)
                    con.Update();
            }
        }

        //Implementation of IWorkspaceItem
        public void CreateConnection(IWorkspaceItem target)
        {
            if (target is GateContainer)
                (target as GateContainer).Gate.ConnectWith(Gate);
            else if (target is OutputFieldContainer)
                (target as OutputFieldContainer).Field.ConnectWith(Gate);
        }

        public void DeleteConnection(IWorkspaceItem target)
        {
            if (target is GateContainer)
                (target as GateContainer).Gate.Disconnect(Gate);
            else if (target is OutputFieldContainer)
                (target as OutputFieldContainer).Field.Disconnect(Gate);
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

        public Point GetEndPoint(int conNum)
        {
            double left = Canvas.GetLeft(this);
            double top = Canvas.GetTop(this) + Height / (Connections.Count + 2) + conNum * 10;

            Point pt = new Point(left, top);

            return pt;
        }
    }
}
