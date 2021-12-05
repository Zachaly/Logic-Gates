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
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WorkSpace_Drop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData(DataFormats.Serializable);

            if (obj is UIElement)
            {
                Point position = e.GetPosition(WorkSpace);

                Canvas.SetLeft((UIElement)obj, position.X);
                Canvas.SetTop((UIElement)obj, position.Y);
                WorkSpace.Children.Add((UIElement)obj);
            }
        }

        private void WorkSpace_DragOver(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData(DataFormats.Serializable);

            if (obj is UIElement)
            {
                Point position = e.GetPosition(WorkSpace);

                Canvas.SetLeft((UIElement)obj, position.X);
                Canvas.SetTop((UIElement)obj, position.Y);
                WorkSpace.Children.Add((UIElement)obj);
            }
        }
    }
}
