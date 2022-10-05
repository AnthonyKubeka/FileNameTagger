using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TagTemplateTagType
    {
        public string Name; 
        public TagTypeTypeEnum TagTypeType;
        public IEnumerable<string> Values; 
    }
}
