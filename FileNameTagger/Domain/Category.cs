using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        [Ignore]
        public bool IsChecked { get; set; }

        public Category()
        {

        }

        public Category(string name)
        {
            Name = name;
            IsChecked = false;
        }
    }
}
