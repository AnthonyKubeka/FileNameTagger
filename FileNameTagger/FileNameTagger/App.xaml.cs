using Domain;
using Repository;
using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FileNameTagger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { //need to move to connection strings config of some kind
        private static string databaseName = "FileNameTagger.db";
        private static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);
        public static SQLiteAsyncConnection connection = new SQLiteAsyncConnection(App.databasePath);
        public static IRepositoryBase<Tag> TagRepository = new RepositoryBase<Tag>(App.connection);
        public static IRepositoryBase<TagType> TagTypesRepository = new RepositoryBase<TagType>(App.connection);
    }
}
