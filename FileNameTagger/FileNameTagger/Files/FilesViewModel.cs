using Domain;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FileNameTagger.Files
{
    public class FilesViewModel : INotifyPropertyChanged
    {
        private File selectedFile;
        private ObservableCollection<File> files;
        public event PropertyChangedEventHandler PropertyChanged = delegate { }; //property change is never null since we assign it an empty anonymous subscriber

        public File SelectedFile
        {
            get
            {
                return selectedFile;
            }
            set
            {
                if (selectedFile != value)
                {
                    selectedFile = value;
                    DeleteCommand.RaiseCanExecuteChanged(); //this is so the button isn't always disabled (since it is at the beginning when no value is seelected yet)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedFile"));
                }
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
                if (files != value)
                {
                    files = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Files")); //the delegate is callbacking what we are passing. We pass the sender (this viewmodel) and the thing to callback
                }
            }
        }

        public RelayCommand DeleteCommand { get; private set; } //private set as we only want this to be settable once, on construction

        public FilesViewModel()
        {
            this.DeleteCommand = new RelayCommand(OnDelete, CanDelete);
        }
        private void OnDelete()
        {
            if (this.CanDelete())
            {
                Files.Remove(SelectedFile);
            }
        }

        private bool CanDelete()
        {
            return SelectedFile != null;
        }
    }
}
