﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayerJournal
{
    public class PrayerItemList : ObservableCollection<PrayerItem>
    {
        private ObservableCollection<PrayerItem> CurrentItems { get; set; }
        private ObservableCollection<PrayerItem> HistoryItems {  get; set; }

        public PrayerItemList() { 
            CurrentItems = new ObservableCollection<PrayerItem>();
            HistoryItems = new ObservableCollection<PrayerItem>();
            generateSample(10, 5);
        }
        public ObservableCollection<PrayerItem> GetCurrentItems()
        {
            return CurrentItems;
        }

        public ObservableCollection<PrayerItem> GetHistoryItems()
        {
            return HistoryItems;
        }

        private void generateSample(int countForCurrent, int countForHistory) { 
            for (int i=0; i<countForCurrent;i++) { 
                PrayerItem item = new PrayerItem { Summary = "Pray", Description="For this to make an impact"};
                CurrentItems.Add(item);
            }
            for (int i = 0; i < countForHistory; i++)
            {
                PrayerItem item = new PrayerItem { Summary = "Pray", Description = "For this to make an impact" };
                HistoryItems.Add(item);
            }
        }
    }
}