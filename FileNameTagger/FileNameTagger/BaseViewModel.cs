

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileNameTagger
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /*Notes:
         * ref - Pass by reference (i.e. modify the address space instead of making a 'copy') i.e. changing b by ref and then using b again will actually have b changed without an assignment
         * virtual - so we can redefine / extend the method in classes that inherit from the base
         * SetProperty will call PropertyChanged when value of property changes 
         * */
    }
}
