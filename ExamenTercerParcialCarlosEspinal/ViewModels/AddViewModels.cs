using Plugin.Media;
using Plugin.Media.Abstractions;
using ExamenTercerParcialCarlosEspinal.Models;
using ExamenTercerParcialCarlosEspinal.Services;
using ExamenTercerParcialCarlosEspinal.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static Xamarin.Essentials.AppleSignInAuthenticator;

namespace ExamenTercerParcialCarlosEspinal.ViewModels
{
    public class AddViewModels : BaseViewModels
    {
        #region VARIABLES

        private string _Descripcion;
        private DateTime _Fecha;        
        private string _Photo_Record;
        Image imagenNota;
        NotaServices services;
        private string opcion;
        private string key;
        private bool _IsImageDefault;
        private bool _IsImageEdit;
        #endregion

        #region OBJETOS
        public string Descripcion
        {
            get { return _Descripcion; }
            set
            {
                _Descripcion = value;
                OnPropertyChanged();
            }
        }

        public DateTime Fecha
        {
            get { return _Fecha; }
            set
            {
                _Fecha = value;
                OnPropertyChanged();
            }
        }



        public string Photo_Record
        {
            get { return _Photo_Record; }
            set
            {
                _Photo_Record = value;
                OnPropertyChanged();
            }
        }

        public bool IsImageDefault
        {
            get { return _IsImageDefault; }
            set
            {
                _IsImageDefault = value;
                OnPropertyChanged();
            }
        }

        public bool IsImageEdit
        {
            get { return _IsImageEdit; }
            set
            {
                _IsImageEdit = value;
                OnPropertyChanged();
            }
        }

        #endregion
        public AddViewModels(Image imageParam, string opcionReceived, Nota notaReceived)
        {
            imagenNota = imageParam;
            services = new NotaServices();
            opcion = opcionReceived;

            if (opcion.Equals("Editar"))
            {
                CargarParaEditar(notaReceived);
                IsImageDefault = false;
                IsImageEdit = true;
            }
            else
            {
                IsImageDefault = true;
                IsImageEdit = false;
            }

            TomarFotoCommand = new Command(async () => await TomarFoto());
            GuardarCommand = new Command(() => GuardarNota());
            ListarCommand = new Command(() => ListarPersonas());
        }

        private void CargarParaEditar(Nota notaReceived)
        {
            Descripcion = notaReceived.Descripcion;
            Fecha = notaReceived.Fecha;
            Photo_Record = notaReceived.Photo_Record;            
            key = notaReceived.Key;
        }

        private async void GuardarNota()
        {
            string response = ValidarCampos();
            if (!response.Equals("OK"))
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", response, "Ok");
                return;
            }

            Nota nota = new Nota()
            {
                Descripcion = Descripcion,
                Fecha = Fecha,
                Photo_Record = Photo_Record
                
                
            };

            if (opcion.Equals("Editar"))
            {
                // EDITAR
                bool confirm = await services.UpdateNota(nota, key);
                if (confirm)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Actualizado correctamente.", "Ok");
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Se produjo un error al actualizar.", "Ok");
                }
            }
            else
            {
                // GUARDAR
                bool confirm = await services.InsertarNota(nota);
                if (confirm)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Registrado correctamente.", "Ok");
                    Limpiar();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Se produjo un error al registrar.", "Ok");
                }
            }

        }


        private void Limpiar()
        {
            Descripcion = "";
            //Fecha =) ;
            
            imagenNota.Source = "profile.png";
        }


        private string ValidarCampos()
        {
            if (string.IsNullOrEmpty(Descripcion))
            {
                return "Debe ingresar una descripcion.";
            }
            
            else if (string.IsNullOrEmpty(Photo_Record))
            {
                return "Debe ingresar la fotografia";
            }

            return "OK";
        }

        public static bool ValidateOnlyString(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-ZáéíóúÁÉÍÓÚ ]+$");
        }

        public static bool ValidateOnlyNumber(string text)
        {
            return Regex.IsMatch(text, @"^[0-9]*$");
        }

        private async void ListarPersonas()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ListPage());
        }

        private Task TomarFoto()
        {
            GetImageFromCamera();
            return Task.CompletedTask;
        }

        private async void GetImageFromCamera()
        {
            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                });

                if (file == null)
                    return;

                imagenNota.Source = ImageSource.FromStream(() => { return file.GetStream(); });
                byte[] byteArray = File.ReadAllBytes(file.Path);
                Photo_Record = System.Convert.ToBase64String(byteArray);
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "Se produjo un error al tomar la fotografia.", "Ok");
            }
        }

        public ICommand TomarFotoCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand ListarCommand { get; set; }

    }
}
