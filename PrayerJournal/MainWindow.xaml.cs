using Microsoft.EntityFrameworkCore;
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
        public int _currentItemsIndex = 0;
        PrayerItemsContext db = new PrayerItemsContext();
        public MainWindow()
        {
            InitializeComponent();
            startupConfiguration();
            initialUIConfiguration();
        }

        private void startupConfiguration()
        {
            _currentItems = new PrayerItemList().GetCurrentItems();
            _historyItems = new PrayerItemList().GetHistoryItems();

            listboxCurrentItems.ItemsSource = _currentItems;
            listboxHistoryItems.ItemsSource = _historyItems;
            
            _currentItemsIndex = listboxCurrentItems.SelectedIndex;
        }

        private void initialUIConfiguration()
        {
            listboxCurrentItems.SelectedIndex = 0;
            listboxHistoryItems.SelectedIndex = 0;
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Binding bindingSummary = new Binding();
            Binding bindingDescription = new Binding();
            Binding bindingCheckbox = new Binding();
            Binding bindingDate = new Binding();

            if (tabControl.SelectedIndex == 0)
            {
                //bindingSummary.Source = _currentItems;
                bindingSummary.ElementName = "listboxCurrentItems";
                bindingDescription.ElementName = "listboxCurrentItems";
                bindingCheckbox.ElementName = "listboxCurrentItems";
                bindingDate.ElementName = "listboxCurrentItems";
                listboxCurrentItems.Focus();
            }
            else
            {
                //bindingDescription.Source = _historyItems;
                bindingSummary.ElementName = "listboxHistoryItems";
                bindingDescription.ElementName = "listboxHistoryItems";
                bindingCheckbox.ElementName = "listboxHistoryItems";
                bindingDate.ElementName = "listboxHistoryItems";
                listboxHistoryItems.Focus();
                //binding.ElementName = "listboxHistoryItems";

            }
            bindingSummary.Path = new PropertyPath("SelectedItem.Summary");
            textboxSummary.SetBinding(TextBox.TextProperty, bindingSummary);

            bindingDescription.Path = new PropertyPath("SelectedItem.Description");
            textboxDescription.SetBinding(TextBox.TextProperty, bindingDescription);

            bindingCheckbox.Path = new PropertyPath("SelectedItem.IsHistory");
            checkboxIsHistory.SetBinding(CheckBox.IsCheckedProperty, bindingCheckbox);

            bindingDate.Path = new PropertyPath("SelectedItem.CreatedDate");
            datepickerCreatedDate.SetBinding(DatePicker.SelectedDateProperty, bindingDate);
        }

        private void Add_PrayerItem(object sender, RoutedEventArgs e)
        {
            PrayerItem prayerItem = new PrayerItem();
            _currentItems.Add(prayerItem);

            tabControl.SelectedIndex = 0;

            //_currentItemsIndex = _currentItems.Count - 1;
            //listboxCurrentItems.SelectedIndex = _currentItemsIndex;

            listboxCurrentItems.SelectedIndex = _currentItems.Count - 1;
            textboxSummary.Focus();

            db.PrayerItems.Attach(prayerItem);
            db.SaveChanges();
            //listboxCurrentItems.SelectedItem = _currentItems[_currentItems.Count - 1];
        }

        /// <summary>
        /// When a user focuses on the Summary textbox, it selects all of the text. 
        /// [Card: User Interaction Improvements]
        /// </summary>
        private void textboxSummary_GotFocus(object sender, RoutedEventArgs e)
        {
            //var txtControl = sender as TextBox;
            textboxSummary.Dispatcher.BeginInvoke(new Action(() =>
            {
                textboxSummary.SelectAll();
            }));
            saveCurrentlySelectedItem();

        }

        private void textboxSummary_SaveItem(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && tabControl.SelectedIndex == 0)
            {
                //PrayerItem item = (PrayerItem)listboxCurrentItems.SelectedItem;
                //db.Update(item);
                //db.SaveChanges();
                textboxDescription.Focus();
                textboxSummary.Focus();
                saveCurrentlySelectedItem();
            }
            else if (e.Key == Key.Enter && tabControl.SelectedIndex == 1)
            {
                textboxDescription.Focus();
                textboxSummary.Focus();
                saveCurrentlySelectedItem();
            }
        }

        private void Save_PrayerItem(object sender, RoutedEventArgs e)
        {
            textboxDescription.Focus();
            textboxSummary.Focus();
            saveCurrentlySelectedItem();

        }

        private void Delete_PrayerItem(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result = MessageBox.Show("Do you want to permanently delete this item?", "Deleting Selected Item", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (tabControl.SelectedIndex == 0 && Result == MessageBoxResult.Yes)
            {
                int currentIndex = listboxCurrentItems.SelectedIndex;
                PrayerItem selectedItem = listboxCurrentItems.SelectedItem as PrayerItem;
                _currentItems.Remove(selectedItem);
                db.Remove(selectedItem);
                listboxCurrentItems.SelectedIndex = getReturnIndexAfterItemDeleted(currentIndex);

            }
            else if (tabControl.SelectedIndex == 1 && Result == MessageBoxResult.Yes)
            {
                int historyIndex = listboxHistoryItems.SelectedIndex;
                PrayerItem selectedItem = listboxHistoryItems.SelectedItem as PrayerItem;
                _historyItems.Remove(selectedItem);
                db.Remove(selectedItem);
                listboxHistoryItems.SelectedIndex = getReturnIndexAfterItemDeleted(historyIndex);
            }
            textboxSummary.Focus();
        }

        /// <summary>
        /// Returns a valid index for after an item has been deleted.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int getReturnIndexAfterItemDeleted(int index)
        {
            int returnIndex = 0;
            if (index <= 0)
            {
                returnIndex = 0;
            }

            if (index > 0)
            {
                returnIndex = index - 1;
            }
            return returnIndex;
        }

        private void History_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // This has to be done so that the messagebox doesn't pop up when the app is initialized.
            MessageBoxResult Result = MessageBoxResult.No;

            if (tabControl.SelectedIndex == 0)
            {
                Result = MessageBox.Show("Do you want to move this item to the history list?", "Move Item To History List", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }

            // Move the item
            if (tabControl.SelectedIndex == 0 && Result == MessageBoxResult.Yes)
            {
                int currentIndex = listboxHistoryItems.SelectedIndex;
                PrayerItem currentItem = getCurrentlySelectedItem();
                _currentItems.Remove(currentItem);
                _historyItems.Add(currentItem);
                currentItem.IsHistory = true;
                db.Update(currentItem);
                db.SaveChanges();
                listboxCurrentItems.SelectedIndex = getReturnIndexAfterItemDeleted(currentIndex);
            }
        }

        private PrayerItem getCurrentlySelectedItem()
        {
            PrayerItem currentItem = (PrayerItem)listboxCurrentItems.SelectedItem;
            return currentItem;
        }

        private void saveCurrentlySelectedItem()
        {
            PrayerItem item = getCurrentlySelectedItem();
            db.Update(item);
            db.SaveChanges();
        }

        private void checkboxIsHistory_Unchecked(object sender, RoutedEventArgs e)
        {
            // This has to be done so that the messagebox doesn't pop up when the app is initialized.
            MessageBoxResult Result = MessageBoxResult.No;
            if (tabControl.SelectedIndex == 1)
            {
                Result = MessageBox.Show("Do you want to move this item to the current list?", "Move Item To Current List", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }

            // Move the item
            if (tabControl.SelectedIndex == 1 && Result == MessageBoxResult.Yes)
            {
                int historyIndex = listboxHistoryItems.SelectedIndex;
                PrayerItem currentItem = (PrayerItem)listboxHistoryItems.SelectedItem;
                _historyItems.Remove(currentItem);
                _currentItems.Add(currentItem);
                currentItem.IsHistory = false;
                db.Update(currentItem);
                db.SaveChanges();
                listboxHistoryItems.SelectedIndex = getReturnIndexAfterItemDeleted(historyIndex);
            }
        }

        private void listboxCurrentItems_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
    }
}