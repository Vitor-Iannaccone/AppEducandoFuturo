using SQLite;

namespace AppEducandoFuturo.Models
{
    public class Progresso
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int Pontuacao { get; set; } = 0;

        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        // FK para o Aluno
        public int AlunoId { get; set; }

        // FK para o Módulo
        public int ModuloId { get; set; }
    }
}