using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domain
{
    public class Actor
    {
        public string Name { get; set; }
        public GendersEnum Gender { get; set; }
        public bool IsChecked { get; set; }

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
