

using Domain;
using Repository;
using Shared;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;

namespace FileNameTagger.TagTypes
{
    public class TextTagTypeViewModel : BaseViewModel, ITagTypeViewModel
    {
        private Tag tag;

        public TagType TagType

        { get; private set; }

        public Tag Tag
        {
            get { return tag; }
            set { SetProperty(ref tag, value); }
        }

        public TextTagTypeViewModel()
        {
        }

        public TextTagTypeViewModel(TagType tagType)
        {
            this.tag = new Tag(tagType.TagTypeId, "");
            this.TagType = tagType;
        }
    }
}
