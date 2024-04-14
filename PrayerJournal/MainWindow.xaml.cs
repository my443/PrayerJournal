using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrayerJournal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<PrayerItem> _currentItems = new ObservableCollection<PrayerItem>();
        public ObservableCollection<PrayerItem> _historyItems = new ObservableCollection<PrayerItem>();
        public MainWindow()
        {
            InitializeComponent();
            _currentItems = new PrayerItemList().GetCurrentItems();
            _historyItems = new PrayerItemList().GetHistoryItems();
            
            listboxCurrentItems.ItemsSource = _currentItems;
            listboxHistoryItems.ItemsSource = _historyItems;

            initialUIConfiguration();
            
            makeList();
        }

        private void initialUIConfiguration() { 
            listboxCurrentItems.SelectedIndex = 0;
            listboxCurrentItems.Focus();
            listboxHistoryItems.SelectedIndex = 0;
        }
        private void makeList() { 
            PrayerItem item = new PrayerItem();
            item.Summary = "Pray that this software has an impact";
            item.Description = "I pray this software would have an impact.";
            item.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
            _currentItems.Add(item);
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Binding bindingSummary = new Binding();
            Binding bindingDescription = new Binding();

            if (tabControl.SelectedIndex == 0)
            {
                //bindingSummary.Source = _currentItems;
                bindingSummary.ElementName = "listboxCurrentItems";
                bindingDescription.ElementName = "listboxCurrentItems";
                listboxCurrentItems.Focus();
            }
            else
            {
                //bindingDescription.Source = _historyItems;
                bindingSummary.ElementName = "listboxHistoryItems";
                bindingDescription.ElementName = "listboxHistoryItems";
                listboxHistoryItems.Focus();
                //binding.ElementName = "listboxHistoryItems";

            }
            bindingSummary.Path = new PropertyPath("SelectedItem.Summary");
            textboxSummary.SetBinding(TextBox.TextProperty, bindingSummary);

            bindingDescription.Path = new PropertyPath("SelectedItem.Description");
            textboxDescription.SetBinding(TextBox.TextProperty, bindingDescription);

        }
    }
}