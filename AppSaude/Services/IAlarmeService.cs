using AppSaude.MVVM.Models;

namespace AppSaude.Services
{
    public interface IAlarmeService
    {

        Task InitializeAsync();
        Task<List<Alarme>> GetAlarmes();
        Task<Alarme> GetAlarme(int Id);
        Task<int> AddAlarme(Alarme alarme);
        Task<int> DeleteAlarme(Alarme alarme);
        Task<int> UpdateAlarme(Alarme alarme);
        Task SetUpDb();

    }
}
