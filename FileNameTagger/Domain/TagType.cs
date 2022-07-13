using Shared;
using SQLite;
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
        [PrimaryKey, AutoIncrement]
        public int TagTypeId { get; set; }
        public string Name { get; set; }
        public int TagTypeTypeId { get; set; }

        public TagType()
        {

        }

        public TagType(string name, TagTypeTypeEnum tagTypeType)
        {
            Name = name;
            TagTypeTypeId = (int) tagTypeType; 
        }
    }
}
