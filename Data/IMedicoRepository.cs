using System.Collections.Generic;
using System.Threading.Tasks;
using DesafioBackEnd.Models;

namespace DesafioBackEnd.Data
{
    public interface IMedicoRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T : class;
         Task<bool> SalvarTodos();
         Task<IEnumerable<Medico>> BuscarTodosMedicos();
         Task<IEnumerable<Medico>> BuscarListaMedicosPorId(int Id);
         Task<Medico> BuscarMedicoPorId(int Id);
         Task<IEnumerable<Especialidade>> BuscarMedicoPorEspecialista(string  specialty);
         bool ValidaCpf(string cpf);
    }
}