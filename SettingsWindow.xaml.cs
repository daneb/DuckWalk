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

namespace DuckWalk
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addSearchEngineBtn_Click(object sender, RoutedEventArgs e)
        {
            DataTransfer dataTransfer= new DataTransfer();

            if (EngineNameBox.Text.Length > 0 && EnginePrefixBox.Text.Length > 0)
                if (dataTransfer.AddSearchEngine(EngineNameBox.Text, EnginePrefixBox.Text))
                    MessageBox.Show("You added a search engine successfully", "Added Search Engine");
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            DataTransfer dataTransfer= new DataTransfer();
            var listOfSearchEngine = dataTransfer.GetSearchEngines();
            listOfSearchEngine.ForEach(x => SearchEngineComboBox.Items.Add(x));

        }
    }
}
