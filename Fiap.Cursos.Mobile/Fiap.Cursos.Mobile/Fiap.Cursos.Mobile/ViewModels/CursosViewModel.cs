using Fiap.Cursos.Mobile.Models;
using Fiap.Cursos.Mobile.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using System;
using System.Windows.Input;
using Fiap.Cursos.Mobile.Views;

namespace Fiap.Cursos.Mobile.ViewModels
{
    public class CursosViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public CursosViewModel(INavigation navigation) : base(navigation)
        {
            PesquisarCursosCommand = new DelegateCommand(CarregarCursos, (o) => { return true; });

            DetalhesCursoCommand = new DelegateCommand(async (o) =>
            {
                await Navigation.PushAsync(new CursoDetalheView(o as Curso));
            }, (o) => true);
        }

        private ObservableCollection<Curso> cursos;
        private string _filtro;

        public ObservableCollection<Curso> Cursos
        {
            get { return cursos; }
            set
            {
                cursos = value;
                NotifyPropertyChanged("Cursos");
            }
        }

        public ICommand PesquisarCursosCommand { get; private set; }   
        public ICommand DetalhesCursoCommand { get; set; }

        public string Filtro
        {
            get { return _filtro; }
            set
            {
                if (_filtro != value)
                {
                    _filtro = value;
                    NotifyPropertyChanged("Filtro");

                    if (PesquisarCursosCommand.CanExecute(null))
                        PesquisarCursosCommand.Execute(null);
                }
            }
        }

        private void CarregarCursos(object parameter)
        {
            //if (Cursos != null) return;

            CursoService cursoService = new CursoService();
            var cursosAsync = string.IsNullOrEmpty(Filtro) ? cursoService.GetAll() : Filtro.Length > 2 ? cursoService.GetByFiltro(Filtro) : null;
            if (cursosAsync == null) return;

            IsLoading = true;

            cursosAsync.ContinueWith((r =>
            {
                if (r.Exception == null)
                    Cursos = new ObservableCollection<Curso>(r.Result);
                else
                {
                    // Alerta para exception!
                }

                IsLoading = false;
            }));
        }

        public override void Load()
        {
            CarregarCursos(null);
        }
    }
}
