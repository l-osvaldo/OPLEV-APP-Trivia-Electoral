using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class AppUser
    {

        public string _id;
        public string _nombre;
        public string _email;
        public string _edad;
        public string _sexo;
        public string _municipio;
        public string _password;
        public string _registrado;

        public AppUser(string id, string nombre, string email, string edad, string sexo, string municipio, string password, string registrado)
        {
            _id = id;
            _nombre = nombre;
            _email = email;
            _edad = edad;
            _sexo = sexo;
            _municipio = municipio;
            _password = password;
            _registrado = registrado;
        }

    }
}
