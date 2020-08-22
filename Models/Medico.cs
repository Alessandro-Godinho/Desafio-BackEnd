

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DesafioBackEnd.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Crm { get; set; }
        public ICollection<Especialidade> Especialidades { get; set; }
    }
}