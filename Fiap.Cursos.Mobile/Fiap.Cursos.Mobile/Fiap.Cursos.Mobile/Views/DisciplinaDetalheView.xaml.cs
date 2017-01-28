using Fiap.Cursos.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Fiap.Cursos.Mobile.Views
{
    public partial class DisciplinaDetalheView : ContentPage
    {
        public DisciplinaDetalheView(string id)
        {
            InitializeComponent();
            BindingContext = new DisciplinaDetalheViewModel(id, Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((DisciplinaDetalheViewModel)BindingContext).Load();
        }
    }
}
