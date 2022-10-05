using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TagTemplate
    {
        public string TemplateName { get; set; }
        public List<TagTemplateTagType> TagTemplateTagTypes { get; set; }


    }
}
