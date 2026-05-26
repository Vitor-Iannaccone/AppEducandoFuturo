using AppEducandoFuturo.Data.Repositories;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Services
{
    public class ModuloService
    {
        private readonly ModuloRepository _moduloRepository;
        private readonly ProgressoRepository _progressoRepository;

        public ModuloService(ModuloRepository moduloRepository, ProgressoRepository progressoRepository)
        {
            _moduloRepository = moduloRepository;
            _progressoRepository = progressoRepository;
        }

        public async Task<List<Modulo>> BuscarTodosAsync() =>
            await _moduloRepository.BuscarTodosAsync();

        public async Task<Modulo> BuscarPorIdAsync(int id) =>
            await _moduloRepository.BuscarPorIdAsync(id);

        public async Task<(bool Sucesso, string Mensagem)> SalvarAsync(Modulo modulo)
        {
            if (string.IsNullOrWhiteSpace(modulo.Titulo))
                return (false, "O título é obrigatório.");

            await _moduloRepository.SalvarAsync(modulo);
            return (true, "Módulo salvo com sucesso!");
        }

        public async Task DeletarAsync(Modulo modulo) =>
            await _moduloRepository.DeletarAsync(modulo);

        // Busca o progresso do aluno em cada módulo
        public async Task<int> BuscarProgressoAlunoAsync(int alunoId, int moduloId)
        {
            var progresso = await _progressoRepository.BuscarPorAlunoEModuloAsync(alunoId, moduloId);
            return progresso?.Pontuacao ?? 0;
        }
    }
}