using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayerJournal
{
    public class PrayerItem
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateOnly CreatedDate{ get; set; }
        public Boolean IsHistory { get; set; }

    }
}
