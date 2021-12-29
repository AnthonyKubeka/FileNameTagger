using Domain;
using Shared;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace FileNameTagger
{
    public class MainWindowViewModel : BindableBase
    {

        private ObservableCollection<File> files;
        private File selectedFile; 
        private ObservableCollection<Studio> studios;
        private ObservableCollection<Actor> actors;
        private ObservableCollection<ResolutionsEnum> resolutions;
        private ObservableCollection<Category> categories;
        private ObservableCollection<Tag> tags;

        public string SelectedFileName
        {
            get
            {
                if (SelectedFile != null)
                {
                    return SelectedFile.Name;
                }
                else
                {
                    return "No File Selected";
                }
            }
        }
        public File SelectedFile
        {
            get
            {
                return selectedFile;
            }

            set
            {
                SetProperty(ref selectedFile, value);
            }
        }

        public ObservableCollection<Tag> Tags
        {
            get
            {
                return tags;
            }

            set
            {
                SetProperty(ref tags, value);
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
                SetProperty(ref studios, value);
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
                SetProperty(ref actors, value);
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
                SetProperty(ref categories, value);
            }
        }

        public ObservableCollection<ResolutionsEnum> Resolutions
        {
            get
            {
                return resolutions;
            }

            set
            {
                SetProperty(ref resolutions, value);
            }
        }
        public ObservableCollection<File> Files
        {
            //fire the event when the properties change
            get
            {
                return files;
            }

            set
            {
                SetProperty(ref files, value);
            }
        }
        private BindableBase currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand<string>(OnNav);
            AddFileToFilesViewCommand = new RelayCommand(OnAddFilesToFilesView);
            DeleteFileCommand = new RelayCommand<File>(OnDeleteFile);
            this.LoadActors();
            this.LoadStudios();
            this.LoadCategories();
            this.LoadResolutions();
            this.files = new ObservableCollection<File>();
            this.tags = new ObservableCollection<Tag>();

        }

        public RelayCommand<File> DeleteFileCommand { get; private set; } //private set as we only want this to be settable once, on construction

        public RelayCommand<string> NavCommand { get; private set; }
        public RelayCommand AddFileToFilesViewCommand { get; private set; }

    private void OnNav(string destination)
        {
            switch (destination)
            {
                case "files":
                    //CurrentViewModel = filesViewModel;
                    break;
                case "tags":
                default: 
                   // CurrentViewModel = tagViewModel;
                    break;
            }
        }

        private void OnDeleteFile(File fileToDelete)
        {
            Files.Remove(fileToDelete);
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
                return filename;
            }
            else
            {
                return "File not found";
            }

        }

        void OnAddFilesToFilesView()
        {
            var fileToAdd = (new File (this.SelectFileFromFileExplorer()));
            files.Add(fileToAdd);
            tags.Add(new Tag(fileToAdd));
        }

        public void LoadResolutions()
        {
            Resolutions = new ObservableCollection<ResolutionsEnum>();

            this.Resolutions.Add(ResolutionsEnum.SD);
            this.Resolutions.Add(ResolutionsEnum.HD);
            this.Resolutions.Add(ResolutionsEnum.FHD);
            this.Resolutions.Add(ResolutionsEnum.QHD);
            this.Resolutions.Add(ResolutionsEnum.UHD);
        }

        public void LoadCategories()
        {
            if (DesignerProperties.GetIsInDesignMode(
               new System.Windows.DependencyObject())) return;

            Categories = new ObservableCollection<Category>();

            this.Categories.Add(new Category("action"));
            this.Categories.Add(new Category("adventure"));
            this.Categories.Add(new Category("superhero"));
        }

        public void LoadActors()
        {
            if (DesignerProperties.GetIsInDesignMode(
               new System.Windows.DependencyObject())) return;

            Actors = new ObservableCollection<Actor>();

            this.Actors.Add(new Actor("Jennifer Lawrence", GendersEnum.Female));
            this.Actors.Add(new Actor("Robert Downey Jr.", GendersEnum.Male));
        }

        public void LoadStudios()
        {
            if (DesignerProperties.GetIsInDesignMode(
               new System.Windows.DependencyObject())) return;

            Studios = new ObservableCollection<Studio>();

            this.Studios.Add(new Studio("Warner Bros."));
            this.Studios.Add(new Studio("Marvel Studios"));
        }
    }
}
