using ExamenTercerParcialCarlosEspinal.Models;
using ExamenTercerParcialCarlosEspinal.ViewModels;
using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.AppleSignInAuthenticator;





namespace ExamenTercerParcialCarlosEspinal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPage : ContentPage
    {
        AudioRecorderService gravador;
        AudioPlayer reprodutor;
        public AddPage(string opcion = "Guardar", Nota nota = null)
        {
            InitializeComponent();
            PDate.MinimumDate = new DateTime(Convert.ToInt32(DateTime.Now.ToString("yyyy")), Convert.ToInt32(DateTime.Now.ToString("MM")), Convert.ToInt32(DateTime.Now.ToString("dd")));
            PDate.MaximumDate = new DateTime(Convert.ToInt32(DateTime.Now.ToString("yyyy")) + 1, 1, 31);

            if (opcion.Equals("Guardar"))
            {
                BindingContext = new AddViewModels(imagePersona, opcion, nota);
            }
            else
            {
                BindingContext = new AddViewModels(imagePersona2, opcion, nota);
            }

            gravador = new AudioRecorderService
            {
                StopRecordingAfterTimeout = true,
                TotalAudioTimeout = TimeSpan.FromSeconds(15),
                AudioSilenceTimeout = TimeSpan.FromSeconds(2)
            };

            reprodutor = new AudioPlayer();
            reprodutor.FinishedPlaying += Finaliza_Reproducao;
           
        }

      

        async void Gravar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!gravador.IsRecording)
                {
                    gravador.StopRecordingOnSilence = TimeoutSwitch.IsToggled;

                    GravarButton.IsEnabled = false;
                    ReproduzirButton.IsEnabled = false;

                    //Começa gravação
                    var audioRecordTask = await gravador.StartRecording();

                    GravarButton.Text = "Parar de grabar";
                    GravarButton.IsEnabled = true;

                    await audioRecordTask;

                    GravarButton.Text = "Grabar";
                    ReproduzirButton.IsEnabled = true;
                }
                else
                {
                    GravarButton.IsEnabled = false;

                    //parar a gravação...
                    await gravador.StopRecording();

                    GravarButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                //blow up the app!
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

         public void Reproduzir_Clicked(object sender, EventArgs e)
        {
            try
            {
                var filePath = gravador.GetAudioFilePath();

                if (filePath != null)
                {
                    ReproduzirButton.IsEnabled = false;
                    GravarButton.IsEnabled = false;

                    reprodutor.Play(filePath);
                }
            }
            catch (Exception ex)
            {
                //blow up the app!
                //await DisplayAlert("Erro", ex.Message, "OK");
            }
        }


        void Finaliza_Reproducao(object sender, EventArgs e)
        {
            ReproduzirButton.IsEnabled = true;
            GravarButton.IsEnabled = true;
        }


    }
}