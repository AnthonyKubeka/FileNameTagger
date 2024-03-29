﻿

using Domain;
using Repository;
using Shared;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FileNameTagger.TagTypes
{
    public class TextListTagTypeViewModel : BaseViewModel, ITagTypeViewModel
    {
        private ObservableCollection<Tag> tags;
        private Tag selectedTag;

        public TagType TagType //should be passed in from parent view and doesn't change
                               //the TagTypeTypeId determines what TagTypeView to bind to this viewmodel
        { get; private set; }

        public ObservableCollection<Tag> Tags
        {
            get { return tags; }
            set { SetProperty(ref tags, value); }
        }

        public Tag SelectedTag
        {
            get { return selectedTag; }
            set { SetProperty(ref selectedTag, value); }
        }

        public RelayCommand<string> AddTagCommand { get; private set; }
        public RelayCommand<Tag> UpdateTagDataCommand { get; private set; }
        public RelayCommand<Tag> DeleteTagCommand { get; private set; }

        public TextListTagTypeViewModel()
        {

        }

        public TextListTagTypeViewModel(TagType tagType, IEnumerable<Tag> tags)
        {
            TagType = tagType;
            AddTagCommand = new RelayCommand<string>(AddTag);
            UpdateTagDataCommand = new RelayCommand<Tag>(UpdateTag);
            DeleteTagCommand = new RelayCommand<Tag>(DeleteTag);
            Tags = new ObservableCollection<Tag>(tags);
        }

        public void AddTag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                return;

            var tag = new Tag(TagType.Name, tagName);
            Tags.Add(tag);
            TagType.Tags = Tags;
        }
        public void UpdateTag(Tag tag)
        {
            if (tag == null)
                return;

            TagType.Tags = Tags;
        }

        public void DeleteTag(Tag tag)
        {
            if (tag == null)
                return;
            Tags.Remove(tag);
            TagType.Tags = Tags; 
        }

        public string ToString()
        {
            var result = "";
            foreach (var tag in Tags)
            {
                if (tag.IsChecked)
                result += $"{tag.Value}-";
            }

            var tagString = "";
            if (result.Length > 0)
            {
               tagString = result.Remove(result.Length - 1, 1);
            }

            return tagString; 
        }

        public int GetTagTypeTypeId()
        {
            return this.TagType.TagTypeTypeId;
        }
    }
}
