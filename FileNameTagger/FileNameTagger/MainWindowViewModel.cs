using FileNameTagger.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNameTagger
{
    public class MainWindowViewModel
    {
        public object TagViewModel { get; set; }

        public MainWindowViewModel()
        {
            TagViewModel = new TagViewModel();
        }
    }
}
