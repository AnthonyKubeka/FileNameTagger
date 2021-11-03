using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Tag
    {
        public Tag(IEnumerable<Actor> actors, IEnumerable<Category> categories, Studio studio, string title, ResolutionsEnum resolution)
        {
            Actors = actors;
            Categories = categories;
            Studio = studio;
            Title = title;
            Resolution = resolution;
        }

        public string ExportedTagName { get; set; }

        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public Studio Studio { get; set; }
        public string Title { get; set; }
        public ResolutionsEnum Resolution { get; set; }


    }
}
