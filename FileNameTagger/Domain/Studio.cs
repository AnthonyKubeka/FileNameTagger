using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Studio : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private bool isChecked;
        [PrimaryKey, AutoIncrement]
        public int StudioId { get; set; }
        public string Name { get; set; }
        [Ignore]
        public bool IsChecked
        {
            get
            {
                return isChecked; 
            }

            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                }
            }
        }

        public Studio()
        {

        }

        public Studio(string name)
        {
            Name = name;
            IsChecked = false; 
        }


        public void Update(bool isChecked)
        {
            this.IsChecked = isChecked; 
        }
    }
}
