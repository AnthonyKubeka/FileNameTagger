using System.Collections.Generic;
using Shared;
using System;
using System.Linq; 
namespace Domain
{
    public class Tag
    {

        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
        public string Title { get; set; }
        public ResolutionsEnum Resolution { get; set; }
        public string ExportedTagName { get; set; }
        public File File;
        public DateTime ReleaseDate { get; set; }

        public Tag(IEnumerable<Actor> actors, IEnumerable<Category> categories, IEnumerable<Studio> studios, string title, ResolutionsEnum resolution, File file, DateTime releaseDate)
        {
            Actors = actors;
            Categories = categories;
            Studios = studios;
            Title = title;
            Resolution = resolution;
            File = file;
            ReleaseDate = releaseDate;
        }

        public Tag(File file)
        {
            File = file; 
        }

        public string ExportTagName()
        {
            var studiosString = string.Join("-", this.Studios.Select(x => x.Name));
            var actorsString = string.Join("-", this.Actors.Select(x => x.Name));
            var categoriesString = string.Join("-", this.Categories.Select(x => x.Name));
            var titleWithDash = "";

            if (!String.IsNullOrEmpty(this.Title))
            {
                titleWithDash = $"{this.Title}-";
            }

            var tag = $"{studiosString}-{titleWithDash}{this.Resolution}-{actorsString}-{categoriesString}-{this.ReleaseDate.ToShortDateString()}";
            return tag; 
            
        }


    }
}
