using SQLite;
using AppEducandoFuturo.Models;
using AppEducandoFuturo.Data;

namespace AppEducandoFuturo.Data.Repositories
{
    public class UsuarioRepository
    {
        private async Task<SQLiteAsyncConnection> GetDb() =>
            await AppDatabase.GetDatabaseAsync();

        public async Task<List<Usuario>> BuscarTodosAsync()
        {
            var db = await GetDb();
            return await db.Table<Usuario>().ToListAsync();
        }

        public async Task<Usuario> BuscarPorEmailAsync(string email)
        {
            var db = await GetDb();
            return await db.Table<Usuario>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Usuario> BuscarPorIdAsync(int id)
        {
            var db = await GetDb();
            return await db.Table<Usuario>().Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SalvarAsync(Usuario usuario)
        {
            var db = await GetDb();
            return usuario.Id == 0
                ? await db.InsertAsync(usuario)
                : await db.UpdateAsync(usuario);
        }

        public async Task<int> DeletarAsync(Usuario usuario)
        {
            var db = await GetDb();
            return await db.DeleteAsync(usuario);
        }
    }
}