using SQLite;

namespace AppEducandoFuturo.Models
{
    public class Conteudo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        // "Texto", "Video" ou "Atividade"
        public string Formato { get; set; }

        public string Corpo { get; set; }

        public bool Concluido { get; set; } = false;

        // FK para o Módulo ao qual pertence
        public int ModuloId { get; set; }
    }
}