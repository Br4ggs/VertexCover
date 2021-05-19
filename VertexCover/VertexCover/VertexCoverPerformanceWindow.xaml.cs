using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace VertexCover
{
    /// <summary>
    /// Interaction logic for VertexCoverPerformanceWindow.xaml
    /// </summary>
    public partial class VertexCoverPerformanceWindow : Window
    {
        private readonly Regex numberRegex = new Regex("[^0-9]+");
        private int maxSize;
        private int incrementSize;
        private int repetitions;
        private int kPercentage;
        private readonly VertexCoverPerformance performance;

        public VertexCoverPerformanceWindow(VertexCoverPerformance performance)
        {
            InitializeComponent();
            this.performance = performance;
        }

        private void KPercentageSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            kPercentage = (int)e.NewValue;
            KPercentageIndicator.Text = $"{kPercentage}%";
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numberRegex.IsMatch(e.Text);
        }
        private void MaxSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse((sender as TextBox)?.Text, out var value))
                maxSize = value;
        }

        private void IncrementSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse((sender as TextBox)?.Text, out var value))
                incrementSize = value;
        }

        private void RepetitionsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse((sender as TextBox)?.Text, out var value))
                repetitions = value;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DefaultExt = ".csv"
            };

            float percentageForK = kPercentage / 100f;
            var values = performance.PerformanceTest(maxSize, incrementSize, repetitions, percentageForK);

            if (fileDialog.ShowDialog() == true)
            {
                File.WriteAllText(fileDialog.FileName, ToCSV(values));
                Close();
            }
        }

        private static string ToCSV(IEnumerable<PerformanceTest> tests)
        {
            string csv = "Graph Size, K Size, Regular Speed, Preprocessed Speed\n";
            foreach (PerformanceTest performanceTest in tests)
            {
                csv += $"{performanceTest}\n";
            }
            return csv;
        }
    }
}
