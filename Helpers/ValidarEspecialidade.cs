using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
//criando meu proprio data anotation
namespace DesafioBackEnd.Helpers
{
    public class ValidarEspecialidade : ValidationAttribute
{
   public override bool IsValid(object lista)
   {
       var especialidades = (IEnumerable<string>)lista;
      if(especialidades.Contains(""))
         return false;

      return especialidades.Any();  
   }
}
}