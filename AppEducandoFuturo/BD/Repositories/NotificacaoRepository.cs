using SQLite;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Data.Repositories
{
    public class NotificacaoRepository
    {
        private async Task<SQLiteAsyncConnection> GetDb() =>
            await AppDatabase.GetDatabaseAsync();

        public async Task<List<Notificacao>> BuscarTodosAsync()
        {
            var db = await GetDb();
            return await db.Table<Notificacao>().ToListAsync();
        }

        public async Task<List<Notificacao>> BuscarPorUsuarioAsync(int usuarioId)
        {
            var db = await GetDb();
            return await db.Table<Notificacao>().Where(n => n.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task<List<Notificacao>> BuscarNaoLidasAsync(int usuarioId)
        {
            var db = await GetDb();
            return await db.Table<Notificacao>()
                .Where(n => n.UsuarioId == usuarioId && !n.Lida)
                .ToListAsync();
        }

        public async Task<int> SalvarAsync(Notificacao notificacao)
        {
            var db = await GetDb();
            return notificacao.Id == 0
                ? await db.InsertAsync(notificacao)
                : await db.UpdateAsync(notificacao);
        }

        public async Task<int> DeletarAsync(Notificacao notificacao)
        {
            var db = await GetDb();
            return await db.DeleteAsync(notificacao);
        }
    }
}