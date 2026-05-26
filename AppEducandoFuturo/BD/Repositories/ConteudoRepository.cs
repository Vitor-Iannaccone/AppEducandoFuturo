using SQLite;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Data.Repositories
{
    public class ConteudoRepository
    {
        private async Task<SQLiteAsyncConnection> GetDb() =>
            await AppDatabase.GetDatabaseAsync();

        public async Task<List<Conteudo>> BuscarTodosAsync()
        {
            var db = await GetDb();
            return await db.Table<Conteudo>().ToListAsync();
        }

        public async Task<Conteudo> BuscarPorIdAsync(int id)
        {
            var db = await GetDb();
            return await db.Table<Conteudo>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Conteudo>> BuscarPorModuloAsync(int moduloId)
        {
            var db = await GetDb();
            return await db.Table<Conteudo>().Where(c => c.ModuloId == moduloId).ToListAsync();
        }

        public async Task<int> SalvarAsync(Conteudo conteudo)
        {
            var db = await GetDb();
            return conteudo.Id == 0
                ? await db.InsertAsync(conteudo)
                : await db.UpdateAsync(conteudo);
        }

        public async Task<int> DeletarAsync(Conteudo conteudo)
        {
            var db = await GetDb();
            return await db.DeleteAsync(conteudo);
        }
    }
}