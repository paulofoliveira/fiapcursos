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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((CursosViewModel)BindingContext).Load();
        }

        //protected void Curso_Tapped(object sender, ItemTappedEventArgs e)
        //{
        //    Curso curso = e.Item as Curso;
        //    Navigation.PushAsync(new CursoDetalheView(curso));
        //}
    }
}
