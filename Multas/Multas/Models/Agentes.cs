using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes{

        //id, nome, esquadra, foto

        public int ID { get; set; }

        [Required(ErrorMessage = "Por favor, escreva o Nome do Agente.")]
        [RegularExpression("([A-ZÁÉÍÓÚa-záéíóúàèìòùäëïöüãõâêîôûçñ]+( |-|')?)+",
                          ErrorMessage = "Só pode escrever letras no nome. Deve começar por uma maiúscula.")]

        public string Nome { get; set; }

        [Required(ErrorMessage = "Não se esqueça de indicar a Esquadra onde o agente trabalha, por favor.")]
        //   [RegularExpression("(Tomar|Ourém|Torres Novas|Lisboa|Leiria)")]

        public string Esquadra { get; set; }

        public string Fotografia { get; set; }
        
        //identifica as multas passadas pelo Agente
        public ICollection<Multas> ListaDasMultas { get; set; }
    }
}