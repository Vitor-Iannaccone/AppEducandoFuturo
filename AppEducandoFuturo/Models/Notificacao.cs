using SQLite;

namespace AppEducandoFuturo.Models
{
    public class Notificacao
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Titulo { get; set; }

        public string Mensagem { get; set; }

        public DateTime DataEnvio { get; set; } = DateTime.Now;

        public bool Lida { get; set; } = false;

        // FK para o usuário destinatário
        public int UsuarioId { get; set; }
    }
}
