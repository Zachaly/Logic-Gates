using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Symulator_układów_logicznych
{
    /// <summary>
    /// Logika interakcji dla klasy GateContainer.xaml
    /// </summary>
     
    // class for a graphical field containg the logic gate 
    partial class GateContainer : UserControl
    {
        public LogicGate Gate;

        public GateContainer()
        {
            InitializeComponent();
        }

        public GateContainer(LogicGate gate, Color color)
        {
            InitializeComponent();
            Gate = gate;
            ContentLabel.Content = gate.Name;
            ContentLabel.Background = new SolidColorBrush(color);
        }

        // creates connection between two boxes, differs if you connect input with output and vice versa
        void CreateConnection(GateContainer target, bool isThisInput)
        {
            if (isThisInput)
                target.Gate.ConnectWith(Gate);
            else
                Gate.ConnectWith(target.Gate);
            
        }

        private void Content_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
        }
    }
}
