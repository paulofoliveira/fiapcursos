using Fiap.Cursos.Mobile.Models;
using Fiap.Cursos.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fiap.Cursos.Mobile.ViewModels
{
    public class DisciplinaDetalheViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _id;
        private string _idCurso;
        private string _idDisciplina;
        private bool _ativo;
        private byte _modulo;

        private CursoDisciplina _cursoDisciplina;
        private ObservableCollection<Disciplina> _disciplinas;

        public DisciplinaDetalheViewModel(INavigation navigation) : base(navigation)
        {

        }

        public DisciplinaDetalheViewModel(string id, INavigation navigation) : this(navigation)
        {
            Id = id;

            CancelarCommand = new DelegateCommand(async (o) => await Navigation.PopModalAsync(), (o) => true);
            SalvarCommand = new DelegateCommand(Salvar, (o) => true);
        }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        public string Titulo
        {
            get
            {
                if (Disciplinas == null || string.IsNullOrEmpty(IdDisciplina)) return "";
                Disciplina item = Disciplinas.FirstOrDefault(p => p.Id == IdDisciplina);
                if (item == null) throw new Exception("Disciplina não pode ser nula.");
                return "Editar " + item.Nome;
            }
        }

        public string IdDisciplina
        {
            get
            {
                return _idDisciplina;
            }
            set
            {
                _idDisciplina = value;
                NotifyPropertyChanged("IdDisciplina");
            }
        }

        public string IdCurso
        {
            get
            {
                return _idCurso;
            }
            set
            {
                _idCurso = value;
                NotifyPropertyChanged("IdCurso");
            }
        }

        public bool Ativo
        {
            get
            {
                return _ativo;
            }
            set
            {
                _ativo = value;
                NotifyPropertyChanged("Ativo");
            }
        }


        public byte Modulo
        {
            get
            {
                return _modulo;
            }
            set
            {
                _modulo = value;
                NotifyPropertyChanged("Modulo");
            }
        }


        public ObservableCollection<Disciplina> Disciplinas
        {
            get { return _disciplinas; }
            set
            {
                _disciplinas = value;
                NotifyPropertyChanged("Disciplinas");
            }
        }

        public async override void Load()
        {
            if (string.IsNullOrEmpty(Id)) return;

            IsLoading = true;

            Task<CursoDisciplina> cursoDisciplinaTask = new CursoDisciplinaService().GetByIDAsync(Id);
            Task<IEnumerable<Disciplina>> disciplinasTask = new DisciplinaService().GetAll();

            await Task.WhenAll(new Task[] { cursoDisciplinaTask, disciplinasTask }).ContinueWith(r =>
             {
                 if (r.Exception != null) return;

                 CursoDisciplina item = cursoDisciplinaTask.Result;

                 IdCurso = item.IdCurso;
                 IdDisciplina = item.IdDisciplina;
                 Ativo = item.Ativo;
                 Modulo = item.Modulo;

                 Disciplinas = new ObservableCollection<Disciplina>(disciplinasTask.Result);

                 NotifyPropertyChanged("Titulo");
                 IsLoading = false;
             });

        }

        public ICommand SalvarCommand { get; private set; }
        public ICommand CancelarCommand { get; private set; }

        private async void Salvar(object args)
        {
            CursoDisciplinaService service = new CursoDisciplinaService();

            try
            {
                CursoDisciplina item = new CursoDisciplina
                {
                    Id = Id,
                    IdCurso = IdCurso,
                    IdDisciplina = IdDisciplina,
                    Ativo = Ativo,
                    Modulo = Modulo
                };

                await service.UpdateAsync(item);

                NotifyPropertyChanged("Titulo");

                MessagingCenter.Send(item, "CursoDisciplinaAtualizado");

                await Navigation.PopModalAsync();
            }
            catch (Exception)
            {

            }
        }
    }
}
