using Fiap.Cursos.Mobile.Models;
using Fiap.Cursos.Mobile.Services;
using Fiap.Cursos.Mobile.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fiap.Cursos.Mobile.ViewModels
{
    public class CursoDetalheViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private Curso _curso;

        public CursoDetalheViewModel(Curso curso, INavigation navigation) : this(navigation)
        {
            Curso = curso;

            LoginCommand = new DelegateCommand(RealizarLoginNoGoogle, (o) => true);
            DetalhesDisciplinaCommand = new DelegateCommand(ExibirModalDetalheDisciplina, (o) => true);

            MessagingCenter.Subscribe<CursoDisciplina>(this, "CursoDisciplinaAtualizado", (cd) => { CarregarDisciplinas(); });

            AcessarDetalheDisciplina = App.Client.CurrentUser != null;
        }

        private async void ExibirModalDetalheDisciplina(object args)
        {
            await Navigation.PushModalAsync(new DisciplinaDetalheView(args.ToString()));
        }

        private async void RealizarLoginNoGoogle(object args)
        {
            IAuthService loginService = DependencyService.Get<IAuthService>();

            try
            {
                var user = App.Client.CurrentUser;

                if (user == null)
                {
                    user = await loginService.LoginAsync();
                    App.Client.CurrentUser = user;
                }

                AcessarDetalheDisciplina = true;

            }
            catch (InvalidOperationException)
            {

            }
        }

        public CursoDetalheViewModel(INavigation navigation) : base(navigation)
        {

        }

        private ObservableCollection<DisciplinaGrupo> _disciplinas;

        public ObservableCollection<DisciplinaGrupo> Disciplinas
        {
            get { return _disciplinas; }
            set
            {
                _disciplinas = value;
                NotifyPropertyChanged("Disciplinas");
            }
        }

        private bool _acessarDetalheDisciplina;

        public bool AcessarDetalheDisciplina
        {
            get { return _acessarDetalheDisciplina; }
            set
            {
                _acessarDetalheDisciplina = value;
                NotifyPropertyChanged("AcessarDetalheDisciplina");
            }
        }

        public Curso Curso
        {
            get
            {
                return _curso;
            }
            set
            {
                _curso = value;
                NotifyPropertyChanged("Curso");
            }

        }

        public override void Load()
        {
            if (Disciplinas != null) return;
            if (Curso == null) return;

            CarregarDisciplinas();
        }

        private void CarregarDisciplinas()
        {
            IsLoading = true;

            new DisciplinaService().GetAllDisciplinasByCursoIdAsync(Curso.Id).ContinueWith(r =>
            {
                if (r.Exception != null) return;

                Disciplinas = new ObservableCollection<DisciplinaGrupo>(r.Result);
                IsLoading = false;
            });
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand DetalhesDisciplinaCommand { get; private set; }
    }
}
