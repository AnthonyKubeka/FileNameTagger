

using Domain;
using Repository;
using Shared;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FileNameTagger.TagTypes
{
    public class DateTagTypeViewModel : BaseViewModel, ITagTypeViewModel
    {
        private Tag dateTag;
        private DateTime date;
        private bool yearOnly; 

        public TagType TagType { get; private set; }

        public Tag DateTag
        {
            get { return dateTag; }
            set { SetProperty(ref dateTag, value); }
        }

        public DateTime Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }   
        }
        public Visibility DatePickerVisibility { get; set; }
        public Visibility YearComboBoxVisibility { get; set; }

        public bool YearOnly
        {
            get
            {
                return yearOnly;
            }

            set
            {
                if (yearOnly != value)
                {
                    if (yearOnly)
                    {
                        DatePickerVisibility = Visibility.Hidden;
                        YearComboBoxVisibility = Visibility.Visible;
                    }
                    else
                    {
                        DatePickerVisibility = Visibility.Visible;
                        YearComboBoxVisibility = Visibility.Hidden;
                    }
                    SetProperty(ref yearOnly, value);
                    /*PropertyChanged(this, new PropertyChangedEventArgs("DatePickerVisibility"));
                    PropertyChanged(this, new PropertyChangedEventArgs("YearComboBoxVisibility"));*/
                }
            }
        }

        public DateTagTypeViewModel()
        {
        }

        public DateTagTypeViewModel(TagType tagType)
        {
            DatePickerVisibility = Visibility.Visible;
            YearComboBoxVisibility = Visibility.Hidden;
            TagType = tagType;
            DateTag = new Tag(tagType.Name, DateTime.Now.ToString());
            Date = Convert.ToDateTime(DateTag.Value);
        }

        public string ToString()
        {
            return YearOnly ? Date.Year.ToString() : Date.ToString("yyyy.MM.dd");
        }

        public int GetTagTypeTypeId()
        {
            return this.TagType.TagTypeTypeId;
        }
    }
}
