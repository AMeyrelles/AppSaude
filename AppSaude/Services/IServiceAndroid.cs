using AppSaude.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSaude.Services
{
    public interface IServiceAndroid : IAlarmService
    {
        bool IsRunning { get; set; }
        void Start();
        void Stop();      
    }        
}
