using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace PrayerJournal
{
    public class PrayerItemList : ObservableCollection<PrayerItem>
    {
        private ObservableCollection<PrayerItem> CurrentItems { get; set; }
        private ObservableCollection<PrayerItem> HistoryItems {  get; set; }
        PrayerItemsContext db = new PrayerItemsContext();

        public PrayerItemList() { 
            
            HistoryItems = new ObservableCollection<PrayerItem>();

            var currentItems = db.PrayerItems.Where(Item => Item.IsHistory == false).ToList();
            var historyItems = db.PrayerItems.Where(Item => Item.IsHistory == true).ToList();
            CurrentItems = new ObservableCollection<PrayerItem>(currentItems);
            HistoryItems = new ObservableCollection<PrayerItem>(historyItems);
            //using (PrayerItemsContext context = new PrayerItemsContext())
            //{
            //    var items = context.PrayerItems.ToList();
            //}
            //generateSample(10, 5);
        }
        public ObservableCollection<PrayerItem> GetCurrentItems()
        {
            
            return CurrentItems;
        }

        public ObservableCollection<PrayerItem> GetHistoryItems()
        {
            return HistoryItems;
        }

        public void AddItemToCurrentItems(PrayerItem item)
        {
            CurrentItems.Add(item);
        }

        private void generateSample(int countForCurrent, int countForHistory) { 
            for (int i=0; i<countForCurrent;i++) { 
                PrayerItem item = new PrayerItem { Summary = "Pray", Description="For this to make an impact"};
                CurrentItems.Add(item);
            }
            for (int i = 0; i < countForHistory; i++)
            {
                PrayerItem item = new PrayerItem { Summary = "Pray History", Description = "For this to make an impact" };
                item.IsHistory = true;
                HistoryItems.Add(item);
            }
        }
    }
}
