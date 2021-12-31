using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Studio
    {
        [PrimaryKey, AutoIncrement]
        public int StudioId { get; set; }
        public string Name { get; set; }
        [Ignore]
        public bool IsChecked { get; set; }

        public Studio()
        {

        }

        public Studio(string name)
        {
            Name = name;
            IsChecked = false; 
        }
    }
}
