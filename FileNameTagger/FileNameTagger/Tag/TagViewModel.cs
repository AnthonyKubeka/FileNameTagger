using Domain;
using Shared;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FileNameTagger.Tag
{
    public class TagViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { }; //property change is never null since we assign it an empty anonymous subscriber
        private ObservableCollection<Studio> studios;
        private ObservableCollection<Actor> actors;
        private ObservableCollection<ResolutionsEnum> resolutions;
        private ObservableCollection<Category> categories;
        private Studio selectedStudio;
        private Actor selectedActor;
        private Category selectedCategory;
        private ResolutionsEnum selectedResolution;

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

        public ObservableCollection<ResolutionsEnum> Resolutions
        {
            get
            {
                return resolutions;
            }

            set
            {
                if (resolutions != value)
                {
                    resolutions = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Resolutions"));
                }
            }
        }

        public TagViewModel()
        {

        }
    }
}
