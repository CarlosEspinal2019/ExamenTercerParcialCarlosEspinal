using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamenTercerParcialCarlosEspinal.Firebase
{
    public class Conexion
    {
        public static FirebaseClient firebase = new FirebaseClient("https://examentercerparcialcarlosespi-default-rtdb.firebaseio.com/");
    }
}
