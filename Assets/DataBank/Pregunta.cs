using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class Pregunta
    {
        public string id;
        public string pregunta;
        public string opcion_a;
        public string opcion_b;
        public string opcion_c;
        public string opcion_d;
        public string respuesta;
        public string rubro;
        public string subrubro;
        public string etiquetas;
        public string version;
        public string numero_respuestas;

        public Pregunta(string id, string pregunta, string opc_a, string opc_b, string opc_c, string opc_d, string respuesta, 
            string rubro, string subrubro, string etiquetas, string version, string numero_respuestas)
        {
            this.id = id;
            this.pregunta = pregunta;
            this.opcion_a = opc_a;
            this.opcion_b = opc_b;
            this.opcion_c = opc_c;
            this.opcion_d = opc_d;
            this.respuesta = respuesta;
            this.rubro = rubro;
            this.subrubro = subrubro;
            this.etiquetas = etiquetas;
            this.version = version;
            this.numero_respuestas = numero_respuestas;
        }

    }
}
