using AppEducandoFuturo.Data.Repositories;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Services
{
    public class AtividadeService
    {
        private readonly AtividadeRepository _atividadeRepository;
        private readonly ProgressoRepository _progressoRepository;
        private readonly UsuarioRepository _usuarioRepository;

        public AtividadeService(AtividadeRepository atividadeRepository,
            ProgressoRepository progressoRepository,
            UsuarioRepository usuarioRepository)
        {
            _atividadeRepository = atividadeRepository;
            _progressoRepository = progressoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<Atividade>> BuscarPorModuloAsync(int moduloId) =>
            await _atividadeRepository.BuscarPorModuloAsync(moduloId);

        public async Task<(bool Sucesso, string Mensagem)> SalvarAsync(Atividade atividade)
        {
            if (string.IsNullOrWhiteSpace(atividade.Pergunta))
                return (false, "A pergunta é obrigatória.");

            if (string.IsNullOrWhiteSpace(atividade.RespostaCorreta))
                return (false, "A resposta correta é obrigatória.");

            await _atividadeRepository.SalvarAsync(atividade);
            return (true, "Atividade salva com sucesso!");
        }

        public async Task DeletarAsync(Atividade atividade) =>
            await _atividadeRepository.DeletarAsync(atividade);

        // Verifica resposta e atualiza pontuação do aluno
        public async Task<(bool Correto, string Mensagem, int Pontos)> ResponderAsync(
     int alunoId, Atividade atividade, string respostaAluno)
        {
            bool correto = respostaAluno.Trim().ToLower() ==
                           atividade.RespostaCorreta.Trim().ToLower();

            if (!correto)
                return (false, "Resposta incorreta. Tente novamente!", 0);

            // Verifica se já respondeu essa atividade
            var progresso = await _progressoRepository
                .BuscarPorAlunoEModuloAsync(alunoId, atividade.ModuloId);

            var respondidas = progresso?.AtividadesRespondidas?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToList() ?? new List<int>();

            // Só pontua se ainda não respondeu essa atividade
            bool jaRespondida = respondidas.Contains(atividade.Id);

            if (progresso == null)
            {
                progresso = new Progresso
                {
                    AlunoId = alunoId,
                    ModuloId = atividade.ModuloId,
                    Pontuacao = jaRespondida ? 0 : atividade.Pontuacao,
                    AtividadesRespondidas = atividade.Id.ToString()
                };
            }
            else
            {
                if (!jaRespondida)
                {
                    progresso.Pontuacao += atividade.Pontuacao;
                    progresso.AtividadesRespondidas = string.IsNullOrEmpty(progresso.AtividadesRespondidas)
                        ? atividade.Id.ToString()
                        : progresso.AtividadesRespondidas + "," + atividade.Id;
                }
            }

            progresso.DataAtualizacao = DateTime.Now;
            await _progressoRepository.SalvarAsync(progresso);

            // Atualiza pontuação total do usuário só se não havia respondido
            if (!jaRespondida)
            {
                var usuario = await _usuarioRepository.BuscarPorIdAsync(alunoId);
                usuario.PontuacaoTotal += atividade.Pontuacao;
                await _usuarioRepository.SalvarAsync(usuario);
            }

            return (true, $"Correto! +{(jaRespondida ? 0 : atividade.Pontuacao)} pontos!",
                jaRespondida ? 0 : atividade.Pontuacao);
        }
    }
}