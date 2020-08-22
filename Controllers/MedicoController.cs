using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DesafioBackEnd.Data;
using DesafioBackEnd.Dtos;
using DesafioBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
namespace DesafioBackEnd.Controllers {

    [ApiController]
    [Route ("medico")]
    public class MedicoController : ControllerBase {
        private readonly IMedicoRepository _repo;
        private readonly IMapper _mapper;
        //injeção de dependencia dos repositorios e do autoMapper
        public MedicoController (IMedicoRepository repo, IMapper mapper) {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarMedicos () {
            var medico = await _repo.BuscarTodosMedicos ();
            var Retorno = _mapper.Map<IEnumerable<MedicoDto>> (medico);

            return Ok (Retorno);
        }

        [HttpGet ("{especialidade}")]
        public async Task<IActionResult> BuscarMedicosPorEspecialidade (string especialidade) {

            List<MedicoDto> lista = new List<MedicoDto> ();
            var especialidades = await _repo.BuscarMedicoPorEspecialista (especialidade);
            if (especialidades == null) {
                return BadRequest ("Não existe médico com essa especialidade");
            }
            //buscando cada especialidade encontrada no parametro
            foreach (var esp in especialidades) {
                var medico = await _repo.BuscarMedicoPorId (esp.MedicoId);
                //mapeamento propiedades do Model com Dto utilizando o Auto Mapper
                var medicoRetornado = _mapper.Map<MedicoDto>(medico);
                lista.Add (medicoRetornado);
            }

            return Ok (lista);
        }

        [HttpPost]
        public async Task<IActionResult> SalvarMedico (MedicoDto medicoDto) {
            
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            if (!_repo.ValidaCpf (medicoDto.Cpf)) {
                return BadRequest ("CPF inválido");
            }
            //preparando meus registro da tabela de medico pra ser salva no banco
            var medico = new Medico {
                Nome = medicoDto.Nome,
                Crm = medicoDto.Crm,
                Cpf = medicoDto.Cpf

            };

            for (int i = 0; i < medicoDto.Especialidades.Count; i++) {
                //preparando meus registro da tabela de especialidades pra ser salva no banco
                var especialista = new Especialidade {
                    Descricao = medicoDto.Especialidades[i],
                    Medico = medico
                };
                _repo.Add (especialista);
            }

            _repo.Add (medico);

            if (await _repo.SalvarTodos ()) {
                return Ok (200);
            }

            return BadRequest ("Médico não cadastrado");
        }

        [HttpDelete ("{id})")]
        public async Task<IActionResult> DeletarMedico (int id) {

            var medico = await _repo.BuscarMedicoPorId (id);
            if (medico == null) {
                return BadRequest ("Médico não encontrado");
            }
            _repo.Delete (medico);
            if (await _repo.SalvarTodos ()) {
                return Ok (200);
            }
            return BadRequest ("Médico não cadastrado");
        }
    }
}