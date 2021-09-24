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

namespace DuckWalk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int tabCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void newTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            wbTabControl.Items.Add(tabItem);
            tabItem.Name = "tab_" + tabCount;
            tabCount++;

            tabItem.Header = $"New Blank Page ({tabCount})";

        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void bkForward_Click(object sender, RoutedEventArgs e)
        {
            if (defaultBrowser.CanGoForward)
                defaultBrowser.ForwardCommand.Execute(sender);
        }

        private void bkBack_Click(object sender, RoutedEventArgs e)
        {
            if (defaultBrowser.CanGoBack)
                defaultBrowser.BackCommand.Execute(sender);
        }

        private void bkRefresh_Click(object sender, RoutedEventArgs e)
        {
            defaultBrowser.ReloadCommand.Execute(sender);
        }
    }
}
