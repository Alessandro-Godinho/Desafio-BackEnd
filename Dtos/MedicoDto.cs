using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DesafioBackEnd.Helpers;

namespace DesafioBackEnd.Dtos
{
    public class MedicoDto 
    {
         public int Id { get; set; }
        [Required( ErrorMessage="Nome é obrigatório",AllowEmptyStrings=false)]
        [StringLength(255,MinimumLength=4, ErrorMessage="É necessário informar um nome entre 4 e 255 caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage="CPF é obrigatório",AllowEmptyStrings=false)]
        [RegularExpression(@"^\d{3}.?\d{3}.?\d{3}-?\d{2}$", ErrorMessage = "Informe um cpf válido")]
        public string Cpf { get; set; }
        [Required(ErrorMessage="CRM é obrigatório",AllowEmptyStrings=false)]
        public string Crm { get; set; }
        [ValidarEspecialidade(ErrorMessage="Especialidade é obrigatório e não pode conter vazio")]
        public List<string> Especialidades { get; set; }

    
    }
}