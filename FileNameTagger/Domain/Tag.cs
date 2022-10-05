using System.Collections.Generic;
using Shared;
using System;
using System.Linq;
using SQLite;
using System.ComponentModel;

namespace Domain
{
    public class Tag : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        [PrimaryKey, AutoIncrement]
        public int TagId { get; set; }
        public int TagTypeId { get; set; }
        public string Value { get; set; }
        [Ignore]
        public bool IsChecked { get; set; }

        public Tag()
        {

        }

        public Tag(int tagTypeId, string value)
        {
            TagTypeId = tagTypeId; 
            Value = value;
            IsChecked = false;
        }

    }
}
