using SQLite;

namespace AppEducandoFuturo.Models
{
    public class Atividade
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Pergunta { get; set; }

        // Opções separadas por "|" ex: "Sim|Não|Talvez"
        public string Opcoes { get; set; }

        [NotNull]
        public string RespostaCorreta { get; set; }

        public int Pontuacao { get; set; } = 10;

        // FK para o Módulo ao qual pertence
        public int ModuloId { get; set; }
    }
}