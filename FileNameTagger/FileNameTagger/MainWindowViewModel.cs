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

namespace FileNameTagger
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties
        private File loadedFile;
        private string loadedFileName;
        private string title;
        private string exportedTag;
        private Studio selectedStudio;
        private Actor selectedActor;
        private Category selectedCategory;
        private ObservableCollection<Studio> studios;
        private ObservableCollection<Actor> actors;
        private ResolutionsEnum selectedResolution; 
        private ObservableCollection<Category> categories;
        private Tag tag;
        private DateTime releaseDate;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged = delegate { }; //property change is never null since we assign it an empty anonymous subscriber

        #region Commands
        public RelayCommand<File> DeleteFileCommand { get; private set; }

        public RelayCommand AddFileCommand { get; private set; }//private set as we only want this to be settable once, on construction
        public RelayCommand SaveTagCommand { get; private set; }
        public RelayCommand<object> AddStaticDataCommand { get; private set; }
        public RelayCommand<object> UpdateStaticDataCommand { get; private set; }
        public RelayCommand<object> DeleteStaticDataCommand { get; private set; }
        public IRepositoryBase<Studio> studioRepository { get; set;  }
        public IRepositoryBase<Actor> actorRepository { get; set;  }
        public IRepositoryBase<Category> categoryRepository { get; set;  }
        #endregion

        #region Property Definitions
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

        public Studio SelectedStudio
        {
            get
            {
                return selectedStudio;
            }

            set
            {
                if (selectedStudio != value)
                {
                    selectedStudio = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedStudio"));
                }
            }
        }

        public Actor SelectedActor
        {
            get
            {
                return selectedActor;
            }

            set
            {
                if (selectedActor != value)
                {
                    selectedActor = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedActor"));
                }
            }
        }

        public Category SelectedCategory
        {
            get
            {
                return selectedCategory;
            }

            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCategory"));
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
        #endregion

        public MainWindowViewModel()
        {

            this.AddFileCommand = new RelayCommand(OnAddFile);
            this.SaveTagCommand = new RelayCommand(OnSaveTag);
            this.UpdateStaticDataCommand = new RelayCommand<object>(UpdateStaticData);
            this.DeleteStaticDataCommand = new RelayCommand<object>(DeleteStaticData);
            this.AddStaticDataCommand = new RelayCommand<object>(AddStaticData);

            initDatabase();
            ReleaseDate = DateTime.Now; 
            this.LoadedFile = new File("No File Selected");
            this.Title = "";
            this.Tag = new Tag(this.LoadedFile);
            this.exportedTag = "No Tag Created For File";
        }

        private void initDatabase()
        {
            var connection = new SQLiteAsyncConnection(App.databasePath);
            connection.CreateTableAsync<Actor>();
            connection.CreateTableAsync<Studio>();
            connection.CreateTableAsync<Category>();

            this.studioRepository = new RepositoryBase<Studio>(connection);
            this.actorRepository = new RepositoryBase<Actor>(connection);
            this.categoryRepository = new RepositoryBase<Category>(connection);

            var actors = connection.Table<Actor>().OrderBy(actors => actors.Name).ToListAsync().Result;
            var categories = connection.Table<Category>().OrderBy(categories => categories.Name).ToListAsync().Result;
            var studios = connection.Table<Studio>().OrderBy(studios => studios.Name).ToListAsync().Result;

            this.Actors = new ObservableCollection<Actor>(actors);
            this.Categories = new ObservableCollection<Category>(categories);
            this.Studios = new ObservableCollection<Studio>(studios);
        }

        private void UpdateStaticData(object dataToUpdate)
        {
            if (dataToUpdate == null)
            {
                return; 
            }
            var type = dataToUpdate.GetType(); 
            if (type == null)
            {
                throw new ArgumentNullException();
            }else if (type == typeof(Studio))
            {
                var studio = dataToUpdate as Studio; 
                this.studioRepository.Update(studio);
                //var studioToUpdate = this.studios.Where(x => x.StudioId == studio.StudioId).FirstOrDefault();
            }else if (type == typeof(Actor))
            {
                var actor = dataToUpdate as Actor;
                this.actorRepository.Update(actor); 
            }else if (type == typeof(Category))
            {
                var category = dataToUpdate as Category;
                this.categoryRepository.Update(category);
            }

        }

        private void DeleteStaticData(object dataToDelete)
        {
            if (dataToDelete == null)
            {
                return;
            }
            switch (dataToDelete.GetType().Name)
            {
                case nameof(Studio):
                    var studioToDelete = dataToDelete as Studio;
                    this.studioRepository.Delete(studioToDelete);
                    this.Studios.Remove(studioToDelete);
                    break;
                case nameof(Actor):
                    var actorToDelete = dataToDelete as Actor;
                    this.actorRepository.Delete(actorToDelete);
                    this.Actors.Remove(actorToDelete);
                    break;
                case nameof(Category):
                    var categoryToDelete = dataToDelete as Category;
                    this.categoryRepository.Delete(categoryToDelete);
                    this.Categories.Remove(categoryToDelete);
                    break;
            }
        }

        private void AddStaticData(object dataToAdd)
        {
            switch (dataToAdd.GetType().Name)
            {
                case nameof(Studio):
                    var studioWithInfo = dataToAdd as Studio;
                    string? studioName = studioWithInfo.Name; 
                    if (string.IsNullOrWhiteSpace(studioName))
                    {
                        return; 
                    }
                    var studioToAdd = new Studio(studioName);
                    this.studioRepository.Create(studioToAdd);
                    this.Studios.Add(studioToAdd);
                    break;
                case nameof(Actor):
                    var actorWithInfo = dataToAdd as Actor;
                    string? actorName = actorWithInfo.Name;
                    if (string.IsNullOrWhiteSpace(actorName))
                    {
                        return;
                    }
                    var actorToAdd = new Actor(actorName);
                    this.actorRepository.Create(actorToAdd);
                    this.Actors.Add(actorToAdd);
                    break;
                case nameof(Category):
                    var categoryWithInfo = dataToAdd as Category;
                    string? categoryName = categoryWithInfo.Name;
                    if (string.IsNullOrWhiteSpace(categoryName))
                    {
                        return;
                    }
                    var categoryToAdd = new Category(categoryName);
                    this.categoryRepository.Create(categoryToAdd);
                    this.Categories.Add(categoryToAdd);
                    break;
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

        private void OnAddFile()
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
