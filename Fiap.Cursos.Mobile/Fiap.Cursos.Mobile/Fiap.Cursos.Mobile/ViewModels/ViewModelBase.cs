using System.ComponentModel;
using Xamarin.Forms;

namespace Fiap.Cursos.Mobile.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private INavigation nav;

        public ViewModelBase(INavigation navigation)
        {
            nav = navigation;
        }

        private bool loading;

        public bool IsLoading
        {
            get { return loading; }
            set
            {
                loading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get { return nav; } }

        public abstract void Load();
    }
}
