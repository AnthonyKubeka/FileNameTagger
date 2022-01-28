using Domain;
using Shared;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq; 

namespace FileNameTagger
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private File loadedFile;
        private string loadedFileName;
        private string title;
        private string exportedTag; 
        private ObservableCollection<Studio> studios;
        private ObservableCollection<Actor> actors;
        private ResolutionsEnum selectedResolution; 
        private ObservableCollection<Category> categories;
        private Tag tag;
        private DateTime releaseDate; 
        public event PropertyChangedEventHandler PropertyChanged = delegate { }; //property change is never null since we assign it an empty anonymous subscriber

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
                    PropertyChanged(this, new PropertyChangedEventArgs("ExportedTag"));
                }
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

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title != value)
                {
                    title = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
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
        public ObservableCollection<Studio> Studios
        {
            get
            {
                return studios;
            }

            set
            {
                if (studios != value)
                {
                    studios = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Studios"));
                }
            }
        }

        public ObservableCollection<Actor> Actors
        {
            get
            {
                return actors;
            }

            set
            {
                if (actors != value)
                {
                    actors = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Actors"));
                }
            }
        }

        public ObservableCollection<Category> Categories
        {
            get
            {
                return categories;
            }

            set
            {
                if (categories != value)
                {
                    categories = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Categories"));
                }
            }
        }

        public ResolutionsEnum SelectedResolution
        {
            get
            {
                return selectedResolution;
            }

            set
            {
                if (selectedResolution != value)
                {
                    selectedResolution = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedResolution"));
                }
            }
        }

        public MainWindowViewModel()
        {
            this.AddFileCommand = new RelayCommand(OnAddFile);
            this.SaveTagCommand = new RelayCommand(OnSaveTag);
            ReadDatabase();
            ReleaseDate = DateTime.Now; 
            this.LoadedFile = new File("No File Selected");
            this.Tag = new Tag(this.LoadedFile);
            this.exportedTag = "No Tag Created For File";
        }

        public RelayCommand<File> DeleteFileCommand { get; private set; } 

        public RelayCommand AddFileCommand { get; private set; }//private set as we only want this to be settable once, on construction
        public RelayCommand SaveTagCommand { get; private set; }

        private void ReadDatabase()
        {
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Actor>();
                connection.CreateTable<Studio>();
                connection.CreateTable<Category>();

                var actors = connection.Table<Actor>().ToList().OrderBy(actors => actors.Name).ToList();
                var categories = connection.Table<Category>().ToList().OrderBy(categories => categories.Name).ToList();
                var studios = connection.Table<Studio>().ToList().OrderBy(studios => studios.Name).ToList();

                this.Actors = new ObservableCollection<Actor>(actors);
                this.Categories = new ObservableCollection<Category>(categories);
                this.Studios = new ObservableCollection<Studio>(studios);

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
                    this.selectedResolution = ResolutionsEnum.UHD; 
                    break;
                case 1080:
                    this.selectedResolution = ResolutionsEnum.FHD;
                    break;
                case 1440:
                    this.selectedResolution = ResolutionsEnum.QHD;
                    break;
                case 720:
                    this.selectedResolution = ResolutionsEnum.HD;
                    break;
                default:
                    this.selectedResolution = ResolutionsEnum.SD;
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

                this.loadedFileName = "Hello Mojo"; 

                return filename;
            }
            else
            {
                return "File not found";
            }

        }

        void OnAddFile()
        {
            var filename = SelectFileFromFileExplorer();
            var fileToAdd = new File (filename);
            SetResolutionFromFileInfo(filename);
            this.LoadedFile = fileToAdd; 
        }

        void OnSaveTag()
        {
            var tagToSave = new Tag(this.Actors.Where(x => x.IsChecked), this.Categories.Where(x => x.IsChecked), this.Studios.Where(x => x.IsChecked), this.Title, this.SelectedResolution, this.LoadedFile, this.ReleaseDate);
            this.Tag = tagToSave;
            this.ExportedTag = this.Tag.ExportTagName();
        }
    }
}
