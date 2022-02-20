using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Symulator_układów_logicznych
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
        }


        // Creates the type of logic gate that the button represents in the middle of workspace
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GateContainer gateContainer = new GateContainer(Gate.Clone(), Color);
            Canvas.SetLeft(gateContainer, WorkSpace.ActualWidth / 2 - this.ActualWidth);
            Canvas.SetTop(gateContainer, WorkSpace.ActualHeight / 2 - this.ActualHeight);
            WorkSpace.Children.Add(gateContainer);
            MainWindow.currentSchema.UpdateSchema();
        }
    }
}
