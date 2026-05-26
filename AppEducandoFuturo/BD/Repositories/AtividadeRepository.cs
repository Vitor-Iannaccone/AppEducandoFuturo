using SQLite;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Data.Repositories
{
    public class AtividadeRepository
    {
        private async Task<SQLiteAsyncConnection> GetDb() =>
            await AppDatabase.GetDatabaseAsync();

        public async Task<List<Atividade>> BuscarTodosAsync()
        {
            var db = await GetDb();
            return await db.Table<Atividade>().ToListAsync();
        }

        public async Task<Atividade> BuscarPorIdAsync(int id)
        {
            var db = await GetDb();
            return await db.Table<Atividade>().Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Atividade>> BuscarPorModuloAsync(int moduloId)
        {
            var db = await GetDb();
            return await db.Table<Atividade>().Where(a => a.ModuloId == moduloId).ToListAsync();
        }

        public async Task<int> SalvarAsync(Atividade atividade)
        {
            var db = await GetDb();
            return atividade.Id == 0
                ? await db.InsertAsync(atividade)
                : await db.UpdateAsync(atividade);
        }

        public async Task<int> DeletarAsync(Atividade atividade)
        {
            var db = await GetDb();
            return await db.DeleteAsync(atividade);
        }
    }
}