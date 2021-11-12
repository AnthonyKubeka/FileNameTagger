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
        }

        public RelayCommand<string> NavCommand { get; private set; }

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

        public void OpenFileExplorer()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
            }

        }
    }
}
