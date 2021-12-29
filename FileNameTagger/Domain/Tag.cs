using System.Collections.Generic;
using Shared;
namespace Domain
{
    public class Tag
    {

        public string ExportedsTagName { get; set; }

        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
        public string Title { get; set; }
        public ResolutionsEnum Resolution { get; set; }

        public File File;


        public Tag(IEnumerable<Actor> actors, IEnumerable<Category> categories, IEnumerable<Studio> studios, string title, ResolutionsEnum resolution, File file)
        {
            Actors = actors;
            Categories = categories;
            Studios = studios;
            Title = title;
            Resolution = resolution;
            File = file; 
        }

        public Tag(File file)
        {
            File = file; 
        }



    }
}
