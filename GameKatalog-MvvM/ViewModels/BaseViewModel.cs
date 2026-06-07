using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameKatalog_MvvM.ViewModels
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Informiert die View darüber,wasgeändert worden ist.
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Hilfsmethode zum Setzen von Eigenschaften prüft zuerst ob sich der Wert wirklich geändert hat
        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
        {
            // Wenn alter und neuer Wert gleich sind keine änderungen 
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            // Neuen Wert speichern und oberfläche akktulasieren 
            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
