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
using Repository;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Text;

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
        public ICommand DropFileCommand { get; private set; }
        public RelayCommand SaveTagCommand { get; private set; }
        public RelayCommand ClearTagCommand { get; private set; }
        public RelayCommand ImportTagTemplateCommand { get; private set; }
        public RelayCommand ExportTagTemplateCommand { get; private set; }
        #endregion

        #region Property Definitions
        public string ResolutionFromFile { get; set; }
        public TagTypeViewModelFactory TagTypeViewModelFactory { get; set; }
        public TagTemplate TagTemplate { get; set; }
        public List<TagType> TagTypes { get; set; }

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
        #endregion

        public MainWindowViewModel()
        {

            #region Command Instantiations
            AddFileCommand = new RelayCommand(OnAddFileFromFileExplorer);
            DropFileCommand = new RelayCommand<string>(OnDropFile);
            ImportTagTemplateCommand = new RelayCommand(OnImportTagTemplate);
            ExportTagTemplateCommand = new RelayCommand(OnExportTagTemplate);
            SaveTagCommand = new RelayCommand(OnSaveTag);
            ClearTagCommand = new RelayCommand(OnClearTag);
            #endregion

            #region Componenent and data initialisations
            TagTypeViewModels = new ObservableCollection<ITagTypeViewModel>();
            TagTypeViewModelFactory = new TagTypeViewModelFactory();
            LoadedFile = new Domain.File("No File Selected");
            exportedTag = "No Tag Created For File";
            #endregion
            TagTypeViewModelFactory = new TagTypeViewModelFactory();
            TagTypes = new List<TagType>();
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

        private void SetTagAndTagTypeFromTagTemplateFromDisk(TagTemplate tagTemplate)
        {
            //treat each load of a tag template as fresh
            if (tagTemplate == null || tagTemplate.TagTemplateTagTypes == null || tagTemplate.TagTemplateTagTypes.Count == 0)
            {
                return;
            }
            TagTypes.Clear();

            foreach (var tagTemplateTagType in tagTemplate.TagTemplateTagTypes)
            {
               
                if (tagTemplateTagType.Values != null && tagTemplateTagType.Values.Any()) //enforce business rule that a tag type must have a value to be 'loaded'
                {
                    var tags = new List<Tag>();
                    foreach(var value in tagTemplateTagType.Values)
                    {
                        var tag = new Tag(tagTemplateTagType.Name, value);
                        tags.Add(tag);
                    }
                    var tagType = new TagType(tagTemplateTagType.Name, tagTemplateTagType.TagTypeType, tags);
                    TagTypes.Add(tagType);
                }
            }
        }

        private void LoadTagTypeViewModels()
        {
            var unorderedTagTypeViewModels = new List<ITagTypeViewModel>();
            foreach (var tagType in TagTypes)
            {
                unorderedTagTypeViewModels.Add(TagTypeViewModelFactory.GetTagTypeViewModel(tagType));
            }
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
            var dialog = new Microsoft.Win32.OpenFileDialog() { DereferenceLinks = false, FileName = "" };
            switch (fileType)
            {
                case FileTypeEnum.Video:
                    dialog.DefaultExt = ".mp4";
                    dialog.Filter = "Video files(*.mp4)|*.mkv|All files (*.*)|*.*";
                    break;
                case FileTypeEnum.JSON:
                    dialog.DefaultExt = ".json";
                    dialog.Filter = "JSON files(*.json)|*.json|All files (*.*)|*.*";
                    break;
            }

            bool? result = dialog.ShowDialog();
            var videoFileExtensions = new List<string>() { ".mp4", ".mkv", ".avi", ".wmv", ".flv", ".mov", ".mpg", ".mpeg", ".m4v", ".webm" }; 
            if (result == true)
            {
                string filename = dialog.FileName;
                FileInfo file = new FileInfo(filename);
                var fileExtension = file.Extension; 

                if (!videoFileExtensions.Contains(fileExtension) && fileType == FileTypeEnum.Video)
                {
                    MessageBox.Show("Invalid file format, FileNameTagger only works with video files.");
                    return String.Empty; 
                }

                return filename;
            }
            else
            {
                return String.Empty;
            }

        }

        public void SaveFileToFileExplorer(string tagTemplate)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Json files(*.json)| *.json | Text files(*.txt) | *.txt";
            saveFileDialog1.Title = "Export Tag Template";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();

                byte[] tagTemplateToWrite = new UTF8Encoding(true).GetBytes(tagTemplate);
                fs.Write(tagTemplateToWrite, 0, tagTemplateToWrite.Length);
                fs.Close();
            }
        }

        private void OnDropFile(string filename)
        {
            OnAddFile(filename);
        }

        private void OnAddFileFromFileExplorer()
        {
            var filename = SelectFileFromFileExplorer(FileTypeEnum.Video);
            OnAddFile(filename);
        }

        private void OnAddFile(string filename)
        {
            OnClearTag();
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

        private void OnExportTagTemplate()
        {
            var tagTemplateToExport = new TagTemplate();
            var tagTemplateTagTypes = new List<TagTemplateTagType>();
            foreach(var tagType in TagTypes)
            {
                var tagTemplateTagType = new TagTemplateTagType();
                tagTemplateTagType.Name = tagType.Name;
                tagTemplateTagType.TagTypeType = (TagTypeTypeEnum)tagType.TagTypeTypeId;
                tagTemplateTagType.Values = tagType.Tags.Select(x => x.Value);
                tagTemplateTagTypes.Add(tagTemplateTagType);
            }
            tagTemplateToExport.TagTemplateTagTypes = tagTemplateTagTypes;
            tagTemplateToExport.TemplateName = "New";
            var tagTemplate = JsonConvert.SerializeObject(tagTemplateToExport, Formatting.Indented);

            SaveFileToFileExplorer(tagTemplate);        
        }

        private void OnClearTag()
        {
        }
    }
}
