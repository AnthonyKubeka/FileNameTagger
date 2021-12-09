using Domain;
using FileNameTagger.Files;
using FileNameTagger.Tag;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNameTagger
{
    public class MainWindowViewModel : BindableBase
    {
        private TagViewModel tagViewModel = new TagViewModel();
        private FilesViewModel filesViewModel = new FilesViewModel();
        private BindableBase currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        public FilesViewModel FilesViewModel
        {
            get { return filesViewModel; }
            set { SetProperty(ref filesViewModel, value); }
        }

        public TagViewModel TagsViewModel
        {
            get { return tagViewModel; }
            set { SetProperty(ref tagViewModel, value); }
        }

        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand<string>(OnNav);
            AddFileToFilesViewCommand = new RelayCommand(OnAddFilesToFilesView);
        }

        public RelayCommand<string> NavCommand { get; private set; }
        public RelayCommand AddFileToFilesViewCommand { get; private set; }

    private void OnNav(string destination)
        {
            switch (destination)
            {
                case "files":
                    CurrentViewModel = filesViewModel;
                    break;
                case "tags":
                default: 
                    CurrentViewModel = tagViewModel;
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
                return filename;
            }
            else
            {
                return "File not found";
            }

        }

        void OnAddFilesToFilesView()
        {
            var fileToAdd = this.SelectFileFromFileExplorer();
        }
    }
}
