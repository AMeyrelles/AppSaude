using AppSaude.MVVM.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSaude.Services
{
    public class AlarmeService : IAlarmeService
    {
        private SQLiteAsyncConnection _dbConnection;
        public async Task InitializeAsync()
        {
            await SetUpDb();
        }

        private async Task SetUpDb()
        {
            if (_dbConnection == null)
            {
                string dbPath = Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.LocalApplicationData),
                    "DBAppSaude.db3");

                _dbConnection = new SQLiteAsyncConnection(dbPath);

                // Criar tabela para o modelo Alarme
                await _dbConnection.CreateTableAsync<Alarme>();
            }
        }

        public async Task<int> AddAlarme(Alarme alarme) => await _dbConnection.InsertAsync(alarme);

        public async Task<int> Deletelarme(Alarme alarme) => await _dbConnection.DeleteAsync(alarme);

        public async Task<int> UpdateAlarme(Alarme alarme) => await _dbConnection.UpdateAsync(alarme);

        public async Task<List<Alarme>> GetAlarmes() => await _dbConnection.Table<Alarme>().ToListAsync();

        public async Task<Alarme> GetAlarme(int id) => await _dbConnection.Table<Alarme>().FirstOrDefaultAsync(x => x.Id == id);

        public Task<int> DeleteAlarme(Alarme alarme)
        {
            throw new NotImplementedException();
        }

        Task IAlarmeService.SetUpDb()
        {
            throw new NotImplementedException();
        }
    }

}