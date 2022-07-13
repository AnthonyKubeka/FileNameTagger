

using Domain;
using Repository;
using Shared;
using SQLite;
using System.Collections.ObjectModel;

namespace FileNameTagger
{
    public class TagTypeViewModel : BaseViewModel
    {
        private ObservableCollection<Tag> tags;
        private TagType tagType;
        private Tag selectedTag; 

        public IRepositoryBase<Tag> TagRepository { get; set; }

        public TagType TagType //should be passed in from parent view and doesn't change
        {
            get { return tagType; } 
            set { tagType = value; } 
        }   

        public ObservableCollection<Tag> Tags
        {
            get { return tags; }
            set { SetProperty(ref tags, value); }
        }

        public Tag SelectedTag
        {
            get { return selectedTag; }
            set { SetProperty(ref selectedTag, value); }
        }

        public RelayCommand<string> AddTagCommand { get; private set; }

        public TagTypeViewModel()
        {
            this.AddTagCommand = new RelayCommand<string>(AddTag);

            InitDatabase();
        }

        private void InitDatabase()
        {
            var connection = new SQLiteAsyncConnection(App.databasePath);
            connection.CreateTableAsync<Tag>(); //could probably put this in parent viewmodel
            this.TagRepository = new RepositoryBase<Tag>(connection);
            var tags = connection.Table<Tag>().Where(tag => tag.TagTypeId == this.TagType.TagTypeTypeId).OrderBy(tag => tag.Value).ToListAsync().Result;
            this.Tags = new ObservableCollection<Tag>(tags);
        }

        private void AddTag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                return;

            //if (string.Equals(tagName, )) value in collection of tags

            var tag = new Tag(this.TagType.TagTypeId, tagName);
            this.TagRepository.Create(tag);
        }
    }
}
