

using Domain;
using Repository;
using Shared;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;

namespace FileNameTagger.TagTypes
{
    public class TextListTagTypeViewModel : BaseViewModel, ITagTypeViewModel
    {
        private ObservableCollection<Tag> tags;
        private Tag selectedTag;

        public IRepositoryBase<Tag> TagRepository { get; set; }

        public TagType TagType //should be passed in from parent view and doesn't change
                               //the TagTypeTypeId determines what TagTypeView to bind to this viewmodel
        { get; private set; }

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
        public RelayCommand<Tag> UpdateTagDataCommand { get; private set; }
        public RelayCommand<Tag> DeleteTagCommand { get; private set; }

        public TextListTagTypeViewModel()
        {

        }

        public TextListTagTypeViewModel(TagType tagType)
        {
            TagType = tagType;
            AddTagCommand = new RelayCommand<string>(AddTag);
            UpdateTagDataCommand = new RelayCommand<Tag>(UpdateTag);
            DeleteTagCommand = new RelayCommand<Tag>(DeleteTag);

            InitDatabase();
        }

        public void InitDatabase()
        {
            var connection = new SQLiteAsyncConnection(App.databasePath);
            connection.CreateTableAsync<Tag>(); //could probably put this in parent viewmodel
            TagRepository = new RepositoryBase<Tag>(connection);
            var tags = connection.Table<Tag>().Where(tag => tag.TagTypeId == TagType.TagTypeTypeId).OrderBy(tag => tag.Value).ToListAsync().Result;
            Tags = new ObservableCollection<Tag>(tags);
        }

        public void AddTag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                return;

            //if (string.Equals(tagName, )) value in collection of tags

            var tag = new Tag(TagType.TagTypeId, tagName);
            TagRepository.Create(tag);
            Tags.Add(tag);
        }

        public void UpdateTag(Tag tag)
        {
            if (tag == null)
                return;

            TagRepository.Update(tag);
        }

        public void DeleteTag(Tag tag)
        {
            if (tag == null)
                return;

            TagRepository.Delete(tag);
            Tags.Remove(tag);
        }
    }
}
