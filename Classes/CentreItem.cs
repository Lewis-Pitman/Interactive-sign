using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Interactive_sign.Classes
{
    public class CentreItem
    {
        [PrimaryKey]
        public int ItemID { get; set; }
        public string? ItemTitle { get; set; }
        public string? ItemAddress { get; set; }
        public int ItemOpenTime { get; set; }
        public int ItemCloseTime { get; set; }
        public string? ItemCategory { get; set; }
        public byte[] ItemImage { get; set; }
        

    }
}
