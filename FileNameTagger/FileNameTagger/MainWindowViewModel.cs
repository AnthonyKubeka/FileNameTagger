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
using System.Windows ;
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
        public string ResolutionFromFile { get; set; }
        public RelayCommand AddFileCommand { get; private set; }//private set as we only want this to be settable once, on construction
        public RelayCommand SaveTagCommand { get; private set; }
        public RelayCommand ClearTagCommand { get; private set; }
        public RelayCommand ImportTagTemplateCommand { get; private set; }
        public IRepositoryBase<TagType> TagTypesRepository { get; set; }
        public TagTypeViewModelFactory TagTypeViewModelFactory {get; set;}
    #endregion

    #region Property Definitions

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

            set
            {
                if (exportedTag != value)
                {
                    exportedTag = value;
                    SetProperty(ref exportedTag, value);
                }
            }
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

            set
            {
                if (loadedFileName != value)
                {
                    loadedFileName = value;
                    SetProperty(ref loadedFileName, value);
                }
            }
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

        public Collection<TagType> TagTypes { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            
            #region Command Instantiations
            AddFileCommand = new RelayCommand(OnAddFile);
            ImportTagTemplateCommand = new RelayCommand(OnImportTagTemplate);
           // SaveTagCommand = new RelayCommand(OnSaveTag);
            ClearTagCommand = new RelayCommand(OnNewTag);
            #endregion

            #region Componenent and data initialisations
            TagTypeViewModels = new ObservableCollection<ITagTypeViewModel>();
            TagTypeViewModelFactory = new TagTypeViewModelFactory();
            InitDatabase();
            LoadedFile = new Domain.File("No File Selected");
            exportedTag = "No Tag Created For File";
            #endregion
            TagTypeViewModelFactory = new TagTypeViewModelFactory();
        }

        private void InitDatabase()
        {
            var connection = new SQLiteAsyncConnection(App.databasePath);
            connection.CreateTableAsync<TagType>();
            connection.CreateTableAsync<Tag>();
            TagTypesRepository = new RepositoryBase<TagType>(connection);
            var tagTypes = connection.Table<TagType>().OrderBy(tagTypes => tagTypes.Name).ToListAsync().Result; 
            TagTypes = new Collection<TagType>(tagTypes);
            foreach (var tagType in TagTypes)
            {
                TagTypeViewModels.Add(TagTypeViewModelFactory.GetTagTypeViewModel(tagType)); 
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

        public string SelectFileFromFileExplorer()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = ""; 
            dialog.DefaultExt = ".mp4";
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;

                loadedFileName = "Hello Mojo"; 

                return filename;
            }
            else
            {
                return "File not found";
            }

        }

        private void OnAddFile()
        {
            OnNewTag();
            var filename = SelectFileFromFileExplorer();
            var fileToAdd = new Domain.File(filename);
            SetResolutionFromFileInfo(filename);
            LoadedFile = fileToAdd; 
        }

        private void OnImportTagTemplate()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            var filename = "";
            dialog.FileName = "";
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                filename = dialog.FileName; 
            }
            if (!string.IsNullOrEmpty(filename))
            {
                using (StreamReader streamReader = new StreamReader(filename))
                {
                    string tagTemplateJson = streamReader.ReadToEnd();
                    var TagTemplate = JsonConvert.DeserializeObject<TagTemplate>(tagTemplateJson);
                    Console.WriteLine("hi");
                }
            }
            
        }

        private void OnNewTag()
        {
        }

/*        void OnSaveTag()
        {
            Tag tagToSave = null; 
            if (releaseDateYearOnly)
            {
               tagToSave = new Tag(Actors.Where(x => x.IsChecked), Categories.Where(x => x.IsChecked), Studios.Where(x => x.IsChecked), Title, SelectedResolution, LoadedFile, null, releaseYear);
            }
            else
            {
               tagToSave = new Tag(Actors.Where(x => x.IsChecked), Categories.Where(x => x.IsChecked), Studios.Where(x => x.IsChecked), Title, SelectedResolution, LoadedFile, ReleaseDate, string.Empty);
            }
            Tag = tagToSave;
            ExportedTag = Tag.ExportTagName();
        }*/
    }
}
