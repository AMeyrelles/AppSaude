using AppSaude.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
