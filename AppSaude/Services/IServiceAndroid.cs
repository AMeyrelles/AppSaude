using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppSaude.Services
{
    public interface IServiceAndroid
    {
        
        bool IsRunning { get; }
        void Start();
        void Stop();
        Task CheckAlarmsAsync();
    }
}
