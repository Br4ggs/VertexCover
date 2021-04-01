using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for GenerateWindow.xaml
    /// </summary>
    public partial class GenerateWindow : Window
    {
        public int Nodes { get; private set; }
        public int Density { get; private set; }

        public GenerateWindow()
        {
            Nodes = 0;
            Density = 0;
            InitializeComponent();
        }

        private void NodesTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void NodesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Nodes = Int32.Parse((sender as TextBox).Text);
        }

        private void DensitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Density = (int)e.NewValue;
            DensityIndicator.Text = Density.ToString() + "%";
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
