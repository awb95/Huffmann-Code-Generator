using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Huffmann_Code_Generator.ViewModel
{
    /// <summary>
    /// Basisklasse um PropertyChanged Funktionalität in allen ViewModel Klassen zu implementieren
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetProperty<T>(ref T storage, T value,[CallerMemberName] string property = null)
        {
            if (Object.Equals(storage, value)) return;
            storage = value;
            if (PropertyChanged != null)       
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }   
}
