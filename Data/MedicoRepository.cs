using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Data {
    public class MedicoRepository : IMedicoRepository {
        private readonly DataContext _context;
        public MedicoRepository (DataContext context) {
            _context = context;
        }
        public void Add<T> (T entity) where T : class {
            _context.Add (entity);
        }
        public void Delete<T> (T entity) where T : class {
            _context.Remove (entity);
        }

        public async Task<IEnumerable<Medico>> BuscarTodosMedicos () {
            var medico = _context.Medicos.Include (p => p.Especialidades).ToListAsync ();
            return await medico;
        }

        public async Task<IEnumerable<Medico>> BuscarListaMedicosPorId (int Id) {
            var medico = _context.Medicos.Include (p => p.Especialidades).Where (u => u.Id == Id).ToListAsync ();
            return await medico;
        }

        public async Task<IEnumerable<Especialidade>> BuscarMedicoPorEspecialista (string specialty) {
            var medico = _context.Especialidades.Where (u => u.Descricao == specialty).ToListAsync ();
            return await medico;
        }
        public async Task<Medico> BuscarMedicoPorId (int Id) {
            var medico = _context.Medicos.Include (x => x.Especialidades).FirstOrDefaultAsync (u => u.Id == Id);
            return await medico;
        }

        public async Task<bool> SalvarTodos () {
            return await _context.SaveChangesAsync () > 0;
        }

        public bool ValidaCpf (string cpf) {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim ();
            cpf = cpf.Replace (".", "").Replace ("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring (0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse (tempCpf[i].ToString ()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString ();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse (tempCpf[i].ToString ()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString ();
            return cpf.EndsWith (digito);
        }
    }
}