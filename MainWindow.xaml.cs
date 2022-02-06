using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Symulator_układów_logicznych
{
    partial class MainWindow : Window
    {
        public static UIElement DragElement; // element used in drag and drop
        internal static GateSchema currentSchema; // elements currently in workspace
        UserControl ConnectedGate; // element that connects to another in this moment

        public MainWindow()
        {
            InitializeComponent();
            ToolBox.Items.Add(new ToolBoxItem(new ANDGate(), Colors.Green, WorkSpace));
            ToolBox.Items.Add(new ToolBoxItem(new NOTGate(), Colors.Brown, WorkSpace));
            currentSchema = new GateSchema(WorkSpace);

        }

        // Drag and drop for elements in workspace
        private void WorkSpace_Drop(object sender, DragEventArgs e)
        {
            Point position = e.GetPosition(WorkSpace);

            Canvas.SetLeft(DragElement, position.X);
            Canvas.SetTop(DragElement, position.Y);

        }

        private void WorkSpace_DragOver(object sender, DragEventArgs e)
        {
            Point position = e.GetPosition(WorkSpace);

            Canvas.SetLeft(DragElement, position.X);
            Canvas.SetTop(DragElement, position.Y);
        }

        // Deletes element from workspace
        public void DeleteContainer(GateContainer container)
        {
            WorkSpace.Children.Remove(container);

            // Deleting all connections of the container
            // Starts from end of the list
            int i = container.Connections.Count - 1;

            while (i >= 0)
            {
                DeleteConnection(container.Connections[i]);
                container.Connections[i].Delete();
                i = container.Connections.Count - 1;
            }
            currentSchema.UpdateSchema();
        }

        // Removes graphical connection
        public void DeleteConnection(VisualConnection con)
        {
            WorkSpace.Children.Remove(con);
        }

        // Shuts down app
        private void Exit(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }


        // Connects 2 elements both graphically and logically
        void CreateGateConnection(object sender, MouseEventArgs ea)
        {
            Point pt = ea.GetPosition((UIElement)sender);

            var result = VisualTreeHelper.HitTest(WorkSpace, pt).VisualHit;

            // Gets element just before canvas in tree, without that wrong element(e.g textbox) will be chosen
            while (result != null && VisualTreeHelper.GetParent(result) as Canvas == null)
                result = VisualTreeHelper.GetParent(result);

            if (result is IWorkspaceItem)
            {
                IWorkspaceItem Gate = result as IWorkspaceItem;

                // NOT gate can have only one input, same with output field

                if (Gate is GateContainer)
                    if ((Gate as GateContainer).Gate is NOTGate &&
                        (Gate as GateContainer).Gate.GetInputs.Count > 0)
                    {
                        WorkSpace.MouseLeftButtonDown -= CreateGateConnection;
                        return;
                    }

                if (Gate is OutputFieldContainer)
                    if ((Gate as OutputFieldContainer).Field.GetInputs.Count > 0)
                    {
                        WorkSpace.MouseLeftButtonDown -= CreateGateConnection;
                        return;
                    }

                // Input field cannot have input
                if (Gate is InputFieldContainer)
                {
                    WorkSpace.MouseLeftButtonDown -= CreateGateConnection;
                    return;
                }


                (ConnectedGate as IWorkspaceItem).CreateConnection(Gate);

                VisualConnection connection = new VisualConnection(ConnectedGate, Gate as UserControl);
                (ConnectedGate as IWorkspaceItem).Connections.Add(connection);
                Gate.Connections.Add(connection);
                WorkSpace.Children.Add(connection);
                currentSchema.UpdateSchema();
            }

            // When connection is set no more gates can be connected
            WorkSpace.MouseLeftButtonDown -= CreateGateConnection;
        }

        // Enables possibility to connect 2 gates with click
        public void ConnectionEnable(UserControl gate)
        {
            ConnectedGate = gate;
            WorkSpace.MouseLeftButtonDown += CreateGateConnection;
        }

        private void AddCustomGate(object sender, RoutedEventArgs e)
        {
            ToolBox.Items.Add(new ToolBoxItem(new CustomGate("XD", currentSchema), Colors.Blue, WorkSpace));
            currentSchema = new GateSchema(WorkSpace);

            SetStandartWorkspace();
            currentSchema.UpdateSchema();
        }

        void SetStandartWorkspace()
        {
            WorkSpace.Children.Clear();

            InputFieldContainer cont = new InputFieldContainer();
            Canvas.SetTop(cont, 200);
            Canvas.SetLeft(cont, 50);
            WorkSpace.Children.Add(cont);

            cont = new InputFieldContainer();
            Canvas.SetTop(cont, 100);
            Canvas.SetLeft(cont, 50);
            WorkSpace.Children.Add(cont);

            OutputFieldContainer output = new OutputFieldContainer();
            Canvas.SetTop(output, 150);
            Canvas.SetRight(output, 50);
            WorkSpace.Children.Add(output);
        }
    }
}
