using Domain;
using Shared;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FileNameTagger.Tag
{
    public class TagViewModel : BindableBase
    {
        private ObservableCollection<Studio> studios;
        private ObservableCollection<Actor> actors;
        private ObservableCollection<ResolutionsEnum> resolutions;
        private ObservableCollection<Category> categories;
        public RelayCommand DeleteCommand { get; private set; }
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

        public TagViewModel()
        {
            this.LoadActors();
            this.LoadStudios();
            this.LoadCategories();
            this.LoadResolutions();

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
