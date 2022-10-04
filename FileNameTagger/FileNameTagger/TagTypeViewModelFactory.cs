using Domain;
using FileNameTagger.TagTypes;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNameTagger
{
    public class TagTypeViewModelFactory
    {
        public ITagTypeViewModel GetTagTypeViewModel(TagType tagType, IEnumerable<Tag> tags)
        {
            switch ((TagTypeTypeEnum)tagType.TagTypeTypeId)
            {
                case TagTypeTypeEnum.TextList:
                    return new TextListTagTypeViewModel(tagType, tags.Where(x => x.TagTypeId == tagType.TagTypeId));
                case TagTypeTypeEnum.Text:
                    return new TextTagTypeViewModel(tagType);
                case TagTypeTypeEnum.Date:
                    return new DateTagTypeViewModel(tagType);
                case TagTypeTypeEnum.Enum:
                    return new EnumTagTypeViewModel(tagType, tags.Where(x => x.TagTypeId == tagType.TagTypeId));
                default:throw new Exception("No view model corresponds to this tag type type");
            }
        }
    }
}
