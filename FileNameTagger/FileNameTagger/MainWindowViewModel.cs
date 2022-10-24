using Domain;
using Shared;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using SQLite.Net;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Input;
using SQLite;
using Repository;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using System.IO;

namespace FileNameTagger
{

    public class MainWindowViewModel : BaseViewModel
    {
        #region Properties
        private Domain.File loadedFile;
        private string loadedFileName;
        private string exportedTag;
        private ObservableCollection<ITagTypeViewModel> tagTypeViewModels;
        #endregion

        #region Commands
        public RelayCommand AddFileCommand { get; private set; }//private set as we only want this to be settable once, on construction
        public RelayCommand SaveTagCommand { get; private set; }
        public RelayCommand ClearTagCommand { get; private set; }
        public RelayCommand ImportTagTemplateCommand { get; private set; }
        #endregion

        #region Property Definitions
        public string ResolutionFromFile { get; set; }
        public TagTypeViewModelFactory TagTypeViewModelFactory { get; set; }
        public TagTemplate TagTemplate { get; set; }

        public ObservableCollection<ITagTypeViewModel> TagTypeViewModels
        {
            get
            {
                return tagTypeViewModels;
            }

            set
            {
                SetProperty(ref tagTypeViewModels, value);
            }
        }


        public string ExportedTag
        {
            get
            {
                return exportedTag;
            }

            set { SetProperty(ref exportedTag, value); }
        }

        public string LoadedFileName
        {
            get
            {
                if (loadedFile != null)
                {
                    return loadedFile.Name;
                }
                else
                {
                    return "No File Selected";
                }
            }

            set { SetProperty(ref loadedFileName, value); }
        }

        public Domain.File LoadedFile
        {
            get
            {
                return loadedFile;
            }

            set
            {
                if (loadedFile != value)
                {
                    loadedFile = value;
                    SetProperty(ref loadedFile, value);
                }
            }
        }

        public List<TagType> TagTypesInDb { get; set; }
        public List<Tag> TagsInDb { get; set; }
        public List<TagType> TagTypesInMemory { get; set; }
        public List<Tag> TagsInMemory { get; set; }
        #endregion

        public MainWindowViewModel()
        {

            #region Command Instantiations
            AddFileCommand = new RelayCommand(OnAddFile);
            ImportTagTemplateCommand = new RelayCommand(OnImportTagTemplate);
            SaveTagCommand = new RelayCommand(OnSaveTag);
            ClearTagCommand = new RelayCommand(OnNewTag);
            #endregion

            #region Componenent and data initialisations
            TagTypeViewModels = new ObservableCollection<ITagTypeViewModel>();
            TagTypeViewModelFactory = new TagTypeViewModelFactory();
            LoadedFile = new Domain.File("No File Selected");
            exportedTag = "No Tag Created For File";
            #endregion
            TagTypeViewModelFactory = new TagTypeViewModelFactory();
            TagTypesInMemory = new List<TagType>();
            TagsInMemory = new List<Tag>();
        }

        void OnSaveTag()
        {
            var tagTypeViewModels = this.TagTypeViewModels.ToList();
            var exportedTag = "";
            var orderedTagTypeViewModels = this.GetOrderedTagTypeViewModels(tagTypeViewModels);

            foreach (var tagTypeViewModel in orderedTagTypeViewModels)
            {
                exportedTag += $"{tagTypeViewModel.ToString()}-";
            }
            var exportedTagString = exportedTag.Remove(exportedTag.Length - 1, 1);
            ExportedTag = exportedTagString;
        }

        public IEnumerable<ITagTypeViewModel> GetOrderedTagTypeViewModels(IEnumerable<ITagTypeViewModel> tagTypeViewModels)
        {
            var orderedTagTypeViewModels = new List<ITagTypeViewModel>();
            foreach (var tagTypeViewModel in tagTypeViewModels)
            {
                if (tagTypeViewModel.GetTagTypeTypeId() == (int)TagTypeTypeEnum.Text)
                {
                    orderedTagTypeViewModels.Add(tagTypeViewModel);
                }
            }

            foreach (var tagTypeViewModel in tagTypeViewModels)
            {
                if (tagTypeViewModel.GetTagTypeTypeId() == (int)TagTypeTypeEnum.TextList)
                {
                    orderedTagTypeViewModels.Add(tagTypeViewModel);
                }
            }

            foreach (var tagTypeViewModel in tagTypeViewModels)
            {
                if (tagTypeViewModel.GetTagTypeTypeId() == (int)TagTypeTypeEnum.Enum)
                {
                    orderedTagTypeViewModels.Add(tagTypeViewModel);
                }
            }

            foreach (var tagTypeViewModel in tagTypeViewModels)
            {
                if (tagTypeViewModel.GetTagTypeTypeId() == (int)TagTypeTypeEnum.Date)
                {
                    orderedTagTypeViewModels.Add(tagTypeViewModel);
                }
            }

            return orderedTagTypeViewModels;
        }

        private void InitDatabase()
        {
            App.connection.CreateTableAsync<TagType>();
            App.connection.CreateTableAsync<Tag>();
            TagTypesInDb = App.connection.Table<TagType>().OrderBy(tagTypes => tagTypes.TagTypeTypeId).ToListAsync().Result;
            TagsInDb = App.connection.Table<Tag>().ToListAsync().Result;
        }

        private void SetTagAndTagTypeFromTagTemplateFromDisk(TagTemplate tagTemplate)
        {
            InitDatabase();
            var tagTypes = new List<TagType>();
            var tagsForTagType = new List<Tag>();
            foreach (var tagTemplateTagType in tagTemplate.TagTemplateTagTypes)
            {
                var tagType = new TagType(tagTemplateTagType.Name, tagTemplateTagType.TagTypeType);
                var isTagTypeInDb = TagTypesInDb.Where(x => x.Name == tagType.Name && x.TagTypeTypeId == tagType.TagTypeTypeId).Any();
                if (!isTagTypeInDb)
                {
                    App.TagTypesRepository.Create(tagType);
                    InitDatabase();
                }

                tagTypes.Add(tagType);

                if (tagTemplateTagType.Values != null) //enforce business rule that a tag type must have a value to be 'loaded'
                {
                    foreach(var value in tagTemplateTagType.Values)
                    {
                        var matchingTagType = TagTypesInDb.Where(x => x.Name == tagType.Name && x.TagTypeTypeId == tagType.TagTypeTypeId).First();
                        var tag = new Tag(matchingTagType.TagTypeId, value);
                        var isTagInDb = TagsInDb.Where(x => x.Value == tag.Value && x.TagTypeId == tag.TagTypeId).Any();
                        if (!isTagInDb)
                        {
                            App.TagRepository.Create(tag);
                            InitDatabase();
                        }
                    }
                }
            }
            foreach (var tagType in tagTypes)
            {
                var matchingTagTypeInDb = TagTypesInDb.Where(x => x.Name == tagType.Name && x.TagTypeTypeId == tagType.TagTypeTypeId).FirstOrDefault(); 
                if (matchingTagTypeInDb != null)
                {
                    TagTypesInMemory.Add(matchingTagTypeInDb);
                }
            } 
        }

        private void LoadTagTypeViewModels()
        {
            var unorderedTagTypeViewModels = new List<ITagTypeViewModel>();
            foreach (var tagType in TagTypesInMemory)
            {
                unorderedTagTypeViewModels.Add(TagTypeViewModelFactory.GetTagTypeViewModel(tagType, TagsInDb));
            }
            TagsInMemory = TagsInDb;
            var orderedTagTypeViewModels = GetOrderedTagTypeViewModels(unorderedTagTypeViewModels);
            
            foreach (var tagTypeViewModel in orderedTagTypeViewModels)
            {
                TagTypeViewModels.Add(tagTypeViewModel);
            }    
        }

        public void SetResolutionFromFileInfo(string filename)
        {
            var ffProbe = new NReco.VideoInfo.FFProbe();
            var videoInfo = ffProbe.GetMediaInfo(filename);

            var resolution = videoInfo.Streams[0].Height;

            switch (resolution)
            {
                case 2160:
                    ResolutionFromFile = "2160P";
                    break;
                case 1080:
                    ResolutionFromFile = "1080P";
                    break;
                case 1440:
                    ResolutionFromFile = "1440P";
                    break;
                case 720:
                    ResolutionFromFile = "720P";
                    break;
                default:
                    ResolutionFromFile = "SD";
                    break;
            }

        }

        public string SelectFileFromFileExplorer(FileTypeEnum fileType)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "";
            switch (fileType)
            {
                case FileTypeEnum.Video:
                    dialog.DefaultExt = ".mp4";
                    dialog.Filter = "Video files (*.mp4)|*.mkv|All files (*.*)|*.*";
                    break;
                case FileTypeEnum.JSON:
                    dialog.DefaultExt = ".json";
                    dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                    break;
            }

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;

                return filename;
            }
            else
            {
                return String.Empty;
            }

        }

        private void OnAddFile()
        {
            OnNewTag();
            var filename = SelectFileFromFileExplorer(FileTypeEnum.Video);
            var fileToAdd = new Domain.File(filename);
            SetResolutionFromFileInfo(filename);
            LoadedFile = fileToAdd;
        }

        private void OnImportTagTemplate()
        {

            var filename = SelectFileFromFileExplorer(FileTypeEnum.JSON);
            if (!string.IsNullOrEmpty(filename))
            {
                using (StreamReader streamReader = new StreamReader(filename))
                {
                    string tagTemplateJson = streamReader.ReadToEnd();
                    TagTemplate = JsonConvert.DeserializeObject<TagTemplate>(tagTemplateJson);
                    SetTagAndTagTypeFromTagTemplateFromDisk(TagTemplate);
                }

                LoadTagTypeViewModels();
            }

        }

        private void OnNewTag()
        {
        }
    }
}
