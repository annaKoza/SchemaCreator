using System.ComponentModel;

namespace SchemaCreator.UI.ViewModel
{
    internal class BaseViewModel : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string nazwaWłasności)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności));
        }
        #endregion
    }
}