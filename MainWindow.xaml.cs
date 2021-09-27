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

            if (Environment.GetEnvironmentVariable("ABUSEIPDB_API_KEY") == null)
            {
                MessageBox.Show("Missing Site Rating API Key");
                App.Current.Shutdown(1);
            } 
            else
            {
                DataTransfer dataTransfer= new DataTransfer();
                dataTransfer.CreateXmlFile();
            }
        }

        private void newTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NewBrowser();
        }

        private void Browser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SiteRating siteRating = new SiteRating();
            var sndr = sender as ChromiumWebBrowser;

            string ratingImage = siteRating.RateSite(sndr.Address);
            var uriSource = new Uri(ratingImage, UriKind.Relative);
            RatingImage.Source = new BitmapImage(uriSource);
            AddressBar.Text = sndr.Address;
            currentTabItem.Header = sndr.Address.Substring(8);

            currentBrowser = sndr;
 
        }

        private void FinishedLoadingWebPage(object sender, RoutedEventArgs e)
        {
            var sndr = sender as ChromiumWebBrowser;

            if (sndr?.Address.ToString().Length > 0)
                AddressBar.Text = sndr?.Address.ToString();

            if (currentTabItem != null)
                currentTabItem.Header = sndr?.Address.Substring(8);

        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            var landingPageTabItem = sender as TabItem;
            NewBrowser(landingPageTabItem);
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
                if (AddressBar.Text.StartsWith("http://") || AddressBar.Text.StartsWith("https://"))
                    currentBrowser.Address = AddressBar.Text;
                else
                    currentBrowser.Address = $"{landingPage}/?q={AddressBar.Text}";
            }

            currentTabItem.Header = AddressBar.Text;
        }

        private void settingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings= new SettingsWindow();
            settings.ShowDialog();
        }

        private void NewBrowser(TabItem tabItem = null)
        {
            if (tabItem == null)
            {
                tabItem = new TabItem();
                tabItem.Loaded += FinishedLoadingWebPage;
                tabControl.Items.Add(tabItem);
                tabItem.Name = "tab_" + tabCount;
                tabCount++;

            }
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
            browser.Name = "browser_" + tabCount;

            tabItem.Content = browser;
            browser.Address = landingPage;

            tabItem.Header = $"New Blank Page ({tabCount})";

            tabControl.SelectedItem = tabItem;

            currentTabItem = tabItem;
            currentBrowser = browser;

            browser.Loaded += FinishedLoadingWebPage;
            browser.AddressChanged += Browser_AddressChanged;

        }

    }
}
