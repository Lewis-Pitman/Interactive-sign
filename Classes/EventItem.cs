using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Interactive_sign.Classes
{
    public class EventItem
    {
        [PrimaryKey]
        public int EventID { get; set; }
        public int ItemID { get; set; }
        public string? EventTitle { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string? Date { get; set; }
        public string? En_Description { get; set; }
        public string? Cy_Description { get; set; }
        public string? Fr_Description { get; set; }
        public string? De_Description { get; set; }
        public byte[] EventImage { get; set; }


    }
}
