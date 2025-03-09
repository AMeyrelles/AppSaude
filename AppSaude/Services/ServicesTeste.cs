using AppSaude.MVVM.Models;
using SQLite;

namespace AppSaude.Services
{
    public class ServicesTeste : IServicesTeste
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

                //Inicializando a conexão com o banco de dados
                _dbConnection = new SQLiteAsyncConnection(dbPath);

                await _dbConnection.CreateTableAsync<Alarme>();
                await _dbConnection.CreateTableAsync<Agendamento>();
                await _dbConnection.CreateTableAsync<NotificacaoAlarme>();
                await _dbConnection.CreateTableAsync<NotificacaoAgendamento>();
            }

        }

        private readonly SemaphoreSlim _semaphore = new(1, 1);
        //Alarmes

        public async Task<int> AddAlarme(Alarme alarme)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _dbConnection.InsertAsync(alarme);
            }
            finally
            {
                _semaphore.Release();
            }
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


        //Agendamentos
        public async Task<int> AddAgendamento(Agendamento agendamento)
        {
            return await _dbConnection.InsertAsync(agendamento);
        }
        public async Task<int> DeleteAgendamento(Agendamento agendamento)
        {
            return await _dbConnection.DeleteAsync(agendamento);
        }
        public async Task<int> UpdateAgendamento(Agendamento agendamento)
        {
            return await _dbConnection.UpdateAsync(agendamento);
        }
        public async Task<List<Agendamento>> GetAgendamentos()
        {
            return await _dbConnection.Table<Agendamento>().ToListAsync();
        }
        public async Task<Agendamento> GetAgendamento(int id)
        {
            var agendamento = await _dbConnection.Table<Agendamento>().FirstOrDefaultAsync(x => x.AppointmentsId == id);
            if (agendamento == null)
            {
                Console.WriteLine($"Nenhum agendamento encontrado para o ID: {id}");
            }
            return agendamento;
        }

        //Notificações

        //NotificacoesAlarme
        public async Task<int> AddNotAlarme(NotificacaoAlarme notAlarme)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _dbConnection.InsertAsync(notAlarme);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        public async Task<int> DeleteNotAlarme(NotificacaoAlarme notAlarme)
        {
            return await _dbConnection.DeleteAsync(notAlarme);
        }
        public async Task<int> UpdateNotAlarme(NotificacaoAlarme notAlarme)
        {
            return await _dbConnection.UpdateAsync(notAlarme);
        }
        public async Task<List<NotificacaoAlarme>> GetNotAlarmes()
        {
            return await _dbConnection.Table<NotificacaoAlarme>().ToListAsync();
        }
        public async Task<NotificacaoAlarme> GetNotAlarme(int id)
        {
            return await _dbConnection.Table<NotificacaoAlarme>().FirstOrDefaultAsync(x => x.IdNA == id);
        }


        //NotificacoesAgendamento
        public async Task<int> AddNotAgendamento(NotificacaoAgendamento notAgendamento)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _dbConnection.InsertAsync(notAgendamento);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        public async Task<int> DeleteNotAgendamento(NotificacaoAgendamento notAgendamento)
        {
            return await _dbConnection.DeleteAsync(notAgendamento);
        }
        public async Task<int> UpdateNotAgendamento(NotificacaoAgendamento notAgendamento)
        {
            return await _dbConnection.UpdateAsync(notAgendamento);
        }
        public async Task<List<NotificacaoAgendamento>> GetNotAgendamentos()
        {
            return await _dbConnection.Table<NotificacaoAgendamento>().ToListAsync();
        }
        public async Task<NotificacaoAgendamento> GetNotAgendamento(int id)
        {
            return await _dbConnection.Table<NotificacaoAgendamento>().FirstOrDefaultAsync(x => x.IdNAg == id);
        }
        //FIM
    }
}