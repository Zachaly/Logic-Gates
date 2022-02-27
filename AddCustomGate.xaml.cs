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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.AvalonDock;
using System.Windows.Media;


namespace Symulator_układów_logicznych
{
    /// <summary>
    /// Logika interakcji dla klasy AddCustomGate.xaml
    /// </summary>
    public partial class AddCustomGate : Window
    {
        string name;
        Color color;
        public AddCustomGate(string name)
        {
            this.name = name;
            InitializeComponent();
        }

        private void createBt_Click(object sender, RoutedEventArgs e)
        {
            this.name = nameTb.Text;

            ((MainWindow)Application.Current.MainWindow).customGateName = this.name;

            ((MainWindow)Application.Current.MainWindow).customGateColor = this.color;

            ((MainWindow)Application.Current.MainWindow).customGateSet = true;

            DialogResult = true;
        }
        private void cp_SelectedColorChanged_1(object sender, RoutedEventArgs e)
        {
            if (cp.SelectedColor.HasValue)
            {
                color = cp.SelectedColor.Value;
            }
        }
    }
}
