using System.Collections.Generic;
using Shared;
using System;
using System.Linq;
using System.ComponentModel;

namespace Domain
{
    public class Tag : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string TagTypeName { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }

        public Tag(string tagTypeName, string value)
        {
            TagTypeName = tagTypeName; 
            Value = value;
            IsChecked = false;
        }

    }
}
