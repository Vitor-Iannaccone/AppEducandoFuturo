using System.Security.Cryptography;
using System.Text;
using AppEducandoFuturo.Data.Repositories;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Services
{
    public class AuthService
    {
        private readonly UsuarioRepository _usuarioRepository;

        // Guarda o usuário logado durante a sessão do app
        public Usuario UsuarioLogado { get; private set; }

        // O AuthService recebe o UsuarioRepository por injeção de dependência
        // ou seja, o MauiProgram.cs já passa a instância pronta automaticamente
        public AuthService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // ── CADASTRO ─────────────────────────────────────────────────
        // Cria um novo usuário no banco após validar os dados
        public async Task<(bool Sucesso, string Mensagem)> CadastrarAsync(
            string nome, string email, string senha, string tipoUsuario)
        {
            // 1. Verifica se já existe um usuário com esse email
            var existente = await _usuarioRepository.BuscarPorEmailAsync(email);
            if (existente != null)
                return (false, "E-mail já cadastrado.");

            // 2. Valida campos obrigatórios
            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(senha))
                return (false, "Preencha todos os campos.");

            // 3. Cria o objeto Usuario com a senha criptografada
            var usuario = new Usuario
            {
                Nome = nome,
                Email = email.ToLower().Trim(),
                SenhaCriptografada = CriptografarSenha(senha),
                TipoUsuario = tipoUsuario,

                // Campos específicos com valores padrão
                NivelAcesso = tipoUsuario == "Administrador" ? "Admin" : null,
                UltimoAcesso = DateTime.Now
            };

            // 4. Salva no banco
            await _usuarioRepository.SalvarAsync(usuario);

            return (true, "Cadastro realizado com sucesso!");
        }

        // ── LOGIN ─────────────────────────────────────────────────────
        // Verifica email e senha e inicia a sessão do usuário
        public async Task<(bool Sucesso, string Mensagem)> LoginAsync(
            string email, string senha)
        {
            // 1. Valida campos
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(senha))
                return (false, "Preencha todos os campos.");

            // 2. Busca o usuário pelo email
            var usuario = await _usuarioRepository.BuscarPorEmailAsync(email.ToLower().Trim());
            if (usuario == null)
                return (false, "E-mail não encontrado.");

            // 3. Compara a senha digitada (criptografada) com a do banco
            if (usuario.SenhaCriptografada != CriptografarSenha(senha))
                return (false, "Senha incorreta.");

            // 4. Salva o usuário na sessão
            UsuarioLogado = usuario;

            // 5. Atualiza o último acesso (útil para Administradores)
            usuario.UltimoAcesso = DateTime.Now;
            await _usuarioRepository.SalvarAsync(usuario);

            return (true, $"Bem-vindo, {usuario.Nome}!");
        }

        // Atualiza os dados do usuário logado na sessão
        public async Task AtualizarUsuarioLogadoAsync()
        {
            if (UsuarioLogado == null) return;
            var atualizado = await _usuarioRepository.BuscarPorIdAsync(UsuarioLogado.Id);
            if (atualizado != null)
                UsuarioLogado = atualizado;
        }

        // ── LOGOUT ────────────────────────────────────────────────────
        // Encerra a sessão limpando o usuário logado
        public void Logout()
        {
            UsuarioLogado = null;
        }

        // ── CRIPTOGRAFIA ──────────────────────────────────────────────
        // Converte a senha em um hash MD5 de 32 caracteres
        private string CriptografarSenha(string senha)
        {
            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
