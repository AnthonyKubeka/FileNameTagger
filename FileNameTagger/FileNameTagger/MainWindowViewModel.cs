using Domain;
using Shared;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using SQLite.Net;

using System.Windows.Controls;
using System.Windows.Input;
using SQLite;
using Repository;
using System.Collections.Generic;
using System.Windows ;
using System.Diagnostics;

namespace FileNameTagger
{

    public class MainWindowViewModel : BaseViewModel
    {
        #region Properties
        private File loadedFile;
        private string loadedFileName;
        private string exportedTag;
        private bool releaseDateYearOnly;
        private Tag tag;
        private DateTime releaseDate;
        private string releaseYear;
        private ObservableCollection<ITagTypeViewModel> tagTypeViewModels;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged = delegate { }; //property change is never null since we assign it an empty anonymous subscriber

        #region Commands
        public RelayCommand<File> DeleteFileCommand { get; private set; }

        public RelayCommand AddFileCommand { get; private set; }//private set as we only want this to be settable once, on construction
        public RelayCommand SaveTagCommand { get; private set; }
        public RelayCommand ClearTagCommand { get; private set; }
        public IRepositoryBase<TagType> TagTypesRepository { get; set; }
        //public IRepositoryBase<Tag> TagsRepository { get; set; }
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

        public string ReleaseYear
        {
            get
            {
                return releaseYear;
            }

            set
            {
                if (releaseYear != value)
                {
                    releaseYear = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ReleaseYear"));
                }
            }
        }

        public DateTime ReleaseDate
        {
            get
            {
                return releaseDate;
            }

            set
            {
                if (releaseDate != value)
                {
                    releaseDate = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ReleaseDate"));
                }
            }
        }

        public bool ReleaseDateYearOnly
        {
            get
            {
                return releaseDateYearOnly;
            }

            set
            {
                if (releaseDateYearOnly != value)
                {
                    releaseDateYearOnly = value;
                    if (releaseDateYearOnly)
                    {
                        DatePickerVisibility = Visibility.Hidden;
                        YearComboBoxVisibility = Visibility.Visible;
                    }
                    else
                    {
                        DatePickerVisibility = Visibility.Visible;
                        YearComboBoxVisibility = Visibility.Hidden;
                    }
                    PropertyChanged(this, new PropertyChangedEventArgs("ReleaseDateYearOnly"));
                    PropertyChanged(this, new PropertyChangedEventArgs("DatePickerVisibility"));
                    PropertyChanged(this, new PropertyChangedEventArgs("YearComboBoxVisibility"));
                }
            }
        }

        public Visibility DatePickerVisibility { get; set; }
        public Visibility YearComboBoxVisibility { get; set; }


        public bool ReleaseDateNotYearOnly
        {
            get
            {
                return !releaseDateYearOnly;
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
                    PropertyChanged(this, new PropertyChangedEventArgs("ExportedTag"));
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
                    PropertyChanged(this, new PropertyChangedEventArgs("LoadedFileName"));
                }
            }
        }
        public File LoadedFile
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
                    PropertyChanged(this, new PropertyChangedEventArgs("LoadedFile"));
                }
            }
        }

        public Tag Tag
        {
            get
            {
                return tag;
            }

            set
            {
                if (tag != value)
                {
                    tag = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Tag"));
                }
            }
        }

        public Collection<TagType> TagTypes { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            
            #region Command Instantiations
            AddFileCommand = new RelayCommand(OnAddFile);
           // SaveTagCommand = new RelayCommand(OnSaveTag);
            ClearTagCommand = new RelayCommand(OnNewTag);
            #endregion

            #region Componenent and data initialisations
            TagTypeViewModels = new ObservableCollection<ITagTypeViewModel>();
            TagTypeViewModelFactory = new TagTypeViewModelFactory();
            InitDatabase();
            ReleaseDate = DateTime.Now; 
            LoadedFile = new File("No File Selected");
            exportedTag = "No Tag Created For File";
            ReleaseDateYearOnly = false;
            DatePickerVisibility = Visibility.Visible;
            YearComboBoxVisibility = Visibility.Hidden;
            #endregion
            TagTypeViewModelFactory = new TagTypeViewModelFactory();
        }

        private void InitDatabase()
        {
            var connection = new SQLiteAsyncConnection(App.databasePath);
            connection.CreateTableAsync<TagType>();
            connection.CreateTableAsync<Tag>();
            TagTypesRepository = new RepositoryBase<TagType>(connection);
            //TagsRepository = new RepositoryBase<Tag>(connection);
            var tagTypes = connection.Table<TagType>().OrderBy(tagTypes => tagTypes.Name).ToListAsync().Result; 
            TagTypes = new Collection<TagType>(tagTypes);
            foreach (var tagType in TagTypes)
            {
                TagTypeViewModels.Add(TagTypeViewModelFactory.GetTagTypeViewModel(tagType)); 
            }
        }

/*        public void SetResolutionFromFileInfo(string filename)
        {
            var ffProbe = new NReco.VideoInfo.FFProbe();
            var videoInfo = ffProbe.GetMediaInfo(filename);

            var resolution = videoInfo.Streams[0].Height;
            switch (resolution)
            {
                case 2160:
                    SelectedResolution = ResolutionsEnum.UHD; 
                    break;
                case 1080:
                    SelectedResolution = ResolutionsEnum.FHD;
                    break;
                case 1440:
                    SelectedResolution = ResolutionsEnum.QHD;
                    break;
                case 720:
                    SelectedResolution = ResolutionsEnum.HD;
                    break;
                default:
                    SelectedResolution = ResolutionsEnum.SD;
                    break;
            }
                
        }*/

        public string SelectFileFromFileExplorer()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = ""; 
            dialog.DefaultExt = ".mp4";
            var data = new List<Studio>();
            //data.AddRange(Studios); 
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
            var fileToAdd = new File (filename);
           // SetResolutionFromFileInfo(filename);
            LoadedFile = fileToAdd; 
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
