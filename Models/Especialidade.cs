using System.Collections.Generic;

namespace DesafioBackEnd.Models
{
    public class Especialidade
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public Medico Medico { get; set; }
        public int MedicoId { get; set; }
    }
}