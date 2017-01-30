using Fiap.Cursos.Mobile.Models;
using Fiap.Cursos.Mobile.ViewModels;

using Xamarin.Forms;

namespace Fiap.Cursos.Mobile.Views
{
    public partial class CursosView : ContentPage
    {
        public CursosView()
        {
            InitializeComponent();
            BindingContext = new CursosViewModel(Navigation);

            CursosListView.ItemTapped += (sender, e) =>
            {
                if (e.Item == null) return;
                ((ListView)sender).SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((CursosViewModel)BindingContext).Load();
        }
    }
}
