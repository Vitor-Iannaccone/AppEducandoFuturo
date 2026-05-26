using SQLite;

namespace AppEducandoFuturo.Models
{
    public class Modulo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string Tema { get; set; }

        // FK para o Educador que criou
        public int EducadorId { get; set; }
    }
}