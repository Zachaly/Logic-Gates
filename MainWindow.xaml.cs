using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace Symulator_układów_logicznych
{
    partial class MainWindow : Window
    {
        public static UIElement DragElement { get; set; } // element used in drag and drop
        internal static GateSchema CurrentSchema { get; set; } // elements currently in workspace
        UserControl ConnectedGate; // element that connects to another in this moment

        public MainWindow()
        {
            InitializeComponent();
            ToolBox.Items.Add(new ToolBoxItem(new ANDGate(), Colors.Green, WorkSpace));
            ToolBox.Items.Add(new ToolBoxItem(new NOTGate(), Colors.Brown, WorkSpace));
            CurrentSchema = new GateSchema(WorkSpace);
        }

        #region DragAndDrop

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
        #endregion

        #region ModifyElementsInWorkspace
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
            CurrentSchema.UpdateSchema();
        }

        // Removes graphical connection
        public void DeleteConnection(VisualConnection con)
        {
            WorkSpace.Children.Remove(con);
            CurrentSchema.UpdateSchema();
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
                // Custom gate cannot have more inputs than number of input buffers

                if((Gate.Gate is NOTGate && Gate.Gate.GetInputs.Count > 0)
                    || (Gate.Gate is CustomGate && (Gate.Gate as CustomGate).GetInputs.Count >= (Gate.Gate as CustomGate).Schema.Inputs.Count))
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
                CurrentSchema.UpdateSchema();
            }

            // When connection is set no more gates can be connected
            WorkSpace.MouseLeftButtonDown -= CreateGateConnection;
        }

        // Enables possibility to connect 2 gates with click
        public void ConnectionEnable(UserControl gate)
        {
            ConnectedGate = gate;
            WorkSpace.MouseLeftButtonDown -= CreateGateConnection;
            WorkSpace.MouseLeftButtonDown += CreateGateConnection;
        }
        #endregion

        #region SettingWorkspace

        public string customGateName = "";
        public Color customGateColor = new Color();
        public bool customGateSet = false;
        public void AddCustomGate(object sender, RoutedEventArgs e)
        {
            AddCustomGate newWindow = new AddCustomGate(customGateName);
            newWindow.ShowDialog();
            if (customGateSet)
            {
                ToolBox.Items.Add(new ToolBoxItem(new CustomGate(customGateName, CurrentSchema), customGateColor, WorkSpace));
                CurrentSchema = new GateSchema(WorkSpace);
                SetStandartWorkspace(2);
                CurrentSchema.UpdateSchema();
            }
            customGateSet = false;
        }

        public void SetStandartWorkspace(int n)
        {
            WorkSpace.Children.Clear();

            createInput(n);

            OutputFieldContainer output = new OutputFieldContainer();
            Canvas.SetTop(output, 150);
            Canvas.SetRight(output, 50);
            WorkSpace.Children.Add(output);
        }

        public void FillWorkspaceWithSchema(GateSchema schema)
        {
            schema.FillWorkspaceWithSchema();
            CurrentSchema.UpdateSchema();
            var containers = from UIElement el in WorkSpace.Children where el is GateContainer select el as GateContainer;
        }

        void createInput(int n)
        {
            for (int i = 0; i < n; i++)
            {
                InputFieldContainer cont = new InputFieldContainer();
                Canvas.SetTop(cont, 10 + i * 70);
                Canvas.SetLeft(cont, 50);
                WorkSpace.Children.Add(cont);
            }
        }

        void numberOfGates2(object sender, RoutedEventArgs e)
        {
            SetStandartWorkspace(2);
        }
        void numberOfGates4(object sender, RoutedEventArgs e)
        { 
            SetStandartWorkspace(4);
        }
        void numberOfGates8(object sender, RoutedEventArgs e)
        {
            SetStandartWorkspace(8);
        }

        void ClearSchema(object sender, RoutedEventArgs e)
        {
            SetStandartWorkspace((from UIElement el in WorkSpace.Children where el is InputFieldContainer select el).ToList().Count);
        }
        #endregion

        // Shuts down app
        private void Exit(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}

            