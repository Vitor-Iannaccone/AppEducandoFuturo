using SQLite;

namespace AppEducandoFuturo.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Nome { get; set; }

        [NotNull, Unique]
        public string Email { get; set; }

        [NotNull]
        public string SenhaCriptografada { get; set; }

        // "Aluno", "Educador" ou "Administrador"
        [NotNull]
        public string TipoUsuario { get; set; }

        // ── Campos do Aluno ──────────────────────────────
        public int PontuacaoTotal { get; set; } = 0;

        public int ModulosConcluidos { get; set; } = 0;

        public bool TemConteudoOffline { get; set; } = false;

        // ── Campos do Educador ───────────────────────────
        public string Especialidade { get; set; }

        public int ModulosCriados { get; set; } = 0;

        // ── Campos do Administrador ──────────────────────
        public string NivelAcesso { get; set; } = "Admin";

        public DateTime UltimoAcesso { get; set; } = DateTime.Now;
    }
}