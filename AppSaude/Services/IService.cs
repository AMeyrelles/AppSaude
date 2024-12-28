using AppSaude.MVVM.Models;

namespace AppSaude.Services
{
    public interface IService
    {

        Task InitializeAsync();
        Task<List<Alarme>> GetAlarmes();
        Task<Alarme> GetAlarme(int Id);
        Task<int> AddAlarme(Alarme alarme);
        Task<int> DeleteAlarme(Alarme alarme);
        Task<int> UpdateAlarme(Alarme alarme);


        Task<List<Agendamento>> GetAgendamentos();
        Task<Agendamento> GetAgendamento(int Id);
        Task<int> AddAgendamento(Agendamento agendamento);
        Task<int> DeleteAgendamento(Agendamento agendamento);
        Task<int> UpdateAgendamento(Agendamento agendamento);

        Task SetUpDb();
    }
}
