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
using CefSharp;
using CefSharp.Wpf;

namespace DuckWalk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int tabCount = 0;

        TabItem currentTabItem = null;
        ChromiumWebBrowser currentBrowser = null;
        string landingPage = "https://www.duckduckgo.com";

        public MainWindow()
        {
            InitializeComponent();

            DataTransfer dataTransfer= new DataTransfer();
            dataTransfer.CreateXmlFile();
        }

        private void newTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
            browser.Name = "browser_" + tabCount;

            tabControl.Items.Add(tabItem);
            tabItem.Name = "tab_" + tabCount;
            tabCount++;

            tabItem.Content = browser;
            browser.Address = landingPage;

            tabItem.Header = $"New Blank Page ({tabCount})";

            tabControl.SelectedItem = tabItem;
            
            browser.Loaded += FinishedLoadingWebPage;
            browser.AddressChanged += Browser_AddressChanged;

            currentTabItem = tabItem;
            currentBrowser = browser;

        }

        private void Browser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SiteRating siteRating = new SiteRating();
            var sndr = sender as ChromiumWebBrowser;

            string rating = siteRating.RateSite(sndr.Address);
            AddressBar.Text = sndr.Address;

            currentBrowser = sndr;
 
        }

        private void FinishedLoadingWebPage(object sender, RoutedEventArgs e)
        {
            var sndr = sender as ChromiumWebBrowser;
            AddressBar.Text = sndr.Address.ToString();

            if (currentTabItem != null)
            {
               currentTabItem.Header = sndr.Address.Substring(8);
            }
            
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void bkForward_Click(object sender, RoutedEventArgs e)
        {
            if (currentBrowser.CanGoForward)
                currentBrowser.ForwardCommand.Execute(sender);
        }

        private void bkBack_Click(object sender, RoutedEventArgs e)
        {
            if (currentBrowser.CanGoBack)
                currentBrowser.BackCommand.Execute(sender);
        }

        private void bkRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentBrowser.ReloadCommand.Execute(sender);
        }

        private void wbTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem != null)
            {
                currentTabItem = tabControl.SelectedItem as TabItem;
            }

            if (currentTabItem != null)
            {
                currentBrowser = currentTabItem.Content as ChromiumWebBrowser;
            }
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        private void Search()
        {
            if(currentBrowser != null && AddressBar.Text != String.Empty)
            {
                currentBrowser.Address = $"{landingPage}/?q={AddressBar.Text}";
            }
        }

        private void settingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings= new SettingsWindow();
            settings.ShowDialog();
        }
    }
}
