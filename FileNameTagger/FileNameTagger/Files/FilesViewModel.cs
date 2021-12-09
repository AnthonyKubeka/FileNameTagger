using Domain;
using Shared;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace FileNameTagger.Files
{
    public class FilesViewModel : BindableBase
    {
        private ObservableCollection<File> files;

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

        public RelayCommand<File> DeleteFileCommand { get; private set; } //private set as we only want this to be settable once, on construction

        public FilesViewModel()
        {
            DeleteFileCommand = new RelayCommand<File>(OnDeleteFile);
            Messenger.Default.Register<NamedMessage>(this, UpdateContent);
        }

        private string fileNameToLoad;
        public string FileNameToLoad
        {
            get => fileNameToLoad;
            set
            {
                SetProperty(ref fileNameToLoad, value);
            }
        }
        private void UpdateContent(NamedMessage message)
        {
            FileNameToLoad = $"{message.Name} Pressed";
        }

        private void OnDeleteFile(File fileToDelete)
        {
            Files.Remove(fileToDelete);
        }


    }
}
