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
        private ObservableCollection<PrayerItem> _items = new ObservableCollection<PrayerItem>();
        public MainWindow()
        {
            InitializeComponent();
            
            makeList();
            listboxCurrentItems.ItemsSource = _items;
            
        }

        private void makeList() { 
            PrayerItem item = new PrayerItem();
            item.Summary = "Pray that this software has an impact";
            item.Description = "I pray this software would have an impact.";
            item.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
            _items.Add(item);
        }
    }
}