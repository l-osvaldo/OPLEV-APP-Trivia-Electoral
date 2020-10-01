using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class AppUser
    {

        public string id;
        public string nombre;
        public string email;
        public string edad;
        public string sexo;
        public string municipio;
        public string estado;
        public string password;
        public string score;
        public string registrado;
        public string status;


        public AppUser(string id, string nombre, string email, string edad, string sexo, string municipio, string estado, string password, string score, string registrado, string status)
        {
            this.id = id;
            this.nombre = nombre;
            this.email = email;
            this.edad = edad;
            this.sexo = sexo;
            this.municipio = municipio;
            this.estado = estado;
            this.password = password;
            this.score = score;
            this.registrado = registrado;
            this.status = status;
        }

    }
}
