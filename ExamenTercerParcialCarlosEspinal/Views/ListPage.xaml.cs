using ExamenTercerParcialCarlosEspinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExamenTercerParcialCarlosEspinal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        ListViewModels listaViewModels;
        public ListPage()
        {
            InitializeComponent();
            listaViewModels = new ListViewModels();
            BindingContext = listaViewModels;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listaViewModels.CargarDatos();
        }
    }
}