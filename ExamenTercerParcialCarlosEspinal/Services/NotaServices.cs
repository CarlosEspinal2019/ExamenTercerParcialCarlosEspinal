using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamenTercerParcialCarlosEspinal.Firebase;
using ExamenTercerParcialCarlosEspinal.Models;
using Firebase.Database.Query;

namespace ExamenTercerParcialCarlosEspinal.Services
{
    public class NotaServices
    {
        public async Task<bool> InsertarNota(Nota Nota)
        {
            bool response = false;
            try
            {
                await Conexion.firebase
                .Child("Nota")
                .PostAsync(new Nota()
                {
                    Descripcion = Nota.Descripcion,
                    Fecha = DateTime.Now,
                    Photo_Record = Nota.Photo_Record,
                    Audio_Record = Nota.Audio_Record               
                  
                });
                response = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }


        public async Task<List<Nota>> ListarNotas()
        {
            try
            {
                var data = (await Conexion.firebase
                            .Child("Nota")
                            .OnceAsync<Nota>()).Select(item => new Nota
                            {
                                Key = item.Key,
                                Descripcion = item.Object.Descripcion,
                                Fecha = item.Object.Fecha,
                                Photo_Record = item.Object.Photo_Record,                              

                            }).ToList();

                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }


        public async Task<bool> DeleteNota(string key)
        {
            bool response = false;
            try
            {
                await Conexion.firebase.Child("Nota").Child(key).DeleteAsync();
                response = true;
            }
            catch (Exception ex)
            {
                response = false;
                Debug.WriteLine(ex.Message);
            }
            return response;
        }

        public async Task<bool> UpdateNota(Nota Nota, string key)
        {
            bool response = false;
            try
            {
                await Conexion.firebase
                              .Child("Nota")
                              .Child(key)
                              .PutAsync(Nota);
                response = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }


    }
}
