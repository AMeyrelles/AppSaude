using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSaude
{
    public interface IServiceAndroid
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}
