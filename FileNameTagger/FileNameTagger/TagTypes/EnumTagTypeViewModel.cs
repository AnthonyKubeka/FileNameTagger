

using Domain;
using Repository;
using Shared;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;

namespace FileNameTagger.TagTypes
{
    public class EnumTagTypeViewModel : BaseViewModel, ITagTypeViewModel
    {
        private Tag selectedEnumTag;
        private ObservableCollection<Tag> enumTags; 

        public IRepositoryBase<Tag> TagRepository { get; set; }

        public TagType TagType { get; private set; }

        public ObservableCollection<Tag> EnumTags
        {
            get { return enumTags; }
            set { SetProperty(ref enumTags, value); }
        }

        public Tag SelectedEnumTag
        {
            get { return selectedEnumTag; }
            set { SetProperty(ref selectedEnumTag, value); }
        }

        public RelayCommand<string> AddTagCommand { get; private set; }
        public RelayCommand<Tag> UpdateTagDataCommand { get; private set; }
        public RelayCommand<Tag> DeleteTagCommand { get; private set; }

        public EnumTagTypeViewModel()
        {

        }

        public EnumTagTypeViewModel(TagType tagType)
        {
            this.TagType = tagType;
            this.AddTagCommand = new RelayCommand<string>(AddTag);
            this.UpdateTagDataCommand = new RelayCommand<Tag>(UpdateTag);
            this.DeleteTagCommand = new RelayCommand<Tag>(DeleteTag);

            InitDatabase();
        }

        public void InitDatabase()
        {
            var connection = new SQLiteAsyncConnection(App.databasePath);
            connection.CreateTableAsync<Tag>();
            this.TagRepository = new RepositoryBase<Tag>(connection);
            var tags = connection.Table<Tag>().Where(tag => tag.TagTypeId == this.TagType.TagTypeTypeId).OrderBy(tag => tag.Value).ToListAsync().Result;
            this.EnumTags = new ObservableCollection<Tag>(tags);
        }

        public void AddTag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                return;

            //if (string.Equals(tagName, )) value in collection of tags

            var tag = new Tag(this.TagType.TagTypeId, tagName);
            this.TagRepository.Create(tag);
            this.EnumTags.Add(tag);
        }

        public void UpdateTag(Tag tag)
        {
            if (tag == null)
                return;

            this.TagRepository.Update(tag);
        }

        public void DeleteTag(Tag tag)
        {
            if (tag == null)
                return;

            this.TagRepository.Delete(tag);
            this.EnumTags.Remove(tag);
        }
    }
}
