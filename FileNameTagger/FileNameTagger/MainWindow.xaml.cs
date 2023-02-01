using Domain;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace FileNameTagger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {

            var data = e.Data.GetData("FileName");
            if (data == null)
                return; 

            string[] files = (string[])data;
            var filename = files[0];

            var viewModel = (MainWindowViewModel)DataContext;
            if (viewModel.DropFileCommand.CanExecute(null))
                viewModel.DropFileCommand.Execute(filename);
        }
    }
}
