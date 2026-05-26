using AppEducandoFuturo.Data.Repositories;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Services
{
    public class NotificacaoService
    {
        private readonly NotificacaoRepository _notificacaoRepository;

        public NotificacaoService(NotificacaoRepository notificacaoRepository)
        {
            _notificacaoRepository = notificacaoRepository;
        }

        public async Task<List<Notificacao>> BuscarPorUsuarioAsync(int usuarioId) =>
            await _notificacaoRepository.BuscarPorUsuarioAsync(usuarioId);

        public async Task<int> ContarNaoLidasAsync(int usuarioId)
        {
            var naoLidas = await _notificacaoRepository.BuscarNaoLidasAsync(usuarioId);
            return naoLidas.Count;
        }

        public async Task<(bool Sucesso, string Mensagem)> EnviarAsync(
            string titulo, string mensagem, int usuarioId)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return (false, "O título é obrigatório.");

            var notificacao = new Notificacao
            {
                Titulo = titulo,
                Mensagem = mensagem,
                UsuarioId = usuarioId,
                DataEnvio = DateTime.Now,
                Lida = false
            };

            await _notificacaoRepository.SalvarAsync(notificacao);
            return (true, "Notificação enviada com sucesso!");
        }

        public async Task MarcarComoLidaAsync(Notificacao notificacao)
        {
            notificacao.Lida = true;
            await _notificacaoRepository.SalvarAsync(notificacao);
        }

        public async Task DeletarAsync(Notificacao notificacao) =>
            await _notificacaoRepository.DeletarAsync(notificacao);
    }
}