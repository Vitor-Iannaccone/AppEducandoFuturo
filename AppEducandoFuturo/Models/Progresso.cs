using SQLite;

namespace AppEducandoFuturo.Models
{
    public class Progresso
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int Pontuacao { get; set; } = 0;

        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        // IDs das atividades respondidas corretamente separados por vírgula
        public string AtividadesRespondidas { get; set; } = "";

        public int AlunoId { get; set; }

        public int ModuloId { get; set; }
    }
}