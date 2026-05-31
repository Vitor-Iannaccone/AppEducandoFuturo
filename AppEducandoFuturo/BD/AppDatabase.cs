using SQLite;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Data
{
    public class AppDatabase
    {
        private static SQLiteAsyncConnection _database;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private static readonly string DbPath = Path.Combine(
    FileSystem.AppDataDirectory, "educandofuturo_v2.db3"
);

        public static async Task<SQLiteAsyncConnection> GetDatabaseAsync()
        {
            if (_database != null)
                return _database;

            await _semaphore.WaitAsync();
            try
            {
                if (_database == null)
                {
                    _database = new SQLiteAsyncConnection(DbPath);
                    await _database.CreateTableAsync<Usuario>();
                    await _database.CreateTableAsync<Modulo>();
                    await _database.CreateTableAsync<Conteudo>();
                    await _database.CreateTableAsync<Atividade>();
                    await _database.CreateTableAsync<Progresso>();
                    await _database.CreateTableAsync<Notificacao>();
                }
            }
            finally
            {
                _semaphore.Release();
            }

            return _database;
        }
    }
}


