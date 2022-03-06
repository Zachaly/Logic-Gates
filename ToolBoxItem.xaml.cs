using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LogicGates
{
    // Represents an item in toolbox
    public partial class ToolBoxItem : UserControl
    {
        LogicGate Gate;
        Canvas WorkSpace;
        Color Color;
        public ToolBoxItem(LogicGate gate, Color color, Canvas canvas)
        {
            InitializeComponent();
            Label.Content = gate.Name;
            WorkSpace = canvas;
            Color = color;
            Gate = gate;

            try
            {
                GateContainer.GateColours.Add(Gate.Name, Color);
            }
            catch(System.ArgumentException _) { }

            // If this item is for custom gate, adds ability to modify it or remove it
            if(Gate is CustomGate)
            {
                MenuItem DeleteOption = new MenuItem() { Header = "_Delete" };
                DeleteOption.Click += (sender, ea) =>
                {
                    (Parent as Menu).Items.Remove(this);
                };

                MenuItem ModifyOption = new MenuItem() { Header = "_Modify" };
                ModifyOption.Click += (sender, ea) =>
                {
                    (App.Current.MainWindow as MainWindow).FillWorkspaceWithSchema((Gate as CustomGate).Schema);
                };

                Label.ContextMenu = new ContextMenu();
                Label.ContextMenu.Items.Add(DeleteOption);
                Label.ContextMenu.Items.Add(ModifyOption);
            }
        }


        // Creates the type of logic gate that the button represents in the middle of workspace
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                return;

            GateContainer gateContainer = new GateContainer(Gate.Clone(), Color);
            Canvas.SetLeft(gateContainer, WorkSpace.ActualWidth / 2 - this.ActualWidth);
            Canvas.SetTop(gateContainer, WorkSpace.ActualHeight / 2 - this.ActualHeight);
            WorkSpace.Children.Add(gateContainer);
            MainWindow.CurrentSchema.UpdateSchema();
            Gate.Cloned = false;
        }
    }
}
