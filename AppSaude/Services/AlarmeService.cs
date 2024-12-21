using AppSaude.MVVM.Models;
using SQLite;

namespace AppSaude.Services
{
    public class AlarmeService : IAlarmeService
    {

        private SQLiteAsyncConnection _dbConnection;
        public async Task InitializeAsync()
        {
            await SetUpDb();
        }
        public async Task SetUpDb()
        {
            if (_dbConnection == null)
            {
                string dbPath = Path.Combine(Environment.
                GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DBAppSaude.db3");

                _dbConnection = new SQLiteAsyncConnection(dbPath);
                await _dbConnection.CreateTableAsync<Alarme>();
            }
        }

        public async Task<int> AddAlarme(Alarme alarme)
        {
            return await _dbConnection.InsertAsync(alarme);
        }

        public async Task<int> DeleteAlarme(Alarme alarme)
        {
            return await _dbConnection.DeleteAsync(alarme);
        }
        public async Task<int> UpdateAlarme(Alarme alarme)
        {
            return await _dbConnection.UpdateAsync(alarme);
        }

        public async Task<List<Alarme>> GetAlarmes()
        {
            return await _dbConnection.Table<Alarme>().ToListAsync();
        }

        public async Task<Alarme> GetAlarme(int id)
        {
            return await _dbConnection.Table<Alarme>().FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}