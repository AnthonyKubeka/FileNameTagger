

using Domain;
using Repository;
using Shared;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;

namespace FileNameTagger
{
    public interface ITagTypeViewModel
    {
        string ToString();
        int GetTagTypeTypeId();
    }
}
