using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Symulator_układów_logicznych
{
    // Holds an input signal
    partial class InputFieldContainer : UserControl, IWorkspaceItem
    {
        public LogicGate Gate 
        { 
            get => Field; 
            set => Field = value; 
        }
        public LogicGate Field { get; set; }
        public List<VisualConnection> Connections { get; set; }

        public InputFieldContainer()
        {
            InitializeComponent();
            Field = new InputField();
            Connections = new List<VisualConnection>();
        }

        // Changes the logical state and color on click
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (Field as InputField).ChangeLogicalState();
            if (Field.Output)
                Circle.Fill = new SolidColorBrush(Colors.Red);
            else
                Circle.Fill = new SolidColorBrush(Colors.Gray);

            MainWindow.CurrentSchema.UpdateSchema();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CreateVisualConnection();
        }

        // Implementation of IWorkspaceItem
        public void CreateConnection(IWorkspaceItem target)
        {
            if (target is GateContainer)
                (target as GateContainer).Gate.ConnectWith(Field);
            else if (target is OutputFieldContainer)
                (target as OutputFieldContainer).Field.ConnectWith(Field);
        }

        public void DeleteConnection(IWorkspaceItem target)
        {
            if (target is GateContainer)
                (target as GateContainer).Gate.Disconnect(Field);
            else if (target is OutputFieldContainer)
                (target as OutputFieldContainer).Field.Disconnect(Field);
        }

        public void CreateVisualConnection()
        {
            (App.Current.MainWindow as MainWindow).ConnectionEnable(this);
        }

        public void Delete()
        {
            // Does nothing,
            // You cannot normally delete this element
        }

        public Point GetStartPoint()
        {
            // I had to user shenanigans here bsc GetLeft and GetTop do not work with shapes apparently :/
            return TransformToAncestor(App.Current.MainWindow).Transform(new Point(0 + Circle.Width, 0));
        }

        // Connection cannot end in input, but this must be implementd for compatibility
        public Point GetEndPoint(int conNum)
        {
            return new Point();
        }
    }
}
