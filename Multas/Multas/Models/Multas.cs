using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Multas
    {

        //id, data, FK viatura, FK agente, FK condutor, valor, infração

        public int Id { get; set; }

        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public string Infracao { get; set; }

    }
}