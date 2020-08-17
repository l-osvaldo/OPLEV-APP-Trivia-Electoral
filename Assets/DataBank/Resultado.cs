using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class Resultado
    {
        public string id;
        public string id_user_app;
        public string aciertos;
        public string errores;
        public string detalle;
        public string registrado;

        public Resultado(string id, string id_user_app, string aciertos, string errores, string detalle, string registrado)
        {
            this.id = id;
            this.id_user_app = id_user_app;
            this.aciertos = aciertos;
            this.errores = errores;
            this.detalle = detalle;
            this.registrado = registrado;
        }

    }
}
