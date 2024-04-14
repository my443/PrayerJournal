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
            
            makeList();
        }

        private void makeList() { 
            PrayerItem item = new PrayerItem();
            item.Summary = "Pray that this software has an impact";
            item.Description = "I pray this software would have an impact.";
            item.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
            _currentItems.Add(item);
        }
    }
}