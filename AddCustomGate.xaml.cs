using System.Windows;
using System.Windows.Media;

namespace LogicGates
{
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
            this.name = nameTextBox.Text;

            ((MainWindow)Application.Current.MainWindow).CustomGateName = this.name;

            ((MainWindow)Application.Current.MainWindow).CustomGateColor = this.color;

            ((MainWindow)Application.Current.MainWindow).CustomGateSet = true;

            DialogResult = true;
        }
        private void cp_SelectedColorChanged_1(object sender, RoutedEventArgs e)
        {
            if (picker.SelectedColor.HasValue)
            {
                color = picker.SelectedColor.Value;
            }
        }
    }
}
