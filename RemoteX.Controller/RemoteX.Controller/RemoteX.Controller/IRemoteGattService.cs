using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Controller
{
    interface IRemoteGattService
    {
        void PublishService();

        Task CreateService();

        void CreateCharacteristics();
    }
}
