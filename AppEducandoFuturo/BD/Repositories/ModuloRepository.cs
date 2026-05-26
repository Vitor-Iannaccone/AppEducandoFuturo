using SQLite;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Data.Repositories
{
    public class ModuloRepository
    {
        private async Task<SQLiteAsyncConnection> GetDb() =>
            await AppDatabase.GetDatabaseAsync();

        public async Task<List<Modulo>> BuscarTodosAsync()
        {
            var db = await GetDb();
            return await db.Table<Modulo>().ToListAsync();
        }

        public async Task<Modulo> BuscarPorIdAsync(int id)
        {
            var db = await GetDb();
            return await db.Table<Modulo>().Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Modulo>> BuscarPorEducadorAsync(int educadorId)
        {
            var db = await GetDb();
            return await db.Table<Modulo>().Where(m => m.EducadorId == educadorId).ToListAsync();
        }

        public async Task<int> SalvarAsync(Modulo modulo)
        {
            var db = await GetDb();
            return modulo.Id == 0
                ? await db.InsertAsync(modulo)
                : await db.UpdateAsync(modulo);
        }

        public async Task<int> DeletarAsync(Modulo modulo)
        {
            var db = await GetDb();
            return await db.DeleteAsync(modulo);
        }
    }
}