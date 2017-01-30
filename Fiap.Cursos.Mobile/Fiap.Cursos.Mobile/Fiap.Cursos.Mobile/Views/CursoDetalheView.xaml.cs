using Fiap.Cursos.Mobile.Models;
using Fiap.Cursos.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fiap.Cursos.Mobile.Views
{
    public partial class CursoDetalheView : ContentPage
    {
        private Curso _curso;

        public CursoDetalheView(Curso curso)
        {
            _curso = curso;
            InitializeComponent();
            BindingContext = new CursoDetalheViewModel(_curso, Navigation);

            DetalhesCursoListView.ItemTapped += (sender, e) =>
            {
                if (e.Item == null) return;
                ((ListView)sender).SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((CursoDetalheViewModel)BindingContext).Load();
        }
    }
}
