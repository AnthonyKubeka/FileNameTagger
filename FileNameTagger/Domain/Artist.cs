using Shared;
using SQLite;
using System.Diagnostics;

namespace Domain
    //note on database structure: using sqlite-net-pcl
{
    [DebuggerDisplay("{Name}")] //diagnostic to display value by name
    public class Artist
    {
        [PrimaryKey, AutoIncrement]
        public int ArtistId { get; set; }
        public string Name { get; set; }

        public Artist()
        {
        }

        public Artist(string name)
        {
            Name = name;
        }
    }
}
