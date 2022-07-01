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
        public DateTime? ReleaseDate { get; set; }
        public string ReleaseYear { get; set; }

        public Tag(IEnumerable<Actor> actors, IEnumerable<Category> categories, IEnumerable<Studio> studios, string title, ResolutionsEnum resolution, File file, DateTime? releaseDate, string releaseYear)
        {
            Actors = actors;
            Categories = categories;
            Studios = studios;
            Title = title;
            Resolution = resolution;
            File = file;
            ReleaseDate = releaseDate;
            ReleaseYear = releaseYear; 
        }

        public Tag(File file)
        {
            File = file; 
        }

        private string ConvertResolutionEnumToString(ResolutionsEnum resolution)
        {
            var resolutionsConversionDictionary = new Dictionary<ResolutionsEnum, string>()
           {
               {ResolutionsEnum.SD, "SD" },
               {ResolutionsEnum.HD, "720p" },
               {ResolutionsEnum.FHD, "1080p" },
               {ResolutionsEnum.QHD, "1440p" },
               {ResolutionsEnum.UHD, "2160p" },
           };

            return resolutionsConversionDictionary[resolution];
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

            string releaseDate = null; 
            if (this.ReleaseDate != null)
            {
                releaseDate = this.ReleaseDate.Value.ToShortDateString();
            }
            else
            {
                releaseDate = ReleaseYear; 
            }

            var tag = $"{studiosString}-{titleWithDash}{this.ConvertResolutionEnumToString(this.Resolution)}-{actorsString}-{categoriesString}-{releaseDate}";
            return tag; 
            
        }


    }
}
