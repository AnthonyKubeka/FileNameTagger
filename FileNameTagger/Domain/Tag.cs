using System.Collections.Generic;
using Shared;
using System;
using System.Linq;
using SQLite;

namespace Domain
{
    public class Tag
    {
        [PrimaryKey, AutoIncrement]
        public int TagId { get; set; }
        public int TagTypeId { get; set; }
        public string Value { get; set; }
        [Ignore]
        public bool IsChecked { get; set; }

        public Tag()
        {

        }

        public Tag(int tagTypeId, string value)
        {
            TagTypeId = tagTypeId; 
            Value = value;
            IsChecked = false;
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

    }
}
