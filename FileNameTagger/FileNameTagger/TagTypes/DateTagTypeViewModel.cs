

using Domain;
using Repository;
using Shared;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;

namespace FileNameTagger.TagTypes
{
    public class DateTagTypeViewModel : BaseViewModel, ITagTypeViewModel
    {
        private Tag dateTag;

        public IRepositoryBase<Tag> TagRepository { get; set; }

        public TagType TagType { get; private set; }

        public Tag DateTag
        {
            get { return dateTag; }
            set { SetProperty(ref dateTag, value); }
        }

        public DateTagTypeViewModel()
        {
        }

        public DateTagTypeViewModel(TagType tagType)
        {
            TagType = tagType;
            InitDatabase();
        }

        public void InitDatabase()
        {
            var connection = new SQLiteAsyncConnection(App.databasePath);
            connection.CreateTableAsync<Tag>(); //could probably put this in parent viewmodel
            TagRepository = new RepositoryBase<Tag>(connection);
            var tags = connection.Table<Tag>().Where(tag => tag.TagTypeId == TagType.TagTypeTypeId).OrderBy(tag => tag.Value).ToListAsync().Result;
        }
    }
}
