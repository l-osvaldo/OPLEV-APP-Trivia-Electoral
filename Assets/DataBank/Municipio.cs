using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class Municipio
    {
        public string numdto;
        public string nombredto;
        public string nummpio;
        public string nombrempio;

        public Municipio(string numdto, string nombredto, string nummpio, string nombrempio)
        {
            this.numdto = numdto;
            this.nombredto = nombredto;
            this.nummpio = nummpio;
            this.nombrempio = nombrempio;
        }

    }
}
