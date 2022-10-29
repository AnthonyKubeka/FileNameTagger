using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TagType
    {
        public string Name { get; set; }
        public int TagTypeTypeId { get; set; }
        public IEnumerable<Tag> Tags { get; set; }

        public TagType(string name, TagTypeTypeEnum tagTypeType, IEnumerable<Tag> tags)
        {
            Name = name;
            TagTypeTypeId = (int) tagTypeType;
            Tags = tags;
        }
    }
}
