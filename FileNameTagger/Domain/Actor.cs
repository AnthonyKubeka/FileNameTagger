using Shared;
using SQLite; 
namespace Domain
    //note on database structure: using sqlite-net-pcl
{
    public class Actor
    {
        [PrimaryKey, AutoIncrement]
        public int ActorId { get; set; }
        public string Name { get; set; }
        [Ignore]
        public GendersEnum Gender { get; set; }
        [Ignore]
        public bool IsChecked { get; set; }

        public Actor()
        {

        }

        public Actor(string name)
        {
            Name = name;
            IsChecked = false;
        }

        public Actor(string name, GendersEnum gender)
        {
            Name = name;
            Gender = gender;
        }
    }
}
