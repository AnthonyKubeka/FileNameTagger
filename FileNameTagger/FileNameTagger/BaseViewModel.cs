

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileNameTagger
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string property = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false; 
            }

            storage = value;
            this.OnPropertyChanged(property);
            return true; 
        }

        /*Notes:
         * ref - Pass by reference (i.e. modify the address space instead of making a 'copy') i.e. changing b by ref and then using b again will actually have b changed without an assignment
         * virtual - so we can redefine / extend the method in classes that inherit from the base
         * SetProperty will call PropertyChanged when value of property changes 
         * */
    }
}
