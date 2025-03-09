using AppSaude.MVVM.Models;


namespace AppSaude.Services
{
    public interface IAlarmService
    {
        Task InitializeAsync();
        Task CheckAlarms();
    }
}
