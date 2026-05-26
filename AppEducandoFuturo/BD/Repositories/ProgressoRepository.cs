using SQLite;
using AppEducandoFuturo.Models;

namespace AppEducandoFuturo.Data.Repositories
{
    public class ProgressoRepository
    {
        private async Task<SQLiteAsyncConnection> GetDb() =>
            await AppDatabase.GetDatabaseAsync();

        public async Task<List<Progresso>> BuscarPorAlunoAsync(int alunoId)
        {
            var db = await GetDb();
            return await db.Table<Progresso>().Where(p => p.AlunoId == alunoId).ToListAsync();
        }

        public async Task<Progresso> BuscarPorAlunoEModuloAsync(int alunoId, int moduloId)
        {
            var db = await GetDb();
            return await db.Table<Progresso>()
                .Where(p => p.AlunoId == alunoId && p.ModuloId == moduloId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SalvarAsync(Progresso progresso)
        {
            var db = await GetDb();
            return progresso.Id == 0
                ? await db.InsertAsync(progresso)
                : await db.UpdateAsync(progresso);
        }

        public async Task<int> DeletarAsync(Progresso progresso)
        {
            var db = await GetDb();
            return await db.DeleteAsync(progresso);
        }
    }
}
