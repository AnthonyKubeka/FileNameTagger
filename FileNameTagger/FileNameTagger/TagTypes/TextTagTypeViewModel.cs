

using Domain;
using Repository;
using Shared;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;

namespace FileNameTagger.TagTypes
{
    public class TextTagTypeViewModel : BaseViewModel, ITagTypeViewModel
    {
        private Tag tag;

        public IRepositoryBase<Tag> TagRepository { get; set; }

        public TagType TagType

        { get; private set; }

        public Tag Tag
        {
            get { return tag; }
            set { SetProperty(ref tag, value); }
        }

        public TextTagTypeViewModel()
        {
        }

        public TextTagTypeViewModel(TagType tagType)
        {
            TagType = tagType;
            InitDatabase();
        }

        public void InitDatabase()
        {
            var connection = new SQLiteAsyncConnection(App.databasePath);
            connection.CreateTableAsync<Tag>();
            TagRepository = new RepositoryBase<Tag>(connection);
        }
    }
}
