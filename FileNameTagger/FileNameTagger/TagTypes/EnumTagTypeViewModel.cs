

using Domain;
using Repository;
using Shared;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FileNameTagger.TagTypes
{
    public class EnumTagTypeViewModel : BaseViewModel, ITagTypeViewModel
    {
        private Tag selectedEnumTag;
        private ObservableCollection<Tag> enumTags; 

        public TagType TagType { get; private set; }

        public ObservableCollection<Tag> EnumTags
        {
            get { return enumTags; }
            set { SetProperty(ref enumTags, value); }
        }

        public Tag SelectedEnumTag
        {
            get { return selectedEnumTag; }
            set { SetProperty(ref selectedEnumTag, value); }
        }

        public RelayCommand<string> AddTagCommand { get; private set; }
        public RelayCommand<Tag> UpdateTagDataCommand { get; private set; }
        public RelayCommand<Tag> DeleteTagCommand { get; private set; }

        public EnumTagTypeViewModel()
        {

        }

        public EnumTagTypeViewModel(TagType tagType, IEnumerable<Tag> tags)
        {
            this.TagType = tagType;
            this.AddTagCommand = new RelayCommand<string>(AddTag);
            this.UpdateTagDataCommand = new RelayCommand<Tag>(UpdateTag);
            this.DeleteTagCommand = new RelayCommand<Tag>(DeleteTag);
            this.EnumTags = new ObservableCollection<Tag>(tags); 
        }

        public void AddTag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                return;

            var tag = new Tag(this.TagType.Name, tagName);
            this.EnumTags.Add(tag);
        }

        public void UpdateTag(Tag tag)
        {
            if (tag == null)
                return;
        }

        public void DeleteTag(Tag tag)
        {
            if (tag == null)
                return;
            this.EnumTags.Remove(tag);
        }

        public string ToString()
        {
            return SelectedEnumTag != null ? SelectedEnumTag.Value : "" ; 
        }

        public int GetTagTypeTypeId()
        {
            return this.TagType.TagTypeTypeId;
        }
    }
}
