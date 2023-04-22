using System;
using System.Collections.Generic;
using System.Text;
using ExamenTercerParcialCarlosEspinal.Models;
using ExamenTercerParcialCarlosEspinal.Services;
using ExamenTercerParcialCarlosEspinal.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace ExamenTercerParcialCarlosEspinal.ViewModels
{
    public  class ListViewModels : BaseViewModels
    {
        private List<Nota> _listaNotas;
        NotaServices notaServices;

        public List<Nota> ListaNotas
        {
            get { return _listaNotas; }
            set
            {
                _listaNotas = value;
                _listaNotas.OrderBy(o => o.Key).ToList();
                OnPropertyChanged();
            }
        }

        public ListViewModels()
        {
            notaServices = new NotaServices();

            EditarNotaCommand = new Command<Nota>(async (Nota) => await EditarNota(Nota));
            EliminarNotaCommand = new Command<Nota>(async (Nota) => await EliminarNota(Nota));
        }

        private async Task EliminarNota(Nota nota)
        {
            bool response = await notaServices.DeleteNota(nota.Key);

            if (response)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "Eliminado Correctamente", "Ok");
                CargarDatos();
            }
            else
                await Application.Current.MainPage.DisplayAlert("Aviso", "Se produjo un error al eliminar", "Ok");

        }

        private async Task EditarNota(Nota nota)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddPage("Editar", nota));
        }

        public async void CargarDatos()
        {
            ListaNotas = await notaServices.ListarNotas();
            if (ListaNotas.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "No hay notas registradas", "Ok");
            }
        }

        public ICommand EditarNotaCommand { get; set; }
        public ICommand EliminarNotaCommand { get; set; }

    }
}
